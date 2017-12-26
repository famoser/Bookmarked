using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Famoser.FrameworkEssentials.Services.Interfaces;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
namespace Famoser.Bookmarked.Presentation.Webpage.Platform
{
    public class StorageService : IStorageService
    {
        private readonly Guid _folderGuid;
        public StorageService(Guid folderGuid)
        {
            _folderGuid = folderGuid;
        }

        /**
         * returns true if user is already initialized
         * else initialized the user & returns false
         */
        public bool EnsureUserInitialized()
        {
            var targetFolder = GetTargetFolder();
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
                return false;
            }

            return true;
        }

        private string GetTargetFolder()
        {
            return HttpContext.Current.Server.MapPath("~/private/cache/" + _folderGuid);
        }

        private bool SaveFile(string filePath, string content)
        {
            var targetPath = Path.Combine(GetTargetFolder(), filePath);
            File.WriteAllText(targetPath, content);
            return true;
        }

        private string GetFile(string filePath)
        {
            var targetPath = Path.Combine(GetTargetFolder(), filePath);
            return File.ReadAllText(targetPath);
        }

        private bool SaveFile(string filePath, byte[] content)
        {
            var targetPath = Path.Combine(GetTargetFolder(), filePath);
            File.WriteAllBytes(targetPath, content);
            return true;
        }

        private byte[] GetFileBytes(string filePath)
        {
            var targetPath = Path.Combine(GetTargetFolder(), filePath);
            return File.ReadAllBytes(targetPath);
        }

        private bool RemoveFile(string filePath)
        {
            var targetPath = Path.Combine(GetTargetFolder(), filePath);
            if (File.Exists(filePath))
                File.Delete(targetPath);
            return true;
        }

        public async Task<string> GetCachedTextFileAsync(string filePath)
        {
            return GetFile(filePath);
        }

        public async Task<bool> SetCachedTextFileAsync(string filePath, string content)
        {
            return SaveFile(filePath, content);
        }

        public async Task<byte[]> GetCachedFileAsync(string filePath)
        {
            return GetFileBytes(filePath);
        }

        public async Task<bool> SetCachedFileAsync(string filePath, byte[] content)
        {
            return SaveFile(filePath, content);
        }

        public async Task<bool> DeleteCachedFileAsync(string filePath)
        {
            return RemoveFile(filePath);
        }

        public async Task<string> GetRoamingTextFileAsync(string filePath)
        {
            return GetFile("_roaming" + filePath);
        }

        public async Task<bool> SetRoamingTextFileAsync(string filePath, string content)
        {
            return SaveFile("_roaming" + filePath, content);
        }

        public async Task<byte[]> GetRoamingFileAsync(string filePath)
        {
            return GetFileBytes(filePath);
        }

        public async Task<bool> SetRoamingFileAsync(string filePath, byte[] content)
        {
            return SaveFile(filePath, content);
        }

        public async Task<bool> DeleteRoamingFileAsync(string filePath)
        {
            return RemoveFile("_roaming" + filePath);
        }

        public async Task<string> GetAssetTextFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetAssetFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}