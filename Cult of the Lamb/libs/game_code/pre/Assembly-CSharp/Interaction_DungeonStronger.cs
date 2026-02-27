// Decompiled with JetBrains decompiler
// Type: Interaction_DungeonStronger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_DungeonStronger : Interaction
{
  private string convoText = "Conversation_NPC/Haro/Dungeon{0}/{1}";
  private List<ConversationEntry> entries = new List<ConversationEntry>();
  public UnityEvent Callback;
  public Interaction_WeaponSelectionPodium[] Weapons;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 3f;
    this.AutomaticallyInteract = true;
    LocationManager.OnPlayerLocationSet += new System.Action(this.OnPlayerLocationSet);
  }

  private void OnPlayerLocationSet()
  {
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
    if (this.RevealSelf())
    {
      this.LockDoors();
      this.HideWeapons();
    }
    else
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
  }

  private bool RevealSelf()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        return !DataManager.Instance.HaroOnbardedHarderDungeon1 && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) && GameManager.CurrentDungeonFloor <= 1;
      case FollowerLocation.Dungeon1_2:
        return !DataManager.Instance.HaroOnbardedHarderDungeon2 && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2) && GameManager.CurrentDungeonFloor <= 1;
      case FollowerLocation.Dungeon1_3:
        return !DataManager.Instance.HaroOnbardedHarderDungeon3 && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3) && GameManager.CurrentDungeonFloor <= 1;
      case FollowerLocation.Dungeon1_4:
        return !DataManager.Instance.HaroOnbardedHarderDungeon4 && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4) && GameManager.CurrentDungeonFloor <= 1;
      default:
        return false;
    }
  }

  private void SetVariable()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        DataManager.Instance.HaroOnbardedHarderDungeon1 = true;
        break;
      case FollowerLocation.Dungeon1_2:
        DataManager.Instance.HaroOnbardedHarderDungeon2 = true;
        break;
      case FollowerLocation.Dungeon1_3:
        DataManager.Instance.HaroOnbardedHarderDungeon3 = true;
        break;
      case FollowerLocation.Dungeon1_4:
        DataManager.Instance.HaroOnbardedHarderDungeon4 = true;
        break;
    }
  }

  private int DungeonNumber()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        return 1;
      case FollowerLocation.Dungeon1_2:
        return 2;
      case FollowerLocation.Dungeon1_3:
        return 3;
      case FollowerLocation.Dungeon1_4:
        return 4;
      default:
        return 0;
    }
  }

  private string LocationName()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        return ScriptLocalization.NAMES_Places.Dungeon1_1;
      case FollowerLocation.Dungeon1_2:
        return ScriptLocalization.NAMES_Places.Dungeon1_2;
      case FollowerLocation.Dungeon1_3:
        return ScriptLocalization.NAMES_Places.Dungeon1_3;
      case FollowerLocation.Dungeon1_4:
        return ScriptLocalization.NAMES_Places.Dungeon1_4;
      default:
        return "";
    }
  }

  private void HideWeapons()
  {
    foreach (Interaction_WeaponSelectionPodium weapon in this.Weapons)
    {
      if ((bool) (UnityEngine.Object) weapon)
        weapon.gameObject.SetActive(false);
    }
  }

  private void RevealWeapons()
  {
    foreach (Interaction_WeaponSelectionPodium weapon in this.Weapons)
    {
      if ((bool) (UnityEngine.Object) weapon)
        weapon.gameObject.SetActive(true);
    }
    Interaction_Chest.ChestEvent onChestRevealed = Interaction_Chest.OnChestRevealed;
    if (onChestRevealed == null)
      return;
    onChestRevealed();
  }

  private void LockDoors()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.LockDoors);
    RoomLockController.CloseAll();
  }

  public override void GetLabel()
  {
    this.Label = this.Interactable ? ScriptLocalization.Interactions.Talk : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.InteractIE());
  }

  private IEnumerator InteractIE()
  {
    Interaction_DungeonStronger interactionDungeonStronger = this;
    PlayerFarming.Instance.GoToAndStop(interactionDungeonStronger.transform.position + Vector3.left * 2f);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, interactionDungeonStronger.transform.position);
    PlayerFarming.Instance.state.LookAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, interactionDungeonStronger.transform.position);
    for (int index = 0; LocalizationManager.GetTermData(string.Format(interactionDungeonStronger.convoText, (object) interactionDungeonStronger.DungeonNumber(), (object) index)) != null; ++index)
    {
      string TermToSpeak = string.Format(interactionDungeonStronger.convoText, (object) interactionDungeonStronger.DungeonNumber(), (object) index);
      interactionDungeonStronger.entries.Add(new ConversationEntry(interactionDungeonStronger.gameObject, TermToSpeak)
      {
        CharacterName = "NAMES/Haro",
        soundPath = "event:/dialogue/haro/standard_haro"
      });
    }
    MMConversation.Play(new ConversationObject(interactionDungeonStronger.entries, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    interactionDungeonStronger.Callback?.Invoke();
    yield return (object) new WaitForSeconds(0.5f);
    NotificationCentreScreen.Play(string.Format(LocalizationManager.GetTranslation("Notifications/EnemiesStronger"), (object) $"<color=#FFD201>{interactionDungeonStronger.LocationName()}</color>"));
    interactionDungeonStronger.SetVariable();
    GameManager.GetInstance().StartCoroutine((IEnumerator) interactionDungeonStronger.DelayWeaponsReveal());
  }

  private IEnumerator DelayWeaponsReveal()
  {
    yield return (object) new WaitForSeconds(3f);
    this.RevealWeapons();
  }
}
