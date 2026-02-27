// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WorldMapIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.Alerts;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

public class WorldMapIcon : BaseMonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public Action<WorldMapIcon> OnLocationSelected;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _localPoint;
  [SerializeField]
  public FollowerLocation _location;
  [SerializeField]
  [TermsPopup("")]
  public string _locationTerm;
  [SerializeField]
  public WorldMapIcon.WorldMapRegion _mapRegion;
  [SerializeField]
  public InspectorScene _scene;
  [SerializeField]
  public LocationAlert _alert;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public ParallaxLayer _layer;
  [SerializeField]
  public Vector2 _parallaxPosition;
  [SerializeField]
  public GameObject _youAreHere;

  public RectTransform RectTransform => this._rectTransform;

  public FollowerLocation Location => this._location;

  public WorldMapIcon.WorldMapRegion MapRegion => this._mapRegion;

  public string LocationTerm => this._locationTerm;

  public MMButton Button => this._button;

  public InspectorScene Scene => this._scene;

  public Vector2 ParallaxPosition => this._parallaxPosition;

  public virtual void Awake()
  {
  }

  public virtual void Start()
  {
    if (!Application.isPlaying)
      return;
    this._alert.Configure(this._location);
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._youAreHere.SetActive(this._location == DataManager.Instance.CurrentLocation);
  }

  public void SetPosition(Vector2 pos) => this._parallaxPosition = pos;

  public virtual void OnButtonClicked()
  {
    Action<WorldMapIcon> locationSelected = this.OnLocationSelected;
    if (locationSelected == null)
      return;
    locationSelected(this);
  }

  public void Update()
  {
    if (!((UnityEngine.Object) this._localPoint != (UnityEngine.Object) null))
      return;
    this._rectTransform.anchoredPosition = (Vector2) this._rectTransform.parent.InverseTransformPoint(this._localPoint.parent.TransformPoint(this._localPoint.localPosition));
  }

  public string RegionMaterialProperty() => $"_{this._mapRegion}";

  public string GetLocalisedLocation()
  {
    if (this._location != FollowerLocation.Base)
      return LocalizationManager.GetTranslation(this._locationTerm);
    return string.IsNullOrEmpty(DataManager.Instance.CultName) ? LocalizationManager.GetTranslation("NAMES/Place/Cult") : LocalizeIntegration.Arabic_ReverseNonRTL(DataManager.Instance.CultName);
  }

  public void OnSelect(BaseEventData eventData) => this._alert.TryRemoveAlert();

  public enum WorldMapRegion
  {
    Base,
    Ratau_Hut,
    Shore,
    Sozo,
    Midas_Cave,
    Plimbo_Shop,
    LambTown,
    Graveyard,
  }
}
