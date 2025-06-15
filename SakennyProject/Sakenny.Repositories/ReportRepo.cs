using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;
using Sakenny.Core.DTO;
using Sakenny.Core.Helpers;
using System.Collections.Specialized;
using Sakenny.Core.Specification.SpecParam;

namespace Sakenny.Repository
{
    public class ReportRepo
    {
        private readonly SakennyDbContext sakennyDb;

       public ReportRepo(SakennyDbContext sakennyDb) {
            this.sakennyDb = sakennyDb;
        }
        public async Task<IReadOnlyList<ReportAdminDto>> Get(BaseSpecParam param)
        {
            var report = await sakennyDb.Reports
                .Include(p => p.To)
                .Include(p => p.From)
                .OrderByDescending(p => p.Id)
                .Skip((param.PageIndex-1)*param.PageIndex)
                .Take(param.PageSize)
                .Select(r => new ReportAdminDto()
                {
                    Id = r.Id,
                    Action = r.Action,
                    ContentId = r.ContentId,
                    Description = r.Description,
                    FName = r.From.FirstName + " " + r.From.LastName,
                    SName = r.To.FirstName + " " + r.To.LastName,
                    Status = r.Status,
                    Time = r.Time,
                    type = r.ContentType,
                }).OrderByDescending(r=>r.Id).ToListAsync();

            return report;
        }
        public async Task<bool> ReportBefore(string userId,int postId)
        {
            var report = await sakennyDb.Reports
                .FirstOrDefaultAsync(r => r.FromId == userId && r.ContentId == postId);
            return report != null;
        }
        public async Task<SingleReportDto> GetById(int Id)
        {
            var report = await sakennyDb.Reports
                .Include(p => p.To).Include(p => p.From).FirstOrDefaultAsync(u=>u.Id==Id);

            int count = 0;
            if(report==null)
                return new SingleReportDto();
            if(report.ContentType==ContentType.Post)
            {
                var ob= await sakennyDb.Posts.Include(p=>p.Reports).FirstOrDefaultAsync(p=>p.Id==report.ContentId);
                if(ob !=null)count=ob.Reports.Count();
            }else
            {
                var ob = await sakennyDb.Comments.Include(p => p.reports).FirstOrDefaultAsync(p => p.Id == report.ContentId);
                if (ob != null) count = ob.reports.Count();
            }
            var rt =
                    new SingleReportDto()
                    {
                        Id = report.Id,
                        Action = report.Action,
                        ContentId = report.ContentId,
                        Description = report.Description,
                        FName = report.To.FirstName + " " + report.To.LastName,
                        Status = report.Status,
                        Time = report.Time,
                        SName = report.From.FirstName + " " + report.From.LastName,
                        FId = report.To.Id,
                        SId = report.From.Id,
                        type = report.ContentType,
                        Content = (report.ContentType == ContentType.Post) ?
                        await sakennyDb.Posts.Include(p => p.Reports).FirstOrDefaultAsync(p => Id == report.ContentId)
                        :
                        await sakennyDb.Comments.Include(p => p.reports).FirstOrDefaultAsync(p => Id == report.ContentId)
                        ,
                        ContentReportCount = count,               
                    };
            return rt;
        }
        public async Task<bool> Delete(int Id, ContentType type)
        {
            if (type == ContentType.Post)
            {
                var post = await sakennyDb.Posts.FirstOrDefaultAsync(x => x.Id == Id);
                if(post ==null)
                    return false;
                post.ISDeleted = true;
                sakennyDb.SaveChanges();
            }
            else if (type == ContentType.Comment)
            {
                Comment? comment = await sakennyDb.Comments.FirstOrDefaultAsync(x => x.Id == Id);
                if (comment == null)
                    return false;
                    sakennyDb.Comments.Remove(comment);
                    sakennyDb.SaveChanges();
            }
            return true;
        }
        


    }
}
