using System;
using System.Linq;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;
using Famoser.FrameworkEssentials.View.Interfaces;
using GalaSoft.MvvmLight.Messaging;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class CourseViewModel : BaseViewModel, INavigationBackNotifier
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IHistoryNavigationService _navigationService;
        private readonly IInteractionService _interactionService;
        private readonly IWeekDayService _weekDayService;

        public CourseViewModel(ICourseRepository courseRepository, IHistoryNavigationService navigationService, IInteractionService interactionService, IWeekDayService weekDayService)
        {
            _courseRepository = courseRepository;
            _navigationService = navigationService;
            _interactionService = interactionService;
            _weekDayService = weekDayService;
            Messenger.Default.Register<Course>(this, Messages.Select, SelectCourse);
            if (IsInDesignModeStatic)
            {
                Course = courseRepository.GetCoursesLazy().FirstOrDefault();
            }
        }

        private Course _course;
        public Course Course
        {
            get { return _course; }
            set { Set(ref _course, value); }
        }

        private void SelectCourse(Course obj)
        {
            Course = obj;
        }

        public ICommand SaveCourseCommand => new LoadingRelayCommand(async () =>
        {
            await _courseRepository.SaveCourseAsync(Course);
            _navigationService.GoBack();
        });

        public ICommand EditCourseCommand => new LoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(Pages.AddEditCourse.ToString());
        });

        public ICommand DeleteCourseCommand => new LoadingRelayCommand(async () =>
        {
            if (await _interactionService.ConfirmMessage("do you really want to delete this course?"))
            {
                await _courseRepository.RemoveCourseAsync(Course);
                _navigationService.GoBack();
            }
        });

        public ICommand AddLectureCommand => new LoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(Pages.AddEditLecture.ToString());
            var lecture = new Lecture
            {
                Lecturer = Course.Lecturer,
                Place = Course.Place,
                Course = Course,
                DayOfWeek = DateTime.Now.DayOfWeek
            };
            Messenger.Default.Send(lecture, Messages.Select);
        });

        public ICommand EditLectureCommand => new LoadingRelayCommand<Lecture>((l) =>
        {
            _navigationService.NavigateTo(Pages.AddEditLecture.ToString());
            Messenger.Default.Send(l, Messages.Select);
        });

        public ICommand DeleteLectureCommand => new LoadingRelayCommand<Lecture>(async l =>
        {
            if (await _interactionService.ConfirmMessage("do you really want to delete this lecture?"))
            {
                if (Course.Lectures.Contains(l))
                {
                    Course.Lectures.Remove(l);
                    await _courseRepository.SaveCourseAsync(Course);
                }
            }
        });

        public void HandleNavigationBack(object message)
        {
            if (message is Course back)
            {
                if (back != null)
                {
                    Course = back;
                }
            }
        }
    }
}
