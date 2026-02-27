// Decompiled with JetBrains decompiler
// Type: Bomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Bomb : BaseMonoBehaviour
{
  [SerializeField]
  private float explodeDelay = 3f;
  [SerializeField]
  private Transform container;
  private float moveTimer;
  private float explodeTimer;
  private Health health;
  private Rigidbody2D rigidbody;
  private float childZ;
  private float vx;
  private float vy;
  private float vz;
  private float Scale;
  private float facingAngle;
  private float speed;
  private Vector2 SquishScale = Vector2.one;
  private Vector2 SquishScaleSpeed = Vector2.zero;

  public static void CreateBomb(Vector3 position, Health health, Transform parent)
  {
    Addressables.InstantiateAsync(!((UnityEngine.Object) health != (UnityEngine.Object) null) || health.team != Health.Team.PlayerTeam ? (object) "Assets/Prefabs/Enemies/Weapons/Bomb.prefab" : (object) "Assets/Prefabs/Enemies/Weapons/Bomb Friendly.prefab", position, Quaternion.identity, parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Bomb component = obj.Result.GetComponent<Bomb>();
      obj.Result.transform.position = position;
      Health health1 = health;
      component.health = health1;
      AudioManager.Instance.PlayOneShot("event:/explosion/bomb_fuse", obj.Result);
    });
  }

  private void Awake()
  {
    this.rigidbody = this.GetComponent<Rigidbody2D>();
    this.vz = UnityEngine.Random.Range(-0.3f, -0.15f);
    this.speed = UnityEngine.Random.Range(5f, 10f);
    this.facingAngle = (float) UnityEngine.Random.Range(0, 360);
  }

  private void Update()
  {
    this.explodeTimer += Time.deltaTime;
    this.moveTimer += Time.deltaTime;
    if ((double) this.explodeTimer / (double) this.explodeDelay > 1.0 && this.gameObject.activeInHierarchy)
    {
      Explosion.CreateExplosion(this.transform.position, Health.Team.KillAll, this.health, 5f);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
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

  private void OnDisable() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  private void FixedUpdate()
  {
    if ((UnityEngine.Object) this.rigidbody == (UnityEngine.Object) null)
      return;
    this.speed += (float) ((0.0 - (double) this.speed) / 12.0) * GameManager.FixedDeltaTime;
    this.vx = this.speed * Mathf.Cos(this.facingAngle * ((float) Math.PI / 180f));
    this.vy = this.speed * Mathf.Sin(this.facingAngle * ((float) Math.PI / 180f));
    this.rigidbody.MovePosition(this.rigidbody.position + new Vector2(this.vx, this.vy) * Time.fixedDeltaTime);
  }

  private void Bounce()
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
