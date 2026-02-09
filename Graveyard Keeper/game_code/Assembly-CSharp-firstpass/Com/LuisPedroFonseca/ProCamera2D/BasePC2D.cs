// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.BasePC2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public abstract class BasePC2D : MonoBehaviour
{
  public Com.LuisPedroFonseca.ProCamera2D.ProCamera2D ProCamera2D;
  public Func<Vector3, float> Vector3H;
  public Func<Vector3, float> Vector3V;
  public Func<Vector3, float> Vector3D;
  public Func<float, float, Vector3> VectorHV;
  public Func<float, float, float, Vector3> VectorHVD;
  public Transform _transform;
  public bool _enabled;

  public virtual void Awake()
  {
    this._transform = this.transform;
    if ((UnityEngine.Object) this.ProCamera2D == (UnityEngine.Object) null && (UnityEngine.Object) Camera.main != (UnityEngine.Object) null)
      this.ProCamera2D = Camera.main.GetComponent<Com.LuisPedroFonseca.ProCamera2D.ProCamera2D>();
    else if ((UnityEngine.Object) this.ProCamera2D == (UnityEngine.Object) null)
      this.ProCamera2D = UnityEngine.Object.FindObjectOfType(typeof (Com.LuisPedroFonseca.ProCamera2D.ProCamera2D)) as Com.LuisPedroFonseca.ProCamera2D.ProCamera2D;
    if ((UnityEngine.Object) this.ProCamera2D == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) (this.GetType().Name + ": ProCamera2D not found! Please add the ProCamera2D.cs component to your main camera."));
    }
    else
    {
      if (this.enabled)
        this.Enable();
      this.ResetAxisFunctions();
    }
  }

  public virtual void OnEnable() => this.Enable();

  public virtual void OnDisable() => this.Disable();

  public virtual void OnDestroy() => this.Disable();

  public virtual void OnReset()
  {
  }

  public void Enable()
  {
    if (this._enabled || (UnityEngine.Object) this.ProCamera2D == (UnityEngine.Object) null)
      return;
    this._enabled = true;
    this.ProCamera2D.OnReset += new Action(this.OnReset);
  }

  public void Disable()
  {
    if (!((UnityEngine.Object) this.ProCamera2D != (UnityEngine.Object) null) || !this._enabled)
      return;
    this._enabled = false;
    this.ProCamera2D.OnReset -= new Action(this.OnReset);
  }

  public void ResetAxisFunctions()
  {
    if (this.Vector3H != null)
      return;
    switch (this.ProCamera2D.Axis)
    {
      case MovementAxis.XY:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.x);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.y);
        this.Vector3D = (Func<Vector3, float>) (vector => vector.z);
        this.VectorHV = (Func<float, float, Vector3>) ((h, v) => new Vector3(h, v, 0.0f));
        this.VectorHVD = (Func<float, float, float, Vector3>) ((h, v, d) => new Vector3(h, v, d));
        break;
      case MovementAxis.XZ:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.x);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.z);
        this.Vector3D = (Func<Vector3, float>) (vector => vector.y);
        this.VectorHV = (Func<float, float, Vector3>) ((h, v) => new Vector3(h, 0.0f, v));
        this.VectorHVD = (Func<float, float, float, Vector3>) ((h, v, d) => new Vector3(h, d, v));
        break;
      case MovementAxis.YZ:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.z);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.y);
        this.Vector3D = (Func<Vector3, float>) (vector => vector.x);
        this.VectorHV = (Func<float, float, Vector3>) ((h, v) => new Vector3(0.0f, v, h));
        this.VectorHVD = (Func<float, float, float, Vector3>) ((h, v, d) => new Vector3(d, v, h));
        break;
    }
  }
}
