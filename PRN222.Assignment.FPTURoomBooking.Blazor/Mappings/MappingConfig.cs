using Mapster;
using PRN222.Assignment.FPTURoomBooking.Blazor.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Slot;

namespace PRN222.Assignment.FPTURoomBooking.Blazor.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        // Map BookingModel to BookingDetailsViewModel
        TypeAdapterConfig<BookingModel, BookingDetailsViewModel>.NewConfig()
            .Map(dest => dest.AccountName, src => src.Account.FullName);
        // Map RoomSlotModel to BookingRoomSlotViewModel
        TypeAdapterConfig<SlotModel, BookingSlotViewModel>.NewConfig()
            .Map(dest => dest.RoomName, src => src.Room.Name);
    }
}