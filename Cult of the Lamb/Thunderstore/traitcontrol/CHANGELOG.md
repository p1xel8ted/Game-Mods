### 0.1.5 - 31/01/2026

* Added "Reset All Settings" button to reset all configuration to defaults (vanilla behavior).
* Added "Minimum Traits" and "Maximum Traits" config options - configure how many traits new followers receive (vanilla is 2-3, now configurable 2-8). Also applies to re-indoctrination which normally caps at 6.
* Added "Randomize Traits on Re-indoctrination" option - randomize follower traits when re-indoctrinating at the altar (vanilla only changes appearance/name).
* Fixed "Use Unlocked Traits Only" - now correctly filters by game progression requirements (Tailor unlocked, Pleasure district enabled, etc.) instead of incorrectly checking cult-wide doctrine traits.
* Fixed potential crash on main menu when no save is loaded.
* Fixed infinite loop when "Use Unlocked Traits Only" was enabled with certain config combinations.
* Fixed exclusive trait replacement for negative-to-negative pairs (e.g., Aestivation/Hibernation) - now correctly falls back to random positive trait.
* Trait weights menu now hides traits that aren't available based on game progression when "Use Unlocked Traits Only" is enabled.
* "Include Event Traits" now excludes negative event traits when "Enable Trait Replacement" is also enabled.
* Event traits in the trait replacement pool now respect the "Include Event Traits" config (only positive event traits are used as replacements when enabled).
* Added warning when not enough traits are unlocked to meet the minimum trait count config.
* Updated trait category display format - now shows "Found in: Starting, Rare, Faithful" at end of description.
* Traits not in any pool now show "Granted via other means (rituals, events, etc.)".
* Added warning to "Include Event Traits" description about potential nonsensical trait assignments.
* Limited maximum traits to 8 due to UI constraints.
* Fixed traits not appearing in trait weights - now collects ALL traits from the game enum instead of only those in predefined lists. This adds missing traits like Chionophile and Heliophile.
* Fixed "Include Event Traits" requiring game restart - event traits now show/hide immediately when the setting is changed.
* Added startup logging of all trait internal names with their in-game display names (check BepInEx log). This helps identify which config entry corresponds to which trait you see in-game.
* Trait names in Configuration Manager now show localized display names alongside the internal name, e.g. "Lover of Cold (Chionophile)". This makes it easier to find traits in non-English languages.
* Trait display names update automatically when you change the game language.
* Traits with missing localization are marked with an asterisk (*) prefix. This indicates the trait is not fully implemented in the game itself.
* Improved slider snapping - values below 0.1 now snap directly to 0, making it easier to disable traits.

### 0.1.4 - 31/01/2026

* Fixed guaranteed traits causing followers to only receive one trait instead of the normal 2-3 traits.
* Added category tags to trait descriptions showing which game lists each trait belongs to, e.g. `[Single, Starting, DLC]`.
* Trait weight sliders now snap to 0.05 increments so you can set exactly 0 to disable a trait.

### 0.1.3 - 30/01/2026

* Fixed Spy followers immediately leaving the cult when assigned via random trait selection. The Spy trait is now excluded from the trait pool since it requires special game state setup that only occurs through the normal spy storyline.
* Excluded BishopOfCult trait from trait selection (story-related, granted when converting a bishop).
* Added "Include Event Traits" config option to optionally include story/event-granted traits in the weights list.
* Event traits are now excluded by default: marriage traits, parenting traits, widowing traits, criminal traits, missionary traits, and various DLC/special traits. These traits are normally granted through gameplay events rather than random selection.
* Added new configuration examples to the README for accessing all traits and creating a cult full of immortals.
* Fixed several README scenarios that were missing "Use All Traits Pool" setting (required for traits not in vanilla's default pools).
* Increased maximum trait weight from 10 to 100 so rare traits like Immortal can actually appear reliably when using the full trait pool.
* Added "Guarantee" checkboxes for each unique trait (Immortal, Disciple, Dont Starve, Blind, Born To The Rot) that force new followers to receive that trait regardless of weights.
* Added "Allow Multiple Unique Traits" option to let multiple followers have the same trait that's normally limited to one follower (Immortal, Disciple, Lazy, Snorer, etc.). Use trait weights to control distribution.
* Reorganized config sections so Notifications appears before Trait Weights.
* Indented "Guarantee" config entries under their parent trait toggles for better readability.

### 0.1.2 - 27/01/2026

* Added unique trait toggles for Don't Starve, Blind, and Born To The Rot.
* Added "Prefer Exclusive Counterparts" config - controls whether exclusive traits (like Lazy) are replaced with their counterpart (Industrious) or a random positive trait.
* Added "Use All Traits Pool" option - pull from all traits instead of the game's separate pools (Starting, Rare, Faithful).
* Moved notification settings to their own section.
* Improved config descriptions for clarity.

### 0.1.1 - 26/01/2026

* Trait weight config entries now show the actual game description of each trait.

### 0.1.0 - 26/01/2026

* Initial release
* No Negative Traits feature - replaces negative traits with positive ones
* Trait Weights feature - configure how likely each trait appears on new followers
* Trait backup/restore system - original traits saved and can be restored
* Dynamic ConfigManager refresh - trait weight settings appear/disappear instantly when toggled
* Configurable options for Immortal and Disciple traits
* Option to use unlocked traits only
* Incompatible with "Nothing Negative" mod (auto-detected)
