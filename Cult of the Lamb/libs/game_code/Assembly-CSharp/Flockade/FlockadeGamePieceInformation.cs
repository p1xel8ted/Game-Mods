// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePieceInformation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (CanvasGroup), typeof (HorizontalLayoutGroup), typeof (RectTransform))]
public class FlockadeGamePieceInformation : MonoBehaviour
{
  public const string _KIND_DESCRIPTION_PARAMETER_NAME = "KIND";
  [SerializeField]
  public Image _image;
  [SerializeField]
  public Image _mainBackground;
  [SerializeField]
  public Image _schematicsBackground;
  [SerializeField]
  public Localize _title;
  [SerializeField]
  public TextMeshProUGUI _titleText;
  [SerializeField]
  public Localize _subtitle;
  [SerializeField]
  public TextMeshProUGUI _subtitleText;
  [SerializeField]
  public Localize _body;
  [SerializeField]
  public RectTransform _bodyBackground;
  [SerializeField]
  public LocalizationParamsManager _bodyParameters;
  [SerializeField]
  public TextMeshProUGUI _bodyText;
  [SerializeField]
  public FlockadeBlessing _blessing;
  [SerializeField]
  public FlockadeBottomContainer _container;
  [SerializeField]
  public FlockadeGameBoardSide _closestMargin;
  public RectTransform _blessingRectTransform;
  public CanvasGroup _canvasGroup;
  public FlockadeGamePiece _currentGamePiece;
  public DG.Tweening.Sequence _entryOrExit;
  public Vector2 _flippedBodyTextOffsetMax;
  public Vector2 _flippedBodyTextOffsetMin;
  public HorizontalLayoutGroup _layoutGroup;
  public object _lockedBy;
  public Vector2 _originAnchoredPosition;
  public Vector2 _originBlessingAnchoredPosition;
  public Vector3 _originBodyBackgroundScale;
  public Vector2 _originBodyTextOffsetMax;
  public Vector2 _originBodyTextOffsetMin;
  public Sprite _originMainBackgroundSprite;
  public Sprite _originSchematicsBackgroundSprite;
  public RectTransform _rectTransform;
  public float _width;

  public bool Locked => this._lockedBy != null;

  public virtual void Awake()
  {
    this._canvasGroup = this.GetComponent<CanvasGroup>();
    this._layoutGroup = this.GetComponent<HorizontalLayoutGroup>();
    this._rectTransform = this.GetComponent<RectTransform>();
    this._blessingRectTransform = this._blessing.GetComponent<RectTransform>();
    this._originAnchoredPosition = this._rectTransform.anchoredPosition;
    this._originBlessingAnchoredPosition = this._blessingRectTransform.anchoredPosition;
    this._originBodyBackgroundScale = this._rectTransform.localScale;
    this._originBodyTextOffsetMax = this._bodyText.rectTransform.offsetMax;
    this._originBodyTextOffsetMin = this._bodyText.rectTransform.offsetMin;
    this._originMainBackgroundSprite = this._mainBackground.sprite;
    this._originSchematicsBackgroundSprite = this._schematicsBackground.sprite;
    this._flippedBodyTextOffsetMax = new Vector2(-this._originBodyTextOffsetMin.x, this._originBodyTextOffsetMax.y);
    this._flippedBodyTextOffsetMin = new Vector2(-this._originBodyTextOffsetMax.x, this._originBodyTextOffsetMin.y);
  }

  public virtual IEnumerator Start()
  {
    yield return (object) new WaitForEndOfFrame();
    this._width = this._rectTransform.rect.width;
    this.Exit().Complete(true);
  }

  public virtual void OnEnable()
  {
    this._container.ConfigurationChanged += new Action<bool>(this.Reconfigure);
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateParametersInBody);
  }

  public virtual void OnDisable()
  {
    this._container.ConfigurationChanged -= new Action<bool>(this.Reconfigure);
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateParametersInBody);
  }

  public void Reconfigure(bool isFlipped)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    FlockadeGamePieceInformation.\u003C\u003Ec__DisplayClass38_0 cDisplayClass380 = new FlockadeGamePieceInformation.\u003C\u003Ec__DisplayClass38_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass380.isFlipped = isFlipped;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass380.\u003C\u003E4__this = this;
    if (this._entryOrExit != null && this._entryOrExit.IsActive() && !this._entryOrExit.IsComplete())
    {
      // ISSUE: reference to a compiler-generated method
      this._entryOrExit.OnComplete<DG.Tweening.Sequence>(new TweenCallback(cDisplayClass380.\u003CReconfigure\u003Eg__Reconfigure\u007C0));
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      cDisplayClass380.\u003CReconfigure\u003Eg__Reconfigure\u007C0();
    }
  }

  public DG.Tweening.Sequence Enter()
  {
    DG.Tweening.Sequence entryOrExit = this._entryOrExit;
    if (entryOrExit != null)
      entryOrExit.Kill();
    this._entryOrExit = DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(1f, 0.5833333f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad)).Join((Tween) this._rectTransform.DOAnchorPosX(this._originAnchoredPosition.x, 0.5833333f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuad));
    return this._entryOrExit;
  }

  public DG.Tweening.Sequence Exit()
  {
    float endValue = this._originAnchoredPosition.x + (float) ((this._container.IsFlipped ? -1.0 : 1.0) * ((double) this._width / 2.0 + (double) this._container.GetMargin(this._closestMargin)));
    DG.Tweening.Sequence entryOrExit = this._entryOrExit;
    if (entryOrExit != null)
      entryOrExit.Kill();
    this._entryOrExit = DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(0.0f, 0.5833333f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuad)).Join((Tween) this._rectTransform.DOAnchorPosX(endValue, 0.5833333f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InQuad));
    return this._entryOrExit;
  }

  public bool Lock(object requester)
  {
    if (this._lockedBy != null)
      return false;
    this._lockedBy = requester;
    return true;
  }

  public bool Unlock(object requester)
  {
    if (this._lockedBy == null || this._lockedBy != requester)
      return false;
    this._lockedBy = (object) null;
    return true;
  }

  public void Set(FlockadeGamePiece gamePiece)
  {
    if (this.Locked)
      return;
    if ((bool) (UnityEngine.Object) this._currentGamePiece)
    {
      this._currentGamePiece.Changed -= new Action<IFlockadeGamePiece.State?>(this.Set);
      this._blessing.Bind((FlockadeGamePiece) null);
    }
    this._currentGamePiece = gamePiece;
    if ((bool) (UnityEngine.Object) this._currentGamePiece)
    {
      this._currentGamePiece.Changed += new Action<IFlockadeGamePiece.State?>(this.Set);
      this._blessing.Bind(this._currentGamePiece);
      this.Set(new IFlockadeGamePiece.State?(this._currentGamePiece.Get()));
    }
    else
      this.Set(new IFlockadeGamePiece.State?());
  }

  public void Set(IFlockadeGamePiece.State? gamePiece)
  {
    if (this.Locked)
      return;
    IFlockadeGamePiece.State state;
    string str1;
    if (gamePiece.HasValue)
    {
      state = gamePiece.Value;
      if ((bool) (UnityEngine.Object) state.Configuration)
      {
        state = gamePiece.Value;
        str1 = state.Configuration.Name;
        goto label_5;
      }
    }
    str1 = (string) null;
label_5:
    string str2 = str1;
    if (string.IsNullOrEmpty(str2))
    {
      this._title.Term = (string) null;
      this._titleText.SetText(string.Empty);
    }
    else
      this._title.Term = str2;
    string str3;
    if (gamePiece.HasValue)
    {
      state = gamePiece.Value;
      if ((bool) (UnityEngine.Object) state.Configuration)
      {
        state = gamePiece.Value;
        str3 = state.Configuration.Description;
        goto label_12;
      }
    }
    str3 = (string) null;
label_12:
    string str4 = str3;
    if (string.IsNullOrEmpty(str4))
    {
      this._subtitle.Term = (string) null;
      this._subtitleText.SetText(string.Empty);
    }
    else
      this._subtitle.Term = str4;
    string blessingDescription = FlockadeGamePieceInformation.GetBlessingDescription(gamePiece);
    if (string.IsNullOrEmpty(blessingDescription))
    {
      this._body.Term = (string) null;
      this._bodyText.SetText(string.Empty);
    }
    else
    {
      this._body.Term = blessingDescription;
      this.UpdateParametersInBody();
    }
    Sprite sprite1;
    if (gamePiece.HasValue)
    {
      state = gamePiece.Value;
      if ((bool) (UnityEngine.Object) state.Configuration)
      {
        state = gamePiece.Value;
        sprite1 = state.Configuration.BaseConfiguration.Schematics;
        goto label_22;
      }
    }
    sprite1 = (Sprite) null;
label_22:
    Sprite sprite2 = sprite1;
    this._image.sprite = sprite2;
    this._image.color = (bool) (UnityEngine.Object) sprite2 ? Color.white : Color.clear;
  }

  public static string GetBlessingDescription(IFlockadeGamePiece.State? gamePiece)
  {
    if (!gamePiece.HasValue || !(bool) (UnityEngine.Object) gamePiece.Value.Configuration)
      return (string) null;
    if (gamePiece.Value.Blessing.Nullified)
      return ScriptLocalization.UI_Flockade_Blessing_Effects.Cursed;
    return !(bool) (UnityEngine.Object) gamePiece.Value.Configuration.BlessingConfiguration ? gamePiece.Value.Configuration.BaseConfiguration.KindDescription : gamePiece.Value.Configuration.BlessingConfiguration.Description;
  }

  public void UpdateParametersInBody()
  {
    if (string.IsNullOrEmpty(FlockadeGamePieceInformation.GetBlessingDescription((bool) (UnityEngine.Object) this._currentGamePiece ? new IFlockadeGamePiece.State?(this._currentGamePiece.Get()) : new IFlockadeGamePiece.State?())))
      return;
    this._bodyParameters.SetParameterValue("KIND", LocalizationManager.GetTranslation(this._currentGamePiece.Configuration.BaseConfiguration.KindName));
  }
}
