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
using crm_api.Models.ViewModes;
using System.IO;
using Microsoft.Extensions.Hosting.Internal;

namespace crm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CusContractInfoesController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public CusContractInfoesController(Crm_DbContext context)
        {
            _context = context;
        }
        //合同分页查询
        [HttpPost("[action]/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<ApiResult>> find(CusContractInfo find , int pageSize, int pageIndex)
        {
            var query = from u in _context.CusContractInfos
                        where (u.ContractName.Contains(find.ContractName))
                        where (u.ContractId.Contains(find.ContractId))
                        join a in
                             (from c in _context.SignerInfos
                              where (c.SignerName.Contains(""))
                              select new
                              {
                                  c.SingerId,

                              }) on u.SingerId equals a.SingerId into jointemp
                        from
                                leftjoin in jointemp.DefaultIfEmpty()
                        join b in 
                            ( from b in _context.Salesmen
                              where(b.SalesManName.Contains(""))
                              select new
                              { 
                                b.SalesManId
                              }) on u.SalesManId equals b.SalesManId into sjointemp
                             from sleftjoin in sjointemp.DefaultIfEmpty()
                        select new
                        {
                            ContractId = u.ContractId,
                            ContractName = u.ContractName,
                            SignerId =u.SingerId,
                            SignerName = u.Singer.SignerName,
                            SalesManId =u.SalesManId,
                            SalesManName =u.SalesMan.SalesManName,
                            ContractAmount = u.ContractAmount,
                            ContractEndDate = u.ContractEndDate,
                            ContractInfo = u.ContractInfo,
                            ContractStartDate = u.ContractStartDate,
                            ContractState = u.ContractState
                        };
            var data = await query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
            var datanum = await query.ToListAsync();

            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, datanum.Count);
        }

      
        // GET: api/CusContractInfoes
        [HttpGet("[action]/{SalesManId}")]
        public async Task<ActionResult<ApiResult>> SalesManId(int SalesManId)
        {
            var data = await _context.CusContractInfos
                .Where(a => a.SalesManId == SalesManId)
                .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }
        // GET: api/CusContractInfoes
        [HttpGet("[action]/{SingerId}")]
        public async Task<ActionResult<ApiResult>> SingerId(int SingerId)
        {
            var data = await _context.CusContractInfos
                .Where(a => a.SingerId == SingerId)
                .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }
        // GET: api/CusContractInfoes
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetCusContractInfos()
        {
            var query = from u in _context.CusContractInfos
                  
                        join a in
                             (from c in _context.SignerInfos
                              where (c.SignerName.Contains(""))
                              select new
                              {
                                  c.SingerId,

                              }) on u.SingerId equals a.SingerId into jointemp
                        from
                                leftjoin in jointemp.DefaultIfEmpty()
                        join b in
                            (from b in _context.Salesmen
                             where (b.SalesManName.Contains(""))
                             select new
                             {
                                 b.SalesManId
                             }) on u.SalesManId equals b.SalesManId into sjointemp
                        from sleftjoin in sjointemp.DefaultIfEmpty()
                        select new
                        {
                            ContractId = u.ContractId,
                            ContractName = u.ContractName,
                            SignerId = u.SingerId,
                            SignerName = u.Singer.SignerName,
                            SalesManId = u.SalesManId,
                            SalesManName = u.SalesMan.SalesManName,
                            ContractAmount = u.ContractAmount,
                            ContractEndDate = u.ContractEndDate,
                            ContractInfo = u.ContractInfo,
                            ContractStartDate = u.ContractStartDate,
                            ContractState = u.ContractState
                        };
            var data = await query.ToListAsync();

            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data,data.Count);
        }

        // GET: api/CusContractInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetCusContractInfo(string id)
        {
            var query = from u in _context.CusContractInfos
                        where (u.ContractId==id)
                        join a in
                             (from c in _context.SignerInfos
                              where (c.SignerName.Contains(""))
                              select new
                              {
                                  c.SingerId,

                              }) on u.SingerId equals a.SingerId into jointemp
                        from
                                leftjoin in jointemp.DefaultIfEmpty()
                        join b in
                            (from b in _context.Salesmen
                             where (b.SalesManName.Contains(""))
                             select new
                             {
                                 b.SalesManId
                             }) on u.SalesManId equals b.SalesManId into sjointemp
                        from sleftjoin in sjointemp.DefaultIfEmpty()
                        select new
                        {
                            ContractId = u.ContractId,
                            ContractName = u.ContractName,
                            SingerId = u.SingerId,
                            SignerName = u.Singer.SignerName,
                            SalesManId = u.SalesManId,
                            SalesManName = u.SalesMan.SalesManName,
                            ContractAmount = u.ContractAmount,
                            ContractEndDate = u.ContractEndDate,
                            ContractInfo = u.ContractInfo,
                            ContractStartDate = u.ContractStartDate,
                            ContractState = u.ContractState
                        };
            var data = await query.FirstOrDefaultAsync();


            if (data == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(data,1);
        }

        // PUT: api/CusContractInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutCusContractInfo(string id, CusContractInfo cusContractInfo)
        {
            if (id != cusContractInfo.ContractId)
            {
                return BadRequest();
            }

            _context.Entry(cusContractInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CusContractInfoExists(id))
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

        // POST: api/CusContractInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostCusContractInfo(CusContractInfo cusContractInfo)
        {
            double id;
            try
            {
                id =double.Parse(await _context.CusContractInfos
                    .Select(a => a.ContractId)
                    .MaxAsync());
            }
            catch
            {
                id = 2021194212378;
            }

           
            cusContractInfo.ContractId = (id + 1).ToString();
            _context.CusContractInfos.Add(cusContractInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CusContractInfoExists(cusContractInfo.ContractId))
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

        // DELETE: api/CusContractInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteCusContractInfo(string id)
        {
            var cusContractInfo = await _context.CusContractInfos.FindAsync(id);
            if (cusContractInfo == null)
            {
                return ApiResultHelper.Error();
            }

            _context.CusContractInfos.Remove(cusContractInfo);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool CusContractInfoExists(string id)
        {
            return _context.CusContractInfos.Any(e => e.ContractId == id);
        }
    }
}
