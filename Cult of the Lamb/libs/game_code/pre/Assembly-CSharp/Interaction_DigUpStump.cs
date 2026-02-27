// Decompiled with JetBrains decompiler
// Type: Interaction_DigUpStump
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_DigUpStump : Interaction
{
  public Interaction_Woodcutting woodcutting;
  public ParticleSystem TreeDigParticles;
  private UIProgressIndicator _uiProgressIndicator;
  private Structure _Structure;
  private Structures_Tree _StructureBrain;
  private string sLabelName;
  public bool Activated;
  public static System.Action<Interaction_DigUpStump> PlayerActivatingStart;
  public static System.Action<Interaction_DigUpStump> PlayerActivatingEnd;
  public GameObject PlayerPositionLeft;
  public GameObject PlayerPositionRight;
  public SkeletonAnimation skeletonAnimation;
  [SpineEvent("", "", true, false, false)]
  public string digTreeEventName = "dig";
  public SkeletonAnimation TreeSpine;
  public float Progress;
  public float ProgressTotal = 5f;
  [SerializeField]
  private SkeletonRendererCustomMaterials _materialOverride;
  [SerializeField]
  private UnityEvent onDugUp;
  private string ChoppedLabelName;
  public bool EventListenerActive;
  private bool Chopped;

  public Structure Structure
  {
    get
    {
      if ((UnityEngine.Object) this._Structure == (UnityEngine.Object) null)
        this._Structure = this.GetComponent<Structure>();
      return this._Structure;
    }
  }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Tree StructureBrain
  {
    get
    {
      if (this._StructureBrain == null && this.Structure.Brain != null)
        this._StructureBrain = this.Structure.Brain as Structures_Tree;
      return this._StructureBrain;
    }
    set => this._StructureBrain = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if (!((UnityEngine.Object) this._materialOverride != (UnityEngine.Object) null))
      return;
    this._materialOverride.enabled = true;
  }

  public override void OnDisableInteraction()
  {
    if ((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null)
      this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnDisableInteraction();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null))
      return;
    this._uiProgressIndicator.Recycle<UIProgressIndicator>();
    this._uiProgressIndicator = (UIProgressIndicator) null;
  }

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.ChoppedLabelName = ScriptLocalization.Interactions.DigTree;
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == this.digTreeEventName))
      return;
    ++this.Progress;
    this.OnTreeHit();
    if ((double) this.Progress < (double) this.ProgressTotal)
      return;
    this.OnChoppedDown(true);
  }

  public override void GetLabel() => this.Label = this.ChoppedLabelName;

  private void OnTreeHit()
  {
    float angle = Utils.GetAngle(this.transform.position, PlayerFarming.Instance.gameObject.transform.position);
    BiomeConstants.Instance.EmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, angle);
    CameraManager.shakeCamera(0.1f, Utils.GetAngle(PlayerFarming.Instance.gameObject.transform.position, this.transform.position));
    this.TreeDigParticles.Play();
    if ((UnityEngine.Object) this.TreeSpine != (UnityEngine.Object) null)
    {
      this.TreeSpine.AnimationState.SetAnimation(0, "hit", true);
      this.TreeSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    }
    AudioManager.Instance.PlayOneShot("event:/material/tree_chop", this.transform.position);
    this.TreeSpine.gameObject.transform.DORestart();
    this.TreeSpine.gameObject.transform.DOShakePosition(0.1f, 0.1f, 13, 48.8f);
    float progress = this.Progress / this.ProgressTotal;
    if (!((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this._uiProgressIndicator == (UnityEngine.Object) null)
    {
      this._uiProgressIndicator = BiomeConstants.Instance.ProgressIndicatorTemplate.Spawn<UIProgressIndicator>(BiomeConstants.Instance.transform, this.transform.position + Vector3.back * 1.5f - BiomeConstants.Instance.transform.position);
      this._uiProgressIndicator.Show(progress);
      this._uiProgressIndicator.OnHidden += (System.Action) (() => this._uiProgressIndicator = (UIProgressIndicator) null);
    }
    else
      this._uiProgressIndicator.SetProgress(progress, 0.1f);
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      this.skeletonAnimation = PlayerFarming.Instance.Spine;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      return;
    this.Activated = true;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnInteract(state);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoDigStump());
  }

  private void DigStump()
  {
    if ((double) PlayerFarming.Instance.gameObject.transform.position.x < (double) this.transform.position.x)
      PlayerFarming.Instance.GoToAndStop(this.PlayerPositionLeft, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoDigStump())));
    else
      PlayerFarming.Instance.GoToAndStop(this.PlayerPositionRight, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoDigStump())));
  }

  private IEnumerator DoDigStump()
  {
    Interaction_DigUpStump interactionDigUpStump = this;
    interactionDigUpStump.state.facingAngle = Utils.GetAngle(interactionDigUpStump.state.transform.position, interactionDigUpStump.transform.position);
    interactionDigUpStump.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("actions/dig", 0, true);
    yield return (object) new WaitForSeconds(0.8333333f);
    while (InputManager.Gameplay.GetInteractButtonHeld())
      yield return (object) null;
    interactionDigUpStump.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionDigUpStump.HandleEvent);
    interactionDigUpStump.EndChopping();
    System.Action<Interaction_DigUpStump> playerActivatingEnd = Interaction_DigUpStump.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(interactionDigUpStump);
  }

  public void OnChoppedDown(bool dropLoot)
  {
    this.TreeDigParticles.Play();
    if (dropLoot)
    {
      int num1 = 2;
      if ((double) this.StructureBrain.Data.GrowthStage >= 0.0 && (double) this.StructureBrain.Data.GrowthStage <= 2.0)
        num1 = 1;
      else if ((double) this.StructureBrain.Data.GrowthStage >= 3.0 && (double) this.StructureBrain.Data.GrowthStage <= 4.0)
        num1 = 2;
      else if ((double) this.StructureBrain.Data.GrowthStage >= 5.0)
        num1 = 3;
      if (this.Activated)
        num1 = num1 + TrinketManager.GetLootIncreaseModifier(InventoryItem.ITEM_TYPE.LOG) + UpgradeSystem.GetForageIncreaseModifier;
      int num2 = -1;
      while (++num2 < num1)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LOG, 1, this.transform.position);
    }
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
    if (this.Activated)
      this.EndChopping();
    CameraManager.shakeCamera(1f, Utils.GetAngle(PlayerFarming.Instance.gameObject.transform.position, this.transform.position));
    this.onDugUp?.Invoke();
    AudioManager.Instance.PlayOneShot("event:/material/tree_break", this.transform.position);
    this.woodcutting.StructureBrain.Remove();
  }

  private void EndChopping()
  {
    if (this.EventListenerActive)
      this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    System.Action onCrownReturn = PlayerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StopAllCoroutines();
    this.Activated = false;
    this.Interactable = true;
  }
}
