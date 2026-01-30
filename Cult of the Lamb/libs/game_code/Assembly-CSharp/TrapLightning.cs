// Decompiled with JetBrains decompiler
// Type: TrapLightning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class TrapLightning : BaseMonoBehaviour
{
  [CompilerGenerated]
  public Health \u003CHealth\u003Ek__BackingField;
  public float ExplosionRadius = 1f;
  public float LightingEnemyDamage = 8f;
  public float LightningDelayMin = 4f;
  public float LightningDelayMax = 8f;
  public float LightningSignpostDuration = 2f;
  public float LightningBarrierAliveTime = 0.3f;
  public float BeamWidthSignpost = 0.1f;
  public float BeamWidthAttack = 0.7f;
  [CompilerGenerated]
  public float \u003CNextLightningCounter\u003Ek__BackingField;
  public string ExplosionSFX = "event:/dlc/dungeon05/trap/lightning_rod/explosion";
  public AsyncOperationHandle<GameObject> loadedSignpostBarrier;
  public AsyncOperationHandle<GameObject> loadedAttackBarrier;
  public TrapLightning.TrapState currentState;
  public TrapLightningProng[] prongs;
  public SimpleStateMachine stateMachine;
  public bool isRoomCleared;
  public const float LIGHTNING_DAMAGE_PLAYER = 1f;

  public Health Health
  {
    get => this.\u003CHealth\u003Ek__BackingField;
    set => this.\u003CHealth\u003Ek__BackingField = value;
  }

  public float NextLightningCounter
  {
    get => this.\u003CNextLightningCounter\u003Ek__BackingField;
    set => this.\u003CNextLightningCounter\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    this.Health = this.GetComponent<Health>();
    this.prongs = this.GetComponentsInChildren<TrapLightningProng>();
    this.stateMachine = new SimpleStateMachine();
    this.SetState((TrapLightning.TrapState) new TrapLightning.IdleState(this));
  }

  public void DebugEnable()
  {
    this.isRoomCleared = false;
    this.ResetLightningCounter();
    this.SetState((TrapLightning.TrapState) new TrapLightning.IdleState(this));
  }

  public void DebugDisable() => this.OnRoomCleared();

  public void OnEnable()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.OnRoomCleared);
  }

  public void OnDisable()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.OnRoomCleared);
  }

  public void StopTrap()
  {
    this.stateMachine.SetState((SimpleState) new TrapLightning.DisabledState(this));
  }

  public void StartTrap()
  {
    this.ResetLightningCounter();
    this.SetState((TrapLightning.TrapState) new TrapLightning.IdleState(this));
  }

  public void OnRoomCleared()
  {
    this.isRoomCleared = true;
    this.stateMachine.SetState((SimpleState) new TrapLightning.DisabledState(this));
  }

  public void Update() => this.stateMachine.Update();

  public void SetState(TrapLightning.TrapState newState)
  {
    this.stateMachine.SetState((SimpleState) newState);
  }

  public void ResetLightningCounter()
  {
    this.NextLightningCounter = Random.Range(this.LightningDelayMin, this.LightningDelayMax);
  }

  public void SetRedCirclesActive(bool state)
  {
    foreach (TrapLightningProng prong in this.prongs)
      prong.SetRedCircleActive(state);
  }

  public void StopChargingEffects()
  {
    foreach (TrapLightningProng prong in this.prongs)
      prong.StopChargingEffect();
  }

  public TrapLightningProng[] GetProngs() => this.prongs;

  public void SpawnBarriers(float duration, float beamWidth, bool disableCollision, int layers)
  {
    for (int index1 = 0; index1 < this.prongs.Length; ++index1)
    {
      if (index1 < this.prongs.Length - 1)
      {
        for (int index2 = 0; index2 < layers; ++index2)
          this.CreateBeam(this.prongs[index1].LightningRoot.transform.position, this.prongs[index1 + 1].LightningRoot.transform.position, disableCollision || index1 != 0, duration, beamWidth);
      }
      else if (this.prongs.Length > 2 && index1 == this.prongs.Length - 1)
      {
        for (int index3 = 0; index3 < layers; ++index3)
          this.CreateBeam(this.prongs[index1].LightningRoot.transform.position, this.prongs[0].LightningRoot.transform.position, index1 != 0, duration, beamWidth);
      }
    }
  }

  public void CreateBeam(
    Vector3 from,
    Vector3 target,
    bool disableCollision,
    float duration,
    float beamWidth)
  {
    int num = (int) Vector3.Distance(from, target) + 2;
    List<Vector3> vector3List = new List<Vector3>();
    vector3List.Add(from);
    for (int index = 1; index < num + 1; ++index)
    {
      float t = (float) index / (float) num;
      Vector3 vector3 = Vector3.Lerp(from, target, t) + (Vector3) Random.insideUnitCircle * 0.15f;
      vector3List.Add(vector3);
    }
    vector3List.Add(target);
    ArrowLightningBeam.CreateBeam(vector3List.ToArray(), true, beamWidth, duration, Health.Team.Team2, (Transform) null, disableCollision, signpostDuration: 0.0f, attackFlags: Health.AttackFlags.Trap);
  }

  public void DoExplosion(Vector3 position)
  {
    Explosion.CreateExplosion(position, Health.Team.Team2, this.Health, this.ExplosionRadius, 1f, this.LightingEnemyDamage, attackFlags: Health.AttackFlags.Trap, playSFX: false);
    AudioManager.Instance.PlayOneShot(this.ExplosionSFX, position);
  }

  public abstract class TrapState : SimpleState
  {
    public TrapLightning parent;

    public TrapState(TrapLightning parent) => this.parent = parent;
  }

  public class IdleState(TrapLightning parent) : TrapLightning.TrapState(parent)
  {
    public override void OnEnter()
    {
      this.parent.ResetLightningCounter();
      this.parent.SetRedCirclesActive(true);
    }

    public override void Update()
    {
      if (!GameManager.RoomActive || this.parent.isRoomCleared || (double) (this.parent.NextLightningCounter -= Time.deltaTime) > 0.0)
        return;
      this.parent.ResetLightningCounter();
      this.parent.SetState((TrapLightning.TrapState) new TrapLightning.SignpostState(this.parent));
    }

    public override void OnExit()
    {
    }
  }

  public class SignpostState : TrapLightning.TrapState
  {
    public TrapLightningProng[] prongs;
    public float signpostCounter;

    public SignpostState(TrapLightning parent)
      : base(parent)
    {
      this.prongs = parent.GetProngs();
      this.signpostCounter = 0.0f;
    }

    public override void OnEnter()
    {
      for (int index = 0; index < this.prongs.Length; ++index)
      {
        this.prongs[index].StartChargingEffect();
        this.parent.SpawnBarriers(this.parent.LightningSignpostDuration, this.parent.BeamWidthSignpost, true, 1);
      }
    }

    public override void Update()
    {
      if (GameManager.RoomActive && !this.parent.isRoomCleared)
      {
        if ((double) (this.signpostCounter += Time.deltaTime) <= (double) this.parent.LightningSignpostDuration)
          return;
        this.parent.SetState((TrapLightning.TrapState) new TrapLightning.AttackState(this.parent));
      }
      else
        this.parent.stateMachine.SetState((SimpleState) new TrapLightning.IdleState(this.parent));
    }

    public override void OnExit()
    {
    }
  }

  public class AttackState : TrapLightning.TrapState
  {
    public TrapLightningProng[] prongs;
    public bool hasExploded;
    public float barrierAliveTimeCounter;

    public AttackState(TrapLightning parent)
      : base(parent)
    {
      this.prongs = parent.GetProngs();
      this.hasExploded = false;
      this.barrierAliveTimeCounter = 0.0f;
    }

    public override void OnEnter()
    {
    }

    public override void Update()
    {
      if (GameManager.RoomActive && !this.parent.isRoomCleared)
      {
        if (!this.hasExploded)
        {
          this.parent.SpawnBarriers(this.parent.LightningBarrierAliveTime, this.parent.BeamWidthAttack, false, 5);
          this.hasExploded = true;
          for (int index = 0; index < this.prongs.Length; ++index)
          {
            TrapLightningProng prong = this.prongs[index];
            prong.StopChargingEffect();
            prong.PlayImpactEffect();
            this.parent.DoExplosion(prong.transform.position);
          }
        }
        if (!this.hasExploded || (double) (this.barrierAliveTimeCounter += Time.deltaTime) < (double) this.parent.LightningBarrierAliveTime)
          return;
        this.parent.SetState((TrapLightning.TrapState) new TrapLightning.IdleState(this.parent));
      }
      else
        this.parent.stateMachine.SetState((SimpleState) new TrapLightning.IdleState(this.parent));
    }

    public override void OnExit()
    {
    }
  }

  public class DisabledState(TrapLightning parent) : TrapLightning.TrapState(parent)
  {
    public override void OnEnter()
    {
      this.parent.SetRedCirclesActive(false);
      this.parent.StopChargingEffects();
    }

    public override void Update()
    {
    }

    public override void OnExit()
    {
    }
  }
}
