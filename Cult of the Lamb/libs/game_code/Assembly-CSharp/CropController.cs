// Decompiled with JetBrains decompiler
// Type: CropController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CropController : BaseMonoBehaviour
{
  public InventoryItem.ITEM_TYPE SeedType;
  public List<GameObject> CropStates = new List<GameObject>();
  public GameObject BumperCropObject;
  public GameObject GrowRateIcons;
  public GameObject CropGrowerIcon;
  public int growth;

  public void SetCropImage(
    float growthStage,
    bool BeneffitedFromFertilizer,
    FollowerLocation Location)
  {
    growthStage = Mathf.Min(growthStage, CropController.CropGrowthTimes(this.SeedType));
    if (BeneffitedFromFertilizer && (double) growthStage >= (double) CropController.CropGrowthTimes(this.SeedType))
    {
      int index = -1;
      while (++index < this.CropStates.Count)
        this.CropStates[index].SetActive(false);
      this.BumperCropObject.GetComponentInChildren<Structure>().CreateStructure(Location, this.transform.position, !this.BumperCropObject.activeSelf, false);
      this.BumperCropObject.gameObject.SetActive(true);
      if (this.BumperCropObject.activeSelf)
        return;
      this.BumperCropObject.transform.DOKill();
      this.BumperCropObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f);
    }
    else
    {
      int index = -1;
      while (++index < this.CropStates.Count)
      {
        int num = Mathf.FloorToInt(growthStage / CropController.CropGrowthTimes(this.SeedType) * (float) (this.CropStates.Count - 1));
        if (index == num && (double) growthStage >= (double) CropController.CropGrowthTimes(this.SeedType))
          this.CropStates[index].GetComponentInChildren<Structure>().CreateStructure(Location, this.transform.position, !this.CropStates[index].activeSelf, false);
        if (index == num && !this.CropStates[index].activeSelf)
        {
          this.BumperCropObject.transform.DOKill();
          this.CropStates[index].transform.DOPunchScale(Vector3.one * 0.2f, 0.25f);
        }
        this.CropStates[index].SetActive(index == num);
      }
    }
  }

  public void SetGrowRateIcons(bool show) => this.GrowRateIcons.SetActive(show);

  public void SetCropGrowerIcons(bool show) => this.CropGrowerIcon.SetActive(show);

  public void HideAll()
  {
    foreach (GameObject cropState in this.CropStates)
    {
      if ((Object) cropState != (Object) null)
        cropState.SetActive(false);
    }
    this.BumperCropObject.gameObject.SetActive(false);
  }

  public void Harvest()
  {
    FarmPlot componentInParent = this.GetComponentInParent<FarmPlot>();
    if (!(bool) (Object) componentInParent)
      return;
    componentInParent.Harvested();
  }

  public static int CropStatesForSeedType(InventoryItem.ITEM_TYPE seedType)
  {
    return seedType == InventoryItem.ITEM_TYPE.SEED || seedType == InventoryItem.ITEM_TYPE.SEED_PUMPKIN ? 4 : 4;
  }

  public static float CropGrowthTimes(InventoryItem.ITEM_TYPE seedType)
  {
    float num;
    switch (seedType)
    {
      case InventoryItem.ITEM_TYPE.SEED:
      case InventoryItem.ITEM_TYPE.SEED_PUMPKIN:
      case InventoryItem.ITEM_TYPE.SEED_COTTON:
      case InventoryItem.ITEM_TYPE.SEED_SNOW_FRUIT:
      case InventoryItem.ITEM_TYPE.SEED_CHILLI:
        num = 9f;
        break;
      case InventoryItem.ITEM_TYPE.SEED_MUSHROOM:
      case InventoryItem.ITEM_TYPE.SEED_SOZO:
        num = 15f;
        break;
      case InventoryItem.ITEM_TYPE.SEED_BEETROOT:
        num = 12f;
        break;
      case InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER:
      case InventoryItem.ITEM_TYPE.SEED_HOPS:
        num = 18f;
        break;
      case InventoryItem.ITEM_TYPE.SEED_GRAPES:
        num = 12f;
        break;
      default:
        num = 9f;
        break;
    }
    return num;
  }

  public static Vector2Int GetHarvestsPerSeedRange(InventoryItem.ITEM_TYPE seedType)
  {
    return new Vector2Int(2, 5);
  }
}
