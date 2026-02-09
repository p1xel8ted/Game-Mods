// Decompiled with JetBrains decompiler
// Type: UIUnlockWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIUnlockWeapon : BaseMonoBehaviour
{
  public UI_NavigatorSimple UINav;
  public RectTransform CurrentHighlighted;
  public RectTransform IconContainer;
  public TextMeshProUGUI Title;
  public TextMeshProUGUI Description;
  public TextMeshProUGUI Lore;
  public Image Icon;
  [SerializeField]
  public TMP_Text damageText;
  [SerializeField]
  public TMP_Text speedText;
  public CanvasGroup CurrentHighlightedCanvasGroup;
  public CanvasGroup IconContainerCanvasGroup;
  public GameObject CurseIconPrefab;
  public UIUnlockCurseIcon CurrentCurseIcon;
  [Space]
  [SerializeField]
  public GameObject upgradeParent;
  [SerializeField]
  public TMP_Text upgradeNumberText;
  [SerializeField]
  public TMP_Text upgradeNextNumberText;
  [SerializeField]
  public TMP_Text upgradeDescriptionText;
  public GameObject HoldToUnlockObject;
  public Image HoldToUnlockRadialFill;
  [SerializeField]
  public ParticleSystem particlesystem;
  public Coroutine holdToUnlockRoutine;
  public Vector3 CurrentHighlightedStartingPosition;
  public Vector3 IconContainerStartingPosition;
  public System.Action continueCallback;
  public System.Action cancelCallback;
  public Tween shakeTween;
  public EventInstance holdLoop;

  public void OnEnable()
  {
    this.UINav.OnSelectDown += new System.Action(this.OnSelect);
    this.UINav.OnChangeSelection += new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelection);
    this.UINav.OnDefaultSetComplete += new System.Action(this.OnDefaultSetComplete);
    this.UINav.OnCancelDown += new System.Action(this.OnCancelDown);
  }

  public void OnDisable()
  {
    this.UINav.OnSelectDown -= new System.Action(this.OnSelect);
    this.UINav.OnChangeSelection -= new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelection);
    this.UINav.OnDefaultSetComplete -= new System.Action(this.OnDefaultSetComplete);
    this.UINav.OnCancelDown -= new System.Action(this.OnCancelDown);
    AudioManager.Instance.StopLoop(this.holdLoop);
    this.particlesystem.Stop();
    this.particlesystem.Clear();
  }

  public void Start()
  {
    this.HoldToUnlockObject.SetActive(false);
    this.CurrentHighlightedStartingPosition = this.CurrentHighlighted.localPosition;
    RectTransform currentHighlighted = this.CurrentHighlighted;
    currentHighlighted.localPosition = currentHighlighted.localPosition + Vector3.up * 100f;
    this.CurrentHighlighted.DOLocalMove(this.CurrentHighlightedStartingPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.CurrentHighlightedCanvasGroup.alpha = 0.0f;
    DOTween.To((DOGetter<float>) (() => this.CurrentHighlightedCanvasGroup.alpha), (DOSetter<float>) (x => this.CurrentHighlightedCanvasGroup.alpha = x), 1f, 1f);
    this.IconContainerStartingPosition = this.IconContainer.localPosition;
    RectTransform iconContainer = this.IconContainer;
    iconContainer.localPosition = iconContainer.localPosition + Vector3.down * 100f;
    this.IconContainer.DOLocalMove(this.IconContainerStartingPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.IconContainerCanvasGroup.alpha = 0.0f;
    DOTween.To((DOGetter<float>) (() => this.IconContainerCanvasGroup.alpha), (DOSetter<float>) (x => this.IconContainerCanvasGroup.alpha = x), 1f, 1f);
    this.Populate();
  }

  public void Init(System.Action continueCallback, System.Action cancelCallback)
  {
    this.continueCallback = continueCallback;
    this.cancelCallback = cancelCallback;
  }

  public void Populate()
  {
    this.UINav.enabled = true;
    this.UINav.selectable = (Selectable) null;
    this.UINav.startingItem = (Selectable) null;
    foreach (TarotCards.Card Type in new List<TarotCards.Card>()
    {
      TarotCards.Card.Sword,
      TarotCards.Card.Axe,
      TarotCards.Card.Dagger,
      TarotCards.Card.Hammer,
      TarotCards.Card.Gauntlet
    })
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CurseIconPrefab, (Transform) this.IconContainer);
      gameObject.SetActive(true);
      gameObject.GetComponent<UIUnlockCurseIcon>().Init(Type);
      if ((UnityEngine.Object) this.UINav.startingItem == (UnityEngine.Object) null)
        this.UINav.startingItem = gameObject.GetComponent<Selectable>();
    }
  }

  public IEnumerator PlayRoutine(float Delay, bool SetDefault)
  {
    yield return (object) new WaitForSecondsRealtime(Delay);
    this.UINav.canvasGroup.interactable = true;
    this.UINav.enabled = true;
    if (SetDefault)
      this.UINav.setDefault();
  }

  public void OnCancelDown()
  {
    if (this.holdToUnlockRoutine != null)
      return;
    this.GetComponent<CanvasGroup>().DOFade(0.0f, 0.25f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      System.Action cancelCallback = this.cancelCallback;
      if (cancelCallback != null)
        cancelCallback();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }));
  }

  public void OnDefaultSetComplete()
  {
    this.OnChangeSelection(this.UINav.selectable, (Selectable) null);
  }

  public void OnChangeSelection(Selectable NewSelectable, Selectable PrevSelectable)
  {
    if ((UnityEngine.Object) NewSelectable == (UnityEngine.Object) null)
      return;
    AudioManager.Instance.PlayOneShot("event:/ui/change_selection", PlayerFarming.Instance.gameObject);
    UIUnlockCurseIcon component = NewSelectable.GetComponent<UIUnlockCurseIcon>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      DOTween.To((DOGetter<float>) (() => this.CurrentHighlightedCanvasGroup.alpha), (DOSetter<float>) (x => this.CurrentHighlightedCanvasGroup.alpha = x), 0.0f, 0.3f);
    }
    else
    {
      this.Title.text = TarotCards.LocalisedName(component.Type);
      this.Lore.text = TarotCards.LocalisedLore(component.Type);
      this.Description.text = TarotCards.LocalisedDescription(component.Type, 0, PlayerFarming.Instance);
      this.Icon.sprite = component.Image.sprite;
      float num1 = 0.0f;
      float num2 = 0.0f;
      string damage = ScriptLocalization.UI_WeaponSelect.Damage;
      string speed = ScriptLocalization.UI_WeaponSelect.Speed;
      float num3 = Mathf.Round(num1 * 100f) / 100f;
      float num4 = Mathf.Round(num2 * 100f) / 100f;
      this.damageText.text = string.Format(damage + ": {0}{1}</color>", (object) "<color=#2DFF1C>", (object) num3);
      this.speedText.text = string.Format(speed + ": {0}{1}</color>", (object) "<color=#2DFF1C>", (object) num4);
      float num5 = Mathf.Round(num3 * 100f) / 100f;
      float num6 = Mathf.Round(num4 * 100f) / 100f;
      this.upgradeDescriptionText.text = "";
      this.upgradeDescriptionText.text = $"{LocalizationManager.GetTranslation("UpgradeSystem/WeaponDamageIncrease")} <color=#2DFF1C>{num5.ToString()}</color>\n";
      TMP_Text upgradeDescriptionText = this.upgradeDescriptionText;
      upgradeDescriptionText.text = $"{upgradeDescriptionText.text}{LocalizationManager.GetTranslation("UpgradeSystem/WeaponSpeedIncrease")} <color=#2DFF1C>{num6.ToString()}";
      this.CurrentHighlightedCanvasGroup.alpha = 0.5f;
      DOTween.To((DOGetter<float>) (() => this.CurrentHighlightedCanvasGroup.alpha), (DOSetter<float>) (x => this.CurrentHighlightedCanvasGroup.alpha = x), 1f, 0.3f);
    }
  }

  public void OnSelect() => this.UINav.selectable.GetComponent<UIUnlockCurseIcon>();

  public IEnumerator HoldToUnlock()
  {
    ParticleSystem.EmissionModule emission = this.particlesystem.emission with
    {
      rateOverTime = (ParticleSystem.MinMaxCurve) 0.0f
    };
    this.holdLoop = AudioManager.Instance.CreateLoop("event:/hearts_of_the_faithful/draw_power_loop", true);
    this.UINav.enabled = false;
    this.CurrentHighlighted.DOLocalMove(this.CurrentHighlighted.localPosition + Vector3.up * 100f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    DOTween.To((DOGetter<float>) (() => this.CurrentHighlightedCanvasGroup.alpha), (DOSetter<float>) (x => this.CurrentHighlightedCanvasGroup.alpha = x), 0.0f, 0.5f);
    this.CurrentCurseIcon.costParent.SetActive(false);
    this.IconContainer.DOLocalMove(this.IconContainer.localPosition + Vector3.down * 100f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    DOTween.To((DOGetter<float>) (() => this.IconContainerCanvasGroup.alpha), (DOSetter<float>) (x => this.IconContainerCanvasGroup.alpha = x), 0.0f, 0.5f);
    this.CurrentCurseIcon.upgradeLevel.gameObject.SetActive(false);
    this.CurrentCurseIcon.upgradeLevelBackground.gameObject.SetActive(false);
    this.CurrentCurseIcon.transform.DOLocalMove(Vector3.zero, 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSecondsRealtime(0.75f);
    this.CurrentCurseIcon.transform.DOScale(Vector3.one * 2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    this.HoldToUnlockObject.SetActive(true);
    this.HoldToUnlockObject.transform.localPosition = new Vector3(0.0f, -250f);
    this.HoldToUnlockObject.transform.DOLocalMove(new Vector3(0.0f, -200f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    CanvasGroup c = this.HoldToUnlockObject.GetComponent<CanvasGroup>();
    c.alpha = 0.0f;
    DOTween.To((DOGetter<float>) (() => c.alpha), (DOSetter<float>) (x => c.alpha = x), 1f, 0.5f);
    this.HoldToUnlockRadialFill.fillAmount = 0.0f;
    float Progress = 0.0f;
    float Duration = 3f;
    while ((double) Progress < (double) Duration)
    {
      emission.rateOverTime = (ParticleSystem.MinMaxCurve) (float) (5.0 + (double) Progress * 9.0);
      int num = (int) this.holdLoop.setParameterByName("power", Progress / Duration);
      if (InputManager.UI.GetAcceptButtonHeld())
      {
        Progress = Mathf.Clamp(Progress + Time.unscaledDeltaTime, 0.0f, Duration);
        if (!this.particlesystem.isPlaying)
          this.particlesystem.Play();
      }
      else
        Progress = Mathf.Clamp(Progress - Time.unscaledDeltaTime * 5f, 0.0f, Duration);
      this.CurrentCurseIcon.transform.localScale = Vector3.one * 2f * (float) (1.0 + (double) Progress / 20.0);
      this.CurrentCurseIcon.transform.localPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * Progress * 2f);
      this.HoldToUnlockRadialFill.fillAmount = Progress / Duration;
      if ((double) Progress <= 0.0)
        this.particlesystem.Stop();
      if (InputManager.UI.GetCancelButtonDown())
      {
        emission.rateOverTime = (ParticleSystem.MinMaxCurve) 0.0f;
        AudioManager.Instance.StopLoop(this.holdLoop);
        this.CancelReset();
        this.holdToUnlockRoutine = (Coroutine) null;
        yield break;
      }
      yield return (object) null;
    }
    this.CurrentCurseIcon.SetUnlocked();
    emission.rateOverTime = (ParticleSystem.MinMaxCurve) 0.0f;
    RumbleManager.Instance.Rumble();
    AudioManager.Instance.StopLoop(this.holdLoop);
    AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/draw_power_end", PlayerFarming.Instance.gameObject);
    this.HoldToUnlockObject.SetActive(false);
    CameraManager.instance.ShakeCameraForDuration(1.2f, 1.5f, 0.3f);
    this.CurrentCurseIcon.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.CurrentCurseIcon.WhiteFlash.color = new Color(1f, 1f, 1f, 0.6f);
    DOTweenModuleUI.DOColor(this.CurrentCurseIcon.WhiteFlash, new Color(1f, 1f, 1f, 0.0f), 0.3f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    GameManager.GetInstance().HitStop();
    yield return (object) new WaitForSecondsRealtime(0.4f);
    this.CurrentCurseIcon.SelectedIcon.enabled = true;
    this.CurrentCurseIcon.SelectedIcon.color = Color.white;
    DOTweenModuleUI.DOColor(this.CurrentCurseIcon.SelectedIcon, new Color(1f, 1f, 1f, 0.0f), 0.3f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.CurrentCurseIcon.SelectedIcon.rectTransform.localScale = Vector3.one;
    this.CurrentCurseIcon.SelectedIcon.rectTransform.DOScale(new Vector3(1.5f, 1.5f), 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.CurrentCurseIcon.WhiteFlash.color = Color.white;
    DOTweenModuleUI.DOColor(this.CurrentCurseIcon.WhiteFlash, new Color(1f, 1f, 1f, 0.0f), 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.CurrentCurseIcon.transform.DOShakePosition(0.5f, 20f).SetUpdate<Tweener>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    if (false)
      this.UpgradeCurse();
    else
      this.UnlockCurse();
    this.CurrentCurseIcon.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.particlesystem.Stop();
      this.particlesystem.Clear();
      this.holdToUnlockRoutine = (Coroutine) null;
      System.Action continueCallback = this.continueCallback;
      if (continueCallback != null)
        continueCallback();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }));
  }

  public void UpgradeCurse()
  {
  }

  public void CancelReset()
  {
    CanvasGroup c = this.HoldToUnlockObject.GetComponent<CanvasGroup>();
    c.alpha = 1f;
    DOTween.To((DOGetter<float>) (() => c.alpha), (DOSetter<float>) (x => c.alpha = x), 0.0f, 0.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.HoldToUnlockObject.SetActive(false)));
    this.CurrentCurseIcon.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentCurseIcon.gameObject);
      this.HoldToUnlockObject.SetActive(false);
      this.CurrentHighlighted.localPosition = this.CurrentHighlightedStartingPosition;
      RectTransform currentHighlighted = this.CurrentHighlighted;
      currentHighlighted.localPosition = currentHighlighted.localPosition + Vector3.up * 100f;
      this.CurrentHighlighted.DOLocalMove(this.CurrentHighlightedStartingPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.CurrentHighlightedCanvasGroup.alpha = 0.0f;
      DOTween.To((DOGetter<float>) (() => this.CurrentHighlightedCanvasGroup.alpha), (DOSetter<float>) (x => this.CurrentHighlightedCanvasGroup.alpha = x), 1f, 1f);
      this.IconContainer.localPosition = this.IconContainerStartingPosition;
      RectTransform iconContainer = this.IconContainer;
      iconContainer.localPosition = iconContainer.localPosition + Vector3.down * 100f;
      this.IconContainer.DOLocalMove(this.IconContainerStartingPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.IconContainerCanvasGroup.alpha = 0.0f;
      DOTween.To((DOGetter<float>) (() => this.IconContainerCanvasGroup.alpha), (DOSetter<float>) (x => this.IconContainerCanvasGroup.alpha = x), 1f, 1f);
      this.CurrentCurseIcon.costParent.SetActive(true);
      this.StartCoroutine((IEnumerator) this.PlayRoutine(0.5f, false));
    }));
  }

  public void UnlockCurse()
  {
  }

  [CompilerGenerated]
  public float \u003CStart\u003Eb__28_0() => this.CurrentHighlightedCanvasGroup.alpha;

  [CompilerGenerated]
  public void \u003CStart\u003Eb__28_1(float x) => this.CurrentHighlightedCanvasGroup.alpha = x;

  [CompilerGenerated]
  public float \u003CStart\u003Eb__28_2() => this.IconContainerCanvasGroup.alpha;

  [CompilerGenerated]
  public void \u003CStart\u003Eb__28_3(float x) => this.IconContainerCanvasGroup.alpha = x;

  [CompilerGenerated]
  public void \u003COnCancelDown\u003Eb__32_0()
  {
    System.Action cancelCallback = this.cancelCallback;
    if (cancelCallback != null)
      cancelCallback();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public float \u003COnChangeSelection\u003Eb__34_0() => this.CurrentHighlightedCanvasGroup.alpha;

  [CompilerGenerated]
  public void \u003COnChangeSelection\u003Eb__34_1(float x)
  {
    this.CurrentHighlightedCanvasGroup.alpha = x;
  }

  [CompilerGenerated]
  public float \u003COnChangeSelection\u003Eb__34_2() => this.CurrentHighlightedCanvasGroup.alpha;

  [CompilerGenerated]
  public void \u003COnChangeSelection\u003Eb__34_3(float x)
  {
    this.CurrentHighlightedCanvasGroup.alpha = x;
  }
}
