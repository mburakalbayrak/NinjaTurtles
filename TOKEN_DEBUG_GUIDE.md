## JWT Token Debugging Script

### 1. Login Yap ve Response'u Kaydet

Swagger'da:
1. `/api/auth/login` endpoint'ini aç
2. Email/password gir
3. **Execute** butonuna týkla
4. **Response body**'yi TAM olarak kopyala

Örnek doðru response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQiLCJlbWFpbCI6ImJ1cmFrYWxiYXlyYWtAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkJ1cmFrIEFsYmF5cmFrIiwibmJmIjoxNzM0Nzk4MTk0LCJleHAiOjE3MzQ3OTk5OTQsImlzcyI6Ind3dy5rYXJla29kbGEuY29tIiwiYXVkIjoid3d3LmthcmVrb2RsYS5jb20ifQ.YQgOjjo34OHRleiNsSI9A-_M5dTCOxno7VA31wCAPUE",
  "expiration": "2025-12-21T20:06:34.4285625+03:00",
  "refreshToken": "base64-random-string",
  "refreshTokenExpiration": "2025-12-28T19:36:34.4285625+03:00"
}
```

### 2. Token Formatýný Kontrol Et

Token'da **kaç tane nokta** var?
```bash
# PowerShell'de
$token = "eyJhbGci..."  # Senin token'ýn
($token.ToCharArray() | Where-Object {$_ -eq '.'}).Count
```

**Beklenen:** 2 (iki nokta)
**Eðer 1 veya 0 ise:** Token kesilmiþ veya yanlýþ kopyalanmýþ

### 3. Token Uzunluðunu Kontrol Et

```bash
$token.Length
```

**Beklenen:** ~400-600 karakter (rolleregöre deðiþir)
**Eðer çok kýsa ise (<100):** Token kesilmiþ

### 4. Swagger'da Authorize

1. Swagger sað üstte **?? Authorize** butonuna týkla
2. **Value** kutusuna: `Bearer {token}` yaz
   - `Bearer` kelimesinden sonra **BOÞLUK** olmalý
   - Token'ýn tamamýný yapýþtýr (baþýnda/sonunda boþluk olmamalý)
3. **Authorize** butonuna týkla
4. **Close** ile kapat

### 5. Test Et

```
GET /api/company/getList
```

### 6. Eðer Hala 401 Alýrsan

**Debug Checklist:**

- [ ] Token TAM olarak kopyalandý mý? (baþý-sonu kesilmemiþ)
- [ ] `Bearer ` öneki var mý? (boþluklu)
- [ ] Token'da 2 nokta var mý?
- [ ] Login response'u baþarýlý mý? (200 OK)
- [ ] Uygulama yeniden baþlatýldý mý?
- [ ] appsettings.json doðru SecurityKey içeriyor mu?

### 7. Manuel Test (Postman/cURL)

**Postman:**
```
GET https://localhost:7001/api/company/getList
Headers:
  Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**cURL:**
```bash
curl -X GET "https://localhost:7001/api/company/getList" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### 8. Log Kontrolü

```powershell
# Son 20 satýrý göster
Get-Content "C:\Users\Bureda\Documents\Projeler\NinjaTurtles\NinjaTurtles.WebApi\Logs\log-*.txt" -Tail 20
```

**Aramak istediðin hatalar:**
- `JWT is not well formed` ? Token kesilmiþ
- `signature verification failed` ? SecurityKey uyumsuzluðu
- `token expired` ? Token süresi dolmuþ
- `Authorization header missing` ? Header gönderilmemiþ

---

## Yaygýn Hatalar ve Çözümleri

| Hata | Neden | Çözüm |
|------|-------|-------|
| `only one dot (.)` | Token kesilmiþ | Token'ýn tamamýný kopyala |
| `signature verification failed` | Yanlýþ SecurityKey | Uygulamayý yeniden baþlat |
| `token expired` | Token süresi dolmuþ (>30 dk) | Tekrar login ol |
| `Authorization header missing` | Header gönderilmemiþ | Swagger'da Authorize yap |
| `401 Unauthorized` | Genel auth hatasý | Yukarýdaki tüm adýmlarý kontrol et |

---

## Son Kontrol

Þu komutu çalýþtýr ve çýktýyý paylaþ:

```powershell
# Login response'unu kaydet (Swagger'dan kopyala)
$loginResponse = @"
{paste-login-response-here}
"@ | ConvertFrom-Json

# Token bilgilerini göster
Write-Host "Token Length: $($loginResponse.token.Length)"
Write-Host "Dot Count: $(($loginResponse.token.ToCharArray() | Where-Object {$_ -eq '.'}).Count)"
Write-Host "Token Starts With: $($loginResponse.token.Substring(0, 30))..."
Write-Host "Token Ends With: ...$($loginResponse.token.Substring($loginResponse.token.Length - 30))"
```

Bu çýktýyý paylaþýrsan tam olarak neyin yanlýþ olduðunu görebilirim.
