﻿using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Entry.Base;

namespace Famoser.Bookmarked.View.ViewModels.Entry
{
    public class CreditCardViewModel : WithUrlViewModel<CreditCardModel>
    {
        public CreditCardViewModel(IFolderRepository folderRepository, INavigationService navigationService, IApiService apiService) : base(folderRepository, navigationService, apiService)
        {
        }
    }
}
