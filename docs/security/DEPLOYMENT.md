# Bezpieczne uruchomienie TakeCARE

Przed uruchomieniem skonfiguruj osobne środowiska, wstrzyknij wymagane sekrety i wykonaj opisane testy.

## Środowisko lokalne

1. Zainstaluj .NET SDK 10.0.403 lub zgodny nowszy patch z global.json i Node.js 24 LTS.
2. Uruchom z katalogu repozytorium:
   ```powershell
   pwsh ./scripts/Initialize-LocalSecrets.ps1
   dotnet dev-certs https --trust
   dotnet restore TakeCare.sln
   dotnet test TakeCare.SecurityTests/TakeCare.SecurityTests.csproj -c Release
   dotnet run --project TakeCare --launch-profile https
   ```
   Skrypt generuje nowy losowy klucz podpisu i zapisuje konfigurację w User Secrets. Stare tokeny nie są zgodne z nowym kluczem i formatem. User Secrets nie są szyfrowanym sejfem i służą wyłącznie do developmentu.
3. W drugim terminalu:
   ```powershell
   Set-Location TakeCare/ClientApp
   npm ci
   npm run build
   npm run dev
   ```
   Otwórz https://localhost:3000. Używaj HTTPS również lokalnie: sesja i CSRF mają Secure oraz prefiks __Host-. Vite przekazuje /api do https://localhost:7258. Wyłączenie weryfikacji certyfikatu upstream w vite.config.ts dotyczy tylko lokalnego developmentu.
4. Baza musi już istnieć lub zostać odtworzona na danych testowych. Zmiana frameworka nie uruchamia migracji automatycznie. Migracja 20261031193000_StoreExaminationPdf dodaje opcjonalne pole PDF oraz unikalny indeks jednego badania na wizytę. Przed migracją zrób chronioną kopię i sprawdź na staging duplikaty kwerendą SELECT VisitId, COUNT_BIG(*) FROM dbo.ExaminationTable GROUP BY VisitId HAVING COUNT_BIG(*) > 1. Przy duplikatach migracja zatrzyma się bez ich usuwania: uzgodnij naprawę danych z właścicielem. Po weryfikacji zastosuj migrację osobną, uprawnioną tożsamością. Automatyczny rollback tej migracji usuwa zapisane pliki PDF; nie uruchamiaj go bez kopii i planu odtworzenia.

## Produkcja

- Przenieś ustawienia z .env.example do środowiska usługi lub sejfu sekretów. Plik .env nie jest automatycznie odczytywany przez .NET; nie kopiuj prawdziwych wartości do plików śledzonych przez Git.
- Obowiązkowe sekrety: ConnectionStrings__defaultConnection oraz Jwt__SigningKeyBase64. Klucz JWT: co najmniej 32 bajty z generatora kryptograficznego, zakodowane Base64; zalecane 64 bajty. Brak/niepoprawna konfiguracja zatrzymuje start aplikacji.
- Ustaw ASPNETCORE_ENVIRONMENT=Production i konkretne AllowedHosts. Jwt__Issuer i Jwt__Audience muszą odpowiadać wartościom z aplikacji; rozdziel klucze dev/staging/prod.
- SQL: Encrypt=True, TrustServerCertificate=False, Persist Security Info=False. Zainstaluj zaufany certyfikat serwera. Konto aplikacyjne powinno mieć minimalne uprawnienia; osobna tożsamość do migracji, bez sysadmin/db_owner dla runtime.
- Wystaw SPA i API pod jednym originem HTTPS. Serwuj zawartość ClientApp/dist statycznie i kieruj /api do API przez HTTPS. Nie uruchamiaj serwera deweloperskiego Vite w produkcji. CORS nie jest potrzebny dla tego układu.
- Przy terminacji TLS na proxy skonfiguruj zaufane proxy oraz forwarded headers zgodnie z dokumentacją hostingu, albo użyj TLS również do Kestrela. Nie akceptuj X-Forwarded-For od dowolnego klienta. Obecny limiter działa na IP widzianym przez Kestrel; za proxy bez tej konfiguracji klienci współdzielą limit. Ustaw też limity rozmiaru i prób logowania na brzegu sieci.
- Persistuj i zabezpiecz klucze ASP.NET Data Protection poza repozytorium; przy wielu instancjach współdziel chroniony key ring. Służy on m.in. tokenom CSRF.
- Nie zapisuj body żądań, cookie, Authorization, parametrów zapytań, wyników badań, PESEL ani pełnych wyjątków dostawców w logach proxy/APM. W aplikacji zostają typ błędu i identyfikator korelacji. Log audytowy dostępu do dokumentacji wymaga osobnego projektu z kontrolą dostępu i retencją.
- Email__Enabled=false domyślnie. Przed włączeniem wstrzyknij świeże Email__Public, Email__Secret i zweryfikowane Email__FromEmail. Public oznacza identyfikator konta Mailjet, nie zgodę na publikację. Wiadomości zawierają dane medyczne; przed aktywacją ustal dopuszczalny kanał dostarczania i politykę załączników. Preferuj bezpieczny portal zamiast wysyłania wyników wprost e-mailem.

## Zmiana uprawnień i kontraktów

- Anonimowe są tylko: GET /api/Login/Csrf, POST /api/Login/Login i POST /api/User/AddPatient. POST-y wymagają poprawnego tokenu CSRF.
- Konta personelu tworzy wyłącznie Admin. Publiczna rejestracja zawsze przypisuje Patient, niezależnie od Role w żądaniu.
- Nie ma publicznego endpointu nadawania Admin. Pierwszą rolę administratora nadaj ręcznie zaufanemu, zweryfikowanemu kontu przez kontrolowaną operację administracyjną na bazie. Najpierw sprawdź historię kont personelu utworzonych przy starej publicznej rejestracji. Nie awansuj automatycznie wszystkich starych lekarzy/recepcjonistów.
- Pacjent ma dostęp do swoich wizyt i badań; lekarz do własnych wizyt i wystawionych przez siebie badań; recepcjonista może rezerwować i anulować niezrealizowane wizyty. Recepcjonista nie otrzymuje ogólnego dostępu do wyników. To konserwatywna polityka startowa; zmiany wymagają jawnej specyfikacji i testów.
- Żądane e-maile/identyfikatory nie wystarczają do autoryzacji. Serwer sprawdza relacje do konta z sub w podpisanym JWT.
- Aktualizacje profilu zmieniają tylko białą listę pól. Legacy DTO nadal wymaga części pól rejestracyjnych; Password, Email, Role, PESEL i identyfikatory nie zmieniają danych konta. Osobny proces zmiany hasła/e-maila z ponownym uwierzytelnieniem pozostaje do zaprojektowania.
- Logowanie używa cookie __Host-takecare-session: Secure, HttpOnly, SameSite=Strict, Path=/, bez Domain, ważność do 15 minut. Frontend pobiera token CSRF przed każdym zapisem. Przeglądarka nie powinna przechowywać JWT w localStorage.
- Wylogowanie usuwa cookie. Skopiowany JWT może pozostać ważny do końca krótkiego TTL; nie wdrożono trwałej listy unieważnień jti ani refresh tokenów. Rotacja klucza natychmiast odcina wszystkie stare podpisy po przeładowaniu wszystkich instancji.
- Załącznik musi być PDF <=5 MiB z nagłówkiem %PDF-. PDF jest zapisywany w bazie razem z badaniem i pobierany przez GET /api/Examination/{id}/pdf tylko przez lekarza lub pacjenta przypisanego do badania. Starsze badania nie mają PDF i zwracają 404. Skonfiguruj szyfrowanie bazy i kopii zapasowych (np. TDE lub szyfrowanie dysku z zarządzanymi kluczami), dostęp ograniczony do kont serwisowych i politykę retencji: aplikacja nie dodaje osobnej warstwy szyfrowania kolumny. Podpis pliku nie zastępuje skanera antywirusowego ani sanitizacji aktywnego PDF. Zewnętrzne ścieżki/URL-e Images z klienta są odrzucane. Przed obsługą niezaufanych dokumentów wdroż kwarantannę i skaner.
- Niepowodzenie e-maila po zapisie badania zwraca 201 z emailDelivery=failed; zapis nie jest ponawiany automatycznie. Kolejka/outbox i retry pozostają do wdrożenia przed wymaganiem gwarantowanego dostarczania.

## Weryfikacja przed udostępnieniem

```powershell
python ./scripts/check_configuration.py
dotnet test TakeCare.SecurityTests/TakeCare.SecurityTests.csproj -c Release
dotnet list TakeCare.sln package --vulnerable --include-transitive --no-restore
Set-Location TakeCare/ClientApp
npm ci
npm run build
npm audit
```

Dodatkowo przetestuj prawdziwy SQL Server i certyfikaty, ścieżkę HTTPS przez docelowe proxy, logowanie/cookie/CSRF w przeglądarce oraz Mailjet na odrębnym koncie testowym. Testy integracyjne dostarczone z pakietem używają EF InMemory i nie potwierdzają zachowania SQL Server, dostarczenia e-maila ani konfiguracji istniejącego wdrożenia.

Indeks unikatowy VisitId ogranicza równoczesne utworzenie dwóch badań dla jednej wizyty; przygotowano test generowania SQL i testy InMemory, ale nie wykonano testu równoległego zapisu na działającym SQL Server. Taką próbę wykonaj na staging przed produkcją. Unikalność User.Email nie jest egzekwowana indeksem: pole ma typ nvarchar(max), a migracja musi uwzględnić długości danych i kolację. Nie usuwaj istniejących duplikatów automatycznie.
