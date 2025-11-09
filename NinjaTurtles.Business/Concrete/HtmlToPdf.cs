using DinkToPdf;
using DinkToPdf.Contracts;
using NinjaTurtles.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Business.Concrete
{
    public class HtmlToPdf : IHtmlToPdf
    {
        private readonly IConverter _converter;

        public HtmlToPdf(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] ConvertHtml(string html, string orientation = "portrait")
        {
            StringBuilder stringBuilder = new StringBuilder();
            Orientation orientationType = Orientation.Portrait;
            if (orientation != "portrait")
                orientationType = Orientation.Landscape;
            else
                orientationType = Orientation.Portrait;
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = orientationType,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 2 },
                DocumentTitle = "PDF Report",
                UseCompression = true
                //Out = @physicalPath
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" },
                //HeaderSettings = { FontName = "Tahoma", FontSize = 9, Right = "[page]. sayfadasınız, Toplam Sayfa : [toPage]", Line = false },
                //FooterSettings = { FontName = "Tahoma", FontSize = 9, Line = true, Center = "" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            return _converter.Convert(pdf);
        }
    }
}
