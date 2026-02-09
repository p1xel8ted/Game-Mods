// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DPointerInfluence
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-pointer-influence/")]
public class ProCamera2DPointerInfluence : BasePC2D, IPreMover
{
  public static string ExtensionName = "Pointer Influence";
  public float MaxHorizontalInfluence = 3f;
  public float MaxVerticalInfluence = 2f;
  public float InfluenceSmoothness = 0.2f;
  public Vector2 _influence;
  public Vector2 _velocity;
  public int _prmOrder = 3000;

  public override void Awake()
  {
    base.Awake();
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddPreMover((IPreMover) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePreMover((IPreMover) this);
  }

  public override void OnReset()
  {
    this._influence = Vector2.zero;
    this._velocity = Vector2.zero;
  }

  public void PreMove(float deltaTime)
  {
    if (!this.enabled)
      return;
    this.ApplyInfluence(deltaTime);
  }

  public int PrMOrder
  {
    get => this._prmOrder;
    set => this._prmOrder = value;
  }

  public void ApplyInfluence(float deltaTime)
  {
    Vector3 viewportPoint = this.ProCamera2D.GameCamera.ScreenToViewportPoint(Input.mousePosition);
    float num1 = viewportPoint.x.Remap(0.0f, 1f, -1f, 1f);
    double num2 = (double) viewportPoint.y.Remap(0.0f, 1f, -1f, 1f);
    float x = num1 * this.MaxHorizontalInfluence;
    double verticalInfluence = (double) this.MaxVerticalInfluence;
    float y = (float) (num2 * verticalInfluence);
    this._influence = Vector2.SmoothDamp(this._influence, new Vector2(x, y), ref this._velocity, this.InfluenceSmoothness, float.PositiveInfinity, deltaTime);
    this.ProCamera2D.ApplyInfluence(this._influence);
  }
}
