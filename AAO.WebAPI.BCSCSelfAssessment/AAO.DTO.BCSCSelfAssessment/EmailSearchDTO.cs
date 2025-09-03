namespace AAO.DTO.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EmailSearchDTO
    {
        public string SearchTerm { get; set; }
        public int UserId { get; set; }
}
}