// Decompiled with JetBrains decompiler
// Type: HUD_DisplayName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using BlendModes;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMBiomeGeneration;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class HUD_DisplayName : BaseMonoBehaviour
{
  public static HUD_DisplayName Instance;
  public TextMeshProUGUI Text_English;
  public TextMeshProUGUI Text_Korean;
  public TextMeshProUGUI Text_ChineseTraditional;
  public TextMeshProUGUI Text_ChineseSimplified;
  public TextMeshProUGUI Text_Russian;
  public TextMeshProUGUI Text_Japanese;
  public GameObject Text_English_GO;
  public GameObject Text_Korean_GO;
  public GameObject Text_ChineseTraditional_GO;
  public GameObject Text_ChineseSimplified_GO;
  public GameObject Text_Russian_GO;
  public GameObject Text_Japanese_GO;
  public bool DarkMode;
  public BlendModeEffect blendMode;
  public CanvasGroup canvasGroup;
  [SerializeField]
  public CanvasGroup winterSeverityContainer;
  [SerializeField]
  public CanvasGroup[] winterSeverityIcons;
  public HUD_DisplayName.Positions Position;
  public string _localizationKey;
  public int winterSeverity = -1;
  public Canvas canvas;

  public IEnumerator Start()
  {
    HUD_DisplayName hudDisplayName = this;
    hudDisplayName.winterSeverityContainer.alpha = 0.0f;
    yield return (object) null;
    hudDisplayName.CreateObjectForLanguage();
    hudDisplayName.DisableTexts();
    if ((UnityEngine.Object) hudDisplayName.blendMode == (UnityEngine.Object) null)
      hudDisplayName.blendMode = hudDisplayName.GetComponentInChildren<BlendModeEffect>();
  }

  public void CreateObjectForLanguage()
  {
    switch (LocalizationManager.CurrentLanguage)
    {
      case "English":
        if (!((UnityEngine.Object) this.Text_English_GO == (UnityEngine.Object) null))
          break;
        this.Text_English_GO = Addressables_wrapper.InstantiateSynchrous((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/English.prefab", this.transform);
        break;
      case "Japanese":
        if (!((UnityEngine.Object) this.Text_Japanese_GO == (UnityEngine.Object) null))
          break;
        this.Text_Japanese_GO = Addressables_wrapper.InstantiateSynchrous((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Japanese.prefab", this.transform);
        break;
      case "Russian":
        if (!((UnityEngine.Object) this.Text_Russian_GO == (UnityEngine.Object) null))
          break;
        this.Text_Russian_GO = Addressables_wrapper.InstantiateSynchrous((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Russian.prefab", this.transform);
        break;
      case "Chinese (Simplified)":
        if (!((UnityEngine.Object) this.Text_ChineseSimplified_GO == (UnityEngine.Object) null))
          break;
        this.Text_ChineseSimplified_GO = Addressables_wrapper.InstantiateSynchrous((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Chinese-Simplified.prefab", this.transform);
        break;
      case "Chinese (Traditional)":
        if (!((UnityEngine.Object) this.Text_ChineseTraditional_GO == (UnityEngine.Object) null))
          break;
        this.Text_ChineseTraditional_GO = Addressables_wrapper.InstantiateSynchrous((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Chinese-Traditional.prefab", this.transform);
        break;
      case "Korean":
        if (!((UnityEngine.Object) this.Text_Korean_GO == (UnityEngine.Object) null))
          break;
        this.Text_Korean_GO = Addressables_wrapper.InstantiateSynchrous((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Korean.prefab", this.transform);
        break;
      default:
        if (!((UnityEngine.Object) this.Text_English_GO == (UnityEngine.Object) null))
          break;
        this.Text_English_GO = Addressables_wrapper.InstantiateSynchrous((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/English.prefab", this.transform);
        break;
    }
  }

  public TextMeshProUGUI GetTextLanguage()
  {
    switch (LocalizationManager.CurrentLanguage)
    {
      case "English":
        return (UnityEngine.Object) this.Text_English_GO != (UnityEngine.Object) null ? this.Text_English_GO.GetComponentInChildren<TextMeshProUGUI>(true) : (TextMeshProUGUI) null;
      case "Japanese":
        return (UnityEngine.Object) this.Text_Japanese_GO != (UnityEngine.Object) null ? this.Text_Japanese_GO.GetComponentInChildren<TextMeshProUGUI>(true) : (TextMeshProUGUI) null;
      case "Russian":
        return (UnityEngine.Object) this.Text_Russian_GO != (UnityEngine.Object) null ? this.Text_Russian_GO.GetComponentInChildren<TextMeshProUGUI>(true) : (TextMeshProUGUI) null;
      case "Chinese (Simplified)":
        return (UnityEngine.Object) this.Text_ChineseSimplified_GO != (UnityEngine.Object) null ? this.Text_ChineseSimplified_GO.GetComponentInChildren<TextMeshProUGUI>(true) : (TextMeshProUGUI) null;
      case "Chinese (Traditional)":
        return (UnityEngine.Object) this.Text_ChineseTraditional_GO != (UnityEngine.Object) null ? this.Text_ChineseTraditional_GO.GetComponentInChildren<TextMeshProUGUI>(true) : (TextMeshProUGUI) null;
      case "Korean":
        return (UnityEngine.Object) this.Text_Korean_GO != (UnityEngine.Object) null ? this.Text_Korean_GO.GetComponentInChildren<TextMeshProUGUI>(true) : (TextMeshProUGUI) null;
      default:
        return (UnityEngine.Object) this.Text_English_GO != (UnityEngine.Object) null ? this.Text_English_GO.GetComponentInChildren<TextMeshProUGUI>(true) : (TextMeshProUGUI) null;
    }
  }

  public void PlayWinterDebug()
  {
    HUD_DisplayName.Play("WINTER DEBUG", Position: HUD_DisplayName.Positions.Centre, blend: HUD_DisplayName.textBlendMode.Winter, winterSeverity: this.winterSeverity);
    ++this.winterSeverity;
  }

  public static void Play(
    string Name,
    int Delay = 5,
    HUD_DisplayName.Positions Position = HUD_DisplayName.Positions.BottomRight,
    HUD_DisplayName.textBlendMode blend = HUD_DisplayName.textBlendMode.Normal,
    int winterSeverity = -1)
  {
    if ((UnityEngine.Object) HUD_DisplayName.Instance == (UnityEngine.Object) null)
      HUD_DisplayName.Instance = UnityEngine.Object.FindObjectOfType<HUD_DisplayName>();
    HUD_DisplayName.Instance.StartCoroutine((IEnumerator) HUD_DisplayName.Instance.YieldForText((System.Action) (() =>
    {
      TextMeshProUGUI textLanguage = HUD_DisplayName.Instance.GetTextLanguage();
      if ((UnityEngine.Object) textLanguage == (UnityEngine.Object) null)
        return;
      textLanguage.alpha = 0.0f;
      HUD_DisplayName.Instance.canvasGroup.alpha = 0.0f;
      HUD_DisplayName.Instance._localizationKey = Name;
      Name = LocalizationManager.Sources[0].GetTranslation(HUD_DisplayName.Instance._localizationKey);
      Name = LocalizeIntegration.ConvertToRTL(Name);
      Name = LocalizeIntegration.Arabic_ReverseNonRTL(Name);
      switch (blend)
      {
        case HUD_DisplayName.textBlendMode.FrogBoss:
          textLanguage.color = Color.red;
          textLanguage.GetComponent<BlendModeEffect>().BlendMode = BlendMode.Divide;
          break;
        case HUD_DisplayName.textBlendMode.DungeonFinal:
          textLanguage.color = Color.red;
          textLanguage.GetComponent<BlendModeEffect>().BlendMode = BlendMode.Darken;
          break;
        case HUD_DisplayName.textBlendMode.Winter:
          textLanguage.color = new Color(0.139f, 0.07f, 0.327f, 1f);
          textLanguage.GetComponent<BlendModeEffect>().BlendMode = BlendMode.ColorBurn;
          break;
        default:
          textLanguage.color = new Color(0.0f, 0.41f, 0.8f, 1f);
          textLanguage.GetComponent<BlendModeEffect>().BlendMode = BlendMode.Divide;
          break;
      }
      HUD_DisplayName.Instance.Show(Name, (float) Delay / 1.5f, Position);
      if (winterSeverity == -1)
        return;
      HUD_DisplayName.Instance.winterSeverityContainer.DOFade(1f, 1f);
      HUD_DisplayName.Instance.winterSeverityContainer.DOFade(0.0f, 1f).SetDelay<TweenerCore<float, float, FloatOptions>>((float) Delay / 1.25f);
      int index1 = Mathf.Clamp(winterSeverity - 1, 0, HUD_DisplayName.Instance.winterSeverityIcons.Length - 1);
      HUD_DisplayName.Instance.winterSeverityIcons[index1].gameObject.SetActive(true);
      for (int index2 = 0; index2 < HUD_DisplayName.Instance.winterSeverityIcons.Length; ++index2)
        HUD_DisplayName.Instance.winterSeverityIcons[index2].alpha = index1 > index2 ? 1f : 0.0f;
      HUD_DisplayName.Instance.winterSeverityIcons[index1].DOFade(1f, 0.5f).SetDelay<TweenerCore<float, float, FloatOptions>>(2f);
      HUD_DisplayName.Instance.winterSeverityIcons[index1].transform.parent.transform.DOPunchPosition(new Vector3(0.0f, 20f, 0.0f), 0.5f).SetDelay<Tweener>(2f).OnPlay<Tweener>((TweenCallback) (() =>
      {
        MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
        UIManager.PlayAudio("event:/dlc/env/winter/start_ground");
        UIManager.PlayAudio("event:/ui/faith_up_glassball");
      }));
    })));
  }

  public IEnumerator YieldForText(System.Action andThen)
  {
    while ((UnityEngine.Object) HUD_DisplayName.Instance.GetTextLanguage() == (UnityEngine.Object) null)
      yield return (object) new WaitForSecondsRealtime(0.1f);
    System.Action action = andThen;
    if (action != null)
      action();
  }

  public static void PlayTranslatedText(string Name, int Delay = 5, HUD_DisplayName.Positions Position = HUD_DisplayName.Positions.BottomRight)
  {
    if ((UnityEngine.Object) HUD_DisplayName.Instance == (UnityEngine.Object) null)
      HUD_DisplayName.Instance = UnityEngine.Object.FindObjectOfType<HUD_DisplayName>();
    TextMeshProUGUI textLanguage = HUD_DisplayName.Instance.GetTextLanguage();
    if ((UnityEngine.Object) textLanguage == (UnityEngine.Object) null)
      return;
    textLanguage.color = new Color(0.0f, 0.41f, 0.8f, 1f);
    textLanguage.GetComponent<BlendModeEffect>().BlendMode = BlendMode.Divide;
    Name = LocalizeIntegration.Arabic_ReverseNonRTL(Name);
    HUD_DisplayName.Instance.Show(Name, (float) Delay, Position);
  }

  public static void Play(string Name, float Delay, bool DarkMode = false)
  {
    HUD_DisplayName.Instance.Show(Name, Delay);
    HUD_DisplayName.Instance.DarkMode = DarkMode;
  }

  public void DisableTexts()
  {
    if ((UnityEngine.Object) this.Text_English_GO != (UnityEngine.Object) null)
      this.Text_English_GO.SetActive(false);
    if ((UnityEngine.Object) this.Text_Korean_GO != (UnityEngine.Object) null)
      this.Text_Korean_GO.SetActive(false);
    if ((UnityEngine.Object) this.Text_ChineseTraditional_GO != (UnityEngine.Object) null)
      this.Text_ChineseTraditional_GO.SetActive(false);
    if ((UnityEngine.Object) this.Text_ChineseSimplified_GO != (UnityEngine.Object) null)
      this.Text_ChineseSimplified_GO.SetActive(false);
    if ((UnityEngine.Object) this.Text_Russian_GO != (UnityEngine.Object) null)
      this.Text_Russian_GO.SetActive(false);
    if (!((UnityEngine.Object) this.Text_Japanese_GO != (UnityEngine.Object) null))
      return;
    this.Text_Japanese_GO.SetActive(false);
  }

  public void ShowAndTranslate(string Name, float Delay)
  {
    this.Show(LocalizationManager.Sources[0].GetTranslation(Name), Delay);
  }

  public void Show(string Name, float Delay, HUD_DisplayName.Positions Position = HUD_DisplayName.Positions.BottomRight, bool waitForUI = false)
  {
    HUD_DisplayName.Instance.StartCoroutine((IEnumerator) HUD_DisplayName.Instance.YieldForText((System.Action) (() =>
    {
      TextMeshProUGUI textLanguage = this.GetTextLanguage();
      if ((UnityEngine.Object) textLanguage == (UnityEngine.Object) null)
        return;
      switch (Position)
      {
        case HUD_DisplayName.Positions.BottomRight:
          textLanguage.fontSize = 125f;
          textLanguage.rectTransform.anchoredPosition = (Vector2) (new Vector3(-960f, 91f) * this.canvas.scaleFactor);
          textLanguage.alignment = TextAlignmentOptions.Right;
          break;
        case HUD_DisplayName.Positions.Centre:
          textLanguage.fontSize = 150f;
          textLanguage.rectTransform.localPosition = new Vector3(0.0f, 0.0f) * this.canvas.scaleFactor;
          textLanguage.alignment = TextAlignmentOptions.Center;
          break;
      }
      textLanguage.text = "<uppercase>" + Name;
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.ShowText(Delay, waitForUI));
    })));
  }

  public IEnumerator ShowText(float Delay, bool waitForUI = false)
  {
    HUD_DisplayName hudDisplayName = this;
    while ((UnityEngine.Object) hudDisplayName.GetTextLanguage() == (UnityEngine.Object) null)
      yield return (object) new WaitForSecondsRealtime(0.1f);
    TextMeshProUGUI Text = hudDisplayName.GetTextLanguage();
    yield return (object) new WaitForEndOfFrame();
    if (waitForUI)
    {
      if (LetterBox.IsPlaying)
        yield return (object) null;
      if (UIMenuBase.ActiveMenus.Count > 0)
        yield return (object) null;
    }
    if ((double) Time.timeScale != 0.0 && Text.text != "")
      AudioManager.Instance.PlayOneShot("event:/Stings/area_text_intro", hudDisplayName.gameObject);
    hudDisplayName.canvasGroup.alpha = 1f;
    HUD_DisplayName.Instance.canvasGroup.alpha = 1f;
    Text.transform.parent.gameObject.SetActive(true);
    Text.gameObject.SetActive(true);
    Text.alpha = 0.0f;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) <= 1.0)
    {
      Text.alpha = Mathf.Lerp(0.0f, 1f, Progress);
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(Delay);
    Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) <= 1.0)
    {
      Text.alpha = Mathf.Lerp(1f, 0.0f, Progress);
      yield return (object) null;
    }
    hudDisplayName.canvasGroup.alpha = 0.0f;
    Text.text = "";
    Text.gameObject.SetActive(false);
  }

  public void Hide()
  {
    TextMeshProUGUI textLanguage = this.GetTextLanguage();
    this.StopAllCoroutines();
    this.canvasGroup.alpha = 0.0f;
    if (!((UnityEngine.Object) textLanguage != (UnityEngine.Object) null))
      return;
    textLanguage.text = "";
  }

  public void HideText()
  {
    TextMeshProUGUI textLanguage = this.GetTextLanguage();
    this.StopAllCoroutines();
    if ((UnityEngine.Object) textLanguage != (UnityEngine.Object) null)
    {
      textLanguage.text = "";
      textLanguage.gameObject.SetActive(false);
    }
    this.canvasGroup.alpha = 0.0f;
  }

  public void OnEnable()
  {
    HUD_DisplayName.Instance = this;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.Hide);
    this.canvas = this.GetComponentInParent<Canvas>();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLocalize);
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
  }

  public void OnNewDay()
  {
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    if ((UnityEngine.Object) HUD_DisplayName.Instance == (UnityEngine.Object) null)
      HUD_DisplayName.Instance = UnityEngine.Object.FindObjectOfType<HUD_DisplayName>();
    HUD_DisplayName.Instance._localizationKey = "UI/DayNumber";
    string str = LocalizationManager.Sources[0].GetTranslation(HUD_DisplayName.Instance._localizationKey).Replace("{0}", TimeManager.CurrentDay.ToString());
    if (LocalizeIntegration.IsArabic())
      str = LocalizeIntegration.ConvertToRTL(str);
    TextMeshProUGUI textLanguage = HUD_DisplayName.Instance.GetTextLanguage();
    if ((UnityEngine.Object) textLanguage == (UnityEngine.Object) null)
      return;
    textLanguage.color = new Color(0.0f, 0.41f, 0.8f, 1f);
    textLanguage.GetComponent<BlendModeEffect>().BlendMode = BlendMode.Divide;
    HUD_DisplayName.Instance.Show(str, 3f, HUD_DisplayName.Positions.Centre);
  }

  public void OnDisable()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.Hide);
    HUD_DisplayName.Instance = (HUD_DisplayName) null;
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLocalize);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
  }

  public void OnLocalize()
  {
    this.DisableTexts();
    this.CreateObjectForLanguage();
    TextMeshProUGUI textLanguage = this.GetTextLanguage();
    if ((UnityEngine.Object) textLanguage == (UnityEngine.Object) null)
    {
      Debug.Log((object) "Text is null!");
    }
    else
    {
      textLanguage.alpha = 1f;
      textLanguage.gameObject.SetActive(true);
      textLanguage.fontSize = 150f;
      textLanguage.rectTransform.localPosition = new Vector3(0.0f, 0.0f) * this.canvas.scaleFactor;
      textLanguage.alignment = TextAlignmentOptions.Center;
      if (!((UnityEngine.Object) HUD_DisplayName.Instance != (UnityEngine.Object) null) || HUD_DisplayName.Instance._localizationKey == null)
        return;
      if (HUD_DisplayName.Instance._localizationKey != "UI/DayNumber")
      {
        textLanguage.text = "<uppercase>" + LocalizationManager.Sources[0].GetTranslation(HUD_DisplayName.Instance._localizationKey).ToUpper();
      }
      else
      {
        string text = LocalizationManager.Sources[0].GetTranslation(HUD_DisplayName.Instance._localizationKey).Replace("{0}", TimeManager.CurrentDay.ToString());
        if (LocalizeIntegration.IsArabic())
          text = LocalizeIntegration.ConvertToRTL(text);
        textLanguage.text = "<uppercase>" + text;
      }
    }
  }

  public string GetOrdinalIndicator(int number)
  {
    string Term = "UI/First";
    switch (number)
    {
      case 2:
        Term = "UI/Second";
        break;
      case 3:
        Term = "UI/Third";
        break;
      default:
        if (number == 4 || number == 0)
        {
          Term = "UI/Fourth";
          break;
        }
        break;
    }
    if (number >= 5)
    {
      int num = number % 10;
      if (number >= 5 && number <= 20 || num == 0 || num >= 4)
      {
        Term = "UI/Fourth";
      }
      else
      {
        switch (num)
        {
          case 1:
            Term = "UI/First";
            break;
          case 2:
            Term = "UI/Second";
            break;
          case 3:
            Term = "UI/Third";
            break;
        }
      }
    }
    return LocalizationManager.GetTranslation(Term);
  }

  public enum Positions
  {
    BottomRight,
    Centre,
  }

  public enum textBlendMode
  {
    Normal,
    FrogBoss,
    DungeonFinal,
    Winter,
  }
}
