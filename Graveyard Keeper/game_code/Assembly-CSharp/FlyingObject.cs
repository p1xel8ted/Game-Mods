// Decompiled with JetBrains decompiler
// Type: FlyingObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

#nullable disable
public class FlyingObject : MonoBehaviour
{
  public SpriteRenderer spr;
  public UI2DSprite gui_spr;
  public Transform dest;
  public float speed = 1f;
  public bool _flying;
  public bool _active;
  public float? _override_duration;
  public object _obj;
  public System.Action on_reached_dest;
  public System.Action _on_late_update;
  public const float OVERHEAD_Y_BOUNCE_HEIGHT = 0.043f;

  public static FlyingObject CreateBuffFlyingObject(
    BuffDefinition buff,
    Vector2 world_pos,
    float? duration = null)
  {
    Debug.Log((object) ("CreateBuffFlyingObject buff_id = " + (buff == null ? "null" : buff.id)));
    FlyingObject buffFlyingObject = GUIElements.me.flying_buff_prefab.Copy<FlyingObject>();
    buffFlyingObject._override_duration = duration;
    Vector2 screenPoint = (Vector2) MainGame.me.world_cam.WorldToScreenPoint((Vector3) world_pos);
    buffFlyingObject.transform.position = MainGame.me.gui_cam.ScreenToWorldPoint((Vector3) screenPoint);
    buffFlyingObject.Init((object) buff, buff == null ? string.Empty : buff.GetIconName());
    if (GUIElements.me.buffs_panel.gameObject.activeSelf)
      return buffFlyingObject;
    GUIElements.me.buffs_panel.gameObject.SetActive(true);
    GUIElements.me.buffs_panel.alpha = 0.0f;
    DOTween.To((DOGetter<float>) (() => GUIElements.me.buffs_panel.alpha), (DOSetter<float>) (v => GUIElements.me.buffs_panel.alpha = v), 1f, 0.6f);
    return buffFlyingObject;
  }

  public static FlyingObject CreateFlyingGUISprite(string gui_sprite_name, Transform start_pos)
  {
    FlyingObject flyingGuiSprite = GUIElements.me.flying_buff_prefab.Copy<FlyingObject>();
    Vector2 screenPoint = (Vector2) MainGame.me.world_cam.WorldToScreenPoint(start_pos.position);
    flyingGuiSprite.transform.position = MainGame.me.gui_cam.ScreenToWorldPoint((Vector3) screenPoint);
    flyingGuiSprite.Init((object) null, gui_sprite_name);
    flyingGuiSprite.speed = 0.45f;
    return flyingGuiSprite;
  }

  public void StartSmoothFly(Transform destination, float fly_duration = 1f)
  {
    this._flying = false;
    this.dest = destination;
    this._on_late_update = (System.Action) (() => this.transform.DOMove(this.dest.position, fly_duration).OnComplete<Tweener>(new TweenCallback(this.OnReachedDestination)));
  }

  public void StartSmoothFlyAndBounce(Transform destination, float fly_duration = 1f)
  {
    this._flying = false;
    this.dest = destination;
    Vector3 dest_pos = this.dest.position;
    this._on_late_update = (System.Action) (() => this.transform.DOMove(this.transform.position + new Vector3(0.0f, 0.043f), 1f).OnComplete<Tweener>((TweenCallback) (() =>
    {
      Debug.Log((object) "Finished bounce", (UnityEngine.Object) this);
      this.transform.DOMove(dest_pos, fly_duration).OnComplete<Tweener>(new TweenCallback(this.OnReachedDestination));
    })).SetEase<Tweener>(Ease.OutElastic));
  }

  public void StartSmoothFlyAndBounceToAMovingObject(
    TweenToAMovingTarget.GetTransformDelegate get_target,
    float fly_duration = 1f)
  {
    this._flying = false;
    this._on_late_update = (System.Action) (() => this.transform.DOMove(this.transform.position + new Vector3(0.0f, 0.043f), 1f).OnComplete<Tweener>((TweenCallback) (() =>
    {
      Debug.Log((object) "Finished bounce", (UnityEngine.Object) this);
      if (!GUIElements.me.hud_enabled)
        this.OnReachedDestination();
      else
        TweenToAMovingTarget.DoTweenToAMovingTarget(this.gameObject, get_target, 0.1f, fly_duration, new System.Action(this.OnReachedDestination));
    })).SetEase<Tweener>(Ease.OutElastic));
  }

  public void Init(object o, string sprite_name)
  {
    this._active = this._flying = true;
    this._obj = o;
    this.gui_spr.sprite2D = string.IsNullOrEmpty(sprite_name) ? (UnityEngine.Sprite) null : EasySpritesCollection.GetSprite(sprite_name);
    this.gui_spr.MakePixelPerfect();
  }

  public void Update()
  {
    if (!this._active || !this._flying)
      return;
    Vector2 vector2_1 = (Vector2) this.dest.position - (Vector2) this.transform.position;
    this.transform.position += (Vector3) vector2_1.normalized * this.speed * Time.deltaTime;
    Vector2 vector2_2 = (Vector2) this.dest.position - (Vector2) this.transform.position;
    if ((double) Mathf.DeltaAngle(vector2_1.normalized.Atan2() * 57.29578f, vector2_2.normalized.Atan2() * 57.29578f) <= 100.0)
      return;
    this.OnReachedDestination();
  }

  public void ReallyGiveBuff(BuffDefinition buff)
  {
    BuffsLogics.AddBuff(buff.id, this._override_duration);
  }

  public void OnReachedDestination()
  {
    Debug.Log((object) "FlyingObject reached destination", (UnityEngine.Object) this);
    this._flying = false;
    NGUITools.Destroy((UnityEngine.Object) this.gameObject);
    this._active = false;
    if (this._obj != null && this._obj is BuffDefinition)
      this.ReallyGiveBuff((BuffDefinition) this._obj);
    this.on_reached_dest.TryInvoke();
  }

  public void LateUpdate()
  {
    if (this._on_late_update == null)
      return;
    System.Action onLateUpdate = this._on_late_update;
    this._on_late_update = (System.Action) null;
    onLateUpdate();
  }
}
