// Decompiled with JetBrains decompiler
// Type: Interaction_FirstFreeFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMRoomGeneration;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class Interaction_FirstFreeFollower : Interaction_FollowerSpawn
{
  public SimpleBark simpleBark;
  public new FMOD.Studio.EventInstance receiveLoop;
  public Villager_Info v_i;
  public WorshipperInfoManager wimy;
  public string skin;
  [SerializeField]
  public bool isNarayana;
  public Vector3 startingPosition;

  public void Awake()
  {
    if (DataManager.Instance.GivenNarayanaFollower && this.isNarayana)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      if (DataManager.Instance.GiveExecutionerFollower)
      {
        this.GetComponentInParent<GenerateRoom>().LockingDoors = true;
        GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => RoomLockController.CloseAll()));
      }
      if (!this.isNarayana)
        return;
      this.startingPosition = this.transform.position;
    }
  }

  public override void Start()
  {
    base.Start();
    this.Play("", animate: false);
    this.simpleBark = this.GetComponent<SimpleBark>();
    if ((bool) (UnityEngine.Object) this.simpleBark)
      this.RandomiseBark();
    DataManager.Instance.GivenFreeDungeonFollower = true;
    this.skin = "";
    this.skin = DataManager.GetRandomLockedSkin();
    if (this.skin.IsNullOrEmpty())
      this.skin = DataManager.GetRandomSkin();
    UnityEngine.Debug.Log((object) "?????????????????????????????????");
    UnityEngine.Debug.Log((object) ("skin: " + this.skin));
    this._followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, this.skin);
    if (this._followerInfo.SkinName == "Giraffe")
      this._followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Sparkles");
    if (this._followerInfo.SkinName == "Poppy")
      this._followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Poppy");
    if (this._followerInfo.SkinName == "Pudding")
      this._followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Pudding");
    if (this.isNarayana)
      this.skin = "Narayana";
    this.v_i = Villager_Info.NewCharacter(this.skin);
    this.wimy = this.GetComponent<WorshipperInfoManager>();
    this.wimy.SetV_I(this.v_i);
    if (!this.isNarayana)
      return;
    this.Spine.AnimationState.SetAnimation(0, "Activities/activity-flute", true);
    this._followerInfo.Name = "River Boy";
  }

  public override void Update()
  {
    base.Update();
    if (!this.isNarayana)
      return;
    this.transform.position = this.startingPosition;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (!this.isNarayana)
      return;
    this.Label = this.Interactable ? ScriptLocalization.Interactions.Talk : "";
  }

  public void RandomiseBark()
  {
    int num = UnityEngine.Random.Range(0, 5);
    foreach (ConversationEntry entry in this.simpleBark.Entries)
    {
      switch (PlayerFarming.Location)
      {
        case FollowerLocation.Dungeon1_5:
          num = 5;
          break;
        case FollowerLocation.Dungeon1_6:
          num = 6;
          break;
      }
      string translation = LocalizationManager.GetTranslation("Conversation_NPC/FreeNPC/" + num.ToString());
      string Term = "";
      switch (num)
      {
        case 2:
          switch (PlayerFarming.Location)
          {
            case FollowerLocation.Dungeon1_1:
              Term = "NAMES/CultLeaders/Dungeon1";
              break;
            case FollowerLocation.Dungeon1_2:
              Term = "NAMES/CultLeaders/Dungeon2";
              break;
            case FollowerLocation.Dungeon1_3:
              Term = "NAMES/CultLeaders/Dungeon3";
              break;
            case FollowerLocation.Dungeon1_4:
              Term = "NAMES/CultLeaders/Dungeon4";
              break;
          }
          break;
        case 3:
        case 4:
          switch (PlayerFarming.Location)
          {
            case FollowerLocation.Dungeon1_1:
              Term = "NAMES/Places/Dungeon1_1";
              break;
            case FollowerLocation.Dungeon1_2:
              Term = "NAMES/Places/Dungeon1_2";
              break;
            case FollowerLocation.Dungeon1_3:
              Term = "NAMES/Places/Dungeon1_3";
              break;
            case FollowerLocation.Dungeon1_4:
              Term = "NAMES/Places/Dungeon1_4";
              break;
          }
          break;
      }
      string str = $"<color=yellow>{LocalizationManager.GetTranslation(Term)}</color>";
      entry.Speaker = this.Spine.gameObject;
      entry.SkeletonData = this.Spine;
      entry.TermToSpeak = string.Format(translation, (object) str);
      if (DataManager.Instance.GiveExecutionerFollower)
        entry.TermToSpeak = "Conversation_NPC/FollowerRescue/Executioner/0";
    }
    this.simpleBark.Translate = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sRescue = ScriptLocalization.Interactions.Rescue;
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if ((bool) (UnityEngine.Object) this.simpleBark)
    {
      this.simpleBark.Close();
      this.simpleBark.enabled = false;
    }
    this.Interactable = false;
    this.EndIndicateHighlighted(this.playerFarming);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(state.gameObject, 5f);
    this.GetComponent<FMODLoopSound>()?.StopLoop();
    if (DataManager.Instance.GiveExecutionerFollower)
    {
      DataManager.Instance.GivenExecutionerFollower = true;
      DataManager.Instance.ExecutionerFollowerNoteGiverID = this._followerInfo.ID;
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(this.gameObject, "Conversation_NPC/FollowerRescue/Executioner/1")
      }, (List<MMTools.Response>) null, (System.Action) (() =>
      {
        RoomLockController.RoomCompleted();
        this.\u003C\u003En__0(state);
      })), false);
    }
    else if (this.isNarayana)
    {
      this.Interactable = false;
      this.HasChanged = true;
      this._followerInfo.ID = 10009;
      this.Spine.AnimationState.SetAnimation(0, "Activities/activity-flute-talk", true);
      DataManager.Instance.GivenNarayanaFollower = true;
      List<ConversationEntry> Entries = new List<ConversationEntry>()
      {
        new ConversationEntry(this.gameObject, "Conversation_NPC/FindFollower/Narayana/0"),
        new ConversationEntry(this.gameObject, "Conversation_NPC/FindFollower/Narayana/1")
      };
      foreach (ConversationEntry conversationEntry in Entries)
      {
        string str1;
        string str2 = str1 = "River Boy";
        conversationEntry.TermName = str1;
        conversationEntry.CharacterName = str2;
      }
      this._followerInfo.Traits.Clear();
      this._followerInfo.Traits.Add(FollowerTrait.TraitType.Nudist);
      this._followerInfo.Traits.Add(FollowerTrait.TraitType.MusicLover);
      this._followerInfo.Traits.Add(FollowerTrait.TraitType.FluteLover);
      this._followerInfo.Traits.Add(FollowerTrait.TraitType.Immortal);
      this._followerInfo.TraitsSet = true;
      this._followerInfo.SkinName = "Narayana";
      this._followerInfo.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName("Narayana");
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => this.\u003C\u003En__0(state))), false);
    }
    else
      this.StartCoroutine((IEnumerator) this.Delay((System.Action) (() => this.\u003C\u003En__0(state))));
  }

  public IEnumerator Delay(System.Action callback)
  {
    yield return (object) new WaitForSeconds(1f);
    System.Action action = callback;
    if (action != null)
      action();
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0(StateMachine state) => base.OnInteract(state);
}
