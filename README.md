# Task9 User Panel

## Running the app

You need SQL Server running (I used the Docker image on `localhost:1433`). The database password is not committed in `appsettings.json`, it is read from an environment variable so the secret stays out of the repo:

```
export DB_PASSWORD=YourStrong@Passw0rd
dotnet run
```

On Windows PowerShell use `$env:DB_PASSWORD="..."` instead.

Migrations run automatically on startup so you do not have to set up the database by hand. The app listens on http://localhost:5000.

## Test users

An admin account is seeded automatically on the first run:

* Email: `admin@admin.com`
* Password: `Admin1234!` (local demo only)

Any account you create through the Register page gets the normal `User` role.

## Where things are

* Password hashing lives in `Controllers/AccountController.cs`, using the BCrypt.Net Next package (it handles the salt for us).
* Authentication is set up in `Program.cs` with cookie auth via `AddAuthentication` and `AddCookie`.
* `/Dashboard` is protected with `[Authorize]` on `DashboardController`, so anonymous users are sent to login.
* `/Admin` is protected with `[Authorize(Roles = "Admin")]` on `AdminController`, so a normal user cannot open it even by typing the URL.

## Questions

**Why must passwords not be stored as plain text?**
If the database leaks, every password is exposed right away, and since people reuse passwords it puts their other accounts at risk too.

**Why is raw SHA256 not a good choice for passwords?**
It is too fast. An attacker can hash millions of guesses per second. BCrypt is slow on purpose, which makes brute forcing pointless.

**Why do we use salt?**
So two users with the same password get different hashes. This stops precomputed rainbow table attacks.

**What is the difference between salt and pepper?**
Salt is stored in the database next to the hash. Pepper is a secret kept outside the database (environment variable or config) and added before hashing.

**What is the difference between authentication and authorization?**
Authentication is about who you are. Authorization is about what you are allowed to do.

**Why is hiding a link in a view not enough security?**
Because a user can still call the URL directly. The real check has to happen on the server, on the controller or action.

**Why can a "there is no such user" login message be a problem?**
It tells an attacker which emails are registered, so they know which accounts to target. A generic message avoids leaking that.
