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
    public class SalesGroupsController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public SalesGroupsController(Crm_DbContext context)
        {
            _context = context;
        }
        //销售员默认分组
        [HttpPost("[action]/{id}")]
        public async Task<ActionResult<ApiResult>> DefaultSalesGroup(int id)
        {
            var data = await _context.Salesmen.Where(a => a.SalesManId == id)
                .FirstOrDefaultAsync();
            data.GroupId = 4;

            _context.Entry(data).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesGroupExists(id))
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
        // GET: api/SalesGroups
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetSalesGroups()
        {
            var data= await _context.SalesGroups.ToListAsync();
            if (data.Count == 0) 
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/SalesGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetSalesGroup(int id)
        {
            var salesGroup = await _context.SalesGroups.FindAsync(id);

            if (salesGroup == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(salesGroup,1);
        }

        // PUT: api/SalesGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutSalesGroup(int id, SalesGroup salesGroup)
        {
            if (id != salesGroup.GroupId)
            {
                return BadRequest();
            }

            _context.Entry(salesGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesGroupExists(id))
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

        // POST: api/SalesGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostSalesGroup(SalesGroup salesGroup)
        {
           int id = await _context.SalesGroups.Select(a => a.GroupId).MaxAsync();
            salesGroup.GroupId = id + 1;
            _context.SalesGroups.Add(salesGroup);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalesGroupExists(salesGroup.GroupId))
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
        // DELETE: api/SalesGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteSalesGroup(int id)
        {
            var salesGroup = await _context.SalesGroups.FindAsync(id);
            if (salesGroup == null)
            {
                return ApiResultHelper.Error();
            }

            _context.SalesGroups.Remove(salesGroup);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }
        private bool SalesGroupExists(int id)
        {
            return _context.SalesGroups.Any(e => e.GroupId == id);
        }
    }
}
