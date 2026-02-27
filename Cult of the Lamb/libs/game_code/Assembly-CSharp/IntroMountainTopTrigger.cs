// Decompiled with JetBrains decompiler
// Type: IntroMountainTopTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class IntroMountainTopTrigger : MonoBehaviour
{
  public bool triggered;

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (this.triggered)
      return;
    DLCIntroManager.Instance.DoMountainTopTrigger();
    this.triggered = true;
  }
}
