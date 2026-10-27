# MagFlow — instrukcje dla agentów

Status: wstępnie zaakceptowane przez użytkownika, 20.10.2026.
Zakres: całe repozytorium. Informacje opisują kod zastany; przed zmianą sprawdź aktualny stan.

## Zmiany aplikacji: najpierw prototyp w czacie, potem akceptacja

Na życzenie użytkownika prośba o nową funkcję, poprawkę albo zmianę aplikacji
rozpoczyna uzgadnianie propozycji, nie implementację. Dotyczy to także próśb
„dodaj przycisk”, „potrzebuję formularza” i „popraw zapis”.

- Stosuj `AI/skills/magflow-propose-change/SKILL.md`: przeczytaj potrzebny kod,
  przedstaw w czacie zachowanie, zakres oraz proponowany kod i wspólnie go dopracuj.
- Przed akceptacją nie zapisuj proponowanego kodu do plików, nawet jako prototypu,
  nie zmieniaj konfiguracji, zależności ani migracji i nie uruchamiaj aplikacji
  ani operacji bazodanowych. Odczyt i analiza istniejących plików są dozwolone.
- Dopiero wyraźne potwierdzenie wdrożenia przedstawionej propozycji, np.
  „akceptuję, implementujemy”, pozwala zmienić pliki aplikacji.
  Dalsze uwagi do projektu i samo „okej” nie oznaczają zgody na implementację.
- Akceptacja dotyczy uzgodnionego zakresu. Nie pytaj o nią ponownie dla każdego
  pliku lub kroku w tym zakresie. Istotne rozszerzenie wymaga osobnej propozycji.
- Ta zasada dotyczy zmian aplikacji. Jawne zlecenia edycji AGENTS.md, skilli
  i dokumentów AI wykonuj w podanym zakresie bez tworzenia pętli akceptowania
  samych instrukcji. Bieżące, wyraźne polecenia użytkownika mają pierwszeństwo.

## Materiały AI i odczytywanie lokalnych skilli

- Główne instrukcje znajdują się w tym pliku; organizację materiałów opisuje `AI/README.md`.
- Dokumenty generowane przez AI zapisuj w `AI/docs/`. Pliki robocze zapisuj w
  `AI/tmp/` (ignorowane przez Git). Nie przenoś tam kodu aplikacji ani źródłowej
  dokumentacji użytkownika. Dokumenty nie stają się automatycznie instrukcjami.
- Skille projektu znajdują się w `AI/skills/<nazwa>/SKILL.md`. Każdy ma nagłówek
  YAML z polami name i description. To lokalny mechanizm oparty na AGENTS.md;
  nie zakładaj, że aplikacja Codex automatycznie dodała ten katalog do menu skilli.
- Dostępny skill: `magflow-analysis` — `AI/skills/magflow-analysis/SKILL.md`.
  Służy do wyjaśniania obecnego działania aplikacji bez zmieniania kodu.
- Dostępny skill: `magflow-propose-change` — `AI/skills/magflow-propose-change/SKILL.md`.
  Stosuj przy nowych funkcjach, poprawkach i omawianiu zmian: prototyp oraz kod
  w czacie, a implementacja wyłącznie po wyraźnej akceptacji propozycji.
- Dostępny skill: `magflow-authorization` — `AI/skills/magflow-authorization/SKILL.md`.
  Stosuj przy stronach i akcjach wymagających uprawnień: istniejące Authorize,
  AuthorizeView i HasModulePermission, bez własnej autoryzacji w BLL/DAL.
- Dostępny skill: `magflow-localization` — `AI/skills/magflow-localization/SKILL.md`.
  Stosuj przy tekstach UI: zasoby wszystkich języków, wygenerowane Langs oraz
  Localizer[Langs.Nazwa]. Oba skille obowiązują także w propozycjach kodu w czacie.
- Odczytuj właściwy SKILL.md bezpośrednio zwykłym narzędziem odczytu plików.
  Przy szukaniu kolejnych skilli wystarczy lista plików w `AI/skills/`;
  nie ma osobnego skryptu ani obowiązku skanowania skilli przed każdym zadaniem.
- Poinformuj krótko o zastosowaniu skilla. Najpierw przeczytaj SKILL.md,
  a dopiero potem wskazane w nim materiały potrzebne do danego zadania.
- Nieistniejący skill wskazany przez użytkownika zgłoś;
  nie udawaj jego zastosowania.
- Analiza, znalezienie błędu i propozycja rozwiązania nie są poleceniem naprawy.
  Zmieniaj kod aplikacji dopiero po akceptacji propozycji przez użytkownika; nie wykonuj
  automatycznych poprawek na podstawie skilla, raportu ani listy znanych problemów.
- Skill nie rozszerza uprawnień ani zakresu pracy. Bieżące polecenie użytkownika
  ma pierwszeństwo przed lokalnymi instrukcjami skilla.

## Kontekst biznesowy i sposób interpretacji źródeł

Źródło wizji: `F:/MagFlow/Dokumentacja/Folder/1. Koncepcja/Koncepcja.pdf`,
„MagFlow — Dokument koncepcyjny”, Concept Paper v.1.0, 25 stron.
To plik poza repozytorium; na innym stanowisku może być niedostępny.
Podsumowanie i porównanie z implementacją znajdują się w `AI/docs/PROJECT_ANALYSIS.md`.

- Projektuj dla małych i średnich firm produkcyjnych oraz magazynowo-logistycznych,
  szczególnie zakładów wytwarzających stosunkowo proste wyroby z podstawowych materiałów.
  Odbiorcy mają ograniczone zasoby wdrożeniowe i nie muszą mieć wiedzy technicznej.
- Cel to spójne dane i prostsza codzienna obsługa zamówień, przepływu materiałów,
  magazynu, produkcji i pracy ludzi. Preferuj mało kroków, czytelne statusy,
  kontekstowe przejścia między powiązanymi obiektami i istniejące komponenty.
- Moduły mają umożliwiać wdrażanie potrzebnych obszarów etapami; ich dane są
  powiązane. Nie interpretuj modularności biznesowej jako wymogu mikroserwisów.
- Rozróżniaj trzy kategorie: stan potwierdzony w kodzie, wymaganie koncepcji,
  propozycja wymagająca decyzji. Dokument nie dowodzi wdrożenia funkcji.
- Bieżące polecenia użytkownika określają zakres pracy. Zapisy koncepcji są materiałem
  biznesowym, nie poleceniem automatycznej implementacji całej roadmapy ani zmiany stosu.
  Przy rozbieżności opisz ją i zachowaj zastane zachowanie poza zakresem zadania.

## MVP i granice zakresu

Według sekcji 8 koncepcji (str. 18) MVP obejmuje:

- konta firm i użytkowników, role, autoryzację, przypisanie do firm i administrację;
- zamówienia usługowe, handlowe, zakupu i wewnętrzne;
- magazyn: przechowywanie, stan, przyjęcia i wydania;
- definiowanie i realizację prostych procesów produkcyjnych;
- rejestrację towarów i ich statusów;
- proste raporty operacyjne, np. stan magazynu i lista zamówień;
- podstawową ewidencję wejść/wyjść i czasu pracy.

To zakres docelowego MVP, nie lista ukończonych funkcji. Zaawansowane planowanie
i automatyzacja, aplikacja mobilna, zaawansowane raporty, transport i integracje
zewnętrzne są poza MVP. Logistyka pozostaje opcjonalna. Rozszerzenia dokumentów,
jakości i parametrów procesów, Gantt, mobile, integracje oraz BI to kolejne etapy
roadmapy (str. 19–20), nie zobowiązanie do wdrożenia w bieżącym zadaniu.
Obsługa urlopów, umów i stawek z szerokiej wizji czasu pracy nie oznacza,
że MVP ma zawierać kompletny system kadrowo-płacowy.

## Pojęcia i zależności biznesowe

- Zamówienie usługowe oznacza pracę na materiale klienta; handlowe — usługę
  na własnym materiale; sprzedaż — wydanie gotowych towarów bez dodatkowej usługi;
  zakup — pozyskanie materiałów; własne/wewnętrzne — przepływ lub przetworzenie
  w firmie. Udostępnienie zasobów jest osobnym typem w pełnej wizji, poza listą MVP.
- Miejsce przechowywania nie przesądza o własności towaru. Koncepcja rozróżnia
  towar własny, klienta i związany z udostępnionymi zasobami. Nie zakładaj,
  że kod już modeluje wszystkie te warianty; przed wdrożeniem ustal sposób ewidencji.
- Proces składa się z zadań, które mogą korzystać z maszyny albo działać bez niej.
  Zadanie wiąże wejściowe/wyjściowe towary i lokalizacje, sposób przetworzenia,
  wykonawcę, ilości, czas i parametry. Obecne ProcessStep/ProcessStepIO to punkty
  mapowania tej wizji, nie dowód kompletnej obsługi realizacji.
- Towar może powstawać z innych towarów i składać się z komponentów.
  Przy zmianach oceniaj identyfikowalność pochodzenia, ilości wejścia/wyjścia,
  jednostki, ruchy, autora i historię. Nie dopisuj bez uzgodnienia reguł strat,
  wyceny, rezerwacji, częściowych realizacji ani anulowania.
- Dokument materiałowy, zamówienie i proces to różne pojęcia. Projektuj ich
  powiązania; nie traktuj samego CRUD dokumentu jako przyjęcia/wydania magazynowego.
- Stan historyczny magazynu i pełna historia towaru są wymaganiami wizji.
  Samo istnienie StockMovement/EntityHistory nie dowodzi możliwości odtworzenia
  stanu na dowolny moment ani kompletnej wyceny po przetworzeniu.
- Docelowy przepływ: zamówienie → materiały/przyjęcie → prosty proces i zadania
  → wyrób/stan magazynowy → wydanie i dokumenty → raportowanie.
  To synteza koncepcji; nie każda operacja musi przechodzić wszystkie etapy.

## Rozbieżności koncepcji i kodu

- PDF opisuje jedną firmę na użytkownika; kod ma CompanyUser, kolekcję Companies
  oraz DefaultCompanyId i wybór firmy. Zachowaj obsługę wielu członkostw i jednej
  bieżącej firmy, dopóki użytkownik nie zleci zmiany tego modelu.
- PDF rozważa Blazor Server lub osobny frontend/API; obecny wybór to Web Interactive
  Server i bezpośrednie wywołania BLL. Nie przenoś operacji do API przy okazji zadania.
- JWT, mobilne MAUI/Hybrid, integracje ERP/IoT i chmura to plany. API obecnie jest
  szkieletem. Syncfusion wymieniono w PDF, ale nie jest aktywną biblioteką komponentów
  w bieżących csproj. Nie dodawaj tych zależności wyłącznie na podstawie dokumentu.
- Windows Server/VPS i wspólny serwer SQL to założenia wdrożeniowe dokumentu.
  Kod rejestruje też UseSystemd; nie wyciągaj z tego wniosku o potwierdzonym środowisku
  produkcyjnym ani nie narzucaj jednej topologii bez aktualnych danych wdrożenia.
- Responsywny dostęp z biura, hali i magazynu jest celem; kod ma ResolutionWarning
  i ograniczenia układu. Nie deklaruj gotowej obsługi telefonów ani aplikacji mobilnej.

## Cel i architektura

MagFlow to rozwijany system operacyjny dla małych i średnich firm produkcyjnych
oraz magazynowo-logistycznych: towary, magazyny, kontrahenci,
użytkownicy, a docelowo także zamówienia, dokumenty, maszyny, procesy i planowanie.
To monolit warstwowy z głównym hostem Blazor Interactive Server.

| Projekt | Odpowiedzialność i bezpośrednie referencje projektowe |
| --- | --- |
| MagFlow.Web | Razor, MudBlazor, routing, DI, middleware, endpointy auth; BLL, Domain, Shared |
| MagFlow.API | Osobny host, obecnie tylko GET / z Hello World; BLL, Domain, Shared |
| MagFlow.BLL | Usługi aplikacyjne, mapowanie, bezpieczeństwo, powiadomienia, monitoring; DAL, Domain, Shared |
| MagFlow.DAL | Repozytoria CoreScope i CompanyScope, zapytania; EF, Domain, Shared |
| MagFlow.EF | Konteksty SQL Server, fabryki, Identity stores, migracje, seedery; Domain |
| MagFlow.Domain | Encje CoreScope i CompanyScope, konfiguracja kodów; Shared |
| MagFlow.Shared | DTO, modele formularzy, interfejsy, enumy, walidatory, zasoby |
| MagFlow.Tests | xUnit v3, fixture bazodanowy; BLL, DAL, Domain, EF, Shared, Web |

Nie zakładaj architektury Clean Architecture ani czystej domeny: Domain zależy od Shared
i Identity/EF, a Shared zawiera też zależności MudBlazor. Nie przebudowuj tych granic
przy okazji niezwiązanego zadania.

## Miejsca startowe

- `MagFlow.Web/Program.cs`: logowanie, mapowanie konfiguracji, DI, pipeline, inicjalizacja baz.
- `MagFlow.Web/Extensions/ServiceCollectionExtensions.cs`: główny kontener Microsoft DI,
  repozytoria, usługi opakowane proxy Castle, Identity, telemetry, health checks.
- `MagFlow.Web/Extensions/WebApplicationExtensions.cs`: middleware, Razor, hub, start baz.
- `MagFlow.Web/Helpers/AppEndpointMapper.cs`: endpointy logowania, odświeżania cookies,
  rejestracji i wylogowania. Nie znajdują się w MagFlow.API.
- `MagFlow.Web/Routes.razor`, `App.razor`, `Pages/Layouts/TabsLayout.razor`:
  routing, renderowanie serwerowe, zakładki i odtwarzanie sesji.
- `MagFlow.Web/Helpers/IModuleMapper.cs`: powiązania modułów z komponentami.
- `docfx/docs/coding-standards.md`: istniejące zasady; dokument opisuje również
  postulaty, które nie wszędzie są wdrożone. Nie przedstawiaj ich jako stanu kodu.

## Bazy i izolacja firm

- EF Core 10 i Microsoft SQL Server. Są dwa niezależne modele i zestawy migracji.
- `CoreDbContext` dziedziczy po IdentityDbContext z kluczami Guid. Przechowuje konta,
  role, firmy, członkostwa, ustawienia, moduły/licencje, sesje, zdarzenia i powiadomienia.
- `CompanyDbContext` przechowuje dane operacyjne firmy. Klucze większości obiektów
  biznesowych to int; lokalny User ma Guid zgodny z kontem centralnym.
- Firma w CoreDb zawiera ConnectionString i generowany CompanyNumber. Izolacja
  opiera się na wyborze bazy przez fabrykę, nie na globalnym filtrze TenantId.
- `CompanyContext` odczytuje claim firmy; `TenantProvider` pobiera dane połączenia
  z CoreDb, a `CompanyDbContextFactory` wymaga połączenia i dodatniego numeru firmy.
  Nie stosuj domyślnej bazy jako zastępstwa przy braku kontekstu firmy.
- Konteksty są zwykle tworzone i zwalniane na operację. Jeżeli operacja ma być atomowa,
  wszystkie zapisy muszą używać tego samego kontekstu i transakcji.
- Migracje Core: `MagFlow.EF/Migrations`; firm: `MagFlow.EF/Migrations/CompanyDb`.
  Nie mieszaj snapshotów ani nie usuwaj historii migracji.
- Start Web uruchamia `DbInitializer.Initialize`: MigrateAsync i seed centralny,
  następnie migracje i seed wszystkich firm. Dodanie firmy także inicjalizuje jej bazę.
- Seedery są odkrywane refleksją i porządkowane po Step. Sprawdź ich treść przed
  uruchomieniem; część błędów seedowania jest przechwytywana bez propagacji.
- Nie uruchamiaj aplikacji, aktualizacji baz ani testów destrukcyjnych bez ustalenia,
  że wskazane bazy są przeznaczone do tego zadania.

## Przepływ danych i reguły domenowe

- Typowy przepływ: Razor/formularz → interfejs BLL → mapper → repozytorium → fabryka
  kontekstu → SQL Server. Odczyt wraca jako DTO/QueryResponse.
- Product to definicja produktu; Item to konkretny towar/zasób z ilością,
  lokalizacją, cenami, parametrami i komponentami. Nie używaj ich zamiennie.
- Lokalizacja: Warehouse → WarehouseSector → WarehouseSectorRow → WarehouseSectorRowSlot.
- Mapowanie jest ręczne w `MagFlow.BLL/Mappers`; nie ma wdrożonego AutoMappera/Mapstera.
  Przy nowym typie sprawdź również przełączniki w BaseMapper oraz ładowane Include.
- `BaseCompanyService` zapewnia wspólny CRUD, historię i przywracanie;
  repozytoria bazowe implementują CRUD, archiwum i obsługę zapytań.
- `ISoftDeletable.RemovedAt` steruje archiwum. `archive=true` zwykle oznacza tylko
  usunięte rekordy, nie zbiór aktywnych i usuniętych. Sprawdź daną metodę.
- Statusy zmieniaj przez ChangeStatus i uwzględniaj AllowedStatuses oraz mapowanie
  Deleted→Inactive i Available→Active dla typów bez tych statusów.
- Kody ICodeEntity generuje asynchroniczny zapis CompanyDbContext po nadaniu Id,
  w drugim zapisie tej samej wewnętrznej transakcji. Prefixy pochodzą z Domain/Configs.cs;
  kod Item uwzględnia CompanyNumber. Nie zastępuj tego ręcznym kodem formularza.
- SaveChanges synchroniczne nie ma tego samego override generowania kodów.
  Przy zmianach transakcyjnych uwzględnij transakcję otwieraną przez SaveChangesAsync.
- Zmiany Item wiążą się z ilościami komponentów, StockMovement i historią.
  Sprawdź ItemRepository, ItemService i StockMovementsFactory przed zmianą zapisu.
- `QueryOptions<T>`/`QueryResponse<T>` obsługują filtrowanie, Includes, sortowanie,
  wyszukiwanie i paginację. `QueryHelper.Paginate` liczy strony od zera;
  domyślne QueryOptions.PageNumber wynosi 1 — ustawiaj numer świadomie.
- Metody często zwracają Enums.Result, null lub puste dane po błędzie.
  Nie traktuj braku wyjątku jako potwierdzenia sukcesu.

## Bezpieczeństwo i stan UI

- ASP.NET Core Identity, cookie MagFlow.Auth, FactoryUserStore i FactoryRoleStore.
- Autoryzacja obejmuje role, policies Permission:, modułowe maski PermissionFlags
  w claims perms:<moduł> i SecurityInterceptor dla MinimumRole/MustHaveRole.
- Interfejsy usług w głównym DI są opakowane proxy Castle; wywołania bezpośrednio
  na klasach lub wewnątrz tej samej klasy mogą omijać proxy. Proxy wymaga metod async.
- Ukrycie przycisku przez AuthorizeView nie zastępuje kontroli operacji na serwerze.
  Sprawdź również prawo dostępu do firmy i rekordu.
- Cache uprawnień ma TTL 5 minut, klucz user_permissions_<id>.
  Zmiany ról/firmy oceniaj razem z odświeżeniem claims, cookies i cache.
- Blokowanie kont korzysta też z UserRevocationService, middleware, rewalidacji
  i powiadomień SignalR; zachowaj spójność tych ścieżek.
- Sesje modułów są w bazie centralnej; kolejność zakładek, bieżący moduł,
  sekcje i filtry przechowuje LocalCacheService/Blazored.LocalStorage.
- Korzystaj z istniejących komponentów DataGrid, wizardów, dialogów i walidatorów.
  Sprzątaj subskrypcje i zadania przy zwalnianiu komponentu.
- Teksty UI i walidacji lokalizuj w zasobach resx (bazowe, pl, en).
  Nie edytuj ręcznie generowanych Langs.Designer.cs i Validations.Designer.cs.

## Narzędzia, konfiguracja i weryfikacja

- Wszystkie projekty targetują net10.0; Directory.Build.props ustawia LangVersion=preview,
  wersję, a projekty włączają nullable i implicit usings. Brak global.json.
- Główne biblioteki: EF/Identity 10.0.8, MudBlazor 9.5.0, MudExtensions,
  FluentValidation, Castle DynamicProxy, Blazored.LocalStorage, MailKit/MimeKit,
  Serilog, OpenTelemetry, System.Reactive. Autofac obsługuje osobny host monitora;
  główny Web używa Microsoft DI. Wersje sprawdzaj w csproj.
- Konfiguracja jest mapowana do statycznego AppSettings, nie do IOptions w całej aplikacji.
  Sekcje: ConnectionStrings (CoreDb, CompanyDb, CompanyTemplate), SmtpSettings,
  OtelSettings, Serilog. Nie kopiuj wartości sekretów do dokumentacji i logów.
- Monitor uruchamia osobny host i heartbeat. Powiadomienia idą przez SignalR.
  Health checks: /health/live i /health/ready. Lokalny stack telemetryczny jest w OpenTelemetry/.
- Bezpieczna kompilacja bez zwiększania numeru patch:
  `dotnet build MagFlowApp.sln --no-restore -c Debug -p:AutoIncrementPatch=false`.
  Jeśli brakuje zależności, restore wykonaj osobno i zgłoś ograniczenia środowiska.
- Release domyślnie zwiększa `MagFlow.Web/Properties/VersionPatch.txt`;
  przy weryfikacji zawsze jawnie wyłącz AutoIncrementPatch.
- Testy xUnit v3 używają rzeczywistego SQL Server. TestDatabaseFixture i cleanup
  wykonują EnsureDeletedAsync; nie uruchamiaj całej suity na zwykłej bazie.
  Testy etapów Infrastructure/Crud/Logic/UI są obecnie placeholderami.
- Testy setup używają EnsureCreatedAsync, więc ich sukces nie dowodzi poprawności migracji.
- Weryfikuj zmianę adekwatnymi testami; raportuj wykonane polecenia i ograniczenia.
  Nie naprawiaj masowo zastanych ostrzeżeń przy niezwiązanym zadaniu.
- Przed pracą sprawdź git status i zachowaj zmiany użytkownika. Nie commituj ani
  nie publikuj automatycznie. Stosuj nazewnictwo i styl z istniejących zasad DocFX.

## Znane miejsca wymagające uwagi, nie wzorce do kopiowania

- RollbackTransaction w obu repozytoriach bazowych wywołuje CommitAsync.
- ItemService.AddItem otwiera transakcję, lecz AddAsync(item) nie otrzymuje jej kontekstu.
- WarehouseService.CreateStocktake zapisuje nagłówek, nie zapisuje przygotowanych pozycji
  i kończy się Result.Error również po przejściu głównej ścieżki.
- CompanyContext używa blokującego oczekiwania; wiele catch ukrywa błędy.
- NotificationHub.RegisterUser przyjmuje identyfikator od klienta; przed rozbudową
  sprawdź uwierzytelnienie huba i związanie połączenia z tożsamością serwerową.
- Modele, repozytoria i kafelki nie dowodzą ukończenia modułu. Orders, Documents,
  Machines, Processes, ProductionPlan, Logistics, Reports i Worktime mają szkielety UI.
- Dokumentacja DocFX częściowo opisuje plan, nie stan implementacji.

Nie naprawiaj powyższych problemów automatycznie w zadaniu dokumentacyjnym.
