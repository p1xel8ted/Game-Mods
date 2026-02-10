// Decompiled with JetBrains decompiler
// Type: PhotoInformationBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UI;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class PhotoInformationBox : MonoBehaviour, IPoolListener
{
  [SerializeField]
  public RawImage image;
  [SerializeField]
  public MMButton button;
  [SerializeField]
  public PhotoGalleryAlert _alert;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [CompilerGenerated]
  public PhotoModeManager.PhotoData \u003CPhotoData\u003Ek__BackingField;
  public Action<PhotoInformationBox> OnPhotoSelected;
  public Action<PhotoInformationBox> OnPhotoHovered;
  public bool _removeAlertOnHover;

  public MMButton Button => this.button;

  public PhotoModeManager.PhotoData PhotoData
  {
    get => this.\u003CPhotoData\u003Ek__BackingField;
    set => this.\u003CPhotoData\u003Ek__BackingField = value;
  }

  public void Configure(PhotoModeManager.PhotoData photoData, bool removeAlertOnHover)
  {
    this.PhotoData = photoData;
    this.image.texture = (Texture) this.PhotoData.PhotoTexture;
    this.button.onClick.AddListener(new UnityAction(this.PhotoSelected));
    this.button.OnSelected += new System.Action(this.PhotoHovered);
    this.button.OnDeselected += new System.Action(this.OnPhotoDeselected);
    this.button.Selectable.colors = this.button.Selectable.colors with
    {
      normalColor = new Color(1f, 1f, 1f, 0.0f),
      highlightedColor = StaticColors.GreenColor,
      selectedColor = StaticColors.GreenColor,
      pressedColor = StaticColors.GreenColor
    };
    this._canvasGroup.alpha = 0.0f;
    this._canvasGroup.DOFade(1f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this._removeAlertOnHover = removeAlertOnHover;
    this._alert.Configure(photoData.PhotoName);
  }

  public void OnRecycled()
  {
    this.OnPhotoSelected = (Action<PhotoInformationBox>) null;
    this.OnPhotoHovered = (Action<PhotoInformationBox>) null;
    this.button.interactable = true;
    this.image.raycastTarget = true;
    this.button.onClick.RemoveAllListeners();
    this.button.OnSelected -= new System.Action(this.PhotoHovered);
    this.button.OnDeselected -= new System.Action(this.OnPhotoDeselected);
    this.button.OnConfirmDenied = (System.Action) null;
    this.button.Confirmable = true;
    this.button.SetNormalTransitionState();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.PhotoData.PhotoTexture);
    this.PhotoData.PhotoTexture = (Texture2D) null;
    this.PhotoData = (PhotoModeManager.PhotoData) null;
    this.image.texture = (Texture) null;
    this._canvasGroup.alpha = 1f;
    this._alert.ResetAlert();
  }

  public void OnPhotoDeselected()
  {
    if (this._removeAlertOnHover)
      return;
    this._alert.TryRemoveAlert();
  }

  public void PhotoHovered()
  {
    Action<PhotoInformationBox> onPhotoHovered = this.OnPhotoHovered;
    if (onPhotoHovered != null)
      onPhotoHovered(this);
    if (!this._removeAlertOnHover)
      return;
    this._alert.TryRemoveAlert();
  }

  public void PhotoSelected()
  {
    Action<PhotoInformationBox> onPhotoSelected = this.OnPhotoSelected;
    if (onPhotoSelected != null)
      onPhotoSelected(this);
    if (this._removeAlertOnHover)
      return;
    this._alert.TryRemoveAlert();
  }
}
