// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ParallaxLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (RectTransform))]
public class ParallaxLayer : MonoBehaviour
{
  [SerializeField]
  private float _distance;
  [SerializeField]
  [HideInInspector]
  private RectTransform _rectTransform;

  public RectTransform RectTransform => this._rectTransform;

  public float Distance => this._distance;

  private void Reset() => this._rectTransform = this.GetComponent<RectTransform>();
}
