namespace Domain.Responses;

public class ValidFilter(int PageNumber,int PageSize)
{
    public int PageNumber { get; set; } = PageNumber;
    public int PageSize { get; set; } = PageSize;
}
