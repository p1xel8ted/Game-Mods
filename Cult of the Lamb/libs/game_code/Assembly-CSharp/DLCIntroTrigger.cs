// Decompiled with JetBrains decompiler
// Type: DLCIntroTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DLCIntroTrigger : MonoBehaviour
{
  public bool Activated;

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !collision.gameObject.CompareTag("Player"))
      return;
    this.Activated = true;
    DLCIntroManager.Instance.IncreaseWeather();
  }
}
