using Microsoft.AspNetCore.Http;


namespace NinjaTurtles.Core.DataAccess.Concrete.Dto
{
    public class CreateFileByteWithFileNameDto
    {
        public string FolderPath { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
    }
}
