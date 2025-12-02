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
         string colorCode ="",
         LabelPosition position = LabelPosition.Bottom,
         int pixelsPerModule = 20,
         int margin = 10,
         string? fontFilePath = null,
         float initialFontSize = 100f,
         float minFontSize = 10f,
         int quietZoneModules = 1 // <-- eklendi
     )
        {
            // 1) QR üret (açık modüller saydam kalsın; quiet zone'u biz beyaz dolduracağız)
            using var qrGen = new QRCodeGenerator();
            using var qrData = qrGen.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var pngQr = new PngByteQRCode(qrData);
            byte[] qrPngBytes = pngQr.GetGraphic(
            pixelsPerModule: pixelsPerModule,
                darkColorRgba: RgbaBytes("#000000"),      // siyah
                lightColorRgba: RgbaBytes("#FFFF"), // saydam
                drawQuietZones: false
            );

            using Image<Rgba32> qrImg = Image.Load<Rgba32>(qrPngBytes);

            // 2) Font seçimi
            FontCollection fc = new();
            FontFamily family;
            if (!string.IsNullOrWhiteSpace(fontFilePath) && File.Exists(fontFilePath))
                family = fc.Add(fontFilePath);
            else if (SystemFonts.TryGet("Arial", out var arial))
                family = arial;
            else
                family = SystemFonts.Families.First();

            // 3) Quiet zone ve final boyutlar
            int qzPx = Math.Max(0, quietZoneModules) * pixelsPerModule;
            int qrAreaSize = qrImg.Width + (qzPx * 2); // kare alan (quiet zone dahil)

            // Etiketi ölçmeden önce final genişliği bilmek iyi olur
            float availableWidth = qrAreaSize - (margin * 2);

            // 4) Etiket ölçümü
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
            int finalW = qrAreaSize;
            int finalH = qrAreaSize + textBlockHeight;

            using Image<Rgba32> finalImg = new(finalW, finalH, Color.White);

            // 5) Çizim
            int qrY = position == LabelPosition.Top ? textBlockHeight : 0;

            finalImg.Mutate(ctx =>
            {
                // Quiet zone alanını beyaz doldur (standart gereği)
                var qrAreaRect = new Rectangle(0, qrY, qrAreaSize, qrAreaSize);
                ctx.Fill(Color.White, qrAreaRect);

                // QR’ı quiet zone içine yerleştir (saydam açık modüller sayesinde beyaz boşluk korunur)
                var qrTopLeft = new Point(qzPx, qrY + qzPx);
                ctx.DrawImage(qrImg, qrTopLeft, 1f);

                // Etiket
                int textY = position == LabelPosition.Top ? 0 : qrAreaSize;
                float textX = (finalW - rect.Width) / 2f;
                float baselineY = textY + (textBlockHeight - rect.Height) / 2f;
                ctx.DrawText(labelText, fitFont, Color.Black, new PointF(textX, baselineY));
            });

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(savePath)!);
            await finalImg.SaveAsync(savePath);
            return savePath;
        }

        static byte[] RgbaBytes(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException(nameof(s));
            var h = s.Trim().TrimStart('#');
            byte X(char c) { int v = int.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber); return (byte)((v << 4) | v); }

            byte r, g, b, a;
            if (h.Length == 3) { r = X(h[0]); g = X(h[1]); b = X(h[2]); a = 255; }
            else if (h.Length == 4) { r = X(h[0]); g = X(h[1]); b = X(h[2]); a = X(h[3]); }
            else if (h.Length == 6) { r = Convert.ToByte(h.Substring(0, 2), 16); g = Convert.ToByte(h.Substring(2, 2), 16); b = Convert.ToByte(h.Substring(4, 2), 16); a = 255; }
            else if (h.Length == 8) { r = Convert.ToByte(h.Substring(0, 2), 16); g = Convert.ToByte(h.Substring(2, 2), 16); b = Convert.ToByte(h.Substring(4, 2), 16); a = Convert.ToByte(h.Substring(6, 2), 16); }
            else throw new ArgumentException("Geçersiz hex uzunluğu.");
            return new[] { r, g, b, a };
        }
    }

}
