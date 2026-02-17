// Decompiled with JetBrains decompiler
// Type: CompanionBaseArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Pathfinding;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class CompanionBaseArea : MonoBehaviour
{
  public static List<CompanionBaseArea> AllCompanions = new List<CompanionBaseArea>();
  public static bool AllHide = false;
  public static bool hasGhostTwins;
  [SpineAnimation("", "", true, false)]
  public string SpawnAnimation;
  [SpineAnimation("", "", true, false)]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false)]
  public string RunAnimation;
  [SpineAnimation("", "", true, false)]
  public string RunStopAnimation;
  [SpineAnimation("", "", true, false)]
  public string DigDownAnimation;
  [SpineAnimation("", "", true, false)]
  public string DigUpAnimation;
  [Header("Movement")]
  [Tooltip("The distance at which the companion will stop when following the player or another companion.")]
  public float playerFollowDistance = 0.5f;
  public SkeletonAnimation[] Spines;
  public float flipScaleX = 1f;
  public float lastFollowTime;
  public float lastFacingUpdateTime = -1f;
  public string lastAnimName = "";
  public bool isTwinGhost;
  public bool isYngaGhost;
  public Seeker seeker;
  public Path path;
  public Transform followTarget;
  public RaycastHit[] hits = new RaycastHit[1];
  public float followDuration;
  public float currentSeekDistance = 0.5f;
  public float lastShuffleByDistanceTime;
  public int slotIndex = -1;
  public static bool EnteringFromBuilding;
  public Transform oldFollowTarget;
  public bool currentlyOnGatherPoint;
  public bool justSpawned;
  public bool shouldInstantSpawn = true;

  public static void SpawnCompanionGhosts()
  {
    foreach (Component allCompanion in CompanionBaseArea.AllCompanions)
      UnityEngine.Object.Destroy((UnityEngine.Object) allCompanion.gameObject, 0.1f);
    CompanionBaseArea.AllCompanions.Clear();
  }

  public static void SpawnGhostTwins()
  {
    if (LocationManager.LocationIsDungeon(PlayerFarming.Location))
      return;
    Addressables.InstantiateAsync((object) "Assets/Prefabs/NPC/GhostChildrenNPC/GhostChildrenTwinBad.prefab", PlayerFarming.Instance.transform.position, Quaternion.identity, PlayerFarming.Instance.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (companionLoader =>
    {
      if (companionLoader.Status == AsyncOperationStatus.Succeeded)
      {
        Debug.Log((object) "naughty ghost twin companion.");
        CompanionBaseArea component = companionLoader.Result.GetComponent<CompanionBaseArea>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.isTwinGhost = true;
      }
      else
        Debug.LogError((object) "failed to naughty ghost twin companion.");
    });
    Addressables.InstantiateAsync((object) "Assets/Prefabs/NPC/GhostChildrenNPC/GhostChildrenTwinGood.prefab", PlayerFarming.Instance.transform.position, Quaternion.identity, PlayerFarming.Instance.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (companionLoader =>
    {
      if (companionLoader.Status == AsyncOperationStatus.Succeeded)
      {
        Debug.Log((object) "good ghost twin companion.");
        CompanionBaseArea component = companionLoader.Result.GetComponent<CompanionBaseArea>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.isTwinGhost = true;
      }
      else
        Debug.LogError((object) "failed to spawn good ghost twin companion.");
    });
  }

  public static void SpawnYngaGhosts()
  {
    int itemQuantity = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST);
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    for (int index = 0; index < itemQuantity; ++index)
      Addressables.InstantiateAsync((object) "Assets/Prefabs/NPC/GhostChildrenNPC/GhostLostLamb.prefab", PlayerFarming.Instance.transform.position, Quaternion.identity, PlayerFarming.Instance.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (companionLoader =>
      {
        if (companionLoader.Status == AsyncOperationStatus.Succeeded)
        {
          Debug.Log((object) "Ynga ghost companion.");
          CompanionBaseArea component = companionLoader.Result.GetComponent<CompanionBaseArea>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          component.isYngaGhost = true;
          if (CompanionBaseArea.AllCompanions.Count < 5)
            return;
          UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
        }
        else
          Debug.LogError((object) "failed to spawn Ynga ghost companion.");
      });
  }

  public void OnEnable()
  {
    this.lastFollowTime = Time.time + 1f;
    this.seeker = this.GetComponent<Seeker>() ?? this.gameObject.AddComponent<Seeker>();
    this.transform.position = PlayerFarming.Instance.transform.position;
    this.StickToFloor();
    CompanionBaseArea.AllCompanions.Add(this);
    if (CompanionBaseArea.EnteringFromBuilding)
    {
      this.transform.position = new Vector3(0.0f, 25f, 0.0f);
      if (CompanionBaseArea.AllCompanions.Count == 4)
        CompanionBaseArea.EnteringFromBuilding = false;
    }
    CompanionBaseArea.AllCompanions = CompanionBaseArea.AllCompanions.OrderByDescending<CompanionBaseArea, bool>((Func<CompanionBaseArea, bool>) (c => c.isTwinGhost)).ToList<CompanionBaseArea>();
    this.followTarget = this.GetFollowTarget();
    if (this.currentlyOnGatherPoint)
    {
      this.transform.position = this.followTarget.transform.position;
      this.StickToFloor();
      if (this.shouldInstantSpawn)
        return;
      this.transform.localScale = Vector3.zero;
      this.justSpawned = true;
      DOVirtual.DelayedCall(1f, (TweenCallback) (() => SoulCustomTarget.Create(this.gameObject, PlayerFarming.Instance.transform.position, Color.white, (System.Action) (() =>
      {
        this.transform.DOScale(Vector3.one, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
        this.justSpawned = false;
      }), 0.2f, 2f, false, fromPool: false, sfxPath: "", collectSfxPath: "")));
    }
    else
      this.StickToFloor();
  }

  public void OnDisable()
  {
    CompanionBaseArea.AllCompanions.Remove(this);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void OnDestroy()
  {
    CompanionBaseArea.AllCompanions.Remove(this);
    if (CompanionBaseArea.AllCompanions.Count != 0)
      return;
    foreach (CritterBee critterBee in UnityEngine.Object.FindObjectsOfType<CritterBee>())
    {
      critterBee.AvoidPosition = false;
      critterBee.AvoidPositionDistance = 1f;
    }
  }

  public void Update()
  {
    if (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom && PlayerFarming.Location != FollowerLocation.Base)
    {
      CompanionBaseArea.EnteringFromBuilding = true;
      this.gameObject.SetActive(false);
    }
    else
    {
      this.followTarget = this.GetFollowTarget();
      double num1 = (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.followTarget.position);
      if ((UnityEngine.Object) this.followTarget != (UnityEngine.Object) null)
        this.seeker.StartPath(this.transform.position, this.followTarget.position, new OnPathDelegate(this.PathFound));
      this.FaceTarget(this.followTarget);
      double currentSeekDistance = (double) this.currentSeekDistance;
      if (num1 > currentSeekDistance && this.path != null && this.path.vectorPath.Count > 1)
      {
        this.followDuration += Time.deltaTime;
        double num2 = (double) Mathf.Clamp01(this.followDuration / 1f);
        float num3 = 2.5f;
        double num4 = (double) Mathf.Clamp01(Vector3.Distance(this.transform.position, this.followTarget.transform.position) / num3);
        this.transform.position = (Vector3) Vector2.MoveTowards((Vector2) this.transform.position, (Vector2) this.path.vectorPath[1], Mathf.Lerp(0.0f, 5f, (float) (num2 * num4)) * Time.deltaTime);
      }
      else
        this.followDuration = 0.0f;
      this.StickToFloor();
      if ((UnityEngine.Object) this.oldFollowTarget != (UnityEngine.Object) this.followTarget && !this.justSpawned)
      {
        this.slotIndex = -1;
        if ((double) Time.time > (double) this.lastShuffleByDistanceTime)
        {
          this.lastShuffleByDistanceTime = Time.time + 1f;
          if (this.currentlyOnGatherPoint)
          {
            this.followDuration = 0.0f;
            Vector3 targetPos = this.followTarget.transform.position;
            CompanionBaseArea.AllCompanions.Sort((Comparison<CompanionBaseArea>) ((a, b) => Vector3.Distance(a.transform.position, targetPos).CompareTo(Vector3.Distance(b.transform.position, targetPos))));
            CompanionBaseArea.AllCompanions = CompanionBaseArea.AllCompanions.OrderByDescending<CompanionBaseArea, bool>((Func<CompanionBaseArea, bool>) (c => c.isTwinGhost)).ToList<CompanionBaseArea>();
          }
        }
        if (this.currentlyOnGatherPoint && (UnityEngine.Object) CompanionBaseArea.AllCompanions[0] == (UnityEngine.Object) this)
        {
          foreach (CritterBee critterBee in UnityEngine.Object.FindObjectsOfType<CritterBee>())
          {
            critterBee.AvoidPosition = false;
            critterBee.AvoidPositionDistance = 1f;
          }
        }
        this.currentlyOnGatherPoint = false;
      }
      this.oldFollowTarget = this.followTarget;
    }
  }

  public void StickToFloor()
  {
    int mask = LayerMask.GetMask("Default");
    Vector3 vector3 = this.transform.position;
    if (UnityEngine.Physics.RaycastNonAlloc(vector3 + Vector3.back * 3f, Vector3.forward, this.hits, 10f, mask) > 0)
    {
      if ((UnityEngine.Object) (this.hits[0].collider as MeshCollider) != (UnityEngine.Object) null)
        vector3 = vector3 with { z = this.hits[0].point.z };
    }
    else
      vector3 = vector3 with { z = 0.0f };
    if (this.isTwinGhost && PlayerFarming.Location != FollowerLocation.Base && PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
      vector3.z = PlayerFarming.Instance.transform.position.z;
    this.transform.position = vector3;
  }

  public void PathFound(Path p)
  {
    if (p.error)
      return;
    this.path = p;
  }

  public void FaceTarget(Transform followTarget)
  {
    if ((double) Time.time - (double) this.lastFacingUpdateTime < 0.125)
      return;
    Vector3 vector3 = followTarget.transform.position - this.transform.position;
    float magnitude = vector3.magnitude;
    float num = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
    foreach (SkeletonAnimation spine in this.Spines)
    {
      if (this.currentlyOnGatherPoint && (double) magnitude < (double) this.playerFollowDistance + 0.20000000298023224)
      {
        if ((double) followTarget.transform.localScale.x < 0.0)
          spine.skeleton.ScaleX = 1f * this.flipScaleX;
        else
          spine.skeleton.ScaleX = -1f * this.flipScaleX;
      }
      else
        spine.skeleton.ScaleX = ((double) num > 90.0 || (double) num < -90.0 ? 1f : -1f) * this.flipScaleX;
    }
    this.lastFacingUpdateTime = Time.time;
  }

  public float PlayAnimation(string animName, bool loop, string animAfterToLoop = "")
  {
    if (animName == this.lastAnimName)
      return 0.0f;
    this.lastAnimName = animName;
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

  public IEnumerator PlayAnimationAndWait(string animName)
  {
    if (!(animName == this.lastAnimName))
    {
      this.lastAnimName = animName;
      bool finished = false;
      foreach (SkeletonAnimation spine in this.Spines)
      {
        SkeletonAnimation Spine = spine;
        if (Spine.AnimationState.SetAnimation(0, animName, false) != null)
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

  public Transform GetFollowTarget()
  {
    Vector3 position = PlayerFarming.Instance.transform.position;
    this.currentSeekDistance = this.playerFollowDistance;
    foreach (CompanionGatherPoint gatherPoint in CompanionGatherPoint.GatherPoints)
    {
      if (gatherPoint.ShouldActivateFor(position) && (gatherPoint.affectTwinGhosts && this.isTwinGhost || gatherPoint.affectYngaGhosts && this.isYngaGhost))
      {
        if (this.slotIndex == -1)
        {
          this.slotIndex = 0;
          foreach (CompanionBaseArea allCompanion in CompanionBaseArea.AllCompanions)
          {
            if (!((UnityEngine.Object) allCompanion == (UnityEngine.Object) this))
            {
              if (gatherPoint.affectTwinGhosts && allCompanion.isTwinGhost || gatherPoint.affectYngaGhosts && allCompanion.isYngaGhost)
                ++this.slotIndex;
            }
            else
              break;
          }
          this.currentSeekDistance = 0.0f;
          if (!this.currentlyOnGatherPoint && CompanionBaseArea.AllCompanions.Count > 0 && (UnityEngine.Object) CompanionBaseArea.AllCompanions[0] == (UnityEngine.Object) this)
          {
            foreach (CritterBee critterBee in UnityEngine.Object.FindObjectsOfType<CritterBee>())
            {
              critterBee.AvoidPosition = true;
              critterBee.AvoidPositionDistance = 7f;
              critterBee.AvoidPositionVector = gatherPoint.transform.position;
            }
          }
          this.currentlyOnGatherPoint = true;
          this.shouldInstantSpawn = gatherPoint.shouldInstantSpawn;
        }
        return gatherPoint.GetSlotForCompanion(this.slotIndex);
      }
    }
    int num = CompanionBaseArea.AllCompanions.IndexOf(this);
    if (num > 0 && num < CompanionBaseArea.AllCompanions.Count)
      return CompanionBaseArea.AllCompanions[num - 1].transform;
    PlayerFarming closestPlayer = PlayerFarming.GetClosestPlayer(this.transform.position);
    this.justSpawned = false;
    return !(bool) (UnityEngine.Object) closestPlayer ? (Transform) null : closestPlayer.transform;
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__23_1()
  {
    SoulCustomTarget.Create(this.gameObject, PlayerFarming.Instance.transform.position, Color.white, (System.Action) (() =>
    {
      this.transform.DOScale(Vector3.one, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.justSpawned = false;
    }), 0.2f, 2f, false, fromPool: false, sfxPath: "", collectSfxPath: "");
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__23_2()
  {
    this.transform.DOScale(Vector3.one, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.justSpawned = false;
  }
}
