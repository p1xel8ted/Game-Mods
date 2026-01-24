// Decompiled with JetBrains decompiler
// Type: QuoteScreenController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class QuoteScreenController : BaseMonoBehaviour
{
  public static List<QuoteScreenController.QuoteTypes> QuoteType;
  public float fadeIn = 1f;
  public static System.Action SkipCallback;
  public static System.Action Callback;
  public TextMeshProUGUI Text;
  public static int CURRENT_QUOTE_INDEX;
  [SerializeField]
  public Animator _animator;
  [CompilerGenerated]
  public static string \u003CSFX\u003Ek__BackingField = "event:/dialogue/shop_tarot_clauneck/standard_clauneck";

  public static string SFX
  {
    get => QuoteScreenController.\u003CSFX\u003Ek__BackingField;
    set => QuoteScreenController.\u003CSFX\u003Ek__BackingField = value;
  }

  public void DoManualQuote()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoManualQuoteCoroutine());
  }

  public void DoManualQuote(QuoteScreenController.QuoteTypes quoteType)
  {
    this.StopAllCoroutines();
    QuoteScreenController.QuoteType = new List<QuoteScreenController.QuoteTypes>()
    {
      quoteType
    };
    this.SetQuote(true);
  }

  public IEnumerator DoManualQuoteCoroutine()
  {
    this._animator.Play("In");
    yield return (object) new WaitForSeconds(30f);
    this._animator.Play("Out");
  }

  public void Start()
  {
    AudioManager.Instance.PlayMusic("event:/music/menu/intro_narration_ambience");
    if (LocalizationManager.CurrentLanguage == "English" && QuoteScreenController.QuoteType != null)
    {
      int num = (int) QuoteScreenController.QuoteType[0];
    }
    this.SetQuote(false);
    if (!((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    MonoSingleton<UIManager>.Instance.SetCurrentCursor(0);
    MonoSingleton<UIManager>.Instance.ResetPreviousCursor();
  }

  public void SetQuote(bool FadeText, float waitTime = 7f)
  {
    if (FadeText)
    {
      Color TargetColor = this.Text.color;
      ShortcutExtensionsTMPText.DOColor(this.Text, Color.black, 2f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        this.Text.text = LocalizationManager.GetTranslation($"QUOTE/{QuoteScreenController.QuoteType[QuoteScreenController.CURRENT_QUOTE_INDEX]}");
        AudioManager.Instance.PlayOneShot(QuoteScreenController.SFX);
        this.StartCoroutine((IEnumerator) this.DoScreen(7f));
        ShortcutExtensionsTMPText.DOColor(this.Text, TargetColor, 2f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.StartCoroutine((IEnumerator) this.SkipScreen())));
      }));
    }
    else
    {
      AudioManager.Instance.PlayOneShotDelayed(QuoteScreenController.SFX, 1f);
      if (QuoteScreenController.QuoteType != null)
        this.Text.text = LocalizationManager.GetTranslation($"QUOTE/{QuoteScreenController.QuoteType[QuoteScreenController.CURRENT_QUOTE_INDEX]}");
      this.StartCoroutine((IEnumerator) this.DoScreen(7f));
      this.StartCoroutine((IEnumerator) this.SkipScreen());
      MMTransition.ResumePlay();
    }
  }

  public void OnDestroy()
  {
    QuoteScreenController.SFX = "event:/dialogue/shop_tarot_clauneck/standard_clauneck";
  }

  public static void Init(
    List<QuoteScreenController.QuoteTypes> QuoteType,
    System.Action SkipCallback,
    System.Action Callback)
  {
    QuoteScreenController.CURRENT_QUOTE_INDEX = 0;
    QuoteScreenController.QuoteType = QuoteType;
    QuoteScreenController.SkipCallback = SkipCallback;
    QuoteScreenController.Callback = Callback;
  }

  public IEnumerator SkipScreen()
  {
    QuoteScreenController screenController = this;
    yield return (object) new WaitForSeconds(0.5f);
    while (!Input.GetKeyDown(KeyCode.Escape))
      yield return (object) null;
    AudioManager.Instance.StopCurrentMusic();
    MMTransition.StopCurrentTransition();
    screenController.StopAllCoroutines();
    System.Action skipCallback = QuoteScreenController.SkipCallback;
    if (skipCallback != null)
      skipCallback();
  }

  public IEnumerator DoScreen(float waitTime)
  {
    QuoteScreenController coroutineSupport = this;
    yield return (object) new WaitForSeconds(0.5f);
    MMVibrate.Rumble(0.025f, 0.05f, 3f, (MonoBehaviour) coroutineSupport);
    yield return (object) new WaitForSeconds(waitTime - 0.5f);
    coroutineSupport.StopAllCoroutines();
    if (QuoteScreenController.QuoteType != null)
    {
      if (++QuoteScreenController.CURRENT_QUOTE_INDEX < QuoteScreenController.QuoteType.Count)
      {
        coroutineSupport.SetQuote(true);
      }
      else
      {
        AudioManager.Instance.StopCurrentMusic();
        System.Action callback = QuoteScreenController.Callback;
        if (callback != null)
          callback();
      }
    }
  }

  public void OnDisable()
  {
    AudioManager.Instance.StopCurrentMusic();
    if (!((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
  }

  [CompilerGenerated]
  public void \u003CSetQuote\u003Eb__16_1() => this.StartCoroutine((IEnumerator) this.SkipScreen());

  public enum QuoteTypes
  {
    IntroQuote,
    QuoteBoss1,
    QuoteBoss2,
    QuoteBoss3,
    QuoteBoss4,
    QuoteBoss5,
    IntroQuote2,
    QuoteBoss6,
    QuoteBoss7,
    QuoteBoss8,
    QuoteBoss9,
    Custom,
    DLC_End,
    DLC_Intro,
  }
}
