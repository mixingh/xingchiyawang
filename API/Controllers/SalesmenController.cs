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
using crm_api.Uitl;

namespace crm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesmenController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public SalesmenController(Crm_DbContext context)
        {
            _context = context;
        }
        // 重置销售员密码
        [HttpPost("[action]/{id}")]
        public async Task<ActionResult<ApiResult>> RePwd(int id)
        {
            var data = await _context.Salesmen
                .Where(a => a.SalesManId == id)
                .FirstOrDefaultAsync();
            if (data == null)
            {
                return ApiResultHelper.Error(500);
            }
            else
            {
                    GenerateMD5 mD5 = new GenerateMD5();             
                   data.SalesManPwd = mD5.GenerateMD5Password("123456");
                    _context.Entry(data).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    return ApiResultHelper.Error();

                }
                return ApiResultHelper.Success();
            }

        }
        /*
         * 自定义查询 <2>
         * 通过 销售员的名字 模糊查询
         */
        // 销售员修改密码
        [HttpPost("[action]/{newpwd}")]
        public async Task<ActionResult<ApiResult>> AlterPwd(Salesman salesman, string newpwd)
        {
            var data = await _context.Salesmen
                .Where(a => a.SalesManId == salesman.SalesManId)
                .FirstOrDefaultAsync();
            if (data == null)
            {
                return ApiResultHelper.Error(500);
            }
            else
            {
                GenerateMD5 mD5 = new GenerateMD5();
                if (data.SalesManPwd == mD5.GenerateMD5Password(salesman.SalesManPwd))
                {
                    data.SalesManPwd = mD5.GenerateMD5Password(newpwd);

                    _context.Entry(data).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {

                        return ApiResultHelper.Error();

                    }
                    return ApiResultHelper.Success();
                }
                else
                {
                    return ApiResultHelper.Error(600);
                }
            }

        }
        // 销售员登录
        [HttpPost("[action]")]
        public async Task<ActionResult<ApiResult>> SalesLogin(Salesman salesman)
        {

            var data = await _context.Salesmen
                .Where(a => a.SalesManId == salesman.SalesManId)
                .FirstOrDefaultAsync();
            if (data == null)
            {
                return ApiResultHelper.Error(500);
            }
            else
            {
                GenerateMD5 mD5 = new GenerateMD5();
                if (data.SalesManPwd == mD5.GenerateMD5Password(salesman.SalesManPwd))
                {
                    return ApiResultHelper.Success();
                }
                else
                {
                    return ApiResultHelper.Error(600);
                }
            }

        }
        [HttpPost("[action]/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<ApiResult>> find(Salesman find,int pageSize,int pageIndex)
        {
            var datanum = await _context.Salesmen
                 /* .Where(s => s.SalesManId.ToString().Contains(find.SalesManId.ToString()))*/
                 .Where(a => a.SalesManName.Contains(find.SalesManName))
                  .ToListAsync();
            var data = await _context.Salesmen
                 /* .Where(s => s.SalesManId.ToString().Contains(find.SalesManId.ToString()))*/
                 .Where(a => a.SalesManName.Contains(find.SalesManName))
                  .Join(_context.SalesGroups, a => a.GroupId, b => b.GroupId, (a, b) => new
                  {
                      salesManId = a.SalesManId,
                      salesManName = a.SalesManName,
                      salesManPhone = a.SalesManPhone,
                      salesManImg = a.SalesManImg,
                      salesManPwd = a.SalesManPwd,
                      salesManEmail = a.SalesManEmail,
                      groupId = b.GroupId,
                      groupName = b.GroupName
                      
                  })
                 .Skip(pageSize * (pageIndex - 1))
                 .Take(pageSize)
                 .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, datanum.Count);
        }

        /*
         * 自定义查询 <2>
         * 通过 销售员的名字 模糊查询
         */
        [HttpGet("[action]/{name}")]
        public async Task<ActionResult<ApiResult>> Name(string name)
        { var data = await _context.Salesmen
                .Where(s => s.SalesManName.Contains(name))
                .ToListAsync();
            if(data.Count == 0) {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data,data.Count);
        }

        /* 自定义查询<1>
         * 通过主键 等值查询销售员 
         */
        // GET: api/Salesmen
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ApiResult>> id(int id)
        {
            var data = await _context.Salesmen
                .Where(a => a.SalesManId == id)
                 .Join(_context.SalesGroups, a => a.GroupId, b => b.GroupId, (a, b) => new
                 {
                     salesManId = a.SalesManId,
                     salesManName = a.SalesManName,
                     salesManPhone = a.SalesManPhone,
                     salesManImg = a.SalesManImg,
                     salesManPwd = a.SalesManPwd,
                     salesManEmail = a.SalesManEmail,
                     groupId = b.GroupId,
                     groupName = b.GroupName
                 })
                .ToListAsync();
            if (data.Count == 0) 
            {
                return ApiResultHelper.Error();  
            }
            return ApiResultHelper.Success(data, data.Count);
        }
        

        // GET: api/Salesmen
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetSalesmen()
        {
            var data= await _context.Salesmen
                 .Join(_context.SalesGroups, a => a.GroupId, b => b.GroupId, (a, b) => new
                 {
                     salesManId =a.SalesManId,
                      salesManName=a.SalesManName,
                     salesManPhone=a.SalesManPhone,
                      salesManImg =a.SalesManImg,
                       salesManPwd=a.SalesManPwd,
                     salesManEmail=a.SalesManEmail,
                groupId=b.GroupId,
                groupName=b.GroupName
                 })
                .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/Salesmen/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetSalesman(int id)
        {
            var data = await _context.Salesmen
                .Where(a => a.SalesManId == id)
                 .Join(_context.SalesGroups, a => a.GroupId, b => b.GroupId, (a, b) => new
                 {
                     salesManId = a.SalesManId,
                     salesManName = a.SalesManName,
                     salesManPhone = a.SalesManPhone,
                     salesManImg = a.SalesManImg,
                     salesManPwd = a.SalesManPwd,
                     salesManEmail = a.SalesManEmail,
                     groupId = b.GroupId,
                     groupName = b.GroupName
                 }).FirstOrDefaultAsync();

            if (data == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(data,1);
        }

        // PUT: api/Salesmen/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutSalesman(int id, Salesman salesman)
        {
            if (id != salesman.SalesManId)
            {
                return BadRequest();
            }
            var data = await _context.Salesmen.AsNoTracking().Where(a => a.SalesManId == salesman.SalesManId).FirstOrDefaultAsync();
            salesman.SalesManPwd = data.SalesManPwd;
            _context.Entry(salesman).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesmanExists(id))
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

        // POST: api/Salesmen
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostSalesman(Salesman salesman)
        {
            var id = await _context.Salesmen.Select(a => a.SalesManId).MaxAsync();
            salesman.SalesManId = id + 1;
            //salesman.SalesManPwd = "123456";
            salesman.GroupId = 4;
            GenerateMD5 mD5 = new GenerateMD5();
            salesman.SalesManPwd = mD5.GenerateMD5Password(salesman.SalesManPwd);
            _context.Salesmen.Add(salesman);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalesmanExists(salesman.SalesManId))
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

        // DELETE: api/Salesmen/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteSalesman(int id)
        {
            var salesman = await _context.Salesmen.FindAsync(id);
            if (salesman == null)
            {
                return ApiResultHelper.Error();
            }

            _context.Salesmen.Remove(salesman);
            await _context.SaveChangesAsync();
            return ApiResultHelper.Success();
        }

        private bool SalesmanExists(int id)
        {
            return _context.Salesmen.Any(e => e.SalesManId == id);
        }
    }
}
