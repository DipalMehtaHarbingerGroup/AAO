namespace AAO.DTO.BCSCSelfAssessment
{
    using System.ComponentModel.DataAnnotations;

    public class UserDTO
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public int ProgramDirectorProgramID { get; set; }

        public bool IsActive { get; set; }

        public System.DateTime RegTime { get; set; }

        public string Role { get; set; }

        public bool isLoggedFirst { get; set; }

        public string RenewalMessage { get; set; }

        public string RenewalLink { get; set; }
    }

    public class UserDataDTO
    {
        public int UserId { get; set; }

        public string MasterCustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public System.DateTime? ResidencyStart { get; set; }

        public System.DateTime? ResidencyEnd { get; set; }

        public string ResidencyProgramName { get; set; }

        public string ResidencyProgramId { get; set; }

        public string ProgramDirectorProgramName { get; set; }

        public string ProgramDirectorProgramId { get; set; }

        public bool IsActive { get; set; }

        public System.DateTime RegTime { get; set; }

        public string Role { get; set; }

        public bool isLoggedFirst { get; set; }

        public string RenewalMessage { get; set; }

        public string RenewalLink { get; set; }
    }
}
