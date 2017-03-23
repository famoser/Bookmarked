using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;
using Famoser.FrameworkEssentials.View.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class LectureViewModel : BaseViewModel, INavigationBackNotifier
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IHistoryNavigationService _navigationService;

        public LectureViewModel(IFolderRepository folderRepository, IHistoryNavigationService navigationService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
            Messenger.Default.Register<Lecture>(this, Messages.Select, SelectLecture);
            if (IsInDesignModeStatic)
            {
                Lecture = folderRepository.GetRootFolder().FirstOrDefault().Lectures.FirstOrDefault();
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
            _folderRepository.SaveCourseAsync(vm.Course);
            _navigationService.GoBack();
        });

        public void HandleNavigationBack(object message)
        {
            if (message is Lecture back)
            {
                if (back != null)
                {
                    Lecture = back;
                }
            }
        }
    }
}
