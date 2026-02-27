// Decompiled with JetBrains decompiler
// Type: DeadBodySliding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class DeadBodySliding : BaseMonoBehaviour
{
  private Rigidbody2D rb2d;
  private float Speed = 1700f;
  private Vector2 ScaleX;
  private Vector2 ScaleY;
  private Vector2 StartScale = new Vector2(3f, 3f);
  public Renderer sprite;
  private float ColorProgress;
  private Vector2 Force;
  public CircleCollider2D circleCollider;
  private Health health;
  private Material material;
  private const string SHADER_COLOR_NAME = "_Color";
  public LayerMask hitMask;
  private float _Z;
  private float Zv;
  private bool explode;

  private float Z
  {
    get => this._Z;
    set
    {
      this._Z = value;
      if (!(bool) (UnityEngine.Object) this.sprite)
        return;
      this.sprite.transform.localPosition = Vector3.forward * this.Z;
    }
  }

  private void Start()
  {
    if ((UnityEngine.Object) this.sprite != (UnityEngine.Object) null)
      this.sprite.receiveShadows = true;
    this.hitMask = (LayerMask) ((int) this.hitMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.hitMask = (LayerMask) ((int) this.hitMask | 1 << LayerMask.NameToLayer("Island"));
  }

  private void OnEnable()
  {
    this.health = this.GetComponentInChildren<Health>();
    this.health.DestroyOnDeath = false;
    this.material = this.sprite.material;
    Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.OnChestRevealed);
  }

  private void OnDisable()
  {
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.OnChestRevealed);
  }

  private void OnChestRevealed()
  {
    if (this.explode || !DataManager.Instance.BonesEnabled)
      return;
    this.health.DealDamage((float) int.MaxValue, this.gameObject, this.transform.position);
  }

  public void Init(GameObject enemy, float Angle, float Speed = 1700f, bool explode = false)
  {
    this.rb2d = this.GetComponent<Rigidbody2D>();
    this.Force = new Vector2(Speed * Mathf.Cos(Angle * ((float) Math.PI / 180f)), Speed * Mathf.Sin(Angle * ((float) Math.PI / 180f)));
    this.ScaleX = new Vector2(1f, 0.0f);
    this.ScaleY = new Vector2(1f, 0.0f);
    if ((UnityEngine.Object) this.sprite != (UnityEngine.Object) null)
      this.sprite.transform.localScale = new Vector3(this.ScaleX.x, this.ScaleY.x, 1f);
    this.Z = -0.5f;
    this.Zv = -3f;
    this.ColorProgress = 0.0f;
    this.SetFacing();
    if (this.gameObject.activeInHierarchy)
    {
      this.StartCoroutine((IEnumerator) this.DelayForce());
      this.StartCoroutine((IEnumerator) this.DoScale());
      this.StartCoroutine((IEnumerator) this.DoBounce());
      this.StartCoroutine((IEnumerator) this.HealthRoutine());
    }
    if ((UnityEngine.Object) this.GetComponentInChildren<Health>(true) != (UnityEngine.Object) null)
      this.GetComponentInChildren<Health>(true).OnEnable();
    this.explode = explode;
    if (!explode)
      return;
    this.StartCoroutine((IEnumerator) this.ExplodeRoutine());
  }

  private IEnumerator ExplodeRoutine()
  {
    DeadBodySliding deadBodySliding = this;
    yield return (object) new WaitForSeconds(1f);
    float explodeDelay = 1f;
    Color color = Color.white;
    while ((double) (explodeDelay -= Time.deltaTime) > 0.0)
    {
      if (Time.frameCount % 5 == 0)
        deadBodySliding.material.SetColor("_Color", color = color == Color.white ? new Color(1f, 1f, 1f, 0.0f) : Color.white);
      yield return (object) null;
    }
    deadBodySliding.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.0f));
    Explosion.CreateExplosion(deadBodySliding.transform.position, Health.Team.PlayerTeam, deadBodySliding.health, 4f);
    deadBodySliding.gameObject.Recycle();
  }

  private IEnumerator HealthRoutine()
  {
    if ((bool) (UnityEngine.Object) this.health)
    {
      this.health.invincible = true;
      yield return (object) new WaitForSeconds(0.3f);
      this.health.invincible = false;
    }
  }

  private IEnumerator DoBounce()
  {
    yield return (object) new WaitForSeconds(0.2f);
    while ((double) this.Z < 0.0)
    {
      this.Zv += 0.5f * GameManager.DeltaTime;
      this.Z += this.Zv * Time.deltaTime;
      if ((double) this.Z >= 0.0 && (double) this.Zv > 4.0)
      {
        this.Zv *= -0.4f;
        this.Z = -0.1f;
        this.ScaleX = new Vector2(1.5f, 0.0f);
        this.ScaleY = new Vector2(0.5f, 0.0f);
        if (this.sprite.isVisible)
          CameraManager.shakeCamera(0.1f, (float) UnityEngine.Random.Range(0, 360), false);
      }
      yield return (object) null;
    }
    this.Z = 0.0f;
    this.ScaleX = new Vector2(1.5f, 0.0f);
    this.ScaleY = new Vector2(0.5f, 0.0f);
    if (this.sprite.isVisible)
      CameraManager.shakeCamera(0.3f, (float) UnityEngine.Random.Range(0, 360), false);
  }

  private IEnumerator DoScale()
  {
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 3.0)
    {
      this.ScaleX.y += (float) ((1.0 - (double) this.ScaleX.x) * 0.20000000298023224);
      this.ScaleX.x += (this.ScaleX.y *= 0.7f);
      this.ScaleY.y += (float) ((1.0 - (double) this.ScaleY.x) * 0.20000000298023224);
      this.ScaleY.x += (this.ScaleY.y *= 0.7f);
      this.ScaleX.y += (float) ((1.0 - (double) this.ScaleX.x) * 0.20000000298023224);
      this.ScaleX.x += (this.ScaleX.y *= 0.7f);
      this.ScaleY.y += (float) ((1.0 - (double) this.ScaleY.x) * 0.20000000298023224);
      this.ScaleY.x += (this.ScaleY.y *= 0.7f);
      this.sprite.transform.localScale = new Vector3(this.ScaleX.x * -1f, this.ScaleY.x, 1f);
      yield return (object) null;
    }
  }

  private IEnumerator DelayForce()
  {
    DeadBodySliding deadBodySliding = this;
    yield return (object) new WaitForSeconds(0.2f);
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) deadBodySliding.transform.position, deadBodySliding.Force.normalized, 0.5f, (int) deadBodySliding.hitMask).collider != (UnityEngine.Object) null)
      deadBodySliding.Force *= -1f;
    deadBodySliding.rb2d.AddForce(deadBodySliding.Force);
    while ((double) (deadBodySliding.ColorProgress += 0.1f) <= 1.0)
    {
      if (deadBodySliding.sprite is SpriteRenderer)
        ((SpriteRenderer) deadBodySliding.sprite).color = Color.Lerp(Color.red, Color.white, deadBodySliding.ColorProgress);
      yield return (object) null;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    this.SetFacing();
    if (!this.sprite.isVisible)
      return;
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, (Vector3) collision.contacts[0].point), false);
  }

  private void SetFacing()
  {
    if (!(bool) (UnityEngine.Object) this.rb2d)
      return;
    if ((double) this.rb2d.velocity.x < 0.0)
    {
      this.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    else
    {
      if ((double) this.rb2d.velocity.x <= 0.0)
        return;
      this.transform.localScale = new Vector3(-1f, 1f, 1f);
    }
  }

  public void OnDie()
  {
    AudioManager.Instance.PlayOneShot(SoundConstants.GetBreakSoundPathForMaterial(SoundConstants.SoundMaterial.Bone), this.transform.position);
    this.gameObject.SetActive(false);
  }
}
