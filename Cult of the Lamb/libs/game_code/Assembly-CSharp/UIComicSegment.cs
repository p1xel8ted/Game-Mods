// Decompiled with JetBrains decompiler
// Type: UIComicSegment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using src.Managers;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UIComicSegment : MonoBehaviour
{
  [SerializeField]
  public Image image;
  [SerializeField]
  public UIComicSegment.SegmentTransition transition;
  [SerializeField]
  public bool _canvasGroupFadeIn;
  public CanvasGroup _canvasGroup;
  [HideInInspector]
  public float Delay;
  [HideInInspector]
  public string SFX;
  [HideInInspector]
  public string SFXParamater;
  [HideInInspector]
  public int SFXParamaterValue;
  [HideInInspector]
  public Vector2 Size;
  [HideInInspector]
  public float Scale = 1f;
  [HideInInspector]
  public float ZoomScale = 1f;
  [HideInInspector]
  public bool IsChoice;
  [HideInInspector]
  public bool OnlyEnglish;
  [HideInInspector]
  public UnityEvent OnSegmentShown;
  public List<UIComicSegment.ComicBubble> Bubbles = new List<UIComicSegment.ComicBubble>();
  [Space]
  public int ChoiceVariant;
  public int PageOverride = -1;
  public UnityEvent OnBeginHighlight;
  public UnityEvent OnEndHighlight;
  public MMControlPrompt controlPrompt;
  public Image dissolve;
  public DOTweenAnimation animation;
  public float zoomValue;
  public UIComicPage page;
  public UIComicPanel panel;
  public UIComicMenuController comicMenu;
  public MMScrollRect scrollRect;
  public MMButton button;
  public Vector3 startingPosition;
  public Vector3 startingScale;
  public bool texturesCombined;
  public bool tweening;
  public bool initialised;
  public bool isLeftChoice;
  public bool isSelected;

  public Image Image
  {
    get => this.image;
    set => this.image = value;
  }

  public UIComicSegment.SegmentTransition Transition
  {
    get => this.transition;
    set => this.transition = value;
  }

  public Image Dissolve => this.dissolve;

  public MMButton Button => this.button;

  public void Awake()
  {
    this.page = this.GetComponentInParent<UIComicPage>();
    this.panel = this.GetComponentInParent<UIComicPanel>();
    this.comicMenu = this.GetComponentInParent<UIComicMenuController>();
    this.scrollRect = this.GetComponentInParent<MMScrollRect>();
    this.animation = this.GetComponent<DOTweenAnimation>();
    if ((UnityEngine.Object) this.Image.material != (UnityEngine.Object) null)
      this.image.material = new Material(this.image.material);
    this.startingPosition = this.transform.localPosition;
    this.startingScale = this.transform.localScale;
    if (this.IsChoice && (UnityEngine.Object) this.controlPrompt != (UnityEngine.Object) null)
    {
      this.button = this.gameObject.AddComponent<MMButton>();
      this.button.OnSelected += new System.Action(this.OnSelected);
      this.button.OnDeselected += new System.Action(this.OnDeselected);
      this.button.onClick.AddListener(new UnityAction(this.OnClick));
      this.isLeftChoice = this.controlPrompt.Action == 43;
    }
    for (int index = this.Bubbles.Count - 1; index >= 0; --index)
    {
      if (this.Bubbles[index] == null || (UnityEngine.Object) this.Bubbles[index].Bubble == (UnityEngine.Object) null)
        this.Bubbles.RemoveAt(index);
    }
    if ((UnityEngine.Object) this.controlPrompt != (UnityEngine.Object) null && InputManager.General.InputIsController())
      this.controlPrompt.transform.parent.gameObject.SetActive(false);
    this.initialised = true;
  }

  public void OnEnable() => this.animation.RecreateTweenAndPlay();

  public void ResetValues()
  {
    if (!this.initialised)
      return;
    this.animation.DOKill();
    this.transform.localPosition = this.startingPosition;
    this.transform.localScale = this.startingScale;
    this.animation.RecreateTweenAndPlay();
    if (this._canvasGroupFadeIn)
    {
      if ((UnityEngine.Object) this._canvasGroup == (UnityEngine.Object) null)
        this._canvasGroup = this.GetComponent<CanvasGroup>();
      if ((UnityEngine.Object) this._canvasGroup != (UnityEngine.Object) null)
      {
        this._canvasGroup.DOKill();
        this._canvasGroup.alpha = 0.0f;
        this._canvasGroup.DOFade(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
      }
    }
    if (!this.OnlyEnglish || UIComicMenuController.LanguageCodes.IndexOf(LocalizationManager.CurrentLanguageCode) == 0)
      return;
    this.gameObject.SetActive(false);
  }

  public void OnDisable() => this.animation.DOKill();

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this.button != (UnityEngine.Object) null)
    {
      this.button.OnSelected -= new System.Action(this.OnSelected);
      this.button.OnDeselected -= new System.Action(this.OnDeselected);
      this.button.onClick.RemoveListener(new UnityAction(this.OnClick));
    }
    this.texturesCombined = false;
    if (!((UnityEngine.Object) this.Image != (UnityEngine.Object) null) || !((UnityEngine.Object) this.Image.sprite != (UnityEngine.Object) null))
      return;
    if (this.comicMenu.IsShowingBonus)
    {
      if ((UnityEngine.Object) this.Image.sprite.texture != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.Image.sprite.texture);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.Image.sprite);
    }
    else
    {
      if ((UnityEngine.Object) this.Image.sprite.texture != (UnityEngine.Object) null && this.Image.sprite.texture.name.Contains("dynamically"))
        UnityEngine.Object.Destroy((UnityEngine.Object) this.Image.sprite.texture);
      if (!this.Image.sprite.name.Contains("dynamically"))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.Image.sprite);
    }
  }

  public void Update()
  {
    if (!((UnityEngine.Object) this.comicMenu != (UnityEngine.Object) null) || !((UnityEngine.Object) this.comicMenu.CurrentPage != (UnityEngine.Object) null) || !this.IsChoice || !((UnityEngine.Object) this.controlPrompt != (UnityEngine.Object) null) || !((UnityEngine.Object) this.comicMenu.CurrentPage != (UnityEngine.Object) null) || !((UnityEngine.Object) this.comicMenu.CurrentPage.CurrentPanel != (UnityEngine.Object) null) || !((UnityEngine.Object) this.comicMenu.CurrentPage.CurrentPanel == (UnityEngine.Object) this.panel))
      return;
    if (InputManager.General.InputIsController())
    {
      this.controlPrompt.Action = 38;
      this.controlPrompt.transform.parent.gameObject.SetActive(this.isSelected);
    }
    else
    {
      this.controlPrompt.transform.parent.gameObject.SetActive(true);
      this.controlPrompt.Action = this.isLeftChoice ? 43 : 44;
    }
    if (this.comicMenu.CurrentPage.Animating)
      return;
    if (InputManager.General.InputIsController() && InputManager.UI.GetAcceptButtonDown() && this.isSelected)
    {
      this.OnClick();
    }
    else
    {
      if ((InputManager.General.InputIsController() || this.controlPrompt.Action != 43 || !InputManager.UI.GetPageNavigateLeftDown()) && (this.controlPrompt.Action != 44 || !InputManager.UI.GetPageNavigateRightDown()))
        return;
      this.OnClick();
    }
  }

  public void CombineTextures(bool animate)
  {
    if ((UnityEngine.Object) this.comicMenu == (UnityEngine.Object) null)
      this.comicMenu = this.GetComponentInParent<UIComicMenuController>();
    if (SettingsManager.Settings.Game.Language != "English")
    {
      foreach (UIComicBubble componentsInChild in this.GetComponentsInChildren<UIComicBubble>(true))
      {
        componentsInChild.ForceUpdateBubble();
        componentsInChild.SetText(componentsInChild.Term, componentsInChild.FontSize, componentsInChild.Rotation, componentsInChild.Width, componentsInChild.Height);
        componentsInChild.gameObject.SetActive(true);
      }
      if (this.Bubbles.Count > 0 && this.panel.CombineTextures && !this.comicMenu.IsShowingBonus)
      {
        if (this.texturesCombined)
        {
          foreach (Component componentsInChild in this.GetComponentsInChildren<UIComicBubble>(true))
            componentsInChild.gameObject.SetActive(false);
        }
        else
          this.comicMenu.StartCoroutine((IEnumerator) this.CombineTexturesIE(animate));
      }
      else
      {
        if (this.transition != UIComicSegment.SegmentTransition.Fade || !((UnityEngine.Object) this.image.sprite != (UnityEngine.Object) null))
          return;
        this.ConfigureDissolve(this.image.sprite.texture, animate);
      }
    }
    else
    {
      foreach (Component componentsInChild in this.GetComponentsInChildren<UIComicBubble>(true))
        componentsInChild.gameObject.SetActive(false);
      if (this.comicMenu.IsShowingBonus || this.transition != UIComicSegment.SegmentTransition.Fade || !((UnityEngine.Object) this.image.sprite != (UnityEngine.Object) null))
        return;
      this.ConfigureDissolve(this.image.sprite.texture, animate);
    }
  }

  public IEnumerator CombineTexturesIE(bool animate)
  {
    UIComicSegment uiComicSegment = this;
    UIComicPanel componentInParent1 = uiComicSegment.GetComponentInParent<UIComicPanel>();
    uiComicSegment.texturesCombined = true;
    RectTransform transform = (RectTransform) uiComicSegment.transform;
    Vector3 anchoredPosition = (Vector3) transform.anchoredPosition;
    Transform parent = uiComicSegment.transform.parent;
    uiComicSegment.transform.parent = componentInParent1.transform;
    transform.anchoredPosition = (Vector2) Vector3.zero;
    uiComicSegment.gameObject.SetActive(true);
    uiComicSegment.Image.material = uiComicSegment.comicMenu.ComicMaterial;
    if (!uiComicSegment.comicMenu.IsShowingBonus)
    {
      foreach (UIComicPanel.ComicSegment segment in componentInParent1.Segments)
      {
        if ((UnityEngine.Object) segment.Segment != (UnityEngine.Object) uiComicSegment)
          segment.Segment.gameObject.SetActive(false);
      }
    }
    int x1 = (int) transform.sizeDelta.x;
    int y1 = (int) transform.sizeDelta.y;
    bool flag = x1 > 1920 || y1 > 1080;
    float x2 = (float) ((double) transform.anchoredPosition.x - (double) (x1 / 2) + 960.0);
    float y2 = (float) ((double) transform.anchoredPosition.y - (double) (y1 / 2) + 540.0);
    RenderTexture temp = !flag ? RenderTexture.GetTemporary(1920, 1080) : RenderTexture.GetTemporary(x1, y1);
    uiComicSegment.comicMenu.Camera.targetTexture = temp;
    RenderTexture.active = temp;
    uiComicSegment.comicMenu.Camera.Render();
    Texture2D result = (Texture2D) null;
    if (flag)
    {
      result = new Texture2D(x1, y1, TextureFormat.RGB24, false);
      result.ReadPixels(new Rect(0.0f, 0.0f, (float) x1, (float) y1), 0, 0);
    }
    else
    {
      result = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
      result.ReadPixels(new Rect(0.0f, 0.0f, 1920f, 1080f), 0, 0);
    }
    result.Apply();
    UIComicPage componentInParent2 = uiComicSegment.Image.GetComponentInParent<UIComicPage>();
    string str = "";
    if ((UnityEngine.Object) componentInParent2 != (UnityEngine.Object) null)
      str = componentInParent2.gameObject.name;
    result.name = str + "(dynamically created)";
    Sprite sprite = !flag ? Sprite.Create(result, new Rect(x2, y2, (float) x1, (float) y1), Vector2.one * 0.5f) : Sprite.Create(result, new Rect(0.0f, 0.0f, (float) x1, (float) y1), Vector2.one * 0.5f);
    sprite.name = str + "(dynamically created)";
    uiComicSegment.Image.sprite = sprite;
    RenderTexture.active = (RenderTexture) null;
    uiComicSegment.comicMenu.Camera.targetTexture = (RenderTexture) null;
    RenderTexture.ReleaseTemporary(temp);
    foreach (Component componentsInChild in uiComicSegment.GetComponentsInChildren<UIComicBubble>(true))
      UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild.gameObject);
    uiComicSegment.Bubbles.Clear();
    if (!uiComicSegment.comicMenu.IsShowingBonus)
    {
      foreach (UIComicPanel.ComicSegment segment in componentInParent1.Segments)
      {
        if ((UnityEngine.Object) segment.Segment != (UnityEngine.Object) uiComicSegment)
          segment.Segment.gameObject.SetActive(true);
      }
    }
    transform.parent = parent;
    transform.anchoredPosition = (Vector2) anchoredPosition;
    uiComicSegment.Image.color = Color.white;
    yield return (object) new WaitForEndOfFrame();
    if (uiComicSegment.transition == UIComicSegment.SegmentTransition.Fade)
      uiComicSegment.ConfigureDissolve(result, animate);
  }

  public void ConfigureDissolve(Texture2D texture, bool animate)
  {
    UIManager.PlayAudio("event:/comic sfx/transition_dissolve");
    if (this.comicMenu.CurrentPage.BackgroundIsLight)
    {
      this.UpdateMaterial(animate);
      this.Image.material.SetTexture("_TextureSample0", (Texture) texture);
      this.Image.material.SetTexture("_TextureSample2", (Texture) texture);
    }
    else if ((UnityEngine.Object) this.Dissolve == (UnityEngine.Object) null)
    {
      GameObject gameObject = new GameObject();
      gameObject.transform.parent = this.transform;
      gameObject.transform.SetAsLastSibling();
      this.dissolve = gameObject.AddComponent<Image>();
      ((RectTransform) gameObject.transform).pivot = ((RectTransform) this.Image.transform).pivot;
      ((RectTransform) gameObject.transform).anchoredPosition = Vector2.zero;
      ((RectTransform) gameObject.transform).sizeDelta = ((RectTransform) this.Image.transform).sizeDelta;
      gameObject.transform.localPosition = (Vector3) Vector2.zero;
      gameObject.transform.localScale = (Vector3) Vector2.one;
      gameObject.transform.localRotation = Quaternion.identity;
      this.UpdateMaterial(animate);
      this.dissolve.gameObject.SetActive(false);
      this.Image.color = new Color(this.Image.color.r, this.Image.color.g, this.Image.color.b, animate ? 0.0f : 1f);
    }
    else
      this.UpdateMaterial(animate);
  }

  public void UpdateMaterial(bool animate = true)
  {
    if ((UnityEngine.Object) this.page == (UnityEngine.Object) null)
      this.page = this.GetComponentInParent<UIComicPage>();
    if ((UnityEngine.Object) this.comicMenu == (UnityEngine.Object) null)
      this.comicMenu = this.GetComponentInParent<UIComicMenuController>();
    if (this.transition != UIComicSegment.SegmentTransition.Fade || !((UnityEngine.Object) this.image.sprite != (UnityEngine.Object) null))
      return;
    if (this.page.BackgroundIsLight)
    {
      this.Image.material = new Material(this.comicMenu.WhiteDissolveMaterial);
      this.Image.material.SetFloat("_Dissolve", animate ? 0.0f : 1f);
      this.Image.material.SetTexture("_TextureSample0", (Texture) this.Image.sprite.texture);
      this.Image.material.SetTexture("_TextureSample2", (Texture) this.Image.sprite.texture);
    }
    else
    {
      if (!((UnityEngine.Object) this.dissolve != (UnityEngine.Object) null))
        return;
      this.dissolve.material = new Material(this.comicMenu.BlackDissolveMaterial);
      this.dissolve.material.SetFloat("_Dissolve", animate ? 1f : 0.0f);
    }
  }

  public void OnClick()
  {
    if (this.tweening || this.page.Animating || (UnityEngine.Object) this.comicMenu.CurrentPage == (UnityEngine.Object) null || (UnityEngine.Object) this.comicMenu.CurrentPage.CurrentPanel == (UnityEngine.Object) null || (UnityEngine.Object) this.comicMenu.CurrentPage.CurrentPanel != (UnityEngine.Object) this.panel)
      return;
    this.tweening = true;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.button.OnSelected -= new System.Action(this.OnSelected);
    this.button.OnDeselected -= new System.Action(this.OnDeselected);
    this.button.onClick.RemoveListener(new UnityAction(this.OnClick));
    PersistenceManager.PersistentData.ComicPageIndex.y = this.ChoiceVariant;
    PersistenceManager.PersistentData.ComicPageIndex.x = this.PageOverride != -1 ? this.PageOverride - 2 : PersistenceManager.PersistentData.ComicPageIndex.x;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.button);
    this.OnSelected();
    this.transform.DOPunchScale(this.transform.localScale * 0.05f, 0.5f).OnComplete<Tweener>((TweenCallback) (() => this.comicMenu.RightPage()));
  }

  public void OnSelected()
  {
    this.transform.localScale = Vector3.one;
    this.transform.DOKill();
    this.transform.DOScale(Vector3.one * 1.15f, 0.15f);
    this.OnBeginHighlight?.Invoke();
    this.isSelected = true;
  }

  public void OnDeselected()
  {
    this.transform.localScale = Vector3.one * 1.15f;
    this.transform.DOKill();
    this.transform.DOScale(Vector3.one, 0.15f);
    this.OnEndHighlight?.Invoke();
    this.isSelected = false;
  }

  [CompilerGenerated]
  public void \u003COnClick\u003Eb__57_0() => this.comicMenu.RightPage();

  [Serializable]
  public enum SegmentTransition
  {
    None,
    Fade,
    Punch,
    Scale,
    Pan,
    Zoom,
    PunchPositionX,
    PunchPositionY,
    FadeOut,
    Colour,
  }

  [Serializable]
  public class ComicBubble
  {
    public UIComicBubble Bubble;
  }
}
