using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Repositories.Interfaces;

namespace Famoser.Bookmarked.Business.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IApiRepository<Course, CollectionModel> _repository;

        public CourseRepository(IApiService apiService)
        {
            _repository = apiService.ResolveRepository<Course>();
        }

        private ObservableCollection<Course> _courses;
        public ObservableCollection<Course> GetCoursesLazy()
        {
            if (_courses == null)
            {
                _courses = _repository.GetAllLazy();
                foreach (var course in _courses)
                {
                    foreach (var courseLecture in course.Lectures)
                    {
                        courseLecture.Course = course;
                    }
                }
                _courses.CollectionChanged += CoursesOnCollectionChanged;
            }
            return _courses;
        }

        private void CoursesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
                    {
                        if (newItem is Course course)
                            if (course != null)
                                foreach (var courseLecture in course.Lectures)
                                {
                                    courseLecture.Course = course;
                                }
                    }
                    break;

            }
        }

        public Task<bool> SaveCourseAsync(Course course)
        {
            return _repository.SaveAsync(course);
        }

        public Task<bool> RemoveCourseAsync(Course course)
        {
            return _repository.RemoveAsync(course);
        }

        public Task<bool> SyncAsnyc()
        {
            return _repository.SyncAsync();
        }
    }
}
