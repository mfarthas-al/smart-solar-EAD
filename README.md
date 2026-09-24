# Smart Solar Microgrid Trading System

SE4040 Enterprise Application Development — Assignment 1 (Group Project)

Client-server system for solar microgrid energy trading, consisting of:
- **WebService** — C# ASP.NET Core Web API (FAT service pattern), hosted on IIS, backed by MongoDB
- **WebApp** — Backoffice and Grid Operator web UI (Bootstrap 5 / Tailwind CSS)
- **MobileApp** — Pure native Android app (no cross-platform frameworks) with local SQLite, for Prosumers and Grid Operators
- **Docs** — diagrams, report, screenshots

## Team

| Name | IT Number | Module Owned |
|---|---|---|
| Mohamed Farthas (Leader) | | Backoffice module + architecture/API/DB setup |
| | | Grid Operator module |
| | | Prosumer module |

## Repository

https://github.com/mfarthas-al/smart-solar-EAD

## Getting Started

### WebService (C# ASP.NET Core Web API)

Requires: .NET SDK 8/9, MongoDB (local install or Atlas connection string).

```bash
cd WebService
dotnet restore
dotnet run
```

Mongo connection string / DB name are in `WebService/appsettings.json` under `MongoDbSettings` — point this at your own local MongoDB or a shared Atlas cluster.

### WebApp (React + Vite + Bootstrap 5)

Requires: Node.js (LTS) + npm.

```bash
cd WebApp
npm install
npm run dev
```

Pages go under `src/pages/backoffice`, `src/pages/operator`, `src/pages/auth`. Shared components under `src/components`. API calls under `src/api`.

### MobileApp (Native Android, Java)

Requires: Android Studio (latest stable) + Android SDK.

Open the `MobileApp/` folder directly in Android Studio (File → Open). It will sync Gradle and download anything missing (including the Gradle wrapper jar, which is intentionally not committed to the repo). Set your Google Maps API key in `app/src/main/res/values/strings.xml` (`google_maps_api_key`).

Screens go under:
- `app/src/main/java/com/ead/smartsolarmicrogrid/prosumer` — Prosumer screens
- `app/src/main/java/com/ead/smartsolarmicrogrid/operator` — Grid Operator screens
- `app/src/main/java/com/ead/smartsolarmicrogrid/common` — shared (login, base activity, etc.)
- `app/src/main/java/com/ead/smartsolarmicrogrid/data` — SQLite helper + API client (Retrofit)

## Demo Video

(link to be added)
