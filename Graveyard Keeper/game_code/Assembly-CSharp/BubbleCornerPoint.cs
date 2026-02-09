// Decompiled with JetBrains decompiler
// Type: BubbleCornerPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BubbleCornerPoint : MonoBehaviour
{
  public Transform top_parent_tf;
  public Vector2 shift;
  public Vector2 pixel_shift;
  public Transform _tf;
  public SimpleUITable.Alignment bubble_custom_align = SimpleUITable.Alignment.NotSet;

  public void Init(Transform top_parent)
  {
    this._tf = this.transform;
    this.top_parent_tf = top_parent;
  }

  public void CalcShift(Camera gui_cam)
  {
    if ((Object) this._tf == (Object) null)
      this._tf = this.transform;
    if ((Object) this.top_parent_tf == (Object) null)
      return;
    this.shift = (Vector2) (this.top_parent_tf.position - this._tf.position);
    this.pixel_shift = (Vector2) (gui_cam.WorldToScreenPoint((Vector3) this.shift) - gui_cam.WorldToScreenPoint(Vector3.zero));
  }
}
