// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIWorldMapMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIWorldMapMenuController : UIMenuBase
{
  public const float kLocationFocusTime = 0.5f;
  public static FollowerLocation[] UnlockableMapLocations = new FollowerLocation[6]
  {
    FollowerLocation.Hub1_RatauOutside,
    FollowerLocation.Hub1_Sozo,
    FollowerLocation.HubShore,
    FollowerLocation.Dungeon_Decoration_Shop1,
    FollowerLocation.Dungeon_Location_4,
    FollowerLocation.Graveyard_Location
  };
  [Header("World Map Menu")]
  [SerializeField]
  public WorldMapIcon[] _locations;
  [SerializeField]
  public WorldMapClouds[] _clouds;
  [SerializeField]
  public AnimationCurve _focusCurve;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [Header("Map Controls")]
  [SerializeField]
  public RectTransform _mapContainer;
  [SerializeField]
  public WorldMapParallax _parallax;
  [Header("Header")]
  [SerializeField]
  public TextMeshProUGUI _locationHeader;
  [SerializeField]
  public RectTransform _locationHeaderContainer;
  [SerializeField]
  public CanvasGroup _locationHeaderCanvasGroup;
  [Header("Animations")]
  [SerializeField]
  public GameObject _flames;
  [Header("Rot References")]
  [SerializeField]
  public Dictionary<int, GameObject> _rotStages = new Dictionary<int, GameObject>();
  public FollowerLocation _revealLocation = FollowerLocation.None;
  public bool _reReveal;
  public Vector2 _locationHeaderOrigin;
  public bool _didCancel;
  public Coroutine _focusCoroutine;
  public bool isLoadingAssets;

  public void Start()
  {
    this._canvasGroup.alpha = 0.0f;
    this._flames.SetActive(DataManager.Instance.Lighthouse_Lit);
    foreach (WorldMapIcon location1 in this._locations)
    {
      WorldMapIcon location = location1;
      location.OnLocationSelected += new Action<WorldMapIcon>(this.OnLocationSelected);
      location.Button.OnSelected += (System.Action) (() => this.OnLocationHighlighted(location));
      if (!DataManager.Instance.DiscoveredLocations.Contains(location.Location) && this._revealLocation != location.Location)
        location.gameObject.SetActive(false);
      if (this._revealLocation != FollowerLocation.None)
        location.gameObject.SetActive(false);
    }
  }

  public void InitMountainRot()
  {
    int winterServerity = DataManager.Instance.WinterServerity;
    foreach (KeyValuePair<int, GameObject> rotStage in this._rotStages)
    {
      if (!rotStage.Value.activeSelf)
        rotStage.Value.SetActive(rotStage.Key == winterServerity);
    }
  }

  public void Show(FollowerLocation revealLocation, bool reReveal = false, bool instant = false)
  {
    this._revealLocation = revealLocation;
    this._reReveal = reReveal;
    this._controlPrompts.HideAcceptButton();
    this._controlPrompts.HideCancelButton();
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    this.InitMountainRot();
    foreach (WorldMapClouds cloud in this._clouds)
    {
      if (DataManager.Instance.DiscoveredLocations.Contains(cloud.Location) && this._revealLocation != cloud.Location)
        cloud.Hide();
      if (DataManager.Instance.DiscoveredLocations.Contains(cloud.Location) && this._revealLocation == cloud.Location && this._reReveal)
        cloud.Hide();
    }
  }

  public override void OnShowCompleted()
  {
    FollowerLocation followerLocation = this._revealLocation != FollowerLocation.None ? this._revealLocation : DataManager.Instance.CurrentLocation;
    foreach (WorldMapIcon location in this._locations)
    {
      if (location.Location == followerLocation)
      {
        this.OverrideDefault((Selectable) location.Button);
        break;
      }
    }
    this.ActivateNavigation();
  }

  public override IEnumerator DoShowAnimation()
  {
    UIWorldMapMenuController mapMenuController = this;
    if (mapMenuController._revealLocation != FollowerLocation.None)
    {
      mapMenuController._canvasGroup.interactable = false;
      mapMenuController._mapContainer.localScale = Vector3.one * 0.65f;
      mapMenuController._locationHeaderOrigin = mapMenuController._locationHeaderContainer.anchoredPosition;
      mapMenuController._locationHeaderCanvasGroup.alpha = 0.0f;
    }
    while ((double) mapMenuController._canvasGroup.alpha < 1.0)
    {
      mapMenuController._canvasGroup.alpha += Time.unscaledDeltaTime * 4f;
      yield return (object) null;
    }
    if (mapMenuController._revealLocation != FollowerLocation.None)
    {
      WorldMapIcon targetIcon = (WorldMapIcon) null;
      foreach (WorldMapIcon location in mapMenuController._locations)
      {
        if (location.Location == mapMenuController._revealLocation)
        {
          targetIcon = location;
          break;
        }
      }
      WorldMapClouds targetCloud = (WorldMapClouds) null;
      foreach (WorldMapClouds cloud in mapMenuController._clouds)
      {
        if (cloud.Location == mapMenuController._revealLocation)
        {
          targetCloud = cloud;
          break;
        }
      }
      yield return (object) new WaitForSecondsRealtime(0.5f);
      mapMenuController._mapContainer.DOScale(Vector3.one, 1.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) mapMenuController.DoFocusLocation(targetIcon, 1.5f);
      UIManager.PlayAudio("event:/ui/map_location_pan");
      yield return (object) new WaitForSecondsRealtime(0.25f);
      if (!mapMenuController._reReveal)
      {
        yield return (object) targetCloud.DoHide();
        yield return (object) new WaitForSecondsRealtime(0.25f);
        UIManager.PlayAudio("event:/ui/map_location_appear");
        mapMenuController._locationHeaderContainer.anchoredPosition = (Vector2) Vector3.zero;
        mapMenuController._locationHeaderContainer.localScale = Vector3.one * 4f;
        mapMenuController._locationHeaderContainer.DOScale(Vector3.one * 2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        mapMenuController._locationHeaderCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
        mapMenuController._locationHeader.text = LocalizationManager.GetTranslation(targetIcon.LocationTerm);
        yield return (object) new WaitForSecondsRealtime(1f);
        mapMenuController._locationHeaderContainer.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        mapMenuController._locationHeaderContainer.DOAnchorPos(mapMenuController._locationHeaderOrigin, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
        yield return (object) new WaitForSecondsRealtime(0.5f);
      }
      else
      {
        mapMenuController._locationHeaderCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
        mapMenuController._locationHeader.text = LocalizationManager.GetTranslation(targetIcon.LocationTerm);
      }
      yield return (object) new WaitForSecondsRealtime(1f);
      mapMenuController.Hide();
      targetIcon = (WorldMapIcon) null;
      targetCloud = (WorldMapClouds) null;
    }
  }

  public void OnLocationSelected(WorldMapIcon location)
  {
    if (this.isLoadingAssets || location.Location == DataManager.Instance.CurrentLocation)
      return;
    DataManager.Instance.CurrentLocation = location.Location;
    this._canvasGroup.interactable = false;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, location.Scene.SceneName, 1f, location.GetLocalisedLocation(), (System.Action) (() =>
    {
      this.Hide(true);
      SaveAndLoad.Save();
    }));
    if (DataManager.Instance.VisitedLocations.Contains(location.Location))
      return;
    DataManager.Instance.VisitedLocations.Add(location.Location);
  }

  public void OnLocationHighlighted(WorldMapIcon location)
  {
    this._locationHeader.text = location.GetLocalisedLocation();
    if (InputManager.General.MouseInputActive)
      return;
    this.FocusLocation(location, 0.5f);
  }

  public void FocusLocation(WorldMapIcon location, float time)
  {
    float num = Vector2.Distance(Vector2.zero, location.ParallaxPosition) / 1080f;
    Vector2 targetPosition = location.ParallaxPosition.normalized * num * 150f;
    if (this._focusCoroutine != null)
      this.StopCoroutine(this._focusCoroutine);
    this._focusCoroutine = this.StartCoroutine((IEnumerator) this.DoFocusPosition(targetPosition, time));
  }

  public IEnumerator DoFocusLocation(WorldMapIcon location, float time)
  {
    Vector2 parallaxPosition = location.ParallaxPosition;
    parallaxPosition.x *= (float) (1.0 / ((double) this._parallax.HorizontalIntensity * (double) this._parallax.GlobalIntensity));
    parallaxPosition.y *= (float) (1.0 / ((double) this._parallax.VerticalIntensity * (double) this._parallax.GlobalIntensity));
    yield return (object) this.DoFocusPosition(parallaxPosition, time);
  }

  public IEnumerator DoFocusPosition(Vector2 targetPosition, float time)
  {
    Vector2 currentPosition = this._parallax.RectTransform.anchoredPosition;
    float t = 0.0f;
    while ((double) t <= (double) time)
    {
      t += Time.unscaledDeltaTime;
      this._parallax.RectTransform.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, this._focusCurve.Evaluate(t / time));
      yield return (object) null;
    }
  }

  public override IEnumerator DoHideAnimation()
  {
    UIWorldMapMenuController mapMenuController = this;
    while ((double) mapMenuController._canvasGroup.alpha > 0.0)
    {
      mapMenuController._canvasGroup.alpha -= Time.unscaledDeltaTime * 4f;
      yield return (object) null;
    }
    if (mapMenuController._focusCoroutine != null)
    {
      mapMenuController.StopCoroutine(mapMenuController._focusCoroutine);
      mapMenuController._focusCoroutine = (Coroutine) null;
    }
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this._didCancel = true;
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    MonoSingleton<UIManager>.Instance.UnloadWorldMapAssets();
  }

  [CompilerGenerated]
  public void \u003COnLocationSelected\u003Eb__25_0()
  {
    this.Hide(true);
    SaveAndLoad.Save();
  }
}
