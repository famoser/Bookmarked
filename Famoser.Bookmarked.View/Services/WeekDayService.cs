using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Famoser.LectureSync.Business.Models;
using Famoser.LectureSync.Business.Repositories.Interfaces;
using Famoser.LectureSync.View.Models;
using Famoser.LectureSync.View.Services.Interfaces;

namespace Famoser.LectureSync.View.Services
{
    public class WeekDayService : IWeekDayService
    {
        private readonly ObservableCollection<Course> _courses;
        private readonly ObservableCollection<WeekDay> _weekDays = new ObservableCollection<WeekDay>();
        private readonly Dictionary<DayOfWeek, WeekDay> _weekDayDictionary = new Dictionary<DayOfWeek, WeekDay>();

        public WeekDayService(ICourseRepository repository)
        {
            _courses = repository.GetCoursesLazy();
            var today = new WeekDay("Today " + DateTime.Now.DayOfWeek, DateTime.Now.DayOfWeek);
            _weekDays.Add(today);
            _weekDayDictionary[DateTime.Now.DayOfWeek] = today;

            foreach (var course in _courses)
                AddCourse(course);

            _courses.CollectionChanged += CoursesOnCollectionChanged;
        }

        private void CoursesOnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
                        AddCourse(newItem as Course);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems)
                        RemoveCourse(oldItem as Course);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems)
                        RemoveCourse(oldItem as Course);
                    foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
                        AddCourse(newItem as Course);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ResetService();
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        private void LecturesOnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
                        RemoveLecture(newItem as Lecture);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems)
                        RemoveLecture(oldItem as Lecture);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems)
                        RemoveLecture(oldItem as Lecture);
                    foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
                        AddLecture(newItem as Lecture);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ResetService();
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        private void ResetService()
        {
            foreach (var value in _weekDayDictionary.Values)
                value.Lectures.Clear();
            foreach (var weekDay in _weekDays)
                weekDay.Lectures.Clear();
            foreach (var course in _courses)
                AddCourse(course);
        }

        private void AddCourse(Course course)
        {
            course.Lectures.CollectionChanged += LecturesOnCollectionChanged;
            foreach (var courseLecture in course.Lectures)
            {
                AddLecture(courseLecture);
            }
        }

        private void AddLecture(Lecture lecture)
        {
            if (!_weekDayDictionary.ContainsKey(lecture.DayOfWeek))
            {
                var weekDay = new WeekDay(lecture.DayOfWeek.ToString(), lecture.DayOfWeek);
                _weekDayDictionary[lecture.DayOfWeek] = weekDay;
                var found = false;
                for (int i = 0; i < _weekDays.Count && !found; i++)
                {
                    if (_weekDays[i].DayOfWeek > weekDay.DayOfWeek)
                    {
                        _weekDays.Insert(i, weekDay);
                        found = true;
                    }
                }
                if (!found)
                    _weekDays.Add(weekDay);
            }

            _weekDayDictionary[lecture.DayOfWeek].AddLecture(lecture);
        }

        private void RemoveCourse(Course course)
        {
            course.Lectures.CollectionChanged -= LecturesOnCollectionChanged;
            foreach (var courseLecture in course.Lectures)
            {
                RemoveLecture(courseLecture);
            }
        }

        private void RemoveLecture(Lecture lecture)
        {
            if (_weekDayDictionary.ContainsKey(lecture.DayOfWeek))
            {
                _weekDayDictionary[lecture.DayOfWeek].RemoveLecture(lecture);
                if (_weekDayDictionary[lecture.DayOfWeek].Lectures.Count == 0)
                {
                    _weekDays.Remove(_weekDayDictionary[lecture.DayOfWeek]);
                    _weekDayDictionary.Remove(lecture.DayOfWeek);
                }
            }
        }

        public ObservableCollection<WeekDay> GetWeekDays()
        {
            return _weekDays;
        }

        public WeekDay GetToday()
        {
            return _weekDayDictionary[DateTime.Now.DayOfWeek];
        }
    }
}