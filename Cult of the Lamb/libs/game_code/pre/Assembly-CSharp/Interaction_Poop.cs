// Decompiled with JetBrains decompiler
// Type: Interaction_Poop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Spine;
using Spine.Unity;
using src.UI.Overlays.TutorialOverlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Poop : Interaction
{
  public Structure structure;
  public static List<Interaction_Poop> Poops = new List<Interaction_Poop>();
  private float Scale;
  private float ScaleSpeed;
  private string sString;
  private bool EventListenerActive;
  private bool playedSfx;
  private SkeletonAnimation skeletonAnimation;

  public StructuresData StructureInfo => this.structure.Structure_Info;

  public StructureBrain StructureBrain => this.structure.Brain;

  public bool Activating
  {
    get => this.StructureBrain != null && this.StructureBrain.ReservedByPlayer;
    set => this.StructureBrain.ReservedByPlayer = value;
  }

  private void Start() => this.UpdateLocalisation();

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Interaction_Poop.Poops.Add(this);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_Poop.Poops.Remove(this);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (this.StructureInfo != null && this.StructureInfo.Destroyed && this.StructureInfo.LootCountToDrop != -1)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.POOP, 1, this.transform.position);
    if (this.Activating)
    {
      this.StopAllCoroutines();
      System.Action onCrownReturn = PlayerFarming.OnCrownReturn;
      if (onCrownReturn != null)
        onCrownReturn();
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    if (!((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null) || this.skeletonAnimation.AnimationState == null)
      return;
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public void Play()
  {
    this.Scale = 2f;
    this.StartCoroutine((IEnumerator) this.ScaleRoutine());
  }

  private IEnumerator ScaleRoutine()
  {
    Interaction_Poop interactionPoop = this;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < 5.0)
    {
      interactionPoop.ScaleSpeed += (float) ((1.0 - (double) interactionPoop.Scale) * 0.30000001192092896 * ((double) Time.deltaTime * 60.0));
      interactionPoop.Scale += (interactionPoop.ScaleSpeed *= 0.7f) * (Time.deltaTime * 60f);
      interactionPoop.transform.localScale = Vector3.one * interactionPoop.Scale;
      yield return (object) null;
    }
    interactionPoop.transform.localScale = Vector3.one;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.Clean;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      this.skeletonAnimation = PlayerFarming.Instance.Spine;
    if (!this.EventListenerActive)
    {
      this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
      this.EventListenerActive = true;
    }
    this.StartCoroutine((IEnumerator) this.DoClean());
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "sfxTrigger"))
      return;
    CameraManager.shakeCamera(0.05f, Utils.GetAngle(PlayerFarming.Instance.gameObject.transform.position, this.transform.position));
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.transform.DOKill();
    this.transform.DOPunchScale(Vector3.one * 0.15f, 0.25f);
    if (this.playedSfx)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/sweep", this.transform.position);
    this.playedSfx = true;
  }

  private IEnumerator DoClean()
  {
    Interaction_Poop interactionPoop = this;
    interactionPoop.playedSfx = false;
    interactionPoop.Activating = true;
    interactionPoop.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionPoop.state.facingAngle = Utils.GetAngle(interactionPoop.state.transform.position, interactionPoop.transform.position);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("cleaning", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    interactionPoop.StructureBrain.Remove();
    AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", interactionPoop.transform.position);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionPoop.transform.position);
    interactionPoop.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionPoop.HandleEvent);
    interactionPoop.EventListenerActive = false;
    System.Action onCrownReturn = PlayerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    interactionPoop.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionPoop.Activating = false;
    if (!DataManager.Instance.ShowCultIllness && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Illness))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Illness);
      overlayController.OnHide = overlayController.OnHide + new System.Action(IllnessBar.Instance.Reveal);
    }
    List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType(interactionPoop.StructureInfo.Location, StructureBrain.TYPES.POOP);
    for (int index = structuresOfType.Count - 1; index >= 0; --index)
    {
      if ((double) Vector3.Distance(structuresOfType[index].Data.Position, interactionPoop.transform.position) < 0.5)
      {
        structuresOfType[index].Data.LootCountToDrop = -1;
        structuresOfType[index].Remove();
      }
    }
  }

  public override void GetLabel() => this.Label = this.sString;
}
