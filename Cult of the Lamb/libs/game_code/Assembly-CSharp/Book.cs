// Decompiled with JetBrains decompiler
// Type: Book
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

#nullable disable
public class Book : BaseMonoBehaviour
{
  public Canvas canvas;
  [SerializeField]
  public RectTransform BookPanel;
  public Sprite background;
  public string[] spriteAddresses;
  public bool LoadingComic;
  public Sprite[] bookPages;
  public List<AsyncOperationHandle<IList<Sprite>>> loadedSprites = new List<AsyncOperationHandle<IList<Sprite>>>();
  public AsyncOperationHandle<IList<Sprite>>[] spriteReferences;
  public Material[] bookPagesMaterials;
  public UnityEvent OnIntroStart;
  public UnityEvent OnIntroComplete;
  public bool enableShadowEffect = true;
  public bool DoIntro = true;
  public bool Animate = true;
  public float delay = 1f;
  public int currentPage;
  public int previousPage = -1;
  public Image ClippingPlane;
  public Image NextPageClip;
  public Image Shadow;
  public Image ShadowLTR;
  public Image Left;
  public Image LeftNext;
  public Image Right;
  public Image RightNext;
  public UnityEvent OnFlip;
  public float radius1;
  public float radius2;
  public Vector3 sb;
  public Vector3 st;
  public Vector3 c;
  public Vector3 ebr;
  public Vector3 ebl;
  public Vector3 f;
  public FlipMode mode;
  public RectTransform shadowImage;
  public Vector3 shadowImageStartingPos;
  public RectTransform bookParentPanel;
  public Transform backgroundParent;
  public float rotationSpeed = 5f;
  public float depthEffect = 0.05f;
  public float scaleEffect = 0.05f;
  public float shadowOffset = 5f;
  public Player player;
  public Vector3 startingPosBackgroundParent;
  public Vector3 lastInput;
  public Quaternion bookParentPanelStartRotation;
  public Vector3 bookParentPanelStartPosition;
  public Vector3 bookParentPanelStartScale;
  public bool pageDragging;
  public bool interactable = true;
  public float horizontalInput;
  public float verticalInput;
  [CompilerGenerated]
  public bool \u003CIsMouse\u003Ek__BackingField = true;
  public UIComicMenuController comicMenu;
  public bool tweening;
  public Tween tween;
  public bool doneIntro;
  public bool flippingPage;
  public bool resetting;
  public Vector3 inputVector;
  public bool playedSfx;
  public Coroutine currentCoroutine;

  public int TotalPageCount => this.spriteAddresses.Length;

  public Vector3 EndBottomLeft => this.ebl;

  public Vector3 EndBottomRight => this.ebr;

  public float Height => this.BookPanel.rect.height;

  public bool IsMouse
  {
    get => this.\u003CIsMouse\u003Ek__BackingField;
    set => this.\u003CIsMouse\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.comicMenu = this.GetComponentInParent<UIComicMenuController>();
    this.bookParentPanelStartRotation = this.bookParentPanel.rotation;
    this.bookParentPanelStartPosition = this.bookParentPanel.position;
    this.bookParentPanelStartScale = this.bookParentPanel.localScale;
    if ((bool) (UnityEngine.Object) this.backgroundParent)
      this.startingPosBackgroundParent = (Vector3) ((RectTransform) this.backgroundParent).anchoredPosition;
    this.shadowImageStartingPos = this.shadowImage.localPosition;
    this.lastInput = Input.mousePosition;
    if (!(bool) (UnityEngine.Object) this.canvas)
      this.canvas = this.GetComponentInParent<Canvas>();
    if (!(bool) (UnityEngine.Object) this.canvas)
      Debug.LogError((object) "Book should be a child to canvas");
    this.Left.gameObject.SetActive(false);
    this.Right.gameObject.SetActive(false);
    if (this.DoIntro)
      return;
    this.IntroDone();
  }

  public void UnloadBookPages()
  {
    foreach (AsyncOperationHandle<IList<Sprite>> loadedSprite in this.loadedSprites)
    {
      if (loadedSprite.IsValid())
        Addressables.Release<IList<Sprite>>(loadedSprite);
    }
    this.loadedSprites.Clear();
    this.bookPages = new Sprite[0];
    this.spriteReferences = new AsyncOperationHandle<IList<Sprite>>[0];
  }

  public Sprite GetBookPage(int index)
  {
    if (!((UnityEngine.Object) this.bookPages[index] == (UnityEngine.Object) null))
      return this.bookPages[index];
    this.LoadSprite(index);
    return this.bookPages[index];
  }

  public void LoadBookPages()
  {
    this.LoadingComic = true;
    this.bookPages = new Sprite[this.TotalPageCount];
    this.spriteReferences = new AsyncOperationHandle<IList<Sprite>>[this.TotalPageCount];
    for (int i = 0; i < 3; ++i)
      this.LoadSprite(i);
  }

  public void LoadSprite(int i)
  {
    if ((UnityEngine.Object) this.comicMenu != (UnityEngine.Object) null && this.comicMenu.IsShowingBonus && SettingsManager.Settings.Game.Language != "English")
      return;
    int num1 = this.spriteAddresses[i].IndexOf('[');
    int num2 = this.spriteAddresses[i].IndexOf(']');
    string SpriteName = "";
    if (num1 != -1 && num2 != -1 && num2 > num1)
      SpriteName = this.spriteAddresses[i].Substring(num1 + 1, num2 - num1 - 1);
    string key = Regex.Replace(this.spriteAddresses[i], "\\[.*?\\]", "");
    this.spriteReferences[i] = Addressables.LoadAssetAsync<IList<Sprite>>((object) key);
    this.spriteReferences[i].Completed += (Action<AsyncOperationHandle<IList<Sprite>>>) (h => this.OnSpriteLoaded(h, SpriteName, i));
    this.spriteReferences[i].WaitForCompletion();
    this.loadedSprites.Add(this.spriteReferences[i]);
  }

  public void UnloadSprite(int i)
  {
    if (i >= this.TotalPageCount)
      return;
    this.bookPages[i] = (Sprite) null;
    if (!this.spriteReferences[i].IsValid())
      return;
    Addressables.Release<IList<Sprite>>(this.spriteReferences[i]);
  }

  public void OnSpriteLoaded(
    AsyncOperationHandle<IList<Sprite>> handle,
    string SpriteName,
    int index)
  {
    if (handle.Status == AsyncOperationStatus.Succeeded)
    {
      Sprite sprite1 = handle.Result[0];
      foreach (Sprite sprite2 in (IEnumerable<Sprite>) handle.Result)
      {
        if (sprite2.name == SpriteName)
          sprite1 = sprite2;
      }
      if ((UnityEngine.Object) sprite1 != (UnityEngine.Object) null)
        this.bookPages[index] = sprite1;
      else
        Debug.LogError((object) ("Failed to find sprite with name: " + SpriteName));
    }
    else
      Debug.LogError((object) ("Failed to load sprite: " + handle.DebugName));
  }

  public void CalcCurlCriticalPoints()
  {
    if (!this.doneIntro)
      return;
    this.sb = new Vector3(0.0f, (float) (-(double) this.BookPanel.rect.height / 2.0));
    this.ebr = new Vector3(this.BookPanel.rect.width / 2f, (float) (-(double) this.BookPanel.rect.height / 2.0));
    this.ebl = new Vector3((float) (-(double) this.BookPanel.rect.width / 2.0), (float) (-(double) this.BookPanel.rect.height / 2.0));
    this.st = new Vector3(0.0f, this.BookPanel.rect.height / 2f);
    this.radius1 = Vector2.Distance((Vector2) this.sb, (Vector2) this.ebr);
    float num = this.BookPanel.rect.width / 2f;
    float height = this.BookPanel.rect.height;
    this.radius2 = Mathf.Sqrt((float) ((double) num * (double) num + (double) height * (double) height));
  }

  public Vector3 transformPoint(Vector3 mouseScreenPos)
  {
    if (!this.doneIntro)
      return Vector3.zero;
    if (this.canvas.renderMode == RenderMode.ScreenSpaceCamera)
      return (Vector3) (Vector2) this.BookPanel.InverseTransformPoint(this.canvas.worldCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, this.canvas.planeDistance)));
    if (this.canvas.renderMode != RenderMode.WorldSpace)
      return (Vector3) (Vector2) this.BookPanel.InverseTransformPoint(mouseScreenPos);
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    float enter;
    new Plane(this.transform.TransformPoint(this.ebr), this.transform.TransformPoint(this.ebl), this.transform.TransformPoint(this.st)).Raycast(ray, out enter);
    return (Vector3) (Vector2) this.BookPanel.InverseTransformPoint(ray.GetPoint(enter));
  }

  public void Update()
  {
    if (!this.doneIntro)
      return;
    if (!this.resetting && this.Animate)
      this.RotateBook();
    if (this.pageDragging && this.interactable)
    {
      this.UpdateBook();
    }
    else
    {
      if (this.currentCoroutine != null || this.tweening || (double) (this.delay -= Time.unscaledDeltaTime) > 0.0 || !this.Animate)
        return;
      this.tweening = true;
      float time = 0.0f;
      this.tween = (Tween) DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), 1f, 1f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        this.DragRightPageToPoint(Vector3.Lerp(this.ebr, new Vector3(422f, -300f), time));
        this.pageDragging = false;
      })).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        this.tweening = false;
        if (!this.gameObject.activeSelf)
          return;
        this.delay = 0.75f;
        this.currentCoroutine = this.StartCoroutine((IEnumerator) this.TweenTo(this.ebr, 0.15f, (System.Action) (() =>
        {
          this.UpdateSprites();
          this.RightNext.transform.SetParent(this.BookPanel.transform);
          this.Right.transform.SetParent(this.BookPanel.transform);
          this.Left.gameObject.SetActive(false);
          this.Right.gameObject.SetActive(false);
          this.pageDragging = false;
        })));
      }));
    }
  }

  public void ResetPosition(System.Action callback)
  {
    this.PerformTransformations(Vector3.zero, (System.Action) (() =>
    {
      System.Action action = callback;
      if (action != null)
        action();
      this.resetting = false;
    }));
    this.resetting = true;
  }

  public void Awake() => this.LoadBookPages();

  public void OnEnable()
  {
    if (!this.DoIntro)
      return;
    this.backgroundParent.GetComponent<CanvasGroup>().alpha = 0.0f;
    this.StartCoroutine((IEnumerator) this.Intro());
  }

  public IEnumerator Intro()
  {
    yield return (object) new WaitForSecondsRealtime(0.1f);
    this.OnIntroStart?.Invoke();
    yield return (object) new WaitForSeconds(0.5f);
    this.ThrowBookOntoTable();
  }

  public void OnDisable() => this.currentCoroutine = (Coroutine) null;

  public void OnDestroy()
  {
    this.UnloadBookPages();
    if (this.tween != null)
      this.tween.Kill();
    this.comicMenu = (UIComicMenuController) null;
  }

  public void ThrowBookOntoTable()
  {
    UIManager.PlayAudio("event:/ui/map_location_pan");
    UIManager.PlayAudio("event:/comic sfx/page_rip_impact");
    this.backgroundParent.GetComponent<CanvasGroup>().alpha = 1f;
    this.backgroundParent.DOLocalRotate(new Vector3(0.0f, 0.0f, -100f), 0.01f).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true);
    this.backgroundParent.DOKill();
    this.backgroundParent.localPosition = new Vector3((float) UnityEngine.Random.Range(-250, 250), -2000f, 0.0f);
    ((RectTransform) this.backgroundParent).DOAnchorPos((Vector2) this.startingPosBackgroundParent, 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuad).OnComplete<TweenerCore<Vector2, Vector2, VectorOptions>>(new TweenCallback(this.IntroDone));
    this.backgroundParent.DOLocalRotate(new Vector3(0.0f, 0.0f, 0.0f), 1f).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InCirc);
  }

  public void IntroDone()
  {
    this.DoIntro = false;
    this.doneIntro = true;
    this.OnIntroComplete?.Invoke();
    if ((bool) (UnityEngine.Object) this.backgroundParent)
      ((RectTransform) this.backgroundParent).anchoredPosition = (Vector2) this.startingPosBackgroundParent;
    this.UpdateSprites();
    this.CalcCurlCriticalPoints();
    float x = this.BookPanel.rect.width / 2f;
    float height = this.BookPanel.rect.height;
    this.NextPageClip.rectTransform.sizeDelta = new Vector2(x, height + height * 2f);
    this.ClippingPlane.rectTransform.sizeDelta = new Vector2(x * 2f + height, height + height * 2f);
    float num = Mathf.Sqrt((float) ((double) x * (double) x + (double) height * (double) height));
    float y = x / 2f + num;
    this.Shadow.rectTransform.sizeDelta = new Vector2(x, y);
    this.Shadow.rectTransform.pivot = new Vector2(1f, x / 2f / y);
    this.ShadowLTR.rectTransform.sizeDelta = new Vector2(x, y);
    this.ShadowLTR.rectTransform.pivot = new Vector2(0.0f, x / 2f / y);
  }

  public void RotateBook()
  {
    if (this.flippingPage)
      return;
    if (InputManager.General.InputIsController())
    {
      float verticalSecondaryAxis = InputManager.Gameplay.GetVerticalSecondaryAxis();
      float horizontalSecondaryAxis = InputManager.Gameplay.GetHorizontalSecondaryAxis();
      if ((double) Mathf.Abs(verticalSecondaryAxis) > 0.10000000149011612 || (double) Mathf.Abs(horizontalSecondaryAxis) > 0.10000000149011612)
        this.inputVector = new Vector3(verticalSecondaryAxis, horizontalSecondaryAxis, 0.0f);
    }
    else if (InputManager.General.MouseInputActive)
    {
      Vector3 vector3 = Input.mousePosition - new Vector3((float) (Screen.width / 2), (float) (Screen.height / 2), 0.0f);
      vector3.x /= (float) (Screen.width / 2);
      vector3.y /= (float) (Screen.height / 2);
      this.inputVector = new Vector3(vector3.x, vector3.y, 0.0f);
    }
    if (!(this.inputVector != this.lastInput))
      return;
    this.PerformTransformations(this.inputVector);
    this.lastInput = this.inputVector;
  }

  public void PerformTransformations(Vector3 inputVector, System.Action callback = null)
  {
    float duration = 0.2f;
    this.bookParentPanel.DOKill();
    float x = inputVector.y * this.rotationSpeed;
    float y = -inputVector.x * this.rotationSpeed;
    this.bookParentPanel.DOLocalRotate(new Vector3(x, y, 0.0f), duration).OnComplete<TweenerCore<Quaternion, Vector3, QuaternionOptions>>((TweenCallback) (() =>
    {
      System.Action action = callback;
      if (action == null)
        return;
      action();
    }));
    this.bookParentPanel.DOLocalMove(new Vector3(inputVector.x * this.depthEffect, inputVector.y * this.depthEffect, 0.0f), duration);
    float num1 = (float) (1.0 + (double) inputVector.magnitude * (double) this.scaleEffect);
    this.bookParentPanel.DOScale(new Vector3(num1, num1, 1f), duration);
    this.shadowImage.DOLocalMove(new Vector3(inputVector.x * this.depthEffect + this.shadowOffset, inputVector.y * this.depthEffect - this.shadowOffset, 0.0f), duration);
    float num2 = num1 + 0.1f;
    this.shadowImage.DOScale(new Vector3(num2, num2, 1f), duration);
    this.shadowImage.DOLocalRotate(new Vector3(x * 0.8f, y * 0.8f, 0.0f), duration);
  }

  public void UpdateBook()
  {
    if (!this.doneIntro)
      return;
    Vector3 followLocation = Vector3.zero;
    bool flag = false;
    if (InputManager.General.InputIsController())
    {
      if ((double) Mathf.Abs(this.horizontalInput) > 0.10000000149011612 || (double) Mathf.Abs(this.verticalInput) > 0.10000000149011612)
      {
        Debug.Log((object) "Threshold Reached");
        followLocation = new Vector3(this.horizontalInput * (this.BookPanel.rect.width / 2f), this.verticalInput * (this.BookPanel.rect.height / 2f), 0.0f) + new Vector3(this.BookPanel.rect.width / 2f, this.BookPanel.rect.height / 2f, 0.0f);
        flag = true;
      }
    }
    else if (InputManager.General.MouseInputActive && Input.GetMouseButton(0))
    {
      if (!this.playedSfx)
      {
        UIManager.PlayAudio("event:/comic sfx/page_turn");
        this.playedSfx = true;
      }
      followLocation = this.transformPoint(Input.mousePosition);
      flag = true;
    }
    if (Input.GetMouseButtonUp(0))
      this.playedSfx = false;
    if (!flag)
      return;
    if (this.pageDragging && this.interactable)
    {
      if (this.mode == FlipMode.RightToLeft)
        this.UpdateBookRTLToPoint(followLocation);
      else
        this.UpdateBookLTRToPoint(followLocation);
    }
    else
    {
      if (this.tweening)
        return;
      double num = (double) (this.delay -= Time.unscaledDeltaTime);
    }
  }

  public void StartAutoFlip(Vector3 point)
  {
    this.tweening = true;
    float time = 0.0f;
    this.tween = (Tween) DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), 1f, 1f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.DragRightPageToPoint(Vector3.Lerp(this.ebr, point, time));
      this.pageDragging = false;
    })).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.tweening = false;
      this.delay = 0.75f;
    }));
  }

  public void UpdateBookLTRToPoint(Vector3 followLocation)
  {
    if (!this.doneIntro)
      return;
    this.mode = FlipMode.LeftToRight;
    this.f = followLocation;
    this.ShadowLTR.transform.SetParent(this.ClippingPlane.transform, true);
    this.ShadowLTR.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    this.ShadowLTR.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    this.Left.transform.SetParent(this.ClippingPlane.transform, true);
    this.Right.transform.SetParent(this.BookPanel.transform, true);
    this.Right.transform.localEulerAngles = Vector3.zero;
    this.LeftNext.transform.SetParent(this.BookPanel.transform, true);
    this.c = this.Calc_C_Position(followLocation);
    Vector3 t1;
    float num = (float) (((double) this.CalcClipAngle(this.c, this.ebl, out t1) + 180.0) % 180.0);
    this.ClippingPlane.transform.localEulerAngles = new Vector3(0.0f, 0.0f, num - 90f);
    this.ClippingPlane.transform.position = this.BookPanel.TransformPoint(t1);
    this.Left.transform.position = this.BookPanel.TransformPoint(this.c);
    this.Left.transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Atan2(t1.y - this.c.y, t1.x - this.c.x) * 57.29578f - 90f - num);
    this.NextPageClip.transform.localEulerAngles = new Vector3(0.0f, 0.0f, num - 90f);
    this.NextPageClip.transform.position = this.BookPanel.TransformPoint(t1);
    this.LeftNext.transform.SetParent(this.NextPageClip.transform, true);
    this.Right.transform.SetParent(this.ClippingPlane.transform, true);
    this.Right.transform.SetAsFirstSibling();
    this.ShadowLTR.rectTransform.SetParent((Transform) this.Left.rectTransform, true);
  }

  public void UpdateBookRTLToPoint(Vector3 followLocation)
  {
    if (!this.doneIntro)
      return;
    this.mode = FlipMode.RightToLeft;
    this.f = followLocation;
    this.Shadow.transform.SetParent(this.ClippingPlane.transform, true);
    this.Shadow.transform.localPosition = Vector3.zero;
    this.Shadow.transform.localEulerAngles = Vector3.zero;
    this.Right.transform.SetParent(this.ClippingPlane.transform, true);
    this.Left.transform.SetParent(this.BookPanel.transform, true);
    this.Left.transform.localEulerAngles = Vector3.zero;
    this.RightNext.transform.SetParent(this.BookPanel.transform, true);
    this.c = this.Calc_C_Position(followLocation);
    Vector3 t1;
    float num = this.CalcClipAngle(this.c, this.ebr, out t1);
    if ((double) num > -90.0)
      num += 180f;
    this.ClippingPlane.rectTransform.pivot = new Vector2(1f, 0.35f);
    this.ClippingPlane.transform.localEulerAngles = new Vector3(0.0f, 0.0f, num + 90f);
    this.ClippingPlane.transform.position = this.BookPanel.TransformPoint(t1);
    this.Right.transform.position = this.BookPanel.TransformPoint(this.c);
    this.Right.transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Atan2(t1.y - this.c.y, t1.x - this.c.x) * 57.29578f - (num + 90f));
    this.NextPageClip.transform.localEulerAngles = new Vector3(0.0f, 0.0f, num + 90f);
    this.NextPageClip.transform.position = this.BookPanel.TransformPoint(t1);
    this.RightNext.transform.SetParent(this.NextPageClip.transform, true);
    this.Left.transform.SetParent(this.ClippingPlane.transform, true);
    this.Left.transform.SetAsFirstSibling();
    this.Shadow.rectTransform.SetParent((Transform) this.Right.rectTransform, true);
  }

  public float CalcClipAngle(Vector3 c, Vector3 bookCorner, out Vector3 t1)
  {
    if (!this.doneIntro)
    {
      t1 = Vector3.zero;
      return 0.0f;
    }
    Vector3 vector3 = (c + bookCorner) / 2f;
    float y = bookCorner.y - vector3.y;
    float x1 = bookCorner.x - vector3.x;
    float f = Mathf.Atan2(y, x1);
    float x2 = this.normalizeT1X(vector3.x - y * Mathf.Tan(f), bookCorner, this.sb);
    t1 = new Vector3(x2, this.sb.y, 0.0f);
    return Mathf.Atan2(t1.y - vector3.y, t1.x - vector3.x) * 57.29578f;
  }

  public float normalizeT1X(float t1, Vector3 corner, Vector3 sb)
  {
    if (!this.doneIntro)
      return 0.0f;
    return (double) t1 > (double) sb.x && (double) sb.x > (double) corner.x || (double) t1 < (double) sb.x && (double) sb.x < (double) corner.x ? sb.x : t1;
  }

  public Vector3 Calc_C_Position(Vector3 followLocation)
  {
    if (!this.doneIntro)
      return Vector3.zero;
    this.f = followLocation;
    float f1 = Mathf.Atan2(this.f.y - this.sb.y, this.f.x - this.sb.x);
    Vector3 vector3_1 = new Vector3(this.radius1 * Mathf.Cos(f1), this.radius1 * Mathf.Sin(f1), 0.0f) + this.sb;
    Vector3 a = (double) Vector2.Distance((Vector2) this.f, (Vector2) this.sb) >= (double) this.radius1 ? vector3_1 : this.f;
    float f2 = Mathf.Atan2(a.y - this.st.y, a.x - this.st.x);
    Vector3 vector3_2 = new Vector3(this.radius2 * Mathf.Cos(f2), this.radius2 * Mathf.Sin(f2), 0.0f) + this.st;
    if ((double) Vector2.Distance((Vector2) a, (Vector2) this.st) > (double) this.radius2)
      a = vector3_2;
    return a;
  }

  public void DragRightPageToPoint(Vector3 point)
  {
    if (!this.doneIntro || this.currentPage >= this.TotalPageCount)
      return;
    this.pageDragging = true;
    this.mode = FlipMode.RightToLeft;
    this.f = point;
    this.NextPageClip.rectTransform.pivot = new Vector2(0.0f, 0.12f);
    this.ClippingPlane.rectTransform.pivot = new Vector2(1f, 0.35f);
    this.Left.gameObject.SetActive(true);
    this.Left.rectTransform.pivot = new Vector2(0.0f, 0.0f);
    this.Left.transform.position = this.RightNext.transform.position;
    this.Left.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    this.Left.sprite = this.currentPage < this.TotalPageCount ? this.GetBookPage(this.currentPage) : this.background;
    this.Left.transform.SetAsFirstSibling();
    this.Right.gameObject.SetActive(true);
    this.Right.transform.position = this.RightNext.transform.position;
    this.Right.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    this.Right.sprite = this.currentPage < this.TotalPageCount - 1 ? this.GetBookPage(this.currentPage + 1) : this.background;
    this.RightNext.sprite = this.currentPage < this.TotalPageCount - 2 ? this.GetBookPage(this.currentPage + 2) : this.background;
    this.UpdateMaterials();
    this.LeftNext.transform.SetAsFirstSibling();
    if (this.enableShadowEffect)
      this.Shadow.gameObject.SetActive(true);
    this.UpdateBookRTLToPoint(this.f);
  }

  public void UpdateMaterials()
  {
    if ((UnityEngine.Object) this.RightNext.sprite == (UnityEngine.Object) this.GetBookPage(0))
      this.RightNext.material = this.bookPagesMaterials[0];
    else
      this.RightNext.material = this.bookPagesMaterials[1];
    if ((UnityEngine.Object) this.Right.sprite == (UnityEngine.Object) this.GetBookPage(0))
      this.Right.material = this.bookPagesMaterials[0];
    else
      this.Right.material = this.bookPagesMaterials[1];
    if ((UnityEngine.Object) this.Left.sprite == (UnityEngine.Object) this.GetBookPage(0))
      this.Left.material = this.bookPagesMaterials[0];
    else
      this.Left.material = this.bookPagesMaterials[1];
  }

  public void DragLeftPageToPoint(Vector3 point, bool toBegining = false)
  {
    if (!this.doneIntro || this.currentPage <= 0)
      return;
    this.pageDragging = true;
    this.mode = FlipMode.LeftToRight;
    this.f = point;
    this.NextPageClip.rectTransform.pivot = new Vector2(1f, 0.12f);
    this.ClippingPlane.rectTransform.pivot = new Vector2(0.0f, 0.35f);
    this.Right.gameObject.SetActive(true);
    this.Right.transform.position = this.LeftNext.transform.position;
    this.Right.sprite = this.GetBookPage(this.currentPage - 1);
    this.Right.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    this.Right.transform.SetAsFirstSibling();
    this.Left.gameObject.SetActive(true);
    this.Left.rectTransform.pivot = new Vector2(1f, 0.0f);
    this.Left.transform.position = this.LeftNext.transform.position;
    this.Left.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    if (toBegining)
    {
      this.Left.sprite = this.GetBookPage(0);
      this.LeftNext.sprite = this.background;
      this.currentPage = 2;
    }
    else
    {
      this.Left.sprite = this.currentPage >= 2 ? this.GetBookPage(this.currentPage - 2) : this.background;
      this.LeftNext.sprite = this.currentPage >= 3 ? this.GetBookPage(this.currentPage - 3) : this.background;
      this.LeftNext.gameObject.SetActive(this.currentPage > 0);
    }
    this.UpdateMaterials();
    this.RightNext.transform.SetAsFirstSibling();
    if (this.enableShadowEffect)
      this.ShadowLTR.gameObject.SetActive(true);
    this.UpdateBookLTRToPoint(this.f);
  }

  public void OnMouseDragLeftPage()
  {
    if (!this.doneIntro || !this.interactable)
      return;
    this.DragLeftPageToPoint(this.transformPoint(Input.mousePosition));
  }

  public void OnMouseRelease()
  {
    if (!this.doneIntro || !this.interactable)
      return;
    this.ReleasePage();
  }

  public void ReleasePage(bool callOnflip = true)
  {
    if (!this.doneIntro || !this.pageDragging)
      return;
    this.pageDragging = false;
    this.TweenForward(callOnflip);
  }

  public void UpdateSprites()
  {
    this.LeftNext.sprite = this.currentPage <= 0 || this.currentPage > this.TotalPageCount ? this.background : this.GetBookPage(this.currentPage - 1);
    this.RightNext.sprite = this.currentPage < 0 || this.currentPage >= this.TotalPageCount ? this.background : this.GetBookPage(this.currentPage);
    this.LeftNext.gameObject.SetActive(this.currentPage > 0);
    this.UpdateMaterials();
  }

  public void TweenForward(bool callOnflip = true)
  {
    if (!this.doneIntro)
      return;
    this.flippingPage = true;
    if (this.mode == FlipMode.RightToLeft)
      this.currentCoroutine = this.StartCoroutine((IEnumerator) this.TweenTo(this.ebl, 0.15f, (System.Action) (() => this.Flip(callOnflip))));
    else
      this.currentCoroutine = this.StartCoroutine((IEnumerator) this.TweenTo(this.ebr, 0.15f, (System.Action) (() => this.Flip(callOnflip))));
  }

  public void Flip(bool callOnflip = true)
  {
    if (!this.doneIntro)
      return;
    this.flippingPage = false;
    this.bookParentPanel.DOKill();
    this.bookParentPanel.DORotateQuaternion(this.bookParentPanelStartRotation, 1f).SetUpdate<TweenerCore<Quaternion, Quaternion, NoOptions>>(true).SetEase<TweenerCore<Quaternion, Quaternion, NoOptions>>(Ease.OutCirc);
    this.bookParentPanel.DOScale(this.bookParentPanelStartScale, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc);
    int currentPage;
    if (this.mode == FlipMode.RightToLeft)
    {
      currentPage = this.currentPage;
      this.currentPage += 2;
    }
    else
    {
      currentPage = this.currentPage;
      this.currentPage -= 2;
    }
    this.LeftNext.transform.SetParent(this.BookPanel.transform, true);
    this.Left.transform.SetParent(this.BookPanel.transform, true);
    this.LeftNext.transform.SetParent(this.BookPanel.transform, true);
    this.Left.gameObject.SetActive(false);
    this.Right.gameObject.SetActive(false);
    this.Right.transform.SetParent(this.BookPanel.transform, true);
    this.RightNext.transform.SetParent(this.BookPanel.transform, true);
    this.UpdateSprites();
    this.Shadow.gameObject.SetActive(false);
    this.ShadowLTR.gameObject.SetActive(false);
    UIManager.PlayAudio("event:/comic sfx/page_turn");
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    if (this.OnFlip != null & callOnflip)
      this.OnFlip.Invoke();
    if (this.currentPage == 0)
      this.UnloadAllPages();
    else
      this.UnloadUnusedPages(this.mode == FlipMode.RightToLeft);
    this.previousPage = currentPage;
  }

  public void UnloadAllPages()
  {
    if (this.comicMenu.IsShowingBonus && SettingsManager.Settings.Game.Language != "English")
    {
      int num = 1;
      while (num < this.TotalPageCount)
        ++num;
    }
    else
    {
      for (int i = 3; i < this.TotalPageCount; ++i)
        this.UnloadSprite(i);
    }
    UnityEngine.Resources.UnloadUnusedAssets();
  }

  public void UnloadUnusedPages(bool right)
  {
    if (this.comicMenu.IsShowingBonus && SettingsManager.Settings.Game.Language != "English" || this.previousPage <= -1 || this.previousPage == this.currentPage || this.previousPage <= 2)
      return;
    if (this.previousPage - 1 > 2)
      this.UnloadSprite(this.previousPage - 1);
    if (this.previousPage - 2 > 2)
      this.UnloadSprite(this.previousPage - 2);
    if (this.previousPage - 3 > 2 & right)
      this.UnloadSprite(this.previousPage - 3);
    this.UnloadSprite(this.previousPage);
    this.UnloadSprite(this.previousPage + 1);
    if (right)
      return;
    this.UnloadSprite(this.previousPage + 2);
  }

  public void TweenBack()
  {
    if (!this.doneIntro)
      return;
    if (this.mode == FlipMode.RightToLeft)
      this.currentCoroutine = this.StartCoroutine((IEnumerator) this.TweenTo(this.ebr, 0.15f, (System.Action) (() =>
      {
        this.UpdateSprites();
        this.RightNext.transform.SetParent(this.BookPanel.transform);
        this.Right.transform.SetParent(this.BookPanel.transform);
        this.Left.gameObject.SetActive(false);
        this.Right.gameObject.SetActive(false);
        this.pageDragging = false;
      })));
    else
      this.currentCoroutine = this.StartCoroutine((IEnumerator) this.TweenTo(this.ebl, 0.15f, (System.Action) (() =>
      {
        this.UpdateSprites();
        this.LeftNext.transform.SetParent(this.BookPanel.transform);
        this.Left.transform.SetParent(this.BookPanel.transform);
        this.Left.gameObject.SetActive(false);
        this.Right.gameObject.SetActive(false);
        this.pageDragging = false;
      })));
  }

  public IEnumerator TweenTo(Vector3 to, float duration, System.Action onFinish, bool displace = true)
  {
    int steps = (int) ((double) duration / 0.02500000037252903);
    Vector3 displacement = (to - this.f) / (float) steps;
    for (int i = 0; i < steps - 1; ++i)
    {
      if (this.mode == FlipMode.RightToLeft)
        this.UpdateBookRTLToPoint(this.f + displacement);
      else
        this.UpdateBookLTRToPoint(this.f + displacement);
      yield return (object) new WaitForSeconds(0.025f);
    }
    if (onFinish != null)
      onFinish();
    this.currentCoroutine = (Coroutine) null;
  }

  public void FlipRightPage()
  {
    this.tween.Kill();
    if (this.currentCoroutine != null)
      this.StopCoroutine(this.currentCoroutine);
    this.currentCoroutine = (Coroutine) null;
    float frameTime = 0.0f;
    float xc = (float) (((double) this.EndBottomRight.x + (double) this.EndBottomLeft.x) / 2.0);
    float xl = (float) (((double) this.EndBottomRight.x - (double) this.EndBottomLeft.x) / 2.0 * 0.89999997615814209);
    float h = Mathf.Abs(this.EndBottomRight.y) * 0.9f;
    float dx = (float) ((double) xl * 2.0 / 40.0);
    this.StartCoroutine((IEnumerator) this.FlipRTL(xc, xl, h, frameTime, dx));
  }

  public IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
  {
    float x = xc + xl;
    float y1 = (float) (-(double) h / ((double) xl * (double) xl) * ((double) x - (double) xc) * ((double) x - (double) xc));
    this.DragRightPageToPoint(new Vector3(x, y1, 0.0f));
    for (int i = 0; i < 40; ++i)
    {
      float y2 = (float) (-(double) h / ((double) xl * (double) xl) * ((double) x - (double) xc) * ((double) x - (double) xc));
      this.UpdateBookRTLToPoint(new Vector3(x, y2, 0.0f));
      yield return (object) new WaitForSeconds(frameTime);
      x -= dx;
    }
    this.ReleasePage();
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
    this.DragLeftPageToPoint(new Vector3(x, y1, 0.0f), toBegining);
    for (int i = 0; i < 40; ++i)
    {
      float y2 = (float) (-(double) h / ((double) xl * (double) xl) * ((double) x - (double) xc) * ((double) x - (double) xc));
      this.UpdateBookLTRToPoint(new Vector3(x, y2, 0.0f));
      yield return (object) new WaitForSeconds(frameTime);
      x += dx;
    }
    this.RightNext.gameObject.SetActive(true);
    this.ReleasePage(!toBegining);
  }

  public void FlipToEnd(System.Action callback = null)
  {
    this.StartCoroutine((IEnumerator) this.FlipToEndIE(callback));
  }

  public IEnumerator FlipToEndIE(System.Action callback = null)
  {
    Book book = this;
    float frameTime = 0.0f;
    float xc = (float) (((double) book.EndBottomRight.x + (double) book.EndBottomLeft.x) / 2.0);
    float xl = (float) (((double) book.EndBottomRight.x - (double) book.EndBottomLeft.x) / 2.0 * 0.89999997615814209);
    float h = Mathf.Abs(book.EndBottomRight.y) * 0.9f;
    float dx = (float) ((double) xl * 2.0 / 40.0);
    book.StartCoroutine((IEnumerator) book.FlipLTR(xc, xl, h, frameTime, dx, true));
    ((RectTransform) book.transform).DOAnchorPos((Vector2) new Vector3(-280f, 0.0f, 0.0f), 1f);
    yield return (object) new WaitForSeconds(1f);
    System.Action action = callback;
    if (action != null)
      action();
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__80_3()
  {
    this.tweening = false;
    if (!this.gameObject.activeSelf)
      return;
    this.delay = 0.75f;
    this.currentCoroutine = this.StartCoroutine((IEnumerator) this.TweenTo(this.ebr, 0.15f, (System.Action) (() =>
    {
      this.UpdateSprites();
      this.RightNext.transform.SetParent(this.BookPanel.transform);
      this.Right.transform.SetParent(this.BookPanel.transform);
      this.Left.gameObject.SetActive(false);
      this.Right.gameObject.SetActive(false);
      this.pageDragging = false;
    })));
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__80_4()
  {
    this.UpdateSprites();
    this.RightNext.transform.SetParent(this.BookPanel.transform);
    this.Right.transform.SetParent(this.BookPanel.transform);
    this.Left.gameObject.SetActive(false);
    this.Right.gameObject.SetActive(false);
    this.pageDragging = false;
  }

  [CompilerGenerated]
  public void \u003CTweenBack\u003Eb__112_0()
  {
    this.UpdateSprites();
    this.RightNext.transform.SetParent(this.BookPanel.transform);
    this.Right.transform.SetParent(this.BookPanel.transform);
    this.Left.gameObject.SetActive(false);
    this.Right.gameObject.SetActive(false);
    this.pageDragging = false;
  }

  [CompilerGenerated]
  public void \u003CTweenBack\u003Eb__112_1()
  {
    this.UpdateSprites();
    this.LeftNext.transform.SetParent(this.BookPanel.transform);
    this.Left.transform.SetParent(this.BookPanel.transform);
    this.Left.gameObject.SetActive(false);
    this.Right.gameObject.SetActive(false);
    this.pageDragging = false;
  }
}
