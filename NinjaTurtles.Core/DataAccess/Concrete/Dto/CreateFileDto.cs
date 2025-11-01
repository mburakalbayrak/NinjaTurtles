
namespace NinjaTurtles.Core.DataAccess.Concrete.Dto
{
    public class CreateFileDto
    {
        public string RealName { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public bool IsSuccess { get; set; }

    }
}
