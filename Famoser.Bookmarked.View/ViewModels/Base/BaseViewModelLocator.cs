using Famoser.Bookmarked.Business.Repositories;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Mocks;
using Famoser.Bookmarked.View.Services;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.SyncApi.Services.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.View.ViewModels.Base
{
    public class BaseViewModelLocator : BaseViewModel
    {
        static BaseViewModelLocator()
        {
            SimpleIoc.Default.Register<IApiService, ApiService>();
            SimpleIoc.Default.Register<ISimpleProgressService, ProgressViewModel>();
            if (IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IFolderRepository, MockFolderRepository>();
                SimpleIoc.Default.Register<IApiTraceService, ApiViewModel>();
            }
            else
            {
                SimpleIoc.Default.Register<IFolderRepository, FolderRepository>();
                SimpleIoc.Default.Register<IApiTraceService, ApiViewModel>();
            }

            SimpleIoc.Default.Register<FolderViewModel>();
        }
       

        public FolderViewModel MainViewModel => SimpleIoc.Default.GetInstance<FolderViewModel>();
        public ProgressViewModel ProgressViewModel => SimpleIoc.Default.GetInstance<ISimpleProgressService>() as ProgressViewModel;
        public ApiViewModel ApiViewModel => SimpleIoc.Default.GetInstance<IApiTraceService>() as ApiViewModel;
    }
}
