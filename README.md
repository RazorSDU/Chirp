# Chirp — Twitter-style microblogging clone  
> A full-stack demo that showcases clean architecture, threaded posts and image uploads.

## 🛠 Tech Stack & Skills Demonstrated - By Johan Hoppe Rauer
| Area | Stack / Library | Highlights shown in this project |
|------|-----------------|-----------------------------------|
| Front-end | React 18, Vite, Tailwind CSS | Component isolation, router-first SPA, custom hooks |
| Back-end | .NET 8, ASP.NET Core, EF Core 8 | Clean architecture, layered DI, JWT auth, global filters |
| Database | SQL Server, EF Core Migrations | Fluent configs, transaction-safe seeder, BLOB storage |
| Testing | xUnit, Moq, FluentAssertions | Parameterised theories, mock repositories, fast unit runs |
| Dev X | ESLint, Prettier, Swagger UI | Strict linting, auto-formatted code, live API docs |
| Ops | Docker Compose (SQL) | One-command spin-up for dev & CI |
| API & Mapping | AutoMapper 12, System.Text.Json | DTO projection, camelCase JSON payloads |
| Auth & Security | ASP.NET Core Identity, BCrypt.Net, JWT Bearer | Password hashing, role policies, token validation |
| Middleware & Rate-Limit | ASP.NET Core RateLimiter, ExceptionMiddleware | 100 req/min IP throttle, JSON error envelopes |
| API Docs | Swashbuckle | “Try it out” UI, bearer scheme, inline images |

## ✨ What the App Does

### Core Functionality
* Public timeline that pages the **latest root posts** (no parent).
* Infinite threaded replies with *O(1)* fetch per level.
* Authenticated users can **create, edit, delete** their own posts and entire threads.
* Full-text and parametric **search** across body, author and date ranges.

### Visuals & UX
* Clean, mobile-first layout with Tailwind utility classes.
* Skeleton loaders and spinners for API latency feedback.
* Avatar generation via `ui-avatars.com` to avoid storing profile pics.

### Engineering Features
* **DDD-style domain layer** free of EF Core references.
* Global `ExceptionMiddleware` converts uncaught exceptions to JSON error payloads.
* `RateLimiter` (fixed-window) caps requests to 100/min per IP.
* Swagger “try it out” including inline image rendering for `/posts/{id}/image`.
* Seeder fabricates realistic reply trees and attaches placeholder images.

## 🚀 Running Locally

```bash
# 1. Back-end --------------------------------------------------
dotnet workload install wasm-tools           # prerequisite for .NET 8 Web APIs
cd src/Backend/Chirp.API
dotnet user-secrets set "ConnectionStrings:Default" \
  "Server=(localdb)\\mssqllocaldb;Database=Chirp;Trusted_Connection=True;"
dotnet ef database update                    # run migrations
dotnet run                                   # launches on https://localhost:5001

# 2. Front-end -------------------------------------------------
# requires Node 18+
cd ../../Frontend/chirp.frontend
npm install
npm run dev                                  # Vite dev server on http://localhost:5173
