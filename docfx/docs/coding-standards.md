# MagFlow – Zasady kodowania (C#/.NET)

**Cel:** spójny, czytelny i bezpieczny kod, który łatwo utrzymywać i rozwijać.

## 1. Nazewnictwo i organizacja
- **Projekty/warstwy:** `MagFlow.{Domain|DAL|EF|BLL|API|Web|Shared}`.
- **Pliki:** jeden typ publiczny na plik; nazwa pliku = nazwa typu.
- **Klasy/Metody/Właściwości:** PascalCase.  
- **Pola prywatne:** `_camelCase`.
- **Zmienne:** `camelCase`. Zmienne statyczne: PascalCase.
- **Stałe:**: PASCAL_CASE
- **Async:** metody kończymy sufiksem `Async` (np. `GetOrdersAsync`).
- **Testy:** `MethodName_Should_DoX_When_Y`.

## 2. Styl C#
- Preferuj `var` gdy typ jest oczywisty; w pozostałych miejscach pisz jawnie typ.
- **Zawsze klamry** nawet dla jednolinijkowych `if/for`.
- Używaj `switch expression` i pattern matching, gdy upraszcza kod.
- Nullability **włączone** (`<Nullable>enable</Nullable>`). Nie wyłączaj lokalnie bez powodu.
- `readonly` dla pól/struktur, `record` dla niemutowalnych DTO.

## 3. Parametry, błędy, kontrakty
- Waliduj wejście **na brzegach** (API, handlery, publiczne metody).  
  Stosuj `ArgumentNullException.ThrowIfNull(...)`, **guard clauses**.
- Nie używaj wyjątków do sterowania logiką. Rzucaj tylko w sytuacjach wyjątkowych.

## 4. Asynchroniczność
- Cała ścieżka I/O jest **async** end-to-end. Nie mieszaj sync/async.
- Nigdy `Task.Result`/`.Wait()` w kodzie produkcyjnym (deadlocki).
- Anulowanie: obsługuj `CancellationToken` w API/BLL/DAL.

## 5. DI i konfiguracja
- **Konstruktor injection**; zero service locatorów, zero statycznych singletons (poza czystymi helperami).
- Rejestracje w rozszerzeniach np. `services.AddMagFlowBll()`.
- Konfiguracje przez `IOptions<T>`; sekcje w `appsettings.json` + walidacja `ValidateDataAnnotations()`.

## 6. Logowanie i monitoring
- Serilog jako logger. Poziomy:  
  - `Information` – ścieżki biznesowe,  
  - `Warning` – sytuacje wyjątkowe, ale obsłużone,  
  - `Error` – błędy wymagające uwagi.  
- Nie loguj danych wrażliwych / haseł / tokenów. Maskuj PII.
- **Structured logging** (właściwości, nie konkatenacje stringów).

## 7. EF Core i dostęp do danych
- **Brak logiki domenowej w EF**.  
- Context **scoped**; brak statycznych kontekstów.
- Query: **AsNoTracking** dla odczytów; paginacja zawsze przy listach.
- Transakcje tylko gdy potrzebne; preferuj `SaveChangesAsync` z agregacją zmian.
- Migracje wersjonowane; nie uruchamiaj `Database.Migrate()` w każdym żądaniu.

## 8. API (ASP.NET Core)
- Endpoints zwracają **wyniki typowane** (np. `ActionResult<T>`).  
- Walidacja modelu (`[ApiController]` -> 400 z automatu), dodatkowe reguły w FluentValidation.
- Kody statusów: 200/201/204/400/401/403/404/409/422/500 – używaj świadomie.
- Kontrakty DTO = osobne typy; **nie** eksponuj encji EF.

## 9. Web (Blazor/Frontend)
- Komponenty w `MagFlow.Web/Components` (nazwy: PascalCase).  
- Logika UI w code-behind `.razor.cs` gdy rośnie złożoność.
- **Zależności:** `MagFlow.Web` referuje `MagFlow.BLL` (i ewentualnie kontrakty `MagFlow.Domain` / `MagFlow.Shared`). 
- **DI:** komponenty i `@code` wstrzykują **fasady/aplikacyjne serwisy** z BLL (np. `IOrdersAppService`).  
  Rejestracje w rozszerzeniach: `services.AddMagFlowBll()`.
- **Granica odpowiedzialności:**  
  - Blazor = prezentacja + orkiestrowanie UI (bez reguł biznesowych).  
  - BLL = przypadki użycia, transakcje, walidacje domenowe.  
  - EF/DAL = tylko persystencja.
- **Modele:** komponenty używają **ViewModeli/DTO** z BLL (UI-friendly).  
  Mapowanie: w BLL (np. Mapster/AutoMapper), **nie** w komponentach.
- **Asynchroniczność:** wywołania do BLL zawsze `async` z `CancellationToken` (anuluj na `DisposeAsync` komponentu).
- **Stan i performance:**  
  - Dłuższe operacje: `Task.Run` **nie** w UI; BLL decyduje o równoległości.  
  - Paginacja/filtry realizuje BLL (Web tylko przekazuje parametry).  
  - Unikaj „ciężkich” `StateHasChanged()` w pętlach; batchuj aktualizacje.

## 10. Testy
- Testy jednostkowe dla logiki domenowej i usług aplikacyjnych.  
- Jeden `Assert` na koncept lub `Assert.Multiple`.  
- Izoluj zewnętrzności (bazy, I/O) przez interfejsy i mocki.  
- Testy integracyjne dla API z `WebApplicationFactory`.

## 11. Bezpieczeństwo
- Zawsze `https`.  
- Autoryzacja przez role/claims; **zasady** (policies) > gołe role.  
- Waliduj wejście przeciwko **injection** (SQL, logs, headers).
- Sekrety przez **Secret Manager**/KeyVault, nie w repo.

## 12. Dokumentacja i komentarze
- Komentarze `///` tylko dla publicznego API i trudnych miejsc.  
- Opis systemu, decyzje arch. (ADR) i przykłady w `docfx/docs`.
- DocFX generuje referencję – ten dokument to **zasady** ponad auto-doc.

## 13. Git, wersjonowanie i PR

### Wersjonowanie
  Przykład: `1.3.5.12`  
  - `major` – zmiany przełomowe, łamiące kompatybilność,  
  - `minor` – nowe funkcjonalności, zgodne wstecznie,  
  - `patch` – poprawki błędów, zmiany drobne,  
  - `build` – numer builda
  
### Commit messages
- Każdy commit **zaczyna się od numeru wersji**.  
- Commitujemy **całe funkcjonalności**, a nie pojedyncze pliki.  
- Nazwa commita opisuje **co zostało dodane / zmienione** w ramach funkcjonalności.  
- Jeśli opis nie mieści się w tytule → dodaj szczegóły w opisie (body commita).  

---

**Zmiany w zasadach:** PR do pliku `docfx/docs/Coding-Standards.md`, oznacz reviewerów z zespołu.
