using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Account;

public class GetAccountModel : PaginationParams
{
    public Guid? DepartmentId { get; set; }
}