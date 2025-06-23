using System.Security.Claims;
using BusinessLayerLogic.ExternalProcesses;
using BusinessLayerLogic.Services;
using BusinessLayerLogic.Services.Contracts;
using DatabaseLayerLogic.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;

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
            if (userFiles == null || !userFiles.Any())
            {
                return NotFound("No files found for the user.");
            }
            return Ok(userFiles);
        }

        [HttpPost("UploadAudio")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> InsertFileAndCallSpleeter(MediaFileVM mediaFileVM)
        {
            //TBD : Refactor the code 
            var userName= User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("User name is not available in the claims.");
            }

            var userFolder = await _userFileService.GetFolderByUsername(userName);

            if(userFolder == null)
            {
                string extension = Path.GetExtension(mediaFileVM.mediaFile.FileName).ToLowerInvariant();
                List<string> AllowedFileExtensions = new() { ".mp3", ".wav", ".flac", ".ogg", ".m4a", ".aac" };
                if (!AllowedFileExtensions.Contains(extension))
                {
                    return BadRequest("The extension of the file is not supported. Please upload a valid audio file.");
                }
                string InstanceFolderPath = Guid.NewGuid().ToString() + extension;
                string StoragePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", InstanceFolderPath);
                if (!Directory.Exists(StoragePath))
                {
                    Directory.CreateDirectory(StoragePath);
                }
                FileStream stream = new FileStream(Path.Combine(StoragePath, mediaFileVM.mediaFile.FileName), FileMode.Create);
                await mediaFileVM.mediaFile.CopyToAsync(stream);
                stream.Close();
                string filePath = Path.Combine(StoragePath, mediaFileVM.mediaFile.FileName);
                var fileExecutionResult = await _consoleAppRunner.RunSpleeter(mediaFileVM.noOfStems, StoragePath, filePath);
                var user = await _userService.GetUserByUsername(userName);
                await _userFileService.AddFilesAsync(mediaFileVM.noOfStems, StoragePath, user[0].Pkuser, mediaFileVM.mediaFile.FileName);
                string folderInfo = "FOLDER FOR THIS USER DOES NOT EXIST SO CREATED ONE: " + StoragePath.ToString();
                return Ok(new
                {
                    folderInfo,
                    fileExecutionResult,
                    Message = "File uploaded successfully.",
                    FilePath = filePath,
                    NoOfStems = mediaFileVM.noOfStems
                });
            }
            else
            {

                string extension = Path.GetExtension(mediaFileVM.mediaFile.FileName).ToLowerInvariant();
                List<string> AllowedFileExtensions = new() { ".mp3", ".wav", ".flac", ".ogg", ".m4a", ".aac" };
                if (!AllowedFileExtensions.Contains(extension))
                {
                    return BadRequest("The extension of the file is not supported. Please upload a valid audio file.");
                }
                string folderInfo = "FOLDER FOR THIS USER ALREADY EXISTS: " + userFolder;
                string StoragePath = userFolder;
                FileStream stream = new FileStream(Path.Combine(StoragePath, mediaFileVM.mediaFile.FileName), FileMode.Create);
                await mediaFileVM.mediaFile.CopyToAsync(stream);
                stream.Close();
                string filePath = Path.Combine(StoragePath, mediaFileVM.mediaFile.FileName);
                var fileExecutionResult = await _consoleAppRunner.RunSpleeter(mediaFileVM.noOfStems, StoragePath, filePath);
                var user = await _userService.GetUserByUsername(userName);
                await _userFileService.AddFilesAsync(mediaFileVM.noOfStems, StoragePath, user[0].Pkuser, mediaFileVM.mediaFile.FileName);
                return Ok(new
                {
                    folderInfo,
                    fileExecutionResult,
                    Message = "File uploaded successfully.",
                    FilePath = filePath,
                    NoOfStems = mediaFileVM.noOfStems
                });

            }

        }

    }
}