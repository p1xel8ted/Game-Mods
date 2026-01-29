// Decompiled with JetBrains decompiler
// Type: AnimalMarketplaceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class AnimalMarketplaceManager : MonoBehaviour
{
  [SerializeField]
  public SingleChoiceRewardHeartShop choiceOption;
  [SerializeField]
  public Interaction_SimpleConversation introConversation;
  [SerializeField]
  public SimpleBark barks;
  [SerializeField]
  public SimpleBark purchaseBark;
  [SerializeField]
  public SkeletonAnimation rancher;
  [SerializeField]
  public SkeletonAnimation[] animalSpines;
  [SerializeField]
  [TermsPopup("")]
  public string rancherNameTerm;
  [SerializeField]
  public List<AnimalMarketplaceManager.BarkTerm> barkSet1 = new List<AnimalMarketplaceManager.BarkTerm>();
  [SerializeField]
  public List<AnimalMarketplaceManager.BarkTerm> barkSet2 = new List<AnimalMarketplaceManager.BarkTerm>();
  public bool convoSpent;

  public void Awake()
  {
    if (DataManager.Instance.EncounteredDungeonRancherCount != 0 && DataManager.Instance.EncounteredDungeonRancherCount > 4)
    {
      this.rancher.Skeleton.SetSkin("Rotten");
      this.rancher.AnimationState.SetAnimation(0, "dead", true);
    }
    if (DataManager.Instance.EncounteredDungeonRancherCount > 0)
      this.rancher.Skeleton.SetSkin($"Rot_{DataManager.Instance.EncounteredDungeonRancherCount}");
    ++DataManager.Instance.EncounteredDungeonRancherCount;
    List<InventoryItem.ITEM_TYPE> animalsWeightedList = AnimalMarketplaceManager.GetUnlockedAnimalsWeightedList();
    this.choiceOption.itemOptions.Add(new BuyEntry()
    {
      costType = InventoryItem.ITEM_TYPE.NONE,
      itemToBuy = animalsWeightedList[UnityEngine.Random.Range(0, animalsWeightedList.Count)],
      quantity = 1,
      SingleQuantityItem = true
    });
    foreach (SkeletonAnimation animalSpine in this.animalSpines)
      AnimalData.GetRanchSpineSkin(new StructuresData.Ranchable_Animal()
      {
        Age = 20,
        Colour = UnityEngine.Random.Range(0, 5),
        Ears = UnityEngine.Random.Range(1, 4),
        Horns = UnityEngine.Random.Range(1, 4),
        Head = UnityEngine.Random.Range(1, 4),
        GrowthStage = 6,
        Type = animalsWeightedList[UnityEngine.Random.Range(0, animalsWeightedList.Count)]
      }, animalSpine);
  }

  public static List<InventoryItem.ITEM_TYPE> GetUnlockedAnimalsWeightedList()
  {
    List<InventoryItem.ITEM_TYPE> animalsWeightedList = new List<InventoryItem.ITEM_TYPE>();
    for (int index = 0; index < 6; ++index)
      animalsWeightedList.Add(InventoryItem.ITEM_TYPE.ANIMAL_GOAT);
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(0))
    {
      for (int index = 0; index < 4; ++index)
        animalsWeightedList.Add(InventoryItem.ITEM_TYPE.ANIMAL_LLAMA);
    }
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(1))
    {
      for (int index = 0; index < 3; ++index)
        animalsWeightedList.Add(InventoryItem.ITEM_TYPE.ANIMAL_COW);
    }
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(2))
      animalsWeightedList.Add(InventoryItem.ITEM_TYPE.ANIMAL_TURTLE);
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(3))
      animalsWeightedList.Add(InventoryItem.ITEM_TYPE.ANIMAL_CRAB);
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(4))
      animalsWeightedList.Add(InventoryItem.ITEM_TYPE.ANIMAL_SNAIL);
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(5))
      animalsWeightedList.Add(InventoryItem.ITEM_TYPE.ANIMAL_SPIDER);
    return animalsWeightedList;
  }

  public void OnEnable() => this.SetupBarksAndConvos();

  public void SetupBarksAndConvos()
  {
    DataManager instance = DataManager.Instance;
    int num = instance != null ? instance.EncounteredDungeonRancherCount : 1;
    Debug.Log((object) $"Setting up Dungeon Rancher Barks and Convos: Encounter No.{num}");
    if ((UnityEngine.Object) this.barks == (UnityEngine.Object) null)
    {
      Debug.Log((object) "Dungeon Rancher: No barks set");
    }
    else
    {
      this.barks.Entries.Clear();
      if (num <= 1 || num > 5)
        return;
      List<AnimalMarketplaceManager.BarkTerm> barkTermList = this.barkSet1;
      if (num > 2)
      {
        this.purchaseBark.Entries.Clear();
        this.introConversation.gameObject.SetActive(false);
      }
      else if (!this.convoSpent)
      {
        this.introConversation.gameObject.SetActive(true);
        this.introConversation.Callback.RemoveAllListeners();
        this.introConversation.Callback.AddListener((UnityAction) (() => this.convoSpent = true));
      }
      if (num > 3)
        barkTermList = this.barkSet2;
      // ISSUE: explicit non-virtual call
      Debug.Log((object) $"Dungeon Rancher Bark Set Size: {(barkTermList != null ? __nonvirtual (barkTermList.Count) : -1)}");
      this.barks.gameObject.SetActive(barkTermList != null && barkTermList.Count > 0);
      this.barks.Entries = new List<ConversationEntry>();
      foreach (AnimalMarketplaceManager.BarkTerm barkTerm in barkTermList)
      {
        ConversationEntry conversationEntry = new ConversationEntry(this.rancher.gameObject, barkTerm.Term, CharacterName: this.rancherNameTerm);
        conversationEntry.Speaker = this.rancher.gameObject;
        conversationEntry.SkeletonData = this.rancher;
        conversationEntry.DefaultAnimation = "idle";
        conversationEntry.Offset = Vector3.up;
        Debug.Log((object) ("Dungeon Rancher: Adding bark:" + barkTerm.Term), (UnityEngine.Object) this.barks);
        this.barks.Entries.Add(conversationEntry);
      }
      Debug.Log((object) $"Dungeon Rancher: Added {this.barks.Entries.Count} barks.");
    }
  }

  [CompilerGenerated]
  public void \u003CSetupBarksAndConvos\u003Eb__14_0() => this.convoSpent = true;

  [Serializable]
  public struct BarkTerm
  {
    [TermsPopup("")]
    public string Term;
  }
}
