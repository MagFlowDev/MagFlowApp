# MagFlow – Dokumentacja systemu

Witaj w dokumentacji **MagFlow** – systemu wspierającego zarządzanie procesami produkcyjnymi, magazynowymi oraz operacjami biznesowymi w firmach produkcyjnych.  

Celem projektu jest dostarczenie spójnej platformy, która:
- integruje obszary **zamówień, magazynów, produkcji, planowania, raportowania i ewidencji czasu pracy**,  
- umożliwia **pełną kontrolę procesów** – od zamówienia klienta aż po wydanie towaru i rozliczenie produkcji,  
- zapewnia **elastyczność** (możliwość integracji z systemami zewnętrznymi, np. SAP),  
- ułatwia **przejrzystość pracy zespołów** poprzez moduły raportowe i śledzenie postępu.

---

## Struktura dokumentacji
- **Zasady kodowania** – wytyczne dotyczące stylu programowania, nazewnictwa i organizacji projektu.  
- **Architektura systemu** – opis warstw (`Domain`, `DAL/EF`, `BLL`, `API/Web`) oraz zależności pomiędzy nimi.  
- **Przewodnik po modułach** – szczegółowe opisy funkcjonalności (Zamówienia, Magazyn, Produkcja, Logistyka, Raporty).  
- **API Reference** – automatycznie generowana dokumentacja klas, metod i interfejsów.  
- **Decyzje architektoniczne (ADR)** – spis kluczowych decyzji projektowych wraz z uzasadnieniem.  

---

## Ogólne założenia projektu
- **Technologie:**  
  - Backend: C#, .NET 9/10 (w zależności od środowiska), Entity Framework Core  
  - Frontend: Blazor (Server), MudBlazor + Bootstrap/Syncfusion  
  - Baza danych: Microsoft SQL Server  
  - Architektura: początkowo monolit z wyraźnym podziałem warstw, w przyszłości możliwość skalowania i integracji  

- **Wersjonowanie:**  
  Schemat `[major].[minor].[patch].[build]`  
  Przykład: `1.2.0.15`  

- **Repozytorium i CI/CD:**  
  - GitHub jako repozytorium kodu,  
  - GitHub Actions dla CI/CD i publikacji dokumentacji.  

---

## Dla kogo jest ta dokumentacja?
- **Deweloperzy** – znajdziesz tu opis stylu kodowania, strukturę projektu oraz API Reference.  
- **Architekci/Analitycy** – spis decyzji architektonicznych oraz logikę podziału na moduły.  
- **Użytkownicy biznesowi** – ogólny wgląd w funkcjonalności systemu i procesy, które wspiera.  

---

**Notatka:** Dokumentacja jest stale rozwijana i będzie uzupełniana wraz z rozwojem kolejnych modułów systemu.