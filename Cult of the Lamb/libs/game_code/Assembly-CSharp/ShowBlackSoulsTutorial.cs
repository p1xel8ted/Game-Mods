// Decompiled with JetBrains decompiler
// Type: ShowBlackSoulsTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShowBlackSoulsTutorial : BaseMonoBehaviour
{
  public void Play()
  {
    if (DataManager.Instance.BlackSoulsEnabled)
      return;
    Object.FindObjectOfType<HUD_BlackSoul>().DoTutorial();
  }
}
