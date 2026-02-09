// Decompiled with JetBrains decompiler
// Type: PolygonColliderAutoSize
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (PolygonCollider2D))]
public class PolygonColliderAutoSize : MonoBehaviour
{
  public float padding;
  public float offset;
  public Vector2 shift = Vector2.zero;
  public Vector2 expand = Vector2.zero;
  public UIWidget _ui_widget;
  public PolygonCollider2D _collider;
  public int _width;
  public int _height;

  public void Update() => this.RecalcColliderSize(false);

  public void OnValidate()
  {
    this.padding = (float) Mathf.RoundToInt(this.padding * 10f) / 10f;
    this.offset = (float) Mathf.RoundToInt(this.offset * 10f) / 10f;
    this.RecalcColliderSize(true);
  }

  public void RecalcColliderSize(bool force)
  {
    if ((Object) this._ui_widget == (Object) null)
      this._ui_widget = this.GetComponent<UIWidget>();
    if ((Object) this._collider == (Object) null)
    {
      this._collider = this.GetComponent<PolygonCollider2D>();
      if ((Object) this._collider == (Object) null)
        this._collider = this.gameObject.AddComponent<PolygonCollider2D>();
    }
    if ((Object) this._ui_widget == (Object) null || (Object) this._collider == (Object) null)
    {
      this._width = this._height = 0;
    }
    else
    {
      if (!force && this._width == this._ui_widget.width && this._height == this._ui_widget.height && this._collider.points.Length == 4)
        return;
      this._width = this._ui_widget.width;
      this._height = this._ui_widget.height;
      float num1 = (float) this._width / 2f;
      float num2 = (float) this._height / 2f - this.offset;
      float num3 = 0.0f;
      float num4 = 0.0f;
      switch (this._ui_widget.pivot)
      {
        case UIWidget.Pivot.TopLeft:
          num4 = (float) this._width / 2f;
          num3 = (float) -this._height / 2f;
          break;
        case UIWidget.Pivot.Top:
          num3 = (float) -this._height / 2f;
          break;
        case UIWidget.Pivot.Left:
          num4 = (float) this._width / 2f;
          break;
      }
      float num5 = num4 + this.shift.x;
      float num6 = num3 + this.shift.y;
      Vector2[] vector2Array = new Vector2[4]
      {
        new Vector2(num1 + num5 + this.expand.x, num2 + this.padding + num6 + this.expand.y),
        new Vector2(-num1 + num5 - this.expand.x, num2 + this.padding + num6 + this.expand.y),
        new Vector2(-num1 + num5 - this.expand.x, -num2 + this.padding + num6 - this.expand.y),
        new Vector2(num1 + num5 + this.expand.x, -num2 + this.padding + num6 - this.expand.y)
      };
      Vector2[] points = this._collider.points;
      bool flag = points.Length != 4;
      if (!flag)
      {
        for (int index = 0; index < vector2Array.Length; ++index)
        {
          if (!points[index].EqualsTo(vector2Array[index]))
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return;
      this._collider.points = vector2Array;
    }
  }
}
