
namespace NinjaTurtles.Core.Utilities.Results
{
    public class FileStreamMemoryResult
    {
        public Stream Stream { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public bool EnableRangeProcessing { get; set; }
    }
}
