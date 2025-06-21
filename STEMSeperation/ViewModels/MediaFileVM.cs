namespace PresentationLayer.ViewModels
{
    public class MediaFileVM
    {
        public int noOfStems { get; set; }
        public required IFormFile mediaFile { get; set; }
    }
}
