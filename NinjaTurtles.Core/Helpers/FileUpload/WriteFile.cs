using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using NinjaTurtles.Core.DataAccess.Concrete.Dto;
using NinjaTurtles.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Core.Helpers.FileUpload
{
    public static class WriteFile
    {
        public static bool CheckIfImageFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".jpeg" || extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".gif"); // Change the extension based on your need
        }

        public static IDataResult<CreateFileDto> CreateFile(string folderPath, IFormFile file)
        {
            string fileName = String.Empty;
            CreateFileDto createFile = new();

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                //fileName = DateTime.Now.Ticks + extension;
                fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.
                var pathBuilt = Path.Combine(folderPath, fileName);
                //if (!Directory.Exists(pathBuilt))
                //{
                //    Directory.CreateDirectory(pathBuilt);
                //}
                createFile.RealName = file.FileName;
                var path = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                createFile.IsSuccess = true;
                createFile.FileName = fileName;
                createFile.FullPath = path;
                return new SuccessDataResult<CreateFileDto>(createFile);
            }
            catch (Exception e)
            {
                createFile.IsSuccess = false;
                createFile.FileName = e.Message;
                return new ErrorDataResult<CreateFileDto>(createFile);
            }
        }

        public static IDataResult<CreateFileDto> CreateFileWithFileName(CreateFileWithFileNameDto model)
        {
            string fileName = String.Empty;
            CreateFileDto createFile = new();
            try
            {
                if (!Directory.Exists(model.FolderPath))
                {
                    Directory.CreateDirectory(model.FolderPath);
                }
                var extension = "." + model.File.FileName.Split('.')[model.File.FileName.Split('.').Length - 1];
                fileName = model.FileName;
                var pathBuilt = Path.Combine(model.FolderPath, fileName);
                createFile.RealName = model.File.FileName;
                var path = Path.Combine(model.FolderPath, fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }
                createFile.IsSuccess = true;
                createFile.FileName = fileName;
                createFile.FullPath = path;
                return new SuccessDataResult<CreateFileDto>(createFile);
            }
            catch (Exception e)
            {
                createFile.IsSuccess = false;
                createFile.FileName = e.Message;
                return new ErrorDataResult<CreateFileDto>(createFile);
            }
        }

        public static IFormFile ConvertByteArrayToFormFile(byte[] fileBytes, string fileName, string contentType = "application/pdf")
        {
            var stream = new MemoryStream(fileBytes);
            var formFile = new FormFile(stream, 0, fileBytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary()
                {
                    { "Content-Disposition", $"form-data; name=\"SeaFile\"; filename=\"{fileName}\"" }
                },
                ContentType = contentType,
                ContentDisposition = $"form-data; name=\"SeaFile\"; filename=\"{fileName}\""
            };
            return formFile;
        }
    }
}
