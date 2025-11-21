using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace CandidateApi.DTOs
{
    public class CandidateCreateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? Company { get; set; }

        // From form: "Interested In" -> in grid it is "Designation"
        public string? InterestedIn { get; set; }

        public string? Budget { get; set; }

        // Angular will send this either as JSON string or CSV string; you can adapt
        public string? Technologies { get; set; }

        public string? AboutProject { get; set; }

        // Multiple document upload should be allowed (note in assignment)
        public List<IFormFile>? Documents { get; set; }
    }
}
