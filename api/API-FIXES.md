# API fixes — code vs. `openapi.yaml`

`api/openapi.yaml` is the source of truth. The C# controllers below don't match it yet.

## 🔴 Crashes (fix first)

### 1. UserRating: 3 GETs on the same route
- **File:** `WatchWorld.Api/Controllers/UserRatingController.cs`
- **Problem:** `GetAllUserRatingsByUserIdAsync`, `GetAllUserRatingsToUserIdAsync` and `GetUserRatingByIdAsync` all use `[HttpGet]` with no template → `AmbiguousMatchException` (500) on `GET /api/UserRating`.
- **Fix (per spec):**
  - `[HttpGet("by-user/{userId}")]` → ratings a user has given
  - `[HttpGet("to-user/{userId}")]` → ratings a user has received
  - `[HttpGet("{id}")]` → one rating
- **Also:** `GetUserRatingByIdAsync` ignores the result and returns the ID it was given, not the rating.

### 2. IndividualWatch: `UpdateWatch` has no HTTP attribute
- **File:** `WatchWorld.Api/Controllers/IndividualWatchController.cs`
- **Problem:** No `[HttpPut]` → the action matches every HTTP method on `/api/IndividualWatch` and clashes with GET/POST.
- **Fix:** `[HttpPut("{id}")]` + add a `Guid id` parameter.

### 3. `[Authorize]` without authentication configured
- **File:** `WatchWorld.Api/Program.cs`
- **Problem:** No `AddAuthentication` / `UseAuthentication` / `UseAuthorization` → every `[Authorize]` endpoint returns 500 instead of checking access.
- **Fix:** Set up authentication (e.g. JWT Bearer), or remove `[Authorize]` until it exists.

## 🟠 Wrong behaviour

### 4. Borrow: `PUT /{userId}` for the time slot
- **File:** `BorrowController.cs` → `UpdateTimeSlot`
- **Problem:** Route says `{userId}`, the parameter is `id` → never bound. Uses `request.borrowId` anyway.
- **Fix:** `[HttpPut("{id}/timeslot")]`, use the route `id`.

### 5. UserRating: PUT and DELETE have no `{id}` in the route
- **Fix:** `[HttpPut("{id}")]` and `[HttpDelete("{id}")]`.

### 6. `CreatedAtAction(..., new { id = new Guid() })`
- **Where:** Create actions in several controllers
- **Problem:** Always sends an empty GUID (`00000000-...`) in the `Location` header.
- **Fix:** Use the created object's `Id`, and point to the `GetById` action, not `Get`/itself.

## 🟡 Best practice

### 7. DELETE with a request body
- **Where:** Watches, IndividualWatch, Listing, Borrow, User, UserRating
- **Problem:** `{id}` is in the route, but the action reads the ID from a body. Some clients/proxies drop DELETE bodies.
- **Fix:** `Delete(Guid id, CancellationToken ct)` — delete the `Delete*Request` classes.

### 8. PUT on Watches ignores `{id}`
- **File:** `WatchesController.cs` → `UpdateWatch`
- **Fix:** Add `Guid id` and use it instead of an ID from the body.

### 9. No Swagger/OpenAPI in the API
- **File:** `Program.cs`
- **Fix (later):** Serve `api/openapi.yaml` with Swagger UI, or add `AddOpenApi()` to compare the generated spec with ours.

## 🔒 Security
- `Status.md` contains the Mother SA password in plain text. Remove it, share it outside the repo, and **rotate the password** (it's in git history).

## ✅ Checklist
- [ ] 1. UserRating GET routes
- [ ] 2. IndividualWatch `[HttpPut("{id}")]`
- [ ] 3. Authentication setup
- [ ] 4. Borrow `{id}/timeslot`
- [ ] 5. UserRating PUT/DELETE `{id}`
- [ ] 6. `CreatedAtAction` IDs
- [ ] 7. DELETE via route ID
- [ ] 8. Watches PUT uses route ID
- [ ] 9. Swagger UI
- [ ] Rotate Mother password
