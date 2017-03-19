using Famoser.Bookmarked.Business.Repositories;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Mocks;
using Famoser.Bookmarked.View.Services;
using Famoser.Bookmarked.View.Services.Interfaces;

namespace Famoser.Bookmarked.View.ViewModels.Base
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
