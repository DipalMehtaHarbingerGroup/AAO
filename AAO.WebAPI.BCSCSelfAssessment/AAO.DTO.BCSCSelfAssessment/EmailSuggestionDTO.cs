namespace AAO.DTO.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EmailSuggestionDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}