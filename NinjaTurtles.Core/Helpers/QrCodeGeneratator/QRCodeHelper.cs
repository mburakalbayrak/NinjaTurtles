using QRCoder;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace NinjaTurtles.Core.Helpers.QrCodeGeneratator
{
    public static class QRCodeHelper
    {
        public enum LabelPosition { Top, Bottom }

        public static async Task<string> GenerateQrWithLabelAsync(
            string content,
            string labelText,
            string savePath,
            LabelPosition position = LabelPosition.Bottom,
            int pixelsPerModule = 20,
            int margin = 10,
            string? fontFilePath = null,
            float initialFontSize = 24f,
            float minFontSize = 10f)
        {
            // QR üret
            using var qrGen = new QRCodeGenerator();
            using var qrData = qrGen.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var pngQr = new PngByteQRCode(qrData);
            byte[] qrPngBytes = pngQr.GetGraphic(
    pixelsPerModule: 20,
    drawQuietZones: false);
            using Image<Rgba32> qrImg = Image.Load<Rgba32>(qrPngBytes);

            // Font seçimi (TryGet + fallback)
            FontCollection fc = new();
            FontFamily family;
            if (!string.IsNullOrWhiteSpace(fontFilePath) && File.Exists(fontFilePath))
                family = fc.Add(fontFilePath);
            else if (SystemFonts.TryGet("Arial", out var arial))
                family = arial;
            else
                family = SystemFonts.Families.First();

            // Metni ölç – MeasureSize -> FontRectangle
            float availableWidth = qrImg.Width - (margin * 2);
            float currentSize = initialFontSize;
            Font fitFont = new Font(family, currentSize, FontStyle.Regular);
            FontRectangle rect;

            do
            {
                fitFont = new Font(family, currentSize, FontStyle.Regular);
                rect = TextMeasurer.MeasureSize(labelText, new TextOptions(fitFont) { Dpi = 72, WrappingLength = 0 });
                if (rect.Width <= availableWidth) break;
                currentSize -= 2f;
            } while (currentSize >= minFontSize);

            int textBlockHeight = (int)Math.Ceiling(rect.Height) + (margin * 2);
            int finalW = qrImg.Width;
            int finalH = qrImg.Height + textBlockHeight;

            using Image<Rgba32> finalImg = new(finalW, finalH, Color.White);

            // Çizim
            int qrY = position == LabelPosition.Top ? textBlockHeight : 0;
            finalImg.Mutate(ctx =>
            {
                ctx.DrawImage(qrImg, new Point(0, qrY), 1f);

                int textY = position == LabelPosition.Top ? 0 : qrImg.Height;
                float textX = (finalW - rect.Width) / 2f;
                float baselineY = textY + (textBlockHeight - rect.Height) / 2f;

                // Doğru overload: text, font, color, location
                ctx.DrawText(labelText, fitFont, Color.Black, new PointF(textX, baselineY));
            });

            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
            await finalImg.SaveAsync(savePath);
            return savePath;
        }
    }
}
