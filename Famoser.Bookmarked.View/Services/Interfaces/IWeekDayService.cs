using System.Collections.ObjectModel;
using Famoser.LectureSync.View.Models;

namespace Famoser.LectureSync.View.Services.Interfaces
{
    public interface IWeekDayService
    {
        ObservableCollection<WeekDay> GetWeekDays();
        WeekDay GetToday();
    }
}
