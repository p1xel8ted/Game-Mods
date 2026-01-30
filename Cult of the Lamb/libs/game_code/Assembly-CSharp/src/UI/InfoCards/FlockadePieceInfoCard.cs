// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.FlockadePieceInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using I2.Loc;
using Lamb.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class FlockadePieceInfoCard : UIInfoCardBase<FlockadeGamePieceConfiguration>
{
  public const string _KIND_DESCRIPTION_PARAMETER_NAME = "KIND";
  [Header("Piece Info")]
  [SerializeField]
  public Image _gamePiece;
  [SerializeField]
  public CanvasGroup _gamePieceContainer;
  [SerializeField]
  public GameObject _withBlessing;
  [SerializeField]
  public GameObject _withoutBlessing;
  [SerializeField]
  public Image _blessingIcon;
  [SerializeField]
  public Image _blessingOutline;
  [SerializeField]
  public Localize _name;
  [SerializeField]
  public TextMeshProUGUI _nameText;
  [SerializeField]
  public Localize _description;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public Localize _blessingDescription;
  [SerializeField]
  public TextMeshProUGUI _blessingDescriptionText;
  [SerializeField]
  public LocalizationParamsManager _blessingDescriptionParameters;
  [SerializeField]
  public Image _scribeIcon;
  [SerializeField]
  public Image _shepherdIcon;
  [SerializeField]
  public Image _shieldIcon;
  [SerializeField]
  public Image _swordIcon;
  [SerializeField]
  public Color _currentKind;
  [SerializeField]
  public Color _otherKind;
  public Dictionary<FlockadeGamePiece.Kind, Image> _kindIcons;
  public FlockadeGamePieceConfiguration _configuration;

  public CanvasGroup GamePieceContainer => this._gamePieceContainer;

  public RectTransform GamePiece => this._gamePiece.rectTransform;

  public Dictionary<FlockadeGamePiece.Kind, Image> KindIcons
  {
    get
    {
      Dictionary<FlockadeGamePiece.Kind, Image> kindIcons1 = this._kindIcons;
      if (kindIcons1 != null)
        return kindIcons1;
      Dictionary<FlockadeGamePiece.Kind, Image> dictionary = new Dictionary<FlockadeGamePiece.Kind, Image>();
      dictionary.Add(FlockadeGamePiece.Kind.Shield, this._shieldIcon);
      dictionary.Add(FlockadeGamePiece.Kind.Scribe, this._scribeIcon);
      dictionary.Add(FlockadeGamePiece.Kind.Sword, this._swordIcon);
      dictionary.Add(FlockadeGamePiece.Kind.Shepherd, this._shepherdIcon);
      Dictionary<FlockadeGamePiece.Kind, Image> kindIcons2 = dictionary;
      this._kindIcons = dictionary;
      return kindIcons2;
    }
  }

  public virtual void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateParametersInBlessingDescription);
  }

  public virtual void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateParametersInBlessingDescription);
  }

  public override void Configure(FlockadeGamePieceConfiguration configuration)
  {
    this._configuration = configuration;
    this._shepherdIcon.gameObject.SetActive(FlockadePieceManager.IsAnyPieceOfSameKindUnlocked(FlockadePieceType.Shepherd));
    if ((bool) (Object) configuration)
    {
      this._gamePiece.sprite = configuration.Image;
      this._gamePieceContainer.alpha = 1f;
      this._withBlessing.gameObject.SetActive((bool) (Object) configuration.BlessingConfiguration);
      this._withoutBlessing.gameObject.SetActive(!(bool) (Object) configuration.BlessingConfiguration);
      if ((bool) (Object) configuration.BlessingConfiguration)
      {
        this._blessingIcon.sprite = configuration.BlessingConfiguration.Icon;
        this._blessingIcon.color = configuration.BlessingConfiguration.Color;
        this._blessingOutline.sprite = configuration.BlessingConfiguration.Outline;
        this._blessingOutline.color = configuration.BlessingConfiguration.Color;
      }
      this._name.Term = configuration.Name;
      this._description.Term = configuration.Description;
      foreach (KeyValuePair<FlockadeGamePiece.Kind, Image> kindIcon in this.KindIcons)
        kindIcon.Value.color = kindIcon.Key == configuration.BaseConfiguration.Kind ? this._currentKind : this._otherKind;
      this._blessingDescription.Term = (bool) (Object) configuration.BlessingConfiguration ? configuration.BlessingConfiguration.Description : configuration.BaseConfiguration.KindDescription;
      this.UpdateParametersInBlessingDescription();
    }
    else
    {
      this._gamePiece.sprite = (Sprite) null;
      this._gamePieceContainer.alpha = 0.0f;
      this._withBlessing.gameObject.SetActive(false);
      this._withoutBlessing.gameObject.SetActive(false);
      this._name.Term = (string) null;
      this._nameText.SetText(string.Empty);
      this._description.Term = (string) null;
      this._descriptionText.SetText(string.Empty);
      foreach (Graphic graphic in this.KindIcons.Values)
        graphic.color = this._otherKind;
      this._blessingDescription.Term = (string) null;
      this._blessingDescriptionText.SetText(string.Empty);
    }
  }

  public void UpdateParametersInBlessingDescription()
  {
    if (!(bool) (Object) this._configuration)
      return;
    this._blessingDescriptionParameters.SetParameterValue("KIND", LocalizationManager.GetTranslation(this._configuration.BaseConfiguration.KindName));
  }
}
