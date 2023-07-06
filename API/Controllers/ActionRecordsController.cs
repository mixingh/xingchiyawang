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
    public class ActionRecordsController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public ActionRecordsController(Crm_DbContext context)
        {
            _context = context;
        }
        //通过行动编号查找
        [HttpPost("[action]/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<ApiResult>> find(ActionRecord actionRecord , int pageSize, int pageIndex)
        {
            var data = await _context.ActionRecords
                .Where(a => a.RecordId.Contains(actionRecord.RecordId))
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();
            var datanum = await _context.ActionRecords
                .Where(a => a.RecordId.Contains(actionRecord.RecordId))
                .ToListAsync();

            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, datanum.Count);
        }

        [HttpGet("Sid/{salesmanid}/Sid/{cusid}")]
        public async Task<ActionResult<ApiResult>> SidCid(int salesmanid, int cusid)//通过销售员编号查找行动记录
        {
            var ActionRecord = await _context.ActionRecords
                .Where(a => a.SalesManId == salesmanid)
                .Where(b => b.CustomerId == cusid)
                .ToListAsync();
            if (ActionRecord.Count == 0)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(ActionRecord, ActionRecord.Count);

        }
        [HttpGet("[action]/{salesmanid}")]
        public async Task<ActionResult<ApiResult>> Sid(int salesmanid)//通过销售员编号查找行动记录
        {
            var data = await _context.ActionRecords
                .Where(a => a.SalesManId == salesmanid)
                .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data,data.Count);
        }

        [HttpGet("[action]/{cusid}")]
        public async Task<ActionResult<ApiResult>> Cid(int cusid)//通过客户编号查找行动记录
        {
            var data= await _context.ActionRecords
                .Where(a => a.CustomerId == cusid)
                .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data,data.Count);

        }
        // GET: api/ActionRecords
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetActionRecords()
        {
            var data= await _context.ActionRecords.ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/ActionRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetActionRecord(string id)
        {
            var actionRecord = await _context.ActionRecords.FindAsync(id);

            if (actionRecord == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(actionRecord);
        }

        // PUT: api/ActionRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutActionRecord(string id, ActionRecord actionRecord)
        {
            if (id != actionRecord.RecordId)
            {
                return BadRequest();
            }

            _context.Entry(actionRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActionRecordExists(id))
                {
                    return ApiResultHelper.Error();
                }
                else
                {
                    throw;
                }
            }

            return ApiResultHelper.Success() ;
        }

        // POST: api/ActionRecords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostActionRecord(ActionRecord actionRecord)
        {
            /*  actionRecord.RecordId = System.Guid.NewGuid().ToString("N");
              actionRecord.RecordDate = DateTime.Now;*/
            try
            {
                var id = int.Parse(await _context.ActionRecords.Select(a => a.RecordId).MaxAsync());
                actionRecord.RecordId = (id + 1).ToString();
                _context.ActionRecords.Add(actionRecord);
            }
            catch
            {
                actionRecord.RecordId = "20210721";
            }
          
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActionRecordExists(actionRecord.RecordId))
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

        // DELETE: api/ActionRecords/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteActionRecord(string id)
        {
            var actionRecord = await _context.ActionRecords.FindAsync(id);
            if (actionRecord == null)
            {
                return ApiResultHelper.Error();
            }

            _context.ActionRecords.Remove(actionRecord);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool ActionRecordExists(string id)
        {
            return _context.ActionRecords.Any(e => e.RecordId == id);
        }
    }
}
