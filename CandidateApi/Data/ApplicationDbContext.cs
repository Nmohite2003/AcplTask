using Microsoft.EntityFrameworkCore;
using CandidateApi.Models;

namespace CandidateApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateDocument> CandidateDocuments { get; set; }
    }
}
