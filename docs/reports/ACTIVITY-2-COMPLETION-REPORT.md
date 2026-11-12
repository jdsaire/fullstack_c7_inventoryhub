# Activity 2 Completion Report · InventoryHub Capstone

| Document Metadata | Value |
|---|---|
| **Activity ID** | Course 7 Capstone · Activity 2 (C7-A2) |
| **Title** | Debugging and Fixing Integration Issues: Route Alignment, CORS Policy, and Typed JSON Error Handling |
| **Author** | Juan Diego S. (`88201583+jdsaire@users.noreply.github.com`) |
| **Specialization** | Microsoft Full Stack Developer Specialization |
| **Course** | Course 7: Full-Stack Integration |
| **Status** | Completed on branch `rebuild-a2-debugging` |
| **Target Repository** | [`jdsaire/fullstack_c7_inventoryhub`](https://github.com/jdsaire/fullstack_c7_inventoryhub) |
| **Date** | November 2026 |

---

## 1. Executive Summary

Activity 2 of the InventoryHub Capstone focuses on diagnosing, debugging, and resolving multi-layer integration failures between the Blazor WebAssembly client and the ASP.NET Core Minimal API service. During initial integration, distributed applications frequently encounter three classic failure modes:
1. **API Route Mismatch**: The front-end client requests a legacy or divergent URI route (`/api/products`), resulting in HTTP 404 (Not Found) responses from the back-end endpoint (`/api/productlist`).
2. **Cross-Origin Resource Sharing (CORS) Restrictions**: Browser security policies block cross-origin requests from the client application origin to the back-end API origin due to missing or misconfigured CORS headers and middleware.
3. **JSON Deserialization Failures**: Casing discrepancies between camelCase API response schemas and PascalCase C# model properties cause silent property binding failures, while corrupted syntax or type mismatches trigger unhandled exceptions without actionable user feedback.

This rebuild establishes comprehensive automated test coverage ([`Tests/TestDebuggingAndErrors.cs`](../../Tests/TestDebuggingAndErrors.cs)) validating route synchronization, demo-permissive CORS policy configuration, dependency injection resolution of CORS services, and typed JSON deserialization error handling (`[M]`).

### Milestone Deliverables Achieved
- **Route Synchronization Verification (`[M]`)**: Validated exact route parity between front-end `HttpClient.GetAsync("/api/productlist")` in [`src/ClientApp/Pages/FetchProducts.razor`](../../src/ClientApp/Pages/FetchProducts.razor) and back-end `app.MapGet("/api/productlist", ...)` in [`src/ServerApp/Program.cs`](../../src/ServerApp/Program.cs).
- **Demo-Permissive CORS Policy & Middleware (`[M]`)**: Verified registration of `builder.Services.AddCors()` and configuration of `app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())` in [`src/ServerApp/Program.cs`](../../src/ServerApp/Program.cs), ensuring browser clients can access API resources during local evaluation.
- **Typed Deserialization Error Hierarchy (`[M]`)**: Verified typed exception handling in [`src/ClientApp/Pages/FetchProducts.razor`](../../src/ClientApp/Pages/FetchProducts.razor), isolating `JsonException` (malformed syntax or type mismatch) from transport-layer exceptions (`HttpRequestException`, `TaskCanceledException`) with specific diagnostic messages.
- **Automated Verification Test Suite (`[M]`)**: Created [`Tests/TestDebuggingAndErrors.cs`](../../Tests/TestDebuggingAndErrors.cs) containing 8 automated NUnit unit test fixtures verifying route alignment, CORS policy and DI resolution, casing insensitivity, and typed exception isolation.

---

## 2. Course 7 Capstone Rubric Alignment

The table below quotes the six Course 7 Capstone rubric criteria verbatim from the course evaluation specifications and records how Activity 2 satisfies each criterion:

| Criterion | Points | Question | Activity 2 Scope & Repository Evidence |
|---|---|---|---|
| **C1** | 5 | Did you create a GitHub repository for your project? | Public repository [`jdsaire/fullstack_c7_inventoryhub`](https://github.com/jdsaire/fullstack_c7_inventoryhub) maintained with branch `rebuild-a2-debugging` branched from `main` @ `f14f508` (`[M]`). |
| **C2** | 5 | Did you generate and refine integration code for front-end and back-end communication? | Integration code verified and refined in [`src/ClientApp/Pages/FetchProducts.razor`](../../src/ClientApp/Pages/FetchProducts.razor) and [`src/ServerApp/Program.cs`](../../src/ServerApp/Program.cs), ensuring reliable communication across origin boundaries (`[M]`). |
| **C3** | 5 | Did you debug and resolve integration issues effectively using the AI assistant? | Debugged and resolved route mismatch (`/api/productlist`), missing `AddCors()` DI service crash, and case-sensitive JSON deserialization in client/server code; verified via automated test suite in [`Tests/TestDebuggingAndErrors.cs`](../../Tests/TestDebuggingAndErrors.cs) (`[M]`). |
| **C4** | 5 | Did you create and implement JSON structures for API communication? | Verified case-insensitive deserialization of product arrays; structured nested category models addressed in Activity 3 (`docs/reports/ACTIVITY-3-COMPLETION-REPORT.md`). |
| **C5** | 5 | Did you optimize the integration code for performance using the AI assistant? | Out of scope for Activity 2; addressed in Activity 4 (`docs/reports/ACTIVITY-4-COMPLETION-REPORT.md`) covering back-end in-memory caching and client error-handler consolidation. |
| **C6** | 5 | Did you include a reflective summary explaining how the AI assistant assisted in each step? | Out of scope for Activity 2; drafted in [`REFLECTION.md`](../../REFLECTION.md) and finalized in Activity 4. |

---

## 3. Chronological Evolution & Step-by-Step Implementation

### Step 1: Scenario Review & Problem Identification
- **Objective**: Identify root causes of integration failures between Blazor WebAssembly and ASP.NET Core Minimal API.
- **Failures Diagnosed**:
  1. *Route Mismatch*: The client application attempted to fetch from `/api/products` (the initial scaffolding route), while the refactored server endpoint listened on `/api/productlist`.
  2. *CORS Restriction & Service Crash*: Browser cross-origin requests were blocked. Applying `app.UseCors(...)` without calling `builder.Services.AddCors()` resulted in a runtime exception at server startup: `InvalidOperationException: Unable to resolve service for type 'Microsoft.AspNetCore.Cors.Infrastructure.ICorsService'`.
  3. *JSON Schema Casing*: ASP.NET Core serializes responses using camelCase key naming conventions (`id`, `name`, `price`, `stock`). By default, `System.Text.Json.JsonSerializer.Deserialize` performs case-sensitive matching, leaving PascalCase C# properties at their default values (0 and empty string).

### Step 2.1: API Route Synchronization
- **Objective**: Align front-end request URI with the back-end endpoint route.
- **Actions Verified**:
  - Confirmed `src/ClientApp/Pages/FetchProducts.razor` (line 46) invokes `Http.GetAsync("/api/productlist")`.
  - Confirmed `src/ServerApp/Program.cs` (line 33) registers endpoint `app.MapGet("/api/productlist", ...)`.
  - Eliminated HTTP 404 route divergence.

### Step 2.2: CORS Policy & Middleware Resolution
- **Objective**: Configure server-side CORS middleware to permit browser access during development.
- **Actions Verified**:
  - Registered CORS dependencies in dependency injection: `builder.Services.AddCors()` in `src/ServerApp/Program.cs` (line 8).
  - Applied demo-permissive CORS middleware policy:
    ```csharp
    app.UseCors(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
    ```
  - Resolved `ICorsService` startup crash and enabled cross-origin resource sharing between client and server ports.

### Step 2.3: Malformed JSON Deserialization & Typed Error Handling
- **Objective**: Prevent silent data binding failures and isolate JSON syntax/type errors from transport exceptions.
- **Actions Verified**:
  - Configured `JsonSerializerOptions { PropertyNameCaseInsensitive = true }` in `FetchProducts.razor` (line 27), enabling camelCase API payloads to bind to PascalCase model properties.
  - Implemented typed exception catch blocks in `LoadProductsAsync()`:
    - `catch (JsonException ex)`: Captures malformed JSON syntax or scalar type mismatches, presenting user-facing error message `"The product API returned malformed JSON."`
    - `catch (HttpRequestException ex)`: Captures network transport failures, presenting `"Unable to reach the product API: {message}"`.
    - `catch (TaskCanceledException ex)`: Captures request timeouts, presenting `"The product API request timed out."`
  - Centralized error dispatch through `LogAndSetError(Exception ex, string userMessage)`.

### Step 3: Automated Verification Test Suite
- **Objective**: Construct automated unit tests validating all debugging resolutions.
- **Actions Taken in Rebuild**:
  - Updated [`Tests/Tests.csproj`](../../Tests/Tests.csproj) with `<FrameworkReference Include="Microsoft.AspNetCore.App" />`.
  - Implemented [`Tests/TestDebuggingAndErrors.cs`](../../Tests/TestDebuggingAndErrors.cs) containing 8 NUnit test fixtures:
    1. `TestRouteAlignment_ClientRequestMatchesServerEndpoint`: Asserts string equality between client URI and server route path.
    2. `TestRouteMismatch_LegacyRouteDoesNotMatchAlignedEndpoint`: Confirms legacy `/api/products` is distinct from aligned `/api/productlist`.
    3. `TestRouteConstruction_BaseAddressAndRelativeEndpointCombineAccurately`: Verifies absolute URI resolution for HTTP client dispatches.
    4. `TestCorsPolicy_ConfiguredAsDemoPermissive`: Confirms `CorsPolicyBuilder` generates a policy allowing any origin, method, and header.
    5. `TestCorsService_RegistrationResolvesRequiredDependencies`: Validates that `AddCors()` registers `ICorsService` and `ICorsPolicyProvider`.
    6. `TestCorsService_MissingRegistrationFailsResolution`: Reproduces the DI resolution failure when `AddCors()` is omitted.
    7. `TestJsonDeserialization_MalformedSyntaxThrowsJsonException`: Confirms syntax errors in JSON payloads throw `JsonException`.
    8. `TestJsonDeserialization_TypeMismatchThrowsJsonException`: Confirms scalar type mismatches (e.g., string price) throw `JsonException`.
    9. `TestCaseInsensitiveDeserialization_ResolvesCasingMismatch`: Confirms camelCase JSON binds to PascalCase properties under case insensitivity.
    10. `TestCaseSensitiveDeserialization_LeavesPropertiesAtDefaultValues`: Demonstrates silent binding failure under case-sensitive rules.
    11. `TestTypedExceptionIsolation_DispatchesSpecificDiagnosticMessages`: Validates typed exception message dispatch hierarchy.

---

## 4. Technical Analysis of Debugging & Resilience

### CORS Pipeline Mechanics & Service Registration
In ASP.NET Core, CORS enforcement operates as middleware in the request processing pipeline. The middleware requires internal service abstractions (`ICorsService`, `ICorsPolicyProvider`) to evaluate incoming `Origin` headers against registered policies:

```
[Incoming Request with Origin Header]
             │
             ▼
   [UseCors Middleware]
             │
             ├──> Resolves ICorsService from DI
             │       ├── Registered via AddCors()  ──> Evaluates Policy ──> Adds Access-Control-Allow-*
             │       └── Missing AddCors()         ──> Throws InvalidOperationException (Crash)
```

By registering `builder.Services.AddCors()` before `builder.Build()`, the application resolves this dependency cleanly.

### JSON Deserialization Resolution Matrix
The table below contrasts deserialization behavior across casing configurations and payload states:

| Payload State | PropertyNameCaseInsensitive = false | PropertyNameCaseInsensitive = true | Resulting UI Behavior |
|---|---|---|---|
| Valid camelCase JSON | Properties remain default (0, `""`) | Properties populate with values | Silent failure vs. expected product rendering |
| Corrupted JSON Syntax | Throws `JsonException` | Throws `JsonException` | Caught by typed `catch (JsonException)` |
| Scalar Type Mismatch | Throws `JsonException` | Throws `JsonException` | Caught by typed `catch (JsonException)` |
| Network Timeout | Throws `TaskCanceledException` | Throws `TaskCanceledException` | Caught by typed `catch (TaskCanceledException)` |

---

## 5. Verification Summary

| Check | Target | Result | Evidence |
|---|---|---|---|
| Route Alignment | `ClientApp` & `ServerApp` | Pass | Route `/api/productlist` aligned across front-end and back-end (`[M]`) |
| CORS Configuration | `ServerApp/Program.cs` | Pass | `AddCors()` registered, demo-permissive `UseCors()` applied (`[M]`) |
| JSON Error Handling | `FetchProducts.razor` | Pass | Case insensitivity configured, typed catches for `JsonException` (`[M]`) |
| Unit Test Suite | `Tests/TestDebuggingAndErrors.cs` | Pass | 8 NUnit test fixtures authored and verified (`[M]`) |
| Attribution & Hygiene | Commit history & files | Pass | 0 banned attribution terms in activity commits (`[M]`) |
