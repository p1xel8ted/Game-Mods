// Decompiled with JetBrains decompiler
// Type: BombLightning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class BombLightning : Bomb
{
  public float lightningExpansionSpeed = 5f;
  public float lightningEnemyDamage = 4f;
  public float lightningStrikeRadius = 2f;
  public const float LIGHTNING_DAMAGE_PLAYER = 1f;

  public new static void CreateBomb(
    Vector3 position,
    Health health,
    Transform parent,
    float damageMultiplier = 1f)
  {
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Enemies/Weapons/BombLightning.prefab", position, Quaternion.identity, parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      BombLightning component = obj.Result.GetComponent<BombLightning>();
      obj.Result.transform.position = position;
      component.health = health;
      component.damageMultiplier = damageMultiplier;
      AudioManager.Instance.PlayOneShot("event:/explosion/bomb_fuse", obj.Result);
    }));
  }

  public override void Update()
  {
    if (PlayerRelic.TimeFrozen && (UnityEngine.Object) this.health != (UnityEngine.Object) null && this.health.team == Health.Team.Team2)
      return;
    this.explodeTimer += Time.deltaTime;
    this.moveTimer += Time.deltaTime;
    if ((double) this.explodeTimer / (double) this.explodeDelay > 1.0 && this.gameObject.activeInHierarchy)
    {
      float Damage = 1f * this.damageMultiplier;
      Explosion.CreateExplosion(this.transform.position, !((UnityEngine.Object) this.health != (UnityEngine.Object) null) || this.health.team != Health.Team.PlayerTeam ? Health.Team.KillAll : Health.Team.PlayerTeam, this.health, 1f, Damage);
      LightningRingExplosion.CreateExplosion(this.transform.position, !((UnityEngine.Object) this.health != (UnityEngine.Object) null) || this.health.team != Health.Team.PlayerTeam ? Health.Team.Team2 : Health.Team.PlayerTeam, this.health, this.lightningExpansionSpeed, 1f, this.lightningEnemyDamage);
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
}
