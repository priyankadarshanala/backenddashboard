using AngularAuthYtAPI.Models;

namespace AngularAuthYtAPI.Interface
{
    public interface IJobsRepository
    {
        Task<IEnumerable<Jobs>> GetAllJobs();
        Task<IEnumerable<Jobs>> GetJobsByCompany(string companyName);
        Task<int> GetTotalJobsCount();
        Task<int> GetTotalAppliedJobsCount();
        Task AddJob(Jobs job);
        Task<Jobs> GetJob(int jobId);
        Task UpdateJob(Jobs job);
        Task DeleteJob(int jobId);
        Task<IEnumerable<Jobs>> GetJobsLastWeek();
        Task<IEnumerable<Jobs>> GetJobsLastMonth();
        Task<IEnumerable<Jobs>> GetJobsLastYear();

        Task<int> GetJobsLastWeekCount();
        Task<int> GetJobsLastMonthCount();
        Task<int> GetJobsLastYearCount();

    }
}
