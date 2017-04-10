using Famoser.Bookmarked.Business.Repositories;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Repositories.Mocks;
using Famoser.Bookmarked.Business.Services;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Services;
using Famoser.Bookmarked.View.ViewModels.Entry;
using Famoser.Bookmarked.View.ViewModels.Folder;
using Famoser.SyncApi.Services.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.View.ViewModels.Base
{
    public class BaseViewModelLocator : BaseViewModel
    {
        static BaseViewModelLocator()
        {
            SimpleIoc.Default.Register<IApiService, ApiService>();
            SimpleIoc.Default.Register<IApiTraceService, ApiViewModel>();
            SimpleIoc.Default.Register<IEncryptionService, EncryptionService>();
            SimpleIoc.Default.Register<IPasswordService, PasswordService>();
            SimpleIoc.Default.Register<ISimpleProgressService, ProgressViewModel>();
            SimpleIoc.Default.Register<IViewService, ViewService>();

            if (IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IFolderRepository, MockFolderRepository>();
            }
            else
            {
                SimpleIoc.Default.Register<IFolderRepository, FolderRepository>();
            }

            SimpleIoc.Default.Register<NavigationViewModel>();
            SimpleIoc.Default.Register<AddFolderViewModel>();
            SimpleIoc.Default.Register<EditFolderViewModel>();
            SimpleIoc.Default.Register<ViewFolderViewModel>();
            SimpleIoc.Default.Register<WebpageViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<GarbageViewModel>();
            SimpleIoc.Default.Register<FindViewModel>();
        }


        public AddFolderViewModel AddFolderViewModel => SimpleIoc.Default.GetInstance<AddFolderViewModel>();
        public NavigationViewModel NavigationViewModel => SimpleIoc.Default.GetInstance<NavigationViewModel>();
        public GarbageViewModel GarbageViewModel => SimpleIoc.Default.GetInstance<GarbageViewModel>();
        public FindViewModel FindViewModel => SimpleIoc.Default.GetInstance<FindViewModel>();
        public EditFolderViewModel EditFolderViewModel => SimpleIoc.Default.GetInstance<EditFolderViewModel>();
        public ViewFolderViewModel ViewFolderViewModel => SimpleIoc.Default.GetInstance<ViewFolderViewModel>();
        public WebpageViewModel WebpageViewModel => SimpleIoc.Default.GetInstance<WebpageViewModel>();
        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        public ProgressViewModel ProgressViewModel => SimpleIoc.Default.GetInstance<ISimpleProgressService>() as ProgressViewModel;
        public ApiViewModel ApiViewModel => SimpleIoc.Default.GetInstance<IApiTraceService>() as ApiViewModel;
    }
}
