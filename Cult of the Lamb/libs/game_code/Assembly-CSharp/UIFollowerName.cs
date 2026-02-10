// Decompiled with JetBrains decompiler
// Type: UIFollowerName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class UIFollowerName : MonoBehaviour
{
  [SerializeField]
  public TMP_Text nameText;
  [SerializeField]
  public Localize _localize;
  [SerializeField]
  public Material normalMaterial;
  [SerializeField]
  public Material normalMaterialNoStyle;
  [SerializeField]
  public Material twitchMaterial;
  [SerializeField]
  public Material twitchMaterialNoStyle;
  [SerializeField]
  public SpriteRenderer textRenderer;
  public int index = -1;
  public Follower follower;
  public bool _shown = true;
  public bool showAllNames;
  public bool showTwitchNames;
  public TMP_FontAsset regularFont;
  public bool isTextSet;
  public const float DELTA_UPDATE = 0.5f;
  public float _deltaUpdate = 0.5f;

  public void Awake()
  {
    this.follower = this.GetComponentInParent<Follower>();
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
    {
      this.follower.OnFollowerBrainAssigned += new System.Action(this.OnBrainAssigned);
      if (this.follower.Brain != null)
        this.OnBrainAssigned();
    }
    this.showAllNames = SettingsManager.Settings.Game.ShowFollowerNames;
    this.showTwitchNames = TwitchManager.FollowerNamesEnabled;
    this.OnFontChanged(true);
    this.regularFont = this.nameText.font;
    if (!this.showAllNames)
      this.Hide(false);
    GameplaySettingsUtilities.OnShowFollowerNamesChanged += new Action<bool>(this.OnFollowerNamesEnabledChanged);
    TwitchManager.OnFollowerNamesEnabledChanged += new Action<bool>(this.OnTwitchFollowerNamesEnabledChanged);
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
      this.follower.OnFollowerBrainAssigned -= new System.Action(this.OnBrainAssigned);
    GameplaySettingsUtilities.OnShowFollowerNamesChanged -= new Action<bool>(this.OnFollowerNamesEnabledChanged);
    TwitchManager.OnFollowerNamesEnabledChanged -= new Action<bool>(this.OnTwitchFollowerNamesEnabledChanged);
    if (!((UnityEngine.Object) FollowersNameManager.Instance != (UnityEngine.Object) null))
      return;
    FollowersNameManager.Instance.RemoveNameplate(this.index);
  }

  public void OnEnable()
  {
    this.OnFontChanged(true);
    Singleton<AccessibilityManager>.Instance.OnRemoveTextStylingChanged += new Action<bool>(this.OnFontChanged);
    Singleton<AccessibilityManager>.Instance.OnDyslexicFontValueChanged += new Action<bool>(this.OnFontChanged);
  }

  public void OnDisable()
  {
    Singleton<AccessibilityManager>.Instance.OnRemoveTextStylingChanged -= new Action<bool>(this.OnFontChanged);
    Singleton<AccessibilityManager>.Instance.OnDyslexicFontValueChanged -= new Action<bool>(this.OnFontChanged);
  }

  public void OnFontChanged(bool value)
  {
    bool dyslexicFont1 = SettingsManager.Settings.Accessibility.DyslexicFont;
    if (LocalizationManager.CurrentLanguage == "English")
    {
      if (dyslexicFont1)
      {
        if ((UnityEngine.Object) LocalizationManager.DyslexicFontAsset == (UnityEngine.Object) null)
        {
          LocalizationManager.CheckForDyslexicFonts((Action<TMP_FontAsset>) (dyslexicFont =>
          {
            this.nameText.font = LocalizationManager.DyslexicFontAsset;
            this.RegenerateLabels();
          }));
        }
        else
        {
          this.nameText.font = LocalizationManager.DyslexicFontAsset;
          this.RegenerateLabels();
        }
      }
      else if ((UnityEngine.Object) this.regularFont == (UnityEngine.Object) null || this.regularFont.name.Contains("Dyslexic"))
      {
        LocalizationManager.GetLaptureRegularFont((Action<TMP_FontAsset>) (font =>
        {
          this.regularFont = font;
          this.nameText.font = this.regularFont;
          this.RegenerateLabels();
        }));
      }
      else
      {
        this.nameText.font = this.regularFont;
        this.RegenerateLabels();
      }
    }
    else if ((UnityEngine.Object) this.regularFont == (UnityEngine.Object) LocalizationManager.DyslexicFontAsset)
    {
      LocalizationManager.GetLaptureRegularFont((Action<TMP_FontAsset>) (font =>
      {
        this.regularFont = font;
        this.nameText.font = this.regularFont;
        this.RegenerateLabels();
      }));
    }
    else
    {
      this.nameText.font = this.regularFont;
      this.RegenerateLabels();
    }
  }

  public void RegenerateLabels() => this.StartCoroutine((IEnumerator) this.Regenerate());

  public IEnumerator Regenerate()
  {
    yield return (object) new WaitForSecondsRealtime(0.5f);
    if (this.isTextSet && (UnityEngine.Object) FollowersNameManager.Instance != (UnityEngine.Object) null)
      FollowersNameManager.Instance.Regenerate();
  }

  public void OnFollowerNamesEnabledChanged(bool value)
  {
    this.showAllNames = value;
    if (TwitchManager.FollowerNamesEnabled && this.IsTwitchFollower())
      return;
    if (value)
      this.Show();
    else
      this.Hide();
  }

  public void OnTwitchFollowerNamesEnabledChanged(bool value)
  {
    this.showTwitchNames = value;
    if (!this.IsTwitchFollower() || SettingsManager.Settings.Game.ShowFollowerNames)
      return;
    if (value)
      this.Show();
    else
      this.Hide();
  }

  public bool IsTwitchFollower()
  {
    return this.follower.Brain != null && !string.IsNullOrEmpty(this.follower.Brain.Info.ViewerID);
  }

  public void OnBrainAssigned()
  {
    this.follower.OnFollowerBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.SetText();
  }

  public void Update()
  {
    if ((double) (this._deltaUpdate -= Time.deltaTime) > 0.0)
      return;
    this._deltaUpdate = 0.5f + UnityEngine.Random.Range(0.0f, 0.5f);
    this.SetText();
    if (!GameManager.InMenu || !(this.transform.localScale == Vector3.one))
      return;
    this.Hide();
    this.StartCoroutine((IEnumerator) this.WaitForOutOfMenu());
  }

  public IEnumerator WaitForOutOfMenu()
  {
    while (GameManager.InMenu)
      yield return (object) new WaitForSeconds(0.1f);
    this.Show();
  }

  public void SetText()
  {
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null) || this.follower.Brain == null)
      return;
    string str = "";
    if (!string.IsNullOrEmpty(this.follower.Brain.Info.ViewerID) && (this.showAllNames || this.showTwitchNames))
      str = !string.IsNullOrEmpty(this.follower.Brain.Info.ViewerID) ? $"{(this.showTwitchNames ? "<sprite name=\"icon_TwitchIcon\">" : "")} {this.follower.Brain.Info.Name}" : "";
    else if (this.showAllNames)
      str = this.follower.Brain.Info.Name;
    if (this.nameText.text != str)
    {
      this.nameText.text = str;
      this.nameText.transform.localScale = Vector3.one;
      if (this.index <= -1)
      {
        FollowersNameManager.Nameplate nameplate = new FollowersNameManager.Nameplate();
        nameplate.SpriteRendererSource = this.textRenderer;
        nameplate.TMPTextSource = this.nameText;
        this.index = FollowersNameManager.Instance.GetNewIndex;
        FollowersNameManager.Instance.AddNameplate(nameplate);
      }
      else
        this.RegenerateLabels();
      this.isTextSet = true;
    }
    if (SettingsManager.Settings.Accessibility.DyslexicFont || !(LocalizationManager.CurrentLanguage == "English"))
      return;
    if (!string.IsNullOrEmpty(this.follower.Brain.Info.ViewerID) && this.showTwitchNames)
    {
      if (SettingsManager.Settings.Accessibility.RemoveTextStyling)
        this.nameText.fontSharedMaterial = this.twitchMaterialNoStyle;
      else
        this.nameText.fontSharedMaterial = this.twitchMaterial;
    }
    else
    {
      if (!this.showAllNames)
        return;
      if (SettingsManager.Settings.Accessibility.RemoveTextStyling)
        this.nameText.fontSharedMaterial = this.normalMaterialNoStyle;
      else
        this.nameText.fontSharedMaterial = this.normalMaterial;
    }
  }

  public void Show(bool animate = true)
  {
    if (this._shown || !this.showAllNames && (string.IsNullOrEmpty(this.follower.Brain.Info.ViewerID) || !this.showTwitchNames))
      return;
    this._shown = true;
    this.transform.DOKill();
    this.transform.localScale = Vector3.zero;
    if (animate)
      this.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.transform.localScale = Vector3.one;
  }

  public void Hide(bool animate = true)
  {
    if (!this._shown)
      return;
    this._shown = false;
    this.transform.DOKill();
    this.transform.localScale = Vector3.one;
    if (animate)
      this.transform.DOScale(Vector3.zero, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    else
      this.transform.localScale = Vector3.zero;
  }

  [CompilerGenerated]
  public void \u003COnFontChanged\u003Eb__18_0(TMP_FontAsset dyslexicFont)
  {
    this.nameText.font = LocalizationManager.DyslexicFontAsset;
    this.RegenerateLabels();
  }

  [CompilerGenerated]
  public void \u003COnFontChanged\u003Eb__18_1(TMP_FontAsset font)
  {
    this.regularFont = font;
    this.nameText.font = this.regularFont;
    this.RegenerateLabels();
  }

  [CompilerGenerated]
  public void \u003COnFontChanged\u003Eb__18_2(TMP_FontAsset font)
  {
    this.regularFont = font;
    this.nameText.font = this.regularFont;
    this.RegenerateLabels();
  }
}
