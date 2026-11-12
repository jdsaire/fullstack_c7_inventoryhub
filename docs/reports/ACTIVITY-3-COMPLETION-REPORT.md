# Activity 3 Completion Report · InventoryHub Capstone

| Document Metadata | Value |
|---|---|
| **Activity ID** | Course 7 Capstone · Activity 3 (C7-A3) |
| **Title** | Standardized JSON Structures, Nested Category Models, and Live Interactive Demo Hub |
| **Author** | Juan Diego S. (`88201583+jdsaire@users.noreply.github.com`) |
| **Specialization** | Microsoft Full Stack Developer Specialization |
| **Course** | Course 7: Full-Stack Integration |
| **Status** | Completed on branch `rebuild-a3-json` |
| **Target Repository** | [`jdsaire/fullstack_c7_inventoryhub`](https://github.com/jdsaire/fullstack_c7_inventoryhub) |
| **Date** | November 2026 |

---

## 1. Executive Summary

Activity 3 of the InventoryHub Capstone establishes structured JSON communication schemas between the front-end client and the back-end API service. While initial scaffolding in Activity 1 and debugging in Activity 2 established flat product attributes (`id`, `name`, `price`, `stock`), enterprise integration architectures require hierarchical domain models to represent relational associations—such as categorizing inventory items into dedicated departments (e.g., Electronics, Accessories, Home Office).

In this rebuild activity, two major milestones were achieved:
1. **Hierarchical JSON Schema & Deserialization Contracts (`[M]`)**: Implemented automated unit test coverage in [`Tests/TestJsonStructures.cs`](../../Tests/TestJsonStructures.cs) asserting that Minimal API responses serialize nested `Category` objects (`Id`, `Name`) and that Blazor WebAssembly client models bind these nested graphs with complete type safety, case insensitivity, and null safety.
2. **SafeVault-Style Live Interactive Demo (`[M]`)**: In accordance with project governance (DECISION 2=1), accelerated the delivery of the complete multi-page WCAG 2.1 AAA accessible dark-theme web application to GitHub Pages (`index.html`, `products.html`, `tests.html`, `css/styles.css`, `data/products.json`, `.nojekyll`), providing an immediate in-browser exploration interface without requiring local execution.

### Milestone Deliverables Achieved
- **Nested Category Domain Model (`[M]`)**: Maintained strongly-typed `Category` class (`Id`, `Name`) bound within `Product.Category?` in [`src/ClientApp/Pages/FetchProducts.razor`](../../src/ClientApp/Pages/FetchProducts.razor) and emitted by [`src/ServerApp/Program.cs`](../../src/ServerApp/Program.cs).
- **Automated Contract Test Suite (`[M]`)**: Created [`Tests/TestJsonStructures.cs`](../../Tests/TestJsonStructures.cs) with 8 automated NUnit test methods verifying schema completeness, nested object binding, null safety, contract serialization, and malformed scalar handling.
- **GitHub Pages Multi-Page Demo Hub (`[M]`)**: Delivered [`index.html`](../../index.html) (Mission Hub & Architecture Overview), [`products.html`](../../products.html) (Interactive Catalog with dynamic search and category filtering), and [`tests.html`](../../tests.html) (In-Browser Test Runner) styled after Course 8 SafeVault standards.
- **Simulated Inventory Snapshot (`[SIM]`)**: Created bundled [`data/products.json`](../../data/products.json) snapshot badged `[SIM]` powering client-side exploration on GitHub Pages.
- **Documentation Alignment (`[M]`)**: Updated [`README.md`](../../README.md) to feature live GitHub Pages demo links, remove legacy "run locally" phrasing, and align the disclosure heading.

---

## 2. Course 7 Capstone Rubric Alignment

The table below quotes the six Course 7 Capstone rubric criteria verbatim from the course evaluation specifications and records how Activity 3 satisfies each criterion:

| Criterion | Points | Question | Activity 3 Scope & Repository Evidence |
|---|---|---|---|
| **C1** | 5 | Did you create a GitHub repository for your project? | Public repository [`jdsaire/fullstack_c7_inventoryhub`](https://github.com/jdsaire/fullstack_c7_inventoryhub) maintained on branch `rebuild-a3-json` with clean commit boundaries (`[M]`). |
| **C2** | 5 | Did you generate and refine integration code for front-end and back-end communication? | Integration code verified in [`src/ClientApp/Pages/FetchProducts.razor`](../../src/ClientApp/Pages/FetchProducts.razor) and [`src/ServerApp/Program.cs`](../../src/ServerApp/Program.cs), with live interactive demonstration on GitHub Pages (`[M]`). |
| **C3** | 5 | Did you debug and resolve integration issues effectively using the AI assistant? | Verified in Activity 2; test coverage maintained in [`Tests/TestDebuggingAndErrors.cs`](../../Tests/TestDebuggingAndErrors.cs) (`[M]`). |
| **C4** | 5 | Did you create and implement JSON structures for API communication? | Standardized JSON schema containing top-level product fields and nested `Category` objects (`Id`, `Name`) implemented in API and front-end; validated via automated test suite in [`Tests/TestJsonStructures.cs`](../../Tests/TestJsonStructures.cs) (`[M]`). |
| **C5** | 5 | Did you optimize the integration code for performance using the AI assistant? | Out of scope for Activity 3; addressed in Activity 4 (`docs/reports/ACTIVITY-4-COMPLETION-REPORT.md`) covering back-end in-memory caching and client error-handler consolidation. |
| **C6** | 5 | Did you include a reflective summary explaining how the AI assistant assisted in each step? | Out of scope for Activity 3; drafted in [`REFLECTION.md`](../../REFLECTION.md) and finalized in Activity 4. |

---

## 3. Chronological Evolution & Step-by-Step Implementation

### Step 1: Scenario Review & Schema Definition
- **Objective**: Review the course requirement for structuring product details with relational category metadata.
- **Contract Specification**:
  - Top-level scalar properties: `id` (integer), `name` (string), `price` (floating-point number), `stock` (integer).
  - Nested category object: `category` with properties `id` (integer) and `name` (string).
  - Schema sample:
    ```json
    {
      "id": 1,
      "name": "Laptop",
      "price": 1200.50,
      "stock": 25,
      "category": {
        "id": 101,
        "name": "Electronics"
      }
    }
    ```

### Step 2.1: Server-Side Nested Endpoint Modification
- **Objective**: Update back-end `/api/productlist` to emit structured category objects.
- **Actions Verified**:
  - `src/ServerApp/Program.cs` returns anonymous objects incorporating nested category instances:
    - Item 1 (`Laptop`): `Category = new { Id = 101, Name = "Electronics" }`
    - Item 2 (`Headphones`): `Category = new { Id = 102, Name = "Accessories" }`

### Step 2.2: Front-End Model Binding
- **Objective**: Enable Blazor client models to parse and display nested category details.
- **Actions Verified**:
  - Defined C# model `public class Category { public int Id { get; set; } public string Name { get; set; } = string.Empty; }` inside `FetchProducts.razor`.
  - Added property `public Category? Category { get; set; }` to `Product` model class.
  - Updated template rendering to display `@product.Category?.Name` with safe navigation operator.

### Step 3: Automated Verification Test Suite
- **Objective**: Provide automated test assertions for all schema contracts.
- **Actions Taken in Rebuild**:
  - Authored [`Tests/TestJsonStructures.cs`](../../Tests/TestJsonStructures.cs) with 8 NUnit unit test methods:
    1. `TestJsonSchema_ContainsAllStandardProperties`: Asserts scalar and object field presence.
    2. `TestNestedCategoryModel_DeserializesAccurately`: Asserts accurate binding of nested category properties.
    3. `TestMultipleProducts_DistinctNestedCategories`: Validates multiple records across category boundaries.
    4. `TestCategoryNullSafety_GracefullyHandlesMissingCategory`: Confirms nullable safety when `category` is null.
    5. `TestCategoryModel_DirectInstantiationAndProperties`: Validates model property setters and getters.
    6. `TestJsonSerialization_IncludesNestedCategoryStructure`: Confirms serialization contract parity.
    7. `TestCaseInsensitiveNestedBinding`: Verifies camelCase nested fields map to PascalCase C# properties.
    8. `TestMalformedNestedCategory_ThrowsJsonException`: Verifies non-object scalar values trigger `JsonException`.

### Step 4: Multi-Page Dark-Theme Live Demo Delivery (DECISION 2=1)
- **Objective**: Provide an interactive web application on GitHub Pages mirroring Course 8 SafeVault standards.
- **Actions Taken in Rebuild**:
  - Configured root [`.nojekyll`](../../.nojekyll) for GitHub Pages asset delivery.
  - Built dark-theme stylesheet [`css/styles.css`](../../css/styles.css) (WCAG 2.1 AAA accessible, `#090d16` background).
  - Authored [`index.html`](../../index.html) overview hub with architecture metrics and navigation cards.
  - Authored [`products.html`](../../products.html) interactive catalog with category filters and search.
  - Authored [`tests.html`](../../tests.html) interactive in-browser test suite runner.
  - Created bundled [`data/products.json`](../../data/products.json) snapshot badged `[SIM]`.

---

## 4. Technical Analysis: Nested Object Deserialization

### System.Text.Json Object Graph Resolution
In modern .NET runtime environments, `System.Text.Json` resolves nested object graphs hierarchically:
```
JSON Payload                     C# Strong-Typed Model
┌───────────────────────┐        ┌─────────────────────────┐
│ {                     │        │ class Product           │
│   "id": 1,            │ ────>  │   int Id                │
│   "name": "Laptop",   │ ────>  │   string Name           │
│   "category": {       │        │   Category? Category    │
│     "id": 101,        │ ────>  │     ├── int Id          │
│     "name": "Electr." │ ────>  │     └── string Name     │
│   }                   │        │ }                       │
│ }                     │        └─────────────────────────┘
└───────────────────────┘
```

Because `System.Text.Json` maps nested properties automatically when target types match, no custom `JsonConverter` is required. Configuring `PropertyNameCaseInsensitive = true` ensures both root properties and nested category fields map cleanly across casing conventions.

---

## 5. Verification Summary

| Check | Target | Result | Evidence |
|---|---|---|---|
| Schema Implementation | `ServerApp/Program.cs` | Pass | Seed data returns nested Category objects (`[M]`) |
| Client Binding | `FetchProducts.razor` | Pass | `Category` class and `Product.Category?` rendered (`[M]`) |
| Automated Tests | `Tests/TestJsonStructures.cs` | Pass | 8 NUnit test fixtures authored and verified (`[M]`) |
| Interactive Demo | `index.html`, `products.html`, `tests.html` | Pass | Full multi-page dark-theme web app on GitHub Pages (`[M]`) |
| Data Layer Badging | `data/products.json` & UI | Pass | Persistent `[SIM]` badging displayed across all views (`[M]`) |
| Attribution & Hygiene | Commit history & files | Pass | 0 banned attribution terms in activity commits (`[M]`) |
