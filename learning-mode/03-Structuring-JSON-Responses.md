# 3. Structuring JSON responses

Activity 3's job: add a nested `Category` object to `/api/productlist`, one commit —
`feat(activity3): add nested category object to JSON response and front-end model`.

## Back-end: nesting an anonymous object

[Program.cs](../src/ServerApp/Program.cs)'s response gained a `Category = new { Id = ..., Name =
... }` property on each product — an anonymous object nested inside another anonymous object.
ASP.NET Core's default JSON serialization handles this with no extra configuration; the nesting
in C# maps directly to nesting in the JSON output.

## Front-end: a matching nested class

[FetchProducts.razor](../src/ClientApp/Pages/FetchProducts.razor) gained a `Category` class
(`Id`, `Name`) and a `Category?` property on `Product`. `System.Text.Json` maps a nested JSON
object onto a nested C# class automatically, as long as the class's shape mirrors the response's
nested object — no custom converter needed, which is exactly why this was a pure model change
with no changes to the deserialization call itself.

## The contract freeze

From this commit onward, `/api/productlist`'s shape — `id`, `name`, `price`, `stock`, and a
nested `category` object with `id`/`name` — does not change field names or nesting for the rest
of the build. Activity 4's optimization pass only touches performance characteristics (caching,
call frequency), never this shape. Verified directly with `curl` against the running server:

```json
[{"id":1,"name":"Laptop","price":1200.5,"stock":25,"category":{"id":101,"name":"Electronics"}},
 {"id":2,"name":"Headphones","price":50,"stock":100,"category":{"id":102,"name":"Accessories"}}]
```
