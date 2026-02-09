// Decompiled with JetBrains decompiler
// Type: MissionRewardChest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class MissionRewardChest : BaseMonoBehaviour
{
  public MissionRewardChest.ChestType TypeOfChest;
  public SkeletonAnimation Spine;
  public GameObject Shadow;
  public GameObject Lighting;
  public int rewardAmount;
  public MissionManager.Mission mission;

  public event MissionRewardChest.RewardEvent OnRewardsCollected;

  public void Play(MissionManager.Mission mission) => this.mission = mission;

  public void ShowReward() => this.StartCoroutine((IEnumerator) this.GiveRewardDelay());

  public IEnumerator GiveRewardDelay()
  {
    MissionRewardChest missionRewardChest = this;
    missionRewardChest.Shadow.transform.localScale = new Vector3(3f, 2f, 1f);
    missionRewardChest.Spine.AnimationState.SetAnimation(0, "open", false);
    missionRewardChest.ChestOpenSfx();
    missionRewardChest.Spine.AnimationState.AddAnimation(0, "opened", true, 0.0f);
    yield return (object) new WaitForSeconds(0.25f);
    int index1 = missionRewardChest.mission.GoldenMission ? missionRewardChest.mission.Rewards.Length - 1 : missionRewardChest.mission.Rewards.Length;
    for (int index2 = 0; index2 < index1; ++index2)
    {
      PickUp pickUp = InventoryItem.Spawn(missionRewardChest.mission.Rewards[index2].itemToBuy, Mathf.Clamp(missionRewardChest.mission.Rewards[index2].quantity, 1, int.MaxValue), missionRewardChest.transform.position + Vector3.back, 0.0f);
      pickUp.SetInitialSpeedAndDiraction(3f + Random.Range(-0.5f, 1f), (float) (270 + Random.Range(-90, 90)));
      pickUp.OnPickedUp += new PickUp.PickUpEvent(missionRewardChest.ItemPickedUp);
      ++missionRewardChest.rewardAmount;
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", missionRewardChest.gameObject);
    }
    if (missionRewardChest.mission.GoldenMission)
    {
      PickUp pickUp = InventoryItem.Spawn(missionRewardChest.mission.Rewards[index1].itemToBuy, Mathf.Clamp(missionRewardChest.mission.Rewards[index1].quantity, 1, int.MaxValue), missionRewardChest.transform.position + Vector3.back, 0.0f);
      pickUp.SetInitialSpeedAndDiraction(3f + Random.Range(-0.5f, 1f), (float) (270 + Random.Range(-90, 90)));
      pickUp.OnPickedUp += new PickUp.PickUpEvent(missionRewardChest.ItemPickedUp);
      FoundItemPickUp component = pickUp.GetComponent<FoundItemPickUp>();
      if (missionRewardChest.mission.Rewards[index1].itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN)
      {
        component.FollowerSkinForceSelection = true;
        component.SkinToForce = missionRewardChest.mission.FollowerSkin;
      }
      else if (missionRewardChest.mission.Rewards[index1].itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION)
        component.DecorationType = missionRewardChest.mission.Decoration;
      ++missionRewardChest.rewardAmount;
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", missionRewardChest.gameObject);
    }
  }

  public void ItemPickedUp(PickUp p)
  {
    --this.rewardAmount;
    if (this.rewardAmount > 0)
      return;
    MissionRewardChest.RewardEvent rewardsCollected = this.OnRewardsCollected;
    if (rewardsCollected == null)
      return;
    rewardsCollected();
  }

  public void ChestOpenSfx()
  {
    switch (this.TypeOfChest)
    {
      case MissionRewardChest.ChestType.Wooden:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_open", this.gameObject);
        break;
      case MissionRewardChest.ChestType.Silver:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_big_open", this.gameObject);
        break;
      case MissionRewardChest.ChestType.Gold:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_big_open", this.gameObject);
        break;
      default:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_open", this.gameObject);
        break;
    }
  }

  public struct Reward
  {
    public InventoryItem.ITEM_TYPE RewardItemType;
    public int Quantity;
  }

  public enum ChestType
  {
    None,
    Wooden,
    Silver,
    Gold,
  }

  public delegate void RewardEvent();
}
