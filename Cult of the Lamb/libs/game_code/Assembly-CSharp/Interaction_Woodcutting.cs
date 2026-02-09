// Decompiled with JetBrains decompiler
// Type: Interaction_Woodcutting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Woodcutting : TreeBase
{
  public bool RequireUseOthersFirst;
  public UIProgressIndicator _uiProgressIndicator;
  public Interaction_DigUpStump digUpStump;
  public ParticleSystem TreeHitParticles;
  public string sLabelName;
  public GameObject PlayerPositionLeft;
  public GameObject PlayerPositionRight;
  public List<PlayerFarming> activatingPlayers = new List<PlayerFarming>();
  [SpineEvent("", "", true, false, false)]
  public string chopWoodEventName = "Chop";
  public SkeletonAnimation TreeSpine;
  public GameObject disableOnCut;
  public CircleCollider2D collider;
  public LayerMask collisionMask;
  public bool harvested;
  public UnityEvent onChoppedDown;
  public float growthStageCache;
  public bool EventListenerActive;
  public bool Chopped;
  public bool[] buttonDown = new bool[2];
  public float ShowTimer;
  public bool helpedFollower;

  public override bool InactiveAfterStopMoving => false;

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
    if ((UnityEngine.Object) this._playerFarming != (UnityEngine.Object) null && (UnityEngine.Object) this._playerFarming.Spine != (UnityEngine.Object) null)
      this._playerFarming.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnDisableInteraction();
    TreeBase.Trees.Remove((TreeBase) this);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnTreeProgressChanged -= new System.Action<int>(this.OnTreeHit);
    this.StructureBrain.OnTreeComplete -= new System.Action<bool>(this.OnChoppedDown);
    this.StructureBrain.OnRegrowTree -= new System.Action(this.OnRegrowTree);
    this.StructureBrain.OnRegrowTreeProgressChanged -= new System.Action(this.SetSaplingState);
  }

  public override void OnDestroy()
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

  public void OnBrainAssigned()
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

  public void Start()
  {
    this.FreezeCoopPlayersOnHoldToInteract = false;
    this.collider = this.GetComponent<CircleCollider2D>();
    this.UpdateLocalisation();
  }

  public void CreatUI()
  {
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabelName = ScriptLocalization.Interactions.ChopWood;
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e == null || e.Data == null)
      return;
    if (e.Data.Name == this.chopWoodEventName && this.StructureBrain != null)
      this.StructureBrain.TreeHit(1f + UpgradeSystem.Chopping);
    if (!(e.Data.Name == "swipe_1") || PlayerFarming.players == null)
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!((UnityEngine.Object) player == (UnityEngine.Object) null) && !((UnityEngine.Object) player.Spine == (UnityEngine.Object) null) && player.Spine.AnimationState != null && trackEntry != null)
      {
        TrackEntry current = player.Spine.AnimationState.GetCurrent(0);
        if (current != null && trackEntry != null && (double) trackEntry.TrackTime == (double) current.TrackTime)
        {
          if ((UnityEngine.Object) player.gameObject != (UnityEngine.Object) null)
            CameraManager.shakeCamera(0.1f, Utils.GetAngle(player.gameObject.transform.position, this.transform.position));
          else
            CameraManager.shakeCamera(0.1f, 0.1f);
          GameManager instance = GameManager.GetInstance();
          if ((UnityEngine.Object) instance != (UnityEngine.Object) null)
            MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, player, coroutineSupport: (MonoBehaviour) instance);
        }
      }
    }
  }

  public override void GetLabel()
  {
    if ((!this.RequireUseOthersFirst || this.RequireUseOthersFirst && DataManager.Instance.FirstTimeChop) && !this.Chopped)
    {
      this.EventListenerActive = false;
      if (this.StructureBrain != null)
        this.Label = this.StructureBrain.TreeChopped || this.StructureBrain.Data.IsSapling ? "" : this.sLabelName;
      if (string.IsNullOrEmpty(this.label))
        return;
      this.Interactable = true;
    }
    else
      this.Label = "";
  }

  public void OnTreeHit(int followerID)
  {
    float Angle = (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null ? Utils.GetAngle(this.transform.position, this.playerFarming.gameObject.transform.position) : UnityEngine.Random.value * 360f;
    BiomeConstants.Instance.EmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, Angle);
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

  public void SetMidChopState()
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

  public void SetSaplingState()
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
    DataManager.Instance.FirstTimeChop = true;
    base.OnInteract(state);
    if ((UnityEngine.Object) this.interactorSkeletonAnimation == (UnityEngine.Object) null)
      this.interactorSkeletonAnimation = this.playerFarming.Spine;
    if ((UnityEngine.Object) this.interactorSkeletonAnimation == (UnityEngine.Object) null)
      return;
    this.activatingPlayers.Add(this._playerFarming);
    this._playerFarming.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.StartCoroutine((IEnumerator) this.DoWoodCutting(this._playerFarming));
  }

  public void OnRegrowTree()
  {
    Debug.Log((object) ("OnRegrowTree!!!! " + ((object) this.digUpStump)?.ToString()));
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
      if (this.activatingPlayers.Count > 0)
        num1 = num1 + TrinketManager.GetLootIncreaseModifier(InventoryItem.ITEM_TYPE.LOG, this.playerFarming) + UpgradeSystem.GetForageIncreaseModifier;
      int num2 = -1;
      while (++num2 < num1)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LOG, 1, this.transform.position);
    }
    if (this.activatingPlayers.Count > 0)
    {
      if (this.helpedFollower)
        CultFaithManager.AddThought(Thought.Cult_HelpFollower);
      for (int index = this.activatingPlayers.Count - 1; index >= 0; --index)
        this.EndChopping(this.activatingPlayers[index]);
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
    this.Chopped = true;
    if ((UnityEngine.Object) this.digUpStump != (UnityEngine.Object) null)
      this.digUpStump.enabled = true;
    Debug.Log((object) "REMOVE TREEEEEE");
    this.StructureBrain.Remove();
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
    if ((UnityEngine.Object) this._playerFarming != (UnityEngine.Object) null && (UnityEngine.Object) this._playerFarming.Spine != (UnityEngine.Object) null)
      this._playerFarming.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.onChoppedDown?.Invoke();
  }

  public override void Update()
  {
    base.Update();
    if (this.Structure.Structure_Info == null)
      return;
    if (this.activatingPlayers.Count > 0)
    {
      foreach (PlayerFarming activatingPlayer in this.activatingPlayers)
      {
        if (InputManager.Gameplay.GetInteractButtonUp(activatingPlayer) && SettingsManager.Settings.Accessibility.HoldActions || (UnityEngine.Object) activatingPlayer != (UnityEngine.Object) null && activatingPlayer.state.CURRENT_STATE == StateMachine.State.Meditate || !SettingsManager.Settings.Accessibility.HoldActions && InputManager.Gameplay.GetAnyButtonDownExcludingMouse(activatingPlayer) && !InputManager.Gameplay.GetInteractButtonDown(activatingPlayer))
          this.buttonDown[PlayerFarming.players.IndexOf(activatingPlayer)] = false;
      }
    }
    if (this.activatingPlayers.Count <= 0 || this.helpedFollower)
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

  public void EndChopping(PlayerFarming player)
  {
    System.Action onCrownReturn = player.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (player.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      player.state.CURRENT_STATE = StateMachine.State.Idle;
    this.activatingPlayers.Remove(player);
    player.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public void ChopTree(PlayerFarming player)
  {
    this.buttonDown[PlayerFarming.players.IndexOf(player)] = true;
    if ((double) player.gameObject.transform.position.x < (double) this.transform.position.x)
    {
      Vector3 TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (this.PlayerPositionLeft.transform.position - this.transform.position).normalized, Vector3.Distance(this.transform.position, this.PlayerPositionRight.transform.position), (int) this.collisionMask).collider != (UnityEngine.Object) null ? this.PlayerPositionRight.transform.position : this.PlayerPositionLeft.transform.position;
      player.GoToAndStop(TargetPosition, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoWoodCutting(player))));
    }
    else
    {
      Vector3 TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (this.PlayerPositionRight.transform.position - this.transform.position).normalized, Vector3.Distance(this.transform.position, this.PlayerPositionLeft.transform.position), (int) this.collisionMask).collider != (UnityEngine.Object) null ? this.PlayerPositionLeft.transform.position : this.PlayerPositionRight.transform.position;
      player.GoToAndStop(TargetPosition, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoWoodCutting(player))));
    }
  }

  public IEnumerator DoWoodCutting(PlayerFarming player)
  {
    Interaction_Woodcutting interactionWoodcutting = this;
    interactionWoodcutting.buttonDown[PlayerFarming.players.IndexOf(player)] = true;
    player.state.facingAngle = Utils.GetAngle(player.state.transform.position, interactionWoodcutting.transform.position);
    player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    player.simpleSpineAnimator.Animate("actions/chop-wood", 0, true);
    while (interactionWoodcutting.buttonDown[PlayerFarming.players.IndexOf(player)] && player.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    interactionWoodcutting.EndChopping(player);
  }

  [CompilerGenerated]
  public void \u003COnTreeHit\u003Eb__26_0()
  {
    this._uiProgressIndicator = (UIProgressIndicator) null;
  }
}
