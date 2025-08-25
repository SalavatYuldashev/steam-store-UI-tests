# s.iuldashev

# Steam Store UI Tests

Автотесты для **store.steampowered.com** на **.NET 8 + Selenium + NUnit** (Page Object, композиция компонентов).

## Требования
- .NET SDK 8.0+
- Google Chrome (актуальная версия)
- Интернет-доступ

## Быстрый старт

```bash
# восстановить пакеты
dotnet restore

# собрать
dotnet build

# запустить все тесты
dotnet test
```

## Конфигурация

Файлы конфигурации лежат в репо и автоматически копируются в `bin` при сборке:

- `src/Core/Config/appsettings.json` — базовые настройки браузера/окружения:
  ```json
  {
    "BaseUrl": "https://store.steampowered.com",
    "Headless": false,
    "Incognito": true,
    "ImplicitWaitSec": 10,
    "UILanguage": "en",
    "Maximize": true
  }
  ```
- `src/TestData/filters.json` — тестовые данные для универсальных фильтров Top Sellers:
  ```json
  {
    "TopSellersFilters": [
      { "Block": "os",        "Name": "SteamOS + Linux", "UseSearch": false },
      { "Block": "category3", "Name": "LAN Co-op",       "UseSearch": false },
      { "Block": "tag",       "Name": "Action",          "UseSearch": true, "SearchText": "action" }
    ]
  }
  ```

> Путь/копирование этих файлов уже настроены в `task2.csproj` через `CopyToOutputDirectory` (ничего дополнительно делать не нужно).

## Запуск отдельных тестов

```bash
# по имени класса
dotnet test --filter "FullyQualifiedName~TopSellersTest"

# с подробным логом
dotnet test --logger 'console;verbosity=detailed'
```

## Структура проекта (важное)

- `src/Pages` — Page Objects
    - `TopSellersWithFiltersPage` — страница «Top Sellers»
    - `Components/FilterApplierComp` — универсальное применение фильтров (читает JSON)
    - `Components/CookieBannerComp` — принятие cookie
- `src/Core/Utils` — конфиги, ожидания (`UiWaits`)

## Полезные заметки

- Если сеть медленная/быстрая — можно подстроить `ImplicitWaitSec` в `appsettings.json`.
- Сравнение цены делается по числовому значению (валюта не сравнивается).

## Частые проблемы

- **FileNotFound filters/appsettings** — убедитесь, что сборка была через `dotnet build`; файлы копируются в `bin` автоматически.
- **WebDriver/Chrome несовместимы** — обновите Chrome до актуальной версии; зависимости подтянутся через NuGet.
