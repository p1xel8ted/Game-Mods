// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDLCMapMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Febucci.UI;
using JetBrains.Annotations;
using MMTools;
using src.Extensions;
using src.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIDLCMapMenuController : UIMenuBase
{
  [SerializeField]
  public bool _debugFakeProgress = true;
  [Space]
  [Header("Nodes")]
  [SerializeField]
  public DungeonWorldMapIcon[] _locations;
  [SerializeField]
  public DungeonWorldMapIcon.IconState[] _depthRules = new DungeonWorldMapIcon.IconState[3]
  {
    DungeonWorldMapIcon.IconState.Completed,
    DungeonWorldMapIcon.IconState.Selectable,
    DungeonWorldMapIcon.IconState.Preview
  };
  [SerializeField]
  public GoopFade _goopFade;
  [SerializeField]
  public CanvasParallax _parallax;
  [SerializeField]
  public GameObject _displacementRingPrefab;
  [SerializeField]
  public GameObject _lockBreakParticlesPrefab;
  [SerializeField]
  public CanvasGroup _containerCanvasGroup;
  [SerializeField]
  public CanvasGroup _locationTextCanvasGroup;
  [SerializeField]
  public Transform _insideConnectionsContainer;
  [SerializeField]
  public Transform _outsideConnectionsContainer;
  [SerializeField]
  public Image _rotMask;
  [SerializeField]
  public SpriteByValueSelector _rotSpriteSelector;
  [SerializeField]
  public Image _rotMaskDistortion;
  [SerializeField]
  public DungeonWorldMapIcon _outsideDefaultLocation;
  [SerializeField]
  public DungeonWorldMapIcon _insideDefaultLocation;
  [SerializeField]
  public DungeonWorldMapIcon _onboardingDefaultLocation;
  [SerializeField]
  public TextMeshProUGUI _locationHeader;
  [SerializeField]
  public DLCMapConnection _connectionPrefab;
  [SerializeField]
  public CrownMapMarker _crownMapMarker;
  [SerializeField]
  public UIDLCMapSideTransitioner _backgroundSideTransitioner;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [Tooltip("The delay between the crown fading out and the map fading out when entering as node to go to a location")]
  [SerializeField]
  public float _pauseSecondsAfterCrownFades;
  [SerializeField]
  public DungeonWorldMapIcon _currentSelectable;
  public int _activeLockCount;
  public Dictionary<int, DungeonWorldMapIcon> _nodeLookup = new Dictionary<int, DungeonWorldMapIcon>();
  public bool _processingLocationSelect;
  public Stack<DungeonWorldMapIcon> _travelHistory = new Stack<DungeonWorldMapIcon>();
  [CompilerGenerated]
  public bool \u003CAutoSelectNextNode\u003Ek__BackingField = true;
  [CompilerGenerated]
  public UIDLCMapMenuController.DLCDungeonSide \u003CCurrentDLCDungeonSide\u003Ek__BackingField = UIDLCMapMenuController.DLCDungeonSide.OutsideMountain;
  public const int TOTAL_NODES_TO_COMPLETE = 80 /*0x50*/;
  public bool _showAllCheat;

  public bool DebugFakeProgress
  {
    get => this._debugFakeProgress;
    set => this._debugFakeProgress = value;
  }

  public void ShowButton() => this.Show();

  public void HideButton() => this.Hide();

  public void RevealAll() => this.Show();

  public void ResetData()
  {
    DataManager.Instance.VisitedLocations.Clear();
    DataManager.Instance.DLCDungeonNodeCurrent = -1;
    DataManager.Instance.CurrentDLCDungeonID = -1;
    DataManager.Instance.CurrentLocation = FollowerLocation.Base;
    DataManager.Instance.RevealDLCDungeonNode = false;
    DataManager.Instance.DLCDungeonNodesCompleted.Clear();
    DataManager.Instance.DLCKey_1 = 0;
    DataManager.Instance.DLCKey_2 = 0;
    DataManager.Instance.DLCKey_3 = 0;
    SaveAndLoad.Save();
  }

  public DungeonWorldMapIcon.IconState[] DepthRules => this._depthRules;

  public bool AutoSelectNextNode
  {
    get => this.\u003CAutoSelectNextNode\u003Ek__BackingField;
    set => this.\u003CAutoSelectNextNode\u003Ek__BackingField = value;
  }

  public CanvasParallax Parallax => this._parallax;

  public UIDLCMapMenuController.DLCDungeonSide CurrentDLCDungeonSide
  {
    get => this.\u003CCurrentDLCDungeonSide\u003Ek__BackingField;
    set => this.\u003CCurrentDLCDungeonSide\u003Ek__BackingField = value;
  }

  public UIDLCMapMenuController.DLCDungeonSide HiddenDLCDungeonSide
  {
    get
    {
      return this.CurrentDLCDungeonSide != UIDLCMapMenuController.DLCDungeonSide.OutsideMountain ? UIDLCMapMenuController.DLCDungeonSide.OutsideMountain : UIDLCMapMenuController.DLCDungeonSide.InsideMountain;
    }
  }

  public IEnumerable<DungeonWorldMapIcon> Locations
  {
    get => (IEnumerable<DungeonWorldMapIcon>) this._locations;
  }

  public DungeonWorldMapIcon HomeLocation => this._outsideDefaultLocation;

  public DungeonWorldMapIcon BaseLocationForCurrentSide
  {
    get
    {
      if (this.CurrentDLCDungeonSide != UIDLCMapMenuController.DLCDungeonSide.InsideMountain)
        return this._outsideDefaultLocation;
      foreach (DungeonWorldMapIcon location in this.Locations)
      {
        if (location.DungeonSide == this.CurrentDLCDungeonSide && (location.CurrentState == DungeonWorldMapIcon.IconState.Selectable || location.CurrentState == DungeonWorldMapIcon.IconState.Completed))
          return location;
      }
      return this._insideDefaultLocation;
    }
  }

  public void AutoSelectNextValidLocation()
  {
    if (!this.AutoSelectNextNode)
      return;
    this.InitialiseCrown();
    DungeonWorldMapIcon dungeonWorldMapIcon;
    if (!this._nodeLookup.TryGetValue(DataManager.Instance.DLCDungeonNodeCurrent, out dungeonWorldMapIcon))
      return;
    MMButton button = dungeonWorldMapIcon.Button;
    foreach (DungeonWorldMapIcon childNode in dungeonWorldMapIcon.ChildNodes)
    {
      if (!childNode.IsLock && childNode.CurrentState == DungeonWorldMapIcon.IconState.Selectable)
      {
        button = childNode.Button;
        MonoBehaviour.print((object) ("Defaulting to child node " + childNode.name));
        break;
      }
    }
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) button);
  }

  public override void Awake() => base.Awake();

  public void Update()
  {
    if (!this._debugFakeProgress || !Input.GetKeyDown(KeyCode.R))
      return;
    this.Show();
  }

  public void RevealAllSelectable()
  {
    this._showAllCheat = true;
    this._depthRules = new DungeonWorldMapIcon.IconState[11]
    {
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable,
      DungeonWorldMapIcon.IconState.Selectable
    };
  }

  public void OnEnable()
  {
    this._locationTextCanvasGroup.alpha = 0.0f;
    this._containerCanvasGroup.alpha = 0.0f;
  }

  public void Show()
  {
    this.Show(false);
    this.InitialiseContent();
    this.LockMenu();
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    foreach (DungeonWorldMapIcon location1 in this.Locations)
    {
      DungeonWorldMapIcon location = location1;
      location.OnDungeonLocationSelected += new Action<DungeonWorldMapIcon>(this.OnLocationSelected);
      location.Button.OnSelected += (System.Action) (() =>
      {
        this.OnLocationHighlighted(location);
        this._currentSelectable = location;
      });
      location.Button.OnDeselected += (System.Action) (() => this.OnLocationDehighlighted(location));
      location.Button.OnPointerEntered += (System.Action) (() => UnityEngine.Debug.Log((object) ("Detected Pointer Entered " + location.gameObject.name)));
      location.Button.OnPointerExited += (System.Action) (() => UnityEngine.Debug.Log((object) ("Detected Pointer Exited " + location.gameObject.name)));
      location.Configure(location, false);
    }
    this._backgroundSideTransitioner.Initialise();
  }

  public override IEnumerator DoShow()
  {
    UIDLCMapMenuController mapMenuController = this;
    mapMenuController._goopFade.gameObject.SetActive(true);
    mapMenuController._goopFade.FadeIn(1f);
    yield return (object) new WaitForSecondsRealtime(1f);
    if ((UnityEngine.Object) MMTransition.Instance != (UnityEngine.Object) null)
      MMTransition.Instance.FadeOutInstant();
    mapMenuController._containerCanvasGroup.alpha = 1f;
    mapMenuController._parallax.parallaxZoom = 5f;
    mapMenuController._parallax.offset = Vector2.up * 30f;
    mapMenuController._parallax.ZoomAsync(0.0f, 2f, Ease.OutCubic);
    mapMenuController._parallax.PanAsync(Vector2.zero, 2f, Ease.OutCubic);
    mapMenuController._goopFade.FadeOut();
    mapMenuController.OnShowStarted();
    if (mapMenuController._addToActiveMenus && (UnityEngine.Object) mapMenuController._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
    {
      System.Action onFirstMenuShow = UIMenuBase.OnFirstMenuShow;
      if (onFirstMenuShow != null)
        onFirstMenuShow();
    }
    System.Action onShow = mapMenuController.OnShow;
    if (onShow != null)
      onShow();
    if (mapMenuController._addToActiveMenus && (UnityEngine.Object) mapMenuController._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 1)
    {
      System.Action onFirstMenuShown = UIMenuBase.OnFirstMenuShown;
      if (onFirstMenuShown != null)
        onFirstMenuShown();
    }
    System.Action onShown = mapMenuController.OnShown;
    if (onShown != null)
      onShown();
    mapMenuController.OnShowCompleted();
    Time.timeScale = 0.0f;
    SimulationManager.Pause();
    AudioManager.Instance.PlayOneShot("event:/ui/open_menu");
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
      BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 0.6f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    yield return (object) mapMenuController.DoShowAnimation();
    InputManager.General.MouseInputEnabled = !InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    Cursor.visible = InputManager.General.MouseInputEnabled && !InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    foreach (MMControlPrompt componentsInChild in mapMenuController.GetComponentsInChildren<MMControlPrompt>())
      componentsInChild.ForceUpdate();
    System.Action onShownCompleted = mapMenuController.OnShownCompleted;
    if (onShownCompleted != null)
      onShownCompleted();
    yield return (object) new WaitForSecondsRealtime(2f);
    mapMenuController.OnShowFinished();
    mapMenuController.IsShowing = false;
    mapMenuController.AutoSelectNextValidLocation();
  }

  public override IEnumerator DoShowAnimation()
  {
    yield return (object) this.\u003C\u003En__0();
  }

  public override void OnShowFinished()
  {
    base.OnShowFinished();
    this.UnlockMenu();
  }

  public override void OnHideStarted()
  {
    this.LockMenu();
    AudioManager.Instance.ResumePausedLoopsAndSFX();
    AudioManager.Instance.ToggleFilter(SoundParams.Filter, false);
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = (PlayerFarming) null;
    base.OnHideStarted();
    this._goopFade.gameObject.SetActive(true);
    this._goopFade.FadeIn();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override IEnumerator DoHideAnimation()
  {
    float num = 1.5f;
    Time.timeScale = 1f;
    SimulationManager.UnPause();
    this._containerCanvasGroup.DOFade(0.0f, 0.25f);
    this._goopFade.FadeOut(num);
    yield return (object) new WaitForSecondsRealtime(num);
    yield return (object) this.\u003C\u003En__1();
  }

  public override IEnumerator DoHide()
  {
    yield return (object) this.\u003C\u003En__2();
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    Time.timeScale = 1f;
    SimulationManager.UnPause();
    MonoSingleton<UIManager>.Instance.UnloadDLCWorldMapAssets();
  }

  public DLCMapConnection ConnectNodes(DungeonWorldMapIcon from, DungeonWorldMapIcon to)
  {
    DLCMapConnection connection = UnityEngine.Object.Instantiate<DLCMapConnection>(this._connectionPrefab, from.DungeonSide == UIDLCMapMenuController.DLCDungeonSide.OutsideMountain ? this._outsideConnectionsContainer : this._insideConnectionsContainer);
    connection.SetEndpoints(from, to);
    from.AddChildConnection(connection);
    to.AddParentConnection(connection);
    return connection;
  }

  public void RefreshConnectionStates()
  {
    foreach (DungeonWorldMapIcon location in this.Locations)
    {
      foreach (DLCMapConnection childConnection in location.ChildConnections)
        childConnection.RefreshState(true);
    }
  }

  public void SetDungeonSide(UIDLCMapMenuController.DLCDungeonSide side)
  {
    this._backgroundSideTransitioner.SetCurrentLayer(side);
    this.RefreshConnectionStates();
  }

  public void SpawnDisplacementRingAt(RectTransform pos, float scale = 1f)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._displacementRingPrefab, this._containerCanvasGroup.transform);
    gameObject.transform.localScale = Vector3.one * scale;
    gameObject.transform.position = pos.position;
  }

  public void SpawnLockBreakParticlesAt(RectTransform pos)
  {
    UnityEngine.Object.Instantiate<GameObject>(this._lockBreakParticlesPrefab, this._containerCanvasGroup.transform).transform.position = pos.position;
  }

  public void InitialiseMapSide()
  {
    int key = DataManager.Instance.DLCDungeonNodeCurrent;
    if (key < 0)
    {
      key = 0;
      DataManager.Instance.DLCDungeonNodeCurrent = key;
    }
    DungeonWorldMapIcon dungeonWorldMapIcon;
    this.CurrentDLCDungeonSide = !this._nodeLookup.TryGetValue(key, out dungeonWorldMapIcon) ? UIDLCMapMenuController.DLCDungeonSide.OutsideMountain : dungeonWorldMapIcon.DungeonSide;
    this._backgroundSideTransitioner.SetCurrentLayer(this.CurrentDLCDungeonSide);
    if (this.CurrentDLCDungeonSide == UIDLCMapMenuController.DLCDungeonSide.OutsideMountain)
      UIManager.PlayAudio("event:/dlc/ui/map/ewefall_enter");
    if (this.CurrentDLCDungeonSide != UIDLCMapMenuController.DLCDungeonSide.InsideMountain)
      return;
    UIManager.PlayAudio("event:/dlc/ui/map/therot_enter");
  }

  public void InitialiseCrown()
  {
    if (DataManager.Instance.DLCDungeonNodeCurrent < 0)
      DataManager.Instance.DLCDungeonNodeCurrent = 0;
    DungeonWorldMapIcon icon;
    if (!this._nodeLookup.TryGetValue(DataManager.Instance.DLCDungeonNodeCurrent, out icon))
    {
      UnityEngine.Debug.LogError((object) $"Could not find current node, ID: {DataManager.Instance.DLCDungeonNodeCurrent}");
    }
    else
    {
      if (DataManager.Instance.FinalDLCMap && !icon.IsEndGameNode)
      {
        foreach (DungeonWorldMapIcon location in this.Locations)
        {
          if (location.IsEndGameNode && location.isActiveAndEnabled && location.Type == DungeonWorldMapIcon.NodeType.Door)
          {
            icon = location;
            DataManager.Instance.DLCDungeonNodeCurrent = icon.ID;
            break;
          }
        }
      }
      this._crownMapMarker.gameObject.SetActive(true);
      this._crownMapMarker.Teleport(icon.transform.position, icon);
      this._crownMapMarker.Show();
    }
  }

  public void OnLocationSelected(DungeonWorldMapIcon location)
  {
    bool flag1 = location.SelectAction == DungeonWorldMapIcon.IconActionType.Nothing;
    bool isLockWithoutKey = location.IsLockWithoutKey;
    int num = location.SelectAction == DungeonWorldMapIcon.IconActionType.Doorway ? 1 : 0;
    bool flag2 = location.CurrentState == DungeonWorldMapIcon.IconState.Completed;
    bool flag3 = location.IsBeaten;
    bool flag4 = location.Type == DungeonWorldMapIcon.NodeType.Base;
    bool flag5 = location.Type == DungeonWorldMapIcon.NodeType.Dungeon5_Boss && location.MiniBossNodesBeaten() < 8;
    if (this._showAllCheat)
    {
      flag2 = false;
      flag3 = false;
    }
    if (num != 0 || location.IsEndGameNode)
      this.OnLocationSelectedAsync(location);
    else if (flag4)
      this.OnCancelButtonInput();
    else if (flag1 | isLockWithoutKey | flag2 | flag3 | flag5)
      location.SelectInvalid();
    else
      this.OnLocationSelectedAsync(location);
  }

  public async System.Threading.Tasks.Task OnLocationSelectedAsync(DungeonWorldMapIcon location)
  {
    location.SelectValid();
    if (MMTransition.IsPlaying || this._processingLocationSelect)
      return;
    this._processingLocationSelect = true;
    this.LockMenu();
    try
    {
      if (location.SelectAction == DungeonWorldMapIcon.IconActionType.Nothing)
        return;
      await this.HandlePathing(location);
      await this.HandleDoorway(location);
      await this.HandleLocation(location);
    }
    finally
    {
      this._processingLocationSelect = false;
      this.UnlockMenu();
      this._travelHistory.Clear();
    }
  }

  public async System.Threading.Tasks.Task HandlePathing(DungeonWorldMapIcon target)
  {
    DungeonWorldMapIcon doorTo;
    DungeonWorldMapIcon start;
    if (!this._nodeLookup.TryGetValue(DataManager.Instance.DLCDungeonNodeCurrent, out start))
      doorTo = (DungeonWorldMapIcon) null;
    else if ((UnityEngine.Object) start == (UnityEngine.Object) target)
    {
      doorTo = (DungeonWorldMapIcon) null;
    }
    else
    {
      List<DungeonWorldMapIcon> nodePath1;
      PooledObject<List<DungeonWorldMapIcon>> pooledObject = CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out nodePath1);
      try
      {
        if (this.TryBuildNodePath(start, target, nodePath1))
        {
          await this.MoveAlongNodePathAsync(nodePath1, target);
          doorTo = (DungeonWorldMapIcon) null;
          return;
        }
      }
      finally
      {
        pooledObject.Dispose();
      }
      pooledObject = new PooledObject<List<DungeonWorldMapIcon>>();
      DungeonWorldMapIcon dungeonWorldMapIcon = (DungeonWorldMapIcon) null;
      List<DungeonWorldMapIcon> nodePath2;
      using (CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out nodePath2))
      {
        foreach (DungeonWorldMapIcon location in this.Locations)
        {
          if (location.DungeonSide == start.DungeonSide && location.SelectAction == DungeonWorldMapIcon.IconActionType.Doorway)
          {
            if (this.TryBuildNodePath(start, location, nodePath2))
            {
              dungeonWorldMapIcon = location;
              break;
            }
            nodePath2.Clear();
          }
        }
      }
      if ((UnityEngine.Object) dungeonWorldMapIcon == (UnityEngine.Object) null)
      {
        doorTo = (DungeonWorldMapIcon) null;
      }
      else
      {
        doorTo = (DungeonWorldMapIcon) null;
        List<DungeonWorldMapIcon> nodePath3;
        using (CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out nodePath3))
        {
          foreach (DungeonWorldMapIcon location in this.Locations)
          {
            if (location.DungeonSide == target.DungeonSide && location.SelectAction == DungeonWorldMapIcon.IconActionType.Doorway)
            {
              if (this.TryBuildNodePath(location, target, nodePath3))
              {
                doorTo = location;
                break;
              }
              nodePath3.Clear();
            }
          }
        }
        if ((UnityEngine.Object) doorTo == (UnityEngine.Object) null)
        {
          doorTo = (DungeonWorldMapIcon) null;
        }
        else
        {
          List<DungeonWorldMapIcon> nodePath4;
          pooledObject = CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out nodePath4);
          try
          {
            this.TryBuildNodePath(start, dungeonWorldMapIcon, nodePath4);
            await this.MoveAlongNodePathAsync(nodePath4, dungeonWorldMapIcon);
          }
          finally
          {
            pooledObject.Dispose();
          }
          pooledObject = new PooledObject<List<DungeonWorldMapIcon>>();
          await this._crownMapMarker.HideAsync();
          this._crownMapMarker.Teleport(doorTo.transform.position, doorTo);
          await this._crownMapMarker.ShowAsync();
          List<DungeonWorldMapIcon> nodePath5;
          pooledObject = CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out nodePath5);
          try
          {
            this.TryBuildNodePath(doorTo, target, nodePath5);
            await this.MoveAlongNodePathAsync(nodePath5, target);
          }
          finally
          {
            pooledObject.Dispose();
          }
          pooledObject = new PooledObject<List<DungeonWorldMapIcon>>();
          doorTo = (DungeonWorldMapIcon) null;
        }
      }
    }
  }

  public bool TryBuildNodePath(
    DungeonWorldMapIcon start,
    DungeonWorldMapIcon goal,
    List<DungeonWorldMapIcon> nodePath)
  {
    nodePath.Clear();
    Dictionary<int, int> distances;
    using (CollectionPool<Dictionary<int, int>, KeyValuePair<int, int>>.Get(out distances))
    {
      this.CalculateDistances(goal, 0, distances);
      if (!distances.ContainsKey(start.ID))
        return false;
      nodePath.Add(start);
      DungeonWorldMapIcon dungeonWorldMapIcon1 = start;
      List<DungeonWorldMapIcon> neighbours;
      using (CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out neighbours))
      {
        while (dungeonWorldMapIcon1.ID != goal.ID)
        {
          dungeonWorldMapIcon1.GatherActiveNeighbours(neighbours);
          DungeonWorldMapIcon dungeonWorldMapIcon2 = (DungeonWorldMapIcon) null;
          int num1 = int.MaxValue;
          foreach (DungeonWorldMapIcon dungeonWorldMapIcon3 in neighbours)
          {
            int num2;
            if (distances.TryGetValue(dungeonWorldMapIcon3.ID, out num2) && num2 < num1 && !nodePath.Contains(dungeonWorldMapIcon3))
            {
              num1 = num2;
              dungeonWorldMapIcon2 = dungeonWorldMapIcon3;
            }
          }
          if ((UnityEngine.Object) dungeonWorldMapIcon2 == (UnityEngine.Object) null)
            return false;
          dungeonWorldMapIcon1 = dungeonWorldMapIcon2;
          nodePath.Add(dungeonWorldMapIcon1);
          if (nodePath.Count > 50)
            throw new Exception("Path Failed: Endless Loop Occured");
        }
      }
    }
    return true;
  }

  public async System.Threading.Tasks.Task MoveAlongNodePathAsync(
    List<DungeonWorldMapIcon> nodePath,
    DungeonWorldMapIcon finalTarget)
  {
    for (int index = nodePath.Count - 1; index >= 0; --index)
    {
      if (nodePath[index].IsLock)
        nodePath.RemoveAt(index);
    }
    if (nodePath == null || nodePath.Count < 2)
      return;
    for (int index = 0; index < nodePath.Count - 1; ++index)
      this._travelHistory.Push(nodePath[index]);
    List<Vector3> path;
    PooledObject<List<Vector3>> pooledObject = CollectionPool<List<Vector3>, Vector3>.Get(out path);
    try
    {
      foreach (DungeonWorldMapIcon dungeonWorldMapIcon in nodePath)
        path.Add(dungeonWorldMapIcon.RectTransform.position);
      await this._crownMapMarker.MoveToAsync(path, finalTarget);
    }
    finally
    {
      pooledObject.Dispose();
    }
    pooledObject = new PooledObject<List<Vector3>>();
  }

  public async System.Threading.Tasks.Task HandleLocation(DungeonWorldMapIcon location)
  {
    UIDLCMapMenuController mapMenuController1 = this;
    if (location.SelectAction != DungeonWorldMapIcon.IconActionType.SendToLocation)
      ;
    else if (location.Location == DataManager.Instance.CurrentLocation)
      ;
    else
    {
      RectTransform component = location.GetComponent<RectTransform>();
      Vector2 anchoredPosition = component.anchoredPosition;
      mapMenuController1._goopFade.gameObject.SetActive(true);
      mapMenuController1._goopFade.FadeIn(UseDeltaTime: false);
      UIManager.PlayAudio("event:/dlc/ui/map/node_dungeon_enter");
      mapMenuController1._parallax.ZoomPanToRectAsync(component, 5f, 2f);
      await mapMenuController1._crownMapMarker.HideAsync();
      if ((double) mapMenuController1._pauseSecondsAfterCrownFades > 0.0099999997764825821)
        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds((double) mapMenuController1._pauseSecondsAfterCrownFades));
      DataManager.Instance.CurrentDLCNodeType = location.Type;
      if (location.Location == FollowerLocation.Dungeon1_5 || location.Location == FollowerLocation.Dungeon1_6)
      {
        int NewLayer = location.DungeonLayer;
        DataManager.Instance.DungeonBossFight = location.IsBoss;
        DataManager.Instance.IsMiniBoss = location.IsMiniBoss;
        DataManager.Instance.IsLambGhostRescue = location.IsNPC;
        DataManager.Instance.RoomVariant = location.Variant;
        DataManager.Instance.MapLockCountToUnlock = location.UnlocksLockCount;
        if (NewLayer >= 4 && location.Type != DungeonWorldMapIcon.NodeType.Dungeon5_Boss && location.Type != DungeonWorldMapIcon.NodeType.Dungeon6_Boss)
        {
          NewLayer = 3;
          GameManager.DungeonUseAllLayers = true;
        }
        else
          GameManager.DungeonUseAllLayers = false;
        GameManager.NextDungeonLayer(NewLayer);
        GameManager.NewRun("", false, location.Location);
        DataManager.Instance.CurrentDLCDungeonID = !location.IsEndGameNode ? location.ID : -1;
        DataManager.Instance.CurrentLocation = FollowerLocation.Base;
      }
      else
        DataManager.Instance.CurrentLocation = location.Location;
      UIMenuBase.ActiveMenus.Clear();
      mapMenuController1._canvasGroup.interactable = false;
      mapMenuController1.FocusLocation((WorldMapIcon) location, 0.5f, 0.3f);
      if (mapMenuController1._debugFakeProgress)
      {
        location.CompleteNode();
        location.SetCurrentNode();
      }
      string sceneName = location.Scene.SceneName;
      if (location.Type == DungeonWorldMapIcon.NodeType.Dungeon5_Boss)
        sceneName = "Dungeon Boss Wolf";
      DOVirtual.DelayedCall(1f, (TweenCallback) (() =>
      {
        if (!this._debugFakeProgress)
        {
          MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, sceneName, 1f, location.GetLocalisedLocation(), (System.Action) (() =>
          {
            if ((UnityEngine.Object) this != (UnityEngine.Object) null)
              this.Hide(true);
            SaveAndLoad.Save();
          }));
        }
        else
        {
          this.Hide();
          System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadDLCWorldMapAssets();
          GameManager.GetInstance().StartCoroutine((IEnumerator) UIManager.LoadAssets(task, (System.Action) (() =>
          {
            UIDLCMapMenuController mapMenuController3 = MonoSingleton<UIManager>.Instance.DLCWorldMapTemplate.Instantiate<UIDLCMapMenuController>();
            mapMenuController3.DebugFakeProgress = true;
            mapMenuController3.Show();
          })));
        }
      }));
    }
  }

  public async System.Threading.Tasks.Task ClearRot()
  {
    UIDLCMapMenuController mapMenuController = this;
    mapMenuController._rotSpriteSelector.SetForceLastSprite();
    List<DungeonWorldMapIcon> endGameLocations = new List<DungeonWorldMapIcon>();
    foreach (DungeonWorldMapIcon location in mapMenuController.Locations)
    {
      if (!location.IsEndGameNode)
        location.RemoveNode();
      else if (location.DungeonSide == UIDLCMapMenuController.DLCDungeonSide.OutsideMountain)
      {
        foreach (DLCMapConnection childConnection in location.ChildConnections)
          childConnection.HideAsync(0.0f, 0.0f);
        location.gameObject.SetActive(false);
        endGameLocations.Add(location);
      }
    }
    while (mapMenuController.IsShowing)
      await System.Threading.Tasks.Task.Yield();
    Ease ease1 = Ease.InOutQuad;
    mapMenuController._parallax.PanAsync(Vector2.up * 3f, 1f, ease1);
    await mapMenuController._parallax.ZoomAsync(0.5f, 1f, ease1);
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5));
    int heartBeatIterations = 2;
    float heartBeatDuration = 1.2f;
    for (int i = 0; i < heartBeatIterations; ++i)
    {
      mapMenuController.SpawnDisplacementRingAt(mapMenuController._rotMask.rectTransform, 0.5f);
      UIManager.PlayAudio("event:/ui/heartbeat");
      mapMenuController._parallax.ScreenShake(0.5f, 0.2f, 0.5f);
      await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds((double) heartBeatDuration));
    }
    UIManager.PlayAudio("event:/dlc/dungeon06/enemy/yngya/attack_phase02_projectile_circle_start");
    mapMenuController._parallax.ScreenRumble(0.05f, 0.5f, 15f);
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds((double) heartBeatDuration * 0.5));
    float duration = 2f;
    Ease ease2 = Ease.OutQuad;
    mapMenuController._rotMask.rectTransform.DOScale(Vector3.one, duration).From(0.0f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ease2).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    mapMenuController._rotMaskDistortion.rectTransform.DOScale(Vector3.one, duration).From(0.0f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ease2).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    DOTweenModuleUI.DOFade(mapMenuController._rotMaskDistortion, 0.0f, duration).From(0.1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(ease2).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.10000000149011612));
    UIManager.PlayAudio("event:/dlc/dungeon06/enemy/yngya/gethit_break_head_a");
    Ease ease3 = Ease.OutCubic;
    mapMenuController._parallax.ZoomAsync(0.0f, 0.8f, ease3);
    mapMenuController._parallax.PanAsync(Vector2.zero, 0.8f, ease3);
    mapMenuController._parallax.ScreenShake(2f, 0.2f, 2f);
    mapMenuController._parallax.ScreenRumble(0.05f, 0.5f, 15f);
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2.0));
    DungeonWorldMapIcon endGameDoorNode = endGameLocations.Find((Predicate<DungeonWorldMapIcon>) (loc => loc.Type == DungeonWorldMapIcon.NodeType.Door));
    if ((UnityEngine.Object) endGameDoorNode != (UnityEngine.Object) null)
    {
      endGameDoorNode.gameObject.SetActive(true);
      await mapMenuController.RevealLocation(endGameDoorNode, false);
    }
    DungeonWorldMapIcon dungeonWorldMapIcon = endGameDoorNode;
    DungeonWorldMapIcon location1 = dungeonWorldMapIcon != null ? dungeonWorldMapIcon.ChildNodes.FirstOrDefault<DungeonWorldMapIcon>() : (DungeonWorldMapIcon) null;
    if ((UnityEngine.Object) location1 != (UnityEngine.Object) null)
    {
      location1.gameObject.SetActive(true);
      await mapMenuController.RevealLocation(location1);
    }
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(3.0));
    endGameLocations = (List<DungeonWorldMapIcon>) null;
    endGameDoorNode = (DungeonWorldMapIcon) null;
  }

  public async System.Threading.Tasks.Task RevealLocation(
    DungeonWorldMapIcon location,
    bool revealConnection = true,
    bool revealNode = true,
    bool zoomReveal = true)
  {
    UIDLCMapMenuController mapMenuController = this;
    DLCMapConnection connection = location.GetParentConnection();
    if (revealConnection)
      connection.SetFill(0.0f);
    if (revealNode)
      location._canvasGroup.alpha = 0.0f;
    else
      location.Configure(location.Type, location.SelectAction, new DungeonWorldMapIcon.IconState?(DungeonWorldMapIcon.IconState.Preview), false);
    while (mapMenuController.IsShowing)
      await System.Threading.Tasks.Task.Yield();
    DungeonWorldMapIcon parent = location.Parent;
    if (parent != null)
    {
      Vector3 position1 = parent.transform.position;
    }
    Vector3 position2 = location.transform.position;
    if (revealConnection)
    {
      if (zoomReveal)
      {
        await mapMenuController._parallax.ZoomPanToRectAsync(parent.RectTransform, 3f, 1.5f, Ease.InOutCubic);
        mapMenuController._parallax.ZoomPanToRectAsync(location.RectTransform, 3f, 1.5f, Ease.InOutCubic);
      }
      UIManager.PlayAudio("event:/dlc/ui/map/reveal_portal_node");
      await connection.Reveal(0.0f, 0.7f);
    }
    else if (zoomReveal)
      await mapMenuController._parallax.ZoomPanToRectAsync(location.RectTransform, 3f, 1.5f, Ease.InOutCubic);
    if (revealConnection & revealNode)
      await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5));
    if (revealNode)
    {
      if (location.Type == DungeonWorldMapIcon.NodeType.Yngya)
        UIManager.PlayAudio("event:/dlc/music/map/yngya_node_reveal");
      UIManager.PlayAudio("event:/door/goop_door_unlock");
      UIManager.PlayAudio("event:/door/door_unlock");
      location._canvasGroup.alpha = 1f;
      location.Content.PopIn();
      mapMenuController.SpawnDisplacementRingAt(location.RectTransform);
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      mapMenuController._parallax.ScreenShake(2f, 0.2f, 2f);
      if (!zoomReveal)
      {
        connection = (DLCMapConnection) null;
      }
      else
      {
        mapMenuController._parallax.ZoomPanToRectAsync(location.RectTransform, 2f, 0.5f, Ease.OutCubic);
        connection = (DLCMapConnection) null;
      }
    }
    else
    {
      location.Configure(location.Type, location.SelectAction, new DungeonWorldMapIcon.IconState?(DungeonWorldMapIcon.IconState.Selectable), false);
      UIManager.PlayAudio("event:/door/door_unlock");
      UIManager.PlayAudio("event:/door/door_unlock");
      mapMenuController.SpawnDisplacementRingAt(location.RectTransform, 0.5f);
      mapMenuController._parallax.ScreenShake(2f, 0.2f, 2f);
      location.Content.Shake();
      connection = (DLCMapConnection) null;
    }
  }

  public async System.Threading.Tasks.Task UnlockLock(DungeonWorldMapIcon location)
  {
    UIDLCMapMenuController mapMenuController = this;
    while (mapMenuController.IsShowing)
      await System.Threading.Tasks.Task.Yield();
    Vector3 position = location.transform.position;
    UIManager.PlayAudio("event:/door/chain_break_sequence");
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5));
    location.Content.Shake();
    await mapMenuController._parallax.ScreenRumble(0.1f, 1f, 15f);
    mapMenuController._parallax.ScreenShake(1f, frequency: 1f);
    mapMenuController.SpawnDisplacementRingAt(location.RectTransform, 0.5f);
    mapMenuController.SpawnLockBreakParticlesAt(location.RectTransform);
    location.CompleteNode();
    mapMenuController.RefreshAllNodes();
    foreach (DLCMapConnection childConnection in location.ChildConnections)
      childConnection.TweenPath(1f, 0.0f, 0.0f, 0.0f);
    DLCMapConnection connection = location.GetParentConnection();
    connection.TweenPath(1f, 0.0f, 0.0f, 0.0f);
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.5));
    UIManager.PlayAudio("event:/dlc/ui/map/reveal_portal_node");
    connection.TweenPath(0.0f, 1f, 0.5f, 0.0f, connection.GetLineRendererForState(DLCMapConnection.ConnectionState.Selectable));
    foreach (DLCMapConnection childConnection in location.ChildConnections)
      await childConnection.TweenPath(0.0f, 1f, 0.5f, 0.5f, childConnection.GetLineRendererForState(DLCMapConnection.ConnectionState.Selectable));
    UIManager.PlayAudio("event:/door/goop_door_unlock");
    UIManager.PlayAudio("event:/door/door_unlock");
    foreach (DungeonWorldMapIcon childNode in location.ChildNodes)
    {
      childNode.Content.Punch();
      childNode.Configure(location.Type, location.SelectAction, new DungeonWorldMapIcon.IconState?(DungeonWorldMapIcon.IconState.Selectable), true);
    }
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2.0));
    connection = (DLCMapConnection) null;
  }

  public void RefreshAllNodes()
  {
    foreach (int key in DataManager.Instance.DLCDungeonNodesCompleted)
    {
      DungeonWorldMapIcon dungeonWorldMapIcon;
      if (this._nodeLookup.TryGetValue(key, out dungeonWorldMapIcon))
        dungeonWorldMapIcon.SetRuntimeComplete();
    }
    foreach (DungeonWorldMapIcon location in this.Locations)
    {
      if (location.IsLock && location.CurrentState == DungeonWorldMapIcon.IconState.Completed)
      {
        DLCMapConnection parentConnection = location.GetParentConnection();
        parentConnection.gameObject.SetActive(false);
        foreach (DLCMapConnection childConnection in location.ChildConnections)
        {
          childConnection.SetEndpoints(parentConnection.From, childConnection.To);
          childConnection.RefreshState(true);
        }
      }
    }
  }

  public async System.Threading.Tasks.Task HandleDoorway(DungeonWorldMapIcon location)
  {
    if (location.SelectAction != DungeonWorldMapIcon.IconActionType.Doorway)
      return;
    await this.RunDoorwayTransitionAsync(location);
  }

  public void CalculateDistances(
    DungeonWorldMapIcon node,
    int currentDistance,
    Dictionary<int, int> distances)
  {
    int id = node.ID;
    int num;
    bool flag = distances.TryGetValue(id, out num);
    if (flag && num <= currentDistance)
      return;
    if (!flag)
      distances.Add(id, currentDistance);
    else if (num > currentDistance)
      distances[id] = currentDistance;
    List<DungeonWorldMapIcon> neighbours;
    using (CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out neighbours))
    {
      node.GatherActiveNeighbours(neighbours);
      foreach (DungeonWorldMapIcon node1 in neighbours)
        this.CalculateDistances(node1, currentDistance + 1, distances);
    }
  }

  public async System.Threading.Tasks.Task MoveToPreviousNode(DungeonWorldMapIcon location)
  {
    DungeonWorldMapIcon previous = this._travelHistory.Count > 0 ? this._travelHistory.Pop() : (DungeonWorldMapIcon) null;
    if (!((UnityEngine.Object) previous != (UnityEngine.Object) null))
    {
      previous = (DungeonWorldMapIcon) null;
    }
    else
    {
      CrownMapMarker crownMapMarker = this._crownMapMarker;
      List<Vector3> path = new List<Vector3>();
      path.Add(location.RectTransform.position);
      path.Add(previous.RectTransform.position);
      DungeonWorldMapIcon targetNode = previous;
      await crownMapMarker.MoveToAsync(path, targetNode);
      previous.SetCurrentNode();
      previous = (DungeonWorldMapIcon) null;
    }
  }

  public System.Threading.Tasks.Task RunDoorwayTransitionAsync(DungeonWorldMapIcon location)
  {
    return this.RunDoorwayTransitionAsync(location, location.DoorwayDestination, this.HiddenDLCDungeonSide);
  }

  public async System.Threading.Tasks.Task RunDoorwayTransitionAsync(
    DungeonWorldMapIcon location,
    DungeonWorldMapIcon to,
    UIDLCMapMenuController.DLCDungeonSide layer)
  {
    int currentDlcDungeonSide = (int) this.CurrentDLCDungeonSide;
    this.CurrentDLCDungeonSide = layer;
    this.LockMenu();
    location.CompleteNode();
    to.SetCurrentNode();
    if (to.CurrentState != DungeonWorldMapIcon.IconState.Completed)
    {
      to.CompleteNode();
      to.SetRuntimeComplete();
    }
    this._backgroundSideTransitioner.SetCurrentLayer(this.CurrentDLCDungeonSide);
    float duration = 1.5f;
    this._crownMapMarker?.HideAsync();
    this._goopFade.gameObject.SetActive(true);
    this._goopFade.FadeIn(duration);
    if (currentDlcDungeonSide == 1)
      UIManager.PlayAudio("event:/dlc/ui/map/ewefall_exit");
    if (currentDlcDungeonSide == 0)
      UIManager.PlayAudio("event:/dlc/ui/map/therot_exit");
    this._parallax.PanAsync(new Vector2(0.0f, 4f), duration, Ease.InCubic);
    await this._parallax.ZoomAsync(2f, duration, Ease.InCubic);
    this._parallax.parallaxZoom = -0.4f;
    this._parallax.offset = Vector2.down * 2f;
    this._backgroundSideTransitioner.TransitionToLayer(this.CurrentDLCDungeonSide);
    if (layer == UIDLCMapMenuController.DLCDungeonSide.OutsideMountain)
      UIManager.PlayAudio("event:/dlc/ui/map/ewefall_enter");
    if (layer == UIDLCMapMenuController.DLCDungeonSide.InsideMountain)
      UIManager.PlayAudio("event:/dlc/ui/map/therot_enter");
    this._goopFade.FadeOut(duration);
    this._parallax.PanAsync(Vector2.zero, duration, Ease.OutCubic);
    await this._parallax.ZoomAsync(0.0f, duration, Ease.OutCubic);
    this.AutoSelectNextValidLocation();
    this.UnlockMenu();
  }

  public void OnLocationHighlighted(DungeonWorldMapIcon location)
  {
    if (location.IsKey || location.IsLock || location.Type == DungeonWorldMapIcon.NodeType.Reward)
    {
      this._locationTextCanvasGroup.DOKill();
      this._locationTextCanvasGroup.DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
    }
    else
    {
      this._locationHeader.GetComponent<TextAnimatorPlayer>().ShowText(location.GetLocalisedLocation());
      this._locationTextCanvasGroup.DOKill();
      this._locationTextCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
    }
    location.OnHighlight();
    this._crownMapMarker?.LookAt(location.RectTransform.position);
    this._crownMapMarker?.SetSelectedNode(location);
    this.FocusLocation((WorldMapIcon) location, 1f);
  }

  public void OnLocationDehighlighted(DungeonWorldMapIcon location) => location.OnDehighlight();

  public void FocusLocation(WorldMapIcon location, float time, float zoom = 0.25f, bool snap = false)
  {
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
    System.Action onCancel = this.OnCancel;
    if (onCancel == null)
      return;
    onCancel();
  }

  public void InitialiseConnections()
  {
    foreach (DungeonWorldMapIcon location in this.Locations)
    {
      if (!location.HasChildConnections)
      {
        foreach (DungeonWorldMapIcon childNode in location.ChildNodes)
          this.ConnectNodes(location, childNode);
        foreach (DungeonWorldMapIcon childNode in location.ChildNodes)
        {
          foreach (DLCMapConnection childConnection in childNode.ChildConnections)
            childConnection.RefreshState(true);
        }
      }
    }
    foreach (DungeonWorldMapIcon location in this.Locations)
    {
      if (location.IsLock && location.CurrentState == DungeonWorldMapIcon.IconState.Completed)
      {
        DLCMapConnection parentConnection = location.GetParentConnection();
        parentConnection.gameObject.SetActive(false);
        foreach (DLCMapConnection childConnection in location.ChildConnections)
        {
          childConnection.SetEndpoints(parentConnection.From, childConnection.To);
          childConnection.RefreshState(true);
        }
      }
    }
  }

  public void BuildNodeLookupTable()
  {
    this._nodeLookup?.Clear();
    foreach (DungeonWorldMapIcon location in this.Locations)
    {
      if (location.CompleteInstantly && !DataManager.Instance.DLCDungeonNodesCompleted.Contains(location.ID))
        DataManager.Instance.DLCDungeonNodesCompleted.Add(location.ID);
      this._nodeLookup.TryAdd(location.ID, location);
    }
  }

  public override void SetActiveStateForMenu(GameObject target, bool state)
  {
    foreach (Behaviour componentsInChild in target.GetComponentsInChildren<MMScrollRect>())
      componentsInChild.enabled = state;
  }

  [CanBeNull]
  public DungeonWorldMapIcon TryGetCurrentLocation()
  {
    return CollectionExtensions.GetValueOrDefault<int, DungeonWorldMapIcon>((IReadOnlyDictionary<int, DungeonWorldMapIcon>) this._nodeLookup, DataManager.Instance.DLCDungeonNodeCurrent);
  }

  public void InitialiseNodeStates()
  {
    foreach (DungeonWorldMapIcon location in this.Locations)
    {
      location.OnDehighlight();
      location.Configure(location.Type, location.SelectAction, new DungeonWorldMapIcon.IconState?(DungeonWorldMapIcon.IconState.None), false);
    }
    foreach (int key in DataManager.Instance.DLCDungeonNodesCompleted)
    {
      DungeonWorldMapIcon dungeonWorldMapIcon;
      if (this._nodeLookup.TryGetValue(key, out dungeonWorldMapIcon))
        dungeonWorldMapIcon.Configure(dungeonWorldMapIcon.Type, dungeonWorldMapIcon.SelectAction, new DungeonWorldMapIcon.IconState?(DungeonWorldMapIcon.IconState.Completed), false);
    }
    foreach (int key in DataManager.Instance.DLCDungeonNodesCompleted)
    {
      DungeonWorldMapIcon dungeonWorldMapIcon;
      if (this._nodeLookup.TryGetValue(key, out dungeonWorldMapIcon))
        dungeonWorldMapIcon.SetRuntimeComplete();
    }
  }

  public void InitHiddenNodes()
  {
    foreach (DungeonWorldMapIcon location in this.Locations)
    {
      if (DataManager.Instance.FinalDLCMap)
      {
        if (!location.IsEndGameNode)
          location.RemoveNode();
      }
      else if (location.IsEndGameNode)
        location.RemoveNode();
    }
    foreach (DungeonWorldMapIcon location in this.Locations)
    {
      if (location.UseShowVariableCondition && !DataManager.Instance.GetVariable(location.ShowIfVariable))
        location.RemoveNode();
    }
    List<DungeonWorldMapIcon> dungeonWorldMapIconList;
    using (CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out dungeonWorldMapIconList))
    {
      foreach (DungeonWorldMapIcon location in this.Locations)
      {
        if (location.SelectAction == DungeonWorldMapIcon.IconActionType.InstantCollect && location.IsBeaten && location.Type != DungeonWorldMapIcon.NodeType.Lock && location.Type != DungeonWorldMapIcon.NodeType.Lock_2 && location.Type != DungeonWorldMapIcon.NodeType.Lock_3)
          dungeonWorldMapIconList.Add(location);
      }
      foreach (DungeonWorldMapIcon dungeonWorldMapIcon in dungeonWorldMapIconList)
        dungeonWorldMapIcon.RemoveNode();
    }
  }

  public void InitialiseContent()
  {
    this.FixOldSaves();
    this.BuildNodeLookupTable();
    this.InitialiseMapSide();
    this.InitialiseCursor();
    this.InitialiseNodeStates();
    this.InitialiseConnections();
    this.RefreshConnectionStates();
    this.InitHiddenNodes();
  }

  public void FixOldSaves()
  {
    if (!DataManager.Instance.RevealedDLCMapDoor)
      return;
    DataManager.Instance.RevealedWolfNode = true;
  }

  public void InitialiseCursor()
  {
    DungeonWorldMapIcon locationForCurrentSide;
    if (!this._nodeLookup.TryGetValue(Mathf.Max(DataManager.Instance.DLCDungeonNodeCurrent, 0), out locationForCurrentSide))
    {
      UnityEngine.Debug.LogError((object) $"Could not find current node for current node id: {DataManager.Instance.DLCDungeonNodeCurrent}");
    }
    else
    {
      if (locationForCurrentSide.DungeonSide != this.CurrentDLCDungeonSide)
        locationForCurrentSide = this.BaseLocationForCurrentSide;
      locationForCurrentSide.SetCurrentNode();
    }
  }

  public void LockMenu()
  {
    if (this._activeLockCount <= 0)
    {
      this.SetActiveStateForMenu(false);
      this.RefreshConnectionStates();
      this._activeLockCount = 0;
      this._controlPrompts.HideAcceptButton();
      this._controlPrompts.HideCancelButton();
    }
    ++this._activeLockCount;
  }

  public void UnlockMenu()
  {
    --this._activeLockCount;
    if (this._activeLockCount == 0)
    {
      this._controlPrompts.ShowAcceptButton();
      this._controlPrompts.ShowCancelButton();
      this.SetActiveStateForMenu(true);
      this.RefreshConnectionStates();
    }
    this._activeLockCount = Mathf.Max(0, this._activeLockCount);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__1() => base.DoHideAnimation();

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__2() => base.DoHide();

  public enum DLCDungeonSide
  {
    InsideMountain,
    OutsideMountain,
  }
}
