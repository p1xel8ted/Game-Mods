// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WorldMapIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.Alerts;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

[ExecuteInEditMode]
public class WorldMapIcon : BaseMonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public Action<WorldMapIcon> OnLocationSelected;
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _localPoint;
  [SerializeField]
  private FollowerLocation _location;
  [SerializeField]
  [TermsPopup("")]
  private string _locationTerm;
  [SerializeField]
  private WorldMapIcon.WorldMapRegion _mapRegion;
  [SerializeField]
  private InspectorScene _scene;
  [SerializeField]
  private LocationAlert _alert;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private ParallaxLayer _layer;
  [SerializeField]
  private Vector2 _parallaxPosition;
  [SerializeField]
  private GameObject _youAreHere;

  public RectTransform RectTransform => this._rectTransform;

  public FollowerLocation Location => this._location;

  public WorldMapIcon.WorldMapRegion MapRegion => this._mapRegion;

  public string LocationTerm => this._locationTerm;

  public MMButton Button => this._button;

  public InspectorScene Scene => this._scene;

  public Vector2 ParallaxPosition => this._parallaxPosition;

  private void Start()
  {
    if (!Application.isPlaying)
      return;
    this._alert.Configure(this._location);
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._youAreHere.SetActive(this._location == DataManager.Instance.CurrentLocation);
  }

  private void OnButtonClicked()
  {
    Action<WorldMapIcon> locationSelected = this.OnLocationSelected;
    if (locationSelected == null)
      return;
    locationSelected(this);
  }

  private void Update()
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
    return string.IsNullOrEmpty(DataManager.Instance.CultName) ? LocalizationManager.GetTranslation("NAMES/Place/Cult") : DataManager.Instance.CultName;
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
  }
}
