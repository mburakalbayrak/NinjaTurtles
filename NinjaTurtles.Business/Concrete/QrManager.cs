using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.DataAccess.Concrete.Dto;
using NinjaTurtles.Core.Helpers.FileUpload;
using NinjaTurtles.Core.Helpers.MailServices;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;
using System.Net;
using static QRCoder.PayloadGenerator;

namespace NinjaTurtles.Business.Concrete
{
    public class QrManager : IQrService
    {
        private IQrCodeMainDal _qrCodeMainDal;
        private IQrCodeHumanDetailDal _qrCodeHumanDetailDal;
        private IQrCodeAnimalDetailDal _qrCodeAnimalDetailDal;
        private ICustomerDal _customerDal;
        private IHttpContextAccessor _httpContextAccessor;
        private ICustomerQrVerificationDal _customerQrVerificationDal;
        private IQrLogDal _qrLogDal;
        private IParamItemDal _paramItemDal;
        private IConfiguration _config;
        private IMapper _mapper;

        public QrManager(IQrCodeMainDal qrCodeMainDal, IQrCodeHumanDetailDal qrCodeHumanDetailDal, IQrCodeAnimalDetailDal qrCodeAnimalDetailDal, ICustomerDal customerDal, IHttpContextAccessor httpContextAccessor, ICustomerQrVerificationDal customerQrVerificationDal, IQrLogDal qrLogDal, IParamItemDal paramItemDal, IConfiguration config, IMapper mapper)
        {
            _qrCodeMainDal = qrCodeMainDal;
            _qrCodeHumanDetailDal = qrCodeHumanDetailDal;
            _qrCodeAnimalDetailDal = qrCodeAnimalDetailDal;
            _customerDal = customerDal;
            _httpContextAccessor = httpContextAccessor;
            _customerQrVerificationDal = customerQrVerificationDal;
            _qrLogDal = qrLogDal;
            _paramItemDal = paramItemDal;
            _config = config;
            _mapper = mapper;
        }

        public IResult CreateAnimalDetail(QrCodeAnimalCreateDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));
            if (qr == null)
                return new ErrorDataResult<QrCodeDetailDto>(data: null, message: Messages.DataNotFound);

            var qrAnimal = _mapper.Map<QrCodeAnimalDetail>(dto);

            qr.DetailTypeId = 2;
            qr.CustomerId = dto.CustomerId;
            _qrCodeAnimalDetailDal.Add(qrAnimal);
            _qrCodeMainDal.Update(qr);
            string clientIp = GetClientIp();

            var qrlog = new QrLog
            {
                LogTypeId = 2,
                QrCodeMainId = qr.Id,
                IpAddress = clientIp,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true,

            };
            _qrLogDal.Add(qrlog);
            return new Result(true, Messages.QrFilled);

        }

        public IResult CreateHumanDetail(QrCodeHumanCreateDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));

            if (qr == null)
                return new Result(false, message: Messages.DataNotFound);
            try
            {
                var qrHuman = _mapper.Map<QrCodeHumanDetail>(dto);

                qr.DetailTypeId = 1;
                qr.CustomerId = dto.CustomerId;
                if (dto.File != null)
                {
                    var directory = @"D:\vhosts\karekodla.com\UploadFiles\";
                    //string directory = _config.GetSection("Directories:FileDirectory").Value;

                    string path = System.IO.Path.Combine(directory, "ProfilePictures");
                    CreateFileWithFileNameDto uploadFile = new CreateFileWithFileNameDto()
                    {
                        File = dto.File,
                        FolderPath = path,
                        FileName = qrHuman.FullName + "_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString() + "_" + dto.File.FileName
                    };
                    var file = WriteFile.CreateFileWithFileName(uploadFile).Data;
                    qrHuman.ProfilePictureUrl = uploadFile.FileName;
                }
                _qrCodeHumanDetailDal.Add(qrHuman);
                _qrCodeMainDal.Update(qr);

                string clientIp = GetClientIp();

                var qrlog = new QrLog
                {
                    LogTypeId = 2,
                    QrCodeMainId = qr.Id,
                    IpAddress = clientIp,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true,

                };
                _qrLogDal.Add(qrlog);
                return new Result(true, Messages.QrFilled);
            }
            catch (Exception ex)
            {
                return new Result(true, ex.Message);
            }
        }

        public IResult CreateRedirectUrl(QrRedirectUrlDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));

            if (qr == null)
                return new Result(false, message: Messages.DataNotFound);
            qr.RedirectUrl = dto.RedirectUrl;
            qr.CustomerId = dto.CustomerId;
            qr.DetailTypeId = 3;
            _qrCodeMainDal.Update(qr);
            string clientIp = GetClientIp();

            var qrlog = new QrLog
            {
                LogTypeId = 2,
                QrCodeMainId = qr.Id,
                IpAddress = clientIp,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true,

            };
            _qrLogDal.Add(qrlog);
            return new Result(true, Messages.QrFilled);

        }

        public IDataResult<QrCodeAnimalDetailDto> GetAnimalDetailVerify(QrUpdateVerifyDto dto)
        {
            var now = DateTime.Now;
            var hasCode = _customerQrVerificationDal.Get(c => c.Code == dto.Code && c.ExpireDate > now);
            if (hasCode == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(message: Messages.VerifyCodeExpired);

            var qr = _qrCodeMainDal.Get(c => c.CustomerId == hasCode.CustomerId && c.Id == dto.QrMainId);
            if (qr == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(null, Messages.DataNotFound);

            var qrAnimal = _qrCodeAnimalDetailDal.Get(c => c.QrMainId == qr.Id);
            if (qrAnimal == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(null, Messages.DataNotFound);

            return new SuccessDataResult<QrCodeAnimalDetailDto>(
                _mapper.Map<QrCodeAnimalDetailDto>(qrAnimal),
                Messages.AccountVerifySuccess
            );
        }

        public IDataResult<QrCodeHumanDetailDto> GetHumanDetailVerify(QrUpdateVerifyDto dto)
        {
            var now = DateTime.Now;
            var hasCode = _customerQrVerificationDal.Get(c => c.Code == dto.Code && c.ExpireDate > now);
            if (hasCode == null)
                return new ErrorDataResult<QrCodeHumanDetailDto>(message: Messages.VerifyCodeExpired);

            var qr = _qrCodeMainDal.Get(c => c.CustomerId == hasCode.CustomerId && c.Id == dto.QrMainId);
            if (qr == null)
                return new ErrorDataResult<QrCodeHumanDetailDto>(null, Messages.DataNotFound);

            var qrHuman = _qrCodeHumanDetailDal.Get(c => c.QrMainId == qr.Id);
            if (qrHuman == null)
                return new ErrorDataResult<QrCodeHumanDetailDto>(null, Messages.DataNotFound);

            var paramList = _paramItemDal.GetList();
            var qrDto = _mapper.Map<QrCodeHumanDetailDto>(qrHuman);

            if (qrHuman.ProfilePictureUrl != null)
            {
                var directory = @"D:\vhosts\karekodla.com\UploadFiles\";
                string filePath = Path.Combine(directory, "ProfilePictures", qrHuman.ProfilePictureUrl);
                if (System.IO.File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    var bytes = File.ReadAllBytes(filePath);
                    qrDto.ProfilePictureData = Convert.ToBase64String(bytes);
                }
            }
            qrDto.Gender = paramList.FirstOrDefault(c => c.Id == qrHuman.GenderId)?.Name;
            qrDto.MaritalStatus = paramList.FirstOrDefault(c => c.Id == qrHuman.MaritalStatusId)?.Name;
            qrDto.EducationStatus = paramList.FirstOrDefault(c => c.Id == qrHuman.EducationStatusId)?.Name;
            qrDto.CityOfResidence = paramList.FirstOrDefault(c => c.Id == qrHuman.CityOfResidenceId)?.Name;
            qrDto.BloodType = paramList.FirstOrDefault(c => c.Id == qrHuman.BloodTypeId)?.Name;
            qrDto.Profession = paramList.FirstOrDefault(c => c.Id == qrHuman.ProfessionId)?.Name;
            qrDto.PrimaryRelation = paramList.FirstOrDefault(c => c.Id == qrHuman.PrimaryRelationId)?.Name;
            qrDto.SecondaryRelation = paramList.FirstOrDefault(c => c.Id == qrHuman.SecondaryRelationId)?.Name;

            return new SuccessDataResult<QrCodeHumanDetailDto>(qrDto, Messages.AccountVerifySuccess);
        }
        public async Task<IDataResult<QrCodeDetailDto>> GetQrDetail(Guid id)
        {
            try
            {
                var qr = _qrCodeMainDal.Get(c => c.Id == id && c.IsActive && !c.IsDeleted);
                if (qr == null)
                    return new ErrorDataResult<QrCodeDetailDto>(null, Messages.DataNotFound);


                string clientIp = GetClientIp();
                var qrlog = new QrLog
                {
                    LogTypeId = 1,
                    QrCodeMainId = qr.Id,
                    IpAddress = clientIp,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true,
                };
                _qrLogDal.Add(qrlog);

                var qrDto = new QrCodeDetailDto
                {
                    DetailTypeId = qr.DetailTypeId ?? default,
                    Empty = !qr.DetailTypeId.HasValue
                };
                if (qrDto.Empty)
                    return new SuccessDataResult<QrCodeDetailDto>(qrDto, Messages.EmptyQr);

                var paramList = _paramItemDal.GetList();
                switch (qr.DetailTypeId)
                {
                    case 1:
                        var qrHuman = _qrCodeHumanDetailDal.Get(c => c.QrMainId == qr.Id);
                        qrDto.HumanDetail = _mapper.Map<QrCodeHumanDetailDto>(qrHuman);
                        if (qrHuman.ProfilePictureUrl != null)
                        {
                            var directory = @"D:\vhosts\karekodla.com\UploadFiles\";
                            string filePath = Path.Combine(directory, "ProfilePictures", qrHuman.ProfilePictureUrl);
                            if (System.IO.File.Exists(filePath))
                            {
                                FileInfo fileInfo = new FileInfo(filePath);
                                var bytes = File.ReadAllBytes(filePath);
                                qrDto.HumanDetail.ProfilePictureData = Convert.ToBase64String(bytes);
                            }
                        }
                        qrDto.HumanDetail.Gender = qrHuman.GenderId != null ? paramList.FirstOrDefault(c => c.Id == qrHuman.GenderId)?.Name : null;
                        qrDto.HumanDetail.MaritalStatus = qrHuman.MaritalStatusId != null ? paramList.FirstOrDefault(c => c.Id == qrHuman.MaritalStatusId)?.Name : null;
                        qrDto.HumanDetail.EducationStatus = qrHuman.EducationStatusId != null ? paramList.FirstOrDefault(c => c.Id == qrHuman.EducationStatusId)?.Name : null;
                        qrDto.HumanDetail.CityOfResidence = qrHuman.CityOfResidenceId != null ? paramList.FirstOrDefault(c => c.Id == qrHuman.CityOfResidenceId)?.Name : null;
                        qrDto.HumanDetail.BloodType = qrHuman.BloodTypeId != null ? paramList.FirstOrDefault(c => c.Id == qrHuman.BloodTypeId)?.Name : null;
                        qrDto.HumanDetail.Profession = qrHuman.ProfessionId != null ? paramList.FirstOrDefault(c => c.Id == qrHuman.ProfessionId)?.Name : null;
                        qrDto.HumanDetail.PrimaryRelation = qrHuman.PrimaryRelationId != null ? paramList.FirstOrDefault(c => c.Id == qrHuman.PrimaryRelationId)?.Name : null;
                        qrDto.HumanDetail.SecondaryRelation = qrHuman.SecondaryRelationId != null ? paramList.FirstOrDefault(c => c.Id == qrHuman.SecondaryRelationId)?.Name : null;
                        break;
                    case 2:
                        var qrAnimal = _qrCodeAnimalDetailDal.Get(c => c.QrMainId == qr.Id);
                        qrDto.AnimalDetail = _mapper.Map<QrCodeAnimalDetailDto>(qrAnimal);
                        break;
                    case 3:
                        qrDto.RedirectUrl = qr.RedirectUrl;
                        break;
                }

                var customer = _customerDal.Get(c => c.Id == qr.CustomerId);

                // ------------------ EKLENEN: basit IP -> konum + zaman ve mapUrl ------------------
                // ... mevcut kodun içinde, customer'ı aldıktan hemen ÖNCE/SONRA uygun yere ekle:

                // Istanbul saati
                var trTz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
                var nowTr = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, trTz);
                var scanTime = nowTr.ToString("d MMMM yyyy HH:mm", new System.Globalization.CultureInfo("tr-TR"));

                // 1) Header'lardan lat/lng almaya çalış
                double? headerLat = TryGetDoubleHeader("X-Geo-Lat");
                double? headerLng = TryGetDoubleHeader("X-Geo-Lng");

                // 2) Varsayılan: IP'den şehir/ilçe/ülke (+ yaklaşık lat/lng varsa harita)
                string city = "", district = "", country = "", mapUrl = "";
                clientIp = GetClientIp();

                try
                {
                    if (headerLat.HasValue && headerLng.HasValue)
                    {
                        // Kesin konumdan direkt harita linki
                        mapUrl = $"https://maps.google.com/?q={headerLat.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)},{headerLng.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

                        // (OPSİYONEL) Lat/Lng -> Şehir-İlçe ters geocode (Nominatim).
                        // Minimal: 2 sn timeout, User-Agent zorunlu.
                        (city, district, country) = await ReverseGeocodeAsync(headerLat.Value, headerLng.Value);
                    }
                    else if (!string.IsNullOrWhiteSpace(clientIp))
                    {
                        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
                        var resp = await http.GetAsync($"https://ipapi.co/{clientIp}/json/");
                        if (resp.IsSuccessStatusCode)
                        {
                            using var s = await resp.Content.ReadAsStreamAsync();
                            using var doc = await System.Text.Json.JsonDocument.ParseAsync(s);
                            city = doc.RootElement.TryGetProperty("city", out var c) ? c.GetString() ?? "" : "";
                            district = doc.RootElement.TryGetProperty("region", out var r) ? r.GetString() ?? "" : "";
                            country = doc.RootElement.TryGetProperty("country_name", out var co) ? co.GetString() ?? "" : "";

                            if (doc.RootElement.TryGetProperty("latitude", out var la) &&
                                doc.RootElement.TryGetProperty("longitude", out var lo) &&
                                la.TryGetDouble(out var latVal) && lo.TryGetDouble(out var lngVal))
                            {
                                mapUrl = $"https://maps.google.com/?q={latVal.ToString(System.Globalization.CultureInfo.InvariantCulture)},{lngVal.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
                            }
                        }
                    }
                }
                catch { /* sessiz geç */ }

                // ---- ŞABLON REPLACE (mail kısmına dokunmadan, body'yi hazırlıyoruz) ----
                var tpl = Messages.QrReadNotificationMailTemplate
                    .Replace("{{ownerName}}", $"{customer.FirstName} {customer.LastName}")
                    .Replace("{{scanTime}}", scanTime)
                    .Replace("{{city}}", city)
                    .Replace("{{district}}", district)
                    .Replace("{{mapUrl}}", mapUrl)
                    .Replace("{{year}}", DateTime.Now.Year.ToString())
                    .Replace("{{securityUrl}}", "https://karekodla.com/security")
                    .Replace("{{helpUrl}}", "https://karekodla.com/help");

                // -------------------------------------------------------------------------------

                MailWorker mail = new MailWorker();
                mail.Init(StaticVars.InfoMailUserName, StaticVars.InfoMailPassword);

                // ---- mail gönderim KISMI AYNI KALDI; sadece body'yi tpl ile veriyoruz ----
                var mailBody = tpl; // önceki: Messages.QrReadNotificationMailTemplate.Replace(...) yerine
                var result = await mail.SendMailAsync(
                    customer.Email,
                    StaticVars.VerifyMailUserName,
                    "Karekodla",
                    mailBody,
                    "Karekodla – QR Okuma Uyarısı",
                    true);

                return new SuccessDataResult<QrCodeDetailDto>(qrDto);
            }
            catch (Exception)
            {
                return new ErrorDataResult<QrCodeDetailDto>(Messages.DataNotFound);
            }

            // ------ local helpers ------
            string GetClientIp()
            {
                try
                {
                    // Eğer bu sınıfta IHttpContextAccessor varsa (_httpContextAccessor), onu kullan:
                    var ctx = _httpContextAccessor.HttpContext;
                    if (ctx == null) return "";

                    var cf = ctx.Request.Headers["CF-Connecting-IP"].ToString();
                    if (IPAddress.TryParse(cf, out _)) return cf;

                    var xff = ctx.Request.Headers["X-Forwarded-For"].ToString();
                    if (!string.IsNullOrWhiteSpace(xff))
                    {
                        var ip = xff.Split(',').Select(s => s.Trim()).FirstOrDefault();
                        if (IPAddress.TryParse(ip, out _)) return ip!;
                    }

                    var xri = ctx.Request.Headers["X-Real-IP"].ToString();
                    if (IPAddress.TryParse(xri, out _)) return xri;

                    return ctx.Connection.RemoteIpAddress?.ToString() ?? "";
                }
                catch { return ""; }
            }
        }

        double? TryGetDoubleHeader(string headerName)
        {
            try
            {
                var ctx = _httpContextAccessor?.HttpContext;
                if (ctx == null) return null;
                if (!ctx.Request.Headers.TryGetValue(headerName, out var values)) return null;
                var s = values.ToString();
                if (double.TryParse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var d))
                    return d;
                return null;
            }
            catch { return null; }
        }

        string GetClientIp()
        {
            try
            {
                var ctx = _httpContextAccessor?.HttpContext;
                if (ctx == null) return "";
                var cf = ctx.Request.Headers["CF-Connecting-IP"].ToString();
                if (System.Net.IPAddress.TryParse(cf, out _)) return cf;

                var xff = ctx.Request.Headers["X-Forwarded-For"].ToString();
                if (!string.IsNullOrWhiteSpace(xff))
                {
                    var ip = xff.Split(',').Select(s => s.Trim()).FirstOrDefault();
                    if (System.Net.IPAddress.TryParse(ip, out _)) return ip!;
                }

                var xri = ctx.Request.Headers["X-Real-IP"].ToString();
                if (System.Net.IPAddress.TryParse(xri, out _)) return xri;

                return ctx.Connection.RemoteIpAddress?.ToString() ?? "";
            }
            catch { return ""; }
        }

        // OPSİYONEL: ters geocode ile şehir/ilçe/ülke
        async Task<(string city, string district, string country)> ReverseGeocodeAsync(double lat, double lng)
        {
            try
            {
                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
                var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}&lon={lng.ToString(System.Globalization.CultureInfo.InvariantCulture)}&addressdetails=1";
                using var req = new HttpRequestMessage(HttpMethod.Get, url);
                req.Headers.UserAgent.ParseAdd("karekodla/1.0"); // Nominatim zorunlu
                using var resp = await http.SendAsync(req);
                if (!resp.IsSuccessStatusCode) return ("", "", "");
                using var s = await resp.Content.ReadAsStreamAsync();
                using var doc = await System.Text.Json.JsonDocument.ParseAsync(s);
                if (!doc.RootElement.TryGetProperty("address", out var addr)) return ("", "", "");
                string city = addr.TryGetProperty("city", out var c) ? c.GetString() ?? "" :
                              addr.TryGetProperty("town", out var t) ? t.GetString() ?? "" :
                              addr.TryGetProperty("village", out var v) ? v.GetString() ?? "" : "";
                string district = addr.TryGetProperty("suburb", out var sb) ? sb.GetString() ?? "" :
                                  addr.TryGetProperty("district", out var d) ? d.GetString() ?? "" : "";
                string country = addr.TryGetProperty("country", out var co) ? co.GetString() ?? "" : "";
                return (city, district, country);
            }
            catch { return ("", "", ""); }
        }

        public IResult UpdateAnimalDetail(QrCodeAnimalUpdateDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));
            if (qr == null)
                return new ErrorDataResult<QrCodeAnimalUpdateDto>(data: null, message: Messages.DataNotFound);

            var qrAnimalData = _qrCodeAnimalDetailDal.Get(c => c.Id == dto.Id && dto.QrMainId == dto.QrMainId);
            qr.DetailTypeId = 2;
            qrAnimalData.ModifiedBy = 1;
            qrAnimalData.ModifiedDate = DateTime.Now;
            _qrCodeMainDal.Update(qr);
            var qrAnimal = _mapper.Map(dto, qrAnimalData);
            _qrCodeAnimalDetailDal.Update(qrAnimal);

            string ipAddress = GetClientIp();
            var qrlog = new QrLog
            {
                LogTypeId = 3,
                QrCodeMainId = qr.Id,
                IpAddress = ipAddress,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true,

            };
            _qrLogDal.Add(qrlog);
            return new Result(true, Messages.QrFilled);

          
        }

        public IResult UpdateHumanDetail(QrCodeHumanUpdateDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));
            if (qr == null)
                return new ErrorDataResult<QrCodeHumanUpdateDto>(data: null, message: Messages.DataNotFound);
            var qrHumanData = _qrCodeHumanDetailDal.Get(c => c.Id == dto.Id && dto.QrMainId == dto.QrMainId);
            qr.DetailTypeId = 1;
            _qrCodeMainDal.Update(qr);
            var qrHuman = _mapper.Map(dto, qrHumanData);
            qrHuman.ModifiedBy = 1;
            qrHuman.ModifiedDate = DateTime.Now;
            if (dto.File != null)
            {
                var directory = @"D:\vhosts\karekodla.com\UploadFiles\";
                //string directory = _config.GetSection("Directories:FileDirectory").Value;

                CreateFileWithFileNameDto uploadFile = new CreateFileWithFileNameDto()
                {
                    File = dto.File,
                    FolderPath = directory,
                    FileName = qrHuman.FullName + "_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString() + "_" + dto.File.FileName
                };
                var file = WriteFile.CreateFileWithFileName(uploadFile).Data;
                if (qrHuman.ProfilePictureUrl != null)
                {
                    var oldFile = System.IO.Path.Combine(directory, qrHuman.ProfilePictureUrl);
                    if (System.IO.File.Exists(oldFile))
                        System.IO.File.Delete(oldFile);
                }
                qrHuman.ProfilePictureUrl = uploadFile.FileName;
            }
            string ipAddress = GetClientIp();

            var qrlog = new QrLog
            {
                LogTypeId = 3,
                QrCodeMainId = qr.Id,
                IpAddress = ipAddress,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true,

            };
            _qrLogDal.Add(qrlog);
            _qrCodeHumanDetailDal.Update(qrHuman);
            return new Result(true, Messages.QrFilled);

        }

        public IResult UpdateRedirectUrl(QrRedirectUrlUpdateDto dto)
        {
            var now = DateTime.Now;
            var hasCode = _customerQrVerificationDal.Get(c => c.Code == dto.Code && c.ExpireDate > now);
            if (hasCode == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(message: Messages.VerifyCodeExpired);

            var qr = _qrCodeMainDal.Get(c => c.CustomerId == hasCode.CustomerId && c.Id == dto.QrMainId);
            if (qr == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(null, Messages.DataNotFound);

            qr.ModifiedDate = DateTime.Now;
            qr.ModifiedBy = 1;
            qr.RedirectUrl = dto.RedirectUrl;
            _qrCodeMainDal.Update(qr);

            string ipAddress = GetClientIp();

            var qrlog = new QrLog
            {
                LogTypeId = 3,
                QrCodeMainId = qr.Id,
                IpAddress = ipAddress,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true,
            };
            return new Result(true, Messages.QrFilled);

        }
    }
}
