// Decompiled with JetBrains decompiler
// Type: TrailObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TrailObject : MonoBehaviour
{
  public SpriteRenderer _spr;
  public static Transform trails_root;
  public static TrailObject _prefab;
  public bool _is_outside = true;
  public const float DEGRADE_SPEED_OUTSIDE = 0.1f;
  public const float DEGRADE_SPEED_INSIDE = 2.5f;
  public const float DEGRADE_SPEED_OUTSIDE_RAIN = 10f;
  public float _alpha = 1f;
  public float _real_alpha = 1f;
  public bool _degrading;
  public float _last_time;

  public static TrailObject Spawn(Vector2 pos, UnityEngine.Sprite spr, bool flip, bool is_outside)
  {
    if ((Object) spr == (Object) null)
    {
      Debug.LogError((object) "Trial spr is null");
      return (TrailObject) null;
    }
    if ((Object) TrailObject._prefab == (Object) null)
    {
      TrailObject._prefab = Resources.Load<TrailObject>("Trails/trail prefab");
      if ((Object) TrailObject._prefab == (Object) null)
        Debug.LogError((object) "Couldn't load trails prefab");
    }
    if ((Object) TrailObject.trails_root == (Object) null)
    {
      TrailObject.trails_root = new GameObject("Trails").transform;
      TrailObject.trails_root.SetParent(MainGame.me.world_root, false);
    }
    TrailObject trailObject = Object.Instantiate<TrailObject>(TrailObject._prefab, TrailObject.trails_root, false);
    trailObject.transform.position = (Vector3) pos;
    trailObject.Init(spr, flip, is_outside);
    GroundObject componentInChildren = trailObject?.GetComponentInChildren<GroundObject>();
    if (!((Object) componentInChildren != (Object) null))
      return trailObject;
    componentInChildren.can_move = false;
    return trailObject;
  }

  public void Init(UnityEngine.Sprite spr, bool flip, bool is_outside)
  {
    this._spr = this.gameObject.AddComponentNotDuplicate<SpriteRenderer>();
    this._spr.sprite = spr;
    this._spr.flipX = flip;
    this._is_outside = is_outside;
    this._last_time = Time.time;
    this._degrading = true;
  }

  public void SetColor(Color c, float alpha = 1f)
  {
    c.a = alpha;
    this._spr.color = c;
    this._alpha = alpha;
    this._real_alpha = alpha;
  }

  public void Update()
  {
    if (!this._degrading)
      return;
    float num = 0.1f;
    if (!this._is_outside)
      num = 2.5f;
    else if (EnvironmentEngine.me.is_rainy)
      num = 10f;
    this._alpha -= (float) (((double) Time.time - (double) this._last_time) * (double) num / 100.0);
    this._last_time = Time.time;
    if ((double) Mathf.Abs(this._real_alpha - this._alpha) > 0.03)
    {
      this._real_alpha = this._alpha;
      this._spr.color.SetAlpha(this._real_alpha);
    }
    if ((double) this._real_alpha >= 0.05)
      return;
    this._degrading = false;
    LeaveTrailComponent.OnTrailObjectDestroyed(this);
    Object.Destroy((Object) this.gameObject);
  }
}
