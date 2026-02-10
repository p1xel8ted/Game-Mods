// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.MMScrollRectConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Scroll Rect Configuration", menuName = "Massive Monster/Scroll Rect Configuration", order = 1)]
public class MMScrollRectConfiguration : ScriptableObject
{
  [Header("Defaults")]
  [SerializeField]
  public ScrollRect.MovementType _movementType = ScrollRect.MovementType.Elastic;
  [SerializeField]
  public float _elasticity = 0.1f;
  [SerializeField]
  public bool _inertia = true;
  [SerializeField]
  public float _decelerationRate = 0.135f;
  [SerializeField]
  public float _scrollSensitivity = 1f;
  [SerializeField]
  public ScrollRect.ScrollbarVisibility _horizontalScrollbarVisbility;
  [SerializeField]
  public ScrollRect.ScrollbarVisibility _verticalScrollbarVisibility;
  [Header("Custom")]
  [SerializeField]
  public AnimationCurve _scrollToEase;
  [SerializeField]
  public float _moveToTravelTime = 1f;

  public void ApplySettings(MMScrollRect mmScrollRect)
  {
    mmScrollRect.movementType = this._movementType;
    mmScrollRect.elasticity = this._elasticity;
    mmScrollRect.inertia = this._inertia;
    mmScrollRect.decelerationRate = this._decelerationRate;
    mmScrollRect.scrollSensitivity = this._scrollSensitivity;
    mmScrollRect.horizontalScrollbarVisibility = this._horizontalScrollbarVisbility;
    mmScrollRect.verticalScrollbarVisibility = this._verticalScrollbarVisibility;
  }

  public AnimationCurve ScrollToEase => this._scrollToEase;

  public float MoveToTravelTime => this._moveToTravelTime;
}
