// Decompiled with JetBrains decompiler
// Type: TrapSpikesSpawnOthers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TrapSpikesSpawnOthers : BaseMonoBehaviour, ISpellOwning
{
  [SerializeField]
  public SpriteRenderer spriteRenderer;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public GameObject particlesParent;
  [SerializeField]
  public GameObject[] particles;
  [SerializeField]
  public string attackSFX = "event:/boss/deathcat/chain_spawner";
  public List<Vector3> cachedParticlePositions = new List<Vector3>();
  public BoxCollider2D boxCollider2D;
  public List<Collider2D> colliders;
  public ContactFilter2D contactFilter2D;
  public Vector2 ScaleX = Vector2.zero;
  public Vector2 ScaleY = Vector2.zero;
  public Health EnemyHealth;
  public Health Origin;
  [CompilerGenerated]
  public Color \u003COverrideColor\u003Ek__BackingField = Color.white;
  public string defaultSkinName;
  public Color defaultOverrideColor = Color.white;
  public static Collider2D[] _hitBuffer = new Collider2D[16 /*0x10*/];

  public SkeletonAnimation Spine => this.spine;

  public Color OverrideColor
  {
    get => this.\u003COverrideColor\u003Ek__BackingField;
    set => this.\u003COverrideColor\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    this.defaultSkinName = this.spine.Skeleton.Skin.Name;
    this.defaultOverrideColor = this.OverrideColor;
    for (int index = 0; index < this.particles.Length; ++index)
      this.cachedParticlePositions.Add(this.particles[index].transform.localPosition);
  }

  public void Start()
  {
    this.boxCollider2D = this.GetComponent<BoxCollider2D>();
    this.contactFilter2D = new ContactFilter2D();
    this.contactFilter2D.NoFilter();
  }

  public void OnEnable()
  {
    this.ScaleX = Vector2.zero;
    this.ScaleY = Vector2.zero;
    this.spine.gameObject.SetActive(false);
    this.StartCoroutine(this.DoAttack());
    this.StartCoroutine(this.DoScale());
  }

  public void OnDisable() => this.ResetToDefaultSettings();

  public void OnDestroy()
  {
    for (int index = 0; index < this.particles.Length; ++index)
    {
      if ((Object) this.particles[index] != (Object) null)
        Object.Destroy((Object) this.particles[index].gameObject);
    }
  }

  public void ResetToDefaultSettings()
  {
    this.Spine.Skeleton.SetSkin(this.defaultSkinName);
    this.OverrideColor = this.defaultOverrideColor;
  }

  public IEnumerator DoScale()
  {
    TrapSpikesSpawnOthers spikesSpawnOthers = this;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime * spikesSpawnOthers.Spine.timeScale) < 5.0)
    {
      spikesSpawnOthers.ScaleX.y += (float) ((1.0 - (double) spikesSpawnOthers.ScaleX.x) * 0.30000001192092896);
      spikesSpawnOthers.ScaleX.x += (spikesSpawnOthers.ScaleX.y *= 0.7f);
      spikesSpawnOthers.ScaleY.y += (float) ((1.0 - (double) spikesSpawnOthers.ScaleY.x) * 0.30000001192092896);
      spikesSpawnOthers.ScaleY.x += (spikesSpawnOthers.ScaleY.y *= 0.7f);
      spikesSpawnOthers.transform.localScale = new Vector3(spikesSpawnOthers.ScaleX.x, spikesSpawnOthers.ScaleY.x, 1f);
      yield return (object) null;
    }
  }

  public IEnumerator DoAttack()
  {
    TrapSpikesSpawnOthers spikesSpawnOthers = this;
    float progress = 0.0f;
    if ((bool) (Object) spikesSpawnOthers.particlesParent)
    {
      foreach (GameObject particle in spikesSpawnOthers.particles)
      {
        if ((Object) particle != (Object) null)
          particle.transform.parent = spikesSpawnOthers.particlesParent.transform;
      }
    }
    yield return (object) new WaitForEndOfFrame();
    for (int index = 0; index < spikesSpawnOthers.particles.Length; ++index)
    {
      if ((Object) spikesSpawnOthers.particles[index] != (Object) null)
      {
        spikesSpawnOthers.particles[index].transform.localPosition = spikesSpawnOthers.cachedParticlePositions[index];
        spikesSpawnOthers.particles[index].gameObject.SetActive(true);
        spikesSpawnOthers.particles[index].transform.parent = (Transform) null;
        spikesSpawnOthers.particles[index].transform.localScale = Vector3.one;
      }
    }
    if (!string.IsNullOrEmpty(spikesSpawnOthers.attackSFX))
      AudioManager.Instance.PlayOneShot(spikesSpawnOthers.attackSFX, spikesSpawnOthers.transform.position);
    spikesSpawnOthers.spriteRenderer.color = spikesSpawnOthers.OverrideColor != Color.white ? spikesSpawnOthers.OverrideColor : Color.white;
    while ((double) progress < 0.25)
    {
      progress += Time.deltaTime * spikesSpawnOthers.Spine.timeScale;
      yield return (object) null;
    }
    spikesSpawnOthers.spriteRenderer.color = spikesSpawnOthers.OverrideColor != Color.white ? spikesSpawnOthers.OverrideColor : Color.yellow;
    progress = 0.0f;
    while ((double) progress < 0.25)
    {
      progress += Time.deltaTime * spikesSpawnOthers.Spine.timeScale;
      yield return (object) null;
    }
    spikesSpawnOthers.spine.gameObject.SetActive(true);
    spikesSpawnOthers.spine.state.SetAnimation(0, spikesSpawnOthers.spine.AnimationName, false);
    spikesSpawnOthers.spriteRenderer.color = spikesSpawnOthers.OverrideColor != Color.white ? spikesSpawnOthers.OverrideColor : Color.red;
    CameraManager.shakeCamera(0.3f, (float) Random.Range(0, 360));
    Transform transform = spikesSpawnOthers.boxCollider2D.transform;
    Bounds bounds = spikesSpawnOthers.boxCollider2D.bounds;
    Vector2 center = (Vector2) bounds.center;
    Vector2 size1 = (Vector2) bounds.size;
    float z = transform.eulerAngles.z;
    Vector2 size2 = size1;
    double angle = (double) z;
    Collider2D[] hitBuffer = TrapSpikesSpawnOthers._hitBuffer;
    int num = Physics2D.OverlapBoxNonAlloc(center, size2, (float) angle, hitBuffer);
    for (int index = 0; index < num; ++index)
    {
      Collider2D collider2D = TrapSpikesSpawnOthers._hitBuffer[index];
      Health component;
      if ((bool) (Object) collider2D && collider2D.TryGetComponent<Health>(out component) && (Object) component != (Object) null && component.team != Health.Team.Team2)
        component.DealDamage(component.team == Health.Team.PlayerTeam ? 1f : 10f, spikesSpawnOthers.gameObject, spikesSpawnOthers.transform.position);
    }
    progress = 0.0f;
    while ((double) progress < 0.75)
    {
      progress += Time.deltaTime * spikesSpawnOthers.Spine.timeScale;
      yield return (object) null;
    }
    spikesSpawnOthers.spriteRenderer.color = spikesSpawnOthers.OverrideColor != Color.white ? spikesSpawnOthers.OverrideColor : Color.white;
    progress = 0.0f;
    while ((double) progress < 0.25)
    {
      progress += Time.deltaTime * spikesSpawnOthers.Spine.timeScale;
      yield return (object) null;
    }
    spikesSpawnOthers.gameObject.Recycle();
  }

  public GameObject GetOwner()
  {
    return !((Object) this.Origin != (Object) null) ? (GameObject) null : this.Origin.gameObject;
  }

  public void SetOwner(GameObject owner) => this.Origin = owner.GetComponent<Health>();
}
