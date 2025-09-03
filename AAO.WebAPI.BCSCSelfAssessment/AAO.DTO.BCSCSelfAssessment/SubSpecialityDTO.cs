namespace AAO.DTO.BCSCSelfAssessment
{
    public class SubSpecialityDTO
    {
        public int SubspecialtyId { get; set; }

        public int QAS_subspecialtyid { get; set; }

        public int BCSCSectionNumber { get; set; }

        public string BCSCSectionTitle { get; set; }

        public int SubprogramId { get; set; }

        public string SpecialityName { get; set; }

        public int ChapterId { get; set; }

        public string ChapterName { get; set; }

        public bool IsActive { get; set; }
    }
}
