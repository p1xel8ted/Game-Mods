// Decompiled with JetBrains decompiler
// Type: UITakePhotoOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Rewired;
using src.UI;
using src.UINavigator;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UITakePhotoOverlayController : UIMenuBase
{
  [SerializeField]
  public MMSlider focusSlider;
  [SerializeField]
  public MMSlider tiltSlider;
  [SerializeField]
  public Image photoFlash;
  [SerializeField]
  public RawImage takenPhotoPreview;
  [SerializeField]
  public Image takenPhotoPreviewOutline;
  public DG.Tweening.Sequence takePhotoSequence;
  [SerializeField]
  public UI_Transitions cameraIcon;
  [SerializeField]
  public Image fadeInImage;
  [SerializeField]
  public Image iconForCameraSnap;
  [SerializeField]
  public Image eyeIcon;
  [SerializeField]
  public PhotoGalleryAlert _galleryAlert;
  [SerializeField]
  public GameObject _controls;
  [Header("Controls")]
  [SerializeField]
  public GameObject _tiltConsole;
  [SerializeField]
  public GameObject _tiltPC;
  [SerializeField]
  public Image transformObjectX;
  [SerializeField]
  public Image transformObjectY;
  [SerializeField]
  public Image transformObjectZ;
  public static UITakePhotoOverlayController instance;
  public Vector3 _takenPhotoPreviewStartPos;
  public bool _takenPhoto;

  public MMSlider FocusSlider => this.focusSlider;

  public MMSlider TiltSlider => this.tiltSlider;

  public Image TransformObjectX => this.transformObjectX;

  public Image TransformObjectY => this.transformObjectY;

  public Image TransformObjectZ => this.transformObjectZ;

  public override void Awake()
  {
    UITakePhotoOverlayController.instance = this;
    base.Awake();
    PhotoModeManager.OnPhotoTaken += new PhotoModeManager.PhotoTakenEvent(this.OnPhotoTaken);
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
    this.OnActiveControllerChanged(InputManager.General.GetLastActiveController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
    PhotoModeManager.OnPhotoSaved += new Action<string>(this.OnPhotoSaved);
    if (PhotoModeManager.ImageReadWriter.GetFiles().Length == 0)
      DataManager.Instance.Alerts.GalleryAlerts.ClearAll();
    if (!CheatConsole.HidingUI)
      return;
    this._controls.gameObject.SetActive(false);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    PhotoModeManager.OnPhotoTaken -= new PhotoModeManager.PhotoTakenEvent(this.OnPhotoTaken);
    PhotoModeManager.OnPhotoSaved -= new Action<string>(this.OnPhotoSaved);
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
    MonoSingleton<UIManager>.Instance.PhotoInformationBoxTemplate.DestroyAll<PhotoInformationBox>();
    this.CompletePreviousTakingPhotoSquence();
    this.ReleaseTexture();
  }

  public void toggleUI(bool show = false) => this.Canvas.enabled = show;

  public void OnActiveControllerChanged(Controller controller)
  {
    this._tiltConsole.SetActive(InputManager.General.InputIsController(controller));
    this._tiltPC.SetActive(!InputManager.General.InputIsController(controller));
  }

  public void OnPhotoSaved(string photo)
  {
    this._galleryAlert.ResetAlert();
    this._galleryAlert.ConfigureSingle();
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    this.SetActiveStateForMenu(true);
    this._takenPhotoPreviewStartPos = this.takenPhotoPreview.transform.position;
    this.iconForCameraSnap.color = StaticColors.GreyColor;
    Vector3 position = this.cameraIcon.transform.position;
    this.cameraIcon.transform.position = position + new Vector3(0.0f, 200f, 0.0f);
    this.cameraIcon.transform.DOKill();
    this.cameraIcon.transform.DOMove(position, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.fadeInImage.gameObject.SetActive(true);
    this.fadeInImage.color = Color.black;
    this.fadeInImage.DOKill();
    DOTweenModuleUI.DOColor(this.fadeInImage, new Color(0.0f, 0.0f, 0.0f, 0.0f), 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public override void OnShowStarted() => base.OnShowStarted();

  public void OnPhotoTaken(Texture2D photo)
  {
    this._takenPhoto = true;
    DataManager.Instance.PhotoUsedCamera = true;
    this.iconForCameraSnap.color = StaticColors.GreenColor;
    this.iconForCameraSnap.DOKill();
    DOTweenModuleUI.DOColor(this.iconForCameraSnap, StaticColors.GreyColor, 1f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.photoFlash.DOKill();
    this.photoFlash.color = new Color(this.photoFlash.color.r, this.photoFlash.color.g, this.photoFlash.color.b, 0.0f);
    if (SettingsManager.Settings != null && SettingsManager.Settings.Accessibility != null && SettingsManager.Settings.Accessibility.DarkMode)
      this.photoFlash.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    DOTweenModuleUI.DOFade(this.photoFlash, 1f, 0.1f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => DOTweenModuleUI.DOFade(this.photoFlash, 0.0f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true))).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.CompletePreviousTakingPhotoSquence();
    this.takenPhotoPreviewOutline.DOKill();
    this.takenPhotoPreview.DOKill();
    this.takenPhotoPreview.rectTransform.DOKill();
    this.takenPhotoPreview.texture = (Texture) photo;
    this.takenPhotoPreview.transform.position = this._takenPhotoPreviewStartPos + new Vector3(0.0f, 200f, 0.0f);
    this.takenPhotoPreview.rectTransform.DOMove(this._takenPhotoPreviewStartPos, 2f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.25f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => PhotoModeManager.TakeScreenShotActive = false));
    this.takenPhotoPreview.color = new Color(this.takenPhotoPreview.color.r, this.takenPhotoPreview.color.g, this.takenPhotoPreview.color.b, 0.0f);
    this.takePhotoSequence = DOTween.Sequence();
    this.takePhotoSequence.Append((Tween) DOTweenModuleUI.DOFade(this.takenPhotoPreviewOutline, 1f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true));
    this.takePhotoSequence.Join((Tween) this.takenPhotoPreview.DOFade(1f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true));
    this.takePhotoSequence.Append((Tween) DOTweenModuleUI.DOFade(this.takenPhotoPreviewOutline, 0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetDelay<TweenerCore<Color, Color, ColorOptions>>(2f));
    this.takePhotoSequence.Join((Tween) this.takenPhotoPreview.DOFade(0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetDelay<TweenerCore<Color, Color, ColorOptions>>(2f));
    this.takePhotoSequence.OnComplete<DG.Tweening.Sequence>((TweenCallback) (() => this.RemovePhotoFromMemory(photo)));
    this.takePhotoSequence.SetUpdate<DG.Tweening.Sequence>(true);
    this.takePhotoSequence.Play<DG.Tweening.Sequence>();
    this.eyeIcon.transform.localScale = new Vector3(1f, 0.0f, 1f);
    this.eyeIcon.transform.DOScale(Vector3.one, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.UpdateTakePhotoObjectives();
  }

  public void CompletePreviousTakingPhotoSquence()
  {
    if (this.takePhotoSequence == null || !this.takePhotoSequence.IsPlaying())
      return;
    this.takePhotoSequence.Complete();
    this.takePhotoSequence.Kill();
  }

  public void RemovePhotoFromMemory(Texture2D photo)
  {
    this.ReleaseTexture();
    if (!((UnityEngine.Object) photo != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) photo);
  }

  public void ReleaseTexture()
  {
    if (!((UnityEngine.Object) this.takenPhotoPreview != (UnityEngine.Object) null) || !((UnityEngine.Object) this.takenPhotoPreview.texture != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.takenPhotoPreview.texture);
    this.takenPhotoPreview.texture = (Texture) null;
  }

  public override void OnCancelButtonInput()
  {
    if (PhotoModeManager.TakeScreenShotActive || !this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public void Update()
  {
    Time.timeScale = 0.0f;
    if (PhotoModeManager.CurrentPhotoState != PhotoModeManager.PhotoState.TakePhoto || PhotoModeManager.TakeScreenShotActive || !InputManager.PhotoMode.GetGalleryFolderButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      return;
    this.OpenGallery();
  }

  public void OpenGallery()
  {
    if (this.IsHiding)
      return;
    DataManager.Instance.PhotoCameraLookedAtGallery = true;
    PhotoModeManager.CurrentPhotoState = PhotoModeManager.PhotoState.Gallery;
    UIPhotoGalleryMenuController gallery = this.Push<UIPhotoGalleryMenuController>(MonoSingleton<UIManager>.Instance.PhotoGalleryMenuTemplate);
    UIPhotoGalleryMenuController galleryMenuController = gallery;
    galleryMenuController.OnHidden = galleryMenuController.OnHidden + (System.Action) (() =>
    {
      gallery = (UIPhotoGalleryMenuController) null;
      PhotoModeManager.CurrentPhotoState = PhotoModeManager.PhotoState.TakePhoto;
    });
  }

  public void UpdateTakePhotoObjectives()
  {
    if (PlayerFarming.Location != FollowerLocation.Base)
      return;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.TakeCultsPhoto);
  }
}
