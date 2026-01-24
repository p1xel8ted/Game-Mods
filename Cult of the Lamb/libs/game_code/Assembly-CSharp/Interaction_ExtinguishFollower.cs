// Decompiled with JetBrains decompiler
// Type: Interaction_ExtinguishFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_ExtinguishFollower : Interaction
{
  public const float WATERING_DURATION_SECONDS = 0.95f;
  public float WateringTime = 0.95f;
  public new string label;
  public EventInstance loopedSound;
  public Follower follower;

  public void Start()
  {
    this.UpdateLocalisation();
    this.ActivateDistance = 2f;
    this.follower = this.GetComponentInParent<Follower>();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.label = LocalizationManager.GetTranslation("Interactions/Extinguish");
  }

  public override void GetLabel() => this.Label = this.label;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.WaterRoutine());
  }

  public IEnumerator WaterRoutine()
  {
    Interaction_ExtinguishFollower extinguishFollower = this;
    extinguishFollower.EndIndicateHighlighted();
    extinguishFollower.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    bool succeeded = false;
    bool failed = false;
    PlayerFarming.Instance.GoToAndStop(extinguishFollower.transform.position, extinguishFollower.transform.parent.gameObject, GoToCallback: (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone);
      this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      this.state.facingAngle = Utils.GetAngle(this.state.transform.position, this.transform.position);
      UIExtinguishMiniGameOverlay extinguishMiniGameOverlay = MonoSingleton<UIManager>.Instance.ExtinguishMinigameOverlayControllerTemplate.Instantiate<UIExtinguishMiniGameOverlay>();
      extinguishMiniGameOverlay.Configure(3);
      GameManager.GetInstance().WaitForSeconds(0.5f, new System.Action(extinguishMiniGameOverlay.Begin));
      extinguishMiniGameOverlay.OnSuccess += (UIExtinguishMiniGameOverlay.ExtinguishEvent) (() => succeeded = true);
      extinguishMiniGameOverlay.OnFailure += (UIExtinguishMiniGameOverlay.ExtinguishEvent) (() => failed = true);
      this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    }));
    while (!succeeded)
    {
      if (failed)
      {
        extinguishFollower.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Aflame());
        GameManager.GetInstance().OnConversationEnd();
        extinguishFollower.follower.Brain._directInfoAccess.Aflame += 10f;
        yield break;
      }
      yield return (object) null;
    }
    extinguishFollower.loopedSound = AudioManager.Instance.CreateLoop("event:/player/watering", extinguishFollower.gameObject, true);
    yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("oldstuff/Farming-water_track", 0, true);
    while ((double) (extinguishFollower.WateringTime -= Time.deltaTime) >= 0.0)
      yield return (object) null;
    AudioManager.Instance.StopLoop(extinguishFollower.loopedSound);
    extinguishFollower.WateringTime = 0.95f;
    GameManager.GetInstance().OnConversationEnd();
    extinguishFollower.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    extinguishFollower.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    for (int index = extinguishFollower.transform.childCount - 1; index >= 0; --index)
    {
      if (extinguishFollower.transform.GetChild(index).tag == "Structure_Hindrance")
        UnityEngine.Object.Destroy((UnityEngine.Object) extinguishFollower.transform.GetChild(index).gameObject);
    }
    extinguishFollower.follower.TimedAnimation("Soaked/shake-off", 2.8f);
    GameManager.GetInstance().WaitForSeconds(2.8f, (System.Action) (() =>
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.CHARCOAL, UnityEngine.Random.Range(1, 3), this.follower.transform.position);
      this.follower.RemoveCursedState(Thought.Aflame);
      this.follower.Brain.BurntToDeath = false;
    }));
    extinguishFollower.GetComponent<interaction_FollowerInteraction>().enabled = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) extinguishFollower);
  }
}
