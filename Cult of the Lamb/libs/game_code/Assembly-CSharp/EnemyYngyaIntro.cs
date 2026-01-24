// Decompiled with JetBrains decompiler
// Type: EnemyYngyaIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class EnemyYngyaIntro : MonoBehaviour
{
  public static EnemyYngyaIntro Instance;
  [SerializeField]
  public EnemyYngyaBoss yngyaBoss;
  [Header("First Intro")]
  [SerializeField]
  public GameObject introCanvas;
  [SerializeField]
  public IntroBellSequence[] bells;
  [SerializeField]
  public NoisyCircleRippleManager circleRippleManager;
  [SerializeField]
  public SkeletonAnimation yngyaVisionVersion;
  public bool skippable;
  public bool skipped;

  public void Start()
  {
    EnemyYngyaIntro.Instance = this;
    this.DisableSkippable();
    if (this.yngyaBoss.SKIP_INTRO)
    {
      this.yngyaBoss.OnStartTransforming += new UnitObject.Action(this.EnableSkippable);
      this.yngyaBoss.OnFinishingTransformation += new UnitObject.Action(this.DisableSkippable);
    }
    if (!DataManager.Instance.DiedToYngyaBoss)
      return;
    for (int index = this.bells.Length - 1; index >= 0; --index)
    {
      Object.Destroy((Object) this.bells[index].GetComponentInChildren<TriggerCanvasGroup>());
      Object.Destroy((Object) this.bells[index]);
    }
    this.introCanvas.SetActive(false);
  }

  public void OnDestroy()
  {
    this.yngyaBoss.OnStartTransforming -= new UnitObject.Action(this.EnableSkippable);
    this.yngyaBoss.OnFinishingTransformation -= new UnitObject.Action(this.DisableSkippable);
  }

  public void Update()
  {
    if (!((Object) PlayerFarming.Instance != (Object) null) || this.skipped || !this.skippable || MonoSingleton<UIManager>.Instance.MenusBlocked || !InputManager.Gameplay.GetAttackButtonDown() && !DungeonSandboxManager.Active)
      return;
    this.skipped = true;
    this.DisableSkippable();
    this.yngyaBoss.OnStartTransforming -= new UnitObject.Action(this.EnableSkippable);
    this.yngyaBoss.OnFinishingTransformation -= new UnitObject.Action(this.DisableSkippable);
    this.StopAllCoroutines();
    CameraManager.instance.Stopshake();
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    this.yngyaBoss.SkipIntro();
  }

  public void EnableSkippable()
  {
    this.skippable = true;
    LetterBox.Instance.ShowSkipPrompt();
  }

  public void DisableSkippable()
  {
    this.skippable = false;
    LetterBox.Instance.HideSkipPrompt();
  }

  public void InitPlayerWalking()
  {
    HUD_Manager.Instance.Hide(false);
    if (DataManager.Instance.DiedToYngyaBoss)
      return;
    GameManager.GetInstance().CamFollowTarget.transform.DORotate(new Vector3(-55f, 0.0f, 0.0f), 2f);
    float t = 0.0f;
    DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 2f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => GameManager.GetInstance().CamFollowTarget.TargetOffset = Vector3.Lerp(Vector3.zero, Vector3.forward * 2f, t)));
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      PlayerFarming.SetMainPlayer(player);
      PlayerFarming.Instance.unitObject.maxSpeed = 0.03f;
      PlayerFarming.Instance.playerController.RunSpeed = 2f;
      PlayerFarming.Instance.playerController.DefaultRunSpeed = 2f;
      PlayerFarming.Instance.simpleSpineAnimator.NorthIdle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-up-slow");
      PlayerFarming.Instance.simpleSpineAnimator.Idle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-slow");
      PlayerFarming.Instance.simpleSpineAnimator.DefaultLoop = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-slow");
      PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle-slow");
      PlayerFarming.Instance.simpleSpineAnimator.NorthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up-slow");
      PlayerFarming.Instance.simpleSpineAnimator.SouthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-down-slow");
      PlayerFarming.Instance.simpleSpineAnimator.NorthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up-diagonal-slow");
      PlayerFarming.Instance.simpleSpineAnimator.SouthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-slow");
      PlayerFarming.Instance.simpleSpineAnimator.ForceDirectionalMovement = true;
      PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-horizontal-slow");
      PlayerFarming.Instance.simpleSpineAnimator.UpdateIdleAndMoving();
      PlayerFarming.Instance.playerWeapon.enabled = false;
      PlayerFarming.Instance.playerSpells.enabled = false;
      PlayerFarming.Instance.AllowDodging = false;
    }
  }

  public void ResetPlayerWalking()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      PlayerFarming.SetMainPlayer(player);
      PlayerFarming.Instance.unitObject.maxSpeed = 0.09f;
      PlayerFarming.Instance.playerController.RunSpeed = 5.5f;
      PlayerFarming.Instance.playerController.DefaultRunSpeed = 5.5f;
      PlayerFarming.Instance.simpleSpineAnimator.NorthIdle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-up");
      PlayerFarming.Instance.simpleSpineAnimator.Idle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle");
      PlayerFarming.Instance.simpleSpineAnimator.DefaultLoop = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle");
      PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
      PlayerFarming.Instance.simpleSpineAnimator.SetDefault(StateMachine.State.Idle, "idle");
      PlayerFarming.Instance.simpleSpineAnimator.NorthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up");
      PlayerFarming.Instance.simpleSpineAnimator.SouthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-down");
      PlayerFarming.Instance.simpleSpineAnimator.NorthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up-diagonal");
      PlayerFarming.Instance.simpleSpineAnimator.SouthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run");
      PlayerFarming.Instance.simpleSpineAnimator.ForceDirectionalMovement = false;
      PlayerFarming.Instance.simpleSpineAnimator.SetDefault(StateMachine.State.Moving, "run-horizontal");
      PlayerFarming.Instance.simpleSpineAnimator.ResetAnimationsToDefaults();
      PlayerFarming.Instance.simpleSpineAnimator.UpdateIdleAndMoving();
      PlayerFarming.Instance.playerWeapon.enabled = true;
      PlayerFarming.Instance.playerSpells.enabled = true;
      PlayerFarming.Instance.AllowDodging = true;
    }
  }

  public void PlayRipple(Transform spawnSlot)
  {
    this.circleRippleManager.UpdateSpawnPoint(spawnSlot);
    this.circleRippleManager.EmitRipple();
  }

  public void SetYngyaVisionAnimation(string animation)
  {
    this.yngyaVisionVersion.AnimationState.SetAnimation(0, animation, true);
  }
}
