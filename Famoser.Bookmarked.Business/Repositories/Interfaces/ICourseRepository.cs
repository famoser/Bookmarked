using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.LectureSync.Business.Models;

namespace Famoser.LectureSync.Business.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        ObservableCollection<Course> GetCoursesLazy();
        Task<bool> SaveCourseAsync(Course course);
        Task<bool> RemoveCourseAsync(Course course);
        Task<bool> SyncAsnyc();
    }
}
