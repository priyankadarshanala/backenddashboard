using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Interface;
using AngularAuthYtAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static AngularAuthYtAPI.Controllers.JobsController;

namespace AngularAuthYtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppliedController : ControllerBase

    {

        private readonly AppDbContext _context;
        public AppliedController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("AppliedJobs")]
        public async Task<ActionResult<IEnumerable<Applied>>> GetAllAppliedJobs()
        {
            var appliedJobs = await _context.appliedjobs.ToListAsync();
            return Ok(appliedJobs);
        }

        [HttpPost("ApplyForJob")]
        public async Task<ActionResult<Applied>> ApplyForJob(JobsRequest request)
        {

            var jobId = request.JobsObj.JobId;
            var job = await _context.jobs.FindAsync(jobId);



            if (job == null)
            {
                return NotFound();
            }



            // Update the 'Applied' property of the job
            job.Applied = true;

            var appliedJob = new Applied
            {

                CompanyName = job.CompanyName,
                JobTitle = job.JobTitle,
                Experience = job.Experience,
                Skills = job.Skills,
                Vacancy = job.Vacancy,
                JobType = job.JobType,
                Qualification = job.Qualification,
                PostedDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow,
            Salary = job.Salary,
                Location = job.CompanyLocation,
                JobDescription = job.JobDescription,
            };



            _context.appliedjobs.Add(appliedJob);
            await _context.SaveChangesAsync();




            return Ok(appliedJob);
        }
    }
}
