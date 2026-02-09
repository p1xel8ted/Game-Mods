// Decompiled with JetBrains decompiler
// Type: UIPhotoGalleryMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.ReadWrite;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using src.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIPhotoGalleryMenuController : UIMenuBase
{
  [SerializeField]
  public Transform _contentContainer;
  [SerializeField]
  public Transform _CardcontentContainer;
  [SerializeField]
  public Transform noPhotos;
  [SerializeField]
  public GameObject locatePrompt;
  [SerializeField]
  public GameObject DeletePrompt;
  [SerializeField]
  public GameObject AcceptPrompt;
  [SerializeField]
  public GameObject SharePrompt;
  [SerializeField]
  public Localize SharePromptLocalize;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public GameObject _controlPrompts;
  [Header("Photo Counter")]
  [SerializeField]
  public GameObject _counterContainer;
  [SerializeField]
  public TMP_Text _photosTaken;
  public List<PhotoInformationBox> photoBoxes = new List<PhotoInformationBox>();
  public PhotoModeManager.PhotoData currentHoveredPhoto;
  public PhotoInformationBox _currentPhoto;
  public Coroutine _loadPhotosCoroutine;
  public bool PhotoIsSharing;
  public List<Texture2D> _texturesToDestroy = new List<Texture2D>();
  public float InputTimeOut;
  public float InputTimeOutTime = 0.5f;
  public bool GalleryNotInFocus;
  public bool photoSelectedPerformingAction;

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this.locatePrompt.gameObject.SetActive(true);
    this._counterContainer.SetActive(true);
    this.SharePrompt.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/ui/open_menu");
    this.noPhotos.gameObject.SetActive(false);
    this.photoBoxes = new List<PhotoInformationBox>();
    this._loadPhotosCoroutine = this.StartCoroutine((IEnumerator) this.LoadPhotos());
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.XboxOne:
      case UnifyManager.Platform.GameCore:
        this.SharePromptLocalize.Term = "UI/PhotoMode/Share_XBOX";
        break;
      case UnifyManager.Platform.PS4:
      case UnifyManager.Platform.PS5:
        this.SharePromptLocalize.Term = "UI/PhotoMode/Share_PLAYSTATION";
        break;
      case UnifyManager.Platform.Switch:
        this.SharePromptLocalize.Term = "UI/PhotoMode/Share_SWITCH";
        break;
    }
    this.InputTimeOut = Time.unscaledTime;
  }

  public override void OnPush()
  {
    base.OnPush();
    this._controlPrompts.SetActive(false);
  }

  public override void OnRelease()
  {
    base.OnRelease();
    this._controlPrompts.SetActive(true);
  }

  public IEnumerator LoadPhotos()
  {
    UIPhotoGalleryMenuController galleryMenuController = this;
    galleryMenuController.UpdatePhotoText();
    string[] filepaths = PhotoModeManager.ImageReadWriter.GetFiles();
    int errors = 0;
    galleryMenuController.DeletePrompt.gameObject.SetActive(filepaths.Length != 0);
    galleryMenuController.AcceptPrompt.gameObject.SetActive(filepaths.Length != 0);
    string[] strArray = filepaths;
    for (int index = 0; index < strArray.Length; ++index)
    {
      string file = strArray[index];
      if (!string.IsNullOrEmpty(file))
      {
        PhotoModeCamera.PhotoReadWriteResult result = PhotoModeCamera.PhotoReadWriteResult.None;
        COTLImageReadWriter imageReadWriter1 = PhotoModeManager.ImageReadWriter;
        imageReadWriter1.OnReadCompleted = imageReadWriter1.OnReadCompleted + (Action<Texture2D>) (texture2D =>
        {
          this.photoBoxes.Add(this.MakePhoto(new PhotoModeManager.PhotoData()
          {
            PhotoName = file,
            PhotoTexture = texture2D
          }, filepaths.IndexOf<string>(file) != 0));
          if (this.photoBoxes.Count == 1)
          {
            this.OverrideDefault((Selectable) this.photoBoxes[0].Button);
            this.ActivateNavigation();
          }
          result = PhotoModeCamera.PhotoReadWriteResult.Success;
        });
        COTLImageReadWriter imageReadWriter2 = PhotoModeManager.ImageReadWriter;
        imageReadWriter2.OnReadError = imageReadWriter2.OnReadError + (Action<MMReadWriteError>) (readWriteError => result = PhotoModeCamera.PhotoReadWriteResult.Error);
        PhotoModeManager.ImageReadWriter.Read(file);
        while (result == PhotoModeCamera.PhotoReadWriteResult.None)
          yield return (object) new WaitForEndOfFrame();
        COTLImageReadWriter imageReadWriter3 = PhotoModeManager.ImageReadWriter;
        imageReadWriter3.OnReadCompleted = imageReadWriter3.OnReadCompleted - (Action<Texture2D>) (texture2D =>
        {
          this.photoBoxes.Add(this.MakePhoto(new PhotoModeManager.PhotoData()
          {
            PhotoName = file,
            PhotoTexture = texture2D
          }, filepaths.IndexOf<string>(file) != 0));
          if (this.photoBoxes.Count == 1)
          {
            this.OverrideDefault((Selectable) this.photoBoxes[0].Button);
            this.ActivateNavigation();
          }
          result = PhotoModeCamera.PhotoReadWriteResult.Success;
        });
        COTLImageReadWriter imageReadWriter4 = PhotoModeManager.ImageReadWriter;
        imageReadWriter4.OnReadError = imageReadWriter4.OnReadError - (Action<MMReadWriteError>) (readWriteError => result = PhotoModeCamera.PhotoReadWriteResult.Error);
        if (result == PhotoModeCamera.PhotoReadWriteResult.Error)
          ++errors;
        yield return (object) new WaitForEndOfFrame();
        galleryMenuController.UpdatePhotoText();
      }
    }
    strArray = (string[]) null;
    if (errors > 0)
    {
      PhotoModeCamera.Instance.IsErrorShown = true;
      UIMenuConfirmationWindow menu = galleryMenuController.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
      menu.Configure(ScriptLocalization.UI_PhotoMode_Error_Read.Heading, ScriptLocalization.UI_PhotoMode_Error_Read.Description, true);
      UIMenuConfirmationWindow confirmationWindow = menu;
      confirmationWindow.OnHide = confirmationWindow.OnHide + (System.Action) (() => PhotoModeCamera.Instance.IsErrorShown = false);
      yield return (object) menu.YieldUntilHidden();
    }
    galleryMenuController.noPhotos.gameObject.SetActive(galleryMenuController.photoBoxes.Count == 0);
    galleryMenuController._CardcontentContainer.gameObject.SetActive(galleryMenuController.photoBoxes.Count > 0);
    galleryMenuController.UpdatePhotoText();
    galleryMenuController._loadPhotosCoroutine = (Coroutine) null;
  }

  public void Update()
  {
    if (PhotoModeCamera.Instance.IsErrorShown || this.PhotoIsSharing || this.GalleryNotInFocus)
      return;
    if (this.currentHoveredPhoto != null && InputManager.PhotoMode.GetDeletePhotoButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && this._canvasGroup.interactable && this.photoBoxes.Count > 0 && (double) Time.unscaledTime - (double) this.InputTimeOut > (double) this.InputTimeOutTime)
    {
      this.InputTimeOut = Time.unscaledTime;
      UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
      confirmationWindow.Configure(ScriptLocalization.UI_PhotoMode.Discard, ScriptLocalization.UI_PhotoMode_Discard.Description);
      confirmationWindow.OnConfirm += (System.Action) (() =>
      {
        int index1 = this.photoBoxes.IndexOf(this._currentPhoto);
        this.photoBoxes[index1].Recycle<PhotoInformationBox>();
        this.photoBoxes.RemoveAt(index1);
        PhotoModeManager.DeletePhoto(this.currentHoveredPhoto.PhotoName);
        int index2 = this.photoBoxes.Count <= index1 ? index1 - 1 : index1;
        if (this.photoBoxes.Count > index2 && index2 > 0)
        {
          this.OverrideDefault((Selectable) this.photoBoxes[index2].Button);
          this.ActivateNavigation();
        }
        else if (this.photoBoxes.Count > 0)
        {
          this.OverrideDefault((Selectable) this.photoBoxes[0].Button);
          this.ActivateNavigation();
        }
        this.noPhotos.gameObject.SetActive(this.photoBoxes.Count == 0);
        this._CardcontentContainer.gameObject.SetActive(this.photoBoxes.Count > 0);
        this.UpdatePhotoText();
        this.DeletePrompt.gameObject.SetActive(this.photoBoxes.Count > 0);
        this.AcceptPrompt.gameObject.SetActive(this.photoBoxes.Count > 0);
        if (this.photoBoxes.Count == 0)
          DataManager.Instance.Alerts.GalleryAlerts.ClearAll();
        this.InputTimeOut = Time.unscaledTime;
      });
    }
    if (!InputManager.PhotoMode.GetGalleryFolderButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || !this._canvasGroup.interactable)
      return;
    string directory = MMImageDataReadWriter.GetDirectory();
    if (!Directory.Exists(directory))
      Directory.CreateDirectory(directory);
    Application.OpenURL(directory);
  }

  public void OnPhotoHovered(PhotoInformationBox photoInformationBox)
  {
    this._currentPhoto = photoInformationBox;
    this.currentHoveredPhoto = photoInformationBox.PhotoData;
  }

  public void OnPhotoSelected(PhotoInformationBox photoInformationBox)
  {
    if ((double) Time.unscaledTime - (double) this.InputTimeOut < (double) this.InputTimeOutTime || this.photoSelectedPerformingAction)
      return;
    this.InputTimeOut = Time.unscaledTime + (this.InputTimeOutTime - 0.1f);
    PhotoModeManager.CurrentPhotoState = PhotoModeManager.PhotoState.EditPhoto;
    this.photoSelectedPerformingAction = this.GalleryNotInFocus = true;
    UIEditPhotoOverlayController overlayController = this.Push<UIEditPhotoOverlayController>(MonoSingleton<UIManager>.Instance.EditPhotoMenuTemplate);
    overlayController.OnHide = overlayController.OnHide + (System.Action) (() => this.InputTimeOut = Time.unscaledTime + this.InputTimeOutTime);
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      PhotoModeManager.CurrentPhotoState = PhotoModeManager.PhotoState.Gallery;
      this.GalleryNotInFocus = false;
    });
    overlayController.OnShownCompleted = overlayController.OnShownCompleted + (System.Action) (() =>
    {
      this.photoSelectedPerformingAction = false;
      this.GalleryNotInFocus = false;
    });
    overlayController.Show(photoInformationBox.PhotoData);
    overlayController.OnNewPhotoCreated += (Action<PhotoModeManager.PhotoData>) (data =>
    {
      this._texturesToDestroy.Add(data.PhotoTexture);
      PhotoInformationBox photoInformationBox1 = this.MakePhoto(data, false);
      photoInformationBox1.Button.SetInteractionState(false);
      photoInformationBox1.transform.SetAsFirstSibling();
      this.photoBoxes.Insert(0, photoInformationBox1);
      this.noPhotos.gameObject.SetActive(this.photoBoxes.Count == 0);
      this._CardcontentContainer.gameObject.SetActive(this.photoBoxes.Count > 0);
      this.UpdatePhotoText();
    });
  }

  public PhotoInformationBox MakePhoto(PhotoModeManager.PhotoData photoData, bool removeOnHover)
  {
    PhotoInformationBox photoInformationBox = MonoSingleton<UIManager>.Instance.PhotoInformationBoxTemplate.Spawn<PhotoInformationBox>(this._contentContainer);
    photoInformationBox.transform.localScale = Vector3.one;
    photoInformationBox.OnPhotoSelected += new Action<PhotoInformationBox>(this.OnPhotoSelected);
    photoInformationBox.OnPhotoHovered += new Action<PhotoInformationBox>(this.OnPhotoHovered);
    photoInformationBox.Configure(photoData, removeOnHover);
    return photoInformationBox;
  }

  public override void OnCancelButtonInput()
  {
    base.OnCancelButtonInput();
    if (!this._canvasGroup.interactable)
      return;
    UIManager.PlayAudio("event:/ui/go_back");
    this._CardcontentContainer.gameObject.SetActive(false);
    this.Hide();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    this._scrollRect.enabled = false;
    if (this._loadPhotosCoroutine != null)
    {
      this.StopCoroutine(this._loadPhotosCoroutine);
      this._loadPhotosCoroutine = (Coroutine) null;
    }
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    foreach (PhotoInformationBox photoBox in this.photoBoxes)
      photoBox.Recycle<PhotoInformationBox>();
    this.photoBoxes = new List<PhotoInformationBox>();
    foreach (UnityEngine.Object @object in this._texturesToDestroy)
      UnityEngine.Object.Destroy(@object);
    this._texturesToDestroy.Clear();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void UpdatePhotoText()
  {
    this._photosTaken.text = string.Format(ScriptLocalization.UI_PhotoMode.Photos.Replace("/{1}", ""), (object) this.photoBoxes.Count);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__25_0()
  {
    int index1 = this.photoBoxes.IndexOf(this._currentPhoto);
    this.photoBoxes[index1].Recycle<PhotoInformationBox>();
    this.photoBoxes.RemoveAt(index1);
    PhotoModeManager.DeletePhoto(this.currentHoveredPhoto.PhotoName);
    int index2 = this.photoBoxes.Count <= index1 ? index1 - 1 : index1;
    if (this.photoBoxes.Count > index2 && index2 > 0)
    {
      this.OverrideDefault((Selectable) this.photoBoxes[index2].Button);
      this.ActivateNavigation();
    }
    else if (this.photoBoxes.Count > 0)
    {
      this.OverrideDefault((Selectable) this.photoBoxes[0].Button);
      this.ActivateNavigation();
    }
    this.noPhotos.gameObject.SetActive(this.photoBoxes.Count == 0);
    this._CardcontentContainer.gameObject.SetActive(this.photoBoxes.Count > 0);
    this.UpdatePhotoText();
    this.DeletePrompt.gameObject.SetActive(this.photoBoxes.Count > 0);
    this.AcceptPrompt.gameObject.SetActive(this.photoBoxes.Count > 0);
    if (this.photoBoxes.Count == 0)
      DataManager.Instance.Alerts.GalleryAlerts.ClearAll();
    this.InputTimeOut = Time.unscaledTime;
  }

  [CompilerGenerated]
  public void \u003COnPhotoSelected\u003Eb__28_0()
  {
    this.InputTimeOut = Time.unscaledTime + this.InputTimeOutTime;
  }

  [CompilerGenerated]
  public void \u003COnPhotoSelected\u003Eb__28_1()
  {
    PhotoModeManager.CurrentPhotoState = PhotoModeManager.PhotoState.Gallery;
    this.GalleryNotInFocus = false;
  }

  [CompilerGenerated]
  public void \u003COnPhotoSelected\u003Eb__28_2()
  {
    this.photoSelectedPerformingAction = false;
    this.GalleryNotInFocus = false;
  }

  [CompilerGenerated]
  public void \u003COnPhotoSelected\u003Eb__28_3(PhotoModeManager.PhotoData data)
  {
    this._texturesToDestroy.Add(data.PhotoTexture);
    PhotoInformationBox photoInformationBox = this.MakePhoto(data, false);
    photoInformationBox.Button.SetInteractionState(false);
    photoInformationBox.transform.SetAsFirstSibling();
    this.photoBoxes.Insert(0, photoInformationBox);
    this.noPhotos.gameObject.SetActive(this.photoBoxes.Count == 0);
    this._CardcontentContainer.gameObject.SetActive(this.photoBoxes.Count > 0);
    this.UpdatePhotoText();
  }
}
