using Mapster;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Account;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;
using PRN222.Assignment.FPTURoomBooking.Services.Models.RoomSlot;

namespace PRN222.Assignment.FPTURoomBooking.Services.Mappings;

public class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Account, AccountModel>.NewConfig()
            .Ignore(dest => dest.Password); // Ignore password when mapping from Account to AccountModel
        TypeAdapterConfig<AccountModel, Account>.NewConfig()
            .Ignore(dest => dest.Department);

        TypeAdapterConfig<BookingModel, Booking>.NewConfig()
            .Ignore(dest => dest.Account);

        TypeAdapterConfig<DepartmentModel, Department>.NewConfig()
            .Ignore(dest => dest.Campus);

        TypeAdapterConfig<RoomModel, Room>.NewConfig()
            .Ignore(dest => dest.Department);

        TypeAdapterConfig<RoomSlotModel, RoomSlot>.NewConfig()
            .Ignore(dest => dest.Room);
    }
}