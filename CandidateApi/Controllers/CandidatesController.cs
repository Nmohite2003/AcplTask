using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CandidateApi.Data;
using CandidateApi.DTOs;
using CandidateApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CandidateApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CandidatesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/candidates?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetCandidates(int page = 1, int pageSize = 10)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Candidates
                .Include(c => c.Documents)
                .OrderByDescending(c => c.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                items
            });
        }

        // GET: api/candidates/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCandidate(int id)
        {
            var candidate = await _context.Candidates
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidate == null) return NotFound();

            return Ok(candidate);
        }

        // POST: api/candidates
        // Accepts multipart/form-data from Angular (FormData)
        [HttpPost]
        public async Task<IActionResult> CreateCandidate([FromForm] CandidateCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var candidate = new Candidate
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Contact = dto.Contact,
                Company = dto.Company,
                Designation = dto.InterestedIn,      // Designation in grid = InterestedIn in form
                Budget = dto.Budget,
                Technologies = dto.Technologies,
                AboutProject = dto.AboutProject
            };

            // Handle multiple document upload
            if (dto.Documents != null && dto.Documents.Any())
            {
                var root = _env.WebRootPath;
                if (string.IsNullOrEmpty(root))
                {
                    root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                var uploadFolder = Path.Combine(root, "uploads");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                foreach (var file in dto.Documents)
                {
                    if (file.Length <= 0) continue;

                    var uniqueName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadFolder, uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    candidate.Documents.Add(new CandidateDocument
                    {
                        FileName = file.FileName,
                        FilePath = Path.Combine("uploads", uniqueName)
                    });
                }
            }

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCandidate), new { id = candidate.Id }, candidate);
        }

        // DELETE: api/candidates/5 (optional)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var candidate = await _context.Candidates
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidate == null) return NotFound();

            // Optionally delete files from disk
            foreach (var doc in candidate.Documents)
            {
                var root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var fullPath = Path.Combine(root, doc.FilePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
