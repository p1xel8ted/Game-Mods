// Decompiled with JetBrains decompiler
// Type: VisInvisMonoBehaviour
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
public class VisInvisMonoBehaviour : MonoBehaviour
{
  public bool visible = true;
  [NonSerialized]
  public Transform _tf;
  [NonSerialized]
  public bool _tf_is_set;
  public bool _outside_camera;
  public bool outside_camera_calculations;
  public bool ignore_unity_became_visible;
  public static int UPDATE_GROUPS = 3;
  public int _update_group = -1;

  public Transform tf
  {
    get
    {
      if (!this._tf_is_set)
      {
        this._tf_is_set = true;
        this._tf = this.transform;
      }
      return this._tf;
    }
  }

  public void OnBecameVisible()
  {
    if (!Application.isPlaying || this.ignore_unity_became_visible)
      return;
    this.visible = true;
  }

  public void OnBecameInvisible()
  {
    if (!Application.isPlaying || this.ignore_unity_became_visible)
      return;
    this.visible = false;
  }

  public bool IsVisible() => this.visible;

  public void Update() => this.VisInvisUpdate();

  public void VisInvisUpdate()
  {
    if (!this.outside_camera_calculations)
      return;
    if (this._update_group == -1)
      this._update_group = UnityEngine.Random.Range(0, VisInvisMonoBehaviour.UPDATE_GROUPS);
    if (Application.isPlaying && Time.frameCount % VisInvisMonoBehaviour.UPDATE_GROUPS != this._update_group)
      return;
    bool flag = this.IsOutsideCamera();
    if (flag == this._outside_camera)
      return;
    this._outside_camera = flag;
    if (flag)
      this.OnMovedOutsideCamera();
    else
      this.OnMovedInsideCamera();
  }

  public virtual void OnMovedOutsideCamera()
  {
  }

  public virtual void OnMovedInsideCamera()
  {
  }

  public bool is_inside_camera => !this._outside_camera;

  public bool IsOutsideCamera(float max_coord = 1f)
  {
    if (!Application.isPlaying)
      return false;
    Vector3 position = this.tf.position;
    Vector2 mainCameraPosV2 = GJCommons.main_camera_pos_v2;
    float num1 = (position.x - mainCameraPosV2.x) / (float) Screen.width;
    float num2 = (position.y - mainCameraPosV2.y) / (float) Screen.height;
    return (double) num1 > (double) max_coord || (double) num2 > (double) max_coord || (double) num1 < -(double) max_coord || (double) num2 < -(double) max_coord;
  }
}
