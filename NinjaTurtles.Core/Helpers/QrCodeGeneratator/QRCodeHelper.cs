using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;


namespace NinjaTurtles.Core.Helpers.QrCode
{
    public static class QRCodeHelper
    {

        public static string SaveQRCodeAsImage(string content, string filePath, ImageFormat format)
        {
            // QR kodu oluştur
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            using var qrCodeImage = qrCode.GetGraphic(20);

            // Belirtilen dosya yoluna QR kodu kaydet
            qrCodeImage.Save(filePath, format);

            return filePath; // Kaydedilen dosya yolunu döndür
        }
    }
}
