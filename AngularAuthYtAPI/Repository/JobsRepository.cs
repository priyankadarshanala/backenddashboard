using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Interface;
using AngularAuthYtAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthYtAPI.Respository
{
    public class JobsRepository : IJobsRepository
    {
        private readonly AppDbContext _dbContext;

        public JobsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Jobs>> GetAllJobs()
        {
            return await _dbContext.jobs.ToListAsync();
        }

        public async Task<IEnumerable<Jobs>> GetJobsByCompany(string companyName)
        {
            return await _dbContext.jobs
                .Where(j => j.CompanyName == companyName)
                .ToListAsync();
        }
        public async Task<IEnumerable<Jobs>> GetJobsLastWeek()
        {
            var lastWeek = DateTime.UtcNow.AddDays(-7);
            return await _dbContext.jobs.Where(job => job.PostedDate >= lastWeek).ToListAsync();
        }

        public async Task<IEnumerable<Jobs>> GetJobsLastMonth()
        {
            var today = DateTime.UtcNow;
            var firstDayOfLastMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfLastMonth.AddMonths(1).AddDays(-1);

            return await _dbContext.jobs
                .Where(job => job.PostedDate >= firstDayOfLastMonth && job.PostedDate <= lastDayOfLastMonth)
                .ToListAsync();
        }

        public async Task<IEnumerable<Jobs>> GetJobsLastYear()
        {
            var today = DateTime.UtcNow;
            var firstDayOfLastYear = new DateTime(today.Year - 1, 1, 1);
            var lastDayOfLastYear = new DateTime(today.Year - 1, 12, 31);

            return await _dbContext.jobs
                .Where(job => job.PostedDate >= firstDayOfLastYear && job.PostedDate <= lastDayOfLastYear)
                .ToListAsync();
        }
        public async Task<int> GetTotalJobsCount()
        {
            return await _dbContext.jobs.CountAsync();
        }
        public async Task<int> GetTotalAppliedJobsCount()
        {
            return await _dbContext.ResumesUpload.Select(r => r.JobId).Distinct().CountAsync();
        }
        public async Task AddJob(Jobs job)
        {
            _dbContext.jobs.Add(job);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Jobs> GetJob(int jobId)
        {
            return await _dbContext.jobs.FindAsync(jobId);
        }

        public async Task UpdateJob(Jobs job)
        {
            _dbContext.Entry(job).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteJob(int jobId)
        {
            var job = await _dbContext.jobs.FindAsync(jobId);
            if (job != null)
            {
                _dbContext.jobs.Remove(job);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> GetJobsLastWeekCount()
        {
            var startDate = DateTime.UtcNow.AddDays(-7);
            var endDate = DateTime.UtcNow;
            return await _dbContext.jobs.CountAsync(job => job.PostedDate >= startDate && job.PostedDate <= endDate);
        }

        public async Task<int> GetJobsLastMonthCount()
        {
            var startDate = DateTime.UtcNow.AddMonths(-1);
            var endDate = DateTime.UtcNow;
            return await _dbContext.jobs.CountAsync(job => job.PostedDate >= startDate && job.PostedDate <= endDate);
        }

        public async Task<int> GetJobsLastYearCount()
        {
            var startDate = DateTime.UtcNow.AddYears(-1);
            var endDate = DateTime.UtcNow;
            return await _dbContext.jobs.CountAsync(job => job.PostedDate >= startDate && job.PostedDate <= endDate);
        }

    }
}
