using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.Services.Interfaces;
using PCLCrypto;

namespace Famoser.Bookmarked.Business.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IStorageService _storageService;

        public PasswordService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        private string _password;
        public string GetPasswordAsync()
        {
            return _password;
        }

        public async Task<bool> SetPassword(string password)
        {
            try
            {
                var fileContent = await _storageService.GetCachedTextFileAsync(".pw");
                if (!string.IsNullOrEmpty(fileContent))
                {
                    //if file already exists can only change if unlocked
                    if (_password == null)
                        return false;
                }
            }
            catch
            {
                //probably file not found exception
            }

            try
            {
                return await _storageService.SetCachedTextFileAsync(".pw", password);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return false;
        }

        public async Task<bool> TryPassword(string password)
        {
            try
            {
                var fileContent = await _storageService.GetCachedTextFileAsync(".pw");
                if (!string.IsNullOrEmpty(fileContent))
                {
                    if (fileContent == password)
                    {
                        _password = password;
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                //probably file not found exception
            }

            //set new file with pw because old one could not be opened
            try
            {
                return await _storageService.SetCachedTextFileAsync(".pw", password);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return false;
        }
    }
}
