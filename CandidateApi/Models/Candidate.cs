using System;
using System.Collections.Generic;

namespace CandidateApi.Models
{
    public class Candidate
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? Company { get; set; }

        // In the assignment: "Designation in grid and interested in field are same"
        public string? Designation { get; set; }   // maps from InterestedIn field of the form

        public string? Budget { get; set; }

        // Multi-select technologies; stored as comma-separated string: "Angular,React,SQL"
        public string? Technologies { get; set; }

        public string? AboutProject { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CandidateDocument> Documents { get; set; } = new List<CandidateDocument>();
    }
}
