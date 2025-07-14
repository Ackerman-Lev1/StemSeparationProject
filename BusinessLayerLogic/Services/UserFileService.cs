using BusinessLayerLogic.Services.Contracts;
using DatabaseLayerLogic.Models;
using DatabaseLayerLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayerLogic.Services
{
    public class UserFileService : IUserFileService
    {
        private readonly IFilesRepository _filesRepository;
        public UserFileService(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }
        public async Task<List<UserFile>> GetUserFiles(string userName) => 
            await _filesRepository.GetUserFiles(userName);
        public async Task<UserFile> AddFilesAsync(int noOfStems, string originalTrackFilePath, int userId, string fileName)
        {
            UserFile userFile = await _filesRepository.AddUserFiles(noOfStems, originalTrackFilePath, userId, fileName); 
            return userFile; 
        }

        public async Task<string> GetFolderByUsername(string userName) =>
            await _filesRepository.GetFolderByUsername(userName);
    }
}