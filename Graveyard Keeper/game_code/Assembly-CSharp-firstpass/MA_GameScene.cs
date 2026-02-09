// Decompiled with JetBrains decompiler
// Type: MA_GameScene
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class MA_GameScene : MonoBehaviour
{
  public void OnGUI()
  {
    GUI.Label(new Rect(20f, 40f, 640f, 190f), "This is the Game Scene. We have used a Dynamic Sound Group Creator prefab to populate temporary Sound Groups into Master Audio as soon as that prefab becomes enabled (on Scene change for us). If we were to load a different Scene now, those sounds would vanish from the mixer.");
  }
}
