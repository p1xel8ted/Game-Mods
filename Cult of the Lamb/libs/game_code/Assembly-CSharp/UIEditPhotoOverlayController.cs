// Decompiled with JetBrains decompiler
// Type: UIEditPhotoOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.ReadWrite;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using src.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UIEditPhotoOverlayController : UIMenuBase
{
  public Action<PhotoModeManager.PhotoData> OnNewPhotoCreated;
  [SerializeField]
  public Canvas canvas;
  [SerializeField]
  public RawImage image;
  [SerializeField]
  public Transform stickerContent;
  [SerializeField]
  public StickerItem stickerItemTemplate;
  [SerializeField]
  public Image photoFlash;
  [SerializeField]
  public RawImage takenPhotoPreview;
  [SerializeField]
  public Image takenPhotoPreviewOutline;
  [SerializeField]
  public RectTransform _standardPrompts;
  [SerializeField]
  public RectTransform _editStickersPrompts;
  [SerializeField]
  public float movementSpeed;
  [SerializeField]
  public float rotateSpeed;
  [SerializeField]
  public float scaleSpeed;
  [SerializeField]
  public RectTransform PhotoIcon;
  [SerializeField]
  public GameObject controls;
  [SerializeField]
  public GameObject backControlPrompt;
  public List<StickerItem> stickerItems = new List<StickerItem>();
  public PhotoModeManager.PhotoData photoData;
  public StickerItem currentSticker;
  public float cachedScale;
  public bool stickerEnteredPhotoZone;
  public bool reselecting;
  public Tween tween;
  public List<StickerItem> placedStickers = new List<StickerItem>();
  public StickerItem CacheCurrentSticker;
  public bool inputsDisabled;
  public bool _placementToggle;
  public Texture2D FullResScreenshot;

  public void Show(PhotoModeManager.PhotoData photoData, bool immediate = false)
  {
    this.photoData = photoData;
    this.image.texture = (Texture) photoData.PhotoTexture;
    this.Show(immediate);
    this._standardPrompts.transform.DOKill();
    this._editStickersPrompts.transform.DOKill();
    this._editStickersPrompts.transform.localPosition = new Vector3(0.0f, -200f, 0.0f);
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this._standardPrompts.DOKill();
    this._standardPrompts.anchoredPosition = (Vector2) new Vector3(0.0f, -200f, 0.0f);
    this._standardPrompts.DOAnchorPos((Vector2) Vector3.zero, 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
    this.PhotoIcon.anchoredPosition = (Vector2) new Vector3(0.0f, 200f, 0.0f);
    this.PhotoIcon.DOKill();
    this.PhotoIcon.DOAnchorPos((Vector2) Vector3.zero, 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
    if (CheatConsole.HidingUI)
    {
      this.controls.gameObject.SetActive(false);
      this.backControlPrompt.gameObject.SetActive(false);
    }
    foreach (StickerData allSticker in PhotoModeManager.GetAllStickers())
    {
      StickerData stickerData = allSticker;
      StickerItem stickerItem = UnityEngine.Object.Instantiate<StickerItem>(this.stickerItemTemplate, this.stickerContent);
      this.stickerItems.Add(stickerItem);
      stickerItem.Button.onClick.AddListener((UnityAction) (() =>
      {
        if (InputManager.PhotoMode.GetPlaceStickerButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          this._placementToggle = true;
        this.SpawnSticker(stickerData);
        MonoSingleton<UINavigatorNew>.Instance.Clear();
      }));
      stickerItem.Configure(stickerData, this);
    }
    this.OverrideDefault((Selectable) this.stickerItems[0].Button);
    this.ActivateNavigation();
  }

  public void Update()
  {
    if (this.inputsDisabled || PhotoModeCamera.Instance.IsErrorShown || PhotoModeManager.TakeScreenShotActive)
      return;
    if ((UnityEngine.Object) this.currentSticker != (UnityEngine.Object) null)
    {
      if (!this._placementToggle)
      {
        if (InputManager.PhotoMode.GetPlaceStickerButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        {
          if (((RectTransform) this.image.transform).rect.Contains(this.image.transform.InverseTransformPoint(this.currentSticker.transform.position)))
            this.PlaceSticker(this.currentSticker);
          if ((UnityEngine.Object) this.currentSticker == (UnityEngine.Object) null)
            return;
        }
      }
      else if (InputManager.PhotoMode.GetPlaceStickerButtonUp(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        this._placementToggle = false;
      if (InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        this.currentSticker.transform.position += (Vector3) new Vector2(InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer), InputManager.UI.GetVerticalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer)) * this.movementSpeed * Time.unscaledDeltaTime;
      else
        this.currentSticker.transform.position = (Vector3) InputManager.General.GetMousePosition(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
      Vector3 localScale = this.currentSticker.transform.localScale;
      if ((double) Mathf.Abs(InputManager.PhotoMode.GetStickerScaleAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer)) > 0.0)
      {
        this.currentSticker.transform.DOComplete();
        Vector3 vector3 = localScale + Vector3.one * this.scaleSpeed * 3f * Time.unscaledDeltaTime * InputManager.PhotoMode.GetStickerScaleAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
        vector3.x = Mathf.Clamp(vector3.x, this.currentSticker.StickerData.MinScale, this.currentSticker.StickerData.MaxScale);
        vector3.y = Mathf.Clamp(vector3.y, this.currentSticker.StickerData.MinScale, this.currentSticker.StickerData.MaxScale);
        this.currentSticker.transform.localScale = vector3;
        this.cachedScale = vector3.x;
      }
      this.currentSticker.transform.Rotate(0.0f, 0.0f, this.rotateSpeed * 2f * this.movementSpeed * Time.unscaledDeltaTime * InputManager.PhotoMode.GetStickerRotateAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
      if (InputManager.PhotoMode.GetUndoButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        this.UndoStickerPlacement();
      if (InputManager.PhotoMode.GetFlipStickerButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        this.currentSticker.Flip();
      if (!InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      {
        bool flag = ((RectTransform) this.image.transform).rect.Contains(this.image.transform.InverseTransformPoint(this.currentSticker.transform.position));
        if (flag || !this.stickerEnteredPhotoZone)
        {
          this.stickerEnteredPhotoZone = flag;
          if ((double) this.currentSticker.transform.localScale.x <= 0.0 && this.tween != null)
          {
            this.currentSticker.transform.DOKill();
            this.tween = (Tween) this.currentSticker.transform.DOScale(this.cachedScale, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
            this.DisableStickers();
            this.reselecting = false;
          }
        }
        else if (this.stickerEnteredPhotoZone && (double) this.currentSticker.transform.localScale.x > 0.0 && (this.tween == null || !this.tween.IsPlaying()))
        {
          this.currentSticker.transform.DOKill();
          this.tween = (Tween) this.currentSticker.transform.DOScale(0.0f, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
          this.EnableStickers();
          this.reselecting = true;
        }
      }
    }
    else
    {
      if (InputManager.PhotoMode.GetSaveButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        this.SaveNewPhoto();
      if (InputManager.PhotoMode.GetClearStickersButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        this.ClearStickers();
    }
    if (!((UnityEngine.Object) this.CacheCurrentSticker != (UnityEngine.Object) this.currentSticker))
      return;
    this.CacheCurrentSticker = this.currentSticker;
    if ((UnityEngine.Object) this.currentSticker == (UnityEngine.Object) null)
    {
      this._standardPrompts.DOKill();
      this._editStickersPrompts.DOKill();
      this._standardPrompts.DOAnchorPos((Vector2) Vector3.zero, 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
      this._editStickersPrompts.DOAnchorPos((Vector2) new Vector3(0.0f, -200f, 0.0f), 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
    }
    else
    {
      this._standardPrompts.DOKill();
      this._editStickersPrompts.DOKill();
      this._standardPrompts.DOAnchorPos((Vector2) new Vector3(0.0f, -200f, 0.0f), 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
      this._editStickersPrompts.DOAnchorPos((Vector2) Vector3.zero, 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
    }
  }

  public override void OnCancelButtonInput()
  {
    if (this.inputsDisabled || PhotoModeManager.TakeScreenShotActive)
      return;
    base.OnCancelButtonInput();
    if ((UnityEngine.Object) this.currentSticker != (UnityEngine.Object) null)
    {
      this.CancelSticker();
    }
    else
    {
      if (!((UnityEngine.Object) this.currentSticker == (UnityEngine.Object) null) || !this._canvasGroup.interactable)
        return;
      this.DisableAllInputs();
      this.Hide();
    }
  }

  public void ClearStickers()
  {
    for (int index = this.image.transform.childCount - 1; index >= 0; --index)
    {
      GameObject gameObject = this.image.transform.GetChild(index).gameObject;
      if ((UnityEngine.Object) this.currentSticker == (UnityEngine.Object) null || (UnityEngine.Object) gameObject != (UnityEngine.Object) this.currentSticker.gameObject)
        gameObject.gameObject.SetActive(false);
    }
  }

  public void SaveNewPhoto()
  {
    PhotoModeManager.TakeScreenShotActive = true;
    this.StartCoroutine((IEnumerator) this.SaveNewPhotoIE());
  }

  public IEnumerator SaveNewPhotoIE()
  {
    UIEditPhotoOverlayController overlayController = this;
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive");
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    Transform parent = overlayController.image.transform.parent;
    Vector3 s = overlayController.image.transform.localScale;
    overlayController.image.transform.SetParent(overlayController._canvas.transform);
    overlayController.image.transform.SetAsLastSibling();
    overlayController.image.transform.localScale = Vector3.one;
    overlayController.image.transform.localPosition = Vector3.zero;
    overlayController.canvas.worldCamera = PhotoModeCamera.Instance.Camera;
    overlayController.canvas.renderMode = RenderMode.ScreenSpaceCamera;
    overlayController.canvas.gameObject.layer = LayerMask.NameToLayer("Default");
    overlayController.canvas.planeDistance = 1f;
    BiomeConstants.Instance.ppv.enabled = false;
    overlayController.currentSticker?.gameObject.SetActive(false);
    RenderTexture originalCameraTargetTexture = PhotoModeCamera.Instance.Camera.targetTexture;
    RenderTexture originalActiveRenderTexture = RenderTexture.active;
    RenderTexture rt = RenderTexture.GetTemporary(PhotoModeManager.ResolutionX, PhotoModeManager.ResolutionY, 0);
    Texture2D screenshot = new Texture2D(PhotoModeManager.ResolutionX, PhotoModeManager.ResolutionY, TextureFormat.RGB24, false);
    yield return (object) new WaitForEndOfFrame();
    PhotoModeCamera.Instance.Camera.targetTexture = rt;
    PhotoModeCamera.Instance.Camera.Render();
    PhotoModeCamera.Instance.Camera.targetTexture = originalCameraTargetTexture;
    yield return (object) new WaitForEndOfFrame();
    RenderTexture.active = rt;
    screenshot.ReadPixels(new Rect(0.0f, 0.0f, (float) PhotoModeManager.ResolutionX, (float) PhotoModeManager.ResolutionY), 0, 0);
    screenshot.Apply();
    yield return (object) new WaitForEndOfFrame();
    RenderTexture.active = originalActiveRenderTexture;
    RenderTexture.ReleaseTemporary(rt);
    rt = (RenderTexture) null;
    yield return (object) new WaitForEndOfFrame();
    overlayController.image.transform.SetParent(parent);
    overlayController.image.transform.localScale = s;
    overlayController.image.transform.localPosition = Vector3.zero;
    overlayController.canvas.worldCamera = (Camera) null;
    overlayController.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    overlayController.canvas.gameObject.layer = LayerMask.NameToLayer("UI");
    overlayController.canvas.planeDistance = 100f;
    PhotoModeCamera.Instance.Camera.cullingMask |= 1 << LayerMask.NameToLayer("UI");
    BiomeConstants.Instance.ppv.enabled = true;
    overlayController.currentSticker?.gameObject.SetActive(true);
    yield return (object) null;
    string fileName = overlayController.GetScreenShotName();
    PhotoModeCamera.PhotoReadWriteResult result = PhotoModeCamera.PhotoReadWriteResult.None;
    COTLImageReadWriter imageReadWriter1 = PhotoModeManager.ImageReadWriter;
    imageReadWriter1.OnWriteError = imageReadWriter1.OnWriteError + (Action<MMReadWriteError>) (writeError => result = PhotoModeCamera.PhotoReadWriteResult.Error);
    COTLImageReadWriter imageReadWriter2 = PhotoModeManager.ImageReadWriter;
    imageReadWriter2.OnWriteCompleted = imageReadWriter2.OnWriteCompleted + (System.Action) (() => result = PhotoModeCamera.PhotoReadWriteResult.Success);
    PhotoModeManager.ImageReadWriter.Write(screenshot, fileName);
    Action<string> onPhotoSaved = PhotoModeManager.OnPhotoSaved;
    if (onPhotoSaved != null)
      onPhotoSaved(fileName);
    screenshot.name = fileName;
    while (result == PhotoModeCamera.PhotoReadWriteResult.None)
      yield return (object) null;
    COTLImageReadWriter imageReadWriter3 = PhotoModeManager.ImageReadWriter;
    imageReadWriter3.OnWriteError = imageReadWriter3.OnWriteError - (Action<MMReadWriteError>) (writeError => result = PhotoModeCamera.PhotoReadWriteResult.Error);
    COTLImageReadWriter imageReadWriter4 = PhotoModeManager.ImageReadWriter;
    imageReadWriter4.OnWriteCompleted = imageReadWriter4.OnWriteCompleted - (System.Action) (() => result = PhotoModeCamera.PhotoReadWriteResult.Success);
    if (result == PhotoModeCamera.PhotoReadWriteResult.Error)
    {
      PhotoModeCamera.Instance.IsErrorShown = true;
      PhotoModeManager.ImageReadWriter.Delete(fileName);
      UIMenuConfirmationWindow menu = overlayController.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
      menu.Configure(ScriptLocalization.UI_PhotoMode_Error_Write.Heading, ScriptLocalization.UI_PhotoMode_Error_Write.Heading, true);
      UIMenuConfirmationWindow confirmationWindow = menu;
      confirmationWindow.OnHide = confirmationWindow.OnHide + (System.Action) (() =>
      {
        PhotoModeCamera.Instance.IsErrorShown = false;
        UnityEngine.Object.Destroy((UnityEngine.Object) screenshot);
        screenshot = (Texture2D) null;
        this.OnCancelButtonInput();
        PhotoModeManager.TakeScreenShotActive = false;
      });
      yield return (object) menu.YieldUntilHidden();
    }
    else
    {
      PhotoModeManager.PhotoData photoData = new PhotoModeManager.PhotoData()
      {
        PhotoName = fileName,
        PhotoTexture = screenshot
      };
      Action<PhotoModeManager.PhotoData> onNewPhotoCreated = overlayController.OnNewPhotoCreated;
      if (onNewPhotoCreated != null)
        onNewPhotoCreated(photoData);
      overlayController.OnPhotoSaved(screenshot);
    }
    PhotoModeManager.TakeScreenShotActive = false;
  }

  public void OnPhotoSaved(Texture2D photo)
  {
    this.photoFlash.DOKill();
    this.photoFlash.color = new Color(this.photoFlash.color.r, this.photoFlash.color.g, this.photoFlash.color.b, 0.0f);
    DOTweenModuleUI.DOFade(this.photoFlash, 1f, 0.1f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => DOTweenModuleUI.DOFade(this.photoFlash, 0.0f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true))).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.takenPhotoPreviewOutline.DOKill();
    this.takenPhotoPreview.DOKill();
    this.takenPhotoPreview.texture = (Texture) photo;
    this.takenPhotoPreview.color = new Color(this.takenPhotoPreview.color.r, this.takenPhotoPreview.color.g, this.takenPhotoPreview.color.b, 0.0f);
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.Append((Tween) DOTweenModuleUI.DOFade(this.takenPhotoPreviewOutline, 1f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true));
    sequence.Join((Tween) this.takenPhotoPreview.DOFade(1f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true));
    sequence.Append((Tween) DOTweenModuleUI.DOFade(this.takenPhotoPreviewOutline, 0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetDelay<TweenerCore<Color, Color, ColorOptions>>(2f));
    sequence.Join((Tween) this.takenPhotoPreview.DOFade(0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetDelay<TweenerCore<Color, Color, ColorOptions>>(2f));
    sequence.OnComplete<DG.Tweening.Sequence>((TweenCallback) (() => PhotoModeManager.TakeScreenShotActive = false));
    sequence.SetUpdate<DG.Tweening.Sequence>(true);
    sequence.Play<DG.Tweening.Sequence>();
  }

  public string GetScreenShotName()
  {
    int num = 0;
    string filename;
    do
    {
      ++num;
      filename = this.photoData.PhotoName + $"_{num}";
    }
    while (PhotoModeManager.ImageReadWriter.FileExists(filename));
    return filename;
  }

  public void SpawnSticker(StickerData stickerData)
  {
    if (this.reselecting)
      this.CancelSticker();
    StickerItem stickerItem = UnityEngine.Object.Instantiate<StickerItem>(!stickerData.UsePrefab || !((UnityEngine.Object) stickerData.Prefab != (UnityEngine.Object) null) ? this.stickerItemTemplate : stickerData.Prefab, this.image.transform, (bool) (UnityEngine.Object) this.image.transform);
    stickerItem.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f).SetUpdate<Tweener>(true);
    stickerItem.Configure(stickerData, this);
    UnityEngine.Object.Destroy((UnityEngine.Object) stickerItem.Button);
    stickerItem.GetComponent<Image>().raycastTarget = false;
    if (!InputManager.General.MouseInputActive)
      stickerItem.transform.position = (UnityEngine.Object) this.currentSticker != (UnityEngine.Object) null ? this.currentSticker.transform.position : this.image.transform.position;
    if ((UnityEngine.Object) this.currentSticker != (UnityEngine.Object) null)
    {
      this.currentSticker.transform.DOKill(true);
      stickerItem.transform.rotation = this.currentSticker.transform.rotation;
      stickerItem.transform.localScale = this.currentSticker.transform.localScale;
      stickerItem.Transform.localScale = this.currentSticker.Transform.localScale;
      stickerItem.Image.transform.localScale = this.currentSticker.Image.transform.localScale;
    }
    else
    {
      stickerItem.transform.localScale = Vector3.one * 1.5f;
      this.cachedScale = 1.5f;
    }
    this.stickerEnteredPhotoZone = false;
    this.reselecting = false;
    this.tween = (Tween) null;
    this.currentSticker = stickerItem;
    this.DisableStickers();
    AudioManager.Instance.PlayOneShot("event:/ui/photo_mode/sticker_place");
    MMVibrate.Haptic(MMVibrate.HapticTypes.Selection);
  }

  public void PlaceSticker(StickerItem stickerItem)
  {
    this.SpawnSticker(stickerItem.StickerData);
    this.placedStickers.Add(stickerItem);
    stickerItem.OnStickerPlaced();
  }

  public void CancelSticker()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/go_back");
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    this.EnableStickers();
    foreach (StickerItem stickerItem in this.stickerItems)
    {
      if ((UnityEngine.Object) stickerItem.StickerData == (UnityEngine.Object) this.currentSticker.StickerData)
      {
        this.OverrideDefault((Selectable) stickerItem.Button);
        this.ActivateNavigation();
        break;
      }
    }
    this.currentSticker.gameObject.SetActive(false);
    this.currentSticker = (StickerItem) null;
    this.cachedScale = 1.5f;
    this.stickerEnteredPhotoZone = false;
    this.reselecting = false;
  }

  public void UndoStickerPlacement()
  {
    if (this.placedStickers.Count <= 0)
      return;
    StickerItem placedSticker = this.placedStickers[this.placedStickers.Count - 1];
    this.placedStickers.RemoveAt(this.placedStickers.Count - 1);
    placedSticker.gameObject.SetActive(false);
  }

  public void EnableStickers()
  {
    foreach (StickerItem stickerItem in this.stickerItems)
      stickerItem.Button.Interactable = true;
  }

  public void DisableStickers()
  {
    foreach (StickerItem stickerItem in this.stickerItems)
    {
      stickerItem.Button.Interactable = false;
      stickerItem.DeSelectSticker();
    }
  }

  public void MoveToSticker(StickerData stickerData)
  {
    IMMSelectable newSelectable = (IMMSelectable) null;
    foreach (StickerItem stickerItem in this.stickerItems)
    {
      if ((UnityEngine.Object) stickerItem.StickerData == (UnityEngine.Object) stickerData)
      {
        newSelectable = (IMMSelectable) stickerItem.Button;
        break;
      }
    }
    if (newSelectable == null)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(newSelectable);
  }

  public IMMSelectable GetSticker(StickerData stickerData)
  {
    foreach (StickerItem stickerItem in this.stickerItems)
    {
      if ((UnityEngine.Object) stickerItem.StickerData == (UnityEngine.Object) stickerData)
        return (IMMSelectable) stickerItem.Button;
    }
    return (IMMSelectable) null;
  }

  public void DisableAllInputs()
  {
    this.DisableStickers();
    this.inputsDisabled = true;
  }

  public void EnableAllInputs()
  {
    this.EnableStickers();
    this.inputsDisabled = false;
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override void OnHideCompleted()
  {
    this.image.texture = (Texture) null;
    if ((UnityEngine.Object) this.FullResScreenshot != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.FullResScreenshot);
    this.FullResScreenshot = (Texture2D) null;
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003COnPhotoSaved\u003Eb__35_0()
  {
    DOTweenModuleUI.DOFade(this.photoFlash, 0.0f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }
}
