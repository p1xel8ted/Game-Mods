// Decompiled with JetBrains decompiler
// Type: WolfBossArm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WolfBossArm : MonoBehaviour
{
  [SerializeField]
  public WolfArmPiece root;
  [SerializeField]
  public SkeletonAnimation handSpine;
  [SerializeField]
  public bool moveTransform;
  [SerializeField]
  public Vector3 direction;
  [SerializeField]
  public Vector3 retractOffset;
  [SerializeField]
  public Vector3 slamOffset;
  [SerializeField]
  public Ease expandEase = Ease.OutSine;
  [SerializeField]
  public float expandDuration = 0.75f;
  [SerializeField]
  public float retractDuration = 0.5f;
  [SerializeField]
  public BoneFollower boneFollower;
  [SerializeField]
  public WolfArmPiece[] pieces;
  [CompilerGenerated]
  public HashSet<WolfArmPiece> \u003CPiecesFast\u003Ek__BackingField = new HashSet<WolfArmPiece>();
  public Health health;
  public Vector3[] pieceStartingLocalPositions;
  public Vector3 armStartingLocalPosition;
  public Vector3 expandedCachedPosition;
  public Vector3 expandedCachedRotation;
  public EnemyWolfBoss boss;
  [CompilerGenerated]
  public bool \u003CActive\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CAttacking\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CMoving\u003Ek__BackingField;
  public Vector3 rootTargetPosition;
  public float idleTimer = -1f;
  public string attackTentacleBiteSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_tentacle_bite";

  public BoneFollower BoneFollower => this.boneFollower;

  public WolfArmPiece[] Pieces => this.pieces;

  public HashSet<WolfArmPiece> PiecesFast
  {
    get => this.\u003CPiecesFast\u003Ek__BackingField;
    set => this.\u003CPiecesFast\u003Ek__BackingField = value;
  }

  public WolfArmPiece Root => this.root;

  public bool Active
  {
    get => this.\u003CActive\u003Ek__BackingField;
    set => this.\u003CActive\u003Ek__BackingField = value;
  }

  public bool Attacking
  {
    get => this.\u003CAttacking\u003Ek__BackingField;
    set => this.\u003CAttacking\u003Ek__BackingField = value;
  }

  public bool Moving
  {
    get => this.\u003CMoving\u003Ek__BackingField;
    set => this.\u003CMoving\u003Ek__BackingField = value;
  }

  public Vector3 RootTargetPosition => this.rootTargetPosition;

  public void Awake()
  {
    this.health = this.GetComponentInParent<Health>();
    this.boss = this.GetComponentInParent<EnemyWolfBoss>();
    this.RebuildCache();
  }

  public void Start()
  {
    this.pieceStartingLocalPositions = new Vector3[this.pieces.Length];
    for (int index = 0; index < this.pieces.Length; ++index)
    {
      if (!this.moveTransform)
        this.pieces[index].transform.localPosition = this.pieces[this.pieces.Length - 1].transform.localPosition;
      this.pieceStartingLocalPositions[index] = this.pieces[index].transform.localPosition;
      this.pieces[index].SetSpine(false);
    }
    this.armStartingLocalPosition = this.transform.position;
  }

  public void Update()
  {
    if (this.Attacking || this.Moving || !this.Active)
      return;
    if ((double) this.idleTimer == -1.0)
      this.idleTimer = 0.0f;
    this.root.transform.position = this.rootTargetPosition + this.boss.PositionOffset + new Vector3(Mathf.Sin(this.idleTimer * 0.33f), Mathf.Sin(this.idleTimer * 0.66f), Mathf.Sin(this.idleTimer * 0.5f));
    this.idleTimer += Time.deltaTime;
  }

  public void SmackAttack(
    Vector3 position,
    bool lightning,
    string startSFX,
    string startVO,
    string impactSFX,
    System.Action callback)
  {
    this.root.transform.DOKill();
    Vector3 fromPos = this.rootTargetPosition;
    Vector3 preOffset = this.root.Spine.transform.localPosition;
    AudioManager.Instance.PlayOneShot(startSFX, this.boss.gameObject);
    AudioManager.Instance.PlayOneShot(startVO, this.boss.gameObject);
    this.SmackTarget(position, lightning, impactSFX, (System.Action) (() => this.StartCoroutine((IEnumerator) this.WaitForSeconds(1f, (System.Action) (() =>
    {
      this.SetHandAnimation("animation", true);
      this.root.transform.DOLocalMove(preOffset, 0.66f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
      this.root.transform.DOMove(fromPos + this.boss.PositionOffset, 0.66f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        this.rootTargetPosition = fromPos;
        this.idleTimer = -1f;
        this.Attacking = false;
        System.Action action = callback;
        if (action == null)
          return;
        action();
      }));
      DOTween.To((DOGetter<Vector3>) (() => this.root.RotationOffset), (DOSetter<Vector3>) (x => this.root.RotationOffset = x), new Vector3(0.0f, 0.0f, (double) this.direction.x == -1.0 ? 100f : -260f), 0.66f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => { }));
    })))));
  }

  public void SweepAttack(Vector3[] targets, float duration, float delay, System.Action callback)
  {
    this.root.transform.DOKill();
    Vector3 fromPosition = this.rootTargetPosition;
    foreach (WolfArmPiece piece in this.pieces)
    {
      piece.Spine.AnimationState.SetAnimation(0, "spikes-out", false);
      piece.Spine.AnimationState.AddAnimation(0, "spikes-loop", true, 0.0f);
    }
    this.StartCoroutine((IEnumerator) this.WaitForSeconds(delay, (System.Action) (() =>
    {
      this.Attacking = true;
      foreach (WolfArmPiece piece in this.pieces)
        piece.SetDamageCollider(true);
      this.root.transform.DOPath(targets, duration, PathType.CatmullRom, gizmoColor: (Color?) new Color?()).SetEase<TweenerCore<Vector3, Path, PathOptions>>(Ease.InOutBack).OnComplete<TweenerCore<Vector3, Path, PathOptions>>((TweenCallback) (() =>
      {
        this.StartCoroutine((IEnumerator) this.WaitForSeconds(0.5f, (System.Action) (() => this.Retract(fromPosition + this.boss.PositionOffset, (System.Action) (() =>
        {
          this.rootTargetPosition = fromPosition;
          this.idleTimer = -1f;
          this.Attacking = false;
          this.boneFollower.enabled = true;
          System.Action action = callback;
          if (action == null)
            return;
          action();
        })))));
        foreach (WolfArmPiece piece in this.pieces)
        {
          piece.SetDamageCollider(false);
          piece.Spine.AnimationState.SetAnimation(0, "spikes-in", false);
          piece.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
        }
      })).OnUpdate<TweenerCore<Vector3, Path, PathOptions>>((TweenCallback) (() => this.boneFollower.enabled = false));
    })));
  }

  public void StabAttack(Vector3 target)
  {
    this.Attacking = true;
    this.SetHandAnimation("mouth_open_loop", true);
    this.SetRootDamageCollider(true);
    this.StartCoroutine((IEnumerator) this.WaitForSeconds(this.expandDuration - 0.5f, (System.Action) (() =>
    {
      CameraManager.instance.ShakeCameraForDuration(1.75f, 2f, 0.3f);
      MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.3f);
      this.SetHandAnimation("mouth_close", false, "mouth_closed_loop");
      AudioManager.Instance.PlayOneShot(this.attackTentacleBiteSFX, this.pieces[0].gameObject);
    })));
    float increment = this.expandDuration / 3f / (float) (this.pieces.Length - 3);
    float delay1 = 0.0f;
    for (int index = 0; index < this.pieces.Length; ++index)
    {
      Vector3 localScale = this.pieces[index].transform.localScale;
      this.pieces[index].transform.localScale = Vector3.zero;
      this.pieces[index].transform.DOScale(localScale, 0.5f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(delay1);
      this.pieces[index].FlashWhite(0.0f);
      delay1 += increment;
    }
    CameraManager.instance.ShakeCameraForDuration(0.75f, 1f, 0.3f);
    this.Expand(target, (System.Action) (() =>
    {
      this.SetRootDamageCollider(false);
      float delay2 = this.retractDuration - 0.15f;
      for (int index = 0; index < this.pieces.Length; ++index)
      {
        this.ShrinkAndResetPiece(this.pieces[index], 0.1f, delay2);
        delay2 -= increment;
      }
      this.Retract((System.Action) (() =>
      {
        this.Attacking = false;
        CameraManager.instance.ShakeCameraForDuration(0.75f, 1f, 0.3f);
        MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.3f);
      }), "", hidePieces: false);
    }));
  }

  public void ShrinkAndResetPiece(WolfArmPiece piece, float duration, float delay)
  {
    piece.transform.DOScale(Vector3.one * 0.3f, duration).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(delay).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      piece.transform.localScale = Vector3.one;
      piece.SetSpine(false);
    }));
  }

  public void SmackTarget(
    Vector3 target,
    bool lightningExplosion,
    string impactSFX,
    System.Action callback)
  {
    this.root.transform.DOKill();
    this.Attacking = true;
    DOTween.To((DOGetter<Vector3>) (() => this.root.RotationOffset), (DOSetter<Vector3>) (x => this.root.RotationOffset = x), new Vector3(0.0f, 0.0f, (double) this.direction.x == -1.0 ? 200f : -360f), 0.25f);
    this.root.Spine.transform.DOLocalMove(this.slamOffset, 0.25f);
    target = this.boss.transform.position + (target - this.boss.transform.position).normalized * Mathf.Clamp(Vector3.Distance(target, this.boss.transform.position), 1f, 9f);
    this.SetHandAnimation("swipe-anticipate", false);
    this.root.transform.DOMove(target + Vector3.back * 7.5f, 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.SetHandAnimation(lightningExplosion ? "swipe-down-lightning" : "swipe-down", false);
      this.root.transform.DOMove(target + Vector3.back * 0.5f + Vector3.down * 0.5f, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        Vector3 vector3 = target - this.direction + Vector3.back * 0.1f;
        BiomeConstants.Instance.EmitHammerEffectsInstantiated(vector3, cameraShakeIntensityMax: 1.5f, cameraShakeDuration: 0.5f, scale: 2f, customSFX: impactSFX);
        MMVibrate.RumbleForAllPlayers(1.2f, 1.5f, 0.5f);
        System.Action action = callback;
        if (action != null)
          action();
        if (lightningExplosion)
          LightningRingExplosion.CreateExplosion(vector3, Health.Team.Team2, this.health, 4f);
        else
          Explosion.CreateExplosion(vector3, Health.Team.Team2, this.health, 3f, playSFX: false);
        this.SetRootDamageCollider(true);
        this.StartCoroutine((IEnumerator) this.WaitForSeconds(0.2f, (System.Action) (() => this.SetRootDamageCollider(false))));
      }));
    }));
  }

  public void Expand(Vector3 position, System.Action callback, bool rotate = true, bool animate = false)
  {
    this.Active = true;
    this.Attacking = true;
    this.expandedCachedPosition = this.root.Spine.transform.localPosition;
    this.expandedCachedRotation = this.root.RotationOffset;
    this.rootTargetPosition = position;
    float expandDuration = this.expandDuration;
    float delay = 0.2f;
    if (this.moveTransform)
      this.transform.DOLocalMove(this.armStartingLocalPosition + this.direction * 10f, expandDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.root.transform.DOMove(position, expandDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.expandEase).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(delay).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => { }));
    if (rotate)
    {
      this.root.transform.DOLocalRotate(new Vector3(-160f * this.direction.x, 0.0f, 0.0f), expandDuration).SetDelay<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(delay).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.OutSine);
      DOTween.To((DOGetter<Vector3>) (() => this.root.RotationOffset), (DOSetter<Vector3>) (x => this.root.RotationOffset = x), new Vector3(0.0f, 0.0f, (double) this.direction.x == -1.0 ? 100f : -260f), expandDuration).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(delay).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        this.boneFollower.PositionOffset = this.boneFollower.transform.position - this.boneFollower.bone.GetWorldPosition(this.boss.Spine.transform);
        this.boneFollower.followXYPosition = true;
        this.boneFollower.followZPosition = true;
        System.Action action = callback;
        if (action == null)
          return;
        action();
      }));
    }
    else
      DOVirtual.DelayedCall(expandDuration + delay, (TweenCallback) (() =>
      {
        System.Action action = callback;
        if (action == null)
          return;
        action();
      }));
    this.StartCoroutine((IEnumerator) this.ExpandIE(expandDuration, animate));
  }

  public IEnumerator ExpandIE(float expandDuration, bool animate = false)
  {
    float increment = expandDuration / 3f / (float) (this.pieces.Length - 3);
    for (int i = 0; i < this.pieces.Length; ++i)
    {
      if (i >= 3 && this.moveTransform)
        yield return (object) new UnityEngine.WaitForSeconds(increment);
      this.pieces[i].SetSpine(true);
      if (animate)
        this.pieces[i].transform.DOPunchScale(Vector3.one * UnityEngine.Random.Range(0.2f, 0.3f), 1f);
    }
  }

  public void JiggleArms(float jiggleMin, float jiggleMax, float duration)
  {
    this.StartCoroutine((IEnumerator) this.JiggleArmsIE(jiggleMin, jiggleMax, duration));
  }

  public IEnumerator JiggleArmsIE(float jiggleMin, float jiggleMax, float duration)
  {
    for (int i = this.pieces.Length - 1; i >= 0; --i)
    {
      if (i != 0)
      {
        this.pieces[i].transform.DOPunchScale(Vector3.one * UnityEngine.Random.Range(jiggleMin, jiggleMax), duration);
        yield return (object) new UnityEngine.WaitForSeconds(0.1f);
      }
    }
  }

  public void Retract(Vector3 pos, System.Action callback = null)
  {
    this.Attacking = true;
    this.root.transform.DOMove(pos + this.boss.PositionOffset, 0.66f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.SetHandAnimation("animation", true);
      this.Attacking = false;
      this.idleTimer = -1f;
      System.Action action = callback;
      if (action == null)
        return;
      action();
    }));
    if ((double) this.root.RotationOffset.z == ((double) this.direction.x == -1.0 ? 100.0 : -260.0))
      return;
    DOTween.To((DOGetter<Vector3>) (() => this.root.RotationOffset), (DOSetter<Vector3>) (x => this.root.RotationOffset = x), new Vector3(0.0f, 0.0f, (double) this.direction.x == -1.0 ? 100f : -260f), 0.66f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => { }));
  }

  public void Retract(
    System.Action callback = null,
    string retractAnimation = "animation",
    string idleAnimation = "",
    bool hidePieces = true)
  {
    this.Active = false;
    this.boneFollower.PositionOffset = Vector3.zero;
    this.boneFollower.followXYPosition = false;
    this.boneFollower.followZPosition = false;
    if (!string.IsNullOrEmpty(retractAnimation))
      this.SetHandAnimation(retractAnimation, string.IsNullOrEmpty(idleAnimation), idleAnimation);
    this.root.Spine.transform.DOLocalMove(this.expandedCachedPosition, 0.25f);
    DOTween.To((DOGetter<Vector3>) (() => this.root.RotationOffset), (DOSetter<Vector3>) (x => this.root.RotationOffset = x), this.expandedCachedRotation, 0.25f);
    float dur = this.retractDuration;
    float halfDur = dur / 2f;
    for (int index = 0; index < this.pieces.Length; ++index)
    {
      if (index != 0)
        this.pieces[index].transform.DOLocalMove(this.pieceStartingLocalPositions[index], dur);
    }
    float time = 0.0f;
    Vector3 midPoint = new Vector3();
    DOTween.To((DOGetter<Vector3>) (() => this.root.RotationOffset), (DOSetter<Vector3>) (x => this.root.RotationOffset = x), new Vector3(0.0f, 0.0f, (double) this.direction.x == -1.0 ? -20f : -160f), dur).OnUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      time += Time.deltaTime;
      if ((double) time < (double) halfDur && midPoint == new Vector3())
      {
        this.pieces[0].transform.localPosition = this.pieces[1].transform.localPosition;
      }
      else
      {
        if (midPoint == new Vector3())
        {
          time = 0.0f;
          midPoint = this.pieces[0].transform.localPosition;
        }
        this.pieces[0].transform.localPosition = Vector3.Lerp(midPoint, this.pieceStartingLocalPositions[0] + this.retractOffset, time / dur);
      }
    })).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    if (this.moveTransform)
    {
      this.transform.DOLocalMove(this.armStartingLocalPosition, dur).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    }
    else
    {
      for (int index = this.pieces.Length - 1; index >= 0; --index)
        this.pieces[index].transform.DOLocalMove(this.pieceStartingLocalPositions[index], dur);
    }
    System.Action callback1 = (System.Action) (() =>
    {
      this.root.transform.localEulerAngles = new Vector3((double) this.direction.x == -1.0 ? 0.0f : -360f, 0.0f, 0.0f);
      this.pieces[0].transform.localPosition = this.pieceStartingLocalPositions[0];
      System.Action action = callback;
      if (action == null)
        return;
      action();
    });
    if (hidePieces)
      this.StartCoroutine((IEnumerator) this.RetractIE(dur / 2f, dur, callback1));
    else
      this.StartCoroutine((IEnumerator) this.WaitForSeconds(dur, callback1));
  }

  public IEnumerator RetractIE(float delay, float retractDuration, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(delay);
    float increment = retractDuration / 4f / (float) this.pieces.Length;
    for (int i = this.pieces.Length - 1; i >= 0; --i)
    {
      if (this.moveTransform)
        yield return (object) new UnityEngine.WaitForSeconds(increment);
      this.pieces[i].SetSpine(false);
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator WaitForSeconds(float duration, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(duration);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SetRootPosition(Vector3 position) => this.root.transform.position = position;

  public Vector3 GetRootPosition() => this.root.transform.position;

  public void SetRootDamageCollider(bool enabled) => this.root.SetDamageCollider(enabled);

  public void SetHandAnimation(string animation, bool looping, string afterAnim = "")
  {
    this.handSpine.AnimationState.SetAnimation(0, animation, looping);
    if (looping || string.IsNullOrEmpty(afterAnim))
      return;
    this.handSpine.AnimationState.AddAnimation(0, afterAnim, true, 0.0f);
  }

  public void SetArmsSpikey(bool spikey)
  {
    foreach (WolfArmPiece piece in this.pieces)
    {
      piece.SetDamageCollider(spikey);
      if (spikey)
      {
        piece.Spine.AnimationState.SetAnimation(0, "spikes-out", false);
        piece.Spine.AnimationState.AddAnimation(0, "spikes-loop", true, 0.0f);
      }
      else
      {
        piece.Spine.AnimationState.SetAnimation(0, "spikes-in", false);
        piece.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      }
    }
  }

  public void SetRootPosition(Vector3 position, float duration, Ease ease = Ease.InOutSine)
  {
    if (this.Moving)
      return;
    this.Moving = true;
    this.rootTargetPosition = position;
    this.root.transform.DOMove(position, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ease).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Moving = false));
  }

  public void SetRootRotation(float rotation)
  {
    this.root.transform.DOLocalRotate(new Vector3(rotation, 0.0f, 0.0f), 0.0f);
  }

  public void FlashFillRed()
  {
    for (int index = this.pieces.Length - 1; index >= 0; --index)
      this.pieces[index].FlashFillRed();
  }

  public void RebuildCache()
  {
    this.PiecesFast.Clear();
    for (int index = 0; index < this.pieces.Length; ++index)
    {
      WolfArmPiece piece = this.pieces[index];
      if ((UnityEngine.Object) piece != (UnityEngine.Object) null)
        this.PiecesFast.Add(piece);
    }
  }

  public bool Contains(WolfArmPiece p)
  {
    return (UnityEngine.Object) p != (UnityEngine.Object) null && this.PiecesFast.Contains(p);
  }

  [CompilerGenerated]
  public void \u003CSetRootPosition\u003Eb__65_0() => this.Moving = false;
}
