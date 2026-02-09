// Decompiled with JetBrains decompiler
// Type: AutoFlip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
[RequireComponent(typeof (Book))]
public class AutoFlip : BaseMonoBehaviour
{
  public FlipMode Mode;
  public float PageFlipTime = 1f;
  public float TimeBetweenPages = 1f;
  public float DelayBeforeStarting;
  public bool AutoStartFlip = true;
  public Book ControledBook;
  public int AnimationFramesCount = 40;
  public bool isFlipping;
  public System.Action OnPageFlipRight;
  public System.Action OnPageFlipLeft;
  public UIComicMenuController comicMenu;
  public float pressDelay;
  public int cachedKeyIndex;

  public void Start()
  {
    if (!(bool) (UnityEngine.Object) this.ControledBook)
      this.ControledBook = this.GetComponent<Book>();
    if (this.AutoStartFlip)
      this.StartFlipping();
    this.ControledBook.OnFlip.AddListener(new UnityAction(this.PageFlipped));
    this.comicMenu = this.GetComponentInParent<UIComicMenuController>();
  }

  public void OnEnable() => this.ControledBook.RightNext.gameObject.SetActive(true);

  public void PageFlipped() => this.isFlipping = false;

  public void StartFlipping(System.Action callback = null)
  {
    this.StartCoroutine((IEnumerator) this.FlipToEnd(callback));
  }

  public void OnDisable() => this.isFlipping = false;

  public void Update()
  {
    if (InputManager.General.InputIsController() && InputManager.UI.GetPageNavigateRightDown() || !InputManager.General.InputIsController() && InputManager.Gameplay.GetAttackButtonDown() || this.cachedKeyIndex == 1)
    {
      this.cachedKeyIndex = (double) Time.time <= (double) this.pressDelay || this.ControledBook.currentPage >= this.ControledBook.TotalPageCount || (!this.comicMenu.IsShowingBonus || this.comicMenu.PageIndex.x >= 15) && (this.comicMenu.IsShowingBonus || this.comicMenu.PageIndex.x >= 66) ? 0 : 1;
      this.FlipRightPage();
      UIComicMenuController.ButtonPressed();
    }
    else
    {
      if ((!InputManager.General.InputIsController() || !InputManager.UI.GetPageNavigateLeftDown()) && (InputManager.General.InputIsController() || !InputManager.Gameplay.GetInteract2ButtonDown()) && this.cachedKeyIndex != -1)
        return;
      this.cachedKeyIndex = (double) Time.time <= (double) this.pressDelay || this.ControledBook.currentPage <= 0 ? 0 : -1;
      this.FlipLeftPage();
      UIComicMenuController.ButtonPressed();
    }
  }

  public void FlipRightPage()
  {
    if (this.isFlipping || this.ControledBook.currentPage >= this.ControledBook.TotalPageCount)
      return;
    Vector2Int pageIndex;
    if (this.comicMenu.IsShowingBonus)
    {
      pageIndex = this.comicMenu.PageIndex;
      if (pageIndex.x >= 15)
        return;
    }
    if (!this.comicMenu.IsShowingBonus)
    {
      pageIndex = this.comicMenu.PageIndex;
      if (pageIndex.x >= 66)
        return;
    }
    if ((UnityEngine.Object) this.comicMenu != (UnityEngine.Object) null && this.comicMenu.IsMenuOpen || !UIComicMenuController.AllowInput || this.isFlipping)
      return;
    this.pressDelay = Time.time + 0.5f;
    this.isFlipping = true;
    float frameTime = this.PageFlipTime / (float) this.AnimationFramesCount;
    float xc = (float) (((double) this.ControledBook.EndBottomRight.x + (double) this.ControledBook.EndBottomLeft.x) / 2.0);
    float xl = (float) (((double) this.ControledBook.EndBottomRight.x - (double) this.ControledBook.EndBottomLeft.x) / 2.0 * 0.89999997615814209);
    float h = Mathf.Abs(this.ControledBook.EndBottomRight.y) * 0.9f;
    float dx = xl * 2f / (float) this.AnimationFramesCount;
    this.StartCoroutine((IEnumerator) this.FlipRTL(xc, xl, h, frameTime, dx));
    System.Action onPageFlipRight = this.OnPageFlipRight;
    if (onPageFlipRight != null)
      onPageFlipRight();
    if (this.ControledBook.currentPage == 0)
      ((RectTransform) this.transform).DOAnchorPos((Vector2) Vector3.zero, 1f);
    else if (this.comicMenu.IsShowingBonus && this.ControledBook.currentPage == 28)
    {
      this.ControledBook.RightNext.gameObject.SetActive(false);
      ((RectTransform) this.transform).DOAnchorPos((Vector2) new Vector3(280f, 0.0f, 0.0f), 1f);
    }
    else
    {
      if (this.comicMenu.IsShowingBonus || this.ControledBook.currentPage != 66)
        return;
      ((RectTransform) this.transform).DOAnchorPos((Vector2) new Vector3(280f, 0.0f, 0.0f), 1f);
    }
  }

  public void FlipLeftPage()
  {
    if (this.isFlipping || this.ControledBook.currentPage <= 0 || (UnityEngine.Object) this.comicMenu != (UnityEngine.Object) null && this.comicMenu.IsMenuOpen || !UIComicMenuController.AllowInput || this.isFlipping)
      return;
    this.pressDelay = Time.time + 0.5f;
    this.isFlipping = true;
    float frameTime = this.PageFlipTime / (float) this.AnimationFramesCount;
    float xc = (float) (((double) this.ControledBook.EndBottomRight.x + (double) this.ControledBook.EndBottomLeft.x) / 2.0);
    float xl = (float) (((double) this.ControledBook.EndBottomRight.x - (double) this.ControledBook.EndBottomLeft.x) / 2.0 * 0.89999997615814209);
    float h = Mathf.Abs(this.ControledBook.EndBottomRight.y) * 0.9f;
    float dx = xl * 2f / (float) this.AnimationFramesCount;
    this.StartCoroutine((IEnumerator) this.FlipLTR(xc, xl, h, frameTime, dx));
    System.Action onPageFlipLeft = this.OnPageFlipLeft;
    if (onPageFlipLeft != null)
      onPageFlipLeft();
    if (this.ControledBook.currentPage <= 2)
      ((RectTransform) this.transform).DOAnchorPos((Vector2) new Vector3(-280f, 0.0f, 0.0f), 1f);
    else
      ((RectTransform) this.transform).DOAnchorPos((Vector2) Vector3.zero, 1f);
  }

  public IEnumerator FlipToEnd(System.Action callback = null)
  {
    AutoFlip autoFlip = this;
    yield return (object) new WaitForSeconds(autoFlip.DelayBeforeStarting);
    float frameTime = autoFlip.PageFlipTime / (float) autoFlip.AnimationFramesCount;
    float xc = (float) (((double) autoFlip.ControledBook.EndBottomRight.x + (double) autoFlip.ControledBook.EndBottomLeft.x) / 2.0);
    float xl = (float) (((double) autoFlip.ControledBook.EndBottomRight.x - (double) autoFlip.ControledBook.EndBottomLeft.x) / 2.0 * 0.89999997615814209);
    float h = Mathf.Abs(autoFlip.ControledBook.EndBottomRight.y) * 0.9f;
    float dx = xl * 2f / (float) autoFlip.AnimationFramesCount;
    switch (autoFlip.Mode)
    {
      case FlipMode.RightToLeft:
        while (autoFlip.ControledBook.currentPage < autoFlip.ControledBook.TotalPageCount)
        {
          autoFlip.StartCoroutine((IEnumerator) autoFlip.FlipRTL(xc, xl, h, frameTime, dx));
          yield return (object) new WaitForSeconds(autoFlip.TimeBetweenPages);
        }
        break;
      case FlipMode.LeftToRight:
        autoFlip.StartCoroutine((IEnumerator) autoFlip.FlipLTR(xc, xl, h, frameTime, dx, true));
        ((RectTransform) autoFlip.transform).DOAnchorPos((Vector2) new Vector3(-280f, 0.0f, 0.0f), 1f);
        yield return (object) new WaitForSeconds(autoFlip.TimeBetweenPages);
        break;
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
  {
    float x = xc + xl;
    float y1 = (float) (-(double) h / ((double) xl * (double) xl) * ((double) x - (double) xc) * ((double) x - (double) xc));
    this.ControledBook.DragRightPageToPoint(new Vector3(x, y1, 0.0f));
    for (int i = 0; i < this.AnimationFramesCount; ++i)
    {
      float y2 = (float) (-(double) h / ((double) xl * (double) xl) * ((double) x - (double) xc) * ((double) x - (double) xc));
      this.ControledBook.UpdateBookRTLToPoint(new Vector3(x, y2, 0.0f));
      yield return (object) new WaitForSeconds(frameTime);
      x -= dx;
    }
    this.ControledBook.ReleasePage();
  }

  public IEnumerator FlipLTR(
    float xc,
    float xl,
    float h,
    float frameTime,
    float dx,
    bool toBegining = false)
  {
    float x = xc - xl;
    float y1 = (float) (-(double) h / ((double) xl * (double) xl) * ((double) x - (double) xc) * ((double) x - (double) xc));
    this.ControledBook.DragLeftPageToPoint(new Vector3(x, y1, 0.0f), toBegining);
    for (int i = 0; i < this.AnimationFramesCount; ++i)
    {
      float y2 = (float) (-(double) h / ((double) xl * (double) xl) * ((double) x - (double) xc) * ((double) x - (double) xc));
      this.ControledBook.UpdateBookLTRToPoint(new Vector3(x, y2, 0.0f));
      yield return (object) new WaitForSeconds(frameTime);
      x += dx;
    }
    this.ControledBook.RightNext.gameObject.SetActive(true);
    this.ControledBook.ReleasePage();
  }
}
