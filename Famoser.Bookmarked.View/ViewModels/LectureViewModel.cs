using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;
using Famoser.FrameworkEssentials.View.Interfaces;
using Famoser.LectureSync.Business.Models;
using Famoser.LectureSync.Business.Repositories.Interfaces;
using Famoser.LectureSync.View.Enum;
using Famoser.LectureSync.View.ViewModels.Base;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace Famoser.LectureSync.View.ViewModels
{
    public class LectureViewModel : BaseViewModel, INavigationBackNotifier
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IHistoryNavigationService _navigationService;

        public LectureViewModel(ICourseRepository courseRepository, IHistoryNavigationService navigationService)
        {
            _courseRepository = courseRepository;
            _navigationService = navigationService;
            Messenger.Default.Register<Lecture>(this, Messages.Select, SelectLecture);
            if (IsInDesignModeStatic)
            {
                Lecture = courseRepository.GetCoursesLazy().FirstOrDefault().Lectures.FirstOrDefault();
            }
        }

        private Lecture _lecture;
        public Lecture Lecture
        {
            get { return _lecture; }
            set { Set(ref _lecture, value); }
        }

        public ObservableCollection<DayOfWeek> DayOfWeeks { get; } = new ObservableCollection<DayOfWeek>()
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };

        private void SelectLecture(Lecture obj)
        {
            Lecture = obj;
        }
        public ICommand SaveLectureCommand => new LoadingRelayCommand(() =>
        {
            var vm = SimpleIoc.Default.GetInstance<CourseViewModel>();
            if (!vm.Course.Lectures.Contains(Lecture))
                vm.Course.Lectures.Add(Lecture);
            _courseRepository.SaveCourseAsync(vm.Course);
            _navigationService.GoBack();
        });

        public void HandleNavigationBack(object message)
        {
            var back = message as Lecture;
            if (back != null)
            {
                Lecture = back;
            }
        }
    }
}
