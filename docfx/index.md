# MagFlow – System zarządzania produkcją i magazynem

![Logo](images/logo.png)

Witamy w dokumentacji systemu **MagFlow** 

MagFlow to aplikacja wspierająca **firmy produkcyjne** w zarządzaniu:
- zamówieniami klientów i dostawców,
- gospodarką magazynową (PZ, WZ, RW, MM itd.),
- procesami produkcyjnymi i planowaniem,
- raportowaniem i kontrolą jakości,
- ewidencją czasu pracy i maszyn.

---

## Szybki start
- [Wprowadzenie](docs/introduction.md) – ogólne informacje o projekcie i strukturze dokumentacji.  
- [Zasady kodowania](docs/coding-standards.md) – wytyczne dla programistów.  
- [API Reference](api/index.md) – automatycznie generowana dokumentacja klas i metod.  

---

## Najważniejsze założenia
- **Technologie**: .NET 9/10, C#, EF Core, Blazor Server  
- **Baza danych**: MS SQL Server  
- **Architektura**: monolit warstwowy (Domain, DAL/EF, BLL, Web), z możliwością dalszego skalowania  
- **Wersjonowanie**: `[major].[minor].[patch].[build]` (np. `1.0.0.12`)  
- **Repozytorium**: GitHub + CI/CD (GitHub Actions + DocFX)

---

## Jak korzystać z dokumentacji?
- Nawiguj przez menu po lewej stronie (TOC).  
- Użyj wyszukiwarki (w prawym górnym rogu) do szybkiego znajdowania klas, interfejsów i modułów.  
- Dokumentacja jest aktualizowana przy każdym buildzie z głównej gałęzi repozytorium.

---

**Uwaga:** jeśli znajdziesz brakujące informacje lub chcesz dodać nową sekcję – edytuj pliki `.md` w katalogu `docs/` i zgłoś PR.
