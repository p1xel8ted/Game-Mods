// Decompiled with JetBrains decompiler
// Type: TrapGoop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
public class TrapGoop : BaseMonoBehaviour
{
  public Collider2D collider;
  public SpriteRenderer spriteRenderer;
  public Transform graphics;
  public float lifetime;
  public float spawnDuration = 0.25f;
  public float despawnDuration = 0.25f;
  [Space]
  [SerializeField]
  private Sprite[] sprites;
  public bool useMeshGoop;
  [SerializeField]
  private Mesh[] goopMeshes;
  public MeshRenderer meshRenderer;
  public MeshFilter meshFilter;
  private float spawnTimestamp;
  private bool isSpawning;
  private bool isDespawning;
  private static List<TrapGoop> activeGoop = new List<TrapGoop>();

  public float DamageMultiplier { get; set; } = 1f;

  public float TickDurationMultiplier { get; set; } = 1f;

  public static void CreateGoop(Vector3 position, int amount, float radius, Transform parent)
  {
    for (int index = 0; index < amount; ++index)
      Addressables.InstantiateAsync((object) "Assets/Prefabs/Enemies/Misc/Goop.prefab", position + (Vector3) Random.insideUnitCircle * radius, Quaternion.identity, parent);
  }

  private void OnEnable()
  {
    if (this.useMeshGoop)
    {
      this.spriteRenderer.enabled = false;
      if (this.goopMeshes.Length != 0)
        this.meshFilter.mesh = this.goopMeshes[Random.Range(0, this.goopMeshes.Length)];
      this.meshRenderer.gameObject.SetActive(true);
    }
    else
    {
      this.meshRenderer.gameObject.SetActive(false);
      if (this.sprites.Length != 0)
        this.spriteRenderer.sprite = this.sprites[Random.Range(0, this.sprites.Length)];
    }
    this.graphics.DOComplete();
    this.graphics.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360f));
    this.graphics.transform.localScale = Vector3.zero;
    this.graphics.transform.DOScale(1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.isSpawning = true;
    this.isDespawning = false;
    this.collider.enabled = false;
    if ((bool) (Object) GameManager.GetInstance())
      this.spawnTimestamp = GameManager.GetInstance().CurrentTime - Random.Range(0.1f, 0.3f);
    TrapGoop.activeGoop.Add(this);
  }

  private void OnDisable() => Object.Destroy((Object) this.gameObject);

  private void Update()
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
    Object.Destroy((Object) this.gameObject);
  }

  private void DespawnGoop()
  {
    TrapGoop.activeGoop.Remove(this);
    this.isDespawning = true;
    this.collider.enabled = false;
    this.graphics.DOComplete();
    this.graphics.transform.localScale = Vector3.one;
    this.graphics.transform.DOScale(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team != Health.Team.Team2)
      return;
    component.enemyPoisonDamage = 0.2f * this.DamageMultiplier;
    component.poisonTickDuration = 1f * this.TickDurationMultiplier;
    component.AddPoison(this.gameObject);
  }

  private void OnTriggerExit2D(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team != Health.Team.Team2)
      return;
    component.RemovePoison();
  }

  public static void RemoveAllGoop()
  {
    for (int index = TrapGoop.activeGoop.Count - 1; index >= 0; --index)
    {
      if ((Object) TrapGoop.activeGoop[index] != (Object) null)
        TrapGoop.activeGoop[index].DespawnGoop();
    }
  }
}
