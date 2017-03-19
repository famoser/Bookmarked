using Famoser.LectureSync.Business.Repositories;
using Famoser.LectureSync.Business.Repositories.Interfaces;
using Famoser.LectureSync.Business.Services;
using Famoser.LectureSync.Business.Services.Interfaces;
using Famoser.LectureSync.View.Mocks;
using Famoser.LectureSync.View.Services;
using Famoser.LectureSync.View.Services.Interfaces;
using Famoser.SyncApi.Services.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.LectureSync.View.ViewModels.Base
{
    public class BaseViewModelLocator : BaseViewModel
    {
        static BaseViewModelLocator()
        {
            SimpleIoc.Default.Register<IApiService, ApiService>();
            SimpleIoc.Default.Register<ISimpleProgressService, ProgressViewModel>();
            SimpleIoc.Default.Register<IWeekDayService, WeekDayService>();
            if (IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<ICourseRepository, MockCourseRepository>();
                SimpleIoc.Default.Register<IApiTraceService, ApiViewModel>();
            }
            else
            {
                SimpleIoc.Default.Register<ICourseRepository, CourseRepository>();
                SimpleIoc.Default.Register<IApiTraceService, ApiViewModel>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CourseViewModel>();
            SimpleIoc.Default.Register<LectureViewModel>();
        }
       

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
        public CourseViewModel CourseViewModel => SimpleIoc.Default.GetInstance<CourseViewModel>();
        public LectureViewModel LectureViewModel => SimpleIoc.Default.GetInstance<LectureViewModel>();
        public ProgressViewModel ProgressViewModel => SimpleIoc.Default.GetInstance<ISimpleProgressService>() as ProgressViewModel;
        public ApiViewModel ApiViewModel => SimpleIoc.Default.GetInstance<IApiTraceService>() as ApiViewModel;
    }
}
