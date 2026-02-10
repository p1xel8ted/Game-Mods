// Decompiled with JetBrains decompiler
// Type: ThrownAxe_Enemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class ThrownAxe_Enemy : MonoBehaviour
{
  public Transform AxeSpriteContainer;
  public SpriteRenderer AxeSpriteRenderer;
  public SpriteRenderer AxeShadowSpriteRenderer;
  public float MaxSpeed = 10f;
  public float Speed;
  public float TargetAngle;
  public float ReturnTimer;
  public float ReturnSpeedDelta = 0.1f;
  public float StartingReturnSpeed = 180f;
  public float ReturnSpeed;
  public float RotationSpeedMultiplier = 1f;
  public float TurnSpeed = 5f;
  public float DamageToNeutral = 10f;
  public float NeutralSplashRadius = 2f;
  public float ScreenShakeMultiplier = 1f;
  public UnitObject unitObject;
  public GameObject masterAxe;
  public Health axeHealth;
  public StateMachine axeState;
  public EventInstance loopedSound;
  public Health target;
  public LayerMask unitMask;
  public List<Health> DamagedEnemies = new List<Health>();
  public bool HasResetList;
  public float Damage = 1f;
  public float returnTime;
  public const string PREFAB_ADDRESS = "Assets/Prefabs/DLC_ThrownAxe_Enemy.prefab";

  public static void SpawnThrowingAxe(
    UnitObject owner,
    Vector3 position,
    float angle,
    float returnTime = 0.2f,
    float scale = 1f,
    Action<ThrownAxe_Enemy> callback = null)
  {
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/DLC_ThrownAxe_Enemy.prefab", position, Quaternion.identity, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      ThrownAxe_Enemy component = obj.Result.GetComponent<ThrownAxe_Enemy>();
      component.transform.localScale = Vector3.one * scale;
      component.unitObject = owner;
      component.returnTime = returnTime;
      component.BeginThrow(angle);
      Action<ThrownAxe_Enemy> action = callback;
      if (action == null)
        return;
      action(component);
    }));
  }

  public void OnEnable()
  {
    this.axeState = this.GetComponent<StateMachine>();
    this.masterAxe = this.axeState.gameObject;
    this.axeHealth = this.masterAxe.GetComponent<Health>();
    this.axeHealth.OnDie += new Health.DieAction(this.Health_OnDie);
  }

  public void BeginThrow(float angle)
  {
    this.loopedSound = AudioManager.Instance.CreateLoop("event:/weapon/axe_heavy/spinning_axe", this.gameObject, true);
    this.axeState = this.GetComponent<StateMachine>();
    this.unitMask = (LayerMask) ((int) this.unitMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.unitMask = (LayerMask) ((int) this.unitMask | 1 << LayerMask.NameToLayer("Units"));
    float x = this.AxeSpriteContainer.localScale.x;
    this.AxeSpriteContainer.localScale = new Vector3(2f, 2f, 1f);
    this.AxeSpriteContainer.DOScale(x, 0.2f);
    this.transform.position = this.masterAxe.transform.position;
    this.axeState.facingAngle = this.TargetAngle = angle;
    this.Speed = this.MaxSpeed;
    this.ReturnTimer = this.returnTime;
    this.ReturnSpeed = this.StartingReturnSpeed;
    this.axeState.CURRENT_STATE = StateMachine.State.Moving;
  }

  public void Update()
  {
    switch (this.axeState.CURRENT_STATE)
    {
      case StateMachine.State.Moving:
        if ((double) (this.ReturnTimer -= Time.deltaTime) < 0.0)
        {
          if ((double) this.Speed > -(double) this.MaxSpeed)
          {
            this.ReturnSpeed += this.ReturnSpeedDelta * Time.deltaTime;
            double speed = (double) this.Speed;
            this.Speed = this.MaxSpeed * Mathf.Sin(this.ReturnSpeed);
            if (speed > 0.0 && (double) this.Speed <= 0.0)
            {
              this.DamagedEnemies.Clear();
              this.HasResetList = true;
            }
            if (57.295780181884766 * (double) Mathf.Sin(this.ReturnSpeed) <= -45.0)
            {
              if (!this.HasResetList)
                this.DamagedEnemies.Clear();
              this.axeState.CURRENT_STATE = StateMachine.State.Fleeing;
            }
          }
          if ((double) this.Speed < 0.0)
          {
            this.TargetAngle = Utils.GetAngle(this.unitObject.transform.position, this.transform.position);
            break;
          }
          break;
        }
        break;
      case StateMachine.State.Fleeing:
        this.Speed -= this.ReturnSpeedDelta * Time.deltaTime;
        this.TargetAngle = Utils.GetAngle(this.unitObject.transform.position, this.transform.position);
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.unitObject.transform.position) <= 0.5)
        {
          this.CloseAxe();
          break;
        }
        break;
    }
    this.AxeSpriteContainer.eulerAngles += new Vector3(0.0f, 0.0f, Mathf.Abs(this.RotationSpeedMultiplier) * Time.deltaTime);
    this.axeState.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.axeState.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.axeState.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (double) this.TurnSpeed * (double) Time.deltaTime * 60.0);
    this.transform.position += new Vector3(this.Speed * Mathf.Cos(this.axeState.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime, this.Speed * Mathf.Sin(this.axeState.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime);
  }

  public void CloseAxe()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    AudioManager.Instance.PlayOneShot("event:/weapon/axe_heavy/catch_axe", this.transform.position);
    this.DamagedEnemies.Clear();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.CloseAxe();
  }

  public void OnDisable() => this.CloseAxe();

  public void OnDestroy()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    if (!(bool) (UnityEngine.Object) this.axeHealth)
      return;
    this.axeHealth.OnDie -= new Health.DieAction(this.Health_OnDie);
  }

  public void BiomeGenerator_OnBiomeChangeRoom() => this.CloseAxe();

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if ((UnityEngine.Object) collider == (UnityEngine.Object) null || !this.gameObject.activeSelf)
      return;
    this.target = collider.gameObject.GetComponent<Health>();
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null || this.DamagedEnemies.Contains(this.target))
      return;
    this.DamagedEnemies.Add(this.target);
    if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null) || !this.target.enabled || this.target.team == this.axeHealth.team && !this.target.IsCharmedEnemy || this.target.untouchable || this.target.invincible || !((UnityEngine.Object) this.target.state == (UnityEngine.Object) null) && this.target.state.CURRENT_STATE == StateMachine.State.Dodging || (this.target.team == Health.Team.Neutral || !this.AxeSpriteContainer.gameObject.activeSelf || this.target.invincible) && (this.target.team != Health.Team.Neutral || (double) this.DamageToNeutral <= 0.0))
      return;
    Health.AttackTypes AttackType = Health.AttackTypes.Melee;
    if (this.target.HasShield)
      AttackType = Health.AttackTypes.Heavy;
    if (!this.target.DealDamage(this.target.team == Health.Team.Neutral ? this.DamageToNeutral : this.Damage, this.gameObject, this.transform.position, AttackType: AttackType, AttackFlags: Health.AttackFlags.Penetration))
      CameraManager.shakeCamera(0.2f * this.ScreenShakeMultiplier, Utils.GetAngle(this.transform.position, this.target.transform.position));
    else
      CameraManager.shakeCamera(0.5f * this.ScreenShakeMultiplier, Utils.GetAngle(this.transform.position, this.target.transform.position));
    if (this.target.team == Health.Team.Neutral && (double) this.NeutralSplashRadius > 0.0)
    {
      foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.NeutralSplashRadius, (int) this.unitMask))
      {
        Health component2 = component1.GetComponent<Health>();
        if ((bool) (UnityEngine.Object) component2 && component2.team == Health.Team.Neutral)
          component2.DealDamage(this.DamageToNeutral, this.gameObject, this.transform.position);
      }
    }
    AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
    if (!(bool) (UnityEngine.Object) this.axeHealth || this.axeHealth.team != Health.Team.PlayerTeam || !(bool) (UnityEngine.Object) collider || !((UnityEngine.Object) collider.GetComponentInParent<Projectile>() != (UnityEngine.Object) null))
      return;
    foreach (Component component in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.NeutralSplashRadius))
    {
      Projectile componentInParent = component.GetComponentInParent<Projectile>();
      if ((bool) (UnityEngine.Object) componentInParent)
        componentInParent.DestroyProjectile();
    }
  }
}
