namespace APIBaseTemplate.Common
{
    public interface IPaginated
    {
        int? PageIndex { get; set; }
        int PageSize { get; set; }
    }
}
