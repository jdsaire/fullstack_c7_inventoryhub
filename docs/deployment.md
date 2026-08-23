# Deployment

InventoryHub does not have a live deployment as of this repository's current state. The plan
was GitHub Pages for the front-end (ClientApp) and Render, as a Docker-based Free Web Service,
for the back-end (ServerApp). Reaching that plan took a detour through Azure first, and then a
real, unresolved failure on Render itself — both recorded here because they're genuinely what
happened, not a hypothetical. Deployment was deferred to a future session, outside this
capstone's scope; see "Current status" at the end of this file for exactly where it stands.

## How the hosting choice was reached

The evaluation started with Azure App Service on the Free F1 tier for the back-end, paired
with GitHub Pages for the front-end — Pages was never in question, only where ServerApp would
run. The first resource created in Azure's portal, though, was an Azure Static Web App, picked
because its name was the closest match to "Web App" in the resource picker. That turned out to
be the wrong resource type entirely: Static Web Apps serve static files plus, optionally, Azure
Functions — they have no way to host an ASP.NET Core Minimal API as-is. Before it was
abandoned, that Static Web App resource had already auto-committed a GitHub Actions workflow
and a one-line demo stub straight to this repository's `main` branch. Removing that debris was
the first action of this build (see the scaffold commit's history-reset).

An actual Azure App Service "Web App" resource was configured next — named
`c7-inventoryhub-api`, Free F1 tier, Linux, running .NET 10 (LTS), in the Chile Central region
(chosen over Brazil South for proximity to Lima, Peru, where this project's owner is based).
Basic Authentication was left disabled, which is Azure's current secure-by-default setting for
new App Service resources — not a choice made against Azure's recommendation, just what a new
resource starts with.

That default became the reason the path stalled. With Basic Authentication disabled, the
simpler GitHub Actions deployment method — publish-profile deployment — isn't usable, since it
depends on Basic Authentication being enabled. The documented alternative was a Microsoft Entra
ID app registration with a federated OIDC credential trusting GitHub Actions, plus a Website
Contributor role assignment scoped to just this one Web App. That path was walked through step
by step — the app registration, the federated credential, the IAM role assignment — but never
completed. Basic Authentication itself was never re-enabled at any point; it stayed in its
default disabled state the entire time. To be precise about what this means: no security
trade-off was made or accepted here. The Azure Web App resource was never actually created —
only configured in the portal form — so there was nothing to weaken. The question became moot
once Azure was dropped, not because a less secure setting was chosen along the way.

Weighed against the scope of a graded course capstone, the cumulative friction was the deciding
factor: a resource-type mixup (Static Web App vs. Web App) caused by portal naming, a portal
navigation path that had visibly shifted since the reference documentation was written, and a
multi-step Entra ID/OIDC identity setup just to get a working deploy pipeline. None of that is
disqualifying for a real production system, but it's disproportionate to what this project
needs. The decision was to abandon the Azure path entirely, not to work around any one piece of
it.

Render was chosen instead, and the contrast is really about removing an entire category of
friction rather than trading one small annoyance for another: one Web Service creation form, no
separate identity or app-registration resource to configure, GitHub-native account signup and
repository connection, no credit card required for the free instance tier, and git-push-
triggered auto-deploy that needs no GitHub Actions workflow at all for the back-end (unlike the
Azure path, which would have needed one either way).

The Azure exploration itself isn't treated as wasted effort — it's kept here on record as
documented learning that will matter for a future DevOps-focused course, even though it isn't
part of this capstone's own hosting solution.

## Render's Free-tier trade-offs

The Free Web Service instance sleeps after roughly 15 minutes of inactivity. The first request
after a period of sleep takes about 30-60 seconds to wake the instance and respond — this is
expected behavior for the free tier, not a bug in ServerApp. It only affects the first request
after idle time; every request after that is normal speed until the instance sleeps again. (This
is documented ahead of actually confirming it live — see below.)

## The placeholder URL mechanism

Until this project's Render Web Service has a working, confirmed-live URL, ClientApp's API base
URL configuration ships with the literal string `https://REPLACE-WITH-RENDER-URL.onrender.com`
— a value that visibly fails if it's ever left in place, rather than a plausible-looking fake
URL that could be mistaken for a working one. It's labeled inline as a placeholder awaiting the
real value.

## What actually happened when Render deployment was attempted

The Render Web Service itself was created successfully, and its assigned URL
(`https://fullstack-c7-inventoryhub.onrender.com`) was wired into `appsettings.json` in place of
the placeholder. The build then failed with `failed to solve: failed to read dockerfile: open
Dockerfile: no such file or directory`.

The root cause, confirmed against Render's own documentation
([render.com/docs/docker](https://render.com/docs/docker),
[render.com/docs/monorepo-support](https://render.com/docs/monorepo-support)): Render's
**Dockerfile Path** field is resolved relative to the repository root, regardless of the
service's separately configured **Root Directory** setting — the two fields don't share a base
path, which isn't obvious from the dashboard's field labels alone. `Root Directory` (set to
`src/ServerApp`) correctly scopes the Docker build context, so this repository's Dockerfile
itself, with `COPY` paths relative to `src/ServerApp`, was correct as written — but the
Dockerfile Path field needed the repo-root-relative value `src/ServerApp/Dockerfile`, not the
Root-Directory-relative value `Dockerfile` that had been entered initially.

That correction was applied, but the deploy still failed after repeated retries. With the cause
of the continued failure unresolved, and given the scope of a graded course capstone, the
decision was made to defer completing the Render deployment to a future session, worked outside
this course's scope. `appsettings.json`'s `ApiBaseUrl` was reverted to the literal placeholder
string, since a real-looking URL that doesn't actually respond would misrepresent the project's
working state more than an explicit placeholder does.

## Current status

Not deployed. The Dockerfile, this document, and the config-wiring mechanism are retained as a
documented attempt — in the same spirit as the Azure exploration above — not discarded as
wasted effort. `ApiBaseUrl` is back at the placeholder string. Anyone wanting to run
InventoryHub today runs it locally; see [how-to-run.md](how-to-run.md).
