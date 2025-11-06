using Newtonsoft.Json;

namespace NinjaTurtles.Entities.Dtos
{
    public class QrCodeDetailDto
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Empty { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? DetailTypeId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? RedirectUrl { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QrCodeHumanDetailDto? HumanDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QrCodeAnimalDetailDto? AnimalDetail { get; set; }
    }

    public class QrCodeHumanDetailDto
    {
        public int Id { get; set; } 
        public string? FullName { get; set; } // Ad Soyad
        public DateTime? DateOfBirth { get; set; } // Doğum Tarihi
        public string? Gender { get; set; } // Cinsiyet (Id)
        public string? PhoneNumber { get; set; } // Telefon Numarası
        public string? EmailAddress { get; set; } // Mail Adresi
        public string? MaritalStatus { get; set; } // Medeni Durum (Id)
        public string? EducationStatus { get; set; } // Öğrenim Durumu (Id)
        public string? CityOfResidence { get; set; } // Yaşadığı Şehir (Id)
        public string? BloodType { get; set; } // Kan Grubu (Id)
        public string? Profession { get; set; } // Meslek (Id)
        public string? Allergies { get; set; } // Alerjiler
        public string? RegularMedications { get; set; } // Sürekli Kullanılan İlaçlar
        public string? MedicalHistory { get; set; } // Tıbbi Geçmiş
        public string? PrimaryRelation { get; set; } // 1. Yakınlık Durumu (Id)
        public string? SecondaryRelation { get; set; } // 2. Yakınlık Durumu (Id)
        public string? PrimaryContactFullName { get; set; } // 1. Yakınlık Ad Soyad
        public string? SecondaryContactFullName { get; set; } // 2. Yakınlık Ad Soyad
        public string? PrimaryContactPhone { get; set; } // 1. Yakın Telefon
        public string? SecondaryContactPhone { get; set; } // 2. Yakın Telefon
        public string? Url { get; set; } // Url Link ekleyebilir
        public bool RedirectUrl { get; set; } // Url Link ekleyebilir
        public string? Description { get; set; } // Bireysel Not olarak da kullanılabilir 
        public string? ProfilePictureData { get; set; } 
    }


    public class QrCodeAnimalDetailDto
    {
        public int Id { get; set; }
        public string OwnerName { get; set; } // Sahip Adı
        public string OwnerPhoneNumber { get; set; } // Sahip Telefon Numarası
        public string AnimalName { get; set; } // Hayvanın Adı
        public string? SpeciesId { get; set; } // Tür
        public string? BreedName { get; set; } // Cins
        public DateTime? DateOfBirth { get; set; } // Yaş
        public string? GenderName { get; set; } // Cinsiyet
        public string? ColorPattern { get; set; } // Renk / Desen
        public string? VaccinationStatusName { get; set; } // Aşı Durumu
        public string? IdentificationOrMicrochipNumber { get; set; } // Kimlik / Mikroçip No
        public string? OwnershipStatusName { get; set; } // Sahiplik Durumu
        public string? HealthStatusName { get; set; } // Sağlık Durumu
        public string? NutritionStatusName { get; set; } // Beslenme Durumu
        public string? Allergies { get; set; } // Alerjiler (Ilac, gıda vb)
        public string? RegularMedications { get; set; } // Sürekli Kullanılan İlaçlar
    }
}
