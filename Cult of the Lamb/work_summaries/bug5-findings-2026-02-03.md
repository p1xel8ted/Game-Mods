# Bug #5 Investigation Findings (Interaction Wheel Stays Open)

## Key Findings
- The wheel is hidden immediately when a command is chosen via `UIFollowerInteractionWheelOverlayController.MakeChoice`, which calls `Hide()` before invoking `OnItemChosen`. If the wheel appears later, it is being reopened, not simply never closed.
- Several follower routines end by scheduling callbacks (e.g., `AddAdoration(...)`, `ResourceCustomTarget.Create(...callback...)`) that invoke `Close()` later, after the coroutine has already finished.
- The mass-action wrapper decrements the active routine counter when the coroutine completes, not when the callback chain completes.
- When the counter reaches zero, `MassActionCurrentlyRunning` is set false and the original interaction is closed via `OriginalMassActionInteraction.Close(...)`.
- If the original follower’s later callback calls `Close()` after the mass action flag is cleared, the `Close` prefix no longer forces `reshowMenu = false`, so the vanilla `Close()` path can reshow the interaction menu (wheel reopens).

## Why This Matches the Symptom
- The wheel is hidden immediately on selection.
- It reappears after the original interaction’s callback fires (late `Close()` call with default `reshowMenu: true`).
- Users can move around while the wheel remains open, consistent with the reshow path.

## Secondary Risk
- `BeginMassAction` uses `followers.Count` even if some followers lack `Interaction_FollowerInteraction`; if any routines don’t start, the counter may never reach zero and the original interaction never closes.

## Likely Root Cause
`MassActionCurrentlyRunning` flips to false before callback-based routines complete, allowing late `Close()` calls to reopen the interaction wheel.

## Fix Directions
1) Suppress reshow specifically for the original interaction instance even after mass action ends (track instance and force `reshowMenu = false` in `Close` prefix).
2) Avoid decrementing the counter at coroutine end for callback-based routines; decrement on callback completion or when `Close()` is invoked for that interaction.
3) Hide the original wheel immediately in `BeginMassAction` (still need to prevent reshow from late callbacks).

## Relevant Files
- `CultOfQoL/Patches/Followers/FollowerPatches.cs`
- `CultOfQoL/Core/Routines/RoutineTranspilers.cs`
- `libs/game_code/Assembly-CSharp/interaction_FollowerInteraction.cs`
- `libs/game_code/Assembly-CSharp/Lamb/UI/FollowerInteractionWheel/UIFollowerInteractionWheelOverlayController.cs`

---

## Additional Findings (Claude Session 2)

### OnInteract Transpiler Observation

The `OnInteract` transpiler in `RoutineTranspilers.cs` (lines 60-79) checks `AnyMassActionsEnabled` at **patch time**, not runtime:

```csharp
private static IEnumerable<CodeInstruction> interaction_FollowerInteraction_OnInteract(...)
{
    if (!AnyMassActionsEnabled) return originalCodes;  // <-- PATCH TIME CHECK
    // NOPs DepthOfFieldTween for ALL interactions
}
```

If any mass action config is enabled at game startup, `DepthOfFieldTween` is removed from ALL `OnInteract` calls.

### NullReferenceException Spam - ⚠️ UNVERIFIED

Initial hypothesis: The OnInteract transpiler causes NRE by breaking state initialization.

**Codex validation:** NOT CONFIRMED. The transpiler only NOPs a visual effect call (`DepthOfFieldTween`); it doesn't alter interaction state. The NRE root cause requires separate investigation.

**Status:** NRE root cause is **unknown**. Removing the OnInteract transpiler is optional (visual-only change, not a proven fix).

---

## Consolidated Fix Plan for Bug #5 (Wheel Reopening)

### Fix 1: Track Original Interaction by Instance ✅ VALIDATED
**File:** `FollowerPatches.cs`

Current problem: `OriginalMassActionInteraction` is cleared when counter hits zero, but the original follower's callbacks may still fire later.

**Change:** Keep tracking `OriginalMassActionInteraction` beyond mass action completion. Clear it only when `Close()` is called on that specific instance.

### Fix 2: Force reshowMenu=false for Original Interaction Instance ✅ VALIDATED
**File:** `FollowerPatches.cs`

Change the `Close` prefix to check instance identity, not just `MassActionCurrentlyRunning`:

```csharp
[HarmonyPrefix]
[HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.Close), typeof(bool), typeof(bool), typeof(bool))]
public static void interaction_FollowerInteraction_Close(
    interaction_FollowerInteraction __instance,
    ref bool DoResetFollower, ref bool unpause, ref bool reshowMenu)
{
    // Check if this is the original mass action interaction (even after mass action "completes")
    if (__instance == OriginalMassActionInteraction)
    {
        DoResetFollower = true;
        unpause = true;
        reshowMenu = false;

        // Now safe to clear the reference
        OriginalMassActionInteraction = null;
    }
    // Also handle active mass action case for other followers
    else if (RoutinesTranspilers.MassActionCurrentlyRunning)
    {
        DoResetFollower = true;
        unpause = true;
        reshowMenu = false;
    }
}
```

### Fix 3: Only Count Followers With Valid Interactions ✅ VALIDATED
**File:** `FollowerPatches.cs`

In each mass action block, filter to only followers with non-null `Interaction_FollowerInteraction` before calling `BeginMassAction`:

```csharp
var validFollowers = followers.Where(f => f.Interaction_FollowerInteraction != null).ToList();
if (validFollowers.Count == 0) return;
BeginMassAction(validFollowers.Count, __instance);
```

### Fix 4: Keep Force-Close in DecrementMassActionCounter ⚠️ ADJUSTED
**File:** `FollowerPatches.cs`

**Original proposal:** Remove the force-close call.

**Codex feedback:** Removing force-close could leave the wheel open if the original routine never closes (or gets blocked).

**Adjusted approach:** Keep the force-close but ensure `reshowMenu` is forced false for the original instance via the `Close` prefix (Fix 2 handles this). The force-close acts as a safety net.

```csharp
private static void DecrementMassActionCounter()
{
    RoutinesTranspilers.ActiveMassRoutineCount--;
    if (RoutinesTranspilers.ActiveMassRoutineCount <= 0)
    {
        RoutinesTranspilers.MassActionCurrentlyRunning = false;
        RoutinesTranspilers.ActiveMassRoutineCount = 0;

        // Keep force-close as safety net, but Fix 2 ensures reshowMenu=false
        if (OriginalMassActionInteraction != null)
        {
            OriginalMassActionInteraction.Close(true, true, false);
            // Don't clear OriginalMassActionInteraction here -
            // let the Close prefix handle it in case of late callbacks
        }

        Plugin.WriteLog("[MassAction] All routines completed.");
    }
}
```

### Optional: Remove OnInteract Transpiler 🔶 OPTIONAL/VISUAL
**File:** `RoutineTranspilers.cs`

Removing the `interaction_FollowerInteraction_OnInteract` transpiler would restore depth-of-field effects during mass actions. This is a visual improvement, not a bug fix.

---

## Summary of Changes

| File | Change | Status |
|------|--------|--------|
| `FollowerPatches.cs` | Track original interaction by instance | ✅ Validated |
| `FollowerPatches.cs` | Force `reshowMenu=false` in `Close` prefix for original instance | ✅ Validated |
| `FollowerPatches.cs` | Filter followers with valid interactions before counting | ✅ Validated |
| `FollowerPatches.cs` | Keep force-close but rely on prefix for reshowMenu | ⚠️ Adjusted |
| `RoutineTranspilers.cs` | Remove OnInteract transpiler | 🔶 Optional |

---

## Expected Outcome

1. **Wheel stays closed** - Original interaction tracked by instance; late callbacks still get `reshowMenu=false`
2. **Counter accuracy** - Only valid interactions counted, prevents stuck states
3. **Safety net** - Force-close in `DecrementMassActionCounter` prevents indefinitely open wheels

## Outstanding Issue

**NRE spam** - Root cause unknown. Requires separate investigation. Not addressed by Bug #5 fixes.
