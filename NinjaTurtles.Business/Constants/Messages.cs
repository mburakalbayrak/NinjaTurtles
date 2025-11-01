using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün başarıyla eklendi";
        public static string ProductDeleted = "Ürün başarıyla silindi";
        public static string ProductUpdated = "Ürün başarıyla güncellendi";

        public static string CustomerAdded = "Hesabınız başarıyla oluşturulmuştur, lütfen e-postanıza gelen kod ile hesabızı doğrulayın.";
        public static string HasCustomer= "Hesabınız zaten mevcut, lütfen e-postanıza gelen kodu doğrulayarak aksesuarınızı tanımlamaya devam edin.";
        public static string SendMailCode= "lütfen e-postanıza gelen kodu doğrulayarak aksesuarınızı tanımlamaya devam edin.";
        public static string CustomerDeleted = "Hesabınız silinmiştir.";
        public static string CustomerGetList = "Customerlar getirilmiştir.";
        public static string CompanyAdded = "Firma başarıyla eklendi";
        public static string CompanyUpdated = "Firma başarıyla güncellendi";
        public static string CompanyOrderDetailAdded = "Sipariş başarıyla oluşturuldu";
        public static string QrFilled = "Bilgileriniz karekodunuza başarıyla eşleştirilmilştir.";
        public static string EmptyQr = "Bilgilerinizi doldurduktan sonra aksesuarınızı kullanmaya başlayabilirsiniz.";
        public static string DataNotFound = "Veri bulunamadı.";
        public static string AccountVerifySuccess = "Hesabınız başarıyla doğrulanmıştır.";
        public static string VerifyCodeExpired = "Doğrulama kodu hatalı veya doğrulama kodunun süresi doldu!";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Şifre hatalı";
        public static string SuccessfulLogin = "Giriş başarılı";
        public static string UserAlreadyExists = "Kullanıcı zaten mevcut";
        public static string UserRegistered = "Kullanıcı başarıyla kayıt edildi";
        public static string AccessTokenCreated = "Access token başarıyla oluşturuldu";

        public static string VerifyMailTemplate = @"<!doctype html>
<html lang=""tr"">
<head>
  <meta charset=""utf-8"">
  <meta name=""viewport"" content=""width=device-width,initial-scale=1"">
  <title>Karekodla – E-posta Doğrulama</title>

  <!-- (Opsiyonel) Inter web fontu — bazı e-posta istemcileri görmezden gelir -->
  <!-- <link href=""https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700;800&display=swap"" rel=""stylesheet""> -->

  <style>
    /* Basit, e-posta uyumlu stiller */
    body {
      margin:0; padding:0; background:#f6f7f9;
      font-family:""Inter"",-apple-system,BlinkMacSystemFont,""Segoe UI"",Helvetica,Arial,sans-serif,""Apple Color Emoji"",""Segoe UI Emoji"";
      -webkit-font-smoothing:antialiased; -moz-osx-font-smoothing:grayscale;
    }
    .container { max-width:600px; margin:0 auto; padding:24px; }
    .brand-strip { background:#10b981; background:linear-gradient(135deg,#10b981,#059669); border-radius:14px; padding:16px 20px; }
    .card { background:#ffffff; border-radius:14px; padding:28px; margin-top:-12px; box-shadow:0 8px 24px rgba(2,6,23,0.06); border:1px solid #e5e7eb; }
    .brand { text-align:center; }
    .brand img { height:140px; width:auto; display:inline-block; }

    h1 { font-size:22px; margin:0 0 10px; color:#0b1324; letter-spacing:0.2px; font-weight:700; }
    p  { font-size:14px; line-height:1.65; color:#374151; margin:0 0 14px; }

    .note { background:#ecfdf5; border:1px dashed #86efac; border-radius:12px; padding:10px 12px; color:#065f46; font-size:12px; }
    .code-wrap { background:#f0fdf4; border:1px solid #bbf7d0; border-radius:14px; padding:18px 16px; margin:14px 0; }

    /* Kod alanı monospace kalsın */
    .code-box {
      font-family:ui-monospace,SFMono-Regular,Menlo,Monaco,Consolas,""Liberation Mono"",""Courier New"",monospace;
      letter-spacing:6px; font-weight:800; font-size:30px; text-align:center; color:#064e3b;
    }

    .cta { text-align:center; margin:18px 0 4px; }
    .btn {
      display:inline-block; padding:12px 20px; text-decoration:none; border-radius:12px;
      background:#10b981; color:#ffffff !important; font-weight:700; border:1px solid #059669;
      font-family:""Inter"",-apple-system,BlinkMacSystemFont,""Segoe UI"",Helvetica,Arial,sans-serif,""Apple Color Emoji"",""Segoe UI Emoji"";
    }

    .muted { color:#6b7280; font-size:12px; }
    .footer { text-align:center; color:#9ca3af; font-size:12px; margin-top:16px; }
    a { color:#059669; }

    @media (prefers-color-scheme: dark) {
      body { background:#0b0c0f; }
      .card { background:#0f1117; border-color:#1f2937; box-shadow:none; }
      h1, .code-box { color:#e5e7eb; }
      p, .muted, .footer { color:#cbd5e1; }
      .note { background:#022c22; border-color:#064e3b; color:#a7f3d0; }
      .code-wrap { background:#052e1e; border-color:#065f46; }
      .btn { background:#10b981; border-color:#34d399; }
      a { color:#34d399; }
    }

    @media screen and (max-width:480px){
      .container{ padding:16px; }
      .brand img{ height:120px; }
      .card{ padding:22px; }
      .code-box{ letter-spacing:4px; font-size:26px; }
    }

    /* Outlook (MSO) için güvenli yedek font */
  </style>

  <!--[if mso]>
  <style type=""text/css"">
    body, p, h1, a, span, div { font-family: Arial, sans-serif !important; }
  </style>
  <![endif]-->
</head>
<body>
  <div class=""container"">

    <div class=""brand-strip"">
      <div class=""brand"">
        <img src=""https://karekodla.com/logo.png"" alt=""Karekodla logosu"">
      </div>
    </div>

    <div class=""card"" role=""presentation"">
      <h1>Karekodla hesabını doğrula</h1>
      <p>Merhaba {{displayName}},</p>
      <p>Karekodla hesabını doğrulamak için aşağıdaki kodu kullan:</p>

      <div class=""code-wrap"">
        <div class=""code-box"" aria-label=""Doğrulama kodu"">{{code}}</div>
      </div>

      <p class=""muted"">Kod <strong>5 dakika</strong> içinde kullanılmazsa geçersiz olacaktır.</p>


      <div class=""note"">
        <strong>Güvenlik notu:</strong> Kodu kimseyle paylaşma ve sadece
        <span dir=""ltr"">karekodla.com</span> alanındaki sayfalara gir.
      </div>

      <p>Bu işlemi sen başlatmadıysan, lütfen bu e-postayı görmezden gel ya da bize ulaş:
        <a href=""mailto:destek@karekodla.com"">destek@karekodla.com</a>
      </p>
    </div>

    <div class=""footer"">
      © {{year}} Karekodla • <a href=""{{securityUrl}}"">Güvenlik Merkezi</a> • <a href=""{{helpUrl}}"">Yardım</a>
    </div>
  </div>
</body>
</html>

";



    }
}
