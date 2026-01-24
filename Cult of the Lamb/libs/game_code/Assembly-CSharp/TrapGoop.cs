// Decompiled with JetBrains decompiler
// Type: TrapGoop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class TrapGoop : BaseMonoBehaviour, ISpellOwning
{
  public Collider2D collider;
  public SpriteRenderer spriteRenderer;
  public Transform graphics;
  public float lifetime;
  public float spawnDuration = 0.25f;
  public float despawnDuration = 0.25f;
  [Space]
  [SerializeField]
  public Sprite[] sprites;
  public bool useMeshGoop;
  [SerializeField]
  public Mesh[] goopMeshes;
  public MeshRenderer meshRenderer;
  public MeshFilter meshFilter;
  [CompilerGenerated]
  public float \u003CDamageMultiplier\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public float \u003CTickDurationMultiplier\u003Ek__BackingField = 1f;
  public float spawnTimestamp;
  public bool isSpawning;
  public bool isDespawning;
  public GameObject owner;
  public static List<TrapGoop> activeGoop = new List<TrapGoop>();

  public float DamageMultiplier
  {
    get => this.\u003CDamageMultiplier\u003Ek__BackingField;
    set => this.\u003CDamageMultiplier\u003Ek__BackingField = value;
  }

  public float TickDurationMultiplier
  {
    get => this.\u003CTickDurationMultiplier\u003Ek__BackingField;
    set => this.\u003CTickDurationMultiplier\u003Ek__BackingField = value;
  }

  public static void CreateGoop(
    Vector3 position,
    int amount,
    float radius,
    GameObject owner,
    Transform parent)
  {
    for (int index = 0; index < amount; ++index)
      Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Enemies/Misc/Goop.prefab", position + (Vector3) UnityEngine.Random.insideUnitCircle * radius, Quaternion.identity, parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        if (!((UnityEngine.Object) obj.Result != (UnityEngine.Object) null))
          return;
        obj.Result.GetComponent<TrapGoop>().SetOwner(owner);
      }));
  }

  public void OnEnable()
  {
    if (this.useMeshGoop)
    {
      this.spriteRenderer.enabled = false;
      if (this.goopMeshes.Length != 0)
        this.meshFilter.mesh = this.goopMeshes[UnityEngine.Random.Range(0, this.goopMeshes.Length)];
      this.meshRenderer.gameObject.SetActive(true);
    }
    else
    {
      this.meshRenderer.gameObject.SetActive(false);
      if (this.sprites.Length != 0)
        this.spriteRenderer.sprite = this.sprites[UnityEngine.Random.Range(0, this.sprites.Length)];
    }
    this.graphics.DOComplete();
    this.graphics.transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360f));
    this.graphics.transform.localScale = Vector3.zero;
    this.graphics.transform.DOScale(1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.isSpawning = true;
    this.isDespawning = false;
    this.collider.enabled = false;
    if ((bool) (UnityEngine.Object) GameManager.GetInstance())
      this.spawnTimestamp = GameManager.GetInstance().CurrentTime - UnityEngine.Random.Range(0.1f, 0.3f);
    TrapGoop.activeGoop.Add(this);
  }

  public void OnDisable() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public void Update()
  {
    if (this.isSpawning)
    {
      if ((double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) < (double) this.spawnDuration)
        return;
      this.isSpawning = false;
      this.collider.enabled = true;
    }
    if (!this.isDespawning && (double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) >= (double) this.spawnDuration + (double) this.lifetime)
      this.DespawnGoop();
    if (!this.isDespawning || (double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) < (double) this.spawnDuration + (double) this.lifetime + (double) this.despawnDuration)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void DespawnGoop()
  {
    TrapGoop.activeGoop.Remove(this);
    this.isDespawning = true;
    this.collider.enabled = false;
    this.graphics.DOComplete();
    this.graphics.transform.localScale = Vector3.one;
    this.graphics.transform.DOScale(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team != Health.Team.Team2)
      return;
    component.enemyPoisonDamage = 0.2f * this.DamageMultiplier;
    component.poisonTickDuration = 1f * this.TickDurationMultiplier;
    component.AddPoison(this.gameObject);
  }

  public void OnTriggerExit2D(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team != Health.Team.Team2)
      return;
    component.RemovePoison();
  }

  public static void RemoveAllGoop()
  {
    for (int index = TrapGoop.activeGoop.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) TrapGoop.activeGoop[index] != (UnityEngine.Object) null)
        TrapGoop.activeGoop[index].DespawnGoop();
    }
  }

  public GameObject GetOwner() => this.owner;

  public void SetOwner(GameObject owner) => this.owner = owner;
}
