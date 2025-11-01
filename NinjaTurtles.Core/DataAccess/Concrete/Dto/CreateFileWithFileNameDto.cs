using Microsoft.AspNetCore.Http;


namespace NinjaTurtles.Core.DataAccess.Concrete.Dto
{
    public class CreateFileWithFileNameDto
    {
        public string FolderPath { get; set; }
        public IFormFile File { get; set; }
        public string FileName { get; set; }
    }
}
