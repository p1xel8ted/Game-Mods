// Decompiled with JetBrains decompiler
// Type: Interaction_MonsterHeart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.UI.Overlays.TutorialOverlay;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_MonsterHeart : Interaction
{
  public Vector3 MoveToPosition;
  private bool Enabled = true;
  private PlayerFarming playerFarming;
  public SimpleSpineAnimator simpleSpineAnimator;
  private string sMonsterHeart;
  public Interaction_Chest Chest;
  private Coroutine heartBeatingRoutine;
  public Objectives.CustomQuestTypes ObjectiveToComplete;

  private void Start()
  {
    this.UpdateLocalisation();
    this.heartBeatingRoutine = this.StartCoroutine((IEnumerator) this.BeatingHeartRoutine());
  }

  private IEnumerator BeatingHeartRoutine()
  {
    while (true)
    {
      yield return (object) new WaitForSeconds(0.4f);
      AudioManager.Instance.PlayOneShot("event:/monster_heart/monster_heart_beat");
      yield return (object) new WaitForSeconds(0.7f);
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sMonsterHeart = ScriptLocalization.Interactions.MonsterHeart;
  }

  public override void GetLabel() => this.Label = this.Enabled ? this.sMonsterHeart : "";

  public void Play()
  {
    if (!DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
    {
      this.enabled = true;
    }
    else
    {
      RoomLockController.RoomCompleted();
      this.Chest.RevealBossReward(InventoryItem.ITEM_TYPE.NONE);
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!this.Enabled)
      return;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(state.gameObject, 7f);
    this.playerFarming = state.GetComponent<PlayerFarming>();
    this.playerFarming.GoToAndStop(new GameObject()
    {
      transform = {
        position = this.transform.position + this.MoveToPosition
      }
    }, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.TakeHeart())));
    this.Enabled = false;
  }

  private IEnumerator TakeHeart()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_MonsterHeart interactionMonsterHeart = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionMonsterHeart.HeartHasBeenTaken();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionMonsterHeart.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    interactionMonsterHeart.playerFarming.state.facingAngle = Utils.GetAngle(interactionMonsterHeart.playerFarming.transform.position, interactionMonsterHeart.transform.position);
    interactionMonsterHeart.playerFarming.CustomAnimation("get-heart", false);
    interactionMonsterHeart.playerFarming.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(interactionMonsterHeart.OnSpineEvent);
    AudioManager.Instance.PlayOneShot("event:/monster_heart/monster_heart_sequence");
    interactionMonsterHeart.StopCoroutine(interactionMonsterHeart.heartBeatingRoutine);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(3f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "take-heart":
        CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.playerFarming.transform.position, this.transform.position));
        if ((UnityEngine.Object) this.simpleSpineAnimator != (UnityEngine.Object) null)
        {
          this.simpleSpineAnimator.Animate("dead-noheart", 0, false);
          break;
        }
        SkeletonAnimation componentInChildren = this.GetComponentInChildren<SkeletonAnimation>();
        if (!(bool) (UnityEngine.Object) componentInChildren)
          break;
        componentInChildren.AnimationState.SetAnimation(0, "dead-noheart", false);
        break;
      case "monster-heart-sound":
        AudioManager.Instance.PlayOneShot("event:/monster_heart/monster_heart_beat", this.transform.position);
        break;
      case "monster-heart-zoom":
        GameManager.GetInstance().OnConversationNext(this.state.gameObject, 5f);
        break;
    }
  }

  private void HeartHasBeenTaken()
  {
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.MonsterHeart))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.MonsterHeart);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/MonsterHeart", Objectives.CustomQuestTypes.MonsterHeart), true);
        if ((bool) (UnityEngine.Object) ChainDoor.Instance)
          ChainDoor.Instance.Play((System.Action) (() => this.StartCoroutine((IEnumerator) this.DoHeartTaken())));
        else
          this.StartCoroutine((IEnumerator) this.DoHeartTaken());
      });
    }
    else if ((bool) (UnityEngine.Object) ChainDoor.Instance)
      ChainDoor.Instance.Play((System.Action) (() => this.StartCoroutine((IEnumerator) this.DoHeartTaken())));
    else
      this.StartCoroutine((IEnumerator) this.DoHeartTaken());
  }

  public event Interaction_MonsterHeart.HeartTaken OnHeartTaken;

  private IEnumerator DoHeartTaken()
  {
    Interaction_MonsterHeart interactionMonsterHeart = this;
    interactionMonsterHeart.state.CURRENT_STATE = StateMachine.State.InActive;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      allBrain.AddThought(Thought.InAweOfLeaderChain);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/door/goop_door_unlock");
    GameManager.GetInstance().OnConversationEnd();
    Debug.Log((object) "TAKEN?");
    interactionMonsterHeart.Chest?.RevealBossReward(InventoryItem.ITEM_TYPE.NONE);
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillABossAndTakeTheirHeart);
    if (interactionMonsterHeart.ObjectiveToComplete != Objectives.CustomQuestTypes.None)
      ObjectiveManager.CompleteCustomObjective(interactionMonsterHeart.ObjectiveToComplete);
    Inventory.AddItem(22, 1);
    DataManager.Instance.BossesCompleted.Add(PlayerFarming.Location);
    Interaction_MonsterHeart.HeartTaken onHeartTaken = interactionMonsterHeart.OnHeartTaken;
    if (onHeartTaken != null)
      onHeartTaken();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionMonsterHeart);
  }

  public override void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    Utils.DrawCircleXY(this.transform.position + this.MoveToPosition, 0.4f, Color.blue);
  }

  public delegate void HeartTaken();
}
