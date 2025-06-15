using BusinessLayerLogic.ExternalProcesses;
using DatabaseLayerLogic.Repositories;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;

namespace PresentationLayer.Controllers
{
    public class MediaFileController : ControllerBase
    {
        private readonly IConsoleAppRunner _consoleAppRunner;
        private readonly IUserRepository _userRepository;
        public MediaFileController(IConsoleAppRunner consoleAppRunner, IUserRepository userRepository)
        {
            _consoleAppRunner = consoleAppRunner;
            _userRepository = userRepository;
        }
        [HttpPost("Upload-Audio")]
        [Route("CommandCalling")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> InsertFileAndCallSpleeter(MediaFileVM mediaFileVM)
        {
            //this is the part wehre we need to add the new folder creation for the specific user code
            var originalTrackFilePath = "C:\\Users\\sukum\\Projects\\K_For_Krishna.mp3";
            //Testing is pending for the below code
            var fileExecutionResult = await _consoleAppRunner.RunSpleeter(mediaFileVM.noOfStems, originalTrackFilePath);
            return Ok(fileExecutionResult);

        }

    }
}