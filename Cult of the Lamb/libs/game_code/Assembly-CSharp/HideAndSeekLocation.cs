// Decompiled with JetBrains decompiler
// Type: HideAndSeekLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
