# Materiały AI dla MagFlow

Główne instrukcje znajdują się w [../AGENTS.md](../AGENTS.md).

| Lokalizacja | Zawartość |
| --- | --- |
| docs/ | Dokumenty generowane przez AI: analizy, założenia i propozycje |
| skills/<nazwa>/SKILL.md | Instrukcje konkretnych, powtarzalnych zadań |
| tmp/ | Pliki robocze, ignorowane przez Git; katalog tworzony w razie potrzeby |

Dokumenty źródłowe użytkownika pozostają w dotychczasowych lokalizacjach.
Analiza projektu jest w [docs/PROJECT_ANALYSIS.md](docs/PROJECT_ANALYSIS.md).
Dokumenty AI rozróżniają potwierdzony stan kodu, wymagania i propozycje.
Nie zawierają sekretów z konfiguracji i nie stanowią polecenia wdrażania zmian.

## Skille

Skille odczytujemy bezpośrednio z plików SKILL.md. Do znalezienia plików wystarczy
zwykłe przeglądanie AI/skills/; osobny skrypt nie jest potrzebny.
Główny AGENTS.md wskazuje dostępne skille i ich przeznaczenie.

| Skill | Kiedy go stosować |
| --- | --- |
| [magflow-analysis](skills/magflow-analysis/SKILL.md) | Pytania o obecne działanie i architekturę; analiza bez zmian kodu |
| [magflow-propose-change](skills/magflow-propose-change/SKILL.md) | Nowe funkcje, poprawki i omawianie zmian; prototyp oraz proponowany kod w czacie, implementacja po wyraźnej akceptacji |
| [magflow-authorization](skills/magflow-authorization/SKILL.md) | Authorize na trasie, AuthorizeView w widoku i HasModulePermission w handlerach; bez własnej autoryzacji |
| [magflow-localization](skills/magflow-localization/SKILL.md) | Zasoby wszystkich języków, generowanie Langs i Localizer[Langs.Nazwa] |

Przykład: „Użyj magflow-analysis, żeby wyjaśnić, jak działa zapis towaru”.

Prośba „potrzebuję formularza do dodawania magazynu” uruchamia
magflow-propose-change także bez wymieniania jego nazwy. Najpierw omawiamy
układ, zachowanie i kod w czacie. Uwagi modyfikują propozycję; dopiero
„akceptuję, implementujemy” pozwala zapisać uzgodnioną wersję do plików aplikacji.

Nowy skill umieszczamy w skills/<nazwa>/SKILL.md, z polami name i description
w nagłówku YAML oraz krótką procedurą w treści. Dodajemy jego ścieżkę
i przeznaczenie w głównym AGENTS.md. Nie wymaga to instalacji ani skryptu.
