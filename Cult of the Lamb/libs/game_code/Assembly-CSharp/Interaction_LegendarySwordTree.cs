// Decompiled with JetBrains decompiler
// Type: Interaction_LegendarySwordTree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_LegendarySwordTree : Interaction
{
  [SerializeField]
  public Transform chosenChildPosition;
  [SerializeField]
  public Interaction_LegendarySword legendarySword;
  public FollowerManager.SpawnedFollower spawnedFollower;

  public bool canGiveDemon
  {
    get
    {
      return DataManager.Instance.Followers_Demons_IDs.Contains(100000) && !DataManager.Instance.FoundLegendarySword && DataManager.Instance.ChosenChildMeditationQuestDay <= 0 && DataManager.Instance.GaveChosenChildQuest;
    }
  }

  public bool canPullTheSword
  {
    get
    {
      return TimeManager.CurrentDay - DataManager.Instance.ChosenChildMeditationQuestDay > 3 && DataManager.Instance.ChosenChildMeditationQuestDay > 0;
    }
  }

  public void Awake()
  {
    if (!DataManager.Instance.ChosenChildLeftInTheMidasCave || this.spawnedFollower != null)
      return;
    this.spawnedFollower = this.SpawnFollower(FollowerInfo.GetInfoByID(100000), this.chosenChildPosition.position);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.canPullTheSword)
    {
      this.AutomaticallyInteract = DataManager.Instance.ChosenChildLeftInTheMidasCave;
      this.ActivateDistance = 7.5f;
      this.legendarySword.enabled = !DataManager.Instance.ChosenChildLeftInTheMidasCave;
      this.legendarySword.OnSwordPulled += new System.Action(this.OnSwordPulled);
    }
    else
      this.legendarySword.gameObject.SetActive(false);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.legendarySword.OnSwordPulled -= new System.Action(this.OnSwordPulled);
  }

  public override void GetLabel()
  {
    if (this.canGiveDemon)
    {
      this.Interactable = true;
      this.Label = "Give demon";
    }
    else if (this.canPullTheSword && DataManager.Instance.ChosenChildLeftInTheMidasCave)
    {
      this.AutomaticallyInteract = true;
      this.ActivateDistance = 7.5f;
      this.Label = "Pulling sword";
    }
    else
    {
      this.Label = "";
      this.AutomaticallyInteract = false;
      this.Interactable = false;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.canGiveDemon)
    {
      this.Interactable = false;
      this.StartCoroutine(this.GiveDemon());
    }
    else
    {
      if (!this.canPullTheSword || !DataManager.Instance.ChosenChildLeftInTheMidasCave)
        return;
      this.Interactable = false;
      this.PlayEndMeditationConvo();
    }
  }

  public IEnumerator GiveDemon()
  {
    Interaction_LegendarySwordTree legendarySwordTree = this;
    yield return (object) new WaitForEndOfFrame();
    int ID = 100000;
    FollowerBrain followerBrain = FollowerBrain.FindBrainByID(ID);
    int demonType = DemonModel.GetDemonType(followerBrain._directInfoAccess);
    Demon_Spirit demon = (Demon_Spirit) null;
    foreach (GameObject demon1 in Demon_Arrows.Demons)
    {
      Demon_Spirit component = demon1.GetComponent<Demon_Spirit>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.FollowerInfo.ID == ID)
      {
        demon = component;
        break;
      }
    }
    int index = 0;
    foreach (int followersDemonsType in DataManager.Instance.Followers_Demons_Types)
    {
      if (followersDemonsType == demonType)
      {
        int followersDemonsId = DataManager.Instance.Followers_Demons_IDs[index];
        DataManager.Instance.Followers_Demons_Types.RemoveAt(index);
        DataManager.Instance.Followers_Demons_IDs.RemoveAt(index);
        break;
      }
      ++index;
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(legendarySwordTree.chosenChildPosition.gameObject);
    demon.enabled = false;
    demon.simpleSpineAnimator.transform.DOMove(legendarySwordTree.chosenChildPosition.transform.position + Vector3.back * 0.5f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InExpo);
    AudioManager.Instance.PlayOneShot("event:/Stings/refuse_kneel_sting");
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/door/goop_door_unlock");
    demon.gameObject.SetActive(false);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(legendarySwordTree.chosenChildPosition.position);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    DataManager.Instance.Followers_LeftInTheDungeon_IDs.Add(followerBrain.Info.ID);
    followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_LeftInTheDungeon());
    legendarySwordTree.spawnedFollower = legendarySwordTree.SpawnFollower(followerBrain._directInfoAccess, legendarySwordTree.chosenChildPosition.position);
    GameManager.GetInstance().OnConversationNext(legendarySwordTree.chosenChildPosition.gameObject, 5f);
    legendarySwordTree.playerFarming.GoToAndStop(legendarySwordTree.chosenChildPosition.position + Vector3.left * 2.5f, legendarySwordTree.chosenChildPosition.gameObject);
    yield return (object) new WaitUntil(new Func<bool>(legendarySwordTree.\u003CGiveDemon\u003Eb__12_0));
    legendarySwordTree.PlayStartMeditationConvo();
  }

  public FollowerManager.SpawnedFollower SpawnFollower(FollowerInfo followerInfo, Vector3 position)
  {
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.FollowerPrefab, followerInfo, position, this.transform.parent, BiomeGenerator.Instance.DungeonLocation);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "spawn-in", false);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "meditate-enlightenment", true, 0.0f);
    return spawnedFollower;
  }

  public void PlayStartMeditationConvo()
  {
    this.spawnedFollower.Follower.FacePosition(this.playerFarming.transform.position);
    string str = "Conversation_NPC/ChosenChild/LegendarySword/StartMeditation/";
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(this.spawnedFollower.Follower.gameObject, str + "0"),
      new ConversationEntry(this.spawnedFollower.Follower.gameObject, str + "1")
    };
    foreach (ConversationEntry conversationEntry in Entries)
    {
      conversationEntry.CharacterName = this.spawnedFollower.FollowerBrain.Info.Name;
      conversationEntry.Animation = "worship/talk";
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
    DataManager.Instance.ChosenChildLeftInTheMidasCave = true;
    DataManager.Instance.ChosenChildMeditationQuestDay = TimeManager.CurrentDay;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.LegendarySword);
  }

  public void PlayEndMeditationConvo()
  {
    string str = "Conversation_NPC/ChosenChild/LegendarySword/EndMeditation/";
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(this.spawnedFollower.Follower.gameObject, str + "0"),
      new ConversationEntry(this.spawnedFollower.Follower.gameObject, str + "1")
    };
    foreach (ConversationEntry conversationEntry in Entries)
    {
      conversationEntry.CharacterName = this.spawnedFollower.FollowerBrain.Info.Name;
      conversationEntry.Animation = "worship/talk";
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
    this.legendarySword.enabled = true;
  }

  public IEnumerator TeleportOutSequence()
  {
    Interaction_LegendarySwordTree legendarySwordTree = this;
    DataManager.Instance.Followers_LeftInTheDungeon_IDs.Remove(legendarySwordTree.spawnedFollower.Follower.Brain.Info.ID);
    DataManager.Instance.ChosenChildLeftInTheMidasCave = false;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(legendarySwordTree.gameObject);
    yield return (object) new WaitForSeconds(1.5f);
    yield return (object) new WaitForSeconds(legendarySwordTree.spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "spawn-out", false).Animation.Duration);
    legendarySwordTree.spawnedFollower.Follower.gameObject.SetActive(false);
    if (legendarySwordTree.spawnedFollower.Follower.Brain.CurrentTaskType == FollowerTaskType.LeftInTheDungeon)
      legendarySwordTree.spawnedFollower.Follower.Brain.CurrentTask.Abort();
    GameManager.GetInstance().OnConversationEnd();
  }

  public void OnSwordPulled()
  {
    Inventory.AddItem(InventoryItem.ITEM_TYPE.BROKEN_WEAPON_SWORD, 1);
    DataManager.Instance.FoundLegendarySword = true;
    ObjectiveManager.Add((ObjectivesData) new Objectives_GiveItem("Objectives/GroupTitles/LegendarySword", "NAMES/BlacksmithNPC", 1, InventoryItem.ITEM_TYPE.BROKEN_WEAPON_SWORD), true, true);
    if (!DataManager.Instance.ChosenChildLeftInTheMidasCave)
      return;
    this.StartCoroutine(this.TeleportOutSequence());
  }

  [CompilerGenerated]
  public bool \u003CGiveDemon\u003Eb__12_0() => this.playerFarming.GoToAndStopping;
}
