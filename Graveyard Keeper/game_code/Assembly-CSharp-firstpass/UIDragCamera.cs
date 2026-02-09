// Decompiled with JetBrains decompiler
// Type: UIDragCamera
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Drag Camera")]
public class UIDragCamera : MonoBehaviour
{
  public UIDraggableCamera draggableCamera;

  public void Awake()
  {
    if (!((Object) this.draggableCamera == (Object) null))
      return;
    this.draggableCamera = NGUITools.FindInParents<UIDraggableCamera>(this.gameObject);
  }

  public void OnPress(bool isPressed)
  {
    if (!this.enabled || !NGUITools.GetActive(this.gameObject) || !((Object) this.draggableCamera != (Object) null))
      return;
    this.draggableCamera.Press(isPressed);
  }

  public void OnDrag(Vector2 delta)
  {
    if (!this.enabled || !NGUITools.GetActive(this.gameObject) || !((Object) this.draggableCamera != (Object) null))
      return;
    this.draggableCamera.Drag(delta);
  }

  public void OnScroll(float delta)
  {
    if (!this.enabled || !NGUITools.GetActive(this.gameObject) || !((Object) this.draggableCamera != (Object) null))
      return;
    this.draggableCamera.Scroll(delta);
  }
}
