using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLayerLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLayerLogic.Repositories
{
    public class FilesRepository: IFilesRepository
    {
        private readonly StemseperationContext _context;
        public FilesRepository(StemseperationContext context)
        {
            _context = context;
        }
        public async Task AddUserFiles(int noOfStems, string originalTrackFilePath, int userId,string fileName)
        {
            UserFile userFile; 
            switch (noOfStems)
            {
                case 2:
                    userFile=new UserFile()
                    {
                        UserId = userId,
                        InputPath = originalTrackFilePath,
                        InstanceTime = DateTime.Now,
                        Stem1 = Path.Combine(originalTrackFilePath,fileName,"vocals.wav"),
                        Stem2 = Path.Combine(originalTrackFilePath,fileName,"accompaniment.wav"),
                        Stem3 = null,
                        Stem4=null,
                        Stem5=null
                    }; 
                    break;
                case 4:
                    userFile=new UserFile()
                    {
                        UserId = userId,
                        InputPath = originalTrackFilePath,
                        InstanceTime = DateTime.Now,
                        Stem1 = Path.Combine(originalTrackFilePath,fileName,"vocals.wav"),
                        Stem2 = Path.Combine(originalTrackFilePath,fileName,"drums.wav"),
                        Stem3 = Path.Combine(originalTrackFilePath,fileName,"bass.wav"),
                        Stem4 =  Path.Combine(originalTrackFilePath,fileName,"other.wav"),
                        Stem5=null
                    }; 
                    break;
                case 5:
                    userFile=new UserFile()
                    {
                        UserId = userId,
                        InputPath = originalTrackFilePath,
                        InstanceTime = DateTime.Now,
                        Stem1 = Path.Combine(originalTrackFilePath,fileName,"vocals.wav"),
                        Stem2 = Path.Combine(originalTrackFilePath,fileName,"drums.wav"),
                        Stem3 = Path.Combine(originalTrackFilePath,fileName,"bass.wav"),
                        Stem4 = Path.Combine(originalTrackFilePath,fileName,"other.wav"),
                        Stem5 = Path.Combine(originalTrackFilePath,fileName,"piano.wav")
                    };
                    break;
                default:
                    throw new ArgumentException("Invalid number of stems specified.");
            }
            await _context.UserFiles.AddAsync(
                userFile
            );
            await _context.SaveChangesAsync();
        }
    }
}