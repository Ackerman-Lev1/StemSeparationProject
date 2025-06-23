using DatabaseLayerLogic.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLayerLogic.Repositories
{
    public interface IFilesRepository
    {
        Task<List<UserFile>> GetUserFiles(string userName);
        Task AddUserFiles(int noOfStems, string originalTrackFilePath, int userId,string fileName);
        Task<string> GetFolderByUsername(string userName);
    }
}