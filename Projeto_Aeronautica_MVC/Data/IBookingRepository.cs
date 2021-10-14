using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Data
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IQueryable<Booking>> GetBookingAsync(string userName);

        Task<IQueryable<BookingHistory>> GetBookingHistoryAsync(string userName);

        Task<IQueryable<BookingDetailTemp>> GetDetailTempsAsync(string userName);

        Task AddTicketToBookingAsync(AddTicketViewModel model, string userName);

        Task ModifyBookingDetailTempQuantityAsync(int id, double quantity);

        Task DeleteDetailTempAsync(int id);

        Task<bool> ConfirmBookingAsync(string userName);

        Task DepartureBooking(DepartureViewModel model);

        Task<Booking> GetBookingAsync(int id);

        Task<BookingHistory> GetBookingHistoryAsync(int id);
    }
}
