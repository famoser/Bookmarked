﻿using System;
using System.ComponentModel;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Helper;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Entry.Abstract;
using Famoser.Bookmarked.View.ViewModels.Entry.Base;
using Famoser.FrameworkEssentials.Services;
using Famoser.FrameworkEssentials.Services.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.View.ViewModels.Entry
{
    public class WebpageViewModel : WithUrlViewModel<WebpageModel>
    {
        public WebpageViewModel(IFolderRepository folderRepository, INavigationService navigationService, IApiService apiService) : base(folderRepository, navigationService, apiService)
        {
        }
    }
}
