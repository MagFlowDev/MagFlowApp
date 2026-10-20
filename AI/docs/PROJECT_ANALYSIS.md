# MagFlow — analiza projektu

Data: 19.10.2026. Analiza statyczna repozytorium i kompilacja, bez uruchamiania aplikacji,
testów i połączenia z bazą. Schemat opisano na podstawie encji, konfiguracji EF
i plików migracji; nie potwierdzano zawartości ani schematu działającego SQL Server.
To mapa architektury i najważniejszych ścieżek, a nie dowód poprawności każdej metody.

Aktualizacja 20.10.2026: uwzględniono 25-stronicowy „MagFlow — Dokument koncepcyjny”,
Concept Paper v.1.0, z `F:/MagFlow/Dokumentacja/Folder/1. Koncepcja/Koncepcja.pdf`.
Wizję z PDF opisano osobno od implementacji. Nie powtarzano kompilacji ani nie
uruchamiano testów, aplikacji i migracji przy tej aktualizacji dokumentacji.

## Założenia biznesowe doprecyzowane na podstawie koncepcji

### Odbiorcy i problem (sekcje 1–4, str. 2–5)

System jest kierowany do małych i średnich firm produkcyjnych oraz magazynowo-logistycznych,
szczególnie zakładów wytwarzających względnie proste wyroby z podstawowych materiałów.
Ma porządkować pracę dotąd rozproszoną między papierem, Excelem i starszymi aplikacjami.
Kluczowe kryteria to przystępność wdrożenia, prostota dla nietechnicznych pracowników,
spójne i aktualne dane oraz ograniczenie błędów codziennych operacji.

Wniosek projektowy: rozwijać kompletne, proste ścieżki operacyjne i wykorzystywać
istniejący monolit oraz moduły. Szerokość wizji nie uzasadnia automatycznego wdrażania
złożoności pełnego ERP/MES ani zmiany architektury na mikroserwisy.

### Zakres i słownik biznesowy (sekcja 5, str. 6–12)

| Pojęcie | Znaczenie w koncepcji | Odniesienie do kodu i ograniczenie |
| --- | --- | --- |
| Zamówienie usługowe | Przetwarzanie materiału dostarczonego przez klienta | Order/OrderType są modelem, moduł nie zapewnia jeszcze pełnej obsługi |
| Zamówienie handlowe | Usługa z wykorzystaniem własnych materiałów firmy | Nie utożsamiać z samą sprzedażą gotowego towaru |
| Zakup / sprzedaż | Pozyskanie materiałów / sprzedaż gotowych towarów bez dodatkowej usługi | Wymagają powiązań dostaw, przyjęć, wydań i dokumentów |
| Zamówienie własne | Wewnętrzny transport albo przetworzenie posiadanych zasobów | Nie każde zamówienie wymaga zewnętrznego kontrahenta; obecny Order ma obowiązkowy ContractorId, więc semantyka wymaga decyzji |
| Udostępnienie zasobów | Tymczasowe udostępnianie magazynu, maszyny lub innych zasobów | Pełna wizja, niewymienione w zakresie MVP |
| Definicja i konkretny towar | Cechy podstawowe oraz zmienny stan operacyjny, pochodzenie i powiązania | Product/Item są przydatnym rozdziałem technicznym; nie pokrywają automatycznie całej wizji |
| Proces / zadanie | Przekształcanie wejść w wyjścia, z maszyną lub bez, z wykonawcą, czasem i parametrami | Process/ProcessStep/ProcessStepIO są zalążkiem modelu |
| Dokument | Ewidencja obiegu materiałowego: przyjęcia, wydania, zwroty, operacje wewnętrzne | Nie oznacza automatycznie fakturowania i pełnej księgowości |
| Magazyn | Towary własne i klienta, lokalizacje, bieżący i historyczny stan, inwentaryzacje | Rozbudowana implementacja części operacji; pełna historia i inwentaryzacja nie są potwierdzone |
| Czas pracy | Obecność i czas pracy, docelowo także przerwy, nieobecności i warunki zatrudnienia | WorkDay/DefaultWorkingHour opisują kalendarz, nie dowodzą kompletnego RCP |

Własność towaru i jego fizyczna lokalizacja to odrębne pojęcia. Obecne modele nie
potwierdzają kompletnej obsługi własności klienta i rozliczeń materiału powierzonego.
Przy projektowaniu procesów trzeba doprecyzować straty, jednostki i konwersje,
częściową realizację, rezerwacje, anulowanie i sposób wyceny. PDF nie ustala
jednoznacznych algorytmów tych operacji; nie należy ich dopowiadać jako gotowych reguł.

Synteza docelowej ścieżki operacyjnej: zamówienie → dostępność/przyjęcie materiałów
→ zadania i proste procesy → wyrób i stan magazynowy → wydanie oraz dokumenty
→ raportowanie. To interpretacja powiązań opisanych w koncepcji, nie obowiązkowy
workflow każdego typu zamówienia ani opis gotowej funkcji.

### MVP a stan implementacji (sekcja 8, str. 18)

| Wymaganie MVP | Co potwierdza dotychczasowy przegląd kodu |
| --- | --- |
| Konta firm, użytkownicy, role, autoryzacja i administracja | Istnieją rozbudowane usługi, UI i Identity; nadal wymagają testów ścieżek i izolacji |
| Zamówienia usługowe, handlowe, zakupu i wewnętrzne | Encje i repozytorium istnieją; główny komponent UI jest szkieletem |
| Podstawowy magazyn: przechowywanie, stan, przyjęcia/wydania | Towary, lokalizacje i ruchy są rozwinięte; kompletnego obiegu przyjęć/wydań z dokumentami nie potwierdzono |
| Tworzenie i realizacja prostych procesów | Model danych istnieje; interfejs pozostaje szkieletem |
| Rejestracja towarów i statusów | Rozwinięte Product/Item, formularze, usługi i statusy; istotne ryzyka transakcyjne |
| Proste raporty operacyjne | Listy i filtrowanie są obecne, lecz moduł Reports jest szkieletem; lista nie dowodzi ukończenia raportowania |
| Wejścia/wyjścia i podstawowy czas pracy | Kalendarz firmy i modele istnieją; Worktime jest szkieletem, brak potwierdzenia kompletnego rejestrowania obecności |

Wniosek: projekt zawiera istotny fundament MVP, ale nie można uznać zakresu MVP
z koncepcji za zakończony na podstawie obecnych modułów i modeli.

Poza MVP dokument wymienia zaawansowaną automatyzację planowania, aplikację mobilną,
zaawansowane raporty, transport oraz integracje. Roadmapa (sekcja 9, str. 19–20)
przewiduje kolejno rozszerzenia operacyjne, planowanie/Gantt, mobile, integracje
i automatyzację, analitykę/BI, a opcjonalnie logistykę. Numeracja 1.1–1.6 w tabeli
roadmapy nie powinna być utożsamiana z wersją assembly lub aktualnym wydaniem.

### Nawigacja i wdrażanie (sekcje 6–7, str. 13–17)

Strona marketingowa prowadzi do logowania i hubu aplikacji. Hub udostępnia funkcje
odpowiednie dla roli, a aktywne moduły mają pozwalać na płynne przełączanie i otwieranie
powiązanych danych bez powrotu do ekranu głównego. Obecne lobby, TabsLayout i cache
sesji realizują część tego kierunku. Nie jest to SignalR NotificationHub: słowo
„hub” w koncepcji oznacza także główny ekran produktu.

Preferowany model biznesowy wdrożenia opisany w PDF to hosting zarządzany przez
zespół na VPS/serwerze dedykowanym, z możliwością wdrożenia u klienta.
Dokument nie ustala kompletnego cennika ani zasad rozliczania subskrypcji.
Istnienie licencji modułów w kodzie nie pozwala ich dopowiedzieć.

### Różnice wymagające świadomych decyzji

| Temat | Koncepcja | Stan kodu / konsekwencja |
| --- | --- | --- |
| Przynależność użytkownika | Jedna konkretna firma | CompanyUser i ApplicationUser.Companies obsługują wiele członkostw; DefaultCompanyId i claim wyznaczają bieżącą firmę. Nie ograniczać tego automatycznie |
| Architektura hosta | Alternatywa full-stack Server albo API + oddzielny frontend | Wybrano Interactive Server i wywołania BLL; API nadal jest szkieletem |
| Autoryzacja API | Plan JWT | Bieżąca konfiguracja używa Identity cookies; nie stwierdzono wdrożonego AddJwtBearer w hostach |
| Biblioteki UI | Rozważane MudBlazor, Bootstrap, Syncfusion | Aktywne MudBlazor/MudExtensions i Bootstrap; zapis Syncfusion jest zakomentowany, brak aktywnej referencji pakietu w csproj |
| Dostęp z różnych urządzeń | Responsywny web, później mobile | Kod zawiera ostrzeżenia dla rozdzielczości i ograniczenia układu; gotowość mobilna nie została potwierdzona |
| Hosting i topologia SQL | Windows Server, początkowo wspólny serwer SQL | Jest również UseSystemd, a połączenia firm wybierane są indywidualnie; rzeczywistego wdrożenia i jednej topologii nie potwierdzono |
| Integracje i chmura | ERP, narzędzia sprzedażowe, IoT, Azure/AWS/GCP w przyszłości | Kierunek rozwoju, nie istniejące konektory ani wymaganie migracji teraz |
| Złożoność i zasoby | Mały zespół 2–3 osób, stopniowe wdrażanie | Wniosek: ograniczać zakres do zleconej funkcji, zachowywać prostotę i weryfikować pełną ścieżkę biznesową |

Koncepcja określa intencję produktu, kod potwierdza implementację, a bieżące polecenia
użytkownika określają wykonywaną pracę. Ani roadmapa, ani opis zespołu nie są
upoważnieniem do automatycznej implementacji, wdrożenia lub delegowania zadań.

## Charakter i zakres

MagFlow jest rozwijanym monolitem biznesowym dla przedsiębiorstw produkcyjnych.
Kod obejmuje osiem projektów i 746 plików C#/Razor, w tym migracje oraz pliki generowane.
Główny produkt działa w MagFlow.Web jako Blazor Interactive Server. MagFlow.API
jest osobnym, niemal pustym hostem z endpointem Hello World; Web nie potrzebuje go
jako pośrednika do codziennych operacji biznesowych.

Rozbudowane ścieżki obejmują logowanie, firmy i konta, uprawnienia, licencje modułów,
sesje, towary i definicje produktów, kontrahentów, magazyny, lokalizacje i ruchy.
Modele zamówień, dokumentów, maszyn, procesów i czasu pracy są szersze niż ich UI.
Orders, Documents, Machines, Processes, ProductionPlan, Logistics, Reports i Worktime
mają w głównych komponentach zasadniczo nagłówek i szkielet cyklu życia.
Inwentaryzacja jest rozpoczęta, lecz nie ma kompletnego zapisu pozycji.

## Architektura i zależności

Typowy kierunek wywołań to Web → BLL → DAL → EF → SQL Server.
Domain zawiera encje, Shared kontrakty DTO/formularze i wspólne typy.
Bezpośrednie referencje wszystkich projektów opisano w głównym `AGENTS.md`.
Nie jest to domena niezależna od infrastruktury: Domain używa Identity i adnotacji EF,
a Shared zawiera zależności UI. W komponentach i zapytaniach występują typy encji
oraz wyrażenia EF, więc granice warstw są pragmatyczne, a nie szczelne.

Microsoft DI rejestruje repozytoria i usługi. Usługi biznesowe są udostępniane
przez interfejsy opakowane Castle DynamicProxy. Autofac występuje w osobnym hoście
monitorowania, uruchamianym przez ApplicationMonitorHostedService.

## Start i przebieg sesji

1. Program Web konfiguruje Serilog i OpenTelemetry, mapuje sekcje do AppSettings,
   rejestruje usługi oraz wsparcie systemd.
2. Pipeline ustawia obsługę błędów, forwarded headers, HTTPS, uwierzytelnianie,
   autoryzację, antiforgery, Razor Interactive Server, lokalizację i powiadomienia.
3. InitializeDatabase wywołuje migracje oraz seed CoreDb i każdej zapisanej firmy.
   Błędy inicjalizacji są przechwytywane, więc działający host nie dowodzi sukcesu migracji.
4. Endpointy auth w Web sprawdzają konto i hasło, budują principal z rolami,
   uprawnieniami i firmą, zapisują cookie i kierują do lobby.
5. Użytkownik wybiera firmę i moduły. Sesje są zapisywane centralnie,
   TabsLayout odtwarza aktywne moduły; local storage pamięta kolejność, sekcje i filtry.
6. Formularze i listy wywołują BLL. BLL mapuje modele/DTO na encje,
   repozytoria wybierają kontekst firmy, wykonują operacje i zwracają wynik.

## Model danych

CoreDbContext to IdentityDbContext z kluczami Guid. Centralizuje ApplicationUser,
ApplicationRole i tabele Identity, firmy i CompanyUser, ustawienia, logo,
CompanyModule i licencje/cenniki, UserSession i SessionModule, audyt,
zdarzenia i powiadomienia. CompanyNumber jest generowany przez bazę.

CompanyDbContext zawiera dane pojedynczej firmy:

- Product, typy/kategorie, parametry, jednostki i konwersje, skład produktu.
- Item, jego parametry i komponenty, ilość, status, ceny oraz lokalizacja.
- Warehouse → Sector → Row → Slot; StockMovement opisuje źródło i cel ruchu.
- Contractor, Document/DocumentItem/DocumentType, Order z pozycjami i dostawami.
- Machine i MachineModel z funkcjami, parametrami oraz wpływami.
- Process z krokami, wejściami/wyjściami i dokumentami.
- Lokalny User, RoleClaim/Claim, kalendarz pracy, historia encji,
  Stocktake i pozycje inwentaryzacji.

Izolację zapewnia osobna baza wybierana przez ConnectionString firmy z CoreDb.
CompanyContext/TenantProvider wiążą bieżącą firmę z principalem i fabryką kontekstu.
Nie ma globalnego filtra tenantów, który zabezpieczyłby przypadkowo wybraną złą bazę.
Powiązanie centralnego konta z lokalnym User opiera się na tym samym Guid,
a nie na fizycznym kluczu obcym pomiędzy bazami.

Modele konfigurują liczne relacje z DeleteBehavior.NoAction, klucze złożone,
unikalne dni kalendarza i indeks historii po rodzaju encji oraz Id.
Usuwanie wielu typów jest logiczne przez RemovedAt. StatusEntity ogranicza
dozwolone statusy. CompanyDbContext generuje kody po nadaniu klucza int
przez dwufazowy zapis asynchroniczny we własnej transakcji.

Migracje Core znajdują się w Migrations (ostatnia nazwana Update22),
firm w Migrations/CompanyDb (ostatnia Update4). Te nazwy nie potwierdzają
wersji wdrożonej bazy. Testy EnsureCreated nie sprawdzają historii migracji.

## Najważniejsze klasy i metody

| Obszar | Główne punkty pracy |
| --- | --- |
| Firmy | CompanyService.CreateCompany, SelectCompany, AdminSelectCompany, UpdateModulesLicense; CompanyRepository.AddAsync inicjalizuje bazę firmy |
| Konta | UserService.CreateUser, UpdateUser, StartNewSession, UpdateLastSession, ChangePassword, BlockUser, DeleteUser |
| Uprawnienia | RoleService, AppClaimsPrincipalFactory, PermissionHandler, RoleOrPermissionHandler, DynamicAuthorizationPolicyProvider, SecurityInterceptor |
| Produkty | ProductService.GetProducts/AddProduct/UpdateProduct, parametry, komponenty, jednostki i konwersje |
| Towary | ItemService.AddItem/UpdateItem, UpdateItemComponents, BlockItem, DeleteItem, RestoreItem; ItemRepository.UpdateItemQuantity |
| Magazyny | WarehouseService.GetWarehouse/AddWarehouse/CreateStocktake, WarehouseTreeView, StockMovementsFactory |
| Wspólny CRUD | BaseCompanyService, BaseCompanyRepository, BaseCoreRepository, IRepository |
| Listy | QueryOptions, QueryResponse, QueryHelper, ServerItemsHelper, MagFlowDataGrid i kolumny |
| Mapowanie | Ręczne klasy w BLL/Mappers; BaseMapper wybiera mapper po nazwie typu |
| Komunikacja | EmailService/MailKit, NotificationService, ServerNotificationsService, ClientNotificationService, NotificationHub |
| Stan | LocalCacheService, TabsLayout, BaseModuleComponent, AuthComponentBase |

W odczytach szczegółów ważne są jawne Include: DTO budowane z niezaładowanych
nawigacji mogą nie zawierać kompletnych danych. QueryHelper buduje wyrażenia
sortowania, filtrów, wyszukiwania i aktualizacji CASE. Paginacja liczy od zera,
choć QueryOptions domyślnie ustawia PageNumber=1.

## Bezpieczeństwo, konfiguracja i eksploatacja

Uwierzytelnianie opiera się na ASP.NET Core Identity i cookie MagFlow.Auth.
Uprawnienia są sprawdzane przez role i policies oraz modułowe maski claims.
Proxy Castle sprawdza tylko odpowiednie atrybuty metod, więc samo zarejestrowanie
usługi jako proxy nie daje pełnej autoryzacji każdej operacji.
Rewalidacja, lista odwołanych użytkowników i SignalR wspierają blokowanie sesji.
Cache uprawnień trwa pięć minut i jest kluczowany identyfikatorem użytkownika.

Konfiguracja obejmuje połączenia centralne/firmowe/szablon połączenia, SMTP,
Serilog i OpenTelemetry. Istnieją wartości uwierzytelniające w plikach projektu;
nie powielono ich tutaj. Przeniesienie sekretów wymaga osobnego zadania.
OtelSettings steruje częścią telemetrii, ale Program.cs dodatkowo rejestruje
eksporter logów do localhost:4317. Stack Docker zawiera collector, Zipkin,
Jaeger, Prometheus i Seq. Health checks rozróżniają żywotność i gotowość SQL.

## Ustalenia wymagające dalszej pracy

1. Obie implementacje RollbackTransaction wywołują CommitAsync — potwierdzony błąd kodu.
2. ItemService.AddItem otwiera transakcję, ale zapis nowego towaru nie dostaje jej kontekstu.
   Może to pozostawić zapis towaru poza transakcją zmniejszania stanów komponentów.
3. CompanyDbContext.SaveChangesAsync otwiera własną transakcję przy generowaniu kodów;
   współpraca z transakcjami wyższego poziomu wymaga testu integracyjnego.
4. CreateStocktake zapisuje nagłówek, przygotowuje, lecz nie utrwala pozycji,
   i kończy metodę wynikiem Error.
5. CompanyContext blokuje na Task.Run(...).Result, liczne catch ukrywają błędy,
   a część zapisów zdarzeń i aktualizacji logowania nie jest awaitowana.
6. NotificationHub.RegisterUser przyjmuje identyfikator od klienta; klasa huba
   i jego mapowanie nie deklarują jawnego RequireAuthorization/[Authorize].
   Powiązanie użytkownika z połączeniem wymaga przeglądu bezpieczeństwa.
7. Kontrola UI, uprawnienia BLL, tożsamość firmy i cache powinny być oceniane razem;
   nie ma podstaw, by na podstawie samego widoku uznać całą ścieżkę za zabezpieczoną.
8. Testy funkcjonalne są zalążkiem: cztery Test1 sprawdzają wyłącznie 1 == 1.
   Fixture oraz cleanup tworzą/usuwają prawdziwe bazy SQL Server.
9. Dokumentacja DocFX opisuje też planowane rozwiązania (np. IOptions,
   AutoMapper/Mapster, rozbudowane API), które nie są bieżącą implementacją.

## Weryfikacja i zmiany

Wykonano `dotnet build MagFlowApp.sln --no-restore -c Debug -p:AutoIncrementPatch=false`
na SDK 10.0.401: sukces, 0 błędów, 793 ostrzeżenia. Występują ostrzeżenia nullable,
API wewnętrznego EF, analizatorów Blazor i NuGet NU1902/NU1903.
Te ostatnie są komunikatami lokalnego procesu kompilacji, nie nowym audytem online
aktualnego stanu zależności. Nie uruchamiano testów ani aplikacji.

Przed analizą istniały lokalne modyfikacje MagFlow.Web/appsettings.json oraz
MagFlow.Tests/0_Setup/TestDatabaseFixture.cs. Nie zmieniono tych plików ani kodu
aplikacji. Pierwotnie dodano wyłącznie ten raport oraz propozycję AGENTS.md.
Po wstępnej akceptacji użytkownika 20.10.2026 instrukcje przeniesiono do głównego
`AGENTS.md`, a ten raport do `AI/docs/PROJECT_ANALYSIS.md`.
