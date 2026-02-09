// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DLimitDistance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-limit-distance/")]
public class ProCamera2DLimitDistance : BasePC2D, IPositionDeltaChanger
{
  public static string ExtensionName = "Limit Distance";
  public bool LimitHorizontalCameraDistance = true;
  public float MaxHorizontalTargetDistance = 0.8f;
  public bool LimitVerticalCameraDistance = true;
  public float MaxVerticalTargetDistance = 0.8f;
  public int _pdcOrder = 2000;

  public override void Awake()
  {
    base.Awake();
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddPositionDeltaChanger((IPositionDeltaChanger) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePositionDeltaChanger((IPositionDeltaChanger) this);
  }

  public Vector3 AdjustDelta(float deltaTime, Vector3 originalDelta)
  {
    if (!this.enabled)
      return originalDelta;
    float num1 = this.Vector3H(originalDelta);
    bool flag1 = false;
    if (this.LimitHorizontalCameraDistance)
    {
      float num2 = this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f * this.MaxHorizontalTargetDistance;
      if ((double) this.ProCamera2D.CameraTargetPosition.x > (double) num1 + (double) this.Vector3H(this.ProCamera2D.LocalPosition) + (double) num2)
      {
        num1 = this.ProCamera2D.CameraTargetPosition.x - (this.Vector3H(this.ProCamera2D.LocalPosition) + num2);
        flag1 = true;
      }
      else if ((double) this.ProCamera2D.CameraTargetPosition.x < (double) num1 + (double) this.Vector3H(this.ProCamera2D.LocalPosition) - (double) num2)
      {
        num1 = this.ProCamera2D.CameraTargetPosition.x - (this.Vector3H(this.ProCamera2D.LocalPosition) - num2);
        flag1 = true;
      }
    }
    float num3 = this.Vector3V(originalDelta);
    bool flag2 = false;
    if (this.LimitVerticalCameraDistance)
    {
      float num4 = this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f * this.MaxVerticalTargetDistance;
      if ((double) this.ProCamera2D.CameraTargetPosition.y > (double) num3 + (double) this.Vector3V(this.ProCamera2D.LocalPosition) + (double) num4)
      {
        num3 = this.ProCamera2D.CameraTargetPosition.y - (this.Vector3V(this.ProCamera2D.LocalPosition) + num4);
        flag2 = true;
      }
      else if ((double) this.ProCamera2D.CameraTargetPosition.y < (double) num3 + (double) this.Vector3V(this.ProCamera2D.LocalPosition) - (double) num4)
      {
        num3 = this.ProCamera2D.CameraTargetPosition.y - (this.Vector3V(this.ProCamera2D.LocalPosition) - num4);
        flag2 = true;
      }
    }
    this.ProCamera2D.CameraTargetPositionSmoothed = new Vector2(flag1 ? this.Vector3H(this.ProCamera2D.LocalPosition) + num1 : this.ProCamera2D.CameraTargetPositionSmoothed.x, flag2 ? this.Vector3V(this.ProCamera2D.LocalPosition) + num3 : this.ProCamera2D.CameraTargetPositionSmoothed.y);
    return this.VectorHV(num1, num3);
  }

  public int PDCOrder
  {
    get => this._pdcOrder;
    set => this._pdcOrder = value;
  }
}
