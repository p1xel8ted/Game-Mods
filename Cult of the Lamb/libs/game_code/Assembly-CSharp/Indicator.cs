// Decompiled with JetBrains decompiler
// Type: Indicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using Rewired;
using src.UI.Prompts;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Indicator : MonoBehaviour
{
  [HideInInspector]
  public bool taken;
  public Image BG;
  public Image ContainerImage;
  public RectTransform CoopOffsetTransform;
  public TextMeshProUGUI text;
  public TextMeshProUGUI SecondaryText;
  public TextMeshProUGUI Thirdtext;
  public TextMeshProUGUI Fourthtext;
  public float TargetWidth;
  public float TargetWidthSpeed;
  public float width;
  public float height;
  public CanvasGroup canvasGroup;
  public bool closing;
  public RectTransform ControlPrompt;
  public RectTransform ControlPromptContainer;
  public RectTransform SecondaryControlPrompt;
  public RectTransform ThirdControlPrompt;
  public RectTransform FourthControlPrompt;
  public float Progress;
  [SerializeField]
  public RadialProgress _radialProgress;
  public float Margin = 120f;
  public float ControlPromptSpacing = 15f;
  public float PromptDist;
  public RectTransform CenterObject;
  public bool HoldToInteract;
  public bool Interactable = true;
  public bool HasSecondaryInteraction;
  public bool SecondaryInteractable = true;
  public bool HasThirdInteraction;
  public bool ThirdInteractable = true;
  public bool FourthInteractable = true;
  public bool HasFourthInteraction;
  public GameObject ControlPromptUI;
  public MMControlPrompt primaryControlPrompt;
  public MMControlPrompt secondaryControlPrompt;
  public MMControlPrompt thirdControlPrompt;
  public MMControlPrompt fourthControlPrompt;
  [ActionIdProperty(typeof (RewiredConsts.Action))]
  public int Action = 9;
  public GameObject TopInfoContainer;
  public TMP_Text TopInfoText;
  public SimpleSFX sfx;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public CanvasGroup _controlPromptContainerCanvasGroup;
  [SerializeField]
  public LayoutElement _controlPromptLayoutElement;
  public Vector2 _cachedPosition;
  public Vector2 _cachedContainerPosition;
  [HideInInspector]
  public PlayerFarming playerFarming;
  [CompilerGenerated]
  public bool \u003CDontDestroy\u003Ek__BackingField;
  public CanvasGroup _topInfoCanvasGroup;
  public Vector3 _lastPosition;
  public Resolution _lastResolution;
  public const float BOSS_ROOM_Y_MOD = 75f;
  public Vector2 Easing = new Vector2(0.2f, 0.55f);
  public float ClosingAlphaSpeed = 5f;
  public Vector3 WorldPosition;
  public Vector2 Shake;
  [HideInInspector]
  public float ShakeSpeed;
  public bool UpdatePosition = true;
  public bool WorldPositionShake = true;
  public float ShakeAmount = 25f;
  public float bounce = 0.4f;
  public float easing = 0.8f;
  public bool hidden;
  public float cacheAlpha;
  public bool PlacementObjectEnabled;
  public Vector3 forceShownPosition = Vector3.zero;
  [CompilerGenerated]
  public bool \u003CForceShown\u003Ek__BackingField;

  public RectTransform RectTransform => this._rectTransform;

  public Vector2 CachedPosition => this._cachedPosition;

  public bool DontDestroy
  {
    get => this.\u003CDontDestroy\u003Ek__BackingField;
    set => this.\u003CDontDestroy\u003Ek__BackingField = value;
  }

  public bool IsActive => (double) this.transform.position.x <= 9000.0;

  public void Start()
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null && !this.DontDestroy)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    if ((bool) (UnityEngine.Object) this.TopInfoContainer)
    {
      CanvasGroup component;
      if (this.TopInfoContainer.TryGetComponent<CanvasGroup>(out component))
      {
        this._topInfoCanvasGroup = component;
        this._topInfoCanvasGroup.alpha = 0.0f;
      }
      else
      {
        this._topInfoCanvasGroup = this.TopInfoContainer.AddComponent<CanvasGroup>();
        this._topInfoCanvasGroup.alpha = 0.0f;
      }
      this.TopInfoContainer.gameObject.SetActive(true);
    }
    this._lastPosition = this.transform.position;
    this._lastResolution = Screen.currentResolution;
    if ((UnityEngine.Object) this.sfx == (UnityEngine.Object) null)
      this.sfx = this.GetComponent<SimpleSFX>();
    this._cachedPosition = this._rectTransform.anchoredPosition;
    this._cachedContainerPosition = this._container.anchoredPosition;
    this.HideTopInfo();
  }

  public void OnEnable()
  {
    this.closing = false;
    this.canvasGroup.alpha = 0.0f;
    this.TargetWidth = this.TargetWidthSpeed = 0.0f;
    Singleton<AccessibilityManager>.Instance.OnHoldActionToggleChanged += new System.Action<bool>(this.OnHoldActionToggleChanged);
    this.ControlPrompt.GetComponent<MMControlPrompt>().playerFarming = this.playerFarming;
    if ((bool) (UnityEngine.Object) this.playerFarming)
    {
      this.primaryControlPrompt.playerFarming = this.playerFarming;
      this.secondaryControlPrompt.playerFarming = this.playerFarming;
      this.thirdControlPrompt.playerFarming = this.playerFarming;
      this.fourthControlPrompt.playerFarming = this.playerFarming;
    }
    this.Reset();
    this.HideTopInfo();
  }

  public void OnDisable()
  {
    Singleton<AccessibilityManager>.Instance.OnHoldActionToggleChanged -= new System.Action<bool>(this.OnHoldActionToggleChanged);
    this.forceShownPosition = Vector3.zero;
  }

  public void OnHoldActionToggleChanged(bool value)
  {
    this._radialProgress.gameObject.SetActive(value);
  }

  public void Reset()
  {
    this.closing = false;
    this.ContainerImage.enabled = true;
    if (this.Interactable && this.text.text != "" && this.text.text != " ")
    {
      this._controlPromptContainerCanvasGroup.alpha = 1f;
      this._controlPromptLayoutElement.minWidth = 75f;
      this._controlPromptLayoutElement.preferredWidth = 75f;
      if (this.HoldToInteract && SettingsManager.Settings.Accessibility.HoldActions)
      {
        this.Progress = this._radialProgress.Progress = 0.0f;
        this._radialProgress.gameObject.SetActive(true);
      }
      else
      {
        this._radialProgress.gameObject.SetActive(false);
        this.Progress = this._radialProgress.Progress = 0.0f;
      }
    }
    else if ((double) this._controlPromptContainerCanvasGroup.alpha == 1.0)
    {
      this._controlPromptContainerCanvasGroup.alpha = 0.0f;
      this._controlPromptLayoutElement.minWidth = 0.0f;
      this._controlPromptLayoutElement.preferredWidth = 0.0f;
      this._radialProgress.gameObject.SetActive(false);
    }
    if (this.SecondaryInteractable && !string.IsNullOrEmpty(this.SecondaryText.text))
      this.SecondaryControlPrompt.gameObject.SetActive(this.HasSecondaryInteraction);
    else if (this.SecondaryControlPrompt.gameObject.activeSelf || !string.IsNullOrEmpty(this.SecondaryText.text))
    {
      this.SecondaryControlPrompt.gameObject.SetActive(false);
      this.SecondaryText.text = "";
    }
    if (this.ThirdInteractable && (UnityEngine.Object) this.Thirdtext != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.Thirdtext.text))
      this.ThirdControlPrompt.gameObject.SetActive(this.HasThirdInteraction);
    else if (this.ThirdControlPrompt.gameObject.activeSelf || !string.IsNullOrEmpty(this.Thirdtext.text))
    {
      this.ThirdControlPrompt.gameObject.SetActive(false);
      this.Thirdtext.text = "";
    }
    if (this.FourthInteractable && (UnityEngine.Object) this.Fourthtext != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.Fourthtext.text))
      this.FourthControlPrompt.gameObject.SetActive(this.HasFourthInteraction);
    else if (this.FourthControlPrompt.gameObject.activeSelf || !string.IsNullOrEmpty(this.Fourthtext.text))
    {
      this.FourthControlPrompt.gameObject.SetActive(false);
      this.Fourthtext.text = "";
    }
    if (!string.IsNullOrEmpty(this.text.text) || !string.IsNullOrEmpty(this.SecondaryText.text) || !string.IsNullOrEmpty(this.Thirdtext.text) || !string.IsNullOrEmpty(this.Fourthtext.text))
      return;
    this.ContainerImage.enabled = false;
  }

  public void ShowTopInfo(string text)
  {
    if (!(bool) (UnityEngine.Object) this.TopInfoContainer || !(bool) (UnityEngine.Object) this._topInfoCanvasGroup)
      return;
    this._topInfoCanvasGroup.alpha = 1f;
    this.TopInfoText.text = text;
  }

  public void HideTopInfo()
  {
    if (!(bool) (UnityEngine.Object) this.TopInfoContainer || !(bool) (UnityEngine.Object) this._topInfoCanvasGroup)
      return;
    this._topInfoCanvasGroup.alpha = 0.0f;
  }

  public void Deactivate()
  {
    if (this.closing)
      return;
    this.closing = true;
  }

  public void Activate()
  {
    this.HideTopInfo();
    if (!(bool) (UnityEngine.Object) this.playerFarming.interactor.CurrentInteraction)
      return;
    this.playerFarming.interactor.CurrentInteraction.HasChanged = true;
  }

  public void SetSize()
  {
    this.PromptDist = (float) ((double) this.text.preferredWidth / 2.0 + (double) this.ControlPrompt.rect.width / 2.0) + this.ControlPromptSpacing;
    this.height = this.text.preferredHeight + 20f;
    this.BG.rectTransform.sizeDelta = new Vector2(this.width = this.text.preferredWidth + this.ControlPrompt.rect.width + this.Margin, this.height);
  }

  public void SetPosition(Vector3 WorldPosition) => this.WorldPosition = WorldPosition;

  public void LateUpdate()
  {
    if (!this.UpdatePosition)
      return;
    this._container.anchoredPosition = this._cachedContainerPosition + this.Shake;
  }

  public void PlayShake()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.playerFarming.indicator.gameObject);
    this.playerFarming.indicator.ShakeSpeed = this.playerFarming.indicator.ShakeAmount * ((double) UnityEngine.Random.value < 0.5 ? -1f : 1f);
  }

  public void SetGameObjectActive(bool val)
  {
    if (val)
    {
      this.gameObject.SetActive(val);
      this.SetActivePosition();
      UIPromptBase.UpdateAllPromptPositions();
    }
    else
      this.transform.position = new Vector3(9999f, 9999f, 9999f);
  }

  public bool ForceShown
  {
    get => this.\u003CForceShown\u003Ek__BackingField;
    set => this.\u003CForceShown\u003Ek__BackingField = value;
  }

  public void Update()
  {
    if ((UnityEngine.Object) PlacementObject.Instance != (UnityEngine.Object) null)
    {
      this.PlacementObjectEnabled = true;
      this.hidden = false;
      this.canvasGroup.alpha = 1f;
    }
    else
    {
      if (this.ForceShown)
      {
        this.hidden = false;
        this.closing = false;
        this.canvasGroup.alpha = 1f;
        this.transform.localPosition = this.forceShownPosition;
      }
      else if (this.PlacementObjectEnabled)
      {
        this.Deactivate();
        this.PlacementObjectEnabled = false;
      }
      if ((double) Time.timeScale == 0.0)
      {
        this.hidden = true;
        this.cacheAlpha = this.canvasGroup.alpha;
        this.canvasGroup.alpha = 0.0f;
      }
      if (this.hidden)
      {
        if ((double) Time.timeScale <= 0.0)
          return;
        this.hidden = false;
        this.canvasGroup.alpha = this.cacheAlpha;
      }
      else
      {
        bool flag = false;
        if (!this.closing && (!flag || (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && (UnityEngine.Object) this.playerFarming.interactor != (UnityEngine.Object) null && (UnityEngine.Object) this.playerFarming.interactor.CurrentInteraction != (UnityEngine.Object) null))
        {
          if ((double) this.canvasGroup.alpha < 1.0)
            this.canvasGroup.alpha += 20f * Time.deltaTime;
          this.PromptDist = (float) ((double) this.text.preferredWidth / 2.0 + (double) this.ControlPrompt.rect.width / 2.0) + this.ControlPromptSpacing;
          if (this._radialProgress.gameObject.activeSelf)
            this._radialProgress.Progress = this.Progress;
          this.width = this.text.preferredWidth + this.ControlPrompt.rect.width + this.Margin;
          this.TargetWidthSpeed += (this.width - this.TargetWidth) * this.Easing.x;
          this.TargetWidth += (this.TargetWidthSpeed *= this.Easing.y);
          this.height = this.text.preferredHeight + 20f;
          this.BG.rectTransform.sizeDelta = new Vector2(this.TargetWidth, this.height);
        }
        else
        {
          if ((double) this.canvasGroup.alpha > 0.0)
            this.canvasGroup.alpha -= this.ClosingAlphaSpeed * Time.deltaTime;
          if ((double) this.canvasGroup.alpha > 0.0)
            return;
          this.text.text = "";
          this.transform.position = new Vector3(9999f, 9999f, 9999f);
        }
      }
    }
  }

  public void SetForceShownPosition(Vector3 position) => this.forceShownPosition = position;

  public void SetActivePosition()
  {
    if (this._lastResolution.height == Screen.currentResolution.height && this._lastResolution.width == Screen.width)
    {
      this.transform.position = this._lastPosition;
    }
    else
    {
      this.transform.position = this._lastPosition;
      this._lastResolution = Screen.currentResolution;
    }
    this.RectTransform.offsetMin = new Vector2(0.0f, this.RectTransform.offsetMin.y);
    this.RectTransform.offsetMax = new Vector2(0.0f, this.RectTransform.offsetMax.y);
    if (!((UnityEngine.Object) UIBossHUD.Instance != (UnityEngine.Object) null))
      return;
    RectTransform rectTransform = this._rectTransform;
    rectTransform.localPosition = rectTransform.localPosition + Vector3.up * 75f;
  }

  public void FixedUpdate()
  {
    this.ShakeSpeed += (0.0f - this.Shake.x) * this.bounce;
    this.Shake += new Vector2(this.ShakeSpeed *= this.easing, 0.0f);
  }
}
