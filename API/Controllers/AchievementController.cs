using crm_api.Date;
using crm_api.Models.util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly Crm_DbContext _context;
        public AchievementController(Crm_DbContext context) 
        {
            _context = context;
        }
        // GET: api/Achievement
        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<ApiResult>> GetAchievement(string year, string month)
        {
            
            var data = await _context.CusContractInfos
                 .Join(_context.Salesmen, c => c.SalesManId, cm => cm.SalesManId,
                (c, cm) => new
                {
                    contract = c,
                    salesman = cm
                })
                .Where(a => a.contract.ContractStartDate.ToString().Contains(year + "-" + month))
                .GroupBy(a => new
                {
                    a.contract.SalesManId,
                    a.salesman.SalesManName,
                    a.contract.ContractStartDate
                })
                .Select(a => new
                {
                    SalesManId = a.Key.SalesManId,
                    SalesManName = a.Key.SalesManName,
                    Total = a.Sum(i => i.contract.ContractAmount)
                    
                })
                .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data);
        }
        // GET: api/Achievement
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ApiResult>> Salesmanid(int id)
        {

            var data = await _context.CusContractInfos
                 .Join(_context.Salesmen, c => c.SalesManId, cm => cm.SalesManId,
                (c, cm) => new
                {
                    contract = c,
                    salesman = cm
                })
                .Where(a => a.salesman.SalesManId == id)
                .GroupBy(a => new
                {
                    a.salesman.SalesManId,
                    a.salesman.SalesManName,
                    a.contract.ContractStartDate.Value.Year,
                    a.contract.ContractStartDate.Value.Month
                })
                .Select(a => new
                {
                    SalesManId = a.Key.SalesManId,
                    SalesManName = a.Key.SalesManName,
                    Total = a.Sum(i => i.contract.ContractAmount),
                    Year=a.Key.Year,  
                    Month=a.Key.Month

                })
                .ToListAsync();
            data= data.OrderByDescending(a => a.Year)
                .OrderByDescending(a=>a.Month).ToList();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data);
        }

    }
}
