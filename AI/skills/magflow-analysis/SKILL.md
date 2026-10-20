---
name: magflow-analysis
description: "Stosuj do pytań o obecne działanie, architekturę i reguły MagFlow, bez zmieniania kodu. Prośby o feature lub omawianie zmian obsługuje magflow-propose-change."
---

# Analiza i doprecyzowanie MagFlow

## Zakres

Pomagaj zrozumieć aktualne działanie aplikacji i ocenić pomysły użytkownika.
Wynikiem jest wyjaśnienie lub propozycja, nie poprawka kodu.
Gdy rozmowa dotyczy projektowania konkretnej zmiany lub nowej funkcji, odczytaj
`AI/skills/magflow-propose-change/SKILL.md` i przejdź do prototypu w czacie.

Nie edytuj kodu aplikacji, konfiguracji, zależności ani migracji w ramach tego
skilla. Nie uruchamiaj aplikacji, migracji i testów bazodanowych. Nie wdrażaj
wykrytych poprawek. Implementacja wymaga osobnego, wyraźnego zlecenia użytkownika
i nie jest częścią tej procedury.

## Analiza

- Korzystaj z głównego `AGENTS.md`. W razie potrzeby sięgnij do
  `AI/docs/PROJECT_ANALYSIS.md`, ale aktualne zachowanie potwierdzaj w kodzie.
- Skup się na pytaniu użytkownika. Odczytuj powiązane klasy i metody zamiast
  ponownie analizować całe repozytorium przy każdym pytaniu.
- Przy wyjaśnianiu operacji prześledź potrzebne elementy: widok, usługę BLL,
  mapper, repozytorium i kontekst bazy. Wyjaśnij ich rolę odpowiednio do pytania;
  nie zamieniaj każdej odpowiedzi w pełny audyt.
- Rozdzielaj stan kodu, założenia biznesowe z koncepcji oraz własne propozycje.
  Obecność encji lub pustego modułu nie oznacza gotowej funkcji.
- Nie dopowiadaj reguł biznesowych, których źródła nie określają.
  Wskaż brakujące ustalenie, jeśli wpływa na ocenę rozwiązania.

## Wynik

Najpierw odpowiedz na pytanie i opisz ustalone działanie. Jeśli proponujesz zmianę,
wyjaśnij jej cel, zakres i istotne skutki dla innych modułów lub danych.
Podawaj konkretne pliki lub metody jako uzasadnienie ustaleń.

Znaleziony błąd opisz z warunkiem wystąpienia i konsekwencją, bez samodzielnej naprawy.
Oznacz przypuszczenia i rzeczy nieweryfikowane. Dokument w `AI/docs/` twórz lub
aktualizuj, gdy użytkownik zleca przygotowanie dokumentu; zwykła odpowiedź może
pozostać w rozmowie.
