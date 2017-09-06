using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.Services.Interfaces;

namespace Famoser.Bookmarked.Business.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IStorageService _storageService;
        private readonly IRandomNumberService _randomNumberService;

        public PasswordService(IStorageService storageService, IRandomNumberService randomNumberService)
        {
            _storageService = storageService;
            _randomNumberService = randomNumberService;
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
                _password = password;
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

        #region random number generator

        private static readonly char[] SmallAlpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static readonly char[] BigAlpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] Digits = "0123456789".ToCharArray();
        private static readonly char[] Special = "`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".ToCharArray();

        public string GeneratePassword(PasswordType type, int length)
        {
            if (type == PasswordType.ToCopy || type == PasswordType.ToType)
            {
                IEnumerable<char> available;
                if (type == PasswordType.ToCopy)
                {
                    available = SmallAlpha.Concat(BigAlpha).Concat(Digits).Concat(Special);
                }
                else
                {
                    //PasswordType.ToType
                    available = SmallAlpha.Concat(BigAlpha).Concat(Digits);
                }
                return GenerateRandomPassword(available.ToArray(), length);
            }
            return GenerateRememberPassword(length);
        }

        private string GenerateRandomPassword(char[] chars, int length)
        {
            var res = "";
            for (int i = 0; i < length; i++)
            {
                res += GetRandomChar(chars);
            }
            return res;
        }

        /// <summary>
        /// of the following form:
        /// kud-fi-poy-le-zus
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GenerateRememberPassword(int length)
        {
            var vocals = "aeiou".ToCharArray();
            //skip because ambiguous: ck, jyi
            var normals = "bdfghklmnpqrstvwxz".ToCharArray();

            var res = "";
            do
            {
                if (res != "")
                {
                    res += "-";
                }
                res += GetRandomChar(normals);
                res += GetRandomChar(vocals);
                res += GetRandomChar(normals);
                if (res.Length >= length)
                {
                    break;
                }
                res += "-";

                res += GetRandomChar(normals);
                res += GetRandomChar(vocals);
            } while (res.Length < length);

            return res;
        }

        private char GetRandomChar(char[] chars)
        {
            var place = _randomNumberService.GenerateRandomNumber(0, Convert.ToUInt32(chars.Length - 1));
            return chars[place];
        }
        #endregion
    }
}
