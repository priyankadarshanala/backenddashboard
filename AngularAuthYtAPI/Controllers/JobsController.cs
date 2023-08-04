using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Interface;
using AngularAuthYtAPI.Migrations;
using AngularAuthYtAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AngularAuthYtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobsRepository _jobsRepository;

        public JobsController(IJobsRepository jobsRepository)
        {
            _jobsRepository = jobsRepository;
           
        }

        [HttpGet]
        public async Task<IEnumerable<Jobs>> GetAllJobs()
        {
            return await _jobsRepository.GetAllJobs();
        }

        [HttpGet("company/{companyName}")]
        public async Task<IEnumerable<Jobs>> GetJobsByCompany(string companyName)
        {
            return await _jobsRepository.GetJobsByCompany(companyName);
        }

        [HttpGet("count")]
        public async Task<int> GetTotalJobsCount()
        {
            return await _jobsRepository.GetTotalJobsCount();
        }


        [HttpGet("TotalAppliedJobsCount")]
        public async Task<int> GetTotalAppliedJobsCount()
        {
            return await _jobsRepository.GetTotalAppliedJobsCount();
        }

        [HttpGet("NotAppliedJobsCount")]
        public async Task<int> GetNotAppliedJobsCount()
        {
            var totalJobsCount = await _jobsRepository.GetTotalJobsCount();
            var appliedJobsCount = await _jobsRepository.GetTotalAppliedJobsCount();
            var notAppliedJobsCount = totalJobsCount - appliedJobsCount;
            return notAppliedJobsCount;
        }



        [HttpPost]
        public async Task<IActionResult> AddJob(Jobs job)
        {
            job.PostedDate = DateTime.UtcNow;
            job.EndDate = DateTime.UtcNow;
            await _jobsRepository.AddJob(job);
            return Ok();
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJob(int jobId)
        {
            var job = await _jobsRepository.GetJob(jobId);
            if (job == null)
                return NotFound();

            return Ok(job);
        }
        [HttpPut("{jobId}")]
        public async Task<IActionResult> UpdateJob(int jobId, Jobs updatedJob)
        {
            // Retrieve the existing job from the database
            var existingJob = await _jobsRepository.GetJob(jobId);
            if (existingJob == null)
                return NotFound();

            // Update only the specified properties from the updatedJob object
            existingJob.CompanyName = updatedJob.CompanyName;
            existingJob.JobTitle = updatedJob.JobTitle;
            existingJob.Experience = updatedJob.Experience;
            existingJob.Skills = updatedJob.Skills;
            existingJob.JobType = updatedJob.JobType;
            existingJob.Qualification = updatedJob.Qualification;
            existingJob.CompanyLocation = updatedJob.CompanyLocation;
            existingJob.Vacancy = updatedJob.Vacancy;
           
            existingJob.Salary = updatedJob.Salary;
            existingJob.JobDescription = updatedJob.JobDescription;

            await _jobsRepository.UpdateJob(existingJob);
            return Ok();
        }



        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteJob(int jobId)
        {
            await _jobsRepository.DeleteJob(jobId);
            return Ok();
        }

        [HttpGet("last-week")]
        public async Task<IActionResult> GetJobsLastWeek()
        {
            var jobs = await _jobsRepository.GetJobsLastWeek();

            if (jobs.Count() == 0)
            {
                return NotFound("No jobs found for the last week.");
            }

            return Ok(jobs);
        }

        [HttpGet("last-month")]
        public async Task<IActionResult> GetJobsLastMonth()
        {
            var jobs = await _jobsRepository.GetJobsLastMonth();

            if (jobs.Count() == 0)
            {
                return NotFound("No jobs found for the last month.");
            }

            return Ok(jobs);
        }

        [HttpGet("last-year")]
        public async Task<IActionResult> GetJobsLastYear()
        {
            var jobs = await _jobsRepository.GetJobsLastYear();

            if (jobs.Count() == 0)
            {
                return NotFound("No jobs found for the last year.");
            }

            return Ok(jobs);
        }
        [HttpGet("last-week/count")]
        public async Task<IActionResult> GetJobsLastWeekCount()
        {
            var count = await _jobsRepository.GetJobsLastWeekCount();
            return Ok(count);
        }

        [HttpGet("last-month/count")]
        public async Task<IActionResult> GetJobsLastMonthCount()
        {
            var count = await _jobsRepository.GetJobsLastMonthCount();
            return Ok(count);
        }

        [HttpGet("last-year/count")]
        public async Task<IActionResult> GetJobsLastYearCount()
        {
            var count = await _jobsRepository.GetJobsLastYearCount();
            return Ok(count);
        }

        public class JobsRequest
        {
            public Jobs JobsObj { get; set; }
        }

    }
}
