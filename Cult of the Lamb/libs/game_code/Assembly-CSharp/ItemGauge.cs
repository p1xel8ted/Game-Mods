// Decompiled with JetBrains decompiler
// Type: ItemGauge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class ItemGauge : MonoBehaviour
{
  [SerializeField]
  public GameObject needle;
  [SerializeField]
  public Vector3 startingPoint;
  [SerializeField]
  public Vector3 endPoint;

  public void Awake() => this.SetPosition(0.0f);

  public void SetPosition(float norm)
  {
    Vector3 endValue = Vector3.Lerp(this.startingPoint, this.endPoint, norm);
    this.needle.transform.DOKill();
    this.needle.transform.DOLocalMove(endValue, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  public void OnDrawGizmos()
  {
    Gizmos.DrawSphere(this.transform.TransformPoint(this.startingPoint), 0.05f);
    Gizmos.DrawSphere(this.transform.TransformPoint(this.endPoint), 0.05f);
  }
}
