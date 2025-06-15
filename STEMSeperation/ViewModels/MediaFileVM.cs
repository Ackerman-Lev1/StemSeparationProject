namespace PresentationLayer.ViewModels
{
    public class MediaFileVM
    {
        public string userName { get; set; } = null!;
        //public string filePath { get; set; } = null!;
        public int noOfStems { get; set; }
        public IFormFile mediaFile { get; set; }
    }
}
