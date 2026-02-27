// Decompiled with JetBrains decompiler
// Type: BreakableTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Health))]
public class BreakableTile : BaseMonoBehaviour
{
  private Health health;
  public Transform[] tToShake;
  public List<Vector2> tShakes;
  public SpriteRenderer[] spriteRenderers;
  public bool PlayHitSoundsInOrder;
  private Collider2D BoxCollider;
  public SoundConstants.SoundMaterial soundMaterial;

  private void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.OnHit += new Health.HitAction(this.OnHit);
  }

  private void Start()
  {
    this.tShakes = new List<Vector2>();
    foreach (Transform transform in this.tToShake)
      this.tShakes.Add(Vector2.zero);
    this.spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
    this.BoxCollider = (Collider2D) this.GetComponent<BoxCollider2D>();
  }

  public virtual void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.soundMaterial != SoundConstants.SoundMaterial.None)
      AudioManager.Instance.PlayOneShot(SoundConstants.GetImpactSoundPathForMaterial(this.soundMaterial), this.transform.position);
    this.FlashRed();
    int index = -1;
    while (++index < this.tShakes.Count)
      this.tShakes[index] = new Vector2(Random.Range(-0.5f, 0.5f), 0.0f);
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, Attacker.transform.position), false);
    this.HitFX();
  }

  public void HitFX()
  {
    BiomeConstants.Instance.EmitHitVFX(this.transform.position + Vector3.back, Quaternion.identity.z, "HitFX_Weak");
  }

  public void FlashRed()
  {
    this.StopCoroutine((IEnumerator) this.DoFlashRed());
    this.StartCoroutine((IEnumerator) this.DoFlashRed());
  }

  private IEnumerator DoFlashRed()
  {
    float Progress = 0.0f;
    while ((double) (Progress += 0.1f) <= 1.0)
    {
      foreach (SpriteRenderer spriteRenderer in this.spriteRenderers)
      {
        if ((Object) spriteRenderer != (Object) null)
          spriteRenderer.color = Color.Lerp(Color.red, Color.white, Progress);
      }
      yield return (object) null;
    }
    foreach (SpriteRenderer spriteRenderer in this.spriteRenderers)
    {
      if ((Object) spriteRenderer != (Object) null)
        spriteRenderer.color = Color.white;
    }
  }

  private void Update()
  {
    int index1 = -1;
    while (++index1 < this.tShakes.Count)
    {
      Vector2 tShake = this.tShakes[index1];
      tShake.y += (float) ((0.0 - (double) tShake.x) * 0.30000001192092896);
      tShake.x += (tShake.y *= 0.7f);
      this.tShakes[index1] = tShake;
    }
    int index2 = -1;
    while (++index2 < this.tToShake.Length)
    {
      if ((Object) this.tToShake[index2] != (Object) null)
        this.tToShake[index2].localPosition = new Vector3(this.tShakes[index2].x, this.tToShake[index2].localPosition.y, this.tToShake[index2].localPosition.z);
    }
  }

  public virtual void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (this.soundMaterial != SoundConstants.SoundMaterial.None)
      AudioManager.Instance.PlayOneShot(SoundConstants.GetBreakSoundPathForMaterial(this.soundMaterial), this.transform.position);
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(Attacker.transform.position, this.transform.position), false);
    if (!((Object) this.BoxCollider != (Object) null))
      return;
    Bounds bounds = this.BoxCollider.bounds;
    this.BoxCollider.enabled = false;
    AstarPath.active.UpdateGraphs(bounds);
  }

  private void OnDestroy()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
  }

  private void OnDisable()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
  }

  public void SetParentToRoom(Transform transform)
  {
    transform.parent = BiomeGenerator.Instance.CurrentRoom.generateRoom.transform;
  }
}
