// Decompiled with JetBrains decompiler
// Type: Interaction_DigUpStump
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_DigUpStump : Interaction
{
  public Interaction_Woodcutting woodcutting;
  public ParticleSystem TreeDigParticles;
  public UIProgressIndicator _uiProgressIndicator;
  public Structure _Structure;
  public Structures_Tree _StructureBrain;
  public string sLabelName;
  public bool Activated;
  public static System.Action<Interaction_DigUpStump> PlayerActivatingStart;
  public static System.Action<Interaction_DigUpStump> PlayerActivatingEnd;
  public GameObject PlayerPositionLeft;
  public GameObject PlayerPositionRight;
  [SpineEvent("", "", true, false, false)]
  public string digTreeEventName = "dig";
  public SkeletonAnimation TreeSpine;
  public float Progress;
  public float ProgressTotal = 5f;
  [SerializeField]
  public SkeletonRendererCustomMaterials _materialOverride;
  [SerializeField]
  public UnityEvent onDugUp;
  public bool buttonDown;
  public string ChoppedLabelName;
  public bool EventListenerActive;
  public bool Chopped;

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
    if ((UnityEngine.Object) this.interactorSkeletonAnimation != (UnityEngine.Object) null)
      this.interactorSkeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.buttonDown = false;
    base.OnDisableInteraction();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null))
      return;
    this._uiProgressIndicator.Recycle<UIProgressIndicator>();
    this._uiProgressIndicator = (UIProgressIndicator) null;
  }

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.ChoppedLabelName = ScriptLocalization.Interactions.DigTree;
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
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

  public void OnTreeHit()
  {
    float angle = Utils.GetAngle(this.transform.position, this.playerFarming.gameObject.transform.position);
    BiomeConstants.Instance.EmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, angle);
    CameraManager.shakeCamera(0.1f, Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position));
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
    base.OnInteract(state);
    if ((UnityEngine.Object) this.interactorSkeletonAnimation == (UnityEngine.Object) null)
      this.interactorSkeletonAnimation = this.playerFarming.Spine;
    if ((UnityEngine.Object) this.interactorSkeletonAnimation == (UnityEngine.Object) null)
      return;
    this.Activated = true;
    this.interactorSkeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoDigStump());
  }

  public override void Update()
  {
    base.Update();
    if (!this.Activated || (!InputManager.Gameplay.GetInteractButtonUp() || !SettingsManager.Settings.Accessibility.HoldActions) && (!((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null) || this.playerFarming.state.CURRENT_STATE != StateMachine.State.Meditate) && (SettingsManager.Settings.Accessibility.HoldActions || !InputManager.Gameplay.GetAnyButtonDownExcludingMouse(this.playerFarming) || InputManager.Gameplay.GetInteractButtonDown()))
      return;
    this.buttonDown = false;
  }

  public void DigStump()
  {
    if ((double) this.playerFarming.gameObject.transform.position.x < (double) this.transform.position.x)
      this.playerFarming.GoToAndStop(this.PlayerPositionLeft, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoDigStump())));
    else
      this.playerFarming.GoToAndStop(this.PlayerPositionRight, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoDigStump())));
  }

  public IEnumerator DoDigStump()
  {
    Interaction_DigUpStump interactionDigUpStump = this;
    interactionDigUpStump.buttonDown = true;
    interactionDigUpStump.state.facingAngle = Utils.GetAngle(interactionDigUpStump.state.transform.position, interactionDigUpStump.transform.position);
    interactionDigUpStump.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    interactionDigUpStump.playerFarming.simpleSpineAnimator.Animate("actions/dig", 0, true);
    while (interactionDigUpStump.buttonDown)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.8333333f);
    interactionDigUpStump.interactorSkeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionDigUpStump.HandleEvent);
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
        num1 = num1 + TrinketManager.GetLootIncreaseModifier(InventoryItem.ITEM_TYPE.LOG, this.playerFarming) + UpgradeSystem.GetForageIncreaseModifier;
      int num2 = -1;
      while (++num2 < num1)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LOG, 1, this.transform.position);
    }
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
    if (this.Activated)
      this.EndChopping();
    CameraManager.shakeCamera(1f, Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position));
    this.onDugUp?.Invoke();
    AudioManager.Instance.PlayOneShot("event:/material/tree_break", this.transform.position);
    this.woodcutting.StructureBrain.Remove();
  }

  public void EndChopping()
  {
    if (this.EventListenerActive)
      this.interactorSkeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    System.Action onCrownReturn = this.playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StopAllCoroutines();
    this.Activated = false;
    this.Interactable = true;
  }

  [CompilerGenerated]
  public void \u003COnTreeHit\u003Eb__33_0()
  {
    this._uiProgressIndicator = (UIProgressIndicator) null;
  }

  [CompilerGenerated]
  public void \u003CDigStump\u003Eb__37_0() => this.StartCoroutine((IEnumerator) this.DoDigStump());

  [CompilerGenerated]
  public void \u003CDigStump\u003Eb__37_1() => this.StartCoroutine((IEnumerator) this.DoDigStump());
}
