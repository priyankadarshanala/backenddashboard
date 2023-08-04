using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Migrations;
using AngularAuthYtAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularAuthYtAPI.Models;


namespace AngularAuthYtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeClassController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResumeClassController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("{jobId}")]
        public async Task<IActionResult> PostResume(int jobId, [FromForm] ResumeRequest request)
        {
            if (request.ResumeFile == null || request.ResumeFile.Length == 0)
            {
                return BadRequest("Resume file is required.");
            }

            var appliedJob = await _context.jobs.FindAsync(jobId);

            if (appliedJob == null)
            {
                return NotFound("Applied job not found.");
            }

            var resume = new ResumeClass
            {
                Name = request.Name,
                email = request.Email,
                JobId = jobId,
                FileName = request.ResumeFile.FileName,
                FileData = await GetFileData(request.ResumeFile),
                ApplicationDate = DateTime.UtcNow // Set the application date with the current date and time
            };

            _context.ResumesUpload.Add(resume);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResume", new { id = resume.ResumeId }, resume);
        }


        //[HttpPost("{jobId}")]
        //public async Task<IActionResult> PostResume(int jobId, [FromForm] ResumeRequest request)
        //{
        //    if (request.ResumeFile == null || request.ResumeFile.Length == 0)
        //    {
        //        return BadRequest("Resume file is required.");
        //    }

        //    var appliedJob = await _context.jobs.FindAsync(jobId);

        //    if (appliedJob == null)
        //    {
        //        return NotFound("Applied job not found.");
        //    }

        //    var resume = new ResumeClass
        //    {
        //        Name = request.Name,
        //        email = request.Email,
        //        JobId = jobId,
        //        FileName = request.ResumeFile.FileName,
        //        FileData = await GetFileData(request.ResumeFile)
        //    };

        //    _context.ResumesUpload.Add(resume);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetResume", new { id = resume.ResumeId }, resume);
        //}

        private async Task<byte[]> GetFileData(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        //[HttpGet("JobApplicantsCount/{jobId}")]
        //public async Task<int> GetJobApplicantsCount(int jobId)
        //{
        //    return await _context.ResumesUpload.CountAsync(resume => resume.JobId == jobId);
        //}
        //[HttpGet("TotalApplicantsCount")]
        //public async Task<int> GetTotalApplicantsCount()
        //{
        //    return await _context.ResumesUpload.CountAsync();
        //}
        [HttpGet("TotalApplicantsCount")]
        public async Task<int> GetTotalApplicantsCount(string timePeriod)
        {
            DateTime startDate;

            switch (timePeriod)
            {
                case "last-week":
                    startDate = DateTime.UtcNow.AddDays(-7);
                    break;
                case "last-month":
                    startDate = DateTime.UtcNow.AddMonths(-1);
                    break;
                case "last-year":
                    startDate = DateTime.UtcNow.AddYears(-1);
                    break;
                default:
                    return await _context.ResumesUpload.CountAsync(); // Return total applicants without filtering
            }

            return await _context.ResumesUpload.CountAsync(resume => resume.ApplicationDate >= startDate);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobResumeViewModel>>> GetResumes()
        {
            var jobResumes = await _context.jobs
                .Where(job => _context.ResumesUpload.Any(resume => resume.JobId == job.JobId))
                .Select(job => new JobResumeViewModel
                {

                    CompanyName = job.CompanyName,
                    JobTitle = job.JobTitle,
                    Experience = job.Experience,
                    Skills = job.Skills,
                    Vacancy = job.Vacancy,
                    JobType = job.JobType,
                    Qualification = job.Qualification,
                    PostedDate = job.PostedDate,
                    EndDate = job.EndDate,
                    Salary = job.Salary,
                    CompanyLocation = job.CompanyLocation,
                    JobDescription = job.JobDescription,
                    Resumes = _context.ResumesUpload
                        .Where(resume => resume.JobId == job.JobId)
                        .Select(resume => new ResumeViewModel
                        {
                            ApplicantName = resume.Name,
                            ApplicantEmail = resume.email,
                            ResumeFileName = resume.FileName,
                            ApplicationDate=resume.ApplicationDate,
                            ResumeFileData = Convert.ToBase64String(resume.FileData)
                        })
                        .ToList()
                })
                .ToListAsync();

            return jobResumes;
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<ResumeClass>> GetResume(int id)
        {
            var resume = await _context.ResumesUpload.FindAsync(id);

            if (resume == null)
            {
                return NotFound();
            }

            return resume;
        }

        [HttpGet("AppliedJobsCount")]
        public async Task<int> GetTotalAppliedJobsCount()
        {
            return await _context.ResumesUpload.CountAsync();
        }

        //[HttpGet("RecentApplicants")]
        //public async Task<ActionResult<IEnumerable<RecentApplicantViewModel>>> GetRecentApplicants()
        //{
        //    // You can define "recent" as per your requirement, for example, the last 30 days.
        //    DateTime startDate = DateTime.UtcNow.AddDays(-30);

        //    var recentApplicants = await _context.ResumesUpload
        //        .Where(resume =>
        //            resume.Name != null &&
        //            resume.email != null &&
        //            resume.job.JobTitle != null &&
        //            resume.job.CompanyLocation != null)

        //        .Select(resume => new RecentApplicantViewModel
        //        {
        //            ApplicantName = resume.Name,
        //            ApplicantEmail = resume.email,
        //            JobTitle = resume.job.JobTitle,
        //            CompanyLocation = resume.job.CompanyLocation,
        //            Skills = resume.job.Skills // Add this line to include Skills property
        //        })

        //        .ToListAsync();

        //    return recentApplicants;
        //}

        [HttpGet("RecentApplicants")]
        public async Task<ActionResult<IEnumerable<RecentApplicantViewModel>>> GetRecentApplicants()
        {
            // You can define "recent" as per your requirement, for example, the last 30 days.
            DateTime startDate = DateTime.UtcNow.AddDays(-30);

            var recentApplicants = await _context.ResumesUpload
                .Where(resume =>
                    resume.Name != null &&
                    resume.email != null &&
                    resume.job.JobTitle != null &&
                    resume.job.CompanyLocation != null)

                .Select(resume => new RecentApplicantViewModel
                {
                    ApplicantName = resume.Name,
                    ApplicantEmail = resume.email,
                    JobTitle = resume.job.JobTitle,
                    CompanyLocation = resume.job.CompanyLocation,
                    Skills = resume.job.Skills,
                    ApplicationDate = resume.ApplicationDate // Add this line to include the applicationDate property
                })
                .OrderByDescending(applicant => applicant.ApplicationDate) // Sort by ApplicationDate in descending order
                .Take(10) // Take the first 10 records
                .ToListAsync();

            return recentApplicants;
        }



        [HttpGet("{jobId}/Download")]
        public async Task<IActionResult> DownloadResume(int jobId)
        {
            var resume = await _context.ResumesUpload.FirstOrDefaultAsync(r => r.JobId == jobId);

            if (resume == null)
            {
                return NotFound();
            }

            var memoryStream = new MemoryStream(resume.FileData);
            return File(memoryStream, "application/octet-stream", resume.FileName);
        }
    }



    public class RecentApplicantViewModel
    {
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public string JobTitle { get; set; }
        public string CompanyLocation { get; set; }
        public string Skills { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
    internal class ResumesClass
    {
        public string Name { get; set; }
        public string email { get; set; }
        public int JobId { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }

    public class ResumeRequest
    {
        public string Name { get; set; }

        //public int JobId { get; set; }
        public string Email { get; set; }
        public IFormFile ResumeFile { get; set; }
    }
    public class ResumeViewModel
    {
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public string? ResumeFileName { get; set; }
        public string? ResumeFileData { get; set; }
        public DateTime ApplicationDate { get; internal set; }
    }

    public class JobResumeViewModel
    {
        public int JobId { get; set; }
        public string? CompanyName { get; set; }
        public string? JobTitle { get; set; }
        public string? Experience { get; set; }
        public string? Skills { get; set; }
        public string? Qualification { get; set; }
        public string? JobType { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime EndDate { get; set; }
        public float? Salary { get; set; }
        public int Vacancy { get; set; }
        public string? CompanyLocation { get; set; }
        public string? JobDescription { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public string? ResumeFileName { get; set; }
        public List<ResumeViewModel> Resumes { get; set; }
    }
}

