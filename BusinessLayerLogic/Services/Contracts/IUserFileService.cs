using DatabaseLayerLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayerLogic.Services.Contracts
{
    public interface IUserFileService
    {
        Task<List<UserFile>> GetUserFiles(string userName);
        Task<UserFile> AddFilesAsync(int noOfStems, string originalTrackFilePath, int userId, string fileName);
        Task<string> GetFolderByUsername(string userName);
    }
}