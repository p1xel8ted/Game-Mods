### 0.1.5 - 02/02/2026

**Fixes:**
* Fixed game freezing when using Re-educate or Re-indoctrinate with most trait weights set to zero.
* Fixed doctrine-granted traits (Fertility, Allegiance, etc.) appearing as individual follower traits when they already apply cult-wide.
* Fixed Re-educate causing sin/pleasure accumulation on normal followers when used for trait rerolls.

### 0.1.4 - 01/02/2026

**New Features:**
* Added Minimum and Maximum Traits settings - control how many traits new followers receive (2-8 traits, vanilla is 2-3).
* Added Randomize Traits on Re-indoctrination - reroll follower traits when using the altar (vanilla only changes appearance/name).
* Added Trait Reroll via Reeducation - adds the Re-educate command to normal followers to reroll their traits using your configured settings.
* Added Reset All Settings button to quickly restore vanilla behavior.

**Improvements:**
* Trait names now display in your game language for easier identification.
* Trait weight sliders snap cleanly to 0.05 increments, with very low values automatically rounding to 0 for easy disabling.
* Trait weights menu automatically hides traits you haven't unlocked yet (based on game progression).
* Trait descriptions now show which pools they belong to (Starting, Rare, Faithful, DLC, etc.) to help understand rarity.

**Fixes:**
* Fixed guaranteed unique traits (Immortal, Disciple, etc.) causing followers to only receive 1 trait instead of the normal 2-3.
* Fixed "Use Unlocked Traits Only" not properly checking game progression requirements.
* Fixed crash when opening mod settings on the main menu with no save loaded.
* Fixed rare infinite loop issue with certain config combinations.
* Fixed exclusive trait pairs (like Lazy/Industrious) not working correctly when both traits were negative.
* Fixed some traits missing from the configuration menu.
* Fixed "Include Event Traits" setting requiring a game restart to take effect.

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
