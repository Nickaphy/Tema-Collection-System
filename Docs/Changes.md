# Changes

**Write what you have completed here**

## Refferences (Nicklas)
**UI** -> Facade
**Facade** -> Application && Infrastructure
**Appliation** -> Domain
**Infrastructure** -> Application && Domain

## SQL server (docker automatic setup)
**What works now**
- `docker compose up sqlserver` pulls our server image
- It reads the SA password from a local `.env` file (not pushed to GitHub) instead of having it written directly in *docker-compose.yml*
- Everyone on the team makes their own `.env` file with their own password and it just works

## API (barebones)
**What works now**
- The Api project now actually starts as a real web server (before this it had no `Program.cs`, so it couldn't run at all)
- `Program.cs` does 3 things: starts the web server, turns on support for Controllers (like *WatchesController*), and tells it to send incoming requests to the right controller
- If you run the Api project and call *GET /api/Watches*, the request correctly reaches *WatchesController.Get* — routing works

**What's still missing before the API actually works**
- The Api project isn't connected to the database yet, so nothing is saved or read for real
- *WatchesController* needs an *IWatchesUseCase* to be handed to it, but nothing is registered to provide one yet, so every call to */api/Watches* currently crashes with a 500 error
- *IWatchesRepository* has no way to save a watch yet (only ways to read), so even once it's wired up, creating a watch won't be stored anywhere
- There's a *Dockerfile* + an *api* section in *docker-compose.yml* for the Api project, but it won't build successfully until the above is fixed
