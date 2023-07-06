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
    public class AdminsController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public AdminsController(Crm_DbContext context)
        {
            _context = context;
        }

        //POST 修改密码
        [HttpPost("[action]/{newpwd}")]
        public async Task<ActionResult<ApiResult>> AlterPwd(Admin admin,string newpwd)
        {
            var data =await _context.Admins.Where(a => a.AdminId == admin.AdminId).FirstOrDefaultAsync();
            if (data != null)
            {
                GenerateMD5 mD5 = new GenerateMD5();
                var passwordMD5 = mD5.GenerateMD5Password(admin.AdminPwd);
                if (data.AdminPwd == passwordMD5)
                {
                    data.AdminPwd = mD5.GenerateMD5Password(newpwd);
                    _context.Entry(data).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AdminExists(admin.AdminId))
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
                else
                {
                    return ApiResultHelper.Error(600);
                }
            }
            else
            {
                return ApiResultHelper.Error(500);
            }
          

        }
        // POST: api/Admins 登录
        [HttpPost("[action]")]
        public async Task<ActionResult<ApiResult>> AdminLogin(Admin admin)
        {
            var data = await _context.Admins.Where(a => a.AdminId == admin.AdminId).FirstOrDefaultAsync();

            if (data ==  null)
            {
                return ApiResultHelper.Error();
            } else
            {
                GenerateMD5 mD5 = new GenerateMD5();
                var passwordMD5 = mD5.GenerateMD5Password(admin.AdminPwd);
                if (passwordMD5 == data.AdminPwd)
                {
                    return ApiResultHelper.Success();
                }
                else
                {
                    return ApiResultHelper.Error(600);
                }
            }
                       
        }
        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetAdmins()
        {   
            var data = await _context.Admins.ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data,data.Count);
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(admin);
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutAdmin(int id, Admin admin)
        {

            if (id != admin.AdminId)
            {
                return BadRequest();
            }
         
            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostAdmin(Admin admin)
        {
            GenerateMD5 mD5 = new GenerateMD5();
            admin.AdminPwd=mD5.GenerateMD5Password(admin.AdminPwd);
            _context.Admins.Add(admin);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdminExists(admin.AdminId))
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

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
    }
}
