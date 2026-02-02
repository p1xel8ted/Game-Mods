// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIUpgradeTreeMenuBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using src.Extensions;
using src.UINavigator;
using src.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public abstract class UIUpgradeTreeMenuBase<T> : UIMenuBase where T : UIUpgradeUnlockOverlayControllerBase
{
  public Action<UpgradeSystem.Type> OnUpgradeUnlocked;
  [Header("Upgrade Tree Menu")]
  [SerializeField]
  public UpgradeTreeConfiguration _configuration;
  [SerializeField]
  public AnimationCurve _focusCurve;
  [SerializeField]
  public RectTransform _treeContainer;
  [SerializeField]
  public RectTransform _nodeContainer;
  [SerializeField]
  public RectTransform _connectionContainer;
  [SerializeField]
  public RectTransform _tierLockContainer;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public RectTransform _rootViewport;
  [SerializeField]
  public RectTransform _nodeNameRectTransform;
  [SerializeField]
  public TextMeshProUGUI _nodeNameText;
  [Header("Cursor")]
  [SerializeField]
  public UpgradeMenuCursor _cursor;
  [Header("Node Tree")]
  [SerializeField]
  public Material _materialNode;
  [SerializeField]
  public UpgradeTreeNode _rootNode;
  [SerializeField]
  public List<UpgradeTreeNode> _treeNodes = new List<UpgradeTreeNode>();
  [SerializeField]
  public List<NodeConnectionLine> _nodeConnections = new List<NodeConnectionLine>();
  [SerializeField]
  public List<TierLockIcon> _tierLocks = new List<TierLockIcon>();
  [Header("Upgrade")]
  [SerializeField]
  public T _unlockOverlayTemplate;
  [Header("Points")]
  [SerializeField]
  public GameObject _pointsContainer;
  [SerializeField]
  public TextMeshProUGUI _pointsText;
  [Header("Effects")]
  [SerializeField]
  public UpgradeTreeComputeShaderController _computeShaderController;
  [SerializeField]
  public GoopFade _goopFade;
  public UpgradeSystem.Type revealType = UpgradeSystem.Type.Count;
  public Coroutine _focusCoroutine;
  public GameObject disableBackPrompt;
  public float zoom = 100f;
  public bool _didCancel;
  public bool _isCancelLocked;
  public bool _didUpgraded;
  public Camera currentMain;
  public float previousClipPlane;

  public List<NodeConnectionLine> NodeConnections => this._nodeConnections;

  public UpgradeSystem.Type RevealType => this.revealType;

  public override void Awake()
  {
    base.Awake();
    this.Configure();
    this._canvasGroup.alpha = 0.0f;
  }

  public void Configure()
  {
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
      treeNode.Configured = false;
    this._rootNode.Configure(this.TreeTier());
    foreach (TierLockIcon tierLock in this._tierLocks)
      tierLock.Configure(this.TreeTier());
  }

  public override void OnShowStarted()
  {
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
      BiomeConstants.Instance.GoopFadeOut(1f, UseDeltaTime: false);
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
    {
      treeNode.OnUpgradeNodeSelected -= new Action<UpgradeTreeNode>(this.OnNodeSelected);
      treeNode.OnUpgradeNodeSelected += new Action<UpgradeTreeNode>(this.OnNodeSelected);
    }
    Selectable selectable = (Selectable) null;
    UpgradeSystem.Type[] allCentralUpgrades = this._configuration.GetAllCentralUpgrades();
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
    {
      if ((treeNode.State == UpgradeTreeNode.NodeState.Available || treeNode.State == UpgradeTreeNode.NodeState.Unavailable) && allCentralUpgrades.Contains<UpgradeSystem.Type>(treeNode.Upgrade))
        selectable = (Selectable) treeNode.Button;
    }
    if ((UnityEngine.Object) selectable == (UnityEngine.Object) null)
      selectable = this.GetDefaultSelectable();
    if ((UnityEngine.Object) selectable != (UnityEngine.Object) null)
    {
      this.OverrideDefault(selectable);
      this._treeContainer.anchoredPosition = -selectable.GetComponent<RectTransform>().anchoredPosition;
      this._cursor.RectTransform.anchoredPosition = selectable.GetComponent<RectTransform>().anchoredPosition;
      this._cursor.RectTransform.localScale = selectable.transform.localScale;
    }
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
      treeNode.UpdateAnimationLayerStates();
    this.UpdatePointsText();
  }

  public void Show(UpgradeSystem.Type revealType)
  {
    this.revealType = revealType;
    this.Show();
  }

  public abstract Selectable GetDefaultSelectable();

  public override void OnShowCompleted()
  {
    this.ActivateNavigation();
    if (this.revealType == UpgradeSystem.Type.Count)
      return;
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
    {
      if (!treeNode.IsAvailable() && treeNode.RequiresUpgrade == this.revealType)
      {
        UpgradeSystem.UnlockAbility(this.revealType);
        this.StartCoroutine((IEnumerator) this.DoRevealAnimation(this.revealType));
        break;
      }
    }
  }

  public void OnEnable()
  {
    this._cursor.OnAtRest += new System.Action(this.OnCursorAtRest);
    this._cursor.OnCursorMoved += new Action<Vector2>(this.OnCursorMoved);
    MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
  }

  public new void OnDisable()
  {
    this._cursor.OnAtRest -= new System.Action(this.OnCursorAtRest);
    this._cursor.OnCursorMoved -= new Action<Vector2>(this.OnCursorMoved);
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
  }

  public void OnCursorMoved(Vector2 movement)
  {
    this._scrollRect.content.anchoredPosition = this._scrollRect.content.anchoredPosition + movement;
  }

  public void OnCursorAtRest()
  {
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null)
      return;
    this.OnSelection(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
  }

  public void OnSelection(Selectable selectable)
  {
    UpgradeTreeNode component;
    if (InputManager.General.MouseInputActive || !selectable.TryGetComponent<UpgradeTreeNode>(out component))
      return;
    if (this._focusCoroutine != null)
      this.StopCoroutine(this._focusCoroutine);
    this._focusCoroutine = this.StartCoroutine((IEnumerator) this.DoFocusPosition(-component.RectTransform.anchoredPosition, 0.25f));
  }

  public IEnumerator DoFocusPosition(Vector2 focalPoint, float time, float zoom = 1f)
  {
    Rect rect = this._treeContainer.rect;
    double width1 = (double) rect.width;
    rect = this._rootViewport.rect;
    double width2 = (double) rect.width;
    float max1 = (float) ((width1 - width2) * 0.5);
    rect = this._treeContainer.rect;
    double height1 = (double) rect.height;
    rect = this._rootViewport.rect;
    double height2 = (double) rect.height;
    float max2 = (float) ((height1 - height2) * 0.5);
    focalPoint.x = Mathf.Clamp(focalPoint.x, -max1, max1);
    focalPoint.y = Mathf.Clamp(focalPoint.y, -max2, max2);
    Vector2 currentPosition = this._treeContainer.anchoredPosition;
    Vector2 currentZoom = (Vector2) this._rootViewport.localScale;
    Vector2 targetZoom = Vector2.one * zoom;
    float t = 0.0f;
    while ((double) t <= (double) time)
    {
      t += Time.unscaledDeltaTime;
      this._rootViewport.localScale = (Vector3) Vector2.Lerp(currentZoom, targetZoom, this._focusCurve.Evaluate(t / time));
      this._treeContainer.anchoredPosition = Vector2.Lerp(currentPosition, focalPoint, this._focusCurve.Evaluate(t / time));
      yield return (object) null;
    }
  }

  public virtual void OnNodeSelected(UpgradeTreeNode node)
  {
    if (CheatConsole.QuickUnlock)
    {
      this.PerformUnlock(node);
    }
    else
    {
      this._cursor.LockPosition = true;
      bool didUnlock = false;
      T upgradeOverlayInstance = this._unlockOverlayTemplate.Instantiate<T>();
      this.PushInstance<T>(upgradeOverlayInstance);
      upgradeOverlayInstance.Show(node, false);
      // ISSUE: variable of a boxed type
      __Boxed<T> local1 = (object) upgradeOverlayInstance;
      local1.OnUnlocked = local1.OnUnlocked + (System.Action) (() => didUnlock = true);
      // ISSUE: variable of a boxed type
      __Boxed<T> local2 = (object) upgradeOverlayInstance;
      local2.OnCancel = local2.OnCancel + (System.Action) (() => { });
      // ISSUE: variable of a boxed type
      __Boxed<T> local3 = (object) upgradeOverlayInstance;
      local3.OnShow = local3.OnShow + (System.Action) (() =>
      {
        if (!((UnityEngine.Object) this.disableBackPrompt != (UnityEngine.Object) null))
          return;
        this.disableBackPrompt.gameObject.SetActive(true);
      });
      // ISSUE: variable of a boxed type
      __Boxed<T> local4 = (object) upgradeOverlayInstance;
      local4.OnHidden = local4.OnHidden + (System.Action) (() =>
      {
        if ((UnityEngine.Object) this.disableBackPrompt != (UnityEngine.Object) null)
          this.disableBackPrompt.gameObject.SetActive(false);
        if (didUnlock)
        {
          this.PerformUnlock(node);
        }
        else
        {
          this._cursor.LockPosition = false;
          this.SetActiveStateForMenu(true);
        }
        upgradeOverlayInstance = default (T);
      });
    }
  }

  public abstract int UpgradePoints();

  public abstract void DoUnlock(UpgradeSystem.Type upgrade);

  public abstract UpgradeTreeNode.TreeTier TreeTier();

  public abstract void UpdateTier(UpgradeTreeNode.TreeTier tier);

  public abstract string GetPointsText();

  public virtual void UpdatePointsText()
  {
    if (!((UnityEngine.Object) this._pointsText != (UnityEngine.Object) null))
      return;
    this._pointsText.text = this.GetPointsText();
  }

  public void PerformUnlock(UpgradeTreeNode node)
  {
    Debug.Log((object) $"Unlock {node.Upgrade}!".Colour(Color.red));
    this.DoUnlock(node.Upgrade);
    this.UpdatePointsText();
    this.StartCoroutine((IEnumerator) this.DoUnlockAnimation(node));
  }

  public virtual IEnumerator DoUnlockAnimation(UpgradeTreeNode targetNode)
  {
    UIUpgradeTreeMenuBase<T> upgradeTreeMenuBase1 = this;
    List<UpgradeTreeNode> changedNodes = new List<UpgradeTreeNode>();
    float zoom = 1f;
    bool unlockedNewTier = false;
    bool unlockedCentralNode = false;
    upgradeTreeMenuBase1.OverrideDefault((Selectable) targetNode.Button);
    upgradeTreeMenuBase1.SetActiveStateForMenu(false);
    upgradeTreeMenuBase1._cursor.enabled = false;
    upgradeTreeMenuBase1._cursor.CanvasGroup.DOFade(0.0f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    upgradeTreeMenuBase1._scrollRect.enabled = false;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    targetNode.Configure(upgradeTreeMenuBase1.TreeTier(), true);
    foreach (UpgradeTreeNode nodeConnection in targetNode.NodeConnections)
    {
      if (nodeConnection.NodeTier <= upgradeTreeMenuBase1.TreeTier())
        changedNodes.Add(nodeConnection);
    }
    Vector2 anchoredPosition = targetNode.RectTransform.anchoredPosition;
    if (targetNode.PrerequisiteNodes != null && targetNode.PrerequisiteNodes.Length != 0)
    {
      UpgradeTreeNode prerequisiteNode = targetNode.PrerequisiteNodes[0];
      UIUpgradeTreeMenuBase<T> upgradeTreeMenuBase2 = upgradeTreeMenuBase1;
      List<UpgradeTreeNode> nodes = new List<UpgradeTreeNode>();
      nodes.Add(prerequisiteNode);
      nodes.Add(targetNode);
      ref Vector2 local1 = ref anchoredPosition;
      ref float local2 = ref zoom;
      upgradeTreeMenuBase2.DetermineFocalPointAndZoom(nodes, out local1, out local2);
    }
    yield return (object) upgradeTreeMenuBase1.DoFocusPosition(-anchoredPosition, 0.25f, zoom);
    List<NodeConnectionLine> connectionLines = new List<NodeConnectionLine>();
    foreach (NodeConnectionLine nodeConnection in upgradeTreeMenuBase1._nodeConnections)
    {
      if (nodeConnection.Nodes.Contains(targetNode))
      {
        connectionLines.Add(nodeConnection);
        nodeConnection.PerformLineAnimation();
      }
    }
    if (connectionLines.Count > 0)
      yield return (object) upgradeTreeMenuBase1.YieldForConnections(connectionLines);
    else
      yield return (object) targetNode.DoUpdateStateAnimation();
    yield return (object) new WaitForSecondsRealtime(0.1f);
    zoom = 1f;
    foreach (UpgradeTreeNode treeNode in upgradeTreeMenuBase1._treeNodes)
      treeNode.OnStateDidChange += (Action<UpgradeTreeNode>) (changedNode =>
      {
        if (changedNodes.Contains(changedNode))
          return;
        changedNodes.Add(changedNode);
      });
    UpgradeTreeNode.TreeTier tier1 = upgradeTreeMenuBase1.TreeTier() + 1;
    UpgradeTreeConfiguration.TreeTierConfig configForTier = upgradeTreeMenuBase1._configuration.GetConfigForTier(tier1);
    if (configForTier != null)
    {
      int num1 = upgradeTreeMenuBase1._configuration.NumRequiredNodesForTier(tier1);
      int num2 = upgradeTreeMenuBase1._configuration.NumUnlockedUpgrades();
      Debug.Log((object) $"{tier1 - 1} - {num2}/{num1}".Colour(Color.yellow));
      if (num2 >= num1)
      {
        if (!configForTier.RequiresCentralTier || targetNode.Upgrade == configForTier.CentralNode)
        {
          unlockedNewTier = true;
          upgradeTreeMenuBase1.UpdateTier(tier1);
          upgradeTreeMenuBase1._rootNode.Configure(upgradeTreeMenuBase1.TreeTier(), true);
        }
        else
        {
          foreach (UpgradeTreeNode treeNode in upgradeTreeMenuBase1._treeNodes)
          {
            if (treeNode.Upgrade == configForTier.CentralNode && treeNode.State == UpgradeTreeNode.NodeState.Locked)
            {
              treeNode.Configure(upgradeTreeMenuBase1.TreeTier(), true);
              break;
            }
          }
        }
      }
    }
    foreach (UpgradeTreeNode treeNode in upgradeTreeMenuBase1._treeNodes)
      treeNode.OnStateDidChange -= (Action<UpgradeTreeNode>) (changedNode =>
      {
        if (changedNodes.Contains(changedNode))
          return;
        changedNodes.Add(changedNode);
      });
    TierLockIcon targetTierLock = (TierLockIcon) null;
    if (unlockedNewTier)
    {
      foreach (TierLockIcon tierLock in upgradeTreeMenuBase1._tierLocks)
      {
        if (tierLock.Tier == upgradeTreeMenuBase1.TreeTier())
        {
          targetTierLock = tierLock;
          break;
        }
      }
    }
    else
    {
      UpgradeTreeNode.TreeTier[] values = Enum.GetValues(typeof (UpgradeTreeNode.TreeTier)) as UpgradeTreeNode.TreeTier[];
      int num3 = upgradeTreeMenuBase1._configuration.NumUnlockedUpgrades();
      foreach (UpgradeTreeNode.TreeTier tier2 in values)
      {
        int num4 = upgradeTreeMenuBase1._configuration.NumRequiredNodesForTier(tier2);
        if (num3 == num4)
        {
          unlockedCentralNode = true;
          using (List<TierLockIcon>.Enumerator enumerator = upgradeTreeMenuBase1._tierLocks.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              TierLockIcon current = enumerator.Current;
              if (current.Tier == tier2)
              {
                targetTierLock = current;
                break;
              }
            }
            break;
          }
        }
      }
    }
    Vector2 focalPoint;
    if (unlockedCentralNode)
    {
      yield return (object) upgradeTreeMenuBase1.DoFocusPosition(-targetTierLock.RectTransform.anchoredPosition, 0.25f, zoom);
      yield return (object) targetTierLock.DestroyTierLock();
      yield return (object) new WaitForSecondsRealtime(0.1f);
      if (targetTierLock.Tier - upgradeTreeMenuBase1.TreeTier() == 1 && changedNodes.Count > 0)
      {
        UpgradeTreeNode upgradeTreeNode = changedNodes.Find((Predicate<UpgradeTreeNode>) (x => x.State != 0));
        focalPoint = (UnityEngine.Object) upgradeTreeNode != (UnityEngine.Object) null ? upgradeTreeNode.RectTransform.anchoredPosition : targetNode.RectTransform.anchoredPosition;
      }
      else
        focalPoint = targetNode.RectTransform.anchoredPosition;
    }
    else if (unlockedNewTier)
    {
      List<UpgradeTreeNode> nodes = new List<UpgradeTreeNode>();
      foreach (UpgradeTreeNode treeNode in upgradeTreeMenuBase1._treeNodes)
      {
        if (treeNode.NodeTier == upgradeTreeMenuBase1.TreeTier())
          nodes.Add(treeNode);
      }
      upgradeTreeMenuBase1.DetermineFocalPointAndZoom(nodes, out focalPoint, out zoom);
    }
    else
      upgradeTreeMenuBase1.DetermineFocalPointAndZoom(new List<UpgradeTreeNode>((IEnumerable<UpgradeTreeNode>) changedNodes)
      {
        targetNode
      }, out focalPoint, out zoom);
    yield return (object) upgradeTreeMenuBase1.DoFocusPosition(-focalPoint, 0.25f, zoom);
    if (unlockedNewTier)
      yield return (object) targetTierLock.RevealTier();
    foreach (NodeConnectionLine nodeConnection in upgradeTreeMenuBase1._nodeConnections)
    {
      if (nodeConnection.IsDirty)
        nodeConnection.PerformLineAnimation();
    }
    yield return (object) upgradeTreeMenuBase1.YieldForConnections(upgradeTreeMenuBase1._nodeConnections);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    if ((double) zoom < 1.0)
      yield return (object) upgradeTreeMenuBase1.DoFocusPosition(-targetNode.RectTransform.anchoredPosition, 0.25f);
    upgradeTreeMenuBase1.OnUnlockAnimationCompleted();
    Action<UpgradeSystem.Type> onUpgradeUnlocked = upgradeTreeMenuBase1.OnUpgradeUnlocked;
    if (onUpgradeUnlocked != null)
      onUpgradeUnlocked(targetNode.Upgrade);
    UpgradeTreeNode upgradeTreeNode1 = changedNodes.Find((Predicate<UpgradeTreeNode>) (x => x.State != 0));
    upgradeTreeMenuBase1._cursor.RectTransform.anchoredPosition = !unlockedCentralNode || !((UnityEngine.Object) upgradeTreeNode1 != (UnityEngine.Object) null) ? targetNode.RectTransform.anchoredPosition : upgradeTreeNode1.RectTransform.anchoredPosition;
  }

  public virtual IEnumerator DoRevealAnimation(
    UpgradeSystem.Type typeToReveal,
    float delayHideDuration = 2f,
    bool hideOnComplete = true)
  {
    UIUpgradeTreeMenuBase<T> upgradeTreeMenuBase = this;
    List<UpgradeTreeNode> changedNodes = new List<UpgradeTreeNode>();
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    foreach (UpgradeTreeNode treeNode in upgradeTreeMenuBase._treeNodes)
    {
      if (treeNode.RequiresUpgrade == typeToReveal)
        changedNodes.Add(treeNode);
    }
    UpgradeTreeNode targetNode = changedNodes[0];
    if (typeToReveal == UpgradeSystem.Type.WinterSystem)
    {
      foreach (UpgradeTreeNode upgradeTreeNode in changedNodes)
      {
        if (upgradeTreeNode.Upgrade == UpgradeSystem.Type.Building_Furnace_2)
          targetNode = upgradeTreeNode;
      }
    }
    upgradeTreeMenuBase.OverrideDefault((Selectable) targetNode.Button);
    upgradeTreeMenuBase.SetActiveStateForMenu(false);
    upgradeTreeMenuBase._cursor.enabled = false;
    upgradeTreeMenuBase._cursor.CanvasGroup.DOFade(0.0f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    upgradeTreeMenuBase._scrollRect.enabled = false;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    foreach (UpgradeTreeNode upgradeTreeNode in changedNodes)
      upgradeTreeNode.Configure(upgradeTreeMenuBase.TreeTier(), true);
    foreach (UpgradeTreeNode nodeConnection in targetNode.NodeConnections)
    {
      if (nodeConnection.NodeTier <= upgradeTreeMenuBase.TreeTier())
        changedNodes.Add(nodeConnection);
    }
    Vector2 focalPoint = targetNode.RectTransform.anchoredPosition;
    float zoom = 0.85f;
    if (targetNode.PrerequisiteNodes != null && targetNode.PrerequisiteNodes.Length != 0)
    {
      UpgradeTreeNode prerequisiteNode = targetNode.PrerequisiteNodes[0];
      upgradeTreeMenuBase.DetermineFocalPointAndZoom(new List<UpgradeTreeNode>((IEnumerable<UpgradeTreeNode>) changedNodes)
      {
        prerequisiteNode
      }, out focalPoint, out zoom);
    }
    if (typeToReveal == UpgradeSystem.Type.WinterSystem)
      zoom /= 1.5f;
    UIManager.PlayAudio("event:/ui/map_location_pan");
    yield return (object) upgradeTreeMenuBase.DoFocusPosition(-focalPoint, 1f, zoom);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    UIManager.PlayAudio("event:/unlock_building/unlock");
    foreach (UpgradeTreeNode upgradeTreeNode in changedNodes)
    {
      upgradeTreeNode.ShowNode(hideOnComplete);
      upgradeTreeMenuBase.StartCoroutine((IEnumerator) upgradeTreeNode.DoUpdateStateAnimation());
      foreach (NodeConnectionLine nodeConnection in upgradeTreeMenuBase._nodeConnections)
      {
        nodeConnection.ClearListeners();
        nodeConnection.Start();
      }
    }
    yield return (object) new WaitForSecondsRealtime(delayHideDuration);
    if (hideOnComplete)
    {
      upgradeTreeMenuBase.Hide();
    }
    else
    {
      yield return (object) upgradeTreeMenuBase.DoFocusPosition(-targetNode.RectTransform.anchoredPosition, 0.4f);
      upgradeTreeMenuBase.SetActiveStateForMenu(true);
      upgradeTreeMenuBase._cursor.NavigateTo((Selectable) targetNode.Button);
      upgradeTreeMenuBase._cursor.enabled = true;
      upgradeTreeMenuBase._cursor.CanvasGroup.DOFade(1f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      upgradeTreeMenuBase._scrollRect.enabled = true;
    }
  }

  public void DetermineFocalPointAndZoom(
    List<UpgradeTreeNode> nodes,
    out Vector2 focalPoint,
    out float zoom)
  {
    zoom = 1f;
    List<UpgradeTreeNode> upgradeTreeNodeList = new List<UpgradeTreeNode>();
    for (int index = 0; index < nodes.Count; ++index)
    {
      if (nodes[index].State != UpgradeTreeNode.NodeState.Hidden)
        upgradeTreeNodeList.Add(nodes[index]);
    }
    Bounds bounds = new Bounds((Vector3) upgradeTreeNodeList[0].RectTransform.anchoredPosition, Vector3.zero);
    foreach (UpgradeTreeNode upgradeTreeNode in upgradeTreeNodeList)
      bounds.Encapsulate((Vector3) upgradeTreeNode.RectTransform.anchoredPosition);
    focalPoint = (Vector2) bounds.center;
    float num1 = 250f;
    float num2 = float.MaxValue;
    if ((double) bounds.size.x > 1920.0)
      num2 = (float) (1920.0 / ((double) bounds.size.x + (double) num1));
    float num3 = 400f;
    float num4 = float.MaxValue;
    if ((double) bounds.size.y > 1080.0)
      num4 = (float) (1080.0 / ((double) bounds.size.y + (double) num3));
    zoom = Mathf.Clamp((double) num2 < (double) num4 ? num2 : num4, 0.0f, 1f);
  }

  public IEnumerator YieldForConnections(List<NodeConnectionLine> connectionLines)
  {
    bool completed = false;
    while (!completed)
    {
      completed = true;
      yield return (object) null;
      foreach (NodeConnectionLine connectionLine in connectionLines)
      {
        if (connectionLine.IsDirty)
        {
          completed = false;
          break;
        }
      }
    }
  }

  public void Update() => this.ZoomUpdate();

  public void ZoomUpdate()
  {
    if (!this._scrollRect.enabled)
      return;
    this.OnZoomChanged(InputManager.PhotoMode.GetFocusAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
  }

  public void OnZoomChanged(float zoomAxis)
  {
    zoomAxis *= (float) ((double) Time.unscaledDeltaTime * 2.0 * 100.0);
    this.zoom += zoomAxis;
    this.zoom = Mathf.Clamp(this.zoom, 0.0f, 100f);
    this._scrollRect.transform.localScale = Vector3.Lerp(Vector3.one / 2f, Vector3.one, this.zoom / 100f);
  }

  public virtual void OnUnlockAnimationCompleted()
  {
    this.SetActiveStateForMenu(true);
    this._scrollRect.enabled = true;
    this._cursor.RectTransform.anchoredPosition = -this._treeContainer.anchoredPosition;
    this._cursor.CanvasGroup.DOFade(1f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._cursor.RectTransform.localScale = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null ? MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.transform.localScale : Vector3.one));
    this._cursor.enabled = true;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || this._isCancelLocked || this.IsShowing)
      return;
    if (!this._didUpgraded)
      this._didCancel = true;
    this.Hide();
  }

  public override void OnHideStarted()
  {
    DOTween.KillAll();
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
      treeNode.OnUpgradeNodeSelected -= new Action<UpgradeTreeNode>(this.OnNodeSelected);
    MonoSingleton<MMLogger>.Instance.AddToLog("HIDE STARTED 000001");
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
      BiomeConstants.Instance.GoopFadeOut(1f, UseDeltaTime: false);
    this._cursor.enabled = false;
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_statue_close");
  }

  public override void OnHideCompleted()
  {
    DOTween.KillAll();
    MonoSingleton<MMLogger>.Instance.AddToLog("HIDE ENDED 000002");
    AudioManager.Instance.ResumePausedLoopsAndSFX();
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    MonoSingleton<MMLogger>.Instance.AddToLog("HIDE ENDED 000003");
  }

  public void SetLockCancelInput(bool acitve) => this._isCancelLocked = acitve;

  [CompilerGenerated]
  public void \u003COnUnlockAnimationCompleted\u003Eb__57_0()
  {
    this._cursor.RectTransform.localScale = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null ? MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.transform.localScale : Vector3.one;
  }
}
