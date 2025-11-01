using Microsoft.AspNetCore.Http;

namespace NinjaTurtles.Entities.Dtos
{
    public class QrCodeHumanCreateDto
    {
        public Guid QrMainId { get; set; }
        public int CustomerId { get; set; }
        public string? FullName { get; set; } // Ad Soyad
        public DateTime? DateOfBirth { get; set; } // Doğum Tarihi
        public int? GenderId { get; set; } // Cinsiyet (Id)
        public string? PhoneNumber { get; set; } // Telefon Numarası
        public string? EmailAddress { get; set; } // Mail Adresi
        public int? MaritalStatusId { get; set; } // Medeni Durum (Id)
        public int? EducationStatusId { get; set; } // Öğrenim Durumu (Id)
        public int? CityOfResidenceId { get; set; } // Yaşadığı Şehir (Id)
        public int? BloodTypeId { get; set; } // Kan Grubu (Id)
        public int? ProfessionId { get; set; } // Meslek (Id)
        public string? Allergies { get; set; } // Alerjiler
        public string? RegularMedications { get; set; } // Sürekli Kullanılan İlaçlar
        public string? MedicalHistory { get; set; } // Tıbbi Geçmiş
        public int? PrimaryRelationId { get; set; } // 1. Yakınlık Durumu (Id)
        public int? SecondaryRelationId { get; set; } // 2. Yakınlık Durumu (Id)
        public string? PrimaryContactFullName { get; set; } // 1. Yakınlık Ad Soyad
        public string? SecondaryContactFullName { get; set; } // 2. Yakınlık Ad Soyad
        public string? PrimaryContactPhone { get; set; } // 1. Yakın Telefon
        public string? SecondaryContactPhone { get; set; } // 2. Yakın Telefon
        public string? Url { get; set; } // Url Link ekleyebilir
        public string? Description { get; set; } // Bireysel Not olarak da kullanılabilir 
        public IFormFile? File { get; set; }

    }
}
