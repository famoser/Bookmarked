﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface IImportExportService
    {
        Task<string> ImportExportFileAsync();

        Task<string> ImportCredentialsFileAsync();

        Task<bool> SaveExportFileAsync(string content);

        Task<bool> SaveCredentialsFileAsync(string content);
    }
}