namespace PRN222.Assignment.FPTURoomBooking.Services.Utils;

public class PaginationParams
{
    private string _searchTerm = "";
    private string _orderBy = "";
    private const int MaxPageSize = 1000;
    private int _pageSize = 5;
    private int _pageNumber = 1;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }
    
    // If the value is null or empty, set it to an empty string
    // Else normalize the value to lowercase
    public string SearchTerm
    {
        get => _searchTerm;
        set => _searchTerm = string.IsNullOrEmpty(value) ? string.Empty : value.ToLower();
    }
    
    public string OrderBy
    {
        get => _orderBy;
        set => _orderBy = string.IsNullOrEmpty(value) ? string.Empty : value.ToLower();
        
    }
    
    public bool IsDescending { get; set; }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
    }
} 