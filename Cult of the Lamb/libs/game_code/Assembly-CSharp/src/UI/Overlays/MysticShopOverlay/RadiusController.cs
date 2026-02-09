// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.MysticShopOverlay.RadiusController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
namespace src.UI.Overlays.MysticShopOverlay;

[ExecuteInEditMode]
public class RadiusController : MonoBehaviour
{
  [Header("Master Radius")]
  [SerializeField]
  [Range(0.0f, 2f)]
  public float _expansion;
  [SerializeField]
  [Range(0.0f, 1000f)]
  public float _radius;
  [Header("Radial Graphic")]
  [SerializeField]
  [Range(0.0f, 1000f)]
  public float _radialGraphicOffset;
  [SerializeField]
  public MMUIRadialGraphic _radialGraphic;
  [Header("Radial Graphic Mask")]
  [SerializeField]
  [Range(0.0f, 1000f)]
  public float _radialGraphicMaskOffset;
  [SerializeField]
  public MMUIRadialGraphic _radialGraphicMask;
  [Header("Inner Ring")]
  [SerializeField]
  [Range(0.0f, 1000f)]
  public float _innerRingOffset;
  [SerializeField]
  public MysticShopRingInnerRenderer _innerRenderer;
  [Header("Radial Layout")]
  [SerializeField]
  [Range(0.0f, 1000f)]
  public float _radialGroupOffset;
  [SerializeField]
  public MMRadialLayoutGroup _radialLayoutGroup;
  [Header("Flourishes")]
  [SerializeField]
  [Range(0.0f, 2f)]
  public float _flourishExpansion;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _flourishFill;
  [SerializeField]
  public AnimationCurve _flourishFillCurve;
  [SerializeField]
  [Range(0.0f, 1000f)]
  public float _flourishOffset;
  [SerializeField]
  public AnimationCurve _flourishRadiusCurve;
  [SerializeField]
  public MysticShopFlourishRenderer[] _flourishes;
  [Header("Arms")]
  [SerializeField]
  public RectTransform _selector;
  [SerializeField]
  public RectTransform _upperArm;
  [SerializeField]
  public RectTransform _lowerArm;
  [Header("Planet Sticks")]
  [SerializeField]
  public RectTransform _stickA;
  [SerializeField]
  public RectTransform _stickB;

  public float Expansion
  {
    get => this._expansion;
    set
    {
      this._expansion = value;
      this.Update();
    }
  }

  public void Update()
  {
    float expansion = this._expansion;
    if ((bool) (Object) this._radialGraphic)
      this._radialGraphic.Radius = this._radius * expansion + this._radialGraphicOffset;
    if ((bool) (Object) this._radialGraphicMask)
      this._radialGraphicMask.Radius = this._radius * expansion + this._radialGraphicMaskOffset;
    if ((bool) (Object) this._innerRenderer)
      this._innerRenderer.Radius = this._radius * expansion + this._innerRingOffset;
    if ((bool) (Object) this._radialLayoutGroup)
      this._radialLayoutGroup.Radius = this._radius * expansion + this._radialGroupOffset;
    if (this._flourishes != null)
    {
      foreach (MysticShopFlourishRenderer flourish in this._flourishes)
      {
        flourish.Radius = this._radius * this._flourishRadiusCurve.Evaluate(this._flourishExpansion) + this._flourishOffset;
        flourish.FillScaler = this._flourishFillCurve.Evaluate(this._flourishFill);
      }
    }
    if ((bool) (Object) this._upperArm && (bool) (Object) this._lowerArm && (bool) (Object) this._selector)
    {
      this._selector.anchoredPosition = new Vector2(0.0f, (float) (-(double) this._radius * (1.0 - (double) expansion)));
      this._upperArm.anchoredPosition = new Vector2(0.0f, (float) (-(double) this._radius * (1.0 - (double) expansion)));
      this._lowerArm.anchoredPosition = new Vector2(0.0f, this._radius * (1f - expansion));
    }
    if (!(bool) (Object) this._stickA || !(bool) (Object) this._stickB)
      return;
    this._stickA.anchoredPosition = new Vector2(0.0f, (float) (-(double) this._stickA.rect.height * (1.0 - (double) expansion)));
    this._stickB.anchoredPosition = new Vector2(0.0f, (float) (-(double) this._stickB.rect.height * (1.0 - (double) expansion)));
  }
}
