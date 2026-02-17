// Decompiled with JetBrains decompiler
// Type: PlayerSnowball
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PlayerSnowball : MonoBehaviour
{
  [SerializeField]
  public Projectile projectile;
  [SerializeField]
  public GameObject flockadePiecesPrefab;
  public Vector2 lastPosition = Vector2.zero;
  public const float projectileSize = 1f;

  public void Update()
  {
    foreach (Follower follower in Follower.Followers)
    {
      if ((double) (follower.transform.position - this.transform.position).sqrMagnitude <= 1.0 && (follower.Brain.CurrentTask == null || !(follower.Brain.CurrentTask is FollowerTask_HardManualControl)))
      {
        this.HitFollower(follower);
        break;
      }
    }
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!((UnityEngine.Object) this.projectile.Owner == (UnityEngine.Object) player.health) && (double) (this.transform.position - player.transform.position).sqrMagnitude <= 0.5)
      {
        this.HitPlayer(player);
        break;
      }
    }
    foreach (Interaction_WolfBase wolf in Interaction_WolfBase.Wolfs)
    {
      if ((double) (this.transform.position - wolf.transform.position).sqrMagnitude <= 0.5)
      {
        this.HitWolf(wolf);
        break;
      }
    }
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if ((double) (this.transform.position - ranchable.transform.position).sqrMagnitude <= 0.5)
      {
        this.HitAnimal(ranchable);
        break;
      }
    }
  }

  public void HitPlayer(PlayerFarming player)
  {
    CameraManager.shakeCamera(5f, this.projectile.Angle);
    BiomeConstants.Instance.EmitSnowImpactVFX(this.transform.position, 4f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, player, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    string str1 = player.isLamb ? "lamb" : "goat";
    string str2 = (double) this.projectile.Angle > 270.0 ? "back" : "front";
    player.CustomAnimationWithCallback($"snowball/hit-{str2}-{str1}", false, (System.Action) (() => player.state.CURRENT_STATE = StateMachine.State.Idle));
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.ApplyKnockbackRoutine(player.gameObject));
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void HitFollower(Follower follower)
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/material/snowball_impact_follower", this.transform.position);
    CameraManager.shakeCamera(0.2f, 0.1f);
    string animation = "Snow/hit-back-fall";
    if (follower.SimpleAnimator.Dir > 0 && (double) follower.transform.position.x > (double) this.projectile.Owner.transform.position.x || follower.SimpleAnimator.Dir < 0 && (double) follower.transform.position.x < (double) this.projectile.Owner.transform.position.x)
      animation = "Snow/hit-front-fall";
    follower.TimedAnimation(animation, 2.33333325f, (System.Action) (() => follower.Brain.CompleteCurrentTask()), false);
    BiomeConstants.Instance.EmitSnowImpactVFX(follower.transform.position, 2f);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void HitWolf(Interaction_WolfBase wolf)
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/gethit", this.transform.position);
    CameraManager.shakeCamera(0.2f, 0.1f);
    BiomeConstants.Instance.EmitSnowImpactVFX(wolf.transform.position, 2f);
    wolf.Spank(this.transform.position);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void HitAnimal(Interaction_Ranchable ranchable)
  {
    CameraManager.shakeCamera(0.2f, 0.1f);
    BiomeConstants.Instance.EmitSnowImpactVFX(ranchable.transform.position, 2f);
    ranchable.HitWithSnowball();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public IEnumerator ApplyKnockbackRoutine(GameObject target)
  {
    Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
    if ((UnityEngine.Object) rb == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "PlayerSnowball requires a Rigidbody2D component on the same GameObject.");
    }
    else
    {
      int force = 250;
      float knockbackDuration = 0.3f;
      float f = this.projectile.Angle * ((float) Math.PI / 180f);
      Vector2 direction = new Vector2(Mathf.Cos(f), Mathf.Sin(f)).normalized;
      float elapsed = 0.0f;
      while ((double) elapsed <= (double) knockbackDuration)
      {
        rb.AddForce((float) force * direction);
        elapsed += Time.deltaTime;
        yield return (object) null;
      }
    }
  }
}
