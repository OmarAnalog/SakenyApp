namespace Sakenny.Core.Specification
{
    public class PostSpecParams
    {
        public string? UserId { get; set; }
        public int PageIndex { get; set; } = 1;
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
    }
}
