# PaladinHub

## ⚠️ Конфигурация

1. Уверете се, че сте обновили файла **`appsettings.json`** с вашите настройки, иначе приложението няма да стартира.  
2. Попълнете:
   - Connection string за PostgreSQL база данни.
   - Redis connection string (ако се ползва кеш).
   - JWT ключове, API ключове и други специфични конфигурации.

---

## 📝 Описание

**PaladinHub** е модулен и скалируем .NET 8 уеб проект с вграден Talent Tree Builder. Следва принципите на **Clean Architecture** и съдържа:

- Уеб интерфейс с Razor Pages / MVC
- Админ панел за управление на страници, база данни и продукти
- API и бизнес логика в отделни слоеве
- Работа с PostgreSQL и Redis
- Docker съвместимост

---

## 📂 Структура на проекта

1. **PaladinHub.Web**  
   - Основният уеб проект (MVC + Razor Pages) с UI и админ панел.

2. **PaladinHub.Data**  
   - База данни, DbContext, ентитети и репозитории (EF Core).

3. **PaladinHub.Domain**  
   - Бизнес логика и домейн модели.

4. **PaladinHub.Services**  
   - Сървис слой за интеграции и бизнес операции.

5. **PaladinHub.Tests**  
   - Unit тестове.

6. **Конфигурационни файлове**  
   - `.editorconfig`, `.gitignore`, `Directory.Packages.props`, `LICENSE`, `README.md`.

---

## 🖥️ Изисквания

- **.NET 8 SDK**  
  [Изтегли от тук](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- **IDE**  
  - Visual Studio 2022+
  - Rider
  - VS Code с C# разширение
- **Docker** (за бързо стартиране на база данни и кеш)

---

## 🚀 Стартиране с Docker

Стартирайте Redis и PostgreSQL локално:

```bash
docker run -d --name redisdb -p 6379:6379 redis:7

docker run -d --name postgresdb \
  -e POSTGRES_DB=paladinhubdb \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -p 5432:5432 postgres:16
