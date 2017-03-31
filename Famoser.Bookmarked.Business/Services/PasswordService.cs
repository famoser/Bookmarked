using System;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.Services.Interfaces;

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
        public string GetPassword()
        {
            return _password;
        }

        public async Task<bool> SetPasswordAsync(string password)
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

        public async Task<bool> TryPasswordAsync(string password)
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

        public async Task<bool> CheckIsFirstTimeAsync()
        {
            try
            {
                var fileContent = await _storageService.GetCachedTextFileAsync(".pw");
                if (!string.IsNullOrEmpty(fileContent))
                {
                    return false;
                }
            }
            catch
            {
                // ignored: file does not exist or is corrupt os fuck it
            }
            return true;
        }
    }
}
