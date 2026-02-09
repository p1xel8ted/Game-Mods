// Decompiled with JetBrains decompiler
// Type: BaseCharacterIdle
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BaseCharacterIdle : WorldObjectPartComponent
{
  public float radius = 3f;
  public float wait_time = 3f;
  [Range(0.0f, 1f)]
  public float wait_time_range = 0.2f;
  [Range(0.0f, 1f)]
  public float radius_range = 0.8f;
  public Vector2 start_pos;
  public BaseCharacterComponent ch;
  public float delay;
  [NonSerialized]
  public BaseCharacterIdle.IdleState _state;

  public BaseCharacterIdle.IdleState state => this._state;

  public virtual void StartIdle()
  {
    this.enabled = true;
    if (this._state != BaseCharacterIdle.IdleState.None)
      return;
    this.ch = this.wgo.components.character;
    this.ch.idle_used = true;
    MobSpawner spawner = this.wgo.components.character.spawner;
    this.start_pos = (UnityEngine.Object) spawner == (UnityEngine.Object) null ? this.wgo.pos : (Vector2) spawner.transform.position;
    Debug.Log((object) $"StartIdle, start_pos = {this.start_pos.ToString()}, spawner is null: {((UnityEngine.Object) spawner == (UnityEngine.Object) null).ToString()}");
    this.Wait();
  }

  public virtual void UpdateIdle(float delta_time)
  {
    if (!this.NeedStateChange(delta_time))
      return;
    if (this._state == BaseCharacterIdle.IdleState.Waiting)
      this.ChangeState();
  }

  public bool NeedStateChange(float delta_time)
  {
    if (this._state == BaseCharacterIdle.IdleState.None)
      return false;
    this.delay -= delta_time;
    if ((double) this.delay > 0.0)
      return false;
    this.delay = 0.0f;
    return true;
  }

  public virtual void StopIdle()
  {
    if (this._state == BaseCharacterIdle.IdleState.None)
      return;
    switch (this._state)
    {
      case BaseCharacterIdle.IdleState.Waiting:
        this.delay = 0.0f;
        break;
      case BaseCharacterIdle.IdleState.Moving:
        if (this.wgo.components.character.movement_state != MovementComponent.MovementState.AnimCurve)
        {
          this.wgo.components.character.StopMovement();
          break;
        }
        break;
    }
    this._state = BaseCharacterIdle.IdleState.None;
  }

  public virtual void ChangeState()
  {
    switch (this._state)
    {
      case BaseCharacterIdle.IdleState.Waiting:
        this.MoveToRandomPos();
        break;
      case BaseCharacterIdle.IdleState.Moving:
        this.Wait();
        break;
    }
  }

  public virtual void MoveToRandomPos()
  {
  }

  public virtual void Wait(bool stop = true)
  {
    if (stop)
      this.ch.StopMovement();
    this._state = BaseCharacterIdle.IdleState.Waiting;
    this.delay = this.wait_time * (1f + UnityEngine.Random.Range(-this.wait_time_range, this.wait_time_range));
  }

  public Vector2 GetNextDest()
  {
    bool flag = true;
    int num = 5;
    Vector2 point = this.start_pos;
    while (flag)
    {
      float f = UnityEngine.Random.Range(-3.14159274f, 3.14159274f);
      point = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
      point = point * (float) (((double) this.radius_range + (1.0 - (double) this.radius_range) * (double) UnityEngine.Random.value) * (double) this.radius * 96.0) + this.start_pos;
      flag = (bool) (UnityEngine.Object) Physics2D.OverlapPoint(point, 1) && num > 0;
      if (flag && num == 0)
      {
        flag = false;
        point = this.start_pos;
      }
    }
    if (!(point - this.start_pos).magnitude.EqualsTo(0.0f))
      return point;
    Debug.LogError((object) "No free point for idle movement", (UnityEngine.Object) this.wgo.gameObject);
    return Vector2.zero;
  }

  public override void UpdateComponent(float delta_time) => this.UpdateIdle(delta_time);

  public BaseCharacterIdle.SerializableCharacterIdle Serialize()
  {
    MobSpawner spawner = this.wgo.components.character.spawner;
    return new BaseCharacterIdle.SerializableCharacterIdle()
    {
      has_spawner = (UnityEngine.Object) spawner != (UnityEngine.Object) null,
      start_pos = this.start_pos,
      state = this._state,
      delay = this.delay,
      spawner_coords = (UnityEngine.Object) spawner == (UnityEngine.Object) null ? Vector2.zero : (Vector2) spawner.transform.position
    };
  }

  public void Deserialize(BaseCharacterIdle.SerializableCharacterIdle data)
  {
    this.start_pos = data.start_pos;
    this._state = data.state;
    this.delay = data.delay;
    if (!data.has_spawner)
      return;
    this.wgo.components.character.spawner = WorldMap.GetSpawnerByCoords(data.spawner_coords);
  }

  public enum IdleState
  {
    None,
    Waiting,
    Moving,
  }

  [Serializable]
  public class SerializableCharacterIdle
  {
    public Vector2 spawner_coords;
    public Vector2 start_pos;
    public bool has_spawner;
    public BaseCharacterIdle.IdleState state;
    public float delay;
  }
}
