// Decompiled with JetBrains decompiler
// Type: Kino.IsolineScroller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[RequireComponent(typeof (Isoline))]
[AddComponentMenu("Kino Image Effects/Isoline Scroller")]
public class IsolineScroller : MonoBehaviour
{
  [SerializeField]
  public Vector3 _direction = Vector3.one * 0.577f;
  [SerializeField]
  public float _speed = 0.2f;
  public float _time;

  public Vector3 direction
  {
    get => this._direction;
    set => this._direction = value;
  }

  public float speed
  {
    get => this._speed;
    set => this._speed = value;
  }

  public void Update()
  {
    this.GetComponent<Isoline>().offset += this._direction.normalized * this._speed * Time.deltaTime;
  }
}
