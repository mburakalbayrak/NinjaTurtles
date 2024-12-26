using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class QrCodeAnimalDetail : BaseEntity<int>, IEntity
    {
        public Guid QrMainId { get; set; }
        public string OwnerName { get; set; } // Sahip Adı
        public string OwnerPhoneNumber { get; set; } // Sahip Telefon Numarası
        public string AnimalName { get; set; } // Hayvanın Adı
        public string? SpeciesName { get; set; } // Tür
        public string? BreedName { get; set; } // Cins
        public DateTime? DateOfBirth { get; set; } // Yaş
        public int? GenderId { get; set; } // Cinsiyet
        public string? ColorPattern { get; set; } // Renk / Desen
        public int? VaccinationStatusId { get; set; } // Aşı Durumu
        public string? IdentificationOrMicrochipNumber { get; set; } // Kimlik / Mikroçip No
        public int? OwnershipStatusId { get; set; } // Sahiplik Durumu
        public int? HealthStatusId { get; set; } // Sağlık Durumu
        public int? NutritionStatusId { get; set; } // Beslenme Durumu
        public string? Allergies { get; set; } // Alerjiler (Ilac, gıda vb)
        public string? RegularMedications { get; set; } // Sürekli Kullanılan İlaçlar
    }
}
