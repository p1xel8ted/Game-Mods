// Decompiled with JetBrains decompiler
// Type: UIDragResize
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour
{
  public UIWidget target;
  public UIWidget.Pivot pivot = UIWidget.Pivot.BottomRight;
  public int minWidth = 100;
  public int minHeight = 100;
  public int maxWidth = 100000;
  public int maxHeight = 100000;
  public bool updateAnchors;
  public Plane mPlane;
  public Vector3 mRayPos;
  public Vector3 mLocalPos;
  public int mWidth;
  public int mHeight;
  public bool mDragging;

  public void OnDragStart()
  {
    if (!((Object) this.target != (Object) null))
      return;
    Vector3[] worldCorners = this.target.worldCorners;
    this.mPlane = new Plane(worldCorners[0], worldCorners[1], worldCorners[3]);
    Ray currentRay = UICamera.currentRay;
    float enter;
    if (!this.mPlane.Raycast(currentRay, out enter))
      return;
    this.mRayPos = currentRay.GetPoint(enter);
    this.mLocalPos = this.target.cachedTransform.localPosition;
    this.mWidth = this.target.width;
    this.mHeight = this.target.height;
    this.mDragging = true;
  }

  public void OnDrag(Vector2 delta)
  {
    if (!this.mDragging || !((Object) this.target != (Object) null))
      return;
    Ray currentRay = UICamera.currentRay;
    float enter;
    if (!this.mPlane.Raycast(currentRay, out enter))
      return;
    Transform cachedTransform = this.target.cachedTransform;
    cachedTransform.localPosition = this.mLocalPos;
    this.target.width = this.mWidth;
    this.target.height = this.mHeight;
    Vector3 vector3_1 = currentRay.GetPoint(enter) - this.mRayPos;
    cachedTransform.position += vector3_1;
    Vector3 vector3_2 = Quaternion.Inverse(cachedTransform.localRotation) * (cachedTransform.localPosition - this.mLocalPos);
    cachedTransform.localPosition = this.mLocalPos;
    NGUIMath.ResizeWidget(this.target, this.pivot, vector3_2.x, vector3_2.y, this.minWidth, this.minHeight, this.maxWidth, this.maxHeight);
    if (!this.updateAnchors)
      return;
    this.target.BroadcastMessage("UpdateAnchors");
  }

  public void OnDragEnd() => this.mDragging = false;
}
