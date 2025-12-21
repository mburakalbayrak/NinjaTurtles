# JWT Authentication Yapýsý Güncellemeleri

## ? Yapýlan Deðiþiklikler

### 1. **Refresh Token Mekanizmasý Eklendi**
- Access token süresi 100 dakikadan **30 dakikaya** düþürüldü (güvenlik artýþý)
- Refresh token süresi **7 gün (10080 dakika)** olarak ayarlandý
- Refresh token ile yeni access token alma özelliði eklendi

### 2. **Veritabaný Deðiþiklikleri**
**User** tablosuna eklenen yeni kolonlar:
- `RefreshToken` (nvarchar(500), nullable) - Kullanýcýnýn refresh token'ý
- `RefreshTokenExpiry` (datetime2, nullable) - Refresh token'ýn son kullanma tarihi

**Migration Dosyasý:** `NinjaTurtles.DataAccess\Migrations\20250101000000_AddRefreshTokenToUser.cs`

### 3. **Yeni API Endpoint'leri**

#### **POST /api/auth/refresh-token**
Refresh token ile yeni access token almak için kullanýlýr.

**Request:**
```json
{
  "refreshToken": "your-refresh-token-here"
}
```

**Response (Success):**
```json
{
  "token": "new-jwt-access-token",
  "expiration": "2025-01-01T12:30:00",
  "refreshToken": "new-refresh-token",
  "refreshTokenExpiration": "2025-01-08T12:00:00"
}
```

#### **POST /api/auth/logout** [Authorize gerekli]
Kullanýcýnýn refresh token'ýný iptal eder.

**Request:**
```json
{
  "refreshToken": "your-refresh-token-here"
}
```

**Response (Success):**
```json
"Token iptal edildi"
```

### 4. **Login ve Register Davranýþý**
Her baþarýlý login ve register iþleminde artýk hem **access token** hem de **refresh token** döner:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-01-01T12:30:00",
  "refreshToken": "base64-encoded-random-token",
  "refreshTokenExpiration": "2025-01-08T12:00:00"
}
```

### 5. **JWT Claims Yapýsý (Mevcut - Korundu)**
JWT içinde þu bilgiler bulunur:
- **NameIdentifier**: User ID
- **Email**: Kullanýcý email
- **Name**: FirstName + LastName
- **Role**: Kullanýcýnýn rolleri (OperationClaims)

## ?? Güvenlik Ýyileþtirmeleri

1. **Short-lived Access Tokens**: Access token'lar kýsa süreli (30 dk), çalýnma durumunda zarar minimize edilir
2. **Refresh Token Rotation**: Her refresh iþleminde yeni token üretilir
3. **Token Revocation**: Logout ile token iptal edilebilir
4. **Secure Random Generation**: Refresh token'lar kriptografik güvenli random ile üretilir

## ?? Veritabaný Migration Uygulama

Migration'ý uygulamak için aþaðýdaki SQL komutunu çalýþtýrýn:

```sql
ALTER TABLE [User] 
ADD [RefreshToken] nvarchar(500) NULL,
    [RefreshTokenExpiry] datetime2 NULL;
```

**VEYA** Entity Framework ile:

```bash
dotnet ef database update --project NinjaTurtles.DataAccess --startup-project NinjaTurtles.WebApi
```

## ?? Kullaným Senaryosu

### 1. Login
```http
POST /api/auth/login
{
  "email": "user@example.com",
  "password": "password123"
}
```
**Yanýt:** Access Token + Refresh Token

### 2. API Çaðrýlarý
```http
GET /api/protected-endpoint
Authorization: Bearer {access-token}
```

### 3. Access Token Süresi Dolduðunda
```http
POST /api/auth/refresh-token
{
"refreshToken": "{refresh-token}"
}
```
**Yanýt:** Yeni Access Token + Yeni Refresh Token

### 4. Logout
```http
POST /api/auth/logout
Authorization: Bearer {access-token}
{
  "refreshToken": "{refresh-token}"
}
```

## ?? Gelecek Ýyileþtirmeler (Öneriler)

1. **Token Blacklist**: Redis ile iptal edilen token'larý saklamak
2. **Multi-Device Support**: Ayný kullanýcýnýn birden fazla cihazda oturum açabilmesi
3. **Activity Logging**: Token kullanýmlarýnýn loglanmasý
4. **Rate Limiting**: Refresh token endpoint'ine rate limit eklenmesi
5. **Email Verification**: Email doðrulamasý zorunluluðu

## ?? Notlar

- Refresh token'lar veritabanýnda saklanýyor (güvenli)
- Her refresh iþleminde hem access hem refresh token yenileniyor
- Token'lar base64 encoded 64-byte random deðerler
- Role desteði mevcut ve geliþtirilmeye hazýr
- JWT validation Program.cs'de yapýlandýrýlmýþ durumda
