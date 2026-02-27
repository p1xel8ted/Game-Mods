// Decompiled with JetBrains decompiler
// Type: Interaction_Woodcutting
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
public class Interaction_Woodcutting : TreeBase
{
  public bool RequireUseOthersFirst;
  private UIProgressIndicator _uiProgressIndicator;
  public Interaction_DigUpStump digUpStump;
  public ParticleSystem TreeHitParticles;
  private string sLabelName;
  public bool Activated;
  public GameObject PlayerPositionLeft;
  public GameObject PlayerPositionRight;
  public SkeletonAnimation skeletonAnimation;
  [SpineEvent("", "", true, false, false)]
  public string chopWoodEventName = "Chop";
  public SkeletonAnimation TreeSpine;
  public GameObject disableOnCut;
  private CircleCollider2D collider;
  private LayerMask collisionMask;
  private bool harvested;
  public UnityEvent onChoppedDown;
  private float growthStageCache;
  public bool EventListenerActive;
  private bool Chopped;
  public bool buttonDown;
  private float ShowTimer;
  private bool helpedFollower;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    TreeBase.Trees.Add((TreeBase) this);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    else
      this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Obstacles"));
    if (!((UnityEngine.Object) this.TreeSpine != (UnityEngine.Object) null))
      return;
    this.TreeSpine.skeleton.SetSlotsToSetupPose();
  }

  public override void OnDisableInteraction()
  {
    if ((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null)
      this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnDisableInteraction();
    TreeBase.Trees.Remove((TreeBase) this);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnTreeProgressChanged -= new System.Action<int>(this.OnTreeHit);
    this.StructureBrain.OnTreeComplete -= new System.Action<bool>(this.OnChoppedDown);
    this.StructureBrain.OnRegrowTree -= new System.Action(this.OnRegrowTree);
    this.StructureBrain.OnRegrowTreeProgressChanged -= new System.Action(this.SetSaplingState);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
    if (this.StructureBrain == null)
      return;
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.OnTreeProgressChanged -= new System.Action<int>(this.OnTreeHit);
    this.StructureBrain.OnTreeComplete -= new System.Action<bool>(this.OnChoppedDown);
    this.StructureBrain.OnRegrowTree -= new System.Action(this.OnRegrowTree);
    this.StructureBrain.OnRegrowTreeProgressChanged -= new System.Action(this.SetSaplingState);
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain.TreeChopped || this.StructureBrain.Data.IsSapling)
    {
      this.collider = this.GetComponent<CircleCollider2D>();
      if ((UnityEngine.Object) this.collider != (UnityEngine.Object) null)
        this.collider.enabled = false;
      if ((UnityEngine.Object) this.TreeSpine != (UnityEngine.Object) null && (double) this.StructureBrain.Data.GrowthStage == 0.0)
      {
        this.TreeSpine.skeleton.SetSkin("cut");
        this.TreeSpine.skeleton.SetSlotsToSetupPose();
        this.StructureBrain.Remove();
      }
      else
      {
        this.SetSaplingState();
        this.TreeSpine.skeleton.SetSlotsToSetupPose();
      }
      if ((UnityEngine.Object) this.digUpStump != (UnityEngine.Object) null)
        this.digUpStump.enabled = true;
      this.disableOnCut.SetActive(false);
      this.Interactable = false;
      if (this.StructureBrain.TreeChopped && !this.StructureBrain.Data.IsSapling)
        this.enabled = false;
    }
    else
    {
      if ((UnityEngine.Object) this.digUpStump != (UnityEngine.Object) null)
        this.digUpStump.enabled = false;
      this.SetMidChopState();
    }
    this.StructureBrain.OnTreeProgressChanged += new System.Action<int>(this.OnTreeHit);
    this.StructureBrain.OnTreeComplete += new System.Action<bool>(this.OnChoppedDown);
    this.StructureBrain.OnRegrowTree += new System.Action(this.OnRegrowTree);
    this.StructureBrain.OnRegrowTreeProgressChanged += new System.Action(this.SetSaplingState);
    this.transform.position = this.StructureBrain.Data.Position + new Vector3(0.0f, UnityEngine.Random.Range(-0.02f, 0.02f), 0.0f);
  }

  private void Start()
  {
    this.collider = this.GetComponent<CircleCollider2D>();
    this.UpdateLocalisation();
  }

  private void CreatUI()
  {
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabelName = ScriptLocalization.Interactions.ChopWood;
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == this.chopWoodEventName && this.StructureBrain != null)
      this.StructureBrain.TreeHit(1f + UpgradeSystem.Chopping);
    if (!(e.Data.Name == "swipe_1"))
      return;
    CameraManager.shakeCamera(0.1f, Utils.GetAngle(PlayerFarming.Instance.gameObject.transform.position, this.transform.position));
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
  }

  public override void GetLabel()
  {
    if ((!this.RequireUseOthersFirst || this.RequireUseOthersFirst && DataManager.Instance.FirstTimeChop) && !this.Activated && !this.Chopped)
    {
      this.EventListenerActive = false;
      if (this.StructureBrain == null)
        return;
      this.Label = this.StructureBrain.TreeChopped || this.StructureBrain.Data.IsSapling ? "" : this.sLabelName;
    }
    else
      this.Label = "";
  }

  public void OnTreeHit(int followerID)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      float angle = Utils.GetAngle(this.transform.position, PlayerFarming.Instance.gameObject.transform.position);
      BiomeConstants.Instance.EmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, angle);
    }
    this.TreeHitParticles.Play();
    if ((UnityEngine.Object) this.TreeSpine != (UnityEngine.Object) null)
    {
      this.SetMidChopState();
      this.TreeSpine.AnimationState.SetAnimation(0, "hit", true);
      this.TreeSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    }
    AudioManager.Instance.PlayOneShot("event:/material/tree_chop", this.transform.position);
    this.TreeSpine.gameObject.transform.DORestart();
    this.TreeSpine.gameObject.transform.DOShakePosition(0.1f, 0.1f, 13, 48.8f);
    float progress = this.StructureBrain.Data.Progress / this.StructureBrain.Data.ProgressTarget;
    if ((double) progress == 0.0)
      return;
    if ((UnityEngine.Object) this._uiProgressIndicator == (UnityEngine.Object) null)
    {
      this._uiProgressIndicator = BiomeConstants.Instance.ProgressIndicatorTemplate.Spawn<UIProgressIndicator>(BiomeConstants.Instance.transform, this.transform.position + Vector3.back * 1.5f - BiomeConstants.Instance.transform.position);
      this._uiProgressIndicator.Show(progress);
      this._uiProgressIndicator.OnHidden += (System.Action) (() => this._uiProgressIndicator = (UIProgressIndicator) null);
    }
    else
      this._uiProgressIndicator.SetProgress(progress);
  }

  private void SetMidChopState()
  {
    if ((double) this.StructureBrain.Data.Progress <= 0.0)
      return;
    if ((double) this.StructureBrain.TreeHP > 5.0)
    {
      this.TreeSpine.skeleton.SetSkin("normal-chop1");
      this.TreeSpine.skeleton.SetSlotsToSetupPose();
    }
    else
    {
      this.TreeSpine.skeleton.SetSkin("normal-chop2");
      this.TreeSpine.skeleton.SetSlotsToSetupPose();
    }
  }

  private void SetSaplingState()
  {
    if ((double) this.StructureBrain.Data.GrowthStage >= 1.0 && (double) this.StructureBrain.Data.GrowthStage <= 2.0)
      this.TreeSpine.skeleton.SetSkin("sapling1");
    else if ((double) this.StructureBrain.Data.GrowthStage >= 3.0 && (double) this.StructureBrain.Data.GrowthStage <= 4.0)
      this.TreeSpine.skeleton.SetSkin("sapling2");
    else if ((double) this.StructureBrain.Data.GrowthStage >= 5.0)
      this.TreeSpine.skeleton.SetSkin("sapling3");
    if ((double) this.growthStageCache != (double) this.StructureBrain.Data.GrowthStage)
    {
      this.TreeSpine.AnimationState.SetAnimation(0, "grow", true);
      this.TreeSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    }
    this.growthStageCache = this.StructureBrain.Data.GrowthStage;
    this.TreeSpine.skeleton.SetSlotsToSetupPose();
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    DataManager.Instance.FirstTimeChop = true;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      this.skeletonAnimation = PlayerFarming.Instance.Spine;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      return;
    this.Activated = true;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnInteract(state);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoWoodCutting());
    this.Interactable = false;
  }

  public void OnRegrowTree()
  {
    Debug.Log((object) ("OnRegrowTree!!!! " + (object) this.digUpStump));
    this.enabled = true;
    if ((UnityEngine.Object) this.TreeSpine != (UnityEngine.Object) null)
    {
      this.TreeSpine.skeleton.SetSkin("normal");
      this.TreeSpine.skeleton.SetSlotsToSetupPose();
    }
    if ((UnityEngine.Object) this.digUpStump != (UnityEngine.Object) null)
      this.digUpStump.enabled = false;
    if ((UnityEngine.Object) this.digUpStump != (UnityEngine.Object) null)
      Debug.Log((object) ("digUpStump.enabled  " + this.digUpStump.enabled.ToString()));
    this.disableOnCut.SetActive(true);
    this.Interactable = true;
    this.Chopped = false;
  }

  public void OnChoppedDown(bool dropLoot)
  {
    if (this.harvested)
      return;
    this.harvested = true;
    this.TreeHitParticles.Play();
    if (dropLoot)
    {
      int num1 = this.Structure.Structure_Info.LootCountToDrop;
      if (this.Activated)
        num1 = num1 + TrinketManager.GetLootIncreaseModifier(InventoryItem.ITEM_TYPE.LOG) + UpgradeSystem.GetForageIncreaseModifier;
      int num2 = -1;
      while (++num2 < num1)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LOG, 1, this.transform.position);
    }
    if (this.Activated)
    {
      if (this.helpedFollower)
        CultFaithManager.AddThought(Thought.Cult_HelpFollower);
      this.EndChopping();
    }
    if ((UnityEngine.Object) this.collider != (UnityEngine.Object) null)
      this.collider.enabled = false;
    if ((UnityEngine.Object) this.TreeSpine != (UnityEngine.Object) null)
    {
      this.TreeSpine.skeleton.SetSkin("cut");
      this.TreeSpine.skeleton.SetSlotsToSetupPose();
    }
    AudioManager.Instance.PlayOneShot("event:/material/tree_break", this.transform.position);
    this.disableOnCut.SetActive(false);
    this.Interactable = false;
    this.Chopped = true;
    if ((UnityEngine.Object) this.digUpStump != (UnityEngine.Object) null)
      this.digUpStump.enabled = true;
    Debug.Log((object) "REMOVE TREEEEEE");
    this.StructureBrain.Remove();
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
    if ((bool) (UnityEngine.Object) this.skeletonAnimation)
      this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.onChoppedDown?.Invoke();
  }

  private new void Update()
  {
    if (this.Structure.Structure_Info == null || !this.Activated || this.helpedFollower)
      return;
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTask is FollowerTask_ChopTrees currentTask && currentTask._treeID == this.StructureInfo.ID && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
      {
        this.helpedFollower = true;
        break;
      }
    }
  }

  private void EndChopping()
  {
    System.Action onCrownReturn = PlayerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StopAllCoroutines();
    this.Activated = false;
    this.Interactable = true;
    this.HasChanged = true;
  }

  private void ChopTree()
  {
    this.buttonDown = true;
    if ((double) PlayerFarming.Instance.gameObject.transform.position.x < (double) this.transform.position.x)
    {
      Vector3 TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (this.PlayerPositionLeft.transform.position - this.transform.position).normalized, Vector3.Distance(this.transform.position, this.PlayerPositionRight.transform.position), (int) this.collisionMask).collider != (UnityEngine.Object) null ? this.PlayerPositionRight.transform.position : this.PlayerPositionLeft.transform.position;
      PlayerFarming.Instance.GoToAndStop(TargetPosition, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoWoodCutting())));
    }
    else
    {
      Vector3 TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (this.PlayerPositionRight.transform.position - this.transform.position).normalized, Vector3.Distance(this.transform.position, this.PlayerPositionLeft.transform.position), (int) this.collisionMask).collider != (UnityEngine.Object) null ? this.PlayerPositionLeft.transform.position : this.PlayerPositionRight.transform.position;
      PlayerFarming.Instance.GoToAndStop(TargetPosition, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoWoodCutting())));
    }
  }

  private IEnumerator DoWoodCutting()
  {
    Interaction_Woodcutting interactionWoodcutting = this;
    interactionWoodcutting.state.facingAngle = Utils.GetAngle(interactionWoodcutting.state.transform.position, interactionWoodcutting.transform.position);
    interactionWoodcutting.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("actions/chop-wood", 0, true);
    yield return (object) new WaitForSeconds(0.766666651f);
    while (InputManager.Gameplay.GetInteractButtonHeld() && interactionWoodcutting.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    interactionWoodcutting.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionWoodcutting.HandleEvent);
    interactionWoodcutting.EndChopping();
  }
}
