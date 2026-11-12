# AGENTS.md

## Обзор проекта и стек
- **Платформа:** .NET 8 (Windows Desktop) / C# 12
- **UI-фреймворк:** WPF (Windows Presentation Foundation)
- **Архитектура:** Строгий MVVM (используется CommunityToolkit.Mvvm)  при написании нового кода
- **DI-контейнер:** Microsoft.Extensions.DependencyInjection

## Команды сборки и проверки
Перед завершением любой задачи агент обязан убедиться, что проект собирается и тесты проходят:
- **Сборка:** `dotnet build --configuration Debug`
- **Запуск тестов:** `dotnet test`
- **Форматирование:** `dotnet format --verify-no-changes`

## Архитектурные правила (WPF & MVVM)
- **Code-behind (`.xaml.cs`):** Должен оставаться максимально пустым. Разрешается только вызов `InitializeComponent()` и логика, жестко привязанная к UI-анимациям или фокусу. Вся бизнес-логика и обработка событий выносятся в `ViewModel` через команды (`[RelayCommand]`).
- **Свойства и привязки:** Для уведомлений об изменениях использовать source generators из `CommunityToolkit.Mvvm` (`[ObservableProperty]`), а не ручную реализацию `INotifyPropertyChanged`. (при написании нового кода)
- **Асинхронность:** Никогда не использовать `.Result` или `.Wait()` в UI-потоке во избежание deadlock. Для фоновых задач использовать `async Task` и при необходимости обновлять UI через `Application.Current.Dispatcher`.
- **XAML-разметка:** Использовать `StaticResource` для статических стилей и кистей, `DynamicResource` — только для меняющихся тем оформления. Избегать жестко заданных размеров (`Width`/`Height`), отдавать предпочтение `Grid` и `StackPanel`.

## Структура solution
- `Presentation/Views project BioAlgorithm.Core.csproj` — XAML-окна и UserControl'ы.
- `Presentation/ViewModels project BioAlgorithmViewModel.csproj`  — классы логики представлений.
- `/Models` — доменные модели и DTO.
- `/Services project BioAlgorithm.Services.csproj` — работа с API, файловой системой и диалоговыми окнами.
- `/Data project BioAlgorithm.Data.csproj` — работа с БД.
- `/Resources` — словари ресурсов (`ResourceDictionary`), стили и конвертеры (`IValueConverter`).

## Ограничения и запреты
- **Не устанавливать сторонние NuGet-пакеты** без предварительного согласования.
- **Не изменять** файлы конфигурации сборки (`.csproj`, `Directory.Build.props`) без явного запроса.
- **Не создавать** зависимости от `System.Windows` (UI-типов) внутри папок `/ViewModels` и `/Services` — для диалогов и уведомлений использовать интерфейсы сервисов.
