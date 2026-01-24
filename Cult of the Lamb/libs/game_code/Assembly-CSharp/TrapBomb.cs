// Decompiled with JetBrains decompiler
// Type: TrapBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class TrapBomb : BaseMonoBehaviour
{
  public SpriteRenderer Target;
  public SpriteRenderer TargetWarning;
  public GameObject FuseParticles;
  public GameObject Light;
  public SkeletonAnimation Spine;
  public int ExplosionRange = 4;
  public Health health;
  public SpriteRenderer sprite;
  public SpriteRenderer Shadow;
  public float respawnTimer;
  public bool Detonating;
  public const string SHADER_COLOR_NAME = "_Color";
  public float TriggerRange = 2f;
  public float DetonateTime = 3f;
  public int PlayerDamage = 1;
  public int Team2Damage = 6;
  public bool Respawning;

  public void Start()
  {
    this.Target.enabled = false;
    this.TargetWarning.enabled = false;
    this.FuseParticles.SetActive(false);
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void OnEnable()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void OnDisable()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void RoomLockController_OnRoomCleared()
  {
    this.StopAllCoroutines();
    this.Detonating = true;
    this.Respawning = false;
    this.Spine.AnimationState.SetAnimation(0, "hide", false);
    this.Spine.AnimationState.AddAnimation(0, "hidden", true, 0.0f);
    this.sprite.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    this.Shadow.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    this.Target.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    this.TargetWarning.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.Explode(2);
  }

  public void Update()
  {
    if (this.Detonating)
      return;
    if (this.Respawning)
    {
      if ((double) (this.respawnTimer -= Time.deltaTime) >= 0.0)
        return;
      this.health.enabled = true;
      this.sprite.enabled = true;
      this.Shadow.enabled = true;
      this.Spine.AnimationState.SetAnimation(0, "appear", false);
      this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      this.sprite.transform.localScale = Vector3.zero;
      this.sprite.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.Shadow.transform.localScale = Vector3.zero;
      this.Shadow.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.Respawning = false;
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.gameObject);
    }
    else
    {
      if (Time.frameCount % 10 != 0)
        return;
      foreach (Component component in Health.playerTeam)
      {
        if ((double) this.MagnitudeFindDistanceBetween(component.transform.position, this.health.transform.position) < (double) this.TriggerRange * (double) this.TriggerRange)
        {
          AudioManager.Instance.PlayOneShot("event:/explosion/bomb_fuse", this.transform.position);
          this.StartCoroutine((IEnumerator) this.DoDetonating());
          this.StartCoroutine((IEnumerator) this.FlashCircle());
          break;
        }
      }
    }
  }

  public IEnumerator FlashCircle()
  {
    this.Spine.AnimationState.SetAnimation(0, "explode", true);
    this.FuseParticles.SetActive(true);
    this.Light.SetActive(true);
    this.Target.enabled = this.TargetWarning.enabled = true;
    this.Target.transform.localScale = Vector3.zero;
    this.TargetWarning.transform.localScale = Vector3.zero;
    this.Target.transform.DOScale(Vector3.one * (float) this.ExplosionRange * 0.5f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.TargetWarning.transform.DOScale(Vector3.one * (float) this.ExplosionRange * 0.5f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    Color white = new Color(1f, 1f, 1f, 1f);
    Color color = white;
    float flashTickTimer = 0.0f;
    while (true)
    {
      this.Target.transform.Rotate(new Vector3(0.0f, 0.0f, 150f) * Time.deltaTime);
      if ((double) flashTickTimer >= 0.11999999731779099 && (double) Time.timeScale == 1.0 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        this.Target.material.SetColor("_Color", color = color == white ? Color.red : white);
        this.TargetWarning.material.SetColor("_Color", color);
        flashTickTimer = 0.0f;
      }
      flashTickTimer += Time.deltaTime;
      yield return (object) null;
    }
  }

  public IEnumerator DoDetonating()
  {
    this.Detonating = true;
    float Timer = 0.0f;
    Color white = Color.white;
    while ((double) (Timer += Time.deltaTime) < (double) this.DetonateTime)
      yield return (object) null;
    this.Explode(this.ExplosionRange);
    this.Hide(false);
    this.Light.SetActive(false);
    this.Detonating = false;
  }

  public void Hide(bool Respawn)
  {
    this.Spine.AnimationState.SetAnimation(0, "hide", false);
    this.Spine.AnimationState.AddAnimation(0, "hidden", true, 0.0f);
    this.Respawning = true;
    this.Target.enabled = false;
    this.TargetWarning.enabled = false;
    this.FuseParticles.SetActive(false);
    this.health.enabled = false;
    this.sprite.enabled = false;
    this.Shadow.enabled = false;
    this.StopAllCoroutines();
    if (Respawn)
      this.respawnTimer = 10f;
    else
      this.enabled = false;
  }

  public virtual void Explode(int Size)
  {
    Explosion.CreateExplosion(this.transform.position, Health.Team.KillAll, this.health, (float) Size, (float) this.PlayerDamage, (float) this.Team2Damage);
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.TriggerRange, Color.yellow);
    Utils.DrawCircleXY(this.transform.position, 2f, Color.red);
  }
}
