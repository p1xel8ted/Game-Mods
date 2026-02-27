// Decompiled with JetBrains decompiler
// Type: Vomit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Vomit : Interaction
{
  public Structure structure;
  public static List<Vomit> Vomits = new List<Vomit>();
  [SerializeField]
  private Transform vomitSpriteTransform;
  private string sString;
  private SkeletonAnimation skeletonAnimation;
  private bool EventListenerActive;
  private bool playedSfx;

  public StructuresData StructureInfo => this.structure.Structure_Info;

  public StructureBrain StructureBrain => this.structure.Brain;

  private void Start() => this.UpdateLocalisation();

  public bool Activating
  {
    get => this.StructureBrain.ReservedByPlayer;
    set => this.StructureBrain.ReservedByPlayer = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Vomit.Vomits.Add(this);
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Vomit.Vomits.Remove(this);
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  private void OnBrainAssigned()
  {
    if (this.StructureInfo.Picked)
      return;
    this.StructureInfo.Picked = true;
    this.transform.localScale = Vector3.one;
    this.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
    this.vomitSpriteTransform.transform.localPosition = new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(-0.02f, 0.0f));
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.Clean;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.DoClean());
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      this.skeletonAnimation = PlayerFarming.Instance.Spine;
    if (this.EventListenerActive)
      return;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.EventListenerActive = true;
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
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
    Vomit vomit = this;
    vomit.Activating = true;
    vomit.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    Vector3 position = vomit.transform.position;
    vomit.state.facingAngle = Utils.GetAngle(vomit.state.transform.position, position);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("cleaning", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    vomit.transform.DOKill();
    // ISSUE: reference to a compiler-generated method
    vomit.transform.DOScale(0.0f, 0.15f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(vomit.\u003CDoClean\u003Eb__22_0));
  }

  public static void SpawnLoot(Vector3 position)
  {
    int num = UnityEngine.Random.Range(0, 100);
    if (num < 87)
      return;
    if (num < 90)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, position);
    else if (num < 93)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 1, position);
    else if (num < 95)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, 1, position);
    else if (num < 98)
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BERRY, 3, position);
    }
    else
    {
      if (num >= 100)
        return;
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.Necklace_4, 1, position);
    }
  }

  public override void GetLabel() => this.Label = this.sString;
}
