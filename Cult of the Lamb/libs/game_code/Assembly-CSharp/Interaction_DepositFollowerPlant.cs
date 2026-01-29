// Decompiled with JetBrains decompiler
// Type: Interaction_DepositFollowerPlant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_DepositFollowerPlant : Interaction
{
  [SerializeField]
  public GameObject rewardPosition;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public GameObject flockadePiecesPrefab;
  [SerializeField]
  public Interaction_Follower followerInteraction;
  [SerializeField]
  public InventoryItem.ITEM_TYPE costType;
  [SerializeField]
  public int cost;
  public FollowerInfo follower;
  public bool isReward;
  public bool isFollower;
  public bool claimed;

  public void Configure(FollowerInfo info, bool isReward, bool isFollower)
  {
    this.isReward = isReward;
    this.isFollower = isFollower;
    this.follower = info;
    this.spine.AnimationState.SetAnimation(0, "MutationPlant/growing-4", true);
    FollowerBrain.SetFollowerCostume(this.spine.Skeleton, info, forceUpdate: true);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.claimed)
    {
      this.Interactable = false;
      this.Label = "";
    }
    else
    {
      this.Interactable = true;
      this.Label = LocalizationManager.GetTranslation("Interactions/DepositFollower/FeedFlowers") + InventoryItem.CapacityString(this.costType, this.cost);
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(this.costType) >= this.cost)
      this.StartCoroutine((IEnumerator) this.OpenIE());
    else
      state.GetComponent<PlayerFarming>().indicator.PlayShake();
  }

  public IEnumerator OpenIE()
  {
    Interaction_DepositFollowerPlant depositFollowerPlant = this;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/gardener/flower_feed", depositFollowerPlant.transform.position);
    PlayerFarming.Instance.GoToAndStop(depositFollowerPlant.transform.position + ((double) depositFollowerPlant.transform.position.x < 0.0 ? Vector3.right * 2f : Vector3.left * 2f), depositFollowerPlant.gameObject);
    Inventory.ChangeItemQuantity(depositFollowerPlant.costType, -depositFollowerPlant.cost);
    depositFollowerPlant.claimed = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(depositFollowerPlant.gameObject, 4f);
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/gardener/flower_open_start", depositFollowerPlant.transform.position);
    if (depositFollowerPlant.isReward)
    {
      bool hasTarotReward = !DataManager.Instance.PlayerFoundTrinkets.Contains(TarotCards.Card.Joker);
      bool hasDecoReward = !DataManager.Instance.UnlockedStructures.Contains(StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1) || !DataManager.Instance.UnlockedStructures.Contains(StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2);
      bool hasOutfitReward = !DataManager.Instance.ClothesUnlocked(FollowerClothingType.Normal_MajorDLC_6);
      bool hasLoreReward = !LoreSystem.LoreAvailable(27);
      depositFollowerPlant.spine.AnimationState.SetAnimation(0, "MutationPlant/hatchOld", false);
      depositFollowerPlant.spine.AnimationState.AddAnimation(0, "MutationPlant/hatched", true, 0.0f);
      yield return (object) new WaitForSeconds(1.33333337f);
      bool waiting = true;
      if (hasDecoReward | hasOutfitReward | hasTarotReward | hasLoreReward)
      {
        AudioManager.Instance.PlayOneShot("event:/dlc/env/gardener/flower_open_blueprint", depositFollowerPlant.transform.position);
        while (!hasDecoReward || (double) UnityEngine.Random.value >= 0.10000000149011612)
        {
          if (hasTarotReward && (double) UnityEngine.Random.value < 0.10000000149011612)
          {
            TarotCustomTarget.Create(depositFollowerPlant.rewardPosition.transform.position, depositFollowerPlant.playerFarming.transform.position, 2f, TarotCards.Card.Joker, (System.Action) (() => waiting = false));
            goto label_17;
          }
          if (hasOutfitReward && (double) UnityEngine.Random.value < 0.10000000149011612)
          {
            FollowerOutfitCustomTarget.Create(depositFollowerPlant.rewardPosition.transform.position, depositFollowerPlant.playerFarming.transform.position, 2f, FollowerClothingType.Normal_MajorDLC_6, (System.Action) (() => waiting = false));
            goto label_17;
          }
          if (hasLoreReward && (double) UnityEngine.Random.value < 0.10000000149011612)
          {
            LoreStone component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LORE_STONE, 1, PlayerFarming.Instance.transform.position).GetComponent<LoreStone>();
            component.SetLore(27, isWolfLore: true);
            component.OnInteract(PlayerFarming.Instance.state);
            yield return (object) new WaitForEndOfFrame();
            while (LetterBox.IsPlaying)
              yield return (object) null;
            goto label_17;
          }
        }
        if (!DataManager.Instance.UnlockedStructures.Contains(StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1))
          DecorationCustomTarget.Create(depositFollowerPlant.rewardPosition.transform.position, depositFollowerPlant.playerFarming.transform.position, 2f, StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1, (System.Action) (() => waiting = false));
        else
          DecorationCustomTarget.Create(depositFollowerPlant.rewardPosition.transform.position, depositFollowerPlant.playerFarming.transform.position, 2f, StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2, (System.Action) (() => waiting = false));
      }
      else
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MAGMA_STONE, UnityEngine.Random.Range(20, 30), depositFollowerPlant.rewardPosition.transform.position);
        waiting = false;
      }
label_17:
      while (waiting)
        yield return (object) null;
      Interaction_DepositFollower.Instance.successBark.gameObject.SetActive(true);
    }
    else if (depositFollowerPlant.isFollower)
    {
      depositFollowerPlant.followerInteraction.followerInfo = depositFollowerPlant.follower;
      depositFollowerPlant.spine.AnimationState.SetAnimation(0, "MutationPlant/hatch", false);
      yield return (object) new WaitForSeconds(1.33333337f);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/gardener/flower_open_follower", depositFollowerPlant.transform.position);
      yield return (object) new WaitForSeconds(2f);
      depositFollowerPlant.follower.Location = depositFollowerPlant.follower.HomeLocation = FollowerLocation.Base;
      yield return (object) depositFollowerPlant.StartCoroutine((IEnumerator) depositFollowerPlant.followerInteraction.ConvertIE());
      Interaction_DepositFollower.Instance.successBark.gameObject.SetActive(true);
    }
    else
    {
      depositFollowerPlant.spine.AnimationState.SetAnimation(0, "MutationPlant/hatchOld", false);
      depositFollowerPlant.spine.AnimationState.AddAnimation(0, "MutationPlant/hatched", true, 0.0f);
      yield return (object) new WaitForSeconds(1.33333337f);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(depositFollowerPlant.rewardPosition.transform.position);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/gardener/flower_open_fail", depositFollowerPlant.transform.position);
      yield return (object) new WaitForSeconds(2f);
      Interaction_DepositFollower.Instance.failBark.gameObject.SetActive(true);
    }
    GameManager.GetInstance().OnConversationEnd();
    depositFollowerPlant.Interactable = false;
    depositFollowerPlant.HasChanged = true;
  }
}
