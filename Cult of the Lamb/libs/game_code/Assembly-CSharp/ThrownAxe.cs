// Decompiled with JetBrains decompiler
// Type: ThrownAxe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class ThrownAxe : BaseMonoBehaviour, IHeavyAttackWeapon, ISpellOwning
{
  public GameObject Master;
  [SerializeField]
  public GameObject HeavyContainer;
  [SerializeField]
  public GameObject StandardContainer;
  public Health MasterHealth;
  public StateMachine MasterState;
  public PlayerFarming playerFarming;
  public GameObject AxeSprite;
  public SpriteRenderer AxeSpriteRenderer;
  public SpriteRenderer AxeShadowSpriteRenderer;
  public GameObject weaponOwner;
  public float MaxSpeed = 10f;
  public float Speed;
  public float TargetAngle;
  public float ReturnTime = 1f;
  public float ReturnTimer;
  public float ReturnSpeedDelta = 0.1f;
  public float StartingReturnSpeed = 180f;
  public float ReturnSpeed;
  public float RotationSpeedMultiplier = 1f;
  public EventInstance loopedSound;
  public bool HasResetList;
  public StateMachine state;
  public float TurnSpeed = 5f;
  public float Damage = 1f;
  public Health target;
  public float DamageToNeutral = 10f;
  public float NeutralSplashRadius = 2f;
  public LayerMask unitMask;
  public float ScreenShakeMultiplier = 1f;
  public List<Health> DamagedEnemies = new List<Health>();

  public static void SpawnThrowingAxe(
    PlayerFarming owner,
    Vector3 position,
    float damageMultiplier,
    Sprite AxeImage,
    float Angle,
    float scale = 1f,
    Action<ThrownAxe> callback = null)
  {
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Thrown Axe.prefab", position, Quaternion.identity, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      ThrownAxe component = obj.Result.GetComponent<ThrownAxe>();
      component.Damage = damageMultiplier;
      component.BeginThrow(AxeImage, Angle);
      component.transform.localScale = Vector3.one * scale;
      component.playerFarming = owner;
      Action<ThrownAxe> action = callback;
      if (action == null)
        return;
      action(component);
    }));
  }

  public void OnEnable()
  {
    this.state = this.GetComponent<StateMachine>();
    this.Master = this.state.gameObject;
    this.MasterState = this.state;
    this.MasterHealth = this.Master.GetComponent<Health>();
    this.MasterHealth.OnDie += new Health.DieAction(this.Health_OnDie);
  }

  public void BeginThrow(Sprite AxeImage, float Angle)
  {
    this.loopedSound = AudioManager.Instance.CreateLoop("event:/weapon/axe_heavy/spinning_axe", this.gameObject, true);
    this.state = this.GetComponent<StateMachine>();
    this.unitMask = (LayerMask) ((int) this.unitMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.unitMask = (LayerMask) ((int) this.unitMask | 1 << LayerMask.NameToLayer("Units"));
    float x = this.AxeSprite.transform.localScale.x;
    this.AxeSprite.transform.localScale = new Vector3(2f, 2f, 1f);
    this.AxeSprite.transform.DOScale(x, 0.2f);
    if ((UnityEngine.Object) AxeImage != (UnityEngine.Object) null)
    {
      this.StandardContainer.SetActive(true);
      this.HeavyContainer.SetActive(false);
      this.AxeSpriteRenderer.sprite = AxeImage;
      this.AxeShadowSpriteRenderer.sprite = AxeImage;
    }
    else
    {
      this.StandardContainer.SetActive(false);
      this.HeavyContainer.SetActive(true);
    }
    this.transform.position = this.Master.transform.position;
    this.state.facingAngle = this.TargetAngle = Angle;
    this.MaxSpeed *= UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Axe) ? 1f : 0.75f;
    this.ReturnSpeedDelta *= UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Axe) ? 1f : 2f;
    this.Speed = this.MaxSpeed;
    this.ReturnTimer = this.ReturnTime;
    this.ReturnSpeed = this.StartingReturnSpeed;
    this.state.CURRENT_STATE = StateMachine.State.Moving;
  }

  public void Update()
  {
    switch (this.state.CURRENT_STATE)
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
              this.state.CURRENT_STATE = StateMachine.State.Fleeing;
            }
          }
          if ((double) this.Speed < 0.0)
          {
            this.TargetAngle = Utils.GetAngle(this.playerFarming.transform.position, this.transform.position);
            break;
          }
          break;
        }
        break;
      case StateMachine.State.Fleeing:
        this.Speed -= this.ReturnSpeedDelta * Time.deltaTime;
        this.TargetAngle = Utils.GetAngle(this.playerFarming.transform.position, this.transform.position);
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.playerFarming.transform.position) <= 0.5)
        {
          if ((bool) (UnityEngine.Object) this.playerFarming)
          {
            System.Action onCrownReturn = this.playerFarming.OnCrownReturn;
            if (onCrownReturn != null)
              onCrownReturn();
          }
          if ((bool) (UnityEngine.Object) this.playerFarming && (double) this.playerFarming.playerController.speed <= 0.0)
            this.playerFarming.playerController.unitObject.DoKnockBack((float) (((double) this.TargetAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 0.5f, 0.1f);
          else
            CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.2f);
          this.CloseAxe();
          break;
        }
        break;
    }
    this.AxeSprite.transform.eulerAngles += new Vector3(0.0f, 0.0f, Mathf.Abs(this.RotationSpeedMultiplier) * Time.deltaTime);
    this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (double) this.TurnSpeed * (double) Time.deltaTime * 60.0);
    this.transform.position = this.transform.position + new Vector3(this.Speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime, this.Speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime);
  }

  public GameObject GetOwner() => this.playerFarming.gameObject;

  public void SetOwner(GameObject owner)
  {
    this.playerFarming = owner.GetComponent<PlayerFarming>();
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
    if (!(bool) (UnityEngine.Object) this.MasterHealth)
      return;
    this.MasterHealth.OnDie -= new Health.DieAction(this.Health_OnDie);
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
    if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null) || !this.target.enabled || this.target.team == this.MasterHealth.team && !this.target.IsCharmedEnemy || this.target.untouchable || this.target.invincible || !((UnityEngine.Object) this.target.state == (UnityEngine.Object) null) && this.target.state.CURRENT_STATE == StateMachine.State.Dodging || (this.target.team == Health.Team.Neutral || !this.AxeSprite.gameObject.activeSelf || this.target.invincible) && (this.target.team != Health.Team.Neutral || (double) this.DamageToNeutral <= 0.0))
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
    if (!(bool) (UnityEngine.Object) this.MasterHealth || this.MasterHealth.team != Health.Team.PlayerTeam || !(bool) (UnityEngine.Object) collider || !((UnityEngine.Object) collider.GetComponentInParent<Projectile>() != (UnityEngine.Object) null))
      return;
    foreach (Component component in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.NeutralSplashRadius))
    {
      Projectile componentInParent = component.GetComponentInParent<Projectile>();
      if ((bool) (UnityEngine.Object) componentInParent)
        componentInParent.DestroyProjectile();
    }
  }
}
