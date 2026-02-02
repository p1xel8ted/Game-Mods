// Decompiled with JetBrains decompiler
// Type: Interaction_Extinguish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Extinguish : Interaction
{
  public const float WATERING_DURATION_SECONDS = 0.95f;
  public float WateringTime = 0.95f;
  public new string label;
  public EventInstance loopedSound;
  public Structure structure;

  public void Start()
  {
    this.UpdateLocalisation();
    this.structure = this.GetComponent<Structure>();
    this.ActivateDistance = 2f;
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
    Interaction_Extinguish interactionExtinguish = this;
    interactionExtinguish.EndIndicateHighlighted();
    bool succeeded = false;
    bool failed = false;
    PlayerFarming.Instance.GoToAndStop(interactionExtinguish.transform.position, interactionExtinguish.transform.parent.gameObject, GoToCallback: (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone);
      this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      this.state.facingAngle = Utils.GetAngle(this.state.transform.position, this.transform.position);
      UIExtinguishMiniGameOverlay extinguishMiniGameOverlay = MonoSingleton<UIManager>.Instance.ExtinguishMinigameOverlayControllerTemplate.Instantiate<UIExtinguishMiniGameOverlay>();
      extinguishMiniGameOverlay.Configure(this.structure.Brain.Data.Bounds.x);
      GameManager.GetInstance().WaitForSeconds(0.5f, new System.Action(extinguishMiniGameOverlay.Begin));
      extinguishMiniGameOverlay.OnSuccess += (UIExtinguishMiniGameOverlay.ExtinguishEvent) (() => succeeded = true);
      extinguishMiniGameOverlay.OnFailure += (UIExtinguishMiniGameOverlay.ExtinguishEvent) (() => failed = true);
      this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    }));
    while (!succeeded)
    {
      if (failed)
      {
        GameManager.GetInstance().OnConversationEnd();
        interactionExtinguish.structure.Brain.Collapse();
        yield break;
      }
      yield return (object) null;
    }
    interactionExtinguish.loopedSound = AudioManager.Instance.CreateLoop("event:/player/watering", interactionExtinguish.gameObject, true);
    yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("oldstuff/Farming-water_track", 0, true);
    while ((double) (interactionExtinguish.WateringTime -= Time.deltaTime) >= 0.0)
      yield return (object) null;
    AudioManager.Instance.StopLoop(interactionExtinguish.loopedSound);
    interactionExtinguish.structure.Brain.SetExtinguished();
    interactionExtinguish.WateringTime = 0.95f;
    GameManager.GetInstance().OnConversationEnd();
  }
}
