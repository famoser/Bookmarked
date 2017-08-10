using Famoser.Bookmarked.Business.Repositories;
using Famoser.Bookmarked.Business.Repositories.FolderRepository;
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

            if (IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IFolderRepository, MockFolderRepository>();
            }
            else
            {
                SimpleIoc.Default.Register<IFolderRepository, FolderRepository>();
            }

            SimpleIoc.Default.Register<AddFolderViewModel>();
            SimpleIoc.Default.Register<EditFolderViewModel>();
            SimpleIoc.Default.Register<ViewFolderViewModel>();

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<NavigationViewModel>();
            SimpleIoc.Default.Register<GarbageViewModel>();
            SimpleIoc.Default.Register<FindViewModel>();
            SimpleIoc.Default.Register<ImportViewModel>();

            SimpleIoc.Default.Register<WebpageViewModel>();
            SimpleIoc.Default.Register<NoteViewModel>();
            SimpleIoc.Default.Register<CreditCardViewModel>();
            SimpleIoc.Default.Register<OnlineAccountViewModel>();
            SimpleIoc.Default.Register<ExportViewModel>();
            SimpleIoc.Default.Register<BookViewModel>();
            SimpleIoc.Default.Register<PersonViewModel>();
        }


        public AddFolderViewModel AddFolderViewModel => SimpleIoc.Default.GetInstance<AddFolderViewModel>();
        public EditFolderViewModel EditFolderViewModel => SimpleIoc.Default.GetInstance<EditFolderViewModel>();
        public ViewFolderViewModel ViewFolderViewModel => SimpleIoc.Default.GetInstance<ViewFolderViewModel>();

        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        public NavigationViewModel NavigationViewModel => SimpleIoc.Default.GetInstance<NavigationViewModel>();
        public GarbageViewModel GarbageViewModel => SimpleIoc.Default.GetInstance<GarbageViewModel>();
        public FindViewModel FindViewModel => SimpleIoc.Default.GetInstance<FindViewModel>();
        public ImportViewModel ImportViewModel => SimpleIoc.Default.GetInstance<ImportViewModel>();

        public ProgressViewModel ProgressViewModel => SimpleIoc.Default.GetInstance<ISimpleProgressService>() as ProgressViewModel;
        public ApiViewModel ApiViewModel => SimpleIoc.Default.GetInstance<IApiTraceService>() as ApiViewModel;

        public WebpageViewModel WebpageViewModel => SimpleIoc.Default.GetInstance<WebpageViewModel>();
        public NoteViewModel NoteViewModel => SimpleIoc.Default.GetInstance<NoteViewModel>();
        public CreditCardViewModel CreditCardViewModel => SimpleIoc.Default.GetInstance<CreditCardViewModel>();
        public OnlineAccountViewModel OnlineAccountViewModel => SimpleIoc.Default.GetInstance<OnlineAccountViewModel>();
        public BookViewModel BookViewModel => SimpleIoc.Default.GetInstance<BookViewModel>();
        public PersonViewModel PersonViewModel => SimpleIoc.Default.GetInstance<PersonViewModel>();
    }
}
