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
        string Decrypt(string content, string password);
        string Encrypt(string json, string password);
    }
}
