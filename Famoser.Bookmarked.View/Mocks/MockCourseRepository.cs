using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.LectureSync.Business.Models;
using Famoser.LectureSync.Business.Repositories.Interfaces;

namespace Famoser.LectureSync.View.Mocks
{
    internal class MockCourseRepository : ICourseRepository
    {
        public ObservableCollection<Course> GetCoursesLazy()
        {
            var coll = new ObservableCollection<Course>()
            {
                new Course()
                {
                    Name = "Software Architecture and Engineering",
                    InfoUrl = new Uri("http://www.vvz.ethz.ch/Vorlesungsverzeichnis/lerneinheitPre.do?lerneinheitId=111927&semkez=2017S&lang=de"),
                    Lecturer = "P.Müller, M.Vechev",
                    Place = "CAB G 61",
                    Lectures = new ObservableCollection<Lecture>()
                    {
                        new Lecture()
                        {
                            DayOfWeek = DayOfWeek.Monday,
                            Lecturer = "P.Müller, M.Vechev",
                            Place = "CAB G 61",
                            StartTime = TimeSpan.FromHours(10),
                            EndTime = TimeSpan.FromHours(11)
                        },
                        new Lecture()
                        {
                            DayOfWeek = DayOfWeek.Tuesday,
                            Lecturer = "P.Müller, M.Vechev",
                            Place = "CAB G 63",
                            StartTime = TimeSpan.FromHours(14),
                            EndTime = TimeSpan.FromHours(15)
                        }
                    }
                }
            };

            foreach (var course in coll)
                foreach (var courseLecture in course.Lectures)
                    courseLecture.Course = course;

            return coll;
        }

#pragma warning disable 1998
        public async Task<bool> SaveCourseAsync(Course course)
        {
            return true;
        }

        public async Task<bool> RemoveCourseAsync(Course course)
        {
            return true;
        }

        public async Task<bool> SyncAsnyc()
        {
            return true;
        }
    }
}
