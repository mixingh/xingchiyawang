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
    public class CusExtensionInfoesController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public CusExtensionInfoesController(Crm_DbContext context)
        {
            _context = context;
        }
       
        
        // GET: api/CusExtensionInfoes
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetCusExtensionInfos()
        {
            var data= await _context.CusExtensionInfos.ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/CusExtensionInfoes/Cid
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ApiResult>> cid(int id)
        {
            var data = await _context.CusExtensionInfos
                .Where(a => a.CustomerId==id)
                .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data,data.Count);
        }

        // GET: api/CusExtensionInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetCusExtensionInfo(string id)
        {
            var cusExtensionInfo = await _context.CusExtensionInfos.FindAsync(id);

            if (cusExtensionInfo == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(cusExtensionInfo,1);
        }

        // PUT: api/CusExtensionInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutCusExtensionInfo(string id, CusExtensionInfo cusExtensionInfo)
        {
            if (id != cusExtensionInfo.ExtendId)
            {
                return BadRequest();
            }

            _context.Entry(cusExtensionInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CusExtensionInfoExists(id))
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

        // POST: api/CusExtensionInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostCusExtensionInfo(CusExtensionInfo cusExtensionInfo)
        {
            cusExtensionInfo.ExtendId = System.Guid.NewGuid().ToString("N");

            _context.CusExtensionInfos.Add(cusExtensionInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CusExtensionInfoExists(cusExtensionInfo.ExtendId))
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

        // DELETE: api/CusExtensionInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteCusExtensionInfo(string id)
        {
            var cusExtensionInfo = await _context.CusExtensionInfos.FindAsync(id);
            if (cusExtensionInfo == null)
            {
                return ApiResultHelper.Error();
            }

            _context.CusExtensionInfos.Remove(cusExtensionInfo);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool CusExtensionInfoExists(string id)
        {
            return _context.CusExtensionInfos.Any(e => e.ExtendId == id);
        }
    }
}
