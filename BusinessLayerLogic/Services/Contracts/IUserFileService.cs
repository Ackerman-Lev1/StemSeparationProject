using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayerLogic.Services.Contracts
{
    public interface IUserFileService
    {
        Task AddFilesAsync(int noOfStems, string originalTrackFilePath, int userId, string fileName);
    }
}