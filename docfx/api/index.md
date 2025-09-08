# API Reference

W tej sekcji znajdziesz automatycznie wygenerowaną dokumentację kodu źródłowego MagFlow.

DocFX analizuje projekty `.csproj` i tworzy opis:
- przestrzeni nazw,
- klas, interfejsów i rekordów,
- metod, właściwości i zdarzeń,
- atrybutów i komentarzy XML.

---

## Struktura API

- **MagFlow.Domain** – encje domenowe i logika biznesowa.  
- **MagFlow.DAL / MagFlow.EF** – dostęp do danych i konfiguracje EF Core.  
- **MagFlow.BLL** – usługi biznesowe i przypadki użycia.  
- **MagFlow.API** – kontrolery / endpoints API (jeśli używane).  
- **MagFlow.Web** – komponenty Blazor i warstwa prezentacji.  
- **MagFlow.Shared** – kontrakty, DTO i modele współdzielone.  

---

**Uwaga:** Zawartość tej sekcji jest generowana przy każdym buildzie dokumentacji (`docfx metadata && docfx build`).  
Jeśli czegoś brakuje → sprawdź, czy projekt ma włączone komentarze XML (`<GenerateDocumentationFile>true</GenerateDocumentationFile>` w `.csproj`).
