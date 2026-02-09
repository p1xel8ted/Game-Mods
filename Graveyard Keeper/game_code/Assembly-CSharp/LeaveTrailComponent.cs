// Decompiled with JetBrains decompiler
// Type: LeaveTrailComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LeaveTrailComponent
{
  public BaseCharacterComponent _ch;
  public Vector2 _prev_pos = Vector2.zero;
  public Vector2 _prev_pos_sound = Vector2.zero;
  public Ground.GroudType _ground_under;
  public Ground.GroudType _trail_type;
  public Ground.GroudType _dirty_type;
  public float _dirty_amount;
  public Vector2 _dir = Vector2.zero;
  public string _preset_id;
  public TrailDefinition _trail_definition;
  public bool _is_left_foot = true;
  public bool _update_trails = true;
  public const float LEAVE_TRAIL_DISTANCE = 370f;
  public const float SOUND_TRAIL_DISTANCE = 7000f;
  public static List<TrailObject> _all_trails = new List<TrailObject>();

  public bool UpdateTrails
  {
    get => this._update_trails;
    set => this._update_trails = value;
  }

  public LeaveTrailComponent(BaseCharacterComponent ch, string preset_id)
  {
    this._ch = ch;
    this._preset_id = preset_id;
    this._trail_definition = Resources.Load<TrailDefinition>("Trails/" + preset_id);
    if (!((Object) this._trail_definition == (Object) null))
      return;
    Debug.LogError((object) ("Trail preset not found: " + preset_id));
  }

  public void CustomUpdate()
  {
    if (!this._update_trails)
      return;
    Vector2 position = (Vector2) this._ch.wgo.transform.position;
    this._dir = this._prev_pos - position;
    if ((double) (this._prev_pos_sound - position).sqrMagnitude > 7000.0)
    {
      TrailTypeDefinition byType = this._trail_definition.GetByType(this._ground_under);
      this._prev_pos_sound = position;
      if ((Object) byType != (Object) null && byType.sound != "[None]")
        DarkTonic.MasterAudio.MasterAudio.PlaySound(byType.sound);
    }
    TrailTypeDefinition byType1 = this._trail_definition.GetByType(this._trail_type);
    float num = 370f;
    if ((Object) byType1 != (Object) null && byType1.custom_trail_dist)
      num = byType1.leave_trail_dist;
    if ((double) this._dir.sqrMagnitude < (double) num)
      return;
    this._prev_pos = position;
    this._ground_under = this._ch.GetGroundTypeUnderCharacter();
    if (this._ground_under != this._dirty_type && this.SteppedOnANewSurface(this._ground_under))
      return;
    if (this._trail_type == Ground.GroudType.None)
    {
      this._trail_type = this._ground_under;
      this._dirty_amount = 1f;
      if (this._trail_type == Ground.GroudType.None)
        return;
    }
    this.LeaveTrail();
  }

  public bool SteppedOnANewSurface(Ground.GroudType ground_type)
  {
    this._trail_type = this._dirty_type;
    this._dirty_amount = 1f;
    this._dirty_type = ground_type;
    if (ground_type != Ground.GroudType.Rug)
      return false;
    this._dirty_amount = 0.0f;
    return true;
  }

  public void LeaveTrail()
  {
    float num = 0.9f;
    TrailTypeDefinition byType = this._trail_definition.GetByType(this._trail_type);
    if ((Object) byType != (Object) null && byType.custom_trail_decrease)
      num = byType.trail_decrease;
    if (this._trail_type != this._ground_under)
      this._dirty_amount *= num;
    if ((double) this._dirty_amount < 0.10000000149011612)
    {
      this._dirty_amount = 1f;
      this._trail_type = this._ground_under;
    }
    else
    {
      if (this._trail_type == Ground.GroudType.None)
        return;
      if ((Object) byType == (Object) null)
      {
        this._dirty_amount = 0.0f;
      }
      else
      {
        bool flip;
        UnityEngine.Sprite byDirection = byType.GetByDirection(this._dir, this._is_left_foot, out flip);
        if ((Object) byDirection == (Object) null)
          return;
        TrailObject trailObject = TrailObject.Spawn(this._prev_pos, byDirection, flip, this._ch.cur_environment == BaseCharacterComponent.Environment.Outside);
        LeaveTrailComponent._all_trails.Add(trailObject);
        if ((Object) trailObject == (Object) null)
          return;
        this._is_left_foot = !this._is_left_foot;
        trailObject.SetColor(byType.color, this._dirty_amount);
      }
    }
  }

  public string GetDescriptionString()
  {
    return $"Trail type: {this._trail_type.ToString()}\nAmount: {this._dirty_amount.ToString()}\nCur dirt: {this._dirty_type.ToString()}\nUnder: {this._ground_under.ToString()}";
  }

  public static void RemoveAllTrailsFromTheScene()
  {
    for (int index = 0; index < LeaveTrailComponent._all_trails.Count; ++index)
      Object.Destroy((Object) LeaveTrailComponent._all_trails[index].gameObject);
    LeaveTrailComponent._all_trails.Clear();
  }

  public static void OnTrailObjectDestroyed(TrailObject o)
  {
    LeaveTrailComponent._all_trails.Remove(o);
  }
}
