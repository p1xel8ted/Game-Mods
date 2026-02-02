// Decompiled with JetBrains decompiler
// Type: LongGrass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LongGrass : BaseMonoBehaviour
{
  public List<LongGrass.GrassObject> GrassObjects = new List<LongGrass.GrassObject>();
  public List<Sprite> CutGrassSprites = new List<Sprite>();
  public List<Sprite> NonCutGrassSprites = new List<Sprite>();
  public Health health;
  public BiomeConstants.TypeOfParticle particleType = BiomeConstants.TypeOfParticle.grass;
  public int FrameIntervalOffset;
  public int UpdateInterval = 2;
  [SerializeField]
  public Material grassNormal;
  [SerializeField]
  public Material grassCut;
  [SerializeField]
  public Collider2D c;
  [SerializeField]
  public string altWalkSfx;
  [SerializeField]
  public string altDestroySfx;
  public bool TURN_OFF_ON_LOW_QUALITY;
  public Vector3 Position;
  public bool Dead;
  public float ShakeModifier = 2f;
  public List<GameObject> CurrentCollisions = new List<GameObject>();
  public List<GameObject> PreviousCollisions = new List<GameObject>();
  public Coroutine cShakeGrassRoutine;
  public float Progress;
  public float Duration;
  public List<Tween> resetTweens = new List<Tween>();
  [Range(0.0f, 1f)]
  public float v1 = 0.1f;
  [Range(0.0f, 1f)]
  public float v2 = 0.9f;

  public void OnEnable()
  {
    if (this.TURN_OFF_ON_LOW_QUALITY && SettingsManager.Settings != null && SettingsManager.Settings.Graphics.EnvironmentDetail == 0)
      this.TurnOffEverything();
    this.StartCoroutine((IEnumerator) this.OnEnableIE());
  }

  public void TurnOffEverything()
  {
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
      this.health.enabled = false;
    if ((UnityEngine.Object) this.c != (UnityEngine.Object) null)
      this.c.enabled = false;
    this.enabled = false;
  }

  public IEnumerator OnEnableIE()
  {
    LongGrass l = this;
    if ((UnityEngine.Object) l.health == (UnityEngine.Object) null)
      l.health = l.GetComponent<Health>();
    if ((UnityEngine.Object) l.health != (UnityEngine.Object) null)
      l.health.OnDie += new Health.DieAction(l.OnDie);
    l.CurrentCollisions.Clear();
    l.PreviousCollisions.Clear();
    if (l.cShakeGrassRoutine != null)
      l.StopCoroutine(l.cShakeGrassRoutine);
    yield return (object) null;
    l.Position = l.transform.position;
    TimeManager.AddToRegion(l);
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
      this.health.OnDie -= new Health.DieAction(this.OnDie);
    foreach (LongGrass.GrassObject grassObject in this.GrassObjects)
    {
      grassObject.Rotation = 0.0f;
      grassObject.RotationSpeed = 0.0f;
    }
    this.CurrentCollisions.Clear();
    this.PreviousCollisions.Clear();
    TimeManager.RemoveLongGrass(this);
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
      this.health.OnDie -= new Health.DieAction(this.OnDie);
    TimeManager.RemoveLongGrass(this);
    this.GrassObjects.Clear();
    this.CutGrassSprites.Clear();
    this.NonCutGrassSprites.Clear();
  }

  public void Start() => this.FrameIntervalOffset = UnityEngine.Random.Range(0, this.UpdateInterval);

  public void DropHeartChance()
  {
    int maxExclusive = 0;
    switch (DifficultyManager.PrimaryDifficulty)
    {
      case DifficultyManager.Difficulty.Easy:
        maxExclusive = 25;
        break;
      case DifficultyManager.Difficulty.Medium:
        maxExclusive = 40;
        break;
    }
    if (Interaction_BlueHeart.Hearts.Count + Interaction_RedHeart.Hearts.Count > 1)
      maxExclusive *= 3;
    if (maxExclusive == 0 || UnityEngine.Random.Range(0, maxExclusive) != 17)
      return;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.HALF_HEART, 1, this.transform.position);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.Dead = true;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if ((UnityEngine.Object) player != (UnityEngine.Object) null && DifficultyManager.AssistModeEnabled && (double) player.health.HP + (double) player.health.SpiritHearts + (double) player.health.BlueHearts == 1.0)
      {
        this.DropHeartChance();
        break;
      }
    }
    foreach (LongGrass.GrassObject grassObject in this.GrassObjects)
    {
      if (grassObject != null && (UnityEngine.Object) grassObject.transform != (UnityEngine.Object) null)
        grassObject.transform.eulerAngles = new Vector3(-60f, 0.0f, 0.0f);
    }
    BiomeConstants.Instance.EmitParticleChunk(this.particleType, this.transform.position, (UnityEngine.Object) Attacker != (UnityEngine.Object) null ? AttackLocation - Attacker.transform.position : Vector3.zero, 10);
    if (string.IsNullOrEmpty(this.altDestroySfx))
      AudioManager.Instance.PlayOneShot("event:/player/tall_grass_cut", this.gameObject);
    else
      AudioManager.Instance.PlayOneShot(this.altDestroySfx, this.transform.position);
    if (this.CutGrassSprites.Count > 0)
    {
      foreach (LongGrass.GrassObject grassObject in this.GrassObjects)
      {
        if (grassObject != null && (UnityEngine.Object) grassObject.spriteRederer != (UnityEngine.Object) null && this.CutGrassSprites.Count > 0)
        {
          grassObject.spriteRederer.sprite = this.CutGrassSprites[UnityEngine.Random.Range(0, this.CutGrassSprites.Count)];
          if ((UnityEngine.Object) this.grassCut != (UnityEngine.Object) null)
            grassObject.spriteRederer.material = this.grassCut;
        }
      }
    }
    else
      this.gameObject.Recycle();
    this.enabled = false;
  }

  public virtual void SetCut()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
    {
      this.health.OnDie -= new Health.DieAction(this.OnDie);
      this.health.enabled = false;
    }
    if ((UnityEngine.Object) this.c == (UnityEngine.Object) null)
      this.c = this.GetComponent<Collider2D>();
    if ((UnityEngine.Object) this.c != (UnityEngine.Object) null)
      this.c.enabled = false;
    if (this.CutGrassSprites.Count > 0)
    {
      foreach (LongGrass.GrassObject grassObject in this.GrassObjects)
      {
        if (grassObject != null && (UnityEngine.Object) grassObject.spriteRederer != (UnityEngine.Object) null && this.CutGrassSprites.Count > 0)
        {
          if ((bool) (UnityEngine.Object) grassObject.spriteRederer.GetComponent<RandomSpritePicker>())
            grassObject.spriteRederer.GetComponent<RandomSpritePicker>().enabled = false;
          grassObject.spriteRederer.sprite = this.CutGrassSprites[UnityEngine.Random.Range(0, this.CutGrassSprites.Count)];
          if ((UnityEngine.Object) this.grassCut != (UnityEngine.Object) null)
            grassObject.spriteRederer.material = this.grassCut;
        }
      }
    }
    foreach (LongGrass.GrassObject grassObject in this.GrassObjects)
    {
      if (grassObject != null && !grassObject.Equals((object) null))
      {
        if (!((UnityEngine.Object) grassObject.transform == (UnityEngine.Object) null))
        {
          try
          {
            grassObject.transform.eulerAngles = new Vector3(-60f, 0.0f, 0.0f);
          }
          catch (MissingReferenceException ex)
          {
          }
        }
      }
    }
    this.Dead = true;
    this.enabled = false;
  }

  public virtual void ResetCut()
  {
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.enabled = true;
    this.health.HP = this.health.totalHP;
    if ((UnityEngine.Object) this.c == (UnityEngine.Object) null)
      this.c = this.GetComponent<Collider2D>();
    this.c.enabled = true;
    this.Dead = false;
    if (this.NonCutGrassSprites.Count > 0)
    {
      for (int index = 0; index < this.GrassObjects.Count; ++index)
      {
        if (index < this.GrassObjects.Count - 1 && this.GrassObjects != null && (UnityEngine.Object) this.GrassObjects[index].spriteRederer != (UnityEngine.Object) null && this.NonCutGrassSprites.Count > 0)
        {
          if ((bool) (UnityEngine.Object) this.GrassObjects[index].spriteRederer.GetComponent<RandomSpritePicker>())
            this.GrassObjects[index].spriteRederer.GetComponent<RandomSpritePicker>().enabled = true;
          this.GrassObjects[index].spriteRederer.sprite = this.NonCutGrassSprites[UnityEngine.Random.Range(0, this.NonCutGrassSprites.Count)];
          if ((UnityEngine.Object) this.grassNormal != (UnityEngine.Object) null)
            this.GrassObjects[index].spriteRederer.material = this.grassNormal;
        }
      }
    }
    this.enabled = true;
  }

  public IEnumerator EmitGrassParticles(GameObject Attacker)
  {
    Vector3 position = Attacker.transform.position;
    Vector3 LastPos = position;
    yield return (object) null;
    if (!((UnityEngine.Object) Attacker != (UnityEngine.Object) null))
    {
      Vector3 zero = Vector3.zero;
    }
    else
    {
      Vector3 vector3 = (position - LastPos) * 50f;
    }
  }

  public void Colliding(GameObject g)
  {
    if (this.CurrentCollisions.Contains(g))
      return;
    this.CurrentCollisions.Add(g);
  }

  public void LateUpdate()
  {
    if (PerformanceTest.ReduceCPU || this.TURN_OFF_ON_LOW_QUALITY || (Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval != 0)
      return;
    foreach (GameObject currentCollision in this.CurrentCollisions)
    {
      if (!this.PreviousCollisions.Contains(currentCollision) && (UnityEngine.Object) currentCollision != (UnityEngine.Object) null)
        this.CollisionEnter(currentCollision);
    }
    foreach (GameObject previousCollision in this.PreviousCollisions)
    {
      if (!this.CurrentCollisions.Contains(previousCollision) && (UnityEngine.Object) previousCollision != (UnityEngine.Object) null)
        this.CollisionExit(previousCollision);
    }
    List<GameObject> previousCollisions = this.PreviousCollisions;
    this.PreviousCollisions = this.CurrentCollisions;
    this.CurrentCollisions = previousCollisions;
    this.CurrentCollisions.Clear();
  }

  public void CollisionEnter(GameObject collision)
  {
    if (this.Dead)
      return;
    if (string.IsNullOrEmpty(this.altWalkSfx))
    {
      AudioManager.Instance.PlayOneShot("event:/material/footstep_bush", collision.transform.position);
      AudioManager.Instance.PlayOneShot("event:/player/tall_grass_push", collision.transform.position);
    }
    else
      AudioManager.Instance.PlayOneShot(this.altWalkSfx, collision.transform.position);
    if (this.cShakeGrassRoutine != null)
      this.StopCoroutine(this.cShakeGrassRoutine);
    this.cShakeGrassRoutine = this.StartCoroutine((IEnumerator) this.ShakeGrassRoutine(collision));
  }

  public void CollisionExit(GameObject collision)
  {
    if (this.Dead)
      return;
    if (this.cShakeGrassRoutine != null)
      this.StopCoroutine(this.cShakeGrassRoutine);
    this.cShakeGrassRoutine = this.StartCoroutine((IEnumerator) this.ShakeGrassRoutine(collision));
  }

  public IEnumerator ShakeGrassRoutine(GameObject collision, float clampDistance = -1f)
  {
    if (!((UnityEngine.Object) collision == (UnityEngine.Object) null))
    {
      foreach (LongGrass.GrassObject grassObject in this.GrassObjects)
      {
        if (grassObject != null && (UnityEngine.Object) grassObject.transform != (UnityEngine.Object) null)
        {
          Vector3 position1 = grassObject.transform.position;
          Vector3 position2 = collision.transform.position;
          float num = (double) clampDistance == -1.0 ? Vector3.Distance(position1, position2) : Mathf.Clamp(Vector3.Distance(position1, position2), 0.0f, clampDistance);
          grassObject.RotationSpeed += (float) ((10.0 + (double) UnityEngine.Random.Range(-2, 2)) * ((double) position1.x < (double) position2.x ? -1.0 : 1.0) * (double) this.ShakeModifier * (1.0 - (double) num / 1.0));
        }
      }
      this.Progress = 0.0f;
      this.Duration = 1f;
      while ((double) (this.Progress += Time.fixedDeltaTime) < (double) this.Duration)
      {
        if (this.Dead)
          yield break;
        foreach (LongGrass.GrassObject grassObject in this.GrassObjects)
        {
          if ((double) Time.deltaTime > 0.0 && grassObject != null && (UnityEngine.Object) grassObject.transform != (UnityEngine.Object) null)
          {
            grassObject.RotationSpeed += (float) ((0.0 - (double) grassObject.Rotation) * (double) this.v1 / ((double) Time.fixedDeltaTime * 60.0));
            grassObject.Rotation += (grassObject.RotationSpeed *= this.v2) * (Time.fixedDeltaTime * 60f);
            grassObject.transform.eulerAngles = new Vector3(-60f, 0.0f, grassObject.Rotation);
          }
        }
        yield return (object) new WaitForFixedUpdate();
      }
      foreach (Tween resetTween in this.resetTweens)
      {
        if (resetTween != null && resetTween.active)
          resetTween.Complete();
      }
      this.resetTweens.Clear();
      foreach (LongGrass.GrassObject grassObject in this.GrassObjects)
      {
        if ((double) Mathf.Abs(grassObject.Rotation) > 0.20000000298023224)
        {
          grassObject.RotationSpeed = 0.0f;
          grassObject.Rotation = 0.0f;
          this.resetTweens.Add((Tween) grassObject.transform.DORotate(new Vector3(-60f, 0.0f, 0.0f), UnityEngine.Random.Range(0.0f, 1f)).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.OutBounce));
        }
      }
    }
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.CompareTag("Projectile"))
      return;
    this.Colliding(collision.gameObject);
  }

  [Serializable]
  public class GrassObject
  {
    public Transform transform;
    public float Rotation;
    public float RotationSpeed;
    public SpriteRenderer spriteRederer;
  }
}
