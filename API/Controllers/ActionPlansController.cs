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
    public class ActionPlansController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public ActionPlansController(Crm_DbContext context)
        {
            _context = context;
        }
        //通过行动编号查找
        [HttpPost("[action]/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<ApiResult>> find(ActionPlan actionPlan, int pageSize, int pageIndex)
        {
            var data = await _context.ActionPlans
                .Where(a => a.PlanId.Contains(actionPlan.PlanId))
                .Skip(pageSize * (pageIndex-1))
                .Take(pageSize)
                .ToListAsync();
            var datanum = await _context.ActionPlans
               .Where(a => a.PlanId.Contains(actionPlan.PlanId))
               .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, datanum.Count);
        }
        //通过销售员编号和客户编号查找行动计划
        [HttpGet("sid/{sid}/cid/{cid}")]
        public async Task<ActionResult<ApiResult>> Sid(int sid,int cid)
        {
            var actionPlan = await _context.ActionPlans
                .Where(a => a.SalesManId == sid)
                .Where(b => b.CustomerId == cid)
                .ToListAsync();
            if (actionPlan == null)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(actionPlan, actionPlan.Count);

        }

        //通过销售员编号查找行动计划
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ApiResult>> Sid (int id)
        {
            var actionPlan= await _context.ActionPlans
                .Where(a => a.SalesManId == id)
                .ToListAsync();
            if (actionPlan == null) {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(actionPlan,actionPlan.Count);

        }
        //通过客户编号查找行动计划
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ApiResult>> Cid(int id)
        {
            var actionPlan = await _context.ActionPlans
                .Where(a => a.CustomerId == id)
                .ToListAsync();
            if (actionPlan == null)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(actionPlan,actionPlan.Count);
        }

        // GET: api/ActionPlans
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetActionPlans()//返回所有的行动计划
        {
           var data= await _context.ActionPlans.ToListAsync();
            if (data.Count == 0) 
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/ActionPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetActionPlan(string id)//通过主键查找行动计划
        {
            
            var actionPlan = await _context.ActionPlans.FindAsync(id);
            
            if (actionPlan == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(actionPlan);
        }

        // PUT: api/ActionPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutActionPlan(string id, ActionPlan actionPlan)
        {
            if (id != actionPlan.PlanId)
            {
                return BadRequest();
            }

            _context.Entry(actionPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActionPlanExists(id))
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

        // POST: api/ActionPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostActionPlan(ActionPlan actionPlan)
        {
            /* actionPlan.PlanId = System.Guid.NewGuid().ToString("N");//用uuid生成主键*/
            try
            {
                var id = int.Parse(await _context.ActionPlans.Select(a => a.PlanId).MaxAsync());
                actionPlan.PlanId = (id + 1).ToString();
            }
            catch
            {
                actionPlan.PlanId = "20211207";
            }
            
            _context.ActionPlans.Add(actionPlan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActionPlanExists(actionPlan.PlanId))
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

        // DELETE: api/ActionPlans/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteActionPlan(string id)
        {
            var actionPlan = await _context.ActionPlans.FindAsync(id);
            if (actionPlan == null)
            {
                return ApiResultHelper.Error();
            }

            _context.ActionPlans.Remove(actionPlan);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool ActionPlanExists(string id)
        {
            return _context.ActionPlans.Any(e => e.PlanId == id);
        }
    }
}
