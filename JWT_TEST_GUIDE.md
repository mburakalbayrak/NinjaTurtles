## JWT Test Adýmlarý

### 1. Uygulamayý Baþlat
- Visual Studio'da F5 veya `dotnet run`
- Swagger: https://localhost:{port}/swagger

### 2. Login Endpoint'ini Çaðýr
```json
POST /api/auth/login
{
  "email": "burakalbayrak@gmail.com",
  "password": "your-password"
}
```

**Beklenen Yanýt:**
```json
{
  "token": "eyJhbGci...",
  "expiration": "2025-12-21T15:11:00",
  "refreshToken": "base64-string",
  "refreshTokenExpiration": "2025-12-28T14:41:00"
}
```

### 3. Token'ý Kopyala
- `token` field'ýndaki deðeri kopyala

### 4. JWT.io'da Test Et

**URL:** https://jwt.io

**Adýmlar:**
1. Sol taraftaki "Encoded" kutusuna token'ý yapýþtýr
2. Sað tarafta "VERIFY SIGNATURE" bölümünde:
   - Algorithm: `HS256` (varsayýlan)
   - Secret: `K7mP9nQ2rT5vW8xZ!aB#cD$eF%gH&jK*mN+pR-sU=vW@yZ`

**Beklenen Sonuç:**
```
? Signature Verified
```

**Decoded Payload Görmek Ýstediðin:**
```json
{
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "4",
  "email": "burakalbayrak@gmail.com",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "Burak Albayrak",
  "nbf": 1234567890,
  "exp": 1234569690,
  "iss": "www.karekodla.com",
  "aud": "www.karekodla.com"
}
```

### 5. Swagger'da Authorize

1. Swagger'da sað üstteki **"Authorize"** butonuna týkla
2. `Bearer {token}` yaz (Bearer kelimesinden sonra boþluk!)
3. **Authorize** butonuna týkla
4. **Close** ile kapat

### 6. Protected Endpoint'i Test Et

```
GET /api/company/getList
```

**Beklenen Yanýt:**
```
200 OK
```

**Eðer 401 alýrsan:**
- Token doðru kopyalandý mý?
- Bearer kelimesinden sonra boþluk var mý?
- Uygulama yeniden baþlatýldý mý?
- appsettings.json doðru mu?

---

## Sorun Giderme

### "Invalid Signature" Hatasý
? **Çözüm:** Doðru secret'ý girdiðinden emin ol:
```
K7mP9nQ2rT5vW8xZ!aB#cD$eF%gH&jK*mN+pR-sU=vW@yZ
```

### "401 Unauthorized" Hatasý
? **Çözüm:** 
1. Swagger'da Authorize yaptýn mý?
2. `Bearer ` ön eki var mý?
3. Token'ýn süresi dolmadý mý? (30 dakika)

### "Token Süresi Doldu"
? **Çözüm:** Tekrar login ol veya refresh token kullan:
```json
POST /api/auth/refresh-token
{
  "refreshToken": "your-refresh-token"
}
```

---

## Güvenlik Kontrol Listesi

- ? SecurityKey 43 karakter (344 bit)
- ? ValidateLifetime = true
- ? Access Token: 30 dakika
- ? Refresh Token: 7 gün
- ? [Authorize] attribute kullanýlýyor
- ? HTTPS zorunlu (production'da)
- ? Signature doðrulanýyor

