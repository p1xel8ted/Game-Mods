// Decompiled with JetBrains decompiler
// Type: CompanionCrusade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CompanionCrusade : MonoBehaviour
{
  public static List<CompanionCrusade> AllCompanions = new List<CompanionCrusade>();
  public static bool AllHide = false;
  public Action<CompanionCrusade> OnRoomClear;
  public Action<CompanionCrusade> OnRoomEnter;
  public string hideSFX;
  public string revealSFX;
  public bool isResurrecting;
  public SkeletonAnimation[] Spines;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SpawnAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RunAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RunAnimationUp;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RunAnimationDown;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RunStopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RunStopAnimationUp;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DigDownAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DigUpAnimation;
  public float flipScaleX = 1f;
  public float MoveSpeed = 5f;
  public float AccelerationTime = 0.5f;
  public float TargetSpacing = 3f;
  public float targetReachedDistance = 0.5f;
  public float EnemySightDistance = 2f;
  public float HideCheckInterval = 0.5f;
  public float MovementLockTimeAfterUnhide = 1.5f;
  public Vector2 DigDelayRange = new Vector2(0.3f, 1f);
  public bool spawnDebugSpheres;
  public GameObject debugBreadCrumbSphere;
  public bool isNowFollowing;
  public PlayerFarming player;
  public Vector3 currentTarget;
  public Vector3 lastSavedPosition;
  public float currentVelocity;
  public bool isHidden;
  public int ObstacleLayerMask;
  public float hideCheckTimer;
  public List<Vector3> trailOfTargets = new List<Vector3>();
  public Coroutine FollowPlayerRoutineRef;
  public float lastFollowTime;
  public float ignoreObstacleTime;
  public bool facingUp;
  public float lastFacingUpdateTime = -1f;
  [HideInInspector]
  public string lastAnimName;
  [HideInInspector]
  public bool lastAnimLoop;
  public GameObject savedEnabler;

  public static void RefreshParentsAndPositions(GameObject enabler)
  {
    foreach (CompanionCrusade allCompanion in CompanionCrusade.AllCompanions)
    {
      if (allCompanion.gameObject.activeSelf && (UnityEngine.Object) allCompanion.transform.parent != (UnityEngine.Object) enabler.transform.parent)
        allCompanion.RefreshParentAndPosition(enabler);
    }
  }

  public void RefreshParentAndPosition(GameObject enabler)
  {
    this.isResurrecting = false;
    this.transform.SetParent(enabler.transform.parent);
    this.transform.position = enabler.transform.position;
    this.trailOfTargets.Clear();
    this.trailOfTargets.Add(this.transform.position);
    this.lastSavedPosition = this.transform.position;
    this.savedEnabler = enabler;
    this.RestartFollow(enabler, true);
  }

  public static void ResetCompanions(bool autoHideOnEnter = false)
  {
    foreach (CompanionCrusade allCompanion in CompanionCrusade.AllCompanions)
    {
      allCompanion.isHidden = false;
      allCompanion.lastAnimName = "";
      Transform followTarget = allCompanion.GetFollowTarget();
      if ((UnityEngine.Object) followTarget != (UnityEngine.Object) null && allCompanion.isNowFollowing)
      {
        allCompanion.transform.position = followTarget.position;
        allCompanion.lastSavedPosition = followTarget.position;
        allCompanion.currentTarget = followTarget.position;
        allCompanion.trailOfTargets.Clear();
        allCompanion.trailOfTargets.Add(followTarget.position);
        allCompanion.ignoreObstacleTime = Time.time + 5f;
        Action<CompanionCrusade> onRoomEnter = allCompanion.OnRoomEnter;
        if (onRoomEnter != null)
          onRoomEnter(allCompanion);
      }
      double num = (double) allCompanion.PlayAnimation(allCompanion.IdleAnimation, true);
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && allCompanion.gameObject.activeSelf && (UnityEngine.Object) allCompanion.transform.parent != (UnityEngine.Object) PlayerFarming.Instance.gameObject.transform.parent)
        allCompanion.RefreshParentAndPosition(PlayerFarming.Instance.gameObject);
    }
  }

  public void init(GameObject enabler = null)
  {
    if (this.isNowFollowing)
      return;
    this.enabled = true;
    if ((bool) (UnityEngine.Object) enabler)
    {
      this.savedEnabler = enabler;
      this.FollowPlayerRoutineRef = this.StartCoroutine((IEnumerator) this.FollowPlayerRoutine(enabler));
    }
    this.isNowFollowing = true;
  }

  public void OnEnable()
  {
    GameManager.GetInstance().RemoveFromCamera(this.gameObject);
    this.lastFollowTime = Time.time;
    if (this.RunAnimationDown == null)
      this.RunAnimationDown = this.RunAnimation;
    if (this.RunAnimationUp != null)
      return;
    this.RunAnimationUp = this.RunAnimation;
  }

  public void OnDestroy() => CompanionCrusade.AllCompanions.Remove(this);

  public void OnDisable()
  {
  }

  public void Update()
  {
    if (this.isResurrecting)
      return;
    this.hideCheckTimer += Time.deltaTime;
    if (CompanionCrusade.AllHide)
    {
      if (Health.team2.Count == 0 && this.IsEnemyRoundsFinished())
      {
        CompanionCrusade.AllHide = false;
        Action<CompanionCrusade> onRoomClear = this.OnRoomClear;
        if (onRoomClear != null)
          onRoomClear(this);
      }
    }
    else if ((double) this.hideCheckTimer >= (double) this.HideCheckInterval)
    {
      this.hideCheckTimer = 0.0f;
      foreach (Health health in Health.team2)
      {
        if ((UnityEngine.Object) health != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, health.transform.position) <= (double) this.EnemySightDistance)
        {
          CompanionCrusade.AllHide = true;
          break;
        }
      }
    }
    if ((double) Time.time - (double) this.lastFollowTime <= 8.0)
      return;
    this.RestartFollow();
  }

  public void RestartFollow(GameObject enabler = null, bool playSpawnAnimation = false)
  {
    if (this.FollowPlayerRoutineRef != null)
      this.StopCoroutine(this.FollowPlayerRoutineRef);
    this.FollowPlayerRoutineRef = this.StartCoroutine((IEnumerator) this.FollowPlayerRoutine(enabler, playSpawnAnimation));
    this.lastFollowTime = Time.time;
  }

  public bool IsEnemyRoundsFinished()
  {
    return (UnityEngine.Object) EnemyRoundsBase.Instance == (UnityEngine.Object) null || !EnemyRoundsBase.Instance.isActiveAndEnabled || EnemyRoundsBase.Instance.Completed;
  }

  public IEnumerator FollowPlayerRoutine(GameObject enabler = null, bool playSpawnAnimation = false)
  {
    CompanionCrusade companionCrusade = this;
    yield return (object) new WaitForSeconds(0.05f);
    if (playSpawnAnimation && companionCrusade.SpawnAnimation != null)
    {
      companionCrusade.transform.localScale = Vector3.zero;
      yield return (object) new WaitForSeconds(0.5f);
      float seconds = companionCrusade.PlayAnimation(companionCrusade.SpawnAnimation, false, companionCrusade.IdleAnimation);
      companionCrusade.transform.DOScale(Vector3.one, seconds * 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad);
      yield return (object) new WaitForSeconds(seconds);
    }
    while (true)
    {
      companionCrusade.lastFollowTime = Time.time;
      if (!companionCrusade.isResurrecting)
      {
        if (!CompanionCrusade.AllCompanions.Contains(companionCrusade))
          CompanionCrusade.AllCompanions.Add(companionCrusade);
        if ((UnityEngine.Object) enabler != (UnityEngine.Object) null)
        {
          companionCrusade.transform.SetParent(enabler.transform.parent);
          companionCrusade.savedEnabler = enabler;
        }
        else if ((UnityEngine.Object) companionCrusade.savedEnabler != (UnityEngine.Object) null)
          companionCrusade.transform.SetParent(companionCrusade.savedEnabler.transform.parent);
        Transform target = companionCrusade.GetFollowTarget();
        if ((UnityEngine.Object) target != (UnityEngine.Object) null && (double) Vector3.Distance(target.position, companionCrusade.lastSavedPosition) > (double) companionCrusade.TargetSpacing)
        {
          companionCrusade.lastSavedPosition = target.position;
          companionCrusade.currentTarget = companionCrusade.lastSavedPosition;
          companionCrusade.trailOfTargets.Insert(0, new Vector3(companionCrusade.currentTarget.x, companionCrusade.currentTarget.y, companionCrusade.currentTarget.z));
          if (companionCrusade.spawnDebugSpheres && (UnityEngine.Object) companionCrusade.debugBreadCrumbSphere != (UnityEngine.Object) null)
          {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(companionCrusade.debugBreadCrumbSphere, companionCrusade.currentTarget, Quaternion.identity);
            gameObject.SetActive(true);
            UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 10f);
          }
          while (companionCrusade.trailOfTargets.Count > 30)
            companionCrusade.trailOfTargets.RemoveAt(30);
        }
        if (!CompanionCrusade.AllHide && companionCrusade.isHidden)
        {
          yield return (object) new WaitForSeconds(UnityEngine.Random.Range(companionCrusade.DigDelayRange.x, companionCrusade.DigDelayRange.y));
          yield return (object) companionCrusade.StartCoroutine((IEnumerator) companionCrusade.PlayAnimationAndWait(companionCrusade.DigUpAnimation, companionCrusade.revealSFX));
          companionCrusade.isHidden = false;
          companionCrusade.lastAnimName = "";
        }
        else if (CompanionCrusade.AllHide && !companionCrusade.isHidden)
        {
          float num1 = UnityEngine.Random.Range(companionCrusade.DigDelayRange.x, companionCrusade.DigDelayRange.y);
          double num2 = (double) companionCrusade.PlayAnimation(companionCrusade.IdleAnimation, true);
          yield return (object) new WaitForSeconds(num1 * 0.5f);
          yield return (object) companionCrusade.StartCoroutine((IEnumerator) companionCrusade.PlayAnimationAndWait(companionCrusade.DigDownAnimation, companionCrusade.hideSFX));
          companionCrusade.isHidden = true;
          companionCrusade.lastAnimName = "";
        }
        if (companionCrusade.isHidden)
        {
          yield return (object) null;
          continue;
        }
        if ((UnityEngine.Object) target == (UnityEngine.Object) null)
          target = companionCrusade.GetFollowTarget();
        if ((double) companionCrusade.ignoreObstacleTime != -1.0)
        {
          if ((double) Time.time > (double) companionCrusade.ignoreObstacleTime)
          {
            companionCrusade.ObstacleLayerMask = LayerMask.GetMask("Island");
            companionCrusade.ignoreObstacleTime = -1f;
          }
          else
            companionCrusade.ObstacleLayerMask = LayerMask.GetMask("Island");
        }
        for (int index = 0; index < companionCrusade.trailOfTargets.Count; ++index)
        {
          if (index == companionCrusade.trailOfTargets.Count - 1 || companionCrusade.HasLineOfSight(companionCrusade.transform.position, companionCrusade.trailOfTargets[index]))
          {
            companionCrusade.currentTarget = companionCrusade.trailOfTargets[index];
            companionCrusade.trailOfTargets.RemoveRange(index + 1, companionCrusade.trailOfTargets.Count - (index + 1));
            break;
          }
        }
        double num3 = (double) Vector3.Distance(companionCrusade.transform.position, companionCrusade.currentTarget);
        companionCrusade.RunAnimation = (double) companionCrusade.transform.position.y >= (double) companionCrusade.currentTarget.y - 0.0099999997764825821 || (double) Mathf.Abs(companionCrusade.transform.position.x - companionCrusade.currentTarget.x) >= 2.0 ? companionCrusade.RunAnimationDown : companionCrusade.RunAnimationUp;
        double targetReachedDistance = (double) companionCrusade.targetReachedDistance;
        if (num3 > targetReachedDistance)
        {
          double num4 = (double) companionCrusade.PlayAnimation(companionCrusade.RunAnimation, true);
          Vector3 normalized = (companionCrusade.currentTarget - companionCrusade.transform.position).normalized;
          companionCrusade.currentVelocity = Mathf.MoveTowards(companionCrusade.currentVelocity, companionCrusade.MoveSpeed, Time.deltaTime * companionCrusade.MoveSpeed / companionCrusade.AccelerationTime);
          companionCrusade.transform.position += normalized * companionCrusade.currentVelocity * Time.deltaTime;
          companionCrusade.FaceTarget(companionCrusade.currentTarget);
        }
        else
        {
          companionCrusade.currentVelocity /= 2f;
          if ((double) companionCrusade.currentVelocity < 0.20000000298023224 && (double) companionCrusade.currentVelocity != 0.0)
          {
            companionCrusade.currentVelocity = 0.0f;
            if (companionCrusade.lastAnimName == companionCrusade.RunAnimation || companionCrusade.lastAnimName == "")
            {
              double num5 = (double) companionCrusade.PlayAnimation(companionCrusade.RunStopAnimation, false, companionCrusade.IdleAnimation);
            }
            if (companionCrusade.trailOfTargets.Count > 1 && companionCrusade.currentTarget == companionCrusade.trailOfTargets[companionCrusade.trailOfTargets.Count - 1])
              companionCrusade.trailOfTargets.RemoveAt(companionCrusade.trailOfTargets.Count - 1);
          }
        }
        target = (Transform) null;
      }
      yield return (object) null;
    }
  }

  public void FaceTarget(Vector3 target)
  {
    if ((double) Time.time - (double) this.lastFacingUpdateTime < 0.10000000149011612)
      return;
    Vector3 vector3 = target - this.transform.position;
    float num = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
    foreach (SkeletonRenderer spine in this.Spines)
      spine.skeleton.ScaleX = ((double) num > 90.0 || (double) num < -90.0 ? 1f : -1f) * this.flipScaleX;
    this.lastFacingUpdateTime = Time.time;
  }

  public float PlayAnimation(string animName, bool loop, string animAfterToLoop = "")
  {
    if (animName == this.lastAnimName)
      return 0.0f;
    this.lastAnimName = animName;
    this.lastAnimLoop = loop;
    float num = 0.0f;
    foreach (SkeletonAnimation spine in this.Spines)
    {
      Spine.Animation animation = spine.Skeleton.Data.FindAnimation(animName);
      if (animation != null)
      {
        spine.AnimationState.TimeScale = 1f;
        if (animName == this.RunAnimation)
          spine.AnimationState.TimeScale = 1.5f;
        spine.AnimationState.SetAnimation(0, animName, loop);
        if (animAfterToLoop != "")
          spine.AnimationState.AddAnimation(0, animAfterToLoop, true, 0.0f);
        num = animation.Duration;
      }
    }
    return num;
  }

  public IEnumerator PlayAnimationAndWait(string animName, string sfx)
  {
    CompanionCrusade companionCrusade = this;
    if (!(animName == companionCrusade.lastAnimName))
    {
      companionCrusade.lastAnimName = animName;
      companionCrusade.lastAnimLoop = false;
      bool finished = false;
      foreach (SkeletonAnimation spine in companionCrusade.Spines)
      {
        SkeletonAnimation Spine = spine;
        TrackEntry trackEntry3 = Spine.AnimationState.SetAnimation(0, animName, false);
        if (sfx != "" && string.IsNullOrEmpty(sfx))
          AudioManager.Instance.PlayOneShot(sfx, companionCrusade.transform.position);
        if (trackEntry3 != null)
          Spine.AnimationState.Complete += (Spine.AnimationState.TrackEntryDelegate) (trackEntry1 =>
          {
            if (!(trackEntry1.Animation.Name == animName))
              return;
            finished = true;
            Spine.AnimationState.Complete -= (Spine.AnimationState.TrackEntryDelegate) (trackEntry2 =>
            {
              // ISSUE: unable to decompile the method.
            });
          });
        else
          finished = true;
      }
      yield return (object) new WaitUntil((Func<bool>) (() => finished));
    }
  }

  public bool HasLineOfSight(Vector3 from, Vector3 to)
  {
    return (UnityEngine.Object) this.player != (UnityEngine.Object) null && this.player.GoToAndStopping || (UnityEngine.Object) Physics2D.Linecast((Vector2) from, (Vector2) to, this.ObstacleLayerMask).collider == (UnityEngine.Object) null;
  }

  public Transform GetFollowTarget()
  {
    int num = CompanionCrusade.AllCompanions.IndexOf(this);
    if (num > 0 && num < CompanionCrusade.AllCompanions.Count)
      return CompanionCrusade.AllCompanions[num - 1].transform;
    PlayerFarming closestPlayer = PlayerFarming.GetClosestPlayer(this.transform.position);
    return !(bool) (UnityEngine.Object) closestPlayer ? (Transform) null : closestPlayer.transform;
  }
}
