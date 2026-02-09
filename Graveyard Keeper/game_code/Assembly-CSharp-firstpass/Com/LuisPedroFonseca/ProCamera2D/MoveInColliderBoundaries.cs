// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.MoveInColliderBoundaries
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public class MoveInColliderBoundaries
{
  public Func<Vector3, float> Vector3H;
  public Func<Vector3, float> Vector3V;
  public Func<float, float, Vector3> VectorHV;
  public const float Offset = 0.2f;
  public const float RaySizeCompensation = 0.2f;
  public Transform CameraTransform;
  public Vector2 CameraSize;
  public LayerMask CameraCollisionMask;
  public int TotalHorizontalRays = 3;
  public int TotalVerticalRays = 3;
  public RaycastOrigins _raycastOrigins;
  public CameraCollisionState _cameraCollisionState;
  public RaycastHit _raycastHit;
  public float _verticalDistanceBetweenRays;
  public float _horizontalDistanceBetweenRays;
  public Com.LuisPedroFonseca.ProCamera2D.ProCamera2D _proCamera2D;

  public RaycastOrigins RaycastOrigins => this._raycastOrigins;

  public CameraCollisionState CameraCollisionState => this._cameraCollisionState;

  public MoveInColliderBoundaries(Com.LuisPedroFonseca.ProCamera2D.ProCamera2D proCamera2D)
  {
    this._proCamera2D = proCamera2D;
    switch (this._proCamera2D.Axis)
    {
      case MovementAxis.XY:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.x);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.y);
        this.VectorHV = (Func<float, float, Vector3>) ((h, v) => new Vector3(h, v, 0.0f));
        break;
      case MovementAxis.XZ:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.x);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.z);
        this.VectorHV = (Func<float, float, Vector3>) ((h, v) => new Vector3(h, 0.0f, v));
        break;
      case MovementAxis.YZ:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.z);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.y);
        this.VectorHV = (Func<float, float, Vector3>) ((h, v) => new Vector3(0.0f, v, h));
        break;
    }
  }

  public Vector3 Move(Vector3 deltaMovement)
  {
    this.UpdateRaycastOrigins();
    this.GetOffsetAndForceMovement(this._raycastOrigins.BottomLeft, ref deltaMovement, ref this._cameraCollisionState.HBottomLeft, ref this._cameraCollisionState.VBottomLeft, -1f, -1f);
    this.GetOffsetAndForceMovement(this._raycastOrigins.BottomRight, ref deltaMovement, ref this._cameraCollisionState.HBottomRight, ref this._cameraCollisionState.VBottomRight, 1f, -1f);
    this.GetOffsetAndForceMovement(this._raycastOrigins.TopLeft, ref deltaMovement, ref this._cameraCollisionState.HTopLeft, ref this._cameraCollisionState.VTopLeft, -1f, 1f);
    this.GetOffsetAndForceMovement(this._raycastOrigins.TopRight, ref deltaMovement, ref this._cameraCollisionState.HTopRight, ref this._cameraCollisionState.VTopRight, 1f, 1f);
    float num1 = 0.0f;
    if ((double) this.Vector3H(deltaMovement) != 0.0)
      num1 = this.MoveInAxis(this.Vector3H(deltaMovement), true);
    float num2 = 0.0f;
    if ((double) this.Vector3V(deltaMovement) != 0.0)
      num2 = this.MoveInAxis(this.Vector3V(deltaMovement), false);
    return this.VectorHV(num1, num2);
  }

  public void UpdateRaycastOrigins()
  {
    this._raycastOrigins.BottomRight = this.VectorHV(this.Vector3H(this.CameraTransform.localPosition) + this.CameraSize.x / 2f, this.Vector3V(this.CameraTransform.localPosition) - this.CameraSize.y / 2f);
    this._raycastOrigins.BottomLeft = this.VectorHV(this.Vector3H(this.CameraTransform.localPosition) - this.CameraSize.x / 2f, this.Vector3V(this.CameraTransform.localPosition) - this.CameraSize.y / 2f);
    this._raycastOrigins.TopRight = this.VectorHV(this.Vector3H(this.CameraTransform.localPosition) + this.CameraSize.x / 2f, this.Vector3V(this.CameraTransform.localPosition) + this.CameraSize.y / 2f);
    this._raycastOrigins.TopLeft = this.VectorHV(this.Vector3H(this.CameraTransform.localPosition) - this.CameraSize.x / 2f, this.Vector3V(this.CameraTransform.localPosition) + this.CameraSize.y / 2f);
    this._horizontalDistanceBetweenRays = this.CameraSize.x / (float) (this.TotalVerticalRays - 1);
    this._verticalDistanceBetweenRays = this.CameraSize.y / (float) (this.TotalHorizontalRays - 1);
  }

  public void GetOffsetAndForceMovement(
    Vector3 rayTargetPos,
    ref Vector3 deltaMovement,
    ref bool horizontalCheck,
    ref bool verticalCheck,
    float hSign,
    float vSign)
  {
    Vector3 vector3 = this.VectorHV(this.Vector3H(this.CameraTransform.localPosition), this.Vector3V(this.CameraTransform.localPosition));
    Vector3 normalized = (rayTargetPos - vector3).normalized;
    float maxDistance = (float) ((double) (rayTargetPos - vector3).magnitude + 0.0099999997764825821 + 0.5);
    this.DrawRay(vector3, normalized * maxDistance, Color.yellow);
    if (Physics.Raycast(vector3, normalized, out this._raycastHit, maxDistance, (int) this.CameraCollisionMask))
    {
      if ((double) Mathf.Abs(this.Vector3H(this._raycastHit.normal)) > (double) Mathf.Abs(this.Vector3V(this._raycastHit.normal)))
      {
        horizontalCheck = !verticalCheck;
        if ((double) this.Vector3H(deltaMovement) != 0.0)
          return;
        float num1 = 0.1f * hSign;
        deltaMovement = this.VectorHV(num1, this.Vector3V(deltaMovement));
        float num2 = this.MoveInAxis(this.Vector3H(deltaMovement), true);
        deltaMovement = this.VectorHV(num2, this.Vector3V(deltaMovement));
      }
      else
      {
        verticalCheck = !horizontalCheck;
        if ((double) this.Vector3V(deltaMovement) != 0.0)
          return;
        float num3 = 0.1f * vSign;
        deltaMovement = this.VectorHV(this.Vector3H(deltaMovement), num3);
        float num4 = this.MoveInAxis(this.Vector3V(deltaMovement), false);
        deltaMovement = this.VectorHV(this.Vector3H(deltaMovement), num4);
      }
    }
    else
    {
      horizontalCheck = false;
      verticalCheck = false;
    }
  }

  public float MoveInAxis(float deltaMovement, bool isHorizontal)
  {
    bool flag1 = (double) deltaMovement > 0.0;
    float maxDistance = Mathf.Abs(deltaMovement) + 0.2f;
    Vector3 direction;
    Vector3 vector3_1;
    if (isHorizontal)
    {
      direction = flag1 ? this.CameraTransform.right : -this.CameraTransform.right;
      vector3_1 = flag1 ? this._raycastOrigins.BottomRight : this._raycastOrigins.BottomLeft;
    }
    else
    {
      direction = flag1 ? this.CameraTransform.up : -this.CameraTransform.up;
      vector3_1 = flag1 ? this._raycastOrigins.TopLeft : this._raycastOrigins.BottomLeft;
    }
    float num1 = float.NegativeInfinity;
    bool flag2 = false;
    int num2 = isHorizontal ? this.TotalHorizontalRays : this.TotalVerticalRays;
    for (int index = 0; index < num2; ++index)
    {
      float num3 = isHorizontal ? this.Vector3H(vector3_1) : this.Vector3H(vector3_1) + (float) index * this._horizontalDistanceBetweenRays;
      float num4 = isHorizontal ? this.Vector3V(vector3_1) + (float) index * this._verticalDistanceBetweenRays : this.Vector3V(vector3_1);
      if (isHorizontal)
      {
        if (flag1 && this._cameraCollisionState.VBottomRight && index == 0 || !flag1 && this._cameraCollisionState.VBottomLeft && index == 0)
          num4 += 0.2f;
        if (flag1 && this._cameraCollisionState.VTopRight && index == num2 - 1 || !flag1 && this._cameraCollisionState.VTopLeft && index == num2 - 1)
          num4 -= 0.2f;
      }
      else
      {
        if (flag1 && this._cameraCollisionState.HTopLeft && index == 0 || !flag1 && this._cameraCollisionState.HBottomLeft && index == 0)
          num3 += 0.2f;
        if (flag1 && this._cameraCollisionState.HTopRight && index == num2 - 1 || !flag1 && this._cameraCollisionState.HBottomRight && index == num2 - 1)
          num3 -= 0.2f;
      }
      Vector3 vector3_2 = this.VectorHV(num3, num4);
      if (Physics.Raycast(vector3_2, direction, out this._raycastHit, maxDistance, (int) this.CameraCollisionMask))
      {
        this.DrawRay(vector3_2, direction * maxDistance, Color.red);
        if ((!(isHorizontal & flag2) || !flag1 || (double) num1 > (double) this.Vector3H(this._raycastHit.point)) && (flag1 || (double) num1 < (double) this.Vector3H(this._raycastHit.point)) && (!flag2 || !flag1 || (double) num1 > (double) this.Vector3V(this._raycastHit.point)) && (flag1 || (double) num1 < (double) this.Vector3V(this._raycastHit.point)))
        {
          flag2 = true;
          deltaMovement = isHorizontal ? (float) ((double) this.Vector3H(this._raycastHit.point) - (double) this.Vector3H(vector3_2) + (flag1 ? -0.20000000298023224 : 0.20000000298023224)) : (float) ((double) this.Vector3V(this._raycastHit.point) - (double) this.Vector3V(vector3_2) + (flag1 ? -0.20000000298023224 : 0.20000000298023224));
          num1 = isHorizontal ? this.Vector3H(this._raycastHit.point) : this.Vector3V(this._raycastHit.point);
        }
      }
      else
        this.DrawRay(vector3_2, direction * maxDistance, Color.cyan);
    }
    return deltaMovement;
  }

  public void DrawRay(Vector3 start, Vector3 dir, Color color, float duration = 0.0f)
  {
    if ((double) duration != 0.0)
      Debug.DrawRay(start, dir, color, duration);
    else
      Debug.DrawRay(start, dir, color);
  }
}
