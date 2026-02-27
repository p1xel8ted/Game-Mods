// Decompiled with JetBrains decompiler
// Type: HUD_DisplayName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using BlendModes;
using I2.Loc;
using Lamb.UI;
using MMBiomeGeneration;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class HUD_DisplayName : BaseMonoBehaviour
{
  private static HUD_DisplayName Instance;
  private TextMeshProUGUI Text_English;
  private TextMeshProUGUI Text_Korean;
  private TextMeshProUGUI Text_ChineseTraditional;
  private TextMeshProUGUI Text_ChineseSimplified;
  private TextMeshProUGUI Text_Russian;
  private TextMeshProUGUI Text_Japanese;
  public GameObject Text_English_GO;
  private GameObject Text_Korean_GO;
  private GameObject Text_ChineseTraditional_GO;
  private GameObject Text_ChineseSimplified_GO;
  private GameObject Text_Russian_GO;
  private GameObject Text_Japanese_GO;
  public bool DarkMode;
  public BlendModeEffect blendMode;
  public CanvasGroup canvasGroup;
  private HUD_DisplayName.Positions Position;
  private string _localizationKey;
  private Canvas canvas;

  private void Awake() => this.CreateObjectForLanguage();

  private void CreateObjectForLanguage()
  {
    switch (LocalizationManager.CurrentLanguage)
    {
      case "English":
        if (!((UnityEngine.Object) this.Text_English_GO == (UnityEngine.Object) null))
          break;
        Addressables.InstantiateAsync((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/English.prefab", this.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (result => this.Text_English_GO = result.Result);
        break;
      case "Japanese":
        if (!((UnityEngine.Object) this.Text_Japanese_GO == (UnityEngine.Object) null))
          break;
        Addressables.InstantiateAsync((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Japanese.prefab", this.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (result => this.Text_Japanese_GO = result.Result);
        break;
      case "Russian":
        if (!((UnityEngine.Object) this.Text_Russian_GO == (UnityEngine.Object) null))
          break;
        Addressables.InstantiateAsync((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Russian.prefab", this.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (result => this.Text_Russian_GO = result.Result);
        break;
      case "Chinese (Simplified)":
        if (!((UnityEngine.Object) this.Text_ChineseSimplified_GO == (UnityEngine.Object) null))
          break;
        Addressables.InstantiateAsync((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Chinese-Simplified.prefab", this.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (result => this.Text_ChineseSimplified_GO = result.Result);
        break;
      case "Chinese (Traditional)":
        if (!((UnityEngine.Object) this.Text_ChineseTraditional_GO == (UnityEngine.Object) null))
          break;
        Addressables.InstantiateAsync((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Chinese-Traditional.prefab", this.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (result => this.Text_ChineseTraditional_GO = result.Result);
        break;
      case "Korean":
        if (!((UnityEngine.Object) this.Text_Korean_GO == (UnityEngine.Object) null))
          break;
        Addressables.InstantiateAsync((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/Korean.prefab", this.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (result => this.Text_Korean_GO = result.Result);
        break;
      default:
        if (!((UnityEngine.Object) this.Text_English_GO == (UnityEngine.Object) null))
          break;
        Addressables.InstantiateAsync((object) "Assets/Resources_moved/Prefabs/UI/DisplayNames/English.prefab", this.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (result => this.Text_English_GO = result.Result);
        break;
    }
  }

  private TextMeshProUGUI GetTextLanguage()
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

  public static void Play(
    string Name,
    int Delay = 5,
    HUD_DisplayName.Positions Position = HUD_DisplayName.Positions.BottomRight,
    HUD_DisplayName.textBlendMode blend = HUD_DisplayName.textBlendMode.Normal)
  {
    if ((UnityEngine.Object) HUD_DisplayName.Instance == (UnityEngine.Object) null)
      HUD_DisplayName.Instance = UnityEngine.Object.FindObjectOfType<HUD_DisplayName>();
    Debug.Log((object) ("Play Hud Display name, Dark mode: " + (object) blend));
    HUD_DisplayName.Instance.StartCoroutine((IEnumerator) HUD_DisplayName.Instance.YieldForText((System.Action) (() =>
    {
      TextMeshProUGUI textLanguage = HUD_DisplayName.Instance.GetTextLanguage();
      if ((UnityEngine.Object) textLanguage == (UnityEngine.Object) null)
        return;
      textLanguage.alpha = 0.0f;
      HUD_DisplayName.Instance.canvasGroup.alpha = 0.0f;
      HUD_DisplayName.Instance._localizationKey = Name;
      Name = LocalizationManager.Sources[0].GetTranslation(HUD_DisplayName.Instance._localizationKey);
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
        default:
          textLanguage.color = new Color(0.0f, 0.41f, 0.8f, 1f);
          textLanguage.GetComponent<BlendModeEffect>().BlendMode = BlendMode.Divide;
          break;
      }
      HUD_DisplayName.Instance.Show(Name, (float) Delay / 1.5f, Position);
    })));
  }

  private IEnumerator YieldForText(System.Action andThen)
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
    HUD_DisplayName.Instance.Show(Name, (float) Delay, Position);
  }

  public static void Play(string Name, float Delay, bool DarkMode = false)
  {
    HUD_DisplayName.Instance.Show(Name, Delay);
    HUD_DisplayName.Instance.DarkMode = DarkMode;
  }

  private void DisableTexts()
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

  private void Start()
  {
    this.DisableTexts();
    if (!((UnityEngine.Object) this.blendMode == (UnityEngine.Object) null))
      return;
    this.blendMode = this.GetComponentInChildren<BlendModeEffect>();
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
      textLanguage.text = Name;
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.ShowText(Delay, waitForUI));
    })));
  }

  private IEnumerator ShowText(float Delay, bool waitForUI = false)
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

  private void Hide()
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

  private void OnEnable()
  {
    HUD_DisplayName.Instance = this;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.Hide);
    this.canvas = this.GetComponentInParent<Canvas>();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLocalize);
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
  }

  private void OnNewDay()
  {
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    if ((UnityEngine.Object) HUD_DisplayName.Instance == (UnityEngine.Object) null)
      HUD_DisplayName.Instance = UnityEngine.Object.FindObjectOfType<HUD_DisplayName>();
    HUD_DisplayName.Instance._localizationKey = "UI/DayNumber";
    string Name = LocalizationManager.Sources[0].GetTranslation(HUD_DisplayName.Instance._localizationKey).Replace("{0}", TimeManager.CurrentDay.ToString());
    HUD_DisplayName.Instance.Show(Name, 3f, HUD_DisplayName.Positions.Centre);
  }

  private void OnDisable()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.Hide);
    HUD_DisplayName.Instance = (HUD_DisplayName) null;
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLocalize);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
  }

  private void OnLocalize()
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
      if (HUD_DisplayName.Instance._localizationKey != "UI/DayNumber")
      {
        textLanguage.text = LocalizationManager.Sources[0].GetTranslation(HUD_DisplayName.Instance._localizationKey);
      }
      else
      {
        string str = LocalizationManager.Sources[0].GetTranslation(HUD_DisplayName.Instance._localizationKey).Replace("{0}", TimeManager.CurrentDay.ToString());
        textLanguage.text = str;
      }
    }
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
  }
}
