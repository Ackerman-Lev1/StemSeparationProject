using BusinessLayerLogic.ExternalProcesses;
using BusinessLayerLogic.Services;
using BusinessLayerLogic.Services.Contracts;
using DatabaseLayerLogic.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PresentationLayer.ViewModels;
using System;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize]
    [ApiController]
    public class MediaFileController : ControllerBase
    {
        private readonly IConsoleAppRunner _consoleAppRunner;
        private readonly IUserService _userService;

        private readonly IUserFileService _userFileService;
        public MediaFileController(IConsoleAppRunner consoleAppRunner, IUserService userService, IUserFileService userFileService)
        {
            _consoleAppRunner = consoleAppRunner;
            _userService = userService;
            _userFileService = userFileService; 
        }

        [HttpGet("GetUserFiles")]
        public async Task<ActionResult> GetUserFiles()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("User name is not available in the claims.");
            }

            var userFiles = await _userFileService.GetUserFiles(userName);
            if (userFiles == null || userFiles.Count == 0) 
            {
                return NotFound("No files found for the user.");
            }
            return Ok(userFiles);
        }

        [HttpGet("DownloadUserFiles")]
        public async Task<ActionResult> DownloadUserFilesByUsername()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("User name is not available in the claims.");
            }

            var userFiles = await _userFileService.GetUserFiles(userName);
            if (userFiles == null || userFiles.Count == 0)
            {
                return NotFound("No files found for the user.");
            }

            using var memoryStream = new MemoryStream();
            using (var zip = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                foreach (var filePath in userFiles.SelectMany(f => new[] { f.Stem1, f.Stem2, f.Stem3, f.Stem4, f.Stem5  }))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        var entryName = Path.GetFileName(filePath);
                        if (string.IsNullOrEmpty(entryName))
                        {
                            continue;
                        }
                        var entry = zip.CreateEntry(entryName);
                        if (entry == null)
                        {
                            continue;
                        }

                        using var originalFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        using var zipStream = entry.Open();
                        await originalFileStream.CopyToAsync(zipStream);
                    }
                }
            }

            memoryStream.Position = 0;
            return File(memoryStream.ToArray(), "application/zip", "UserFiles.zip");

        }

        [HttpPost("UploadAudio")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> InsertFileAndCallSpleeter(MediaFileVM mediaFileVM)
        {
            //Checking whether the user is ahenticated or not

            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("User name is not available in the claims.");
            }

            //Checking whether the user has uploaded a correct file or not

            string extension = Path.GetExtension(mediaFileVM.mediaFile.FileName).ToLowerInvariant();
            List<string> AllowedFileExtensions = new() { ".mp3", ".wav", ".flac", ".ogg", ".m4a", ".aac" };
            if (!AllowedFileExtensions.Contains(extension))
            {
                return BadRequest("The extension of the file is not supported. Please upload a valid audio file.");
            }


            //Creating a unique folder for the user to store the uploaded file and processed files

            string InstanceFolderPath = Guid.NewGuid().ToString();// + extension;
            string StoragePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", InstanceFolderPath);
            Console.WriteLine("Storage Path: "+StoragePath);
            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }

            //Saving the uploaded file to the unique folder

            FileStream stream = new FileStream(Path.Combine(StoragePath,mediaFileVM.mediaFile.FileName), FileMode.Create);
            await mediaFileVM.mediaFile.CopyToAsync(stream);
            stream.Close();

            //Creating the file path where file is saved and calling the Spleeter process

            string filePath = Path.Combine(StoragePath,mediaFileVM.mediaFile.FileName);
            Console.WriteLine("File Path: " +filePath);
            var fileExecutionResult = await _consoleAppRunner.RunSpleeter(mediaFileVM.noOfStems, StoragePath, filePath);

            //Creating a user file entry in the database and saving the file information

            string fileNameWithoutExtension = Path.Combine(StoragePath, Path.GetFileNameWithoutExtension(mediaFileVM.mediaFile.FileName));
            Console.WriteLine("File Path for saving in DB: " + fileNameWithoutExtension);
            var user = await _userService.GetUserByUsername(userName);
            var UserFile = await _userFileService.AddFilesAsync(mediaFileVM.noOfStems, StoragePath, user[0].Pkuser, fileNameWithoutExtension);
            return Ok(new
            {
                // fileExecutionResult,
                Message = "File uploaded successfully.",
                FilePath = filePath,
                NoOfStems = mediaFileVM.noOfStems,
                Stem1 = UserFile.Stem1,
                Stem2 = UserFile.Stem2,
                Stem3 = UserFile.Stem3,
                Stem4 = UserFile.Stem4, 
                Stem5 = UserFile.Stem5
            });

        }

    }
}