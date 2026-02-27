// Decompiled with JetBrains decompiler
// Type: ItemGauge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class ItemGauge : MonoBehaviour
{
  [SerializeField]
  private GameObject needle;
  [SerializeField]
  private Vector3 startingPoint;
  [SerializeField]
  private Vector3 endPoint;

  public void SetPosition(float norm)
  {
    Vector3 endValue = Vector3.Lerp(this.transform.TransformPoint(this.startingPoint), this.transform.TransformPoint(this.endPoint), norm);
    this.needle.transform.DOKill();
    this.needle.transform.DOMove(endValue, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawSphere(this.transform.TransformPoint(this.startingPoint), 0.05f);
    Gizmos.DrawSphere(this.transform.TransformPoint(this.endPoint), 0.05f);
  }
}
