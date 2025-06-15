namespace SakennyProject.Helper
{
    public class Pagination<T>

    {
        public Pagination(int pageSize, int pageIndex, int count, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Data = data;
            Count = count;
        }
        public int PageSize { get; }
        public int PageIndex { get; }
        public int Count { get; }
        public IReadOnlyList<T> Data { get; }
    }
}
