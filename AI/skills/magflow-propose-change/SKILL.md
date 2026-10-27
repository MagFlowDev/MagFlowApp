---
name: magflow-propose-change
description: "Stosuj, gdy użytkownik prosi o feature, poprawkę, zmianę UI lub logiki MagFlow albo omawia zmiany. Najpierw przedstaw prototyp i proponowany kod w czacie; implementuj dopiero po wyraźnej akceptacji tej propozycji."
---

# Propozycja zmiany przed implementacją

## Kiedy stosować

Prośby takie jak „potrzebuję formularza do dodawania magazynu”, „dodaj przycisk”
lub „popraw tę metodę” rozpoczynają projektowanie rozwiązania. Nie są jeszcze
zgodą na zapis kodu. Skill dotyczy też kolejnych uwag do omawianego prototypu.
Pytania wyłącznie o istniejące działanie obsługuje magflow-analysis.
Jawna edycja dokumentów AI i instrukcji nie jest zmianą aplikacji objętą tym workflow.

## 1. Poznaj kontekst bez zmieniania plików

Przeczytaj główny AGENTS.md i właściwe elementy aktualnej implementacji.
Dla stron i akcji wymagających uprawnień stosuj również
`../magflow-authorization/SKILL.md`, a dla tekstów UI
`../magflow-localization/SKILL.md`. Te konwencje obowiązują już w proponowanym kodzie.
Ustal, czy funkcja już częściowo istnieje i jakie komponenty można wykorzystać.
Jeśli brakuje informacji o działaniu, zapytaj o konkretną regułę lub oznacz
rozsądne założenie do oceny. Nie wymagaj od użytkownika wyboru szczegółów technicznych,
które można wywnioskować z projektu.

Przed akceptacją używaj odczytu i analizy. Nie edytuj plików aplikacji, konfiguracji,
pakietów ani migracji. Nie zapisuj kodu prototypu do plików roboczych, nie uruchamiaj
aplikacji i nie wykonuj operacji bazodanowych. Dokument propozycji zapisuj tylko,
jeśli użytkownik dodatkowo o niego poprosi; nie zastępuje on prezentacji w czacie.

## 2. Pokaż konkretną propozycję w czacie

Dostosuj szczegółowość do zmiany, ale zawsze pokaż:

- **Zachowanie:** co użytkownik zobaczy i co wykonają akcje; istotne walidacje,
  komunikaty, uprawnienia i sposób powrotu, jeśli dotyczą funkcji.
- **Prototyp:** dla UI układ formularza/widoku, pola i przyciski w czytelnej formie
  tekstowej lub wizualnej; dla logiki przykład wejścia i oczekiwanego wyniku.
- **Zakres plików:** gdzie powstaną zmiany i jak wykorzystają istniejące warstwy.
- **Proponowany kod:** bloki Razor/C#/CSS lub diff, podpisane docelowymi plikami.
  Dla małej zmiany pokaż cały zmieniany fragment lub metodę. Dla większej pokaż
  wszystkie istotne elementy zachowania, a powtarzalne szczegóły opisz jawnie.
  Nie przedstawiaj samej listy kroków zamiast kodu. Kod jest propozycją i nie jest zapisany.
- **Weryfikację:** jak sprawdzisz uzgodnione zachowanie po implementacji oraz
  istotny wpływ na dane i schemat, jeśli występuje.

Opieraj kod na aktualnych klasach i komponentach MagFlow. Nie proponuj fikcyjnych
interfejsów jako już istniejących. Oznacz nowe typy i nierozstrzygnięte reguły.
Nie deklaruj przetestowania kodu, który przedstawiono jedynie w czacie.

## 3. Dopracuj prototyp i poczekaj na decyzję

Uwagi typu „przesuń guzik”, „dodaj pole” lub „zmień walidację” aktualizują
propozycję w czacie. Nadal nie zapisuj kodu do plików.
Przy każdej iteracji pokaż zmienione fragmenty i utrzymuj spójność z resztą
uzgodnionej propozycji, bez ponownego wklejania niezmienionych dużych bloków.

Zakończ propozycję pytaniem, czy użytkownik akceptuje jej wdrożenie. Wyjaśnij krótko,
że zgodnie z uzgodnionym workflow czekasz z zapisem plików na jego decyzję.
Wyraźne „akceptuję, implementujemy”, „wdrażaj tę wersję” albo odpowiedź „tak”
na jednoznaczne pytanie o implementację jest zgodą. Samo „okej”, pochwała,
dalsza dyskusja lub upływ czasu nie są zgodą. Jeśli decyzja jest niejasna,
dopytaj, bez implementowania.

## 4. Zaimplementuj zaakceptowaną wersję

Po potwierdzeniu wykonaj uzgodniony zakres i adekwatną weryfikację.
Nie proś ponownie o zatwierdzenie każdego pliku lub rutynowego szczegółu.
Jeśli odkryjesz konieczność istotnego rozszerzenia zachowania, zmiany modelu danych
lub innego rozwiązania niż uzgodnione, przedstaw różnicę do akceptacji przed jej wdrożeniem.
Sama akceptacja funkcji nie upoważnia do niezwiązanych napraw, wdrożenia na produkcję
ani destrukcyjnych operacji na bazie.

Wynik opisz jako wykonane zmiany i rzeczywiście przeprowadzoną weryfikację.
