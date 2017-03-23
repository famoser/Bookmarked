﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;

namespace Famoser.Bookmarked.View.Mocks
{
    internal class MockFolderRepository : IFolderRepository
    {
        public ObservableCollection<Folder> GetRootFolder()
        {
            var coll = new ObservableCollection<Folder>()
            {
                new Folder()
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
        public async Task<bool> SaveCourseAsync(Folder course)
        {
            return true;
        }

        public async Task<bool> RemoveCourseAsync(Folder course)
        {
            return true;
        }

        public async Task<bool> SyncAsnyc()
        {
            return true;
        }
    }
}