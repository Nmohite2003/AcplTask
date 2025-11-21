using System;

namespace CandidateApi.Models
{
    public class CandidateDocument
    {
        public int Id { get; set; }

        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; } = null!;

        public string FileName { get; set; } = string.Empty;   // original file name
        public string FilePath { get; set; } = string.Empty;   // relative path: "uploads/xyz.pdf"

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
