### 0.2.3 - 02/02/2026

* Added full localization support for all 15 game languages.
* Button labels, confirmation dialogs, and popup messages now display in your game language.
* Config descriptions are localized based on your language at game startup.
* Improved Configuration Manager UI formatting with visual section separators and sub-setting indentation.
* Fixed name files being saved in binary MessagePack format instead of human-readable JSON.
* Name files are now saved as proper JSON that can be viewed and edited in any text editor.
* Note: If you have old `.mp` files in your saves folder, delete them and the mod will regenerate names from the API.

### 0.2.2 - 25/01/2026

* Fixed asterisk indicator not appearing after clicking "get name" button during indoctrination.
* Updated Configuration Manager dependency to 18.4.1.

### 0.2.1 - 24 January 2026

* Woolhaven update.
* Simplified asterisk name indicator (now only shown during indoctrination).
* Removed "Clean Asterisks" button (no longer needed).
* Leftover asterisks in existing saves are now automatically cleaned on load.
* Fixed potential crash during follower name cleanup.
* Fixed backup name source not being used when the primary source fails.
* Fixed naming screen sometimes not properly applying outfit and Twitch changes.
* Names are now pre-loaded on game start for faster indoctrination.
* Improved handling when name lists aren't available yet.

### 0.2.0 - 03 February 2025

* Added button to F1 screen to manually clean-up any leftover asterisks (this happens already at various stages, but now you can manually trigger it.)
* Re-wrote logic to prevent instances where an asterisk might remain after indoctrinating.

### 0.1.9 - 03 September 2024

* Added option to toggle asterisk functionality.
* Added extra cleanup for names that have asterisks that weren't removed for whatever reason.

### 0.1.8 - 25 August 2024

* Fixed the asterisk for Namify names not being removed in all instances.
* Added cleanup method for already named followers (runs when player loads in, and when a follower name is confirmed).

### 0.1.7 - 24 August 2024

* Removal of Fody Costura as it's no longer required and causes delays with updates on Thunderstore.
* Names that have been randomly generated will now have an asterisk (*) next to them when indoctrinating a follower. You do not need to manually remove it.

### 0.1.6 - 23 August 2024

* Maintenance update for 1.4.4.592.
* Reverted/removed UnityDebuggerAssistant requirement.

### 0.1.5 - 23 August 2024

* Maintenance update for 1.4.4.590.
* Implemented hard dependency on UnityDebuggerAssistant.

### 0.1.4 - 17 August 2024

* Maintenance update for Unholy Alliance.

### 0.1.3 - 25 January 2024

* Fixed names being generated as first and last name.

### 0.1.2 - 21 January 2024

* Added ability to add your own names. User-generated/added names are stored in their own list.
* Added ability to manually generate a new list of names.
* Added ability to reload the list of names from file.
* Added button to open a specific name file. It will use whatever you have used in the past to open a JSON file. Notepad etc. is fine.
* Name lists are now saved unencrypted. This is to allow users to edit the files themselves.

### 0.1.1 - 20 January 2024

* Maintenance update for Sins of the Flesh.

### 0.1.0 - 19 October 2022

* Initial release.