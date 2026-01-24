// Decompiled with JetBrains decompiler
// Type: TrapBarrel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TrapBarrel : BaseMonoBehaviour
{
  public float ExplosionSize = 5f;
  public SpriteRenderer sprite;
  public Health health;
  public CircleCollider2D circleCollider2D;
  public Rigidbody2D rb2D;
  public float Speed;
  public Vector2 DirectionalSpeed;
  public bool RightFacing;
  public float Angle;
  public float Z = -0.5f;
  public float ZSpeed = -4f;
  public Vector2 ScaleX = new Vector2(0.5f, 0.0f);
  public Vector2 ScaleY = new Vector2(2f, 0.0f);
  public float DestroyTimer;

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.circleCollider2D = this.GetComponent<CircleCollider2D>();
    this.rb2D = this.GetComponent<Rigidbody2D>();
  }

  public void ChangeSprite(Sprite NewSprite) => this.sprite.sprite = NewSprite;

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    CameraManager.shakeCamera(1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    GameManager.GetInstance().HitStop();
    this.Speed = 15f;
    this.Angle = Utils.GetAngle(Attacker.transform.position, this.transform.position);
    this.gameObject.layer = LayerMask.NameToLayer("Bullets");
    Health health = (Health) null;
    float num1 = float.MaxValue;
    float num2 = 0.0f;
    float num3 = 90f;
    foreach (Health allUnit in Health.allUnits)
    {
      float angle = Utils.GetAngle(this.transform.position, allUnit.transform.position);
      float num4 = Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.transform.position);
      if ((UnityEngine.Object) allUnit != (UnityEngine.Object) this.health && !allUnit.InanimateObject && allUnit.team == Health.Team.Team2 && (double) num4 < 8.0 && (double) num4 < (double) num1 && (double) Mathf.Abs(this.Angle - angle) < 180.0 && (double) angle > (double) this.Angle - (double) num3 && (double) angle < (double) this.Angle + (double) num3)
      {
        health = allUnit;
        num1 = num4;
        num2 = angle;
      }
    }
    if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      this.Angle = num2;
    this.DirectionalSpeed = new Vector2(this.Speed * Mathf.Cos(this.Angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(this.Angle * ((float) Math.PI / 180f)));
    if ((double) this.DirectionalSpeed.x > 0.0)
      this.RightFacing = true;
    else
      this.RightFacing = false;
  }

  public void BounceMe()
  {
    this.ZSpeed += 0.5f * GameManager.DeltaTime;
    this.Z += this.ZSpeed * Time.deltaTime;
    if ((double) this.Z >= 0.0)
    {
      if (this.OnGround(this.transform.position))
      {
        this.ScaleX.x = 1.5f;
        this.ScaleY.x = 0.5f;
        this.ZSpeed *= -1f;
        this.Z = 0.0f;
      }
      else
      {
        this.ScaleX.x -= 0.2f * GameManager.DeltaTime;
        this.ScaleX.x = Mathf.Max(0.0f, this.ScaleX.x);
        this.ScaleY.x -= 0.2f * GameManager.DeltaTime;
        this.ScaleY.x = Mathf.Max(0.0f, this.ScaleY.x);
        if ((double) (this.DestroyTimer += Time.deltaTime) > 0.5 || (double) this.ScaleX.x <= 0.0)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
    }
    else
    {
      this.ScaleX.y += (float) ((1.0 - (double) this.ScaleX.x) * 0.30000001192092896);
      this.ScaleX.x += (this.ScaleX.y *= 0.8f);
      this.ScaleY.y += (float) ((1.0 - (double) this.ScaleY.x) * 0.30000001192092896);
      this.ScaleY.x += (this.ScaleY.y *= 0.8f);
    }
    this.sprite.transform.parent.transform.parent.localPosition = Vector3.forward * this.Z;
    this.sprite.transform.parent.transform.parent.localScale = new Vector3(this.ScaleX.x, this.ScaleY.y, 1f);
  }

  public void Update()
  {
    this.sprite.transform.parent.Rotate(new Vector3(0.0f, this.Speed * (this.RightFacing ? -1f : 1f), 0.0f));
    if ((double) this.Speed <= 0.0)
      return;
    this.BounceMe();
    foreach (Health health in Health.team2)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null && (UnityEngine.Object) health != (UnityEngine.Object) this.health && (double) this.MagnitudeFindDistanceBetween(this.transform.position, health.transform.position) < 0.25)
      {
        this.health.DealDamage(float.MaxValue, this.gameObject, Vector3.zero);
        Explosion.CreateExplosion(this.transform.position + Vector3.forward * this.Z, Health.Team.PlayerTeam, this.health, this.ExplosionSize);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
        break;
      }
    }
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if ((double) this.Speed <= 0.0 || collision.gameObject.layer == LayerMask.NameToLayer("Obstacles") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
      return;
    this.health.DealDamage(float.MaxValue, this.gameObject, Vector3.zero);
    Explosion.CreateExplosion(this.transform.position + Vector3.forward * this.Z, Health.Team.PlayerTeam, this.health, this.ExplosionSize);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void FixedUpdate()
  {
    this.rb2D.MovePosition(this.rb2D.position + this.DirectionalSpeed * Time.fixedDeltaTime);
  }

  public bool OnGround(Vector3 Position)
  {
    LayerMask mask = (LayerMask) LayerMask.GetMask("Island");
    return Physics.Raycast(Position, Vector3.forward, out RaycastHit _, 10f, (int) mask);
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }
}
