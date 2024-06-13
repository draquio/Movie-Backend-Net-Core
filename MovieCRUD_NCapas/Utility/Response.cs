namespace MovieCRUD_NCapas.Utility
{
    public class Response<T>
    {
        public bool status {  get; set; }
        public T? value { get; set; }
        public string? msg { get; set; }
        public List<string>? errors { get; set; }
    }
    public class PagedResponse<T>
    {
        public bool status { get; set; }
        public T? value { get; set; }
        public string? msg { get; set; }
        public List<string>? errors { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
