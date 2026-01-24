// Decompiled with JetBrains decompiler
// Type: BaseRegionEntrance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
