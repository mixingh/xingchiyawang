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
using crm_api.Models.FindModels;
using crm_api.Models.ViewModes;

namespace crm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly Crm_DbContext _context;

        public ComplaintsController(Crm_DbContext context)
        {
            _context = context;
        }
        [HttpPost("[action]/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<ApiResult>> find(FindComplaints find, int pageSize, int pageIndex)
        {
           
            if (find.CustomerName == "")
            {
               var  query = from u in _context.Complaints
                            where (u.ComplaintId.Contains(find.ComplaintId))
                            join a in
                                 (from c in _context.Customers
                                  where c.CustomerName.Contains(find.CustomerName)
                                  select new
                                  {
                                      c.CustomerId,
                                  }) on u.CustomerId equals a.CustomerId into jointemp from 
                        leftjoin in jointemp.DefaultIfEmpty()       
                            select new
                            {
                                ComplaintId = u.ComplaintId,
                                SalesManId = u.SalesManId,
                                SalesManName = u.SalesMan.SalesManName,
                                CustomerId = u.CustomerId,
                                CustomerName = u.Customer.CustomerName,
                                ComplaintInfo = u.ComplaintInfo,
                                ComplaintStyle = u.ComplaintStyle,
                                ComplaintState = u.ComplaintState,
                                ComplaintTime = u.ComplaintTime,
                                ComplaintHandleTime = u.ComplaintHandleTime,
                                ComplaintExplain = u.ComplaintExplain,
                                ComplaintOverTime = u.ComplaintOverTime,
                                ComplaintResult = u.ComplaintResult
                            };
                var data = await query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                var datanum = await query.ToListAsync();

                if (data.Count == 0)
                {
                    return ApiResultHelper.Error();
                }
                return ApiResultHelper.Success(data, datanum.Count);
            }
            else
            {
                var query = from u in _context.Complaints
                            where (u.ComplaintId.Contains(find.ComplaintId))
                            join a in
                                 (from c in _context.Customers
                                  where c.CustomerName.Contains(find.CustomerName)
                                  select new
                                  {
                                      c.CustomerId,
                                  }) on u.CustomerId equals a.CustomerId /*into jointemp from 
                        leftjoin in jointemp.DefaultIfEmpty()  */
                            select new
                            {
                                ComplaintId = u.ComplaintId,
                                SalesManId = u.SalesManId,
                                SalesManName = u.SalesMan.SalesManName,
                                CustomerId = u.CustomerId,
                                CustomerName = u.Customer.CustomerName,
                                ComplaintInfo = u.ComplaintInfo,
                                ComplaintStyle = u.ComplaintStyle,
                                ComplaintState = u.ComplaintState,
                                ComplaintTime = u.ComplaintTime,
                                ComplaintHandleTime = u.ComplaintHandleTime,
                                ComplaintExplain = u.ComplaintExplain,
                                ComplaintOverTime = u.ComplaintOverTime,
                                ComplaintResult = u.ComplaintResult
                            };
                var data = await query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                var datanum = await query.ToListAsync();

                if (data.Count == 0)
                {
                    return ApiResultHelper.Error();
                }
                return ApiResultHelper.Success(data, datanum.Count);
            }
          
          
        }
        // GET: api/Complaints 根据顾客名字模糊查询
        [HttpGet("[action]/{cname}")]
        public async Task<ActionResult<ApiResult>> Byname(string cname)
        {
            var idList = await _context.Customers
                .Where(a => a.CustomerName.Contains(cname))
                .Select(a => a.CustomerId)
                .ToListAsync();
            dynamic data=null;
            foreach (int id in idList)
            {
                data = await _context.Complaints
                    .Where(a => a.CustomerId == id)
                    .ToListAsync();
            }

            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/Complaints 根据顾客id模糊查询
        [HttpGet("[action]/{cid}")]
        public async Task<ActionResult<ApiResult>> Cid(int cid)
        {
            var data = await _context.Complaints
                .Where(a => a.CustomerId.ToString().Contains(cid.ToString()))
                .ToListAsync();
            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }
        // GET: api/Complaints
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetComplaints()
        {
            var query = from u in _context.Complaints
                        join p in _context.Customers on u.CustomerId equals p.CustomerId into jointemp
                        from leftjoin in jointemp.DefaultIfEmpty()
                        join s in _context.Salesmen on u.SalesManId equals s.SalesManId into sjointemp
                        from sleftjoin in sjointemp.DefaultIfEmpty()
                        select new
                        {
                            ComplaintId = u.ComplaintId,
                            SalesManId = u.SalesManId,
                            SalesManName = u.SalesMan.SalesManName,
                            CustomerId = u.CustomerId,
                            CustomerName = u.Customer.CustomerName,
                            ComplaintInfo = u.ComplaintInfo,
                            ComplaintStyle = u.ComplaintStyle,
                            ComplaintState = u.ComplaintState,
                            ComplaintTime = u.ComplaintTime,
                            ComplaintHandleTime = u.ComplaintHandleTime,
                            ComplaintExplain = u.ComplaintExplain,
                            ComplaintOverTime = u.ComplaintOverTime,
                            ComplaintResult = u.ComplaintResult
                        };
            var data = await query.ToListAsync();
    

            if (data.Count == 0)
            {
                return ApiResultHelper.Error();
            }
            return ApiResultHelper.Success(data, data.Count);
        }

        // GET: api/Complaints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetComplaint(string id)
        {
            var query = from u in _context.Complaints
                        where(u.ComplaintId == id)
                        join p in _context.Customers on u.CustomerId equals p.CustomerId into jointemp
                        from leftjoin in jointemp.DefaultIfEmpty()
                        join s in _context.Salesmen on u.SalesManId equals s.SalesManId into sjointemp
                        from sleftjoin in sjointemp.DefaultIfEmpty()
                        select new
                        {
                            ComplaintId = u.ComplaintId,
                            SalesManId = u.SalesManId,
                            SalesManName = u.SalesMan.SalesManName,
                            CustomerId = u.CustomerId,
                            CustomerName = u.Customer.CustomerName,
                            ComplaintInfo = u.ComplaintInfo,
                            ComplaintStyle = u.ComplaintStyle,
                            ComplaintState = u.ComplaintState,
                            ComplaintTime = u.ComplaintTime,
                            ComplaintHandleTime = u.ComplaintHandleTime,
                            ComplaintExplain = u.ComplaintExplain,
                            ComplaintOverTime = u.ComplaintOverTime,
                            ComplaintResult = u.ComplaintResult
                        };
            var data = await query.FirstOrDefaultAsync();
            if (data == null)
            {
                return ApiResultHelper.Error();
            }

            return ApiResultHelper.Success(data,1);
        }

        // PUT: api/Complaints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> PutComplaint(string id, Complaint complaint)
        {
            if (id != complaint.ComplaintId)
            {
                return BadRequest();
            }

            _context.Entry(complaint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplaintExists(id))
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

        // POST: api/Complaints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult>> PostComplaint(Complaint complaint)
        {
            double id;
            try
            {
                id = double.Parse(await _context.Complaints
                    .Select(a => a.ComplaintId)
                    .MaxAsync());
            }
            catch
            {
                id = 2021194212378;
            }


            complaint.ComplaintId = (id + 1).ToString();
            _context.Complaints.Add(complaint);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ComplaintExists(complaint.ComplaintId))
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

        // DELETE: api/Complaints/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> DeleteComplaint(string id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return ApiResultHelper.Error();
            }

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();

            return ApiResultHelper.Success();
        }

        private bool ComplaintExists(string id)
        {
            return _context.Complaints.Any(e => e.ComplaintId == id);
        }
    }
}
