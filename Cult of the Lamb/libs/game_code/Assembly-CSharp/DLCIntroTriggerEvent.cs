// Decompiled with JetBrains decompiler
// Type: DLCIntroTriggerEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DLCIntroTriggerEvent : MonoBehaviour
{
  public bool triggered;

  public void TriggerLogo()
  {
    if (this.triggered)
      return;
    this.triggered = true;
    DLCIntroManager.Instance.ShowLogo();
  }
}
