﻿using Famoser.Bookmarked.Business.Repositories;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Mocks;
using Famoser.Bookmarked.View.Services;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Entry;
using Famoser.Bookmarked.View.ViewModels.Folder;
using Famoser.Bookmarked.View.ViewModels.Folder.Base;
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

            SimpleIoc.Default.Register<AddFolderViewModel>();
            SimpleIoc.Default.Register<EditFolderViewModel>();
            SimpleIoc.Default.Register<ViewFolderViewModel>();
            SimpleIoc.Default.Register<WebpageViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
        }


        public AddFolderViewModel AddFolderViewModel => SimpleIoc.Default.GetInstance<AddFolderViewModel>();
        public EditFolderViewModel EditFolderViewModel => SimpleIoc.Default.GetInstance<EditFolderViewModel>();
        public ViewFolderViewModel ViewFolderViewModel => SimpleIoc.Default.GetInstance<ViewFolderViewModel>();
        public WebpageViewModel WebpageViewModel => SimpleIoc.Default.GetInstance<WebpageViewModel>();
        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        public ProgressViewModel ProgressViewModel => SimpleIoc.Default.GetInstance<ISimpleProgressService>() as ProgressViewModel;
        public ApiViewModel ApiViewModel => SimpleIoc.Default.GetInstance<IApiTraceService>() as ApiViewModel;
    }
}
