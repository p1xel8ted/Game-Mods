// Decompiled with JetBrains decompiler
// Type: GhostLostLambManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class GhostLostLambManager : MonoBehaviour
{
  public List<GameObject> GhostLambsToSpawn;
  public List<GameObject> GhostLambTargets;
  public Animator animator;
  public bool testSetBaseSpawn;
  public int ghostsCount;
  public string currentAnim = "CircleStartPoint";
  public string targetAnim = "CircleStartPoint";
  public float smoothTime = 1f;
  public Vector3[] ghostSmoothing;
  public float maxSpeed = 0.25f;

  public void OnEnable()
  {
    foreach (GameObject gameObject in this.GhostLambsToSpawn)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
        gameObject.SetActive(false);
    }
    this.animator.enabled = false;
    if (this.testSetBaseSpawn)
    {
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST, 4);
      DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AppearInBase;
      this.testSetBaseSpawn = false;
    }
    this.ghostsCount = Mathf.Min(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST), this.GhostLambsToSpawn.Count);
    if (this.ghostsCount == 0 && DataManager.Instance.ghostLostLambState != DataManager.GhostLostLambState.DoNotAppear)
      DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.DoNotAppear;
    else
      DOVirtual.DelayedCall(1f, (TweenCallback) (() =>
      {
        if (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
        {
          if (this.ghostsCount > 0 && DataManager.Instance.ghostLostLambState == DataManager.GhostLostLambState.DoNotAppear)
            DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AppearInBase;
          if (DataManager.Instance.ghostLostLambState != DataManager.GhostLostLambState.AppearInBase)
            return;
          this.animator.enabled = true;
          this.animator.speed = 1f;
          this.StartCoroutine((IEnumerator) this.SpawnAnimatedGhosts());
        }
        else
        {
          if (this.ghostsCount > 0 && DataManager.Instance.ghostLostLambState == DataManager.GhostLostLambState.DoNotAppear)
            DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AlreadyInLambTown;
          if (DataManager.Instance.ghostLostLambState == DataManager.GhostLostLambState.AppearInBase)
            DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AppearInLambTown;
          int num = 0;
          if (DataManager.Instance.ghostLostLambState == DataManager.GhostLostLambState.AppearInLambTown)
          {
            foreach (GameObject gameObject in this.GhostLambsToSpawn)
            {
              if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
              {
                if (num < this.ghostsCount && (UnityEngine.Object) gameObject != (UnityEngine.Object) null)
                {
                  gameObject.SetActive(true);
                  Transform child = gameObject.transform.GetChild(0);
                  child.transform.localScale = Vector3.zero;
                  child.transform.DOScale(Vector3.one, 0.125f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.01f);
                }
                ++num;
              }
            }
            this.animator.enabled = true;
            DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AlreadyInLambTown;
          }
          else
          {
            if (DataManager.Instance.ghostLostLambState != DataManager.GhostLostLambState.AlreadyInLambTown)
              return;
            foreach (GameObject gameObject in this.GhostLambsToSpawn)
            {
              if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
              {
                if (num < this.ghostsCount && (UnityEngine.Object) gameObject != (UnityEngine.Object) null)
                {
                  gameObject.SetActive(true);
                  Transform child = gameObject.transform.GetChild(0);
                  child.transform.localScale = Vector3.zero;
                  child.transform.DOScale(Vector3.one, 0.125f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.01f);
                }
                ++num;
              }
            }
            this.animator.enabled = true;
            this.animator.Play(this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 1f);
            this.animator.Update(0.0f);
          }
        }
      }));
  }

  public void MoveGhostsToTarget(bool init = false)
  {
    if (this.ghostSmoothing == null)
      this.ghostSmoothing = new Vector3[this.GhostLambsToSpawn.Count];
    for (int index = 0; index < this.GhostLambsToSpawn.Count; ++index)
    {
      GameObject gameObject = this.GhostLambsToSpawn[index];
      GameObject ghostLambTarget = index < this.GhostLambTargets.Count ? this.GhostLambTargets[index] : (GameObject) null;
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null) && !((UnityEngine.Object) ghostLambTarget == (UnityEngine.Object) null))
      {
        if (init)
        {
          gameObject.transform.position = ghostLambTarget.transform.position;
          this.ghostSmoothing[index] = Vector3.zero;
        }
        else
        {
          Vector3 position = gameObject.transform.position;
          Vector3 vector3_1 = Vector3.SmoothDamp(position, ghostLambTarget.transform.position, ref this.ghostSmoothing[index], this.smoothTime, float.PositiveInfinity, Time.deltaTime);
          Vector3 vector3_2 = vector3_1 - position;
          if ((double) vector3_2.magnitude > (double) this.maxSpeed)
          {
            vector3_2 = vector3_2.normalized * this.maxSpeed;
            vector3_1 = position + vector3_2;
          }
          gameObject.transform.position = vector3_1;
        }
      }
    }
  }

  public void Update()
  {
    if (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
      return;
    this.FlipGhostsToFacePlayer(PlayerFarming.Instance.transform.position, -1f);
  }

  public void FlipGhostsToFacePlayer(Vector3 targetPosition, float invertMultiplier = 1f)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    Vector3 vector3 = targetPosition;
    foreach (GameObject gameObject in this.GhostLambsToSpawn)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        Transform t = gameObject.transform;
        Vector3 position = t.position;
        Vector3 localScale = t.localScale;
        float x = localScale.x;
        float f = ((double) vector3.x > (double) position.x ? Mathf.Abs(x) : -Mathf.Abs(x)) * invertMultiplier;
        if ((double) Mathf.Sign(x) != (double) Mathf.Sign(f))
        {
          localScale.x = f;
          t.localScale = localScale;
          t.DOKill();
          t.DOScaleY(1.25f, 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => t.DOScaleY(1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBounce)));
        }
        else
        {
          localScale.x = f;
          t.localScale = localScale;
        }
      }
    }
  }

  public IEnumerator SpawnAnimatedGhosts()
  {
    GhostLostLambManager ghostLostLambManager = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    int ghostsSpawned = 0;
    ghostLostLambManager.MoveGhostsToTarget(true);
    float startDelay = 2.5f;
    while ((double) startDelay > 0.0)
    {
      ghostLostLambManager.MoveGhostsToTarget();
      startDelay -= Time.deltaTime;
      yield return (object) null;
    }
    foreach (GameObject gameObject in ghostLostLambManager.GhostLambsToSpawn)
    {
      GameObject ghost = gameObject;
      if (!((UnityEngine.Object) ghost == (UnityEngine.Object) null))
      {
        if (ghostsSpawned < ghostLostLambManager.ghostsCount)
        {
          SoulCustomTarget.Create(ghost.transform.position, PlayerFarming.Instance.transform.position, Color.white, (System.Action) (() =>
          {
            if (!((UnityEngine.Object) ghost != (UnityEngine.Object) null))
              return;
            ghost.SetActive(true);
            BiomeConstants.Instance.EmitHitVFXSoul(ghost.transform.position, Quaternion.identity);
            Transform child = ghost.transform.GetChild(0);
            child.transform.localScale = Vector3.zero;
            Vector3 one = Vector3.one;
            one.x *= -1f;
            child.transform.DOScale(one, 0.66f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
          }), 0.2f, 8f, fromPool: false);
          float spawnDelay = 0.33f;
          while ((double) spawnDelay > 0.0)
          {
            ghostLostLambManager.MoveGhostsToTarget();
            spawnDelay -= Time.deltaTime;
            yield return (object) null;
          }
        }
        ++ghostsSpawned;
      }
    }
    while (true)
    {
      ghostLostLambManager.MoveGhostsToTarget();
      Animator component = ghostLostLambManager.GetComponent<Animator>();
      if (DataManager.Instance.ghostLostLambState == DataManager.GhostLostLambState.AppearInLambTown)
      {
        Vector3 targetPosition = new Vector3(1000f, 0.0f, 0.0f);
        foreach (GameObject gameObject in ghostLostLambManager.GhostLambsToSpawn)
        {
          if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null) && (double) gameObject.transform.position.y > 12.0)
          {
            targetPosition = PlayerFarming.Instance.transform.position;
            break;
          }
        }
        ghostLostLambManager.FlipGhostsToFacePlayer(targetPosition);
      }
      else if (ghostLostLambManager.currentAnim == "CircleStartPoint")
      {
        ghostLostLambManager.FlipGhostsToFacePlayer(PlayerFarming.Instance.transform.position);
        if ((double) PlayerFarming.Instance.transform.position.y < 2.25)
        {
          ghostLostLambManager.targetAnim = "CircleToLurk";
          if (ghostLostLambManager.currentAnim != ghostLostLambManager.targetAnim)
          {
            ghostLostLambManager.currentAnim = ghostLostLambManager.targetAnim;
            component.CrossFade(ghostLostLambManager.targetAnim, 0.0f, 0, 0.0f);
            ghostLostLambManager.maxSpeed = 0.2f;
          }
        }
        else if ((double) PlayerFarming.Instance.transform.position.x > (double) ghostLostLambManager.transform.position.x + 3.25)
        {
          component.CrossFade("CircleToWoolhaven", 0.0f, 0, 0.0f);
          DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AppearInLambTown;
          ghostLostLambManager.maxSpeed = 1f;
        }
      }
      else if (ghostLostLambManager.currentAnim == "CircleToLurk")
      {
        ghostLostLambManager.FlipGhostsToFacePlayer(PlayerFarming.Instance.transform.position);
        if ((double) PlayerFarming.Instance.transform.position.y > 2.25)
        {
          ghostLostLambManager.targetAnim = "CircleStartPoint";
          if (ghostLostLambManager.currentAnim != ghostLostLambManager.targetAnim)
          {
            ghostLostLambManager.currentAnim = ghostLostLambManager.targetAnim;
            component.CrossFade(ghostLostLambManager.targetAnim, 0.0f, 0, 0.0f);
            ghostLostLambManager.maxSpeed = 0.15f;
          }
        }
      }
      yield return (object) null;
    }
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__5_0()
  {
    if (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
    {
      if (this.ghostsCount > 0 && DataManager.Instance.ghostLostLambState == DataManager.GhostLostLambState.DoNotAppear)
        DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AppearInBase;
      if (DataManager.Instance.ghostLostLambState != DataManager.GhostLostLambState.AppearInBase)
        return;
      this.animator.enabled = true;
      this.animator.speed = 1f;
      this.StartCoroutine((IEnumerator) this.SpawnAnimatedGhosts());
    }
    else
    {
      if (this.ghostsCount > 0 && DataManager.Instance.ghostLostLambState == DataManager.GhostLostLambState.DoNotAppear)
        DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AlreadyInLambTown;
      if (DataManager.Instance.ghostLostLambState == DataManager.GhostLostLambState.AppearInBase)
        DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AppearInLambTown;
      int num = 0;
      if (DataManager.Instance.ghostLostLambState == DataManager.GhostLostLambState.AppearInLambTown)
      {
        foreach (GameObject gameObject in this.GhostLambsToSpawn)
        {
          if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
          {
            if (num < this.ghostsCount && (UnityEngine.Object) gameObject != (UnityEngine.Object) null)
            {
              gameObject.SetActive(true);
              Transform child = gameObject.transform.GetChild(0);
              child.transform.localScale = Vector3.zero;
              child.transform.DOScale(Vector3.one, 0.125f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.01f);
            }
            ++num;
          }
        }
        this.animator.enabled = true;
        DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AlreadyInLambTown;
      }
      else
      {
        if (DataManager.Instance.ghostLostLambState != DataManager.GhostLostLambState.AlreadyInLambTown)
          return;
        foreach (GameObject gameObject in this.GhostLambsToSpawn)
        {
          if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
          {
            if (num < this.ghostsCount && (UnityEngine.Object) gameObject != (UnityEngine.Object) null)
            {
              gameObject.SetActive(true);
              Transform child = gameObject.transform.GetChild(0);
              child.transform.localScale = Vector3.zero;
              child.transform.DOScale(Vector3.one, 0.125f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.01f);
            }
            ++num;
          }
        }
        this.animator.enabled = true;
        this.animator.Play(this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 1f);
        this.animator.Update(0.0f);
      }
    }
  }
}
