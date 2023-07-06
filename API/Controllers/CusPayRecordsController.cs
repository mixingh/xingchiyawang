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
    public class CusPayRecordsController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public CusPayRecordsController(Crm_DbContext context)
        {
            _context = context;
        }
        // GET: api/CusPayRecords 通过合同id查找 顾客回款记录
        [HttpGet("[action]/{ContractId}")]
        public async Task<ActionResult<ApiResult>> ContractId(string ContractId)
        {
            var data = await _context.CusPayRecords
                .Where(a => a.ContractId == ContractId)
                .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/CusPayRecords
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetCusPayRecords()
        {
            var data= await _context.CusPayRecords.ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error(); 
            }
            return ApiResultHelper.Success(data,data.Count);
        }

        // GET: api/CusPayRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetCusPayRecord(string id)
        {
            var cusPayRecord = await _context.CusPayRecords.FindAsync(id);

            if (cusPayRecord == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(cusPayRecord,1);
        }

        // PUT: api/CusPayRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutCusPayRecord(string id, CusPayRecord cusPayRecord)
        {
            if (id != cusPayRecord.CuspayId)
            {
                return BadRequest();
            }

            _context.Entry(cusPayRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CusPayRecordExists(id))
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

        // POST: api/CusPayRecords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostCusPayRecord(CusPayRecord cusPayRecord)
        {
            cusPayRecord.CuspayId = Guid.NewGuid().ToString("N");
            _context.CusPayRecords.Add(cusPayRecord);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CusPayRecordExists(cusPayRecord.CuspayId))
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

        // DELETE: api/CusPayRecords/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteCusPayRecord(string id)
        {
            var cusPayRecord = await _context.CusPayRecords.FindAsync(id);
            if (cusPayRecord == null)
            {
                return ApiResultHelper.Success();
            }

            _context.CusPayRecords.Remove(cusPayRecord);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool CusPayRecordExists(string id)
        {
            return _context.CusPayRecords.Any(e => e.CuspayId == id);
        }
    }
}
