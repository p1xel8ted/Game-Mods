// Decompiled with JetBrains decompiler
// Type: Interaction_MissionaryComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_MissionaryComplete : Interaction
{
  private Follower follower;

  private void Start() => this.GetComponent<Follower>().HideAllFollowerIcons();

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.Talk;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.follower = this.GetComponent<Follower>();
    this.follower.State.LookAngle = Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position);
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

  private IEnumerator SuccessIE()
  {
    Interaction_MissionaryComplete missionaryComplete = this;
    string str1 = "\n";
    foreach (InventoryItem missionaryReward in missionaryComplete.follower.Brain._directInfoAccess.MissionaryRewards)
      str1 = $"{str1}{missionaryReward.quantity.ToString()} {FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) missionaryReward.type)} ";
    string str2 = str1 + "\n";
    int num = UnityEngine.Random.Range(1, 4);
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(missionaryComplete.gameObject, LocalizationManager.GetTranslation($"Conversation_NPC/Follower/Missionary/Success{num}")),
      new ConversationEntry(missionaryComplete.gameObject, string.Format(LocalizationManager.GetTranslation("Conversation_NPC/Follower/Missionary/Success_Line2"), (object) str2))
    };
    Entries[0].CharacterName = missionaryComplete.follower.Brain._directInfoAccess.Name;
    Entries[0].Offset = new Vector3(0.0f, 1f, 0.0f);
    Entries[1].CharacterName = missionaryComplete.follower.Brain._directInfoAccess.Name;
    Entries[1].Offset = new Vector3(0.0f, 1f, 0.0f);
    foreach (ConversationEntry conversationEntry in Entries)
      conversationEntry.soundPath = "event:/dialogue/followers/general_talk";
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, missionaryComplete.transform.position);
    PlayerFarming.Instance.state.LookAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, missionaryComplete.transform.position);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), Translate: false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
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
        GameManager.GetInstance().OnConversationNew(true, true, true);
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
      GameManager.GetInstance().OnConversationNew(true, true, true);
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 6f);
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
          ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, missionaryComplete.transform.position, (InventoryItem.ITEM_TYPE) item.type, (System.Action) null);
          Inventory.AddItem(item.type, 1, true);
          yield return (object) new WaitForSeconds(increment);
        }
        item = (InventoryItem) null;
      }
      inventoryItemArray = (InventoryItem[]) null;
      GameManager.GetInstance().OnConversationEnd();
    }
    SimulationManager.UnPause();
    missionaryComplete.follower.Brain._directInfoAccess.WakeUpDay = -1;
    missionaryComplete.follower.Brain.CompleteCurrentTask();
    missionaryComplete.follower.SetOutfit(FollowerOutfitType.Follower, false);
    missionaryComplete.GetComponent<Follower>().ShowAllFollowerIcons();
    missionaryComplete.Finish();
    missionaryComplete.follower.Brain.MakeExhausted();
  }

  private IEnumerator FailureIE()
  {
    Interaction_MissionaryComplete missionaryComplete = this;
    int num = UnityEngine.Random.Range(1, 4);
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(missionaryComplete.gameObject, $"Conversation_NPC/Follower/Missionary/Fail{num}")
    };
    Entries[0].CharacterName = missionaryComplete.follower.Brain._directInfoAccess.Name;
    Entries[0].Offset = new Vector3(0.0f, 1f, 0.0f);
    foreach (ConversationEntry conversationEntry in Entries)
      conversationEntry.soundPath = "event:/dialogue/followers/general_talk";
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, missionaryComplete.transform.position);
    PlayerFarming.Instance.state.LookAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, missionaryComplete.transform.position);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    DeadWorshipper worshipper = (DeadWorshipper) null;
    missionaryComplete.follower.Die(callback: (System.Action<GameObject>) (obj =>
    {
      worshipper = obj.GetComponent<DeadWorshipper>();
      worshipper.SetOutfit(FollowerOutfitType.Sherpa);
    }));
    missionaryComplete.Finish();
    yield return (object) new WaitForSeconds(2f);
    SimulationManager.UnPause();
    worshipper.SetOutfit(FollowerOutfitType.Follower);
  }

  private void Finish()
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
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    this.enabled = false;
  }
}
