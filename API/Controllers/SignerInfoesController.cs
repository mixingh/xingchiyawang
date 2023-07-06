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
    public class SignerInfoesController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public SignerInfoesController(Crm_DbContext context)
        {
            _context = context;
        }
        // GET: api/SignerInfoes
        [HttpPost("[action]/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<ApiResult>> find(SignerInfo find, int pageSize,int pageIndex)
        {
            var datanum = await _context.SignerInfos
                .Where(a => a.SignerName.Contains(find.SignerName))
            /* .Where(a => a.SingerId.ToString().Contains(find.SingerId.ToString()))*/
                
                .ToListAsync();
            var data = await _context.SignerInfos
                 .Where(a => a.SignerName.Contains(find.SignerName))
            /*   .Where(a => a.SingerId.ToString().Contains(find.SingerId.ToString()))  */           
              .Skip(pageSize * (pageIndex-1))
              .Take(pageSize)
              .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(data, datanum.Count);
        }
        // GET: api/SignerInfoes
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetSignerInfos()
        {
            var data = await _context.SignerInfos.ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
      
            return ApiResultHelper.Success(data,data.Count);
        }

        // GET: api/SignerInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetSignerInfo(int id)
        {
            var signerInfo = await _context.SignerInfos.FindAsync(id);

            if (signerInfo == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(signerInfo,1);
        }

        // PUT: api/SignerInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutSignerInfo(int id, SignerInfo signerInfo)
        {
            if (id != signerInfo.SingerId)
            {
                return BadRequest();
            }

            _context.Entry(signerInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SignerInfoExists(id))
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

        // POST: api/SignerInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostSignerInfo(SignerInfo signerInfo)
        {
            var id = await _context.SignerInfos.Select(a => a.SingerId).MaxAsync();
            signerInfo.SingerId = id + 1;
            _context.SignerInfos.Add(signerInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SignerInfoExists(signerInfo.SingerId))
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

        // DELETE: api/SignerInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteSignerInfo(int id)
        {
            var signerInfo = await _context.SignerInfos.FindAsync(id);
            if (signerInfo == null)
            {
                return ApiResultHelper.Error();
            }

            _context.SignerInfos.Remove(signerInfo);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool SignerInfoExists(int id)
        {
            return _context.SignerInfos.Any(e => e.SingerId == id);
        }
    }
}
