// Decompiled with JetBrains decompiler
// Type: MineLighting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class MineLighting : BaseMonoBehaviour
{
  public float SetupTime = 1f;
  public float ExplodeDelay = 0.5f;
  public float LightningEnemyDamage = 4f;
  public float LightningExpansionSpeed = 5f;
  public string ArmedSFX = string.Empty;
  public string TriggedSFX = string.Empty;
  public string ExplodeSFX = string.Empty;
  public MineLighting.MineLightningStateMachine logicStateMachine;
  public const int LIGHTNING_DAMAGE_PLAYER = 1;

  public void Awake() => this.SetupStateMachine();

  public void Update() => this.logicStateMachine?.Update();

  public void SetupStateMachine()
  {
    this.logicStateMachine = new MineLighting.MineLightningStateMachine();
    this.logicStateMachine.SetParent(this);
  }

  public void PlaceMine(Vector3 position)
  {
    this.transform.position = position;
    this.logicStateMachine.SetState((SimpleState) new MineLighting.SetupState());
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    UnitObject component = other.GetComponent<UnitObject>();
    if (!((Object) component != (Object) null) || component.health.team != Health.Team.PlayerTeam || !(this.logicStateMachine?.GetCurrentState().GetType() == typeof (MineLighting.ArmedState)))
      return;
    this.logicStateMachine.SetState((SimpleState) new MineLighting.TriggeredState());
  }

  public class MineLightningStateMachine : SimpleStateMachine
  {
    [CompilerGenerated]
    public MineLighting \u003CParent\u003Ek__BackingField;

    public MineLighting Parent
    {
      get => this.\u003CParent\u003Ek__BackingField;
      set => this.\u003CParent\u003Ek__BackingField = value;
    }

    public void SetParent(MineLighting parent) => this.Parent = parent;
  }

  public class SetupState : SimpleState
  {
    public MineLighting parent;
    public float progress;

    public override void OnEnter()
    {
      this.parent = ((MineLighting.MineLightningStateMachine) this.parentStateMachine).Parent;
    }

    public override void Update()
    {
      this.progress += Time.deltaTime;
      if ((double) this.progress < (double) this.parent.SetupTime)
        return;
      this.parentStateMachine.SetState((SimpleState) new MineLighting.ArmedState());
    }

    public override void OnExit()
    {
    }
  }

  public class ArmedState : SimpleState
  {
    public MineLighting parent;

    public override void OnEnter()
    {
      this.parent = ((MineLighting.MineLightningStateMachine) this.parentStateMachine).Parent;
      if (string.IsNullOrEmpty(this.parent.ArmedSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.ArmedSFX, this.parent.transform.position);
    }

    public override void Update()
    {
    }

    public override void OnExit()
    {
    }
  }

  public class TriggeredState : SimpleState
  {
    public MineLighting parent;
    public float progress;

    public override void OnEnter()
    {
      this.parent = ((MineLighting.MineLightningStateMachine) this.parentStateMachine).Parent;
      if (string.IsNullOrEmpty(this.parent.TriggedSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.TriggedSFX, this.parent.transform.position);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime;
      if ((double) this.progress < (double) this.parent.ExplodeDelay)
        return;
      this.parentStateMachine.SetState((SimpleState) new MineLighting.ExplodeState());
    }

    public override void OnExit()
    {
    }
  }

  public class ExplodeState : SimpleState
  {
    public MineLighting parent;

    public override void OnEnter()
    {
      this.parent = ((MineLighting.MineLightningStateMachine) this.parentStateMachine).Parent;
      if (!string.IsNullOrEmpty(this.parent.ExplodeSFX))
        AudioManager.Instance.PlayOneShot(this.parent.ExplodeSFX, this.parent.transform.position);
      LightningRingExplosion.CreateExplosion(this.parent.transform.position, Health.Team.Team2, (Health) null, this.parent.LightningExpansionSpeed, 1f, this.parent.LightningEnemyDamage);
      Object.Destroy((Object) this.parent.gameObject);
    }

    public override void Update()
    {
    }

    public override void OnExit()
    {
    }
  }
}
