// Decompiled with JetBrains decompiler
// Type: IcicleControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IcicleControl : MonoBehaviour
{
  [EventRef]
  public string IcicleFallSFX = "event:/material/stone_debris_fall";
  [EventRef]
  public string IcicleCollisionSFX = "event:/material/stone_break";
  public CircleCollider2D Collider;
  public ParticleSystem AOEParticles;
  public Transform IcicleTransform;
  public SpriteRenderer IndicatorIcon;
  public SpriteRenderer Shadow;
  public Material ParticleMaterial;
  public float ParticleChunkCount = 10f;
  public float EnemyDamage = 5f;
  public float shadowScaleAtGround = 3f;
  public float startHeight = -10f;
  public float playerDamage = 1f;
  public float indicatorFlashTimer;
  public Color indicatorColor = Color.white;
  [SerializeField]
  public List<Sprite> particleChunks;

  public void OnEnable()
  {
    this.AOEParticles.gameObject.SetActive(false);
    this.Shadow.gameObject.SetActive(false);
    this.IndicatorIcon.gameObject.SetActive(false);
    this.IcicleTransform.gameObject.SetActive(true);
  }

  public void OnDisable()
  {
    this.Reset();
    this.Invoke("Repool", 0.2f);
  }

  public void Repool() => ObjectPool.Recycle(this.gameObject);

  public void Update()
  {
    if ((double) Time.timeScale == 0.0)
      return;
    if (this.IndicatorIcon.gameObject.activeSelf && (double) this.indicatorFlashTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
    {
      this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
      this.IndicatorIcon.material.SetColor("_Color", this.indicatorColor);
      this.indicatorFlashTimer = 0.0f;
    }
    this.indicatorFlashTimer += Time.deltaTime;
  }

  public void Drop(float delay, float fallSpeed)
  {
    this.Shadow.gameObject.SetActive(true);
    this.IcicleTransform.gameObject.SetActive(true);
    this.IndicatorIcon.gameObject.SetActive(true);
    this.IcicleTransform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.startHeight);
    this.StartCoroutine((IEnumerator) this.DoDrop(delay, fallSpeed));
  }

  public IEnumerator DoDrop(float delay, float fallSpeed)
  {
    IcicleControl icicleControl = this;
    icicleControl.IndicatorIcon.gameObject.SetActive(true);
    icicleControl.Shadow.gameObject.SetActive(true);
    yield return (object) new WaitForSeconds(delay);
    icicleControl.Shadow.transform.DOScale(Vector3.one * icicleControl.shadowScaleAtGround, 4f);
    AudioManager.Instance.PlayOneShot(icicleControl.IcicleFallSFX, icicleControl.transform.position);
    while (PlayerRelic.TimeFrozen)
      yield return (object) null;
    icicleControl.IcicleTransform.DOMoveZ(0.0f, fallSpeed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>().OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(icicleControl.DoLand)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
  }

  public void DoLand()
  {
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.Collider.radius))
    {
      Health component = collider2D.GetComponent<Health>();
      PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(collider2D.gameObject);
      if ((bool) (Object) component && (component.team != Health.Team.PlayerTeam || !TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, farmingComponent)) && !component.ImmuneToTraps)
        component.DealDamage(component.team == Health.Team.PlayerTeam ? this.playerDamage : this.EnemyDamage, this.gameObject, this.transform.position);
    }
    AudioManager.Instance.PlayOneShot(this.IcicleCollisionSFX, this.transform.position);
    this.Shadow.gameObject.SetActive(false);
    this.IcicleTransform.gameObject.SetActive(false);
    this.IndicatorIcon.gameObject.SetActive(false);
    this.AOEParticles.Play();
    if (this.particleChunks.Count <= 0)
      return;
    for (int index = 0; (double) index < (double) this.ParticleChunkCount; ++index)
      Particle_Chunk.AddNew(this.transform.position, (float) Random.Range(0, 360), this.particleChunks);
  }

  public void Reset()
  {
    this.AOEParticles.gameObject.SetActive(false);
    this.Shadow.gameObject.SetActive(false);
    this.Shadow.transform.DOKill();
    this.Shadow.transform.localScale = Vector3.one;
    this.IcicleTransform.transform.DOKill();
    this.IcicleTransform.gameObject.SetActive(false);
    this.IndicatorIcon.gameObject.SetActive(false);
  }
}
