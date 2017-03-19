using System.Collections.ObjectModel;
using Famoser.Bookmarked.View.Models;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface IWeekDayService
    {
        ObservableCollection<WeekDay> GetWeekDays();
        WeekDay GetToday();
    }
}
