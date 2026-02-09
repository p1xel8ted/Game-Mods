// Decompiled with JetBrains decompiler
// Type: SpeechBubbleCornerPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class SpeechBubbleCornerPoint : MonoBehaviour
{
  [HideInInspector]
  public Transform tf;
  [HideInInspector]
  public Transform top_parent_tf;
  [HideInInspector]
  public Vector2 shift;
  [HideInInspector]
  public Vector2 pixel_shift;

  public void Start() => this.Log();

  public void OnEnable() => this.Log();

  public void Init(Transform top_parent)
  {
    this.tf = this.transform;
    this.top_parent_tf = top_parent;
  }

  public void CalcShift(Camera gui_cam)
  {
    this.shift = (Vector2) (this.top_parent_tf.position - this.tf.position);
    this.pixel_shift = (Vector2) (gui_cam.WorldToScreenPoint((Vector3) this.shift) - gui_cam.WorldToScreenPoint(Vector3.zero));
  }

  public void Update()
  {
    if (Application.isPlaying)
      return;
    this.Log();
  }

  public void Log()
  {
    Debug.LogError((object) "SpeechBubbleCornerPoint is obsolete, Use BubbleCornerPoint instead", (Object) this.gameObject);
  }
}
