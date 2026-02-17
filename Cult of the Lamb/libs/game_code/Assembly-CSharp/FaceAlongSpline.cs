// Decompiled with JetBrains decompiler
// Type: FaceAlongSpline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Splines;

#nullable disable
[RequireComponent(typeof (SplineAnimate))]
public class FaceAlongSpline : BaseMonoBehaviour
{
  [SerializeField]
  public bool invertDirection;
  public SplineAnimate _splineAnimate;

  public void Awake() => this._splineAnimate = this.GetComponent<SplineAnimate>();

  public void Update()
  {
    if (!this._splineAnimate.IsPlaying)
      return;
    this.OrientAlongSpline();
  }

  public void OrientAlongSpline()
  {
    Vector3 tangent = (Vector3) this._splineAnimate.Container.EvaluateTangent(this._splineAnimate.NormalizedTime);
    double angle = (double) Utils.GetAngle(this.transform.position, this.transform.position + tangent);
    float x = Mathf.Sign(tangent.x);
    if (this.invertDirection)
      x *= -1f;
    this.gameObject.transform.localScale = new Vector3(x, 1f, 1f);
  }
}
