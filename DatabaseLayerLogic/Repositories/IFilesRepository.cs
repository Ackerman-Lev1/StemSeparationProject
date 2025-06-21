using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLayerLogic.Repositories
{
    public interface IFilesRepository
    {
        Task AddUserFiles(int noOfStems, string originalTrackFilePath, int userId,string fileName);
    }
}