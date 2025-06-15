using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Operators;
using Sakenny.Core.DTO;
using Sakenny.Core.Helpers;
using Sakenny.Core.Models;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository;
using Sakenny.Repository.Data;
using SakennyProject.Erorrs;

namespace SakennyProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportRepo repo;
        private readonly SakennyDbContext sakennyDb;

        public ReportController(ReportRepo repo,SakennyDbContext sakennyDb) {
            this.repo = repo;
            this.sakennyDb = sakennyDb;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReportAdminDto>>> Get([FromQuery]BaseSpecParam param)
        {
            var res=await repo.Get(param);
            return Ok(res);
        }
        [HttpDelete("DeleteContent")]
        public async Task<ActionResult<bool>>Delete(int ReportId,int Id,ContentType type)
        {
          
            var report=await sakennyDb.Reports.FindAsync(ReportId);
            if (report == null)
                return BadRequest(new ApiResponse(400,"No Content"));
            var res = await repo.Delete(Id, type);
            if (!res)
                return BadRequest(new ApiResponse(400, "No Content"));

            report.Action=Sakenny.Core.Helpers.Action.DeleteContent;
            report.Status=Status.Solved;
            sakennyDb.Reports.Update(report);
            sakennyDb.SaveChanges();
            return Ok(res);
        }
        [HttpPut("DeleteUser")]
        public async Task<ActionResult<bool>> DeleteUser(string UserId,int ReportId)
        {
            var user=await sakennyDb.Users.FindAsync(UserId);
            if(user ==null  )
                return BadRequest(new ApiResponse(400, "No User"));
            user.IsDeleted=true;
            sakennyDb.Users.Update(user);
            var report = await sakennyDb.Reports.FindAsync(ReportId);
            if (report == null)
                return BadRequest(new ApiResponse(400, "No Report")); 
            report.Action = Sakenny.Core.Helpers.Action.DeleteUser;
            report.Status = Status.Solved;
            sakennyDb.Reports.Update(report);

            sakennyDb.SaveChanges() ;
            return Ok();
        }
        [HttpPut("NoAction")]
        public async Task<ActionResult<bool>> NoAction( int ReportId)
        {
            var report = await sakennyDb.Reports.FindAsync(ReportId);
            if (report == null)
                return BadRequest(new ApiResponse(400, "No Report"));
            report.Action = Sakenny.Core.Helpers.Action.NoAction;
            report.Status = Status.Solved;
            sakennyDb.Reports.Update(report);
            sakennyDb.SaveChanges();
            return Ok();
        }
        [HttpGet("GetById")]
        public async Task<ActionResult<SingleReportDto>> GetById(int Id)
        {
            var report = await repo.GetById(Id);
            return Ok(report);
        }
    }
}
