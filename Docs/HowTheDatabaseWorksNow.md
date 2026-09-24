# How the database works now

## The short version

Run one command, and you get a real SQL Server database running in Docker, with all the tables
already created and filled with sample data (watches, users, etc.), and an API you can actually
call to create/read/update/delete things.

```bash
docker compose up --build -d
```

That's it. No manual database setup, no separate migration step to remember.

## What it actually does, step by step

1. **Docker starts two containers**: one running SQL Server, one running our API.
2. **The API waits for SQL Server to be ready** (there's a health check for this), then connects to it.
3. **The API creates all the database tables automatically.** It knows what tables to create
   because we generated a "migration" — a file that describes the full database schema based on
   our C# entity classes (`Watches`, `User`, `Borrow`, etc.).
4. **The API fills the database with sample data automatically** — about 150 real watches, 3
   test users, some ratings, listings, and borrow records. This only happens once; if you restart
   the containers, it checks "is there already data here?" and skips re-adding it.
5. **The API is now ready to use** at `http://localhost:8080`, and every request it handles
   (`GET /api/Watches`, `POST /api/User`, etc.) reads from and writes to that real database.

## What changed to make this possible

Before tonight, the pieces existed but weren't connected — running the whole thing would either
crash on startup or every request would fail with a server error. Three things were missing:

1. **No migration existed.** The code that says "create tables in the database" (`MigrateAsync()`
   in `Program.cs`) was already there — but nobody had ever generated the actual migration file
   that tells it *what* tables to create. It was like having a recipe that says "bake it" with no
   actual recipe written down. We generated that file (`dotnet ef migrations add InitialCreate`).

2. **The API wasn't wired up to actually use the database logic.** The code that handles a
   request like "create a watch" existed, but nothing told the API "hey, use that code" — so
   every request crashed with a 500 error. One missing line in `Program.cs` fixed this.

3. **A handful of small pre-existing bugs** were blocking the whole thing from even compiling or
   starting — things like a typo in a file, a wrong value being passed to a database lookup, and
   two database tables being set up in a way that didn't allow adding new rows to them. These
   were fixed so the app can actually run start to finish.

We also turned on **sample data loading**, so instead of starting with an empty database, you
get realistic watch/user data automatically — useful for demoing without having to create
everything by hand first.

## What's confirmed working

- Watches: list them, create a new one — tested live, including restarting the API and
  confirming the new watch was still there (proving it's really saved in the database, not just
  held in memory).
- Users: same thing — list and create, confirmed persisted.

## What's not done yet (on purpose, not by accident)

A few features (Borrow, Image, Listing, UserRating) have the "how to talk to the database" part
missing. Their API endpoints will still return an error if you call them. This was a deliberate
choice to keep tonight's scope to "prove the core thing works," not an oversight — building
those out is its own separate task.

## Related docs

- `Docs/MotherDatabase.md` — the plan for turning this into a database the whole team can share
  over the network, instead of everyone running their own local copy.
