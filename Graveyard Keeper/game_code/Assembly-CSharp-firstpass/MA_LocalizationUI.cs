// Decompiled with JetBrains decompiler
// Type: MA_LocalizationUI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class MA_LocalizationUI : MonoBehaviour
{
  public void OnGUI()
  {
    GUI.Label(new Rect(20f, 40f, 640f, 200f), "This scene shows the automatic Localization of Resource files. Preview the 'hello' sound from the mixer, which will be in Spanish first. Then press stop, and change the 'Use Specific Language' language to another language up in the top section of the Master Audio prefab's Inspector, hit play and hear the difference! The correct folder's audio file will be loaded automatically according to your language settings.");
  }
}
