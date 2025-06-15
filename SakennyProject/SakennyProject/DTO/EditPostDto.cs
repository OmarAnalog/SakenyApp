namespace SakennyProject.DTO
{
    public class EditPostDto
    {
        public int PostId { get; set; }
        public int UnitId { get; set; }
        public bool? IsRented { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
