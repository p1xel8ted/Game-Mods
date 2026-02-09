// Decompiled with JetBrains decompiler
// Type: ObjectDynamicShadowChild
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ObjectDynamicShadowChild : MonoBehaviour
{
  public Material mat;
  public SpriteRenderer _spr;
  public bool _spr_is_set;
  [HideInInspector]
  public float shadow_alpha = 1f;
  public int shadow_n;
  public float _angle;
  public float _skew;
  public bool visible = true;
  public const int LIGHT_COLLIDER_LAYER = 12;
  public Transform _tf;
  public bool _tf_set;
  public bool is_mirrored;
  public const bool SUN_CASTS_SHADOW = true;

  public void SetShadow(Vector2? light_pos, bool is_mirrored_x, Vector3 pos)
  {
    this.CheckSpriteCachedReference();
    if (!light_pos.HasValue)
    {
      this._spr.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
    else
    {
      Vector2 vector2 = light_pos.Value - (Vector2) pos;
      if (!is_mirrored_x)
      {
        this._angle = Mathf.Atan2(vector2.y, vector2.x) + LazyConsts.PI_DIV_2;
      }
      else
      {
        this._angle = Mathf.Atan2(vector2.y, vector2.x) + LazyConsts.PI_DIV_2;
        if ((double) this._angle < 0.0)
          this._angle += LazyConsts.PI2;
        if ((double) this._angle > (double) LazyConsts.PI2)
          this._angle -= LazyConsts.PI2;
      }
      this.SetShadowAngle(this._angle, vector2.magnitude);
    }
  }

  public void SetShadowAngle(float angle, float distance, float? override_alpha = null, float vert_scale = 1f)
  {
    if (this.shadow_n == -1)
    {
      Debug.LogError((object) "Couldn't set shadow for n = -1");
    }
    else
    {
      this.CheckSpriteCachedReference();
      this._angle = angle;
      float num = (float) ((double) override_alpha ?? (double) Mathf.Max(0.04f, Mathf.Min(1f, 150f / distance)) * (double) TimeOfDay.shadow_alpha_k);
      this._spr.color = this._spr.color with
      {
        a = this.shadow_alpha * num
      };
      if (!this._tf_set)
      {
        this._tf = this.transform;
        this._tf_set = true;
      }
      this._tf.eulerAngles = new Vector3(0.0f, 0.0f, this._angle * 57.29578f);
    }
  }

  public void CheckSpriteCachedReference()
  {
    if (this._spr_is_set)
      return;
    this._spr = this.gameObject.GetComponent<SpriteRenderer>();
    this._spr_is_set = true;
    this.mat = this._spr.sharedMaterial;
  }

  public virtual void SetShadowSprite(UnityEngine.Sprite spr)
  {
    this.GetComponent<SpriteRenderer>().sprite = spr;
  }

  public void SetShadowByNumber(
    ObjectDynamicShadow getlight_object,
    int shadow_n,
    Vector2 pos,
    bool is_mirrored)
  {
    if (shadow_n == 0 && Application.isPlaying)
      this.SetShadowAngle(TimeOfDay.me.GetTimeK() * LazyConsts.PI2, 0.1f, new float?((1f - TimeOfDay.shadow_alpha_k) * TimeOfDay.global_shadows_alpha), (float) ((double) Mathf.Abs(TimeOfDay.me.time_of_day) / 4.0 + 0.75));
    else
      this.SetShadow(getlight_object.GetLight(Application.isPlaying ? shadow_n - 1 : shadow_n), getlight_object.is_mirrored, (Vector3) pos);
  }
}
