---
name: magflow-authorization
description: "Stosuj przy projektowaniu i zmianach stron, przycisków, dialogów i akcji MagFlow wymagających uprawnień. Korzystaj z istniejących Authorize, AuthorizeView i HasModulePermission bez własnej autoryzacji."
---

# Autoryzacja MagFlow

Stosuj te zasady także w propozycjach kodu w czacie. Skill nie zmienia zasad
akceptacji implementacji określonych przez magflow-propose-change.

## Kiedy czego użyć

| Mechanizm | Zastosowanie |
| --- | --- |
| `@attribute [Authorize(Policy = ...)]` | Dostęp do całej strony z `@page`, również bezpośrednio przez URL. Obsługuje go `AuthorizeRouteView` w `MagFlow.Web/Routes.razor`. Sam atrybut na komponencie potomnym lub dialogu nie zastępuje kontroli widoku i akcji. |
| `<AuthorizeView Policy="...">` | Widoczność przycisku, sekcji lub komponentu, również bez własnej trasy. Nie sprawdza wykonania handlera. |
| `HasModulePermission("Moduł", PermissionFlags.X)` | Warunek na początku metody akcji w komponencie dziedziczącym po `AuthComponentBase`, przed otwarciem dialogu lub wywołaniem zapisu. |

Dla chronionego przycisku stosuj **AuthorizeView oraz HasModulePermission
w handlerze**, nie tylko jeden z tych mechanizmów. W dialogu zapisującym dane
sprawdzaj uprawnienie również na początku `Submit`. Strona może wymagać `Read`
na trasie oraz `Edit` lub `Delete` dla konkretnych przycisków i metod.

Przykład strony towaru, jak w `MagFlow.Web/Pages/Modules/Wares/Ware/Ware.razor`:

```razor
@attribute [Authorize(Policy = MagFlow.Shared.Constants.Policies.Claims.WARES_READ)]
```

Przykład przycisku i handlera magazynowego:

```razor
<AuthorizeView Policy="@MagFlow.Shared.Constants.Policies.Claims.WAREHOUSES_ADD">
    <Authorized>
        <MudButton OnClick="PrepareStocktake">@Localizer[Langs.StocktakePrepareTitle]</MudButton>
    </Authorized>
</AuthorizeView>
```

```csharp
if (!HasModulePermission("Warehouses", PermissionFlags.Add))
    return;
```

## Nie pisz własnej autoryzacji

- Dobieraj stałe z `MagFlow.Shared.Constants.Policies.Claims` i flagi `Read`,
  `Add`, `Edit`, `Delete`, `Admin` z `MagFlow.Shared/Models/Enumerators/Permissions.cs`.
  Nie wymyślaj flagi `Create`.
- `MagFlow.Web/Pages/Modules/AuthComponentBase.cs` odczytuje i odświeża maski
  modułów ze stanu uwierzytelnienia oraz uwzględnia SuperAdmin. Nie kopiuj parsowania
  claims, operacji bitowych ani wyjątków dla ról do własnych handlerów.
- Policies rejestruje `MagFlow.Web/Extensions/AuthorizationPolicyExtensions.cs`,
  a obsługują je handlery w `MagFlow.BLL/Security/Handlers`. Korzystaj z nich przez
  istniejące mechanizmy UI, zamiast odtwarzać ich działanie.
- Przy zwykłych operacjach modułu zachowuj wzorzec `WarehouseService.AddWarehouse`:
  **nie dodawaj IAuthorizationService/AuthorizeAsync do serwisu ani własnych kontroli
  uprawnień, ról, aktywności konta i członkostwa do repozytorium**. Pobranie autora
  przez `GetUserId` i walidacja danych operacji są odrębnymi czynnościami.
- Zachowuj zabezpieczenia zastane poza zakresem zadania. Istniejący
  `MagFlow.BLL/Helpers/Auth/SecurityInterceptor.cs` obsługuje `MinimumRole`/`MustHaveRole`;
  nie zakładaj, że automatycznie sprawdza policies modułowe na każdej metodzie BLL.
  Nie dodawaj wymagań ról zamiast uzgodnionego uprawnienia modułu.
- Samo ukrycie przycisku nie zabezpiecza osobnego endpointu/API. Gdy zadanie
  wprowadza taką ścieżkę, uzgodnij jej zabezpieczenie w propozycji zmiany;
  nie rozszerzaj samodzielnie mechanizmu autoryzacji.

Wzorce do sprawdzenia: `Ware.razor` i `Ware.razor.cs` w katalogu wskazanym wyżej,
`MagFlow.Web/Pages/Modules/Warehouses/WarehouseWizard.razor` oraz `AuthComponentBase.cs`.
