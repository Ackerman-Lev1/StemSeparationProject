using DatabaseLayerLogic.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLayerLogic.Repositories
{
    public class FilesRepository: IFilesRepository
    {
        private readonly StemseperationContext _context;
        public FilesRepository(StemseperationContext context)
        {
            _context = context;
        }

        public async Task<List<UserFile>> GetUserFiles(string userName)
        {
            var userId = await _context.Users
                .Where(u => u.UserName == userName)
                .Select(u => u.Pkuser)
                .FirstOrDefaultAsync();

            return await _context.UserFiles
                .Where(file => file.UserId == userId)
                .ToListAsync();
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

        public async Task<string> GetFolderByUsername(string userName)
        {
            var userId = await _context.Users
                .Where(u => u.UserName == userName)
                .Select(u => u.Pkuser)
                .FirstOrDefaultAsync();

            var folderPath = await _context.UserFiles
                .Where(folderpath => folderpath.UserId == userId)
                .Select(folderpath => folderpath.InputPath)
                .FirstOrDefaultAsync();

            return folderPath ?? string.Empty;
        }
    }
}