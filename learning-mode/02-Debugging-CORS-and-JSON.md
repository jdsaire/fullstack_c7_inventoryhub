# 2. Debugging CORS and JSON

Activity 2's premise: three specific integration issues, fixed one at a time. What actually
happened here matched the premise for two of the three, and turned up a fourth issue the
mandate doesn't mention at all.

## Fix 1: the route

`fix(activity2): align front-end and back-end API route to /api/productlist` — ServerApp's
endpoint moved from `/api/products` to `/api/productlist`; ClientApp's call updated to match.
Mechanical, no surprises.

## Fix 2: CORS — and a defect in the course's own snippet

`fix(activity2): add CORS policy to allow front-end access` added the mandate's literal
`app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())` to
[Program.cs](../src/ServerApp/Program.cs). That snippet **compiles but crashes the app at
startup**: `Unable to resolve service for type 'ICorsService'`. The fix — confirmed by actually
restarting the server and reading the exception, not just trusting a clean build — was adding
`builder.Services.AddCors()` before `builder.Build()`. The policy itself is unchanged from the
mandate; only the missing service registration was added.

This CORS policy is course-literal and demo-permissive
(`AllowAnyOrigin`/`AllowAnyMethod`/`AllowAnyHeader`) — labeled as such directly in the code
comment. It is not a production security stance.

## Fix 3: malformed JSON handling

`fix(activity2): add try-catch error handling for JSON deserialization` added the mandate's
literal try-catch pattern around the JSON deserialization, split into typed catches
(`JsonException`, `HttpRequestException`, `TaskCanceledException`) instead of one generic
`catch (Exception)`, so each failure mode gets a distinct, useful message.

## Fix 4: the bug the mandate doesn't mention

After the three fixes above, the product list rendered — but as two bullets reading `- $0`, no
name, no exception thrown anywhere. `fix(activity2): deserialize productlist JSON
case-insensitively` is why: ASP.NET Core's Minimal API serializes anonymous response objects
with camelCase keys (`id`, `name`, `price`, `stock`) by default. The `Product` class uses
PascalCase properties. `JsonSerializer.Deserialize` is case-*sensitive* by default, so it
silently left every property at its type's default value — `System.Text.Json` doesn't throw for
unmatched keys, it just doesn't populate them. The array itself deserialized fine (right length,
right structure), which is exactly what made this invisible to the try-catch from Fix 3: there
was no exception to catch.

The fix: `new JsonSerializerOptions { PropertyNameCaseInsensitive = true }`, passed into
`Deserialize`. Nothing later in the four Activities would have caught this otherwise — Activity
3 only adds a field, Activity 4 is performance-only — so it had to be fixed here, in the same
gate where it was found.

See [Glossary.md](Glossary.md) for CORS, camelCase/PascalCase, and related terms.
