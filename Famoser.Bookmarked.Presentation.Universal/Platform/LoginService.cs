using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.Presentation.Universal.Models;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Presentation.Universal.Platform
{
    public class LoginService : ILoginService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IInteractionService _interactionService;
        private readonly IStorageService _storageService;
        private const string AccountId = "Bookmarked";
        private const string FileName = ".pw_info";

        public LoginService(IEncryptionService encryptionService, IInteractionService interactionService, IStorageService storageService)
        {
            _encryptionService = encryptionService;
            _interactionService = interactionService;
            _storageService = storageService;
        }

        public string HashPassword(string password)
        {
            // put the string in a buffer
            IBuffer input = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);

            // hash it with SHA 256
            var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            IBuffer hashed = hasher.HashData(input);

            // return as base64
            return CryptographicBuffer.EncodeToBase64String(hashed);
        }

        private async Task<PasswordInfoModel> GetPasswordInfoModelAsync()
        {
            PasswordInfoModel model = null;
            var pwInfoFile = await _storageService.GetCachedTextFileAsync(FileName);
            if (pwInfoFile != null)
            {
                model = JsonConvert.DeserializeObject<PasswordInfoModel>(pwInfoFile);
            }

            if (model == null)
            {
                if (!await KeyCredentialManager.IsSupportedAsync())
                    return null;

                model = new PasswordInfoModel
                {
                    KeyPhrase = "sign this keyphrase of this bookmarked application to get a secure string"
                };
                var keyCreationResult =
                    await KeyCredentialManager.RequestCreateAsync(AccountId, KeyCredentialCreationOption.ReplaceExisting);

                if (keyCreationResult.Status == KeyCredentialStatus.Success)
                {
                    model.PreferPassword = false;
                }
                else if (keyCreationResult.Status == KeyCredentialStatus.UserPrefersPassword)
                {
                    model.PreferPassword = true;
                }
                await _storageService.SetCachedTextFileAsync(FileName, JsonConvert.SerializeObject(model));
            }
            return model;
        }


        public async void RegisterValidPassword(string hashedPassword)
        {
            PasswordInfoModel model = await GetPasswordInfoModelAsync();
            if (model != null)
            {
                if (model.EncryptedPassword == null)
                {
                    var key = await GetSignedKeyAsync(model.KeyPhrase);
                    if (key != null)
                    {
                        model.EncryptedPassword = _encryptionService.EncryptRaw(hashedPassword, key);
                        await _storageService.SetCachedTextFileAsync(FileName, JsonConvert.SerializeObject(model));
                    }
                }
            }
        }

        public async Task<string> TryAlternativeLogin()
        {
            try
            {
                PasswordInfoModel model = await GetPasswordInfoModelAsync();
                if (model != null)
                {
                    var key = await GetSignedKeyAsync(model.KeyPhrase);
                    if (key != null)
                    {
                        if (model.EncryptedPassword != null)
                        {
                            var myPassword = _encryptionService.DecryptRaw(model.EncryptedPassword, key);
                            return myPassword;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e);
            }
            return null;
        }

        public async void InvalidateAlternativeLogin()
        {
            PasswordInfoModel model = await GetPasswordInfoModelAsync();
            if (model != null)
            {
                model.EncryptedPassword = null;
                await _storageService.SetCachedTextFileAsync(FileName, JsonConvert.SerializeObject(model));
            }
        }

        private async Task<byte[]> GetSignedKeyAsync(string keyPhrase)
        {
            var openKeyResult = await KeyCredentialManager.OpenAsync(AccountId);

            if (openKeyResult.Status == KeyCredentialStatus.Success)
            {
                var userKey = openKeyResult.Credential;
                var buffer = CryptographicBuffer.ConvertStringToBinary(
                    keyPhrase, BinaryStringEncoding.Utf8
                );

                var signResult = await userKey.RequestSignAsync(buffer);

                if (signResult.Status == KeyCredentialStatus.Success)
                {
                    return signResult.Result.ToArray();
                }
            }
            return null;
        }
    }
}
