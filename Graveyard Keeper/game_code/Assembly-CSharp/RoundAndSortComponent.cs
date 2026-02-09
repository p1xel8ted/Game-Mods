// Decompiled with JetBrains decompiler
// Type: RoundAndSortComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RoundAndSortComponent : SnapToGridComponent
{
  public const int GROUND_Z_SHIFT = 2000;
  public const float default_y_k = 20f;
  public const float default_x_k = 0.02f;
  public float _force_z;
  public bool _force_z_mode;
  public bool _tried_to_find_zero_level_spr;
  public SpriteRenderer _zero_level_spr;
  public SpriteRenderer _spr_renderer;
  public bool _spr_renderer_set;
  public bool do_round = true;
  public WorldObjectPart _world_part;
  public float floor_line;
  public Vector3 _prev_pos = new Vector3(97899f, 97899f, 97899f);
  public Vector3 _prev_pos_editor = new Vector3(97899f, 97899f, 97899f);
  public static Dictionary<int, bool> _sprite_layers_are_ground = new Dictionary<int, bool>();
  public ChunkedGameObject _chnk;
  public bool _pos_dirty = true;
  public bool use_late_update = true;
  public bool never_disable;
  public float _delta_pos;

  public override void Update()
  {
    base.Update();
    if (this.use_late_update)
      return;
    this.DoUpdateStuff();
  }

  public void LateUpdate()
  {
    if (!this.use_late_update)
      return;
    this.DoUpdateStuff();
  }

  public void DoUpdateStuff(bool force = false)
  {
    Transform transform = this.transform;
    Vector3 vector3_1;
    if (!Application.isPlaying)
    {
      vector3_1 = transform.position;
      this._delta_pos = (this._prev_pos_editor - vector3_1).sqrMagnitude;
      this._prev_pos_editor = vector3_1;
      this._pos_dirty = false;
    }
    else
    {
      vector3_1 = transform.localPosition;
      this._delta_pos = (this._prev_pos - vector3_1).sqrMagnitude;
      this._prev_pos = vector3_1;
    }
    if (this._delta_pos.EqualsTo(0.0f, 1f / 1000f) && !force && !this._pos_dirty)
      return;
    this._pos_dirty = false;
    float num1 = 20f;
    float num2 = 0.0f;
    bool flag = false;
    SpriteRenderer zeroLevelSprite = this.GetZeroLevelSprite();
    if (!this._spr_renderer_set)
    {
      this._spr_renderer = this.GetComponent<SpriteRenderer>();
      this._spr_renderer_set = true;
    }
    Vector3 pos = Application.isPlaying ? transform.position : vector3_1;
    Vector3 vector3_2 = pos / 96f;
    if (RoundAndSortComponent.DoesSpriteBelongToGround(zeroLevelSprite))
    {
      vector3_2.z = GroundObject.GetGroundZ((Vector2) pos);
      flag = true;
      if ((Object) this._spr_renderer != (Object) null)
        this._spr_renderer.sortingOrder = RoundAndSortComponent.GetSpriteOrderN((Vector2) pos);
    }
    else
    {
      if ((Object) this._world_part == (Object) null)
        this._world_part = this.GetComponentInChildren<WorldObjectPart>();
      float floorLine = this.floor_line;
      if ((bool) (Object) this._world_part)
        floorLine += (float) this._world_part.floor_line;
      float num3 = floorLine / 96f;
      if ((Object) this._spr_renderer != (Object) null)
        this._spr_renderer.sortingOrder = 0;
      vector3_2.z = this._force_z_mode ? this._force_z : (float) (((double) vector3_2.y + (double) num3) * (double) num1 + (double) vector3_2.x * 0.019999999552965164) + num2;
    }
    if (flag)
      vector3_2.z += (float) this.fine_tune_z / 100f;
    vector3_2.z /= 96f;
    transform.position = vector3_2 * 96f;
  }

  public static bool DoesSpriteBelongToGround(SpriteRenderer zspr)
  {
    if ((Object) zspr == (Object) null)
      return false;
    int sortingLayerId = zspr.sortingLayerID;
    if (RoundAndSortComponent._sprite_layers_are_ground.ContainsKey(sortingLayerId))
      return RoundAndSortComponent._sprite_layers_are_ground[sortingLayerId];
    string sortingLayerName = zspr.sortingLayerName;
    bool ground = sortingLayerName.Contains("behind ground") || sortingLayerName.Contains("ground_") || sortingLayerName == "drops" || sortingLayerName.Contains("on_ground");
    RoundAndSortComponent._sprite_layers_are_ground.Add(sortingLayerId, ground);
    return ground;
  }

  public void TryApplySortingToParentSprites(int order)
  {
  }

  public SpriteRenderer GetZeroLevelSprite()
  {
    if (!Application.isPlaying)
      return this.GetComponent<SpriteRenderer>();
    if (this._tried_to_find_zero_level_spr)
      return this._zero_level_spr;
    this._tried_to_find_zero_level_spr = true;
    this._zero_level_spr = this.GetComponent<SpriteRenderer>();
    return this._zero_level_spr;
  }

  public void SetZ()
  {
    this._force_z_mode = false;
    this.Update();
  }

  public void SetZ(float z)
  {
    this._force_z_mode = true;
    this._force_z = z;
    this.Update();
  }

  public void OnChangedSprite() => this._world_part = (WorldObjectPart) null;

  public static int GetSpriteOrderN(Vector2 pos)
  {
    return -Mathf.RoundToInt((float) ((double) pos.y / 96.0 * 10.0));
  }

  public static void DisableComponentOnStaticObjects()
  {
    RoundAndSortComponent[] componentsInChildren = MainGame.me.world_root.GetComponentsInChildren<RoundAndSortComponent>(true);
    Debug.Log((object) ("DisableComponentOnStaticObjects, count = " + componentsInChildren.Length.ToString()));
    foreach (RoundAndSortComponent andSortComponent in componentsInChildren)
    {
      if (!andSortComponent.never_disable)
      {
        andSortComponent._chnk = andSortComponent.GetComponent<ChunkedGameObject>();
        if ((Object) andSortComponent._chnk != (Object) null)
          andSortComponent._chnk.RecalculateChunk();
        WorldGameObject component = andSortComponent.GetComponent<WorldGameObject>();
        if ((Object) component == (Object) null)
          andSortComponent.enabled = false;
        else if (!component.is_player && !component.components.character.enabled)
          andSortComponent.enabled = false;
      }
    }
  }

  public void MarkPositionDirty() => this._pos_dirty = true;
}
