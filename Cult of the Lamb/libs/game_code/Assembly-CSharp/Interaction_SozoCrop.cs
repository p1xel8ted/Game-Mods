// Decompiled with JetBrains decompiler
// Type: Interaction_SozoCrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_SozoCrop : Interaction
{
  [SerializeField]
  public GameObject spawnPos;
  [SerializeField]
  public GameObject lightingVolume;
  [SerializeField]
  public GameObject SpritesGO;

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.PickSozoSeed;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.SpawnWebberFollowerIE());
  }

  public IEnumerator SpawnWebberFollowerIE()
  {
    Interaction_SozoCrop interactionSozoCrop = this;
    string sozoSkin = "Sozo";
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || interactionSozoCrop.playerFarming.state.CURRENT_STATE != StateMachine.State.Idle && interactionSozoCrop.playerFarming.state.CURRENT_STATE != StateMachine.State.Moving && interactionSozoCrop.playerFarming.state.CURRENT_STATE != StateMachine.State.Idle_Winter && interactionSozoCrop.playerFarming.state.CURRENT_STATE != StateMachine.State.Moving_Winter)
      yield return (object) null;
    interactionSozoCrop.playerFarming.GoToAndStop(interactionSozoCrop.transform.position + Vector3.right * 1.25f, interactionSozoCrop.spawnPos);
    GameManager.GetInstance().OnConversationNew(interactionSozoCrop.playerFarming);
    GameManager.GetInstance().OnConversationNext(interactionSozoCrop.spawnPos, 3f);
    yield return (object) new WaitForSeconds(1f);
    interactionSozoCrop.SpritesGO.SetActive(false);
    interactionSozoCrop.GetComponentInParent<CropController>().Harvest();
    FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.Base, sozoSkin);
    info.Name = ScriptLocalization.NAMES.Sozo;
    info.ID = 99996;
    FollowerBrain resurrectingFollower = FollowerBrain.GetOrCreateBrain(info);
    resurrectingFollower.ResetStats();
    if (resurrectingFollower.Info.Age > resurrectingFollower.Info.LifeExpectancy)
      resurrectingFollower.Info.LifeExpectancy = resurrectingFollower.Info.Age + UnityEngine.Random.Range(20, 30);
    else
      resurrectingFollower.Info.LifeExpectancy += UnityEngine.Random.Range(20, 30);
    Follower revivedFollower = FollowerManager.CreateNewFollower(resurrectingFollower._directInfoAccess, interactionSozoCrop.spawnPos.transform.position);
    revivedFollower.Brain.AddThought(Thought.Sozo_Revived);
    resurrectingFollower.Location = FollowerLocation.Base;
    resurrectingFollower.DesiredLocation = FollowerLocation.Base;
    resurrectingFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    FollowerBrain.SetFollowerCostume(revivedFollower.Spine.Skeleton, 0, revivedFollower.Brain.Info.SkinName, revivedFollower.Brain.Info.SkinColour, FollowerOutfitType.None, revivedFollower.Brain.Info.Hat, revivedFollower.Brain.Info.Clothing, revivedFollower.Brain.Info.Customisation, FollowerSpecialType.Sozo, revivedFollower.Brain.Info.Necklace, revivedFollower.Brain.Info.ClothingVariant, revivedFollower.Brain._directInfoAccess);
    revivedFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) revivedFollower.SetBodyAnimation("sozo-spawn", false);
    revivedFollower.AddBodyAnimation("idle", true, 0.0f);
    revivedFollower.State.LockStateChanges = true;
    revivedFollower.Interaction_FollowerInteraction.eventListener.SetPitchAndVibrator(revivedFollower.Brain._directInfoAccess.follower_pitch, revivedFollower.Brain._directInfoAccess.follower_vibrato, revivedFollower.Brain._directInfoAccess.ID);
    revivedFollower.Spine.AnimationState.Event += (Spine.AnimationState.TrackEntryEventDelegate) ((trackEntry, e) =>
    {
      if (e.Data.Name == "Audio/fol sozo pops up from ground")
      {
        AudioManager.Instance.PlayOneShot("event:/followers/npc/sozo_is_born", revivedFollower.gameObject);
      }
      else
      {
        if (!(e.Data.Name == "Audio/sozo wave"))
          return;
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/npc/fol_sozo_wave", revivedFollower.gameObject);
      }
    });
    yield return (object) new WaitForEndOfFrame();
    revivedFollower.FacePosition(interactionSozoCrop.playerFarming.transform.position);
    revivedFollower.HideAllFollowerIcons();
    AudioManager.Instance.PlayOneShot("event:/Stings/thenight_sacrifice_followers");
    float t = 0.0f;
    while ((double) t < 3.2999999523162842)
    {
      t += Time.deltaTime;
      CameraManager.instance.shakeCamera1(t / 6f, UnityEngine.Random.Range(0.0f, 360f));
      MMVibrate.RumbleContinuous(t / 6f, t / 4f, interactionSozoCrop.playerFarming);
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground");
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_reveal");
    CameraManager.instance.ShakeCameraForDuration(1.25f, 1.5f, 0.25f);
    GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 6f);
    MMVibrate.StopRumble();
    yield return (object) null;
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    yield return (object) new WaitForSeconds(3f);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionSozoCrop.lightingVolume);
    revivedFollower.State.LockStateChanges = true;
    List<ConversationEntry> Entries1 = new List<ConversationEntry>()
    {
      new ConversationEntry(revivedFollower.gameObject, "Conversation_NPC/SozoFollower/Intro/0"),
      new ConversationEntry(revivedFollower.gameObject, "Conversation_NPC/SozoFollower/Intro/1"),
      new ConversationEntry(revivedFollower.gameObject, "Conversation_NPC/SozoFollower/Intro/2"),
      new ConversationEntry(revivedFollower.gameObject, "Conversation_NPC/SozoFollower/Intro/3")
    };
    foreach (ConversationEntry conversationEntry in Entries1)
    {
      conversationEntry.CharacterName = info.Name;
      conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
      conversationEntry.LoopAnimation = false;
      conversationEntry.soundPath = " ";
    }
    MMConversation.Play(new ConversationObject(Entries1, (List<MMTools.Response>) null, (System.Action) null), false);
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    revivedFollower.State.LockStateChanges = false;
    DataManager.Instance.SozoAteMushroomDay = TimeManager.CurrentDay;
    int mushrooms = 5;
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL) >= mushrooms)
    {
      GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
      ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
      choice.Offset = new Vector3(0.0f, -300f);
      bool gaveMushrooms = true;
      choice.Show(string.Format(ScriptLocalization.UI_ItemSelector_Context.Give, (object) InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, mushrooms)), "UI/Generic/Decline", (System.Action) (() =>
      {
        revivedFollower.TimedAnimation("Reactions/react-happy1", 1.86666667f);
        revivedFollower.AddBodyAnimation("Conversations/talk-nice2", true, 0.0f);
        GameManager.GetInstance().StartCoroutine((IEnumerator) this.GiveItemsRoutine(revivedFollower.gameObject, InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, mushrooms, (System.Action) (() =>
        {
          g = (GameObject) null;
          if (revivedFollower.Brain.Info.ID != 99996)
            return;
          revivedFollower.Brain._directInfoAccess.SozoBrainshed = true;
          revivedFollower.Brain.CheckChangeState();
        })));
      }), (System.Action) (() =>
      {
        gaveMushrooms = false;
        revivedFollower.TimedAnimation("Reactions/react-sad", 1.86666667f);
        revivedFollower.AddBodyAnimation("Conversations/talk-nice2", true, 0.0f);
        g = (GameObject) null;
      }), revivedFollower.transform.position);
      while ((UnityEngine.Object) g != (UnityEngine.Object) null)
      {
        choice.UpdatePosition(revivedFollower.transform.position);
        yield return (object) null;
      }
      List<ConversationEntry> Entries2 = new List<ConversationEntry>()
      {
        new ConversationEntry(revivedFollower.gameObject, gaveMushrooms ? "Conversation_NPC/SozoFollower/ReceivedMushrooms/0" : "Conversation_NPC/SozoFollower/DeclinedMushrooms/0"),
        new ConversationEntry(revivedFollower.gameObject, gaveMushrooms ? "Conversation_NPC/SozoFollower/ReceivedMushrooms/1" : "Conversation_NPC/SozoFollower/DeclinedMushrooms/1")
      };
      foreach (ConversationEntry conversationEntry in Entries2)
      {
        conversationEntry.CharacterName = info.Name;
        conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
        conversationEntry.LoopAnimation = false;
        conversationEntry.soundPath = " ";
      }
      MMConversation.Play(new ConversationObject(Entries2, (List<MMTools.Response>) null, (System.Action) null), false);
      yield return (object) null;
      while (MMConversation.isPlaying)
        yield return (object) null;
      choice = (ChoiceIndicator) null;
    }
    revivedFollower.State.LockStateChanges = false;
    resurrectingFollower.CompleteCurrentTask();
    FollowerBrain.SetFollowerCostume(revivedFollower.Spine.Skeleton, revivedFollower.Brain._directInfoAccess, forceUpdate: true);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(1f);
    revivedFollower.ShowAllFollowerIcons();
  }

  public IEnumerator GiveItemsRoutine(
    GameObject target,
    InventoryItem.ITEM_TYPE itemType,
    int quantity,
    System.Action callback)
  {
    Interaction_SozoCrop interactionSozoCrop = this;
    for (int i = 0; i < quantity; ++i)
    {
      ResourceCustomTarget.Create(target, interactionSozoCrop.playerFarming.transform.position, itemType, (System.Action) null);
      yield return (object) new WaitForSeconds(0.025f);
    }
    Inventory.ChangeItemQuantity(itemType, -quantity);
    System.Action action = callback;
    if (action != null)
      action();
  }
}
