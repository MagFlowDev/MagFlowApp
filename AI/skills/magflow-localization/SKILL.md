---
name: magflow-localization
description: "Stosuj przy dodawaniu i zmianach tekstów UI MagFlow, także w propozycjach kodu. Używaj zasobów wszystkich języków i Localizer[Langs.Nazwa], bez literalnych ani składanych kluczy."
---

# Lokalizacja MagFlow

Stosuj obecny wzorzec aplikacji również w kodzie prezentowanym w czacie.
Skill nie zmienia zasad akceptacji implementacji.

## Kolejność pracy

1. Sprawdź, czy odpowiedni tekst istnieje w `MagFlow.Web/Resources/Langs.resx`.
   Jeśli pasuje znaczeniem, wykorzystaj istniejącą właściwość Langs.
2. Nowy klucz i właściwe tłumaczenia dodaj do **wszystkich** zasobów:
   `Langs.resx` (bazowy), `Langs.pl.resx` i `Langs.en.resx`. Dotyczy to również
   tooltipów, tytułów dialogów, pustych list oraz komunikatów sukcesu i błędu.
   Jeśli zestaw języków się zmieni, uwzględnij aktualne pliki zasobów.
3. Wygeneruj `Langs` z bazowego resx. Projekt konfiguruje
   `PublicResXFileCodeGenerator`; użyj generatora Visual Studio, ResGen z generowaniem
   klasy typowanej lub `StronglyTypedResourceBuilder`. **Nie dopisuj właściwości
   ręcznie do Langs.Designer.cs**. Nie zakładaj, że samo `dotnet build` uruchamia
   narzędzie skonfigurowane w IDE. Zachowaj namespace i bazę ResourceManager:
   `MagFlow.Web.Resources.Langs`.
4. W Razor i C# odwołuj się do tekstu przez właściwość Langs:

```razor
<MudButton>@Localizer[Langs.Save]</MudButton>
```

```csharp
Snackbar.Add(Localizer[Langs.ChangesSaved], Severity.Success);
```

Nie używaj `Localizer["Save"]`, `Localizer["Zapisz"]` ani
`Localizer[$"StocktakeType_{type}"]`. Brak właściwości w Langs oznacza konieczność
uzupełnienia zasobów i wygenerowania klasy, nie obejście literalnym stringiem.
Wartości komunikatu wstawiaj do lokalizowanego szablonu, zgodnie z istniejącym
`string.Format(Localizer[Langs.RestoreWareConfirmation], ...)`.

## Istniejące wzorce i sprawdzenie

- `MagFlow.Web/_Imports.razor` udostępnia `MagFlow.Web.Resources` i wstrzykuje
  `IStringLocalizer<Langs> Localizer`. W code-behind dodaj potrzebny using.
- Dla enumów stosuj istniejące `GetDisplayName(Localizer)` z
  `MagFlow.BLL/Helpers/EnumsHelper.cs` oraz `Display(Name = ...)` wskazujący klucz
  obecny we wszystkich zasobach. Nie składaj dynamicznych kluczy w UI.
- Walidatory mają osobne zasoby
  `MagFlow.Shared/Validators/Resources/Validations*.resx`; zachowaj ich wzorzec
  i nie edytuj ręcznie `Validations.Designer.cs`.
- Zastane wywołania z literalnymi stringami nie są wzorcem do kopiowania.
  Nie poprawiaj masowo niezwiązanych ekranów przy okazji zadania.
- Sprawdź klucze we wszystkich językach, brak duplikatów, wygenerowane właściwości
  i kompilację. Nie uruchamiaj aplikacji ani operacji bazodanowych wyłącznie
  do weryfikacji tekstów.
