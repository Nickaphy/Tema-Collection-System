# Status

## New database setup
We now have a "mother database", but its only accesible when on the same network,
as yours truly. When the application starts it will always probe the mother database, 
if `!mother then local`. It runs on a 3 second timeout.

### Do this (one time) INSIDE YOUR PROJECT ROOT!!!
1. `docker compose down -v`- close and delete your current local database and volume.
2. Change your `.env` file to: 
    ```
    SA_PASSWORD=YourPersonalPasswordForYourLocalDatabase123%
    MOTHER_SA_PASSWORD=VikingBoner123%
    ```
3. `docker compose up --build -d` - Spins up your local SQL-server + API, disowns the terminal.

### General workflow
- `docker compose up --build -d`, is responsible for SQL and API ONLY (the daemon runs in the background,
so only needs to be run when and if it gets closed) (and maybe after certain changes to docker/sql related stuff)
- Launch the actual entrypoint (Blazor.UI) through your little Visual Studio, or grow a pair an run `dotnet run` inside the BlazorUI project.

