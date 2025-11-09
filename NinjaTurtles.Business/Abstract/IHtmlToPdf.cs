

namespace NinjaTurtles.Business.Abstract
{
    public interface IHtmlToPdf
    {
        byte[] ConvertHtml(String html, string orientation = "portrait");
    }
}
