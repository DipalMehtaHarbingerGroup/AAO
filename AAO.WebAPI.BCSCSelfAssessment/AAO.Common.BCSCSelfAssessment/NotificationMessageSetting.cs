using System.ComponentModel.DataAnnotations;

namespace AAO.Common.BCSCSelfAssessment
{
    public class NotificationMessageSetting
    {
        public int NotificationMessageSettingId { get; set; }

        public int? UserId { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "This Field is required.")]
        [StringLength(500, ErrorMessage = "Write message (Up to 500 Characters)")]
        //[Range(0, 500, ErrorMessage = "Write message (Up to 500 Characters)")]
        public string NotificationMessage { get; set; }

        [Required(ErrorMessage = "This Field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "please enter a number greater than zero.")]
        //[Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int? NoOfTime { get; set; }

        //[Required(ErrorMessage = "This Field is required.")]
        //[Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int? OnholdScreenTime { get; set; }
        public bool? IsUnable { get; set; }
        public bool? ISDoYouWishToSeeThisMessageAgain { get; set; }
        public int Total { get; set; }
    }
}
