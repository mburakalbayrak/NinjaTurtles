using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NinjaTurtles.Core.Extensions
{
    public static class NinjaExtensions
    {
        public static string CleanPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return phoneNumber;
            }
            if (phoneNumber.StartsWith("90"))
            {
                return phoneNumber;
            }
            else if (phoneNumber.StartsWith("0"))
                phoneNumber = "9" + phoneNumber;
            else
            {
                phoneNumber = "90" + phoneNumber;
            }
            return phoneNumber;
        }

        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }


        public static string RemoveSpace(this string word)
        {
            return Regex.Replace(word, @"\s", "");
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string GetImageTagFromHtml(this string text)
        {
            string s = string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                Regex rx = new Regex(@"<img[^>]*>");

                if (rx.Matches(text).Count > 0)
                    s = rx.Matches(text)[0].Value;

                Regex re = new Regex("src=\"/upload");
                s = re.Replace(s, "src=\"/upload");

                Regex res = new Regex("(width=\"[0-9]\")|(height=\"[0-9]\")|(style=\"[^\"]\")|(align=\"[^\"]\")");
                s = res.Replace(s, string.Empty);


            }
            return s;

        }
        public static string GetImageUrlFromHtml(this string text)
        {
            string s = string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                Regex rx = new Regex(@"<img[^>]*>");

                if (rx.Matches(text).Count > 0)
                    s = Regex.Match(text, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
            }
            return s;
        }

        public static string GetContentType(this string extension)
        {
            switch (extension)
            {
                case ".bmp":
                    return "image/bmp";
                case ".gif":
                    return "image/gif";
                case ".ico":
                    return "image/x-icon";
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                case ".tif":
                case ".tiff":
                    return "image/tiff";
                case ".htm":
                case ".html":
                    return "text/html";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt":
                    return "application/vnd.ms-powerpoint";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".zip":
                    return "application/zip";
                case ".rar":
                    return "application/x-rar-compressed";
                case ".msg":
                    return "application/vnd.ms-outlook";
                case ".txt":
                    return "text/plain";
                case ".xml":
                    return "application/xml";
                case ".xlsm":
                    return "application/vnd.ms-excel.sheet.macroEnabled.12";
            }
            return "application/octet-stream";
        }

        public static byte[] StreamToByte(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }


        public async static Task<MemoryStream> FileFromPathToMemoryStreamAsync(this string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            var memory = new MemoryStream();

            using (var file = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                await file.CopyToAsync(memory);
            }
            memory.Position = 0;
            return memory;
        }
    }
}
