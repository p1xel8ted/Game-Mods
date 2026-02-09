// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DLimitSpeed
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-limit-speed/")]
public class ProCamera2DLimitSpeed : BasePC2D, IPositionDeltaChanger
{
  public static string ExtensionName = "Limit Speed";
  public bool LimitHorizontalSpeed = true;
  public float MaxHorizontalSpeed = 2f;
  public bool LimitVerticalSpeed = true;
  public float MaxVerticalSpeed = 2f;
  public int _pdcOrder = 1000;

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
    float num1 = 1f / deltaTime;
    float num2 = this.Vector3H(originalDelta) * num1;
    float num3 = this.Vector3V(originalDelta) * num1;
    if (this.LimitHorizontalSpeed)
      num2 = Mathf.Clamp(num2, -this.MaxHorizontalSpeed, this.MaxHorizontalSpeed);
    if (this.LimitVerticalSpeed)
      num3 = Mathf.Clamp(num3, -this.MaxVerticalSpeed, this.MaxVerticalSpeed);
    return this.VectorHV(num2 * deltaTime, num3 * deltaTime);
  }

  public int PDCOrder
  {
    get => this._pdcOrder;
    set => this._pdcOrder = value;
  }
}
