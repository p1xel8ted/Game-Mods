// Decompiled with JetBrains decompiler
// Type: Interaction_DungeonStronger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public List<ConversationEntry> entries = new List<ConversationEntry>();
  [SerializeField]
  public string prefix = "Haro";
  public UnityEvent Callback;
  public Interaction_WeaponSelectionPodium[] Weapons;

  public string text
  {
    get
    {
      return PlayerFarming.Location == FollowerLocation.Dungeon1_1 && !DataManager.Instance.HaroOnbardedHarderDungeon1 || PlayerFarming.Location == FollowerLocation.Dungeon1_2 && !DataManager.Instance.HaroOnbardedHarderDungeon2 || PlayerFarming.Location == FollowerLocation.Dungeon1_3 && !DataManager.Instance.HaroOnbardedHarderDungeon3 || PlayerFarming.Location == FollowerLocation.Dungeon1_4 && !DataManager.Instance.HaroOnbardedHarderDungeon4 || PlayerFarming.Location == FollowerLocation.Dungeon1_6 && !DataManager.Instance.HaroOnbardedDungeon6 || !GameManager.Layer2 ? $"Conversation_NPC/{this.prefix}/Dungeon{{0}}/{{1}}" : $"Conversation_NPC/{this.prefix}/Special/Encounter_{{0}}/{{1}}";
    }
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 3f;
    this.AutomaticallyInteract = true;
    LocationManager.OnPlayerLocationSet += new System.Action(this.OnPlayerLocationSet);
  }

  public void OnPlayerLocationSet()
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

  public bool RevealSelf()
  {
    if (DungeonSandboxManager.Active)
      return false;
    if (GameManager.Layer2)
    {
      switch (PlayerFarming.Location)
      {
        case FollowerLocation.Dungeon1_1:
          return !DataManager.Instance.HaroOnbardedHarderDungeon1_PostGame && GameManager.CurrentDungeonFloor <= 1;
        case FollowerLocation.Dungeon1_2:
          return !DataManager.Instance.HaroOnbardedHarderDungeon2_PostGame && GameManager.CurrentDungeonFloor <= 1;
        case FollowerLocation.Dungeon1_3:
          return !DataManager.Instance.HaroOnbardedHarderDungeon3_PostGame && GameManager.CurrentDungeonFloor <= 1;
        case FollowerLocation.Dungeon1_4:
          return !DataManager.Instance.HaroOnbardedHarderDungeon4_PostGame && GameManager.CurrentDungeonFloor <= 1;
      }
    }
    else
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
        case FollowerLocation.Dungeon1_6:
          return !DataManager.Instance.HaroOnbardedDungeon6 && GameManager.CurrentDungeonFloor <= 1 && GameManager.CurrentDungeonLayer <= 1;
      }
    }
    return false;
  }

  public void SetVariable()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        DataManager.Instance.HaroOnbardedHarderDungeon1_PostGame = DataManager.Instance.HaroOnbardedHarderDungeon1 = true;
        break;
      case FollowerLocation.Dungeon1_2:
        DataManager.Instance.HaroOnbardedHarderDungeon2_PostGame = DataManager.Instance.HaroOnbardedHarderDungeon2 = true;
        break;
      case FollowerLocation.Dungeon1_3:
        DataManager.Instance.HaroOnbardedHarderDungeon3_PostGame = DataManager.Instance.HaroOnbardedHarderDungeon3 = true;
        break;
      case FollowerLocation.Dungeon1_4:
        DataManager.Instance.HaroOnbardedHarderDungeon4_PostGame = DataManager.Instance.HaroOnbardedHarderDungeon4 = true;
        break;
      case FollowerLocation.Dungeon1_6:
        DataManager.Instance.HaroOnbardedDungeon6 = true;
        break;
    }
  }

  public int DungeonNumber()
  {
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_1 && !DataManager.Instance.HaroOnbardedHarderDungeon1 || PlayerFarming.Location == FollowerLocation.Dungeon1_2 && !DataManager.Instance.HaroOnbardedHarderDungeon2 || PlayerFarming.Location == FollowerLocation.Dungeon1_3 && !DataManager.Instance.HaroOnbardedHarderDungeon3 || PlayerFarming.Location == FollowerLocation.Dungeon1_4 && !DataManager.Instance.HaroOnbardedHarderDungeon4 || PlayerFarming.Location == FollowerLocation.Dungeon1_6 && !DataManager.Instance.HaroOnbardedDungeon6 || PlayerFarming.Location == FollowerLocation.Dungeon1_1 && !DataManager.Instance.HaroOnbardedHarderDungeon1_PostGame && GameManager.Layer2 || PlayerFarming.Location == FollowerLocation.Dungeon1_2 && !DataManager.Instance.HaroOnbardedHarderDungeon2_PostGame && GameManager.Layer2 || PlayerFarming.Location == FollowerLocation.Dungeon1_3 && !DataManager.Instance.HaroOnbardedHarderDungeon3_PostGame && GameManager.Layer2 || PlayerFarming.Location == FollowerLocation.Dungeon1_4 && !DataManager.Instance.HaroOnbardedHarderDungeon4_PostGame && GameManager.Layer2)
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
        case FollowerLocation.Dungeon1_6:
          return 6;
      }
    }
    return 0;
  }

  public string LocationName()
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
      case FollowerLocation.Dungeon1_6:
        return ScriptLocalization.NAMES_Places.Dungeon1_6;
      default:
        return "";
    }
  }

  public void HideWeapons()
  {
    foreach (Interaction_WeaponSelectionPodium weapon in this.Weapons)
    {
      if ((bool) (UnityEngine.Object) weapon)
        weapon.gameObject.SetActive(false);
    }
  }

  public void RevealWeapons()
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

  public void LockDoors()
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

  public IEnumerator InteractIE()
  {
    Interaction_DungeonStronger interactionDungeonStronger = this;
    interactionDungeonStronger.playerFarming.GoToAndStop(interactionDungeonStronger.transform.position + Vector3.left * 2f);
    while (interactionDungeonStronger.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionDungeonStronger.playerFarming.state.facingAngle = Utils.GetAngle(interactionDungeonStronger.playerFarming.transform.position, interactionDungeonStronger.transform.position);
    interactionDungeonStronger.playerFarming.state.LookAngle = Utils.GetAngle(interactionDungeonStronger.playerFarming.transform.position, interactionDungeonStronger.transform.position);
    for (int index = 0; LocalizationManager.GetTermData(string.Format(interactionDungeonStronger.text, (object) interactionDungeonStronger.DungeonNumber(), (object) index)) != null; ++index)
    {
      string TermToSpeak = string.Format(interactionDungeonStronger.text, (object) interactionDungeonStronger.DungeonNumber(), (object) index);
      ConversationEntry conversationEntry = new ConversationEntry(interactionDungeonStronger.gameObject, TermToSpeak);
      conversationEntry.CharacterName = "NAMES/" + interactionDungeonStronger.prefix;
      if (interactionDungeonStronger.prefix == "Haro")
        conversationEntry.soundPath = "event:/dialogue/haro/standard_haro";
      interactionDungeonStronger.entries.Add(conversationEntry);
    }
    MMConversation.Play(new ConversationObject(interactionDungeonStronger.entries, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    interactionDungeonStronger.Callback?.Invoke();
    yield return (object) new WaitForSeconds(0.5f);
    if (PlayerFarming.Location != FollowerLocation.Dungeon1_6)
      NotificationCentreScreen.Play(string.Format(LocalizationManager.GetTranslation("Notifications/EnemiesStronger"), (object) $"<color=#FFD201>{interactionDungeonStronger.LocationName()}</color>"));
    interactionDungeonStronger.SetVariable();
    GameManager.GetInstance().StartCoroutine((IEnumerator) interactionDungeonStronger.DelayWeaponsReveal());
  }

  public IEnumerator DelayWeaponsReveal()
  {
    yield return (object) new WaitForSeconds(2f);
    this.RevealWeapons();
  }
}
