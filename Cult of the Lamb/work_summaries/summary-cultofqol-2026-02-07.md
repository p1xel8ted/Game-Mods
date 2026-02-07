# CultOfQoL Work Summary - 2026-02-07

## Task: Mass Action Notification Consolidation

### Goal
When mass follower actions affect many followers, consolidate per-follower notifications into a single summary notification instead of flooding the notification panel.

### Changes Made

**New config:**
- `MassNotificationThreshold` (int, default 3, range 0-50) in Mass Follower section
- When affected count > threshold, individual notifications suppressed, single summary shown
- Set to 0 to always show summary

**Implementation:**
- Uses game's own `NotificationCentre.NotificationsEnabled` static flag (same pattern as `BlizzardMonster`)
- `NotifySuppressBegin`/`NotifySuppressEnd` helper methods in `FollowerPatches`
- All 10 mass action loops + mass level up coroutine wrapped with suppression
- Summary shows e.g. "Blessed 15 followers" with Positive flair

### Files Modified
- `CultOfQoL/Configs.cs` — Added `MassNotificationThreshold` property
- `CultOfQoL/Plugin.cs` — Bound config (Order 14, top of Mass Follower section)
- `CultOfQoL/Patches/Followers/FollowerPatches.cs` — Added helpers, wrapped all 11 mass action loops
- `Thunderstore/cult/CHANGELOG.md` — Added to 2.4.1 entry
- `Thunderstore/cult/README.md` — Added to Mass Actions feature list
- `Thunderstore/cult/nexusmods_description.txt` — Added to Mass Actions feature list (BBCode)

### Key Decisions
- Default threshold of 3 (most useful with larger cults)
- Faith modifications still apply correctly — only UI notification display is suppressed
- Lazy LINQ evaluation changed to `.ToList()` to get count before loop starts
- No restart required for threshold changes (applies immediately)

### Open Issues
- None
