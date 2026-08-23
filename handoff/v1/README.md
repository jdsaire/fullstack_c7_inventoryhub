# handoff / v1

The first and only build of InventoryHub so far: from a repository holding only unrelated Azure
debris, through four gated Activities, to a working local application — plus an attempted
deployment that was deferred mid-build.

| File | What it is |
|---|---|
| [plan.md](plan.md) | The build plan **as approved before execution**, plus the mid-build amendment approved when deployment scope was cut. Neither edit rewrote the other. |
| [completion-report.md](completion-report.md) | What actually happened: the full commit list, the results against every success criterion, the deviations and why they were taken, the decisions made without asking, and the items left open. |

**Read the completion report first** if you want to know the state of the project. Read the plan
alongside it if you want to know how it was reasoned about beforehand.

## The short version

20 commits — one establishing `main`, 19 on the deploy branch — gathered into pull request
**#1**, open and unmerged. The build was clean after every commit. The JSON contract, frozen at
Activity 3, held unchanged through the final state. No AI product is named anywhere in the
repository. Deployment was attempted — the Render Web Service was created, a real dashboard
configuration issue was root-caused and fixed, but the deploy still failed and was deferred by
the project owner's decision to a future session, outside this course's scope. That deferral did
not affect grading readiness: the 30-point rubric never required a working deployment.

## Related

- [completion-report.md](completion-report.md) · [plan.md](plan.md)
- [../README.md](../README.md) — the handoff index
- [../../README.md](../../README.md) — the project README
- [../../docs/README.md](../../docs/README.md) — the reference documentation
