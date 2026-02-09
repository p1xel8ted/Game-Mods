// Decompiled with JetBrains decompiler
// Type: HideAndSeekLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HideAndSeekLocation : MonoBehaviour
{
  public MarketplaceClothesHiddenSkin controller;

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player"))
      return;
    this.controller.FoundHideAndSeekLocation(this);
  }
}
