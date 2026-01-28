// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ParallaxLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (RectTransform))]
public class ParallaxLayer : MonoBehaviour
{
  [SerializeField]
  public float _distance;
  [SerializeField]
  [HideInInspector]
  public RectTransform _rectTransform;

  public RectTransform RectTransform => this._rectTransform;

  public float Distance => this._distance;

  public void Awake() => this._rectTransform = this.GetComponent<RectTransform>();

  public void Reset() => this._rectTransform = this.GetComponent<RectTransform>();
}
