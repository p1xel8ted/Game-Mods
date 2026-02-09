// Decompiled with JetBrains decompiler
// Type: PanelAutoScroll
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class PanelAutoScroll : MonoBehaviour
{
  public const float AUTO_SCROLL_TIME = 0.4f;
  [SerializeField]
  public Vector2 _offset;
  public UIWidget _widget;
  public UIScrollView _scroll_view;
  public UIPanel _scroll_panel;
  public Transform _scroll_panel_tf;
  public bool _initialized;
  public bool _avaible;

  public bool avaible
  {
    get
    {
      if (!this._initialized)
        this.Init();
      return this._avaible;
    }
  }

  public void Init(UIScrollView scroll_view = null)
  {
    if (this._initialized)
      return;
    this._widget = this.GetComponent<UIWidget>();
    if ((Object) scroll_view == (Object) null)
      scroll_view = this.GetComponentInParent<UIScrollView>();
    this._scroll_view = scroll_view;
    this._scroll_panel = (Object) this._scroll_view != (Object) null ? this._scroll_view.GetComponent<UIPanel>() : (UIPanel) null;
    this._scroll_panel_tf = (Object) this._scroll_view != (Object) null ? this._scroll_view.transform : (Transform) null;
    this._initialized = true;
    this._avaible = (Object) this._widget != (Object) null && (Object) this._scroll_view != (Object) null;
  }

  public void Perform(bool with_animation = true)
  {
    if (!this._initialized)
      this.Init();
    if (!this._avaible)
      return;
    this._scroll_panel_tf.DOKill();
    Vector4 finalClipRegion = this._scroll_panel.finalClipRegion;
    Bounds bounds = this._widget.CalculateBounds(this._scroll_panel_tf);
    Vector2 vector2_1 = new Vector2(finalClipRegion.x, finalClipRegion.y);
    Vector2 vector2_2 = new Vector2(finalClipRegion.z, finalClipRegion.w) / 2f;
    Vector2 vector2_3 = vector2_1 - vector2_2;
    Vector2 vector2_4 = vector2_1 + vector2_2 - (this._scroll_panel.clipSoftness + this._offset);
    Vector2 vector2_5 = vector2_3 + (this._scroll_panel.clipSoftness + this._offset);
    Vector3 vector3 = (Vector3) vector2_4 - bounds.max;
    if (this._scroll_view.movement == UIScrollView.Movement.Horizontal)
    {
      if ((double) vector3.x > 0.0)
      {
        vector3.x = vector2_5.x - bounds.min.x;
        if ((double) vector3.x < 0.0)
          vector3.x = 0.0f;
      }
    }
    else
      vector3.x = 0.0f;
    if (this._scroll_view.movement == UIScrollView.Movement.Vertical)
    {
      if ((double) vector3.y > 0.0)
      {
        vector3.y = vector2_5.y - bounds.min.y;
        if ((double) vector3.y < 0.0)
          vector3.y = 0.0f;
      }
    }
    else
      vector3.y = 0.0f;
    if ((double) vector3.magnitude < 1.0 / 1000.0)
      return;
    if (with_animation)
      this._scroll_panel_tf.DOLocalMove(this._scroll_panel_tf.localPosition + vector3, 0.4f, true).SetEase<Tweener>(Ease.OutCubic);
    else
      this._scroll_panel_tf.localPosition += vector3;
  }
}
