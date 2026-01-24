// Decompiled with JetBrains decompiler
// Type: MarketplaceClothesHiddenSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MarketplaceClothesHiddenSkin : MonoBehaviour
{
  public static System.Action OnCrownReturn;
  public List<HideAndSeekLocation> placesItCouldBe;
  public FoundItemPickUp itemToFind;

  public void OnEnable()
  {
    if (DataManager.Instance.FollowerSkinsUnlocked.Contains("Worm"))
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.itemToFind.gameObject.SetActive(false);
      for (int index = 0; index < this.placesItCouldBe.Count; ++index)
        this.placesItCouldBe[index].controller = this;
    }
  }

  public void FoundHideAndSeekLocation(HideAndSeekLocation location)
  {
    if (!this.placesItCouldBe.Contains(location))
      return;
    this.placesItCouldBe.Remove(location);
    if (this.placesItCouldBe.Count == 0)
    {
      this.itemToFind.gameObject.SetActive(true);
      this.itemToFind.transform.position = location.transform.position;
    }
    location.gameObject.SetActive(false);
    Debug.Log((object) ("Found hiding place - remaining: " + this.placesItCouldBe.Count.ToString()));
  }
}
