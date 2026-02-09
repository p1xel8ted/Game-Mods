// Decompiled with JetBrains decompiler
// Type: MA_TestUI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class MA_TestUI : MonoBehaviour
{
  public void OnGUI()
  {
    GUI.Label(new Rect(20f, 40f, 640f, 260f), "Use left/right arrow keys and left mouse button to play. Music ducks (gets quieter) for Screams, then ramps back up soon after. Sound FX have variations. No code needed to be written for any of the sound triggering or ducking. See ReadMe.pdf for more information on how to set things up. Note the Jukebox control that handles the Playlist Controller in the scene! It's in the Master Audio prefab's Inspector. Also, take note of the DynamicSoundGroupCreator prefab, which adds a new temporary Sound Group during the current Scene only! Go ahead and click on the 'Enemy Spawner' script and turn on the checkbox for 'Spawner Enabled' for enemies! There's one Custom Event 'PlayerOffscreen' that gets triggered from EventSounds on the Player when you move offscreen. The EventSounds script on PlayerSpawner receives that event and plays an arrow sound when it happens. We've also implemented a sample class 'MA_SampleICustomEventReceiver' that implements the ICustomEventReciever class if you wish to see how to do that. It's attached to the main camera prefab. A linked group of Blast is set up in the Scream Group, take a look! Sample music provided by Alchemy Studios. This music 'The Epic Trailer' (longer version) is available on the Asset Store!\n\nHappy gaming - DarkTonic, Inc.");
  }
}
