// Decompiled with JetBrains decompiler
// Type: BaseRegionEntrance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class BaseRegionEntrance : MonoBehaviour
{
  public UnityEvent Trigger;
  public bool showingNorth = true;

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!(collision.gameObject.tag == "Player"))
      return;
    this.DoTrigger();
  }

  public void DoTrigger()
  {
    this.Trigger?.Invoke();
    this.showingNorth = !this.showingNorth;
  }
}
