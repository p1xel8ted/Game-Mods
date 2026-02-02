// Decompiled with JetBrains decompiler
// Type: IntroMountainTopTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
