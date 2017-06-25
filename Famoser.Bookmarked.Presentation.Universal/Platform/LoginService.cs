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
            var pwInfoFile = await _storageService.GetCachedTextFileAsync(".pw_info");
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
                    AccountId = "Bookmarked",
                    KeyPhrase = "sign this keyphrase of this bookmarked application to get a secure string"
                };
                var keyCreationResult =
                    await KeyCredentialManager.RequestCreateAsync(model.AccountId, KeyCredentialCreationOption.ReplaceExisting);

                if (keyCreationResult.Status == KeyCredentialStatus.Success)
                {
                    var userKey = keyCreationResult.Credential;
                    model.KeyId = userKey.Name;
                    model.KeyPhrase += model.KeyId;
                    model.PreferPassword = false;
                }
                else if (keyCreationResult.Status == KeyCredentialStatus.UserPrefersPassword)
                {
                    model.PreferPassword = true;
                }
                await _storageService.SetCachedTextFileAsync(".pw_info", JsonConvert.SerializeObject(model));
            }

            if (model.PreferPassword)
                return null;

            if (model.AccountId == null)
                return null;

            return model;
        }


        public async void RegisterValidPassword(string hashedPassword)
        {
            if (_passwordInfoModel != null)
            {
                _passwordInfoModel.EncryptedPassword = _encryptionService.EncryptRaw(hashedPassword, _passwordInfoModel.EncryptionKey);
                await _storageService.SetCachedTextFileAsync(".pw_info", JsonConvert.SerializeObject(_passwordInfoModel));
            }
        }

        private PasswordInfoModel _passwordInfoModel;

        public async Task<string> TryAlternativeLogin()
        {
            try
            {
                PasswordInfoModel model = await GetPasswordInfoModelAsync();
                if (model != null)
                {
                    var openKeyResult = await KeyCredentialManager.OpenAsync(model.AccountId);

                    if (openKeyResult.Status == KeyCredentialStatus.Success)
                    {
                        var userKey = openKeyResult.Credential;
                        var buffer = CryptographicBuffer.ConvertStringToBinary(
                            model.KeyPhrase, BinaryStringEncoding.Utf8
                        );

                        var signResult = await userKey.RequestSignAsync(buffer);

                        if (signResult.Status == KeyCredentialStatus.Success)
                        {
                            var res = signResult.Result.ToArray();
                            if (model.EncryptedPassword != null)
                            {
                                var result = _encryptionService.DecryptRaw(model.EncryptedPassword, res);
                                return result;
                            }
                            model.EncryptionKey = res;
                            _passwordInfoModel = model;
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
    }
}
