// Decompiled with JetBrains decompiler
// Type: UINormalComicMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using MMTools;
using Rewired;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UINormalComicMenuController : UIMenuBase
{
  [SerializeField]
  public Book book;
  [SerializeField]
  public AutoFlip autoFlip;
  [SerializeField]
  public UIComicMenuController comicMenuController;
  [SerializeField]
  public CanvasGroup controlPromptsGroup;
  [SerializeField]
  public MMControlPrompt nextPrompt;
  [SerializeField]
  public MMControlPrompt previousPrompt;
  [SerializeField]
  public MMControlPrompt quitPrompt;
  [SerializeField]
  public TMP_Text pageNumber;
  [SerializeField]
  public Image sideFeedbackRight;
  [SerializeField]
  public Image sideFeedbackLeft;
  public bool bookOpening;
  [CompilerGenerated]
  public bool \u003CIsShowingBonus\u003Ek__BackingField;
  public Vector2Int pageIndex = Vector2Int.zero;
  [CompilerGenerated]
  public Camera \u003CCamera\u003Ek__BackingField;
  public static bool ComicNormalActive;

  public Book Book => this.book;

  public CanvasGroup ControlPromptsGroup => this.controlPromptsGroup;

  public bool IsShowingBonus
  {
    get => this.\u003CIsShowingBonus\u003Ek__BackingField;
    set => this.\u003CIsShowingBonus\u003Ek__BackingField = value;
  }

  public Camera Camera
  {
    get => this.\u003CCamera\u003Ek__BackingField;
    set => this.\u003CCamera\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    this.Camera = Camera.main;
    this.GetComponent<Canvas>().worldCamera = this.Camera;
    this.controlPromptsGroup.alpha = 0.0f;
    UINormalComicMenuController.ComicNormalActive = true;
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
    this.UpdateInputPrompts();
    this.autoFlip.OnPageFlipRight += new System.Action(this.OnPageFlipRight);
    this.autoFlip.OnPageFlipLeft += new System.Action(this.OnPageFlipLeft);
    this.book.interactable = false;
  }

  public void OnEnable() => this.UpdateInputPrompts();

  public void OnPageFlipRight()
  {
    if (this.IsShowingBonus && SettingsManager.Settings.Game.Language != "English")
      this.comicMenuController.RightPage();
    else
      this.comicMenuController.IncreasePageIndex();
    this.UpdateInputPrompts();
  }

  public void OnPageFlipLeft()
  {
    this.comicMenuController.DecreasePageIndex();
    this.UpdateInputPrompts();
  }

  public override void OnShowStarted()
  {
    this.book.gameObject.SetActive(true);
    this.book.OnFlip.AddListener(new UnityAction(this.OnFrontCoverOpened));
    if (InputManager.General.InputIsController())
    {
      this.UpdateInputPrompts();
      this.book.IsMouse = false;
    }
    base.OnShowStarted();
    this.CanvasGroup.alpha = 1f;
    this.controlPromptsGroup.alpha = 0.0f;
    this.controlPromptsGroup.DOFade(1f, 1.5f);
  }

  public override void OnShowCompleted() => base.OnShowCompleted();

  public void OnActiveControllerChanged(Controller controller) => this.UpdateInputPrompts();

  public void UpdateInputPrompts()
  {
    if (InputManager.General.InputIsController())
    {
      this.nextPrompt.Category = 1;
      this.nextPrompt.Action = 44;
      this.previousPrompt.Category = 1;
      this.previousPrompt.Action = 43;
    }
    else
    {
      this.nextPrompt.Category = 0;
      this.nextPrompt.Action = 2;
      this.previousPrompt.Category = 0;
      this.previousPrompt.Action = 68;
    }
    this.quitPrompt.Category = 1;
    this.quitPrompt.Action = 39;
    this.nextPrompt.ForceUpdate();
    this.previousPrompt.ForceUpdate();
    this.quitPrompt.ForceUpdate();
    this.nextPrompt.gameObject.SetActive(this.IsShowingBonus && this.comicMenuController.PageIndex.x < 15 || !this.IsShowingBonus && this.comicMenuController.PageIndex.x < 34);
    this.previousPrompt.gameObject.SetActive(this.comicMenuController.PageIndex.x > 0);
    if (this.IsShowingBonus)
      this.pageNumber.text = $"{this.comicMenuController.PageIndex.x}/{15}";
    else
      this.pageNumber.text = $"{this.comicMenuController.PageIndex.x}/{34}";
  }

  public void OnFrontCoverOpened()
  {
  }

  public void Update()
  {
  }

  public IEnumerator BookOpenedWithControllerIE()
  {
    this.bookOpening = true;
    float time = 1f;
    while ((double) (time -= Time.deltaTime) >= 0.0)
    {
      this.book.DragRightPageToPoint(Vector3.Lerp(new Vector3(-1085.1f, -428.26f), this.book.EndBottomRight, time));
      this.book.pageDragging = false;
      yield return (object) null;
    }
    this.book.pageDragging = true;
    this.book.OnMouseRelease();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    UINormalComicMenuController.ComicNormalActive = false;
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
    this.autoFlip.OnPageFlipRight -= new System.Action(this.OnPageFlipRight);
    this.autoFlip.OnPageFlipLeft -= new System.Action(this.OnPageFlipLeft);
  }
}
