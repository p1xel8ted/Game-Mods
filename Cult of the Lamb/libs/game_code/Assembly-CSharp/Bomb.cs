// Decompiled with JetBrains decompiler
// Type: Bomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Bomb : BaseMonoBehaviour
{
  public static List<Bomb> bombs = new List<Bomb>();
  [SerializeField]
  public float explodeDelay = 3f;
  [SerializeField]
  public Transform container;
  public float moveTimer;
  public float explodeTimer;
  public Health health;
  public Rigidbody2D rigidbody;
  public float childZ;
  public float vx;
  public float vy;
  public float vz;
  public float Scale;
  public float facingAngle;
  public float speed;
  public float damageMultiplier = 1f;
  public Vector2 SquishScale = Vector2.one;
  public Vector2 SquishScaleSpeed = Vector2.zero;

  public static void CreateBomb(
    Vector3 position,
    Health health,
    Transform parent,
    float damageMultiplier = 1f)
  {
    Addressables_wrapper.InstantiateAsync(!((UnityEngine.Object) health != (UnityEngine.Object) null) || health.team != Health.Team.PlayerTeam ? (object) "Assets/Prefabs/Enemies/Weapons/Bomb.prefab" : (object) "Assets/Prefabs/Enemies/Weapons/Bomb Friendly.prefab", position, Quaternion.identity, parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Bomb component = obj.Result.GetComponent<Bomb>();
      obj.Result.transform.position = position;
      component.health = health;
      component.damageMultiplier = damageMultiplier;
      AudioManager.Instance.PlayOneShot("event:/explosion/bomb_fuse", obj.Result);
    }));
  }

  public void Awake()
  {
    this.rigidbody = this.GetComponent<Rigidbody2D>();
    this.vz = UnityEngine.Random.Range(-0.3f, -0.15f);
    this.speed = UnityEngine.Random.Range(5f, 10f);
    this.facingAngle = (float) UnityEngine.Random.Range(0, 360);
    Bomb.bombs.Add(this);
  }

  public void OnDestroy() => Bomb.bombs.Remove(this);

  public virtual void Update()
  {
    if (PlayerRelic.TimeFrozen && (UnityEngine.Object) this.health != (UnityEngine.Object) null && this.health.team == Health.Team.Team2)
      return;
    this.moveTimer += Time.deltaTime;
    if (!MMConversation.isPlaying)
    {
      this.explodeTimer += Time.deltaTime;
      if ((double) this.explodeTimer / (double) this.explodeDelay > 1.0 && this.gameObject.activeInHierarchy)
      {
        float Damage = 1f * this.damageMultiplier;
        Explosion.CreateExplosion(this.transform.position, !((UnityEngine.Object) this.health != (UnityEngine.Object) null) || this.health.team != Health.Team.PlayerTeam ? Health.Team.KillAll : Health.Team.PlayerTeam, this.health, 5f, Damage);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
    }
    if ((double) this.moveTimer < 1.0)
    {
      this.Scale += (float) ((1.0 - (double) this.Scale) / 7.0);
      this.SquishScaleSpeed.x += (float) ((1.0 - (double) this.SquishScale.x) * 0.30000001192092896);
      this.SquishScale.x += (this.SquishScaleSpeed.x *= 0.7f);
      this.SquishScaleSpeed.y += (float) ((1.0 - (double) this.SquishScale.y) * 0.30000001192092896);
      this.SquishScale.y += (this.SquishScaleSpeed.y *= 0.7f);
      if ((double) Time.timeScale > 0.0)
        this.transform.localScale = new Vector3(this.Scale * this.SquishScale.x, this.Scale * this.SquishScale.y, this.Scale);
      this.container.transform.localPosition = new Vector3(0.0f, 0.0f, this.childZ);
      this.Bounce();
    }
    this.transform.localScale = Vector3.one * (float) (1.0 + (double) Mathf.PingPong(Time.time * 2f, 0.3f) - 0.15000000596046448);
  }

  public void OnDisable() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public void FixedUpdate()
  {
    if ((UnityEngine.Object) this.rigidbody == (UnityEngine.Object) null)
      return;
    this.speed += (float) ((0.0 - (double) this.speed) / 12.0) * GameManager.FixedDeltaTime;
    this.vx = this.speed * Mathf.Cos(this.facingAngle * ((float) Math.PI / 180f));
    this.vy = this.speed * Mathf.Sin(this.facingAngle * ((float) Math.PI / 180f));
    this.rigidbody.MovePosition(this.rigidbody.position + new Vector2(this.vx, this.vy) * Time.fixedDeltaTime);
  }

  public void Bounce()
  {
    if ((double) this.childZ > 0.0)
    {
      if ((double) this.vz > 0.079999998211860657)
      {
        this.vz *= -0.4f;
        this.SquishScale = new Vector2(0.8f, 1.2f);
      }
      else
        this.vz = 0.0f;
      this.childZ = 0.0f;
    }
    else
      this.vz += 0.02f;
    this.childZ += this.vz;
    this.container.transform.localPosition = new Vector3(0.0f, 0.0f, this.childZ);
  }
}
