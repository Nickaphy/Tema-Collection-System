# Mother Database (Team-Shared, LAN-Accessible)

**Status: not implemented yet.** This is the checklist for turning the Dockerized SQL Server
on your machine into the one persistent "mother database" the team connects to over LAN, once
migrations + CRUD (see `docker-compose.yml`) are confirmed working locally.

## 1. Make the data actually persistent

`docker-compose.yml` currently stores SQL Server's data in a **named Docker volume**
(`sqlserver-data`), not a host path. That's fine for local dev, but it means running
`docker compose down -v` (or Docker pruning volumes) permanently deletes the team's data.
Before treating this as "the" database, switch to a bind mount so the data lives in a real,
visible folder on disk:

```yaml
volumes:
  - ./data/sqlserver:/var/opt/mssql   # instead of sqlserver-data:/var/opt/mssql
```

Add `data/` to `.gitignore` — the database files themselves should never be committed.

## 2. Get a stable LAN address for your machine

Your current LAN IP is `192.168.0.236` (Wi-Fi, `wlp99s0`) — but that's DHCP-assigned and can
change on reboot/reconnect. Before sharing this with the team, reserve it as a static/DHCP
lease in your router's admin page (look for "DHCP reservation" / "static lease", keyed to your
machine's MAC address), so teammates aren't chasing a moving target.

## 3. Open the firewall for port 1433

You're on Fedora with `firewalld`. SQL Server's port (1433, already exposed in
`docker-compose.yml`) needs an inbound rule:

```bash
sudo firewall-cmd --add-port=1433/tcp --permanent
sudo firewall-cmd --reload
```

Restrict this to your LAN zone/subnet rather than opening it globally if your `firewalld` zone
setup allows it — check `firewall-cmd --get-active-zones` first.

## 4. What teammates connect with

Once 1-3 are done and `docker compose up -d` (at least the `sqlserver` service) is running on
your machine, teammates on the same network use:

```
Server=<your-reserved-lan-ip>,1433;Database=WatchWorld;User Id=sa;Password=<the team's SA_PASSWORD>;TrustServerCertificate=True;
```

They'll need the same `SA_PASSWORD` value you have in your local `.env` (share it out of band —
Slack/1Password, not committed to git). Point their own `WatchWorld.Api`'s
`ConnectionStrings__DefaultConnection` (or a local `appsettings.Development.json`) at that
connection string instead of `sqlserver,1433` (which only resolves inside your `docker compose`
network) to run the API against your shared DB instead of spinning up their own local instance.

## 5. A caveat worth saying out loud

Sharing the `sa` (system administrator) login over LAN is fine for a dev/demo database nobody
depends on for anything real, but it hands every teammate full admin rights over the SQL Server
instance. If this ever needs to hold real data, create a dedicated least-privilege login for the
app instead of handing out `sa`.

## 6. Availability

The database is only reachable while your machine is on, connected to the network, and
`docker compose` is running (at minimum, `docker compose up -d sqlserver`). There's no
automatic restart-on-boot configured — if you want it to survive a reboot without you manually
starting it, add `restart: unless-stopped` to the `sqlserver` service in `docker-compose.yml`.
