# Reflection

## How an AI coding assistant helped

Across all four activities, an AI coding assistant generated the first draft of each
integration point and then helped diagnose why the literal course starter code didn't
actually run as written — which turned out to be the more valuable part of the process.

- **Integration (Activity 1)**: it produced the `HttpClient` call, the
  `GetAsync`/`ReadAsStringAsync`/`Deserialize` sequence, and suggested pulling that logic out
  of `OnInitializedAsync` into a dedicated `LoadProductsAsync` method for readability.
- **Debugging (Activity 2)**: it traced two failures that had no compiler error to point at.
  Wiring up `app.UseCors(...)` exactly as given in the course material compiles fine but
  crashes the app at startup (`Unable to resolve service for type 'ICorsService'`) — the fix
  was adding `builder.Services.AddCors()`, which the course snippet omits. Separately, the
  product list rendered as two blank rows reading `- $0` with no exception thrown at all: the
  back-end serializes JSON with camelCase keys by ASP.NET Core's default, the front-end
  `Product` class uses PascalCase, and `JsonSerializer.Deserialize` silently leaves unmatched
  properties at their default value instead of failing loudly. Passing
  `PropertyNameCaseInsensitive = true` fixed it.
- **JSON structuring (Activity 3)**: it confirmed that `System.Text.Json` maps a nested JSON
  object straight onto a nested C# class with no extra converter needed, as long as the
  `Category` class's shape mirrors the response's nested object.
- **Optimization (Activity 4)**: it proposed `IMemoryCache` for the back-end response and
  pointed out that the three near-identical `catch` blocks on the front end could share one
  `LogAndSetError` helper instead of repeating the same `Console.WriteLine` line three times.

## Challenges encountered

The recurring challenge was that the course's literal starter snippets look complete but
aren't quite runnable: a missing closing parenthesis in the `foreach` loop, a CORS policy with
no service registration behind it, and a JSON deserialization call with no defense against a
casing mismatch between the two projects. None of these show up as build errors — the app
compiles every time — so each one only surfaced by actually running both projects together and
checking the rendered page, not by trusting a clean build. That's the concrete lesson behind
"test the integration, not just the code": a green build and a working feature are different
claims, and only one of them was checked by default.

## What I learned about using an AI coding assistant effectively

It's fastest at generating a plausible first pass and at explaining *why* something that looks
right is failing once you show it the actual error or the actual (wrong) output — the CORS
crash and the silent JSON mismatch were both diagnosed that way, from a stack trace and from a
side-by-side comparison of the JSON payload against the C# model, not from staring at the code
alone. It's not a substitute for actually running the application, though: every fix in this
project came from observing a real failure first, then asking why, rather than from reading the
starter code and guessing what might be wrong with it.
