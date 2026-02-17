// Decompiled with JetBrains decompiler
// Type: UIComicPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
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
public class UIComicPage : MonoBehaviour
{
  [SerializeField]
  public List<UIComicPage.ComicPanel> panels;
  public int index = -1;
  public Coroutine showingPanelRoutine;
  public UIComicMenuController comicMenu;
  public Image background;
  public Vector2 mousePosition;
  public Vector2Int forcedNextPanel;
  public bool hasChoices;
  [CompilerGenerated]
  public CanvasGroup \u003CCanvasGroup\u003Ek__BackingField;
  public UnityEvent SegmentShow;
  [CompilerGenerated]
  public UIComicPanel \u003CCurrentPanel\u003Ek__BackingField;
  public float pressDelay = 0.5f;

  public List<UIComicPage.ComicPanel> Panels => this.panels;

  public bool Animating => this.showingPanelRoutine != null;

  public Image Background => this.background;

  public bool BackgroundIsLight
  {
    get
    {
      return (double) this.Background.color.r + (double) this.Background.color.g + (double) this.Background.color.b > 1.5;
    }
  }

  public CanvasGroup CanvasGroup
  {
    get => this.\u003CCanvasGroup\u003Ek__BackingField;
    set => this.\u003CCanvasGroup\u003Ek__BackingField = value;
  }

  public UIComicPanel CurrentPanel
  {
    get => this.\u003CCurrentPanel\u003Ek__BackingField;
    set => this.\u003CCurrentPanel\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    this.mousePosition = InputManager.General.GetMousePosition();
    this.CanvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    this.background = this.transform.GetChild(0).GetComponent<Image>();
    this.comicMenu = this.GetComponentInParent<UIComicMenuController>();
    foreach (UIComicPage.ComicPanel panel in this.panels)
      panel.Panel.gameObject.SetActive(false);
    if (!this.comicMenu.IsShowingBonus && this.comicMenu.RequiresLastPanel)
    {
      PersistenceManager.PersistentData.ComicPanelIndex = this.Panels.Count - 1;
      if (!this.Panels[PersistenceManager.PersistentData.ComicPanelIndex].Panel.RequiresInputPost)
        --PersistenceManager.PersistentData.ComicPanelIndex;
      this.comicMenu.RequiresLastPanel = false;
      if (PersistenceManager.PersistentData.ComicPageIndex == new Vector2Int(30, 1) && !PersistenceManager.PersistentData.ComicExploredPages.Contains(new Vector2Int(29, 2)))
        PersistenceManager.PersistentData.ComicPanelIndex = this.panels.Count - 2;
    }
    this.ShowPage();
  }

  public void ShowPage()
  {
    this.StopAllCoroutines();
    this.CanvasGroup.alpha = 1f;
    if (PersistenceManager.PersistentData.ComicPanelIndex != 0 || this.panels.Count <= 1 || this.comicMenu.IsShowingBonus)
    {
      if (!this.comicMenu.IsShowingBonus)
      {
        this.index = PersistenceManager.PersistentData.ComicPanelIndex;
        this.index = Mathf.Clamp(this.index, 0, this.panels.Count - 1);
        foreach (UIComicPage.ComicPanel panel in this.panels)
        {
          if ((UnityEngine.Object) panel.Panel == (UnityEngine.Object) this.panels[this.index].Panel)
          {
            if (panel.Panel.RequiresFade)
              panel.Panel.CanvasGroup.DOFade(1f, 0.2f);
            else
              panel.Panel.CanvasGroup.alpha = 1f;
            foreach (UIComicPanel.ComicSegment segment in this.panels[this.index].Panel.Segments)
              segment.Segment.ResetValues();
          }
          else
          {
            panel.Panel.CanvasGroup.DOKill();
            panel.Panel.CanvasGroup.alpha = 0.0f;
          }
        }
      }
      else
      {
        this.index = 0;
        foreach (UIComicPage.ComicPanel panel in this.panels)
        {
          foreach (UIComicPanel.ComicSegment segment in panel.Panel.Segments)
            segment.Segment.gameObject.SetActive(!segment.Segment.OnlyEnglish || UIComicMenuController.LanguageCodes.IndexOf(LocalizationManager.CurrentLanguageCode) == 0);
        }
      }
      this.showingPanelRoutine = this.StartCoroutine((IEnumerator) this.ShowingPanel(this.panels[this.index].Panel));
    }
    else
    {
      this.index = PersistenceManager.PersistentData.ComicPanelIndex != 0 ? PersistenceManager.PersistentData.ComicPanelIndex : -1;
      for (int index = this.panels.Count - 1; index >= 0; --index)
        this.panels[index].Panel.transform.SetAsLastSibling();
      this.NextPanel();
    }
  }

  public void Update()
  {
    if (UIComicMenuController.IsLoadingPage)
      return;
    int num = InputManager.General.InputIsController() ? (InputManager.UI.GetPageNavigateRightDown() ? 1 : 0) : (InputManager.UI.GetAcceptButtonDown() ? 1 : (InputManager.Gameplay.GetInteractButtonDown() ? 1 : 0));
    bool flag = InputManager.General.InputIsController() ? InputManager.UI.GetPageNavigateLeftDown() : (InputManager.General.MouseInputActive ? InputManager.Gameplay.GetRemoveFlipButtonDown() : InputManager.Gameplay.GetInteract3ButtonDown());
    if (num != 0 && this.showingPanelRoutine == null)
      this.RightPanel();
    else if (flag && PersistenceManager.PersistentData.ComicPageIndex.x > 0)
      this.LeftPanel();
    this.comicMenu.NextPromt.transform.parent.gameObject.SetActive(!this.hasChoices);
  }

  public void RightPanel()
  {
    if (this.hasChoices || (double) Time.time < (double) this.pressDelay)
      return;
    this.pressDelay = Time.time + 0.5f;
    UIManager.PlayAudio("event:/ui/change_selection");
    bool flag = false;
    if (PersistenceManager.PersistentData.ComicPageIndex == new Vector2Int(30, 1) && !PersistenceManager.PersistentData.ComicExploredPages.Contains(new Vector2Int(29, 2)) && PersistenceManager.PersistentData.ComicPanelIndex >= this.panels.Count - 2)
      flag = true;
    if (this.index >= this.panels.Count - 1 | flag)
    {
      if (this.forcedNextPanel != Vector2Int.zero)
        PersistenceManager.PersistentData.ComicPageIndex = new Vector2Int(this.forcedNextPanel.x - 2, this.forcedNextPanel.y);
      this.comicMenu.RightPage();
    }
    else
      this.NextPanel();
    this.comicMenu.UpdatePrompts();
  }

  public void LeftPanel()
  {
    this.comicMenu.RequiresAnimating = false;
    UIManager.PlayAudio("event:/ui/change_selection");
    this.StopAllCoroutines();
    if (this.index <= 0)
      this.comicMenu.LeftPage();
    else
      this.PreviousPanel();
    this.comicMenu.UpdatePrompts();
    this.ResetStylizer();
  }

  public void NextPanel(bool animate = true, bool combineTextures = true)
  {
    ++this.index;
    for (int index = 0; index < this.panels.Count; ++index)
    {
      this.panels[index].Panel.CanvasGroup.DOKill();
      if (index == this.index)
      {
        foreach (UIComicPanel.ComicSegment segment in this.panels[index].Panel.Segments)
          segment.Segment.ResetValues();
        this.panels[index].Panel.UpdateSegmentsMaterial(animate);
        this.panels[index].Panel.CanvasGroup.alpha = 1f;
      }
      else
      {
        this.panels[index].Panel.SetSegmentsMaterial(this.comicMenu.ComicMaterial);
        if (this.panels[this.index].Panel.RequiresFade)
          this.panels[index].Panel.CanvasGroup.DOFade(0.0f, 0.2f);
        else
          this.panels[index].Panel.CanvasGroup.alpha = 0.0f;
      }
    }
    if (this.index > 0)
      this.panels[this.index - 1].Panel.transform.SetAsLastSibling();
    if (!this.comicMenu.IsShowingBonus)
    {
      PersistenceManager.PersistentData.ComicPanelIndex = this.index;
      PersistenceManager.Save();
    }
    this.showingPanelRoutine = this.StartCoroutine((IEnumerator) this.ShowingPanel(this.panels[this.index].Panel));
  }

  public void PreviousPanel()
  {
    this.panels[this.index].Panel.transform.SetAsLastSibling();
    --this.index;
    if (!this.panels[this.index + 1].Panel.RequiresInput)
    {
      if (this.index > 0)
      {
        --this.index;
      }
      else
      {
        this.LeftPanel();
        return;
      }
    }
    for (int index = 0; index < this.panels.Count; ++index)
    {
      if (index == this.index)
      {
        this.panels[index].Panel.UpdateSegmentsMaterial();
        this.panels[index].Panel.CanvasGroup.DOFade(1f, 0.2f);
      }
      else
      {
        this.panels[index].Panel.SetSegmentsMaterial(this.comicMenu.ComicMaterial);
        this.panels[index].Panel.CanvasGroup.DOFade(0.0f, 0.2f);
      }
    }
    if (!this.comicMenu.IsShowingBonus)
    {
      PersistenceManager.PersistentData.ComicPanelIndex = this.index;
      PersistenceManager.Save();
    }
    this.showingPanelRoutine = this.StartCoroutine((IEnumerator) this.ShowingPanel(this.panels[this.index].Panel));
  }

  public void SetMusic(UIComicPanel panel)
  {
    AudioManager.Instance.SetMusicParam(panel.MusicParamater, (float) panel.MusicParamaterValue);
    AudioManager.Instance.SetMusicParam(panel.AltMusicParamater, panel.AltMusicParamaterValue);
    AudioManager.Instance.PlayMusic(panel.Music);
    AudioManager.Instance.SetMusicParam(panel.MusicParamater, (float) panel.MusicParamaterValue);
    AudioManager.Instance.SetMusicParam(panel.AltMusicParamater, panel.AltMusicParamaterValue);
  }

  public IEnumerator ShowingPanel(UIComicPanel panel)
  {
    UIComicPage uiComicPage = this;
    uiComicPage.CurrentPanel = panel;
    panel.gameObject.SetActive(true);
    if (!string.IsNullOrEmpty(panel.Music) && panel.Music[panel.Music.Length - 1] == ' ')
      panel.Music = panel.Music.Remove(panel.Music.Length - 1, 1);
    if (!string.IsNullOrEmpty(panel.Atmo) && panel.Atmo[panel.Atmo.Length - 1] == ' ')
      panel.Atmo = panel.Atmo.Remove(panel.Atmo.Length - 1, 1);
    if (!uiComicPage.comicMenu.IsShowingBonus)
    {
      if (!string.IsNullOrEmpty(panel.Music))
        uiComicPage.SetMusic(panel);
      else
        AudioManager.Instance.StopCurrentMusic();
    }
    if (!string.IsNullOrEmpty(panel.Atmo))
      AudioManager.Instance.PlayAtmos(panel.Atmo);
    else
      AudioManager.Instance.StopCurrentAtmos();
    Color color = uiComicPage.background.color;
    uiComicPage.background.color = Color.magenta;
    uiComicPage.comicMenu.CurrentPage = uiComicPage;
    uiComicPage.background.color = color;
    if (uiComicPage.comicMenu.RequiresCombining)
    {
      bool activeSelf = uiComicPage.comicMenu.FrontCover.gameObject.activeSelf;
      uiComicPage.comicMenu.FrontCover.gameObject.SetActive(false);
      List<float> floatList = new List<float>();
      UIComicPage[] componentsInChildren1 = uiComicPage.comicMenu.PageContainer.GetComponentsInChildren<UIComicPage>(true);
      for (int index = 0; index < componentsInChildren1.Length; ++index)
      {
        if (componentsInChildren1[index].gameObject.activeSelf && (UnityEngine.Object) componentsInChildren1[index].gameObject != (UnityEngine.Object) uiComicPage.gameObject)
        {
          floatList.Add(componentsInChildren1[index].CanvasGroup.alpha);
          componentsInChildren1[index].CanvasGroup.alpha = 0.0f;
        }
      }
      UIComicPanel[] componentsInChildren2 = uiComicPage.GetComponentsInChildren<UIComicPanel>(true);
      for (int index = 0; index < componentsInChildren2.Length; ++index)
      {
        if (componentsInChildren2[index].gameObject.activeSelf && (UnityEngine.Object) componentsInChildren2[index].gameObject != (UnityEngine.Object) panel.gameObject)
        {
          floatList.Add(componentsInChildren2[index].CanvasGroup.alpha);
          componentsInChildren2[index].CanvasGroup.alpha = 0.0f;
        }
      }
      double alpha1 = (double) uiComicPage.CanvasGroup.alpha;
      double alpha2 = (double) panel.CanvasGroup.alpha;
      uiComicPage.CanvasGroup.alpha = 1f;
      panel.CanvasGroup.alpha = 1f;
      uiComicPage.comicMenu.HidePrompts();
      uiComicPage.hasChoices = false;
      foreach (UIComicPanel.ComicSegment segment in panel.Segments)
      {
        if (segment.Segment.IsChoice)
          uiComicPage.hasChoices = true;
        segment.Segment.CombineTextures(uiComicPage.comicMenu.RequiresAnimating);
      }
      for (int index = 0; index < componentsInChildren1.Length; ++index)
      {
        if (componentsInChildren1[index].gameObject.activeSelf && (UnityEngine.Object) componentsInChildren1[index].gameObject != (UnityEngine.Object) uiComicPage.gameObject)
        {
          componentsInChildren1[index].CanvasGroup.alpha = floatList[0];
          floatList.RemoveAt(0);
        }
      }
      for (int index = 0; index < componentsInChildren2.Length; ++index)
      {
        if (componentsInChildren2[index].gameObject.activeSelf && (UnityEngine.Object) componentsInChildren2[index].gameObject != (UnityEngine.Object) panel.gameObject)
        {
          componentsInChildren2[index].CanvasGroup.alpha = floatList[0];
          floatList.RemoveAt(0);
        }
      }
      uiComicPage.CanvasGroup.alpha = uiComicPage.CanvasGroup.alpha;
      panel.CanvasGroup.alpha = panel.CanvasGroup.alpha;
      uiComicPage.comicMenu.ShowPrompts();
      if (activeSelf)
        uiComicPage.comicMenu.FrontCover.gameObject.SetActive(true);
    }
    else
    {
      foreach (UIComicBubble componentsInChild in uiComicPage.GetComponentsInChildren<UIComicBubble>(true))
      {
        componentsInChild.ForceUpdateBubble();
        componentsInChild.SetText(componentsInChild.Term, componentsInChild.FontSize, componentsInChild.Rotation, componentsInChild.Width, componentsInChild.Height);
        componentsInChild.gameObject.SetActive(SettingsManager.Settings.Game.Language != "English");
      }
    }
    uiComicPage.comicMenu.InvertPromptsColouring = panel.InvertPromptsColouring;
    bool skipped = false;
    Coroutine skipRoutine = uiComicPage.StartCoroutine((IEnumerator) uiComicPage.WaitForSkip((System.Action) (() =>
    {
      skipped = true;
      foreach (UIComicPanel.ComicSegment segment in panel.Segments)
      {
        segment.Segment.gameObject.SetActive(true);
        if ((UnityEngine.Object) segment.Segment.Image.sprite != (UnityEngine.Object) null)
          segment.Segment.Image.color = new Color(segment.Segment.Image.color.r, segment.Segment.Image.color.g, segment.Segment.Image.color.b, 1f);
        segment.Segment.GetComponent<DOTweenAnimation>().DOComplete();
        segment.Segment.GetComponent<CanvasGroup>().alpha = segment.Segment.Transition == UIComicSegment.SegmentTransition.FadeOut ? 0.0f : 1f;
        if ((UnityEngine.Object) segment.Segment.Dissolve != (UnityEngine.Object) null)
        {
          segment.Segment.Dissolve.material.SetFloat("_Dissolve", 0.0f);
          segment.Segment.Dissolve.gameObject.SetActive(false);
          segment.Segment.Image.color = new Color(segment.Segment.Image.color.r, segment.Segment.Image.color.g, segment.Segment.Image.color.b, 1f);
        }
        else
          segment.Segment.Image.material.SetFloat("_Dissolve", 1f);
        if (segment.Segment.Transition == UIComicSegment.SegmentTransition.Zoom)
          segment.Segment.Image.transform.localScale = Vector3.one * segment.Segment.ZoomScale;
        this.SetChoices(segment);
      }
    })));
    foreach (UIComicPanel.ComicSegment segment in panel.Segments)
    {
      DOTweenAnimation component = segment.Segment.GetComponent<DOTweenAnimation>();
      component.tween.SetDelay<Tween>(component.delay);
    }
    panel.Configure(uiComicPage.comicMenu.RequiresAnimating);
    float duration = 0.0f;
    foreach (UIComicPanel.ComicSegment segment1 in panel.Segments)
    {
      UIComicPanel.ComicSegment segment = segment1;
      DOTweenAnimation anim = segment.Segment.GetComponent<DOTweenAnimation>();
      duration = Mathf.Max(duration, anim.duration + anim.delay);
      if (!uiComicPage.comicMenu.RequiresAnimating)
      {
        if (segment.Segment.Transition == UIComicSegment.SegmentTransition.Punch)
          segment.Segment.GetComponent<CanvasGroup>().alpha = 1f;
        else if (segment.Segment.Transition == UIComicSegment.SegmentTransition.Fade)
        {
          if ((UnityEngine.Object) segment.Segment.Dissolve != (UnityEngine.Object) null)
          {
            segment.Segment.Dissolve.material.SetFloat("_Dissolve", 0.0f);
            segment.Segment.Dissolve.gameObject.SetActive(false);
            segment.Segment.Image.color = new Color(segment.Segment.Image.color.r, segment.Segment.Image.color.g, segment.Segment.Image.color.b, 1f);
          }
          else
            segment.Segment.Image.material.SetFloat("_Dissolve", 1f);
        }
        else if (segment.Segment.Transition == UIComicSegment.SegmentTransition.Zoom)
          segment.Segment.transform.localScale = Vector3.one * segment.Segment.ZoomScale;
        else if (segment.Segment.Transition == UIComicSegment.SegmentTransition.FadeOut)
          segment.Segment.GetComponent<CanvasGroup>().alpha = 0.0f;
      }
      segment.Segment.gameObject.SetActive(!segment.Segment.OnlyEnglish || UIComicMenuController.LanguageCodes.IndexOf(LocalizationManager.CurrentLanguageCode) == 0);
      uiComicPage.SetChoices(segment);
      if (uiComicPage.comicMenu.RequiresAnimating)
        uiComicPage.StartCoroutine((IEnumerator) uiComicPage.WaitForSeconds(anim.delay, (System.Action) (() =>
        {
          segment.Segment.OnSegmentShown?.Invoke();
          if (!string.IsNullOrEmpty(segment.Segment.SFX))
          {
            if (!string.IsNullOrEmpty(segment.Segment.SFXParamater))
              AudioManager.Instance.PlayOneShotAndSetParameterValue(segment.Segment.SFX, segment.Segment.SFXParamater, (float) segment.Segment.SFXParamaterValue);
            else
              AudioManager.Instance.PlayOneShot(segment.Segment.SFX);
          }
          if (segment.Segment.Transition == UIComicSegment.SegmentTransition.Punch && (double) anim.delay > 0.0)
          {
            segment.Segment.GetComponent<CanvasGroup>().alpha = 1f;
          }
          else
          {
            if (segment.Segment.Transition != UIComicSegment.SegmentTransition.Fade)
              return;
            UIManager.PlayAudio("event:/comic sfx/transition_dissolve");
            if (this.comicMenu.CurrentPage.BackgroundIsLight)
            {
              anim.tween = (Tween) segment.Segment.Image.material.DOFloat(1f, "_Dissolve", anim.duration);
            }
            else
            {
              if (!((UnityEngine.Object) segment.Segment.Dissolve != (UnityEngine.Object) null))
                return;
              anim.tween = (Tween) segment.Segment.Dissolve.material.DOFloat(0.0f, "_Dissolve", anim.duration).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => segment.Segment.Dissolve.gameObject.SetActive(false)));
              segment.Segment.Dissolve.gameObject.SetActive(true);
              segment.Segment.Image.color = new Color(segment.Segment.Image.color.r, segment.Segment.Image.color.g, segment.Segment.Image.color.b, 1f);
            }
          }
        })));
      else
        anim.DOComplete();
    }
    while ((double) (duration -= Time.deltaTime) > 0.0 && uiComicPage.comicMenu.RequiresAnimating && !skipped)
      yield return (object) null;
    uiComicPage.comicMenu.RequiresAnimating = true;
    uiComicPage.comicMenu.RequiresCombining = true;
    uiComicPage.StopCoroutine(skipRoutine);
    uiComicPage.showingPanelRoutine = (Coroutine) null;
    if (uiComicPage.index < uiComicPage.panels.Count - 1 && !uiComicPage.panels[uiComicPage.index + 1].Panel.RequiresInput)
      uiComicPage.RightPanel();
    else if (!uiComicPage.panels[uiComicPage.index].Panel.RequiresInputPost)
      uiComicPage.RightPanel();
    if (PersistenceManager.PersistentData.ComicPageIndex.x == 0)
      uiComicPage.comicMenu.ShowPrompts();
    uiComicPage.comicMenu.UpdatePrompts();
    if (uiComicPage.hasChoices)
    {
      foreach (UIComicPanel.ComicSegment segment in panel.Segments)
      {
        if (segment.Segment.IsChoice)
        {
          MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) segment.Segment.Button);
          break;
        }
      }
    }
  }

  public void SetChoices(UIComicPanel.ComicSegment segment)
  {
    if (!segment.Segment.IsChoice || segment.Segment.PageOverride != 32 /*0x20*/)
      return;
    if (!PersistenceManager.PersistentData.ComicExploredPages.Contains(new Vector2Int(29, 2)))
      this.HideChoices(new Vector2Int(32 /*0x20*/, 1));
    else
      this.ShowChoices();
  }

  public void HideChoices(Vector2Int forcedNextPanel)
  {
    this.forcedNextPanel = forcedNextPanel;
    foreach (UIComicPanel.ComicSegment segment in this.Panels[this.index].Panel.Segments)
    {
      if (segment.Segment.IsChoice)
        segment.Segment.gameObject.SetActive(false);
    }
    this.hasChoices = false;
    this.comicMenu.ShowPrompts();
  }

  public void ShowChoices()
  {
    this.forcedNextPanel = Vector2Int.zero;
    foreach (UIComicPanel.ComicSegment segment in this.Panels[this.index].Panel.Segments)
    {
      if (segment.Segment.IsChoice)
        segment.Segment.gameObject.SetActive(true);
    }
  }

  public IEnumerator WaitForSkip(System.Action skipCallback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(0.5f);
    while (true)
    {
      if (PersistenceManager.PersistentData.ComicExploredHistory.x > this.comicMenu.PageIndex.x && (InputManager.UI.GetAcceptButtonDown() || InputManager.Gameplay.GetAttackButtonDown() || InputManager.UI.GetPageNavigateRightDown()))
      {
        System.Action action = skipCallback;
        if (action != null)
          action();
      }
      yield return (object) null;
    }
  }

  public IEnumerator WaitForSeconds(float delay, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SplitPages(out Sprite leftSprite, out Sprite rightSprite)
  {
    this.SplitPages(this.Panels[0].Panel.Segments[0].Segment.Image.sprite, out leftSprite, out rightSprite);
  }

  public void SplitPages(Sprite sprite, out Sprite leftSprite, out Sprite rightSprite)
  {
    int height = (int) sprite.rect.height;
    int width = (int) sprite.rect.width;
    int num1 = width / 2;
    int x = (sprite.texture.width - width) / 2;
    Texture2D texture1 = new Texture2D(num1, height, TextureFormat.RGB24, false);
    Texture2D texture2 = new Texture2D(num1, height, TextureFormat.RGB24, false);
    Color[] pixels = sprite.texture.GetPixels(x, 0, width, height);
    Color[] colorArray1 = new Color[num1 * height];
    Color[] colorArray2 = new Color[num1 * height];
    for (int index = 0; index < height; ++index)
    {
      int sourceIndex = index * width;
      int destinationIndex = index * num1;
      int num2 = index * num1 + num1;
      Array.Copy((Array) pixels, sourceIndex, (Array) colorArray1, destinationIndex, num1);
      Array.Copy((Array) pixels, sourceIndex + num1, (Array) colorArray2, num2 - num1, num1);
    }
    texture1.SetPixels(colorArray1);
    texture2.SetPixels(colorArray2);
    texture1.Apply();
    texture2.Apply();
    leftSprite = Sprite.Create(texture1, new Rect(0.0f, 0.0f, (float) num1, (float) height), Vector2.one * 0.5f);
    rightSprite = Sprite.Create(texture2, new Rect(0.0f, 0.0f, (float) num1, (float) height), Vector2.one * 0.5f);
  }

  public Sprite SquishPage() => this.SquishPage(1920, 1080);

  public Sprite SquishPage(int width, int height)
  {
    this.comicMenu.HidePrompts();
    RenderTexture temporary = RenderTexture.GetTemporary(1920, 1080);
    this.comicMenu.Camera.targetTexture = temporary;
    RenderTexture.active = temporary;
    this.comicMenu.Camera.Render();
    width = Mathf.Clamp(width, 0, 1920);
    height = Mathf.Clamp(height, 0, 1080);
    Texture2D texture = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
    texture.ReadPixels(new Rect(0.0f, 0.0f, 1920f, 1080f), 0, 0);
    texture.Apply();
    Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, (float) width, (float) height), Vector2.one * 0.5f);
    RenderTexture.active = (RenderTexture) null;
    this.comicMenu.Camera.targetTexture = (RenderTexture) null;
    RenderTexture.ReleaseTemporary(temporary);
    this.comicMenu.ShowPrompts();
    return sprite;
  }

  public void FadePanels()
  {
    for (int index = 0; index < this.panels.Count; ++index)
    {
      this.panels[index].Panel.CanvasGroup.DOKill();
      this.panels[index].Panel.SetSegmentsMaterial(this.comicMenu.ComicMaterial);
      this.panels[index].Panel.CanvasGroup.DOFade(0.0f, 0.2f);
    }
  }

  public void ResetStylizer()
  {
    foreach (UIComicStylizerAdjust componentsInChild in this.GetComponentsInChildren<UIComicStylizerAdjust>(true))
      componentsInChild.KillTween();
    this.comicMenu.Stylizer.Palette = this.comicMenu.ComicPalette;
    this.comicMenu.Stylizer.EffectIntensity = 0.0f;
  }

  public void OnDestroy()
  {
    for (int index = 0; index < this.panels.Count; ++index)
    {
      foreach (UIComicPanel.ComicSegment segment in this.panels[index].Panel.Segments)
      {
        DOTweenAnimation component = segment.Segment.GetComponent<DOTweenAnimation>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.isValid)
        {
          component.DOKill();
          UnityEngine.Object.Destroy((UnityEngine.Object) component);
        }
      }
    }
    if (this.showingPanelRoutine == null)
      return;
    this.StopCoroutine(this.showingPanelRoutine);
    this.showingPanelRoutine = (Coroutine) null;
  }

  [Serializable]
  public class ComicPanel
  {
    public UIComicPanel Panel;
  }
}
