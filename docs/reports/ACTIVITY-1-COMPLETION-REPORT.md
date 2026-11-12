# Activity 1 Completion Report · InventoryHub Capstone

| Document Metadata | Value |
|---|---|
| **Activity ID** | Course 7 Capstone · Activity 1 (C7-A1) |
| **Title** | Front-End & Back-End Integration, Data Models & Integration Test Suite |
| **Author** | Juan Diego S. (`88201583+jdsaire@users.noreply.github.com`) |
| **Specialization** | Microsoft Full Stack Developer Specialization |
| **Course** | Course 7: Full-Stack Integration |
| **Status** | Completed on branch `rebuild-a1-integration` |
| **Target Repository** | [`jdsaire/fullstack_c7_inventoryhub`](https://github.com/jdsaire/fullstack_c7_inventoryhub) |
| **Date** | November 2026 |

---

## 1. Executive Summary

Activity 1 of the InventoryHub Capstone establishes the architectural foundation for seamless communication between the client-side front-end application and the back-end API service. The objective of this activity was to scaffold the full-stack project solution, implement the data retrieval integration inside the Blazor WebAssembly client, configure the initial Minimal API back-end endpoint, and verify model binding and serialization behavior through an automated test fixture (`[M]`).

This rebuild adds automated integration test coverage (`Tests/Tests.csproj`, `Tests/TestIntegration.cs`) to formally validate the contract between client data models and server JSON responses, ensuring that error handling, model properties, and deserialization semantics behave reliably.

### Milestone Deliverables Achieved
- **Full-Stack Solution Architecture (`[M]`)**: Maintained unified solution [`src/FullStackSolution.slnx`](../../src/FullStackSolution.slnx) linking [`src/ClientApp/ClientApp.csproj`](../../src/ClientApp/ClientApp.csproj) (Blazor WebAssembly standalone), [`src/ServerApp/ServerApp.csproj`](../../src/ServerApp/ServerApp.csproj) (ASP.NET Core Minimal API), and [`Tests/Tests.csproj`](../../Tests/Tests.csproj) (NUnit test project).
- **Client-Side Integration Component (`[M]`)**: Verified [`src/ClientApp/Pages/FetchProducts.razor`](../../src/ClientApp/Pages/FetchProducts.razor) implementing `@page "/fetchproducts"`, structured HTML product rendering, state handling (`Loading...` and error states), and asynchronous data retrieval via `HttpClient`.
- **Minimal API Back-End Endpoint (`[M]`)**: Verified [`src/ServerApp/Program.cs`](../../src/ServerApp/Program.cs) providing the initial seed inventory data structure representing product models with identifiers, names, prices, and stock counts.
- **Automated Integration Test Fixture (`[M]`)**: Built [`Tests/TestIntegration.cs`](../../Tests/TestIntegration.cs) providing 6 automated NUnit test fixtures asserting property assignments, JSON serialization, case-insensitive deserialization, seed data validation, and malformed JSON error handling.

---

## 2. Course 7 Capstone Rubric Alignment

The table below quotes the six Course 7 Capstone rubric criteria verbatim from the course evaluation specifications and records how Activity 1 contributes to each criterion:

| Criterion | Points | Question | Activity 1 Scope & Repository Evidence |
|---|---|---|---|
| **C1** | 5 | Did you create a GitHub repository for your project? | Public repository [`jdsaire/fullstack_c7_inventoryhub`](https://github.com/jdsaire/fullstack_c7_inventoryhub) established with clean branch history, solution architecture in [`src/FullStackSolution.slnx`](../../src/FullStackSolution.slnx), and standard root [`.gitignore`](../../.gitignore) (`[M]`). |
| **C2** | 5 | Did you generate and refine integration code for front-end and back-end communication? | Integration code generated and refined in [`src/ClientApp/Pages/FetchProducts.razor`](../../src/ClientApp/Pages/FetchProducts.razor) (`LoadProductsAsync`, `OnInitializedAsync`, typed error handling) and back-end endpoint in [`src/ServerApp/Program.cs`](../../src/ServerApp/Program.cs), verified via automated unit test suite in [`Tests/TestIntegration.cs`](../../Tests/TestIntegration.cs) (`[M]`). |
| **C3** | 5 | Did you debug and resolve integration issues effectively using the AI assistant? | Out of scope for Activity 1 baseline; addressed in Activity 2 (`docs/reports/ACTIVITY-2-COMPLETION-REPORT.md`) covering route alignment, CORS policy registration, and casing mismatch debugging. |
| **C4** | 5 | Did you create and implement JSON structures for API communication? | Out of scope for Activity 1 baseline; addressed in Activity 3 (`docs/reports/ACTIVITY-3-COMPLETION-REPORT.md`) covering nested category schema validation. |
| **C5** | 5 | Did you optimize the integration code for performance using the AI assistant? | Out of scope for Activity 1 baseline; addressed in Activity 4 (`docs/reports/ACTIVITY-4-COMPLETION-REPORT.md`) covering back-end in-memory caching and client error-handler consolidation. |
| **C6** | 5 | Did you include a reflective summary explaining how the AI assistant assisted in each step? | Out of scope for Activity 1 baseline; drafted in [`REFLECTION.md`](../../REFLECTION.md) and finalized in Activity 4. |

---

## 3. Chronological Evolution & Step-by-Step Implementation

### Step 1: Base Application Setup
- **Objective**: Establish the core application directories, project files, and solution structure.
- **Actions Verified**:
  - Scaffolding of `ClientApp` (Blazor WebAssembly standalone) under `src/ClientApp/`.
  - Scaffolding of `ServerApp` (ASP.NET Core Minimal API) under `src/ServerApp/`.
  - Solution file `src/FullStackSolution.slnx` linking the projects.
  - Initial Minimal API endpoint in `ServerApp/Program.cs` returning anonymous seed inventory arrays.

### Step 2: Front-End Integration Code
- **Objective**: Implement the front-end Blazor component to retrieve and render inventory data.
- **Actions Verified**:
  - Created component `src/ClientApp/Pages/FetchProducts.razor` mapped to route `/fetchproducts`.
  - Defined C# model class `Product` with properties `Id`, `Name`, `Price`, and `Stock`.
  - Implemented asynchronous API communication using injected `HttpClient Http` inside lifecycle method `OnInitializedAsync`.

### Step 3: Refinement and Error Handling
- **Objective**: Refactor the initial fetch logic into a dedicated method with structured error handling.
- **Actions Verified**:
  - Separated network dispatch into `LoadProductsAsync()` to keep component lifecycle clean and readable.
  - Implemented structured error capture for transport failures (`HttpRequestException`), request timeouts (`TaskCanceledException`), and invalid payload formats (`JsonException`).
  - Added user feedback bindings (`errorMessage`) so failure states present clear messages in the UI rather than silent failures.

### Step 4: Automated Verification Test Fixture
- **Objective**: Provide automated test validation of integration contracts and deserialization.
- **Actions Taken in Rebuild**:
  - Created `Tests/Tests.csproj` configured for test execution.
  - Implemented `Tests/TestIntegration.cs` with 6 unit test fixtures covering:
    1. `TestProductModel_PropertiesAndInstantiation`: Confirms property setters, getters, and initial null category state.
    2. `TestProductSerialization_ProducesExpectedJson`: Asserts standard JSON string serialization against contract.
    3. `TestProductDeserialization_BindsCaseInsensitively`: Confirms camelCase JSON payloads deserialize accurately into PascalCase properties.
    4. `TestProductDeserialization_CaseSensitive_FailsToBind`: Demonstrates that case-sensitive deserialization causes silent property default initialization, validating the necessity of `PropertyNameCaseInsensitive = true`.
    5. `TestMalformedJson_ThrowsJsonException`: Confirms invalid JSON syntax or mismatched scalar types throw `JsonException`.
    6. `TestEmptyJsonArray_ReturnsEmptyCollection`: Verifies graceful handling of empty collection payloads (`[]`).

---

## 4. Technical Analysis of Integration Architecture

### Front-End Lifecycle & Data Retrieval Pipeline
The client integration follows the Blazor component lifecycle:
1. `OnInitializedAsync()` executes when the component is created.
2. `LoadProductsAsync()` issues an asynchronous GET request via `HttpClient`.
3. `response.EnsureSuccessStatusCode()` verifies HTTP 2xx response codes.
4. `response.Content.ReadAsStringAsync()` retrieves the JSON body.
5. `JsonSerializer.Deserialize<Product[]>` deserializes the payload into strongly-typed `Product` entities.

### Error Isolation Matrix
```
[HTTP GET Request]
        │
        ├──> HttpRequestException  --> "Unable to reach the product API: <message>"
        ├──> TaskCanceledException --> "The product API request timed out."
        └──> JsonException         --> "The product API returned malformed JSON."
```

By isolating `JsonException` from network transport exceptions, the application provides actionable diagnostics when the back-end payload structure deviates from expected schemas.

---

## 5. Verification Summary

| Check | Target | Result | Evidence |
|---|---|---|---|
| Project Structure | `src/FullStackSolution.slnx` | Pass | Solution links `ClientApp`, `ServerApp`, and `Tests` (`[M]`) |
| Component Implementation | `FetchProducts.razor` | Pass | Route `/fetchproducts`, table markup, model bindings (`[M]`) |
| Endpoint Implementation | `ServerApp/Program.cs` | Pass | Seed data returned from API endpoint (`[M]`) |
| Unit Test Suite | `Tests/TestIntegration.cs` | Pass | 6 NUnit test fixtures authored and verified (`[M]`) |
| Attribution & Hygiene | Commit history & files | Pass | 0 banned attribution terms in activity commits (`[M]`) |
