// Decompiled with JetBrains decompiler
// Type: Interaction_MissionaryComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_MissionaryComplete : Interaction
{
  public Follower follower;
  public bool isFailed;
  public const float MISSIONARY_TRAIT_ACQUIRE_CHANCE = 0.25f;

  public void Start() => this.GetComponent<Follower>().HideAllFollowerIcons();

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.Talk;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.follower = this.GetComponent<Follower>();
    this.follower.State.LookAngle = Utils.GetAngle(this.transform.position, this.playerFarming.transform.position);
    this.follower.State.facingAngle = this.follower.State.LookAngle;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -1f));
    HUD_Manager.Instance.Hide(false, 0);
    SimulationManager.Pause();
    if (this.follower.Brain._directInfoAccess.MissionaryRewards.Length != 0)
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.SuccessIE());
    else
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.FailureIE());
    this.Interactable = false;
  }

  public IEnumerator SuccessIE()
  {
    Interaction_MissionaryComplete missionaryComplete = this;
    string str1 = "\n";
    foreach (InventoryItem missionaryReward in missionaryComplete.follower.Brain._directInfoAccess.MissionaryRewards)
      str1 = $"{str1}{missionaryReward.quantity.ToString()} {FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) missionaryReward.type)} ";
    string str2 = str1 + "\n";
    int num1 = UnityEngine.Random.Range(1, 4);
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(missionaryComplete.gameObject, LocalizationManager.GetTranslation($"Conversation_NPC/Follower/Missionary/Success{num1}")),
      new ConversationEntry(missionaryComplete.gameObject, string.Format(LocalizationManager.GetTranslation("Conversation_NPC/Follower/Missionary/Success_Line2"), (object) str2))
    };
    Entries[0].CharacterName = missionaryComplete.follower.Brain._directInfoAccess.Name;
    Entries[0].Offset = new Vector3(0.0f, 1f, 0.0f);
    Entries[1].CharacterName = missionaryComplete.follower.Brain._directInfoAccess.Name;
    Entries[1].Offset = new Vector3(0.0f, 1f, 0.0f);
    foreach (ConversationEntry conversationEntry in Entries)
      conversationEntry.soundPath = "event:/dialogue/followers/general_talk";
    missionaryComplete.playerFarming.state.facingAngle = Utils.GetAngle(missionaryComplete.playerFarming.transform.position, missionaryComplete.transform.position);
    missionaryComplete.playerFarming.state.LookAngle = Utils.GetAngle(missionaryComplete.playerFarming.transform.position, missionaryComplete.transform.position);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, Translate: false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    if (missionaryComplete.follower.Brain._directInfoAccess.MissionaryRewards[0].type == 85)
    {
      FollowerInfo f = FollowerInfo.NewCharacter(FollowerLocation.Base);
      DataManager.SetFollowerSkinUnlocked(f.SkinName);
      if (BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count > 0)
      {
        DataManager.Instance.Followers_Recruit.Add(f);
      }
      else
      {
        BiomeBaseManager.Instance.SpawnExistingRecruits = false;
        yield return (object) new WaitForEndOfFrame();
        GameManager.GetInstance().OnConversationNew(missionaryComplete.playerFarming);
        GameManager.GetInstance().OnConversationNext(BiomeBaseManager.Instance.RecruitSpawnLocation, 6f);
        yield return (object) new WaitForSeconds(0.5f);
        DataManager.Instance.Followers_Recruit.Add(f);
        FollowerManager.SpawnExistingRecruits(BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position);
        UnityEngine.Object.FindObjectOfType<FollowerRecruit>().ManualTriggerAnimateIn();
        BiomeBaseManager.Instance.SpawnExistingRecruits = true;
        yield return (object) new WaitForSeconds(2f);
        GameManager.GetInstance().OnConversationNext(BiomeBaseManager.Instance.RecruitSpawnLocation, 8f);
        yield return (object) new WaitForSeconds(1f);
        GameManager.GetInstance().OnConversationEnd();
      }
      f = (FollowerInfo) null;
    }
    else
    {
      GameManager.GetInstance().OnConversationNew(missionaryComplete.playerFarming);
      GameManager.GetInstance().OnConversationNext(missionaryComplete.playerFarming.gameObject, 6f);
      int total = 0;
      foreach (InventoryItem missionaryReward in missionaryComplete.follower.Brain._directInfoAccess.MissionaryRewards)
        total += missionaryReward.quantity;
      InventoryItem[] inventoryItemArray = missionaryComplete.follower.Brain._directInfoAccess.MissionaryRewards;
      for (int index = 0; index < inventoryItemArray.Length; ++index)
      {
        InventoryItem item = inventoryItemArray[index];
        float increment = 1f / (float) total;
        for (int i = 0; i < item.quantity; ++i)
        {
          AudioManager.Instance.PlayOneShot("event:/followers/pop_in", missionaryComplete.transform.position);
          ResourceCustomTarget.Create(missionaryComplete.playerFarming.gameObject, missionaryComplete.transform.position, (InventoryItem.ITEM_TYPE) item.type, (System.Action) null);
          Inventory.AddItem(item.type, 1, true);
          yield return (object) new WaitForSeconds(increment);
        }
        item = (InventoryItem) null;
      }
      inventoryItemArray = (InventoryItem[]) null;
      GameManager.GetInstance().OnConversationEnd();
    }
    SimulationManager.UnPause();
    missionaryComplete.GetComponent<Follower>().ShowAllFollowerIcons();
    missionaryComplete.Finish();
    missionaryComplete.follower.Brain._directInfoAccess.WakeUpDay = -1;
    missionaryComplete.follower.Brain.CompleteCurrentTask();
    missionaryComplete.follower.Brain.MakeExhausted();
    missionaryComplete.follower.SetOutfit(FollowerOutfitType.Follower, false);
    missionaryComplete.follower.UpdateOutfit();
    if (TimeManager.CurrentDay > 15 && !missionaryComplete.follower.Brain.HasTrait(FollowerTrait.TraitType.MissionaryExcited) && !missionaryComplete.follower.Brain.HasTrait(FollowerTrait.TraitType.MissionaryInspired) && !missionaryComplete.follower.Brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified) && (double) UnityEngine.Random.value < 0.25)
    {
      float num2 = UnityEngine.Random.value;
      if ((double) num2 < 0.5)
      {
        missionaryComplete.follower.Brain.AddTrait(FollowerTrait.TraitType.MissionaryExcited, true);
        missionaryComplete.AddInspiredThought(missionaryComplete.follower.Brain);
      }
      else if ((double) num2 < 0.85000002384185791)
      {
        missionaryComplete.follower.Brain.AddTrait(FollowerTrait.TraitType.MissionaryInspired, true);
        missionaryComplete.AddInspiredThought(missionaryComplete.follower.Brain);
      }
      else
      {
        missionaryComplete.follower.Brain.AddTrait(FollowerTrait.TraitType.MissionaryTerrified, true);
        missionaryComplete.follower.Brain.CurrentState = (FollowerState) new FollowerState_ExistentialDread();
        missionaryComplete.AddDistressedThought(missionaryComplete.follower.Brain);
      }
    }
  }

  public void AddInspiredThought(FollowerBrain brain)
  {
    Thought randomThoughtFromSet = FollowerThoughts.GetRandomThoughtFromSet(Thought.Inspired_0, Thought.Inspired_1, Thought.Inspired_2, Thought.Inspired_3, Thought.Inspired_4);
    brain.AddThought(randomThoughtFromSet);
  }

  public void AddDistressedThought(FollowerBrain brain)
  {
    Thought randomThoughtFromSet = FollowerThoughts.GetRandomThoughtFromSet(Thought.Distressed_0, Thought.Distressed_1, Thought.Distressed_2, Thought.Distressed_3);
    brain.AddThought(randomThoughtFromSet);
  }

  public IEnumerator FailureIE()
  {
    Interaction_MissionaryComplete missionaryComplete = this;
    missionaryComplete.isFailed = true;
    int num = UnityEngine.Random.Range(1, 4);
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(missionaryComplete.gameObject, $"Conversation_NPC/Follower/Missionary/Fail{num}")
    };
    Entries[0].CharacterName = missionaryComplete.follower.Brain._directInfoAccess.Name;
    Entries[0].Offset = new Vector3(0.0f, 1f, 0.0f);
    foreach (ConversationEntry conversationEntry in Entries)
      conversationEntry.soundPath = "event:/dialogue/followers/general_talk";
    missionaryComplete.playerFarming.state.facingAngle = Utils.GetAngle(missionaryComplete.playerFarming.transform.position, missionaryComplete.transform.position);
    missionaryComplete.playerFarming.state.LookAngle = Utils.GetAngle(missionaryComplete.playerFarming.transform.position, missionaryComplete.transform.position);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    DeadWorshipper worshipper = (DeadWorshipper) null;
    missionaryComplete.follower.Die(NotificationCentre.NotificationType.DiedOnMissionary, callback: (System.Action<GameObject>) (obj =>
    {
      worshipper = obj.GetComponent<DeadWorshipper>();
      worshipper.SetOutfit(FollowerOutfitType.Sherpa);
    }), force: true);
    yield return (object) new WaitForSeconds(2f);
    missionaryComplete.Finish();
    SimulationManager.UnPause();
    worshipper?.SetOutfit(FollowerOutfitType.Follower);
  }

  public void Finish()
  {
    List<Structures_Missionary> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Missionary>(FollowerLocation.Base);
    if (structuresOfType.Count > 0)
      structuresOfType[0].Data.MultipleFollowerIDs.Remove(this.follower.Brain.Info.ID);
    this.follower.Brain._directInfoAccess.MissionaryRewards = (InventoryItem[]) null;
    this.follower.Brain._directInfoAccess.MissionaryFinished = false;
    foreach (Interaction_Missionaries missionary in Interaction_Missionaries.Missionaries)
      missionary.UpdateSlots();
    DataManager.Instance.Followers_OnMissionary_IDs.Remove(this.follower.Brain.Info.ID);
    SimulationManager.UnPause();
    Time.timeScale = 1f;
    this.follower.Spine.UseDeltaTime = true;
    this.playerFarming.Spine.UseDeltaTime = true;
    if (this.isFailed)
      return;
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
  }
}
