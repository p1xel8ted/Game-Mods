// Decompiled with JetBrains decompiler
// Type: Interaction_FirstFreeFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using MMTools;
using System.Collections;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class Interaction_FirstFreeFollower : Interaction_FollowerSpawn
{
  private SimpleBark simpleBark;
  private EventInstance receiveLoop;
  private Villager_Info v_i;
  private string skin;
  private WorshipperInfoManager wim;

  protected override void Start()
  {
    base.Start();
    this.Play("", animate: false);
    this.simpleBark = this.GetComponent<SimpleBark>();
    this.RandomiseBark();
    DataManager.Instance.GivenFreeDungeonFollower = true;
    this.skin = "";
    this.skin = DataManager.GetRandomLockedSkin();
    if (this.skin.IsNullOrEmpty())
      this.skin = DataManager.GetRandomSkin();
    Debug.Log((object) "?????????????????????????????????");
    Debug.Log((object) ("skin: " + this.skin));
    this._followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, this.skin);
    if (this._followerInfo.SkinName == "Giraffe")
      this._followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Sparkles");
    this.v_i = Villager_Info.NewCharacter(this.skin);
    this.wim = this.GetComponent<WorshipperInfoManager>();
    this.wim.SetV_I(this.v_i);
  }

  private void RandomiseBark()
  {
    int num = UnityEngine.Random.Range(0, 5);
    foreach (ConversationEntry entry in this.simpleBark.Entries)
    {
      string translation = LocalizationManager.GetTranslation("Conversation_NPC/FreeNPC/" + (object) num);
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
      entry.TermToSpeak = string.Format(translation, (object) str);
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
    this.simpleBark.Close();
    this.simpleBark.enabled = false;
    this.Interactable = false;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(state.gameObject, 5f);
    this.StartCoroutine((IEnumerator) this.Delay((System.Action) (() => base.OnInteract(state))));
  }

  private IEnumerator Delay(System.Action callback)
  {
    yield return (object) new WaitForSeconds(1f);
    System.Action action = callback;
    if (action != null)
      action();
  }
}
