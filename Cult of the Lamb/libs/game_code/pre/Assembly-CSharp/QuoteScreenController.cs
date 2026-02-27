// Decompiled with JetBrains decompiler
// Type: QuoteScreenController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class QuoteScreenController : BaseMonoBehaviour
{
  private static List<QuoteScreenController.QuoteTypes> QuoteType;
  public float fadeIn = 1f;
  private static System.Action SkipCallback;
  private static System.Action Callback;
  public TextMeshProUGUI Text;
  private static int CURRENT_QUOTE_INDEX;

  private void Start()
  {
    AudioManager.Instance.PlayMusic("event:/music/menu/intro_narration_ambience");
    if (LocalizationManager.CurrentLanguage == "English")
    {
      int num = (int) QuoteScreenController.QuoteType[0];
    }
    this.SetQuote(false);
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
  }

  private void SetQuote(bool FadeText)
  {
    if (FadeText)
    {
      Color TargetColor = this.Text.color;
      ShortcutExtensionsTMPText.DOColor(this.Text, Color.black, 2f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        this.Text.text = LocalizationManager.GetTranslation($"QUOTE/{QuoteScreenController.QuoteType[QuoteScreenController.CURRENT_QUOTE_INDEX]}");
        AudioManager.Instance.PlayOneShot("event:/dialogue/shop_tarot_clauneck/standard_clauneck");
        this.StartCoroutine((IEnumerator) this.DoScreen(7f));
        ShortcutExtensionsTMPText.DOColor(this.Text, TargetColor, 2f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.StartCoroutine((IEnumerator) this.SkipScreen())));
      }));
    }
    else
    {
      AudioManager.Instance.PlayOneShotDelayed("event:/dialogue/shop_tarot_clauneck/standard_clauneck", 1f);
      this.Text.text = LocalizationManager.GetTranslation($"QUOTE/{QuoteScreenController.QuoteType[QuoteScreenController.CURRENT_QUOTE_INDEX]}");
      this.StartCoroutine((IEnumerator) this.DoScreen(7f));
      this.StartCoroutine((IEnumerator) this.SkipScreen());
      MMTransition.ResumePlay();
    }
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

  private IEnumerator SkipScreen()
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

  private IEnumerator DoScreen(float waitTime)
  {
    QuoteScreenController coroutineSupport = this;
    yield return (object) new WaitForSeconds(0.5f);
    MMVibrate.Rumble(0.025f, 0.05f, 3f, (MonoBehaviour) coroutineSupport);
    yield return (object) new WaitForSeconds(waitTime - 0.5f);
    coroutineSupport.StopAllCoroutines();
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

  private void OnDisable()
  {
    AudioManager.Instance.StopCurrentMusic();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
  }

  public enum QuoteTypes
  {
    IntroQuote,
    QuoteBoss1,
    QuoteBoss2,
    QuoteBoss3,
    QuoteBoss4,
    QuoteBoss5,
    IntroQuote2,
  }
}
