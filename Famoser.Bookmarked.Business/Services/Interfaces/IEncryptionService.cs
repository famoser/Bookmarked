using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.SyncApi.Properties;

namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IEncryptionService
    {
        string GetJsonAsync(string content, string password);
        string GetContentAsync(string json, string password);
    }
}
