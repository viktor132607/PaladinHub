# PaladinHub

## 📝 Overview
**PaladinHub** is a **.NET 8 web application** (MVC + Razor Pages) featuring a built-in **Talent Tree Builder** for World of Warcraft Paladin specializations (Holy / Protection / Retribution).  

The project follows clean separation of concerns, uses **Entity Framework Core with PostgreSQL** for persistence, **ASP.NET Core Identity** for authentication/authorization (cookie-based and JWT bearer), and **PostgreSQL-backed `IDistributedCache`** for caching (Redis support exists but is not enabled by default).  

An **Admin area** is included to manage content, products, pages, and discussions.

---

## 📦 Technologies
- **.NET 8** – ASP.NET Core MVC + Razor Pages  
- **Entity Framework Core (Npgsql provider)**  
- **ASP.NET Core Identity** with roles (`Admin`, `User`)  
- **IDistributedCache via PostgreSQL** (`__CacheEntries` table)  
- **Docker / Docker Compose** for easy deployment  

Optional integrations:
- JWT Bearer for API authentication (mobile/SPA clients)  
- PostgreSQL seeding and auto migrations on startup  

---

## 📂 Project Structure
The repository contains **one web project**: `PaladinHub/PaladinHub`.  

Key folders:
- `Areas/Admin` → Controllers and views for the admin dashboard  
- `Controllers` (+ `Controllers/Api`) → Public controllers and API endpoints (Talents, Products, Discussions, Spellbook, etc.)  
- `Data` → EF Core `AppDbContext`, entities, seeders, repository implementations  
- `Services` → Business logic layer (Talents, Spellbook, Items, Cart, Discussions, PageBuilder)  
- `Views` → Razor Views for Home, Talent Trees, Products, Discussions, etc.  
- `wwwroot` → Static assets (CSS, JS, images). Includes `talents-editor.js` and `talentstrees.js` for the builder.  
- `Migrations` → EF Core migrations  
- `Tests` → Unit tests with xUnit  

> 🔎 Unlike multi-project solutions (Web/Domain/Services/Data), everything here is consolidated in a single project with clear folder separation.

---

## ⚙️ Configuration
Configuration is managed via `appsettings.json`. Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=paladinhubdb;Username=postgres;Password=postgres;"
  },
  "PostgresCache": {
    "SchemaName": "public",
    "TableName": "__CacheEntries",
    "CreateInfrastructure": true,
    "ExpiredItemsDeletionInterval": "00:30:00"
  },
  "Jwt": {
    "Issuer": "PaladinHub",
    "Audience": "PaladinHubUsers",
    "Key": "<long-random-secret-key>"
  }
}
