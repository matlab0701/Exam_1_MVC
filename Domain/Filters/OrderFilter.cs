namespace Domain.Filters;

public class OrderFilter
{
    public DateTime? OrderDate { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }

}
