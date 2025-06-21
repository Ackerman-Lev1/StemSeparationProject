using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayerLogic.Services.Contracts;
using DatabaseLayerLogic.Repositories;

namespace BusinessLayerLogic.Services
{
    public class UserFileService: IUserFileService
    {
        private  readonly IFilesRepository _filesRepository;
        public UserFileService(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }
        public async Task AddFilesAsync(int noOfStems, string originalTrackFilePath, int userId, string fileName)
        {
           await _filesRepository.AddUserFiles(noOfStems, originalTrackFilePath, userId, fileName); 
        }
    }
}