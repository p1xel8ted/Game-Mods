// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.MMScrollRectConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Scroll Rect Configuration", menuName = "Massive Monster/Scroll Rect Configuration", order = 1)]
public class MMScrollRectConfiguration : ScriptableObject
{
  [Header("Defaults")]
  [SerializeField]
  private ScrollRect.MovementType _movementType = ScrollRect.MovementType.Elastic;
  [SerializeField]
  private float _elasticity = 0.1f;
  [SerializeField]
  private bool _inertia = true;
  [SerializeField]
  private float _decelerationRate = 0.135f;
  [SerializeField]
  private float _scrollSensitivity = 1f;
  [SerializeField]
  private ScrollRect.ScrollbarVisibility _horizontalScrollbarVisbility;
  [SerializeField]
  private ScrollRect.ScrollbarVisibility _verticalScrollbarVisibility;
  [Header("Custom")]
  [SerializeField]
  private AnimationCurve _scrollToEase;
  [SerializeField]
  private float _moveToTravelTime = 1f;

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
