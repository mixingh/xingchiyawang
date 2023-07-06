using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crm_api.Date;
using crm_api.Models;
using crm_api.Models.util;

namespace crm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CusContactInfoesController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public CusContactInfoesController(Crm_DbContext context)
        {
            _context = context;
        }


        // GET: api/CusContactInfoes/{pageSize}/{pageIndex}  分页查询
        [HttpPost("[action]/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<ApiResult>> find(CusContactInfo contactInfo,int pageSize,int pageIndex)
        {
               var data = await _context.CusContactInfos
                    .Where(a => a.ContactName != null && a.ContactName.Contains(contactInfo.ContactName))
                    .Where(a => a.ContactAddress != null && a.ContactAddress.Contains(contactInfo.ContactAddress))
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToListAsync();
            var datanum = await _context.CusContactInfos
                    .Where(a => a.ContactName != null && a.ContactName.Contains(contactInfo.ContactName))
                    .Where(a => a.ContactAddress != null && a.ContactAddress.Contains(contactInfo.ContactAddress))
                    .ToListAsync();
            if (data.Count == 0)
                {
                    return ApiResultHelper.Error();
                }
                return ApiResultHelper.Success(data, datanum.Count);
       
        }

        // GET: api/CusContactInfoes
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetCusContactInfos()
        {
            var data= await _context.CusContactInfos.ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data,data.Count);
        }

        // GET: api/CusContactInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetCusContactInfo(int id)
        {
            var cusContactInfo = await _context.CusContactInfos.FindAsync(id);

            if (cusContactInfo == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(cusContactInfo,1);
        }

        // PUT: api/CusContactInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutCusContactInfo(int id, CusContactInfo cusContactInfo)
        {
            if (id != cusContactInfo.ContactId)
            {
                return BadRequest();
            }

            _context.Entry(cusContactInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CusContactInfoExists(id))
                {
                    return ApiResultHelper.Error();
                }
                else
                {
                    throw;
                }
            }

            return ApiResultHelper.Success();
        }

        // POST: api/CusContactInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostCusContactInfo(CusContactInfo cusContactInfo)
        {
           
            try
            {
                int id= await _context.CusContactInfos.Select(a => a.ContactId).MaxAsync();
                cusContactInfo.ContactId = id + 1;
            }
            catch
            {
                cusContactInfo.ContactId = 20211126;
            
            }
            
        
          
            _context.CusContactInfos.Add(cusContactInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CusContactInfoExists(cusContactInfo.ContactId))
                {
                    return ApiResultHelper.Error();
                }
                else
                {
                    throw;
                }
            }

            return ApiResultHelper.Success();
        }

        // DELETE: api/CusContactInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteCusContactInfo(int id)
        {
            var cusContactInfo = await _context.CusContactInfos.FindAsync(id);
            if (cusContactInfo == null)
            {
                return ApiResultHelper.Error();
            }

            _context.CusContactInfos.Remove(cusContactInfo);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool CusContactInfoExists(int id)
        {
            return _context.CusContactInfos.Any(e => e.ContactId == id);
        }
    }
}
