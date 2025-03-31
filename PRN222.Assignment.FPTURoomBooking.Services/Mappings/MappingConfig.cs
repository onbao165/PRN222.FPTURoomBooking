using Mapster;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Account;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Slot;

namespace PRN222.Assignment.FPTURoomBooking.Services.Mappings;

public class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Account, AccountModel>.NewConfig()
            .Ignore(dest => dest.Password);
            
        TypeAdapterConfig<AccountModel, Account>.NewConfig()
            .Ignore(dest => dest.Department)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt);

        TypeAdapterConfig<BookingModel, Booking>.NewConfig()
            .Ignore(dest => dest.Account)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt);
        
        TypeAdapterConfig<DepartmentModel, Department>.NewConfig()
            .Ignore(dest => dest.Campus)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt);

        TypeAdapterConfig<RoomModel, Room>.NewConfig()
            .Ignore(dest => dest.Campus)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt);
        
        TypeAdapterConfig<SlotModel, Slot>.NewConfig()
            .Ignore(dest => dest.Room)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt);
        
    }
}
