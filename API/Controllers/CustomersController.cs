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
    public class CustomersController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public CustomersController(Crm_DbContext context)
        {
            _context = context;
        }
        // 客户修改密码
        [HttpPost("[action]/{newpwd}")]
        public async Task<ActionResult<ApiResult>> AlterPwd(Customer customer,string newpwd)
        {

            var data = await _context.Customers
                .Where(a => a.CustomerId == customer.CustomerId)
                .FirstOrDefaultAsync();
            if (data == null)
            {
                return ApiResultHelper.Error(500);
            }
            else
            {
                GenerateMD5 mD5 = new GenerateMD5();
                if (data.CustomerPwd == mD5.GenerateMD5Password(customer.CustomerPwd))
                {
                    data.CustomerPwd = mD5.GenerateMD5Password(newpwd);
                   
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
        // 客户登录
        [HttpPost("[action]")]
        public async Task<ActionResult<ApiResult>> CustomerLogin(Customer customer)
        {

            var data = await _context.Customers
                .Where(a => a.CustomerId == customer.CustomerId)
                .FirstOrDefaultAsync();
            if (data == null)
            {
                return ApiResultHelper.Error(500);
            }
            else
            {
                GenerateMD5 mD5 = new GenerateMD5();
                if (data.CustomerPwd == mD5.GenerateMD5Password(customer.CustomerPwd))
                {
                    return ApiResultHelper.Success();
                }
                else
                {
                    return ApiResultHelper.Error(600);
                }
            }
         
        }

        //分页查询
        [HttpPost("[action]/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<ApiResult>> find(Customer customer, int pageSize, int pageIndex)
        {
          
        
               var  data = await _context.Customers
               .Where(a => a.CustomerName != null && a.CustomerName.Contains(customer.CustomerName))
               .Where(a => a.CustomerAddress != null && a.CustomerAddress.Contains(customer.CustomerAddress))
               .Skip(pageSize * (pageIndex - 1))
               .Take(pageSize)
               .ToListAsync();
            var datanum = await _context.Customers
               .Where(a => a.CustomerName != null && a.CustomerName.Contains(customer.CustomerName))
               .Where(a => a.CustomerAddress != null && a.CustomerAddress.Contains(customer.CustomerAddress))
               .ToListAsync();
            if (data.Count == 0)
                {
                    return ApiResultHelper.Error();
                }
                return ApiResultHelper.Success(data, datanum.Count);

        }

        [HttpPut("[action]/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<ApiResult>> find1(Customer customer, int pageSize, int pageIndex)
        {

            var data = await _context.Customers
                  .Where(a => a.CustomerName != null && a.CustomerName.Contains(customer.CustomerName))
                  .Where(a => a.CustomerAddress != null && a.CustomerAddress.Contains(customer.CustomerAddress))
                  .Skip(pageSize * (pageIndex - 1))
                  .Take(pageSize)
                  .ToListAsync();

            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetCustomers()
        {   
            var data= await _context.Customers
                .ToListAsync();
            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetCustomers(int id)
        {

            var data = await _context.Customers
                  .Where(a =>a.CustomerId==id)
                  .ToListAsync();

            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(data, data.Count);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutCustomer(int id, Customer customer)
        {
       

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            /*    GenerateMD5 mD5 = new GenerateMD5();
                customer.CustomerPwd = mD5.GenerateMD5Password(customer.CustomerPwd);*/
             var data = await _context.Customers.AsNoTracking().Where(a => a.CustomerId == customer.CustomerId).FirstOrDefaultAsync();
            if (data.CustomerPwd != null)
            {
                customer.CustomerPwd = data.CustomerPwd;
            }
            
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostCustomer(Customer customer)
        {
            try
            {
                int customerId = await _context.Customers
                 .Select(a => a.CustomerId)
                 .MaxAsync();
                customer.CustomerId = customerId + 1;
            }
            catch
            {
                customer.CustomerId = 2021112601;
            }
            GenerateMD5 mD5 = new GenerateMD5();
            customer.CustomerPwd =mD5.GenerateMD5Password(customer.CustomerPwd);
            _context.Customers.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
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

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return ApiResultHelper.Error();
            }

            try
            {
                _context.Customers.Remove(customer);
            }
            catch(Exception)
            {
                return ApiResultHelper.Error();
            }
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
