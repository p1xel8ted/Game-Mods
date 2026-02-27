// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIUpgradeTreeMenuBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
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
  private UpgradeTreeConfiguration _configuration;
  [SerializeField]
  private AnimationCurve _focusCurve;
  [SerializeField]
  private RectTransform _treeContainer;
  [SerializeField]
  private RectTransform _nodeContainer;
  [SerializeField]
  private RectTransform _connectionContainer;
  [SerializeField]
  private RectTransform _tierLockContainer;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private RectTransform _rootViewport;
  [SerializeField]
  private RectTransform _nodeNameRectTransform;
  [SerializeField]
  private TextMeshProUGUI _nodeNameText;
  [Header("Cursor")]
  [SerializeField]
  protected UpgradeMenuCursor _cursor;
  [Header("Node Tree")]
  [SerializeField]
  private Material _materialNode;
  [SerializeField]
  private UpgradeTreeNode _rootNode;
  [SerializeField]
  protected List<UpgradeTreeNode> _treeNodes = new List<UpgradeTreeNode>();
  [SerializeField]
  private List<NodeConnectionLine> _nodeConnections = new List<NodeConnectionLine>();
  [SerializeField]
  private List<TierLockIcon> _tierLocks = new List<TierLockIcon>();
  [Header("Upgrade")]
  [SerializeField]
  private T _unlockOverlayTemplate;
  [Header("Points")]
  [SerializeField]
  private GameObject _pointsContainer;
  [SerializeField]
  protected TextMeshProUGUI _pointsText;
  [Header("Effects")]
  [SerializeField]
  private UpgradeTreeComputeShaderController _computeShaderController;
  [SerializeField]
  private GoopFade _goopFade;
  private Coroutine _focusCoroutine;
  public GameObject disableBackPrompt;
  private bool _didCancel;
  public bool _didUpgraded;
  private Camera currentMain;
  private float previousClipPlane;

  public override void Awake()
  {
    base.Awake();
    this._rootNode.Configure(this.TreeTier());
    foreach (TierLockIcon tierLock in this._tierLocks)
      tierLock.Configure(this.TreeTier());
    this._canvasGroup.alpha = 0.0f;
  }

  protected override void OnShowStarted()
  {
    AudioManager.Instance.PauseActiveLoops();
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
      BiomeConstants.Instance.GoopFadeOut(1f, UseDeltaTime: false);
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
      treeNode.OnUpgradeNodeSelected += new Action<UpgradeTreeNode>(this.OnNodeSelected);
    Selectable defaultSelectable = this.GetDefaultSelectable();
    if ((UnityEngine.Object) defaultSelectable != (UnityEngine.Object) null)
    {
      this.OverrideDefault(defaultSelectable);
      this._treeContainer.anchoredPosition = -defaultSelectable.GetComponent<RectTransform>().anchoredPosition;
      this._cursor.RectTransform.anchoredPosition = defaultSelectable.GetComponent<RectTransform>().anchoredPosition;
      this._cursor.RectTransform.localScale = defaultSelectable.transform.localScale;
    }
    this.UpdatePointsText();
  }

  protected abstract Selectable GetDefaultSelectable();

  protected override void OnShowCompleted() => this.ActivateNavigation();

  private void OnEnable()
  {
    this._cursor.OnAtRest += new System.Action(this.OnCursorAtRest);
    this._cursor.OnCursorMoved += new Action<Vector2>(this.OnCursorMoved);
    MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
  }

  private void OnDisable()
  {
    this._cursor.OnAtRest -= new System.Action(this.OnCursorAtRest);
    this._cursor.OnCursorMoved -= new Action<Vector2>(this.OnCursorMoved);
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
  }

  private void OnCursorMoved(Vector2 movement)
  {
    this._scrollRect.content.anchoredPosition = this._scrollRect.content.anchoredPosition + movement;
  }

  private void OnCursorAtRest()
  {
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null)
      return;
    this.OnSelection(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
  }

  private void OnSelection(Selectable selectable)
  {
    UpgradeTreeNode component;
    if (InputManager.General.MouseInputActive || !selectable.TryGetComponent<UpgradeTreeNode>(out component))
      return;
    if (this._focusCoroutine != null)
      this.StopCoroutine(this._focusCoroutine);
    this._focusCoroutine = this.StartCoroutine((IEnumerator) this.DoFocusPosition(-component.RectTransform.anchoredPosition, 0.25f));
  }

  private IEnumerator DoFocusPosition(Vector2 focalPoint, float time, float zoom = 1f)
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

  private void OnNodeSelected(UpgradeTreeNode node)
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

  protected abstract int UpgradePoints();

  protected abstract void DoUnlock(UpgradeSystem.Type upgrade);

  protected abstract UpgradeTreeNode.TreeTier TreeTier();

  protected abstract void UpdateTier(UpgradeTreeNode.TreeTier tier);

  protected abstract string GetPointsText();

  protected virtual void UpdatePointsText()
  {
    if (!((UnityEngine.Object) this._pointsText != (UnityEngine.Object) null))
      return;
    this._pointsText.text = this.GetPointsText();
  }

  private void PerformUnlock(UpgradeTreeNode node)
  {
    Debug.Log((object) $"Unlock {node.Upgrade}!".Colour(Color.red));
    this.DoUnlock(node.Upgrade);
    this.UpdatePointsText();
    this.StartCoroutine((IEnumerator) this.DoUnlockAnimation(node));
  }

  private IEnumerator DoUnlockAnimation(UpgradeTreeNode targetNode)
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
      treeNode.OnStateDidChange += new Action<UpgradeTreeNode>(StateChanged);
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
      treeNode.OnStateDidChange -= new Action<UpgradeTreeNode>(StateChanged);
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
      focalPoint = targetTierLock.Tier - upgradeTreeMenuBase1.TreeTier() != 1 || changedNodes.Count <= 0 ? targetNode.RectTransform.anchoredPosition : changedNodes[0].RectTransform.anchoredPosition;
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

    void StateChanged(UpgradeTreeNode changedNode)
    {
      if (changedNodes.Contains(changedNode))
        return;
      changedNodes.Add(changedNode);
    }
  }

  private void DetermineFocalPointAndZoom(
    List<UpgradeTreeNode> nodes,
    out Vector2 focalPoint,
    out float zoom)
  {
    zoom = 1f;
    Bounds bounds = new Bounds((Vector3) nodes[0].RectTransform.anchoredPosition, Vector3.zero);
    foreach (UpgradeTreeNode node in nodes)
      bounds.Encapsulate((Vector3) node.RectTransform.anchoredPosition);
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

  private IEnumerator YieldForConnections(List<NodeConnectionLine> connectionLines)
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

  protected virtual void OnUnlockAnimationCompleted()
  {
    this.SetActiveStateForMenu(true);
    this._scrollRect.enabled = true;
    this._cursor.RectTransform.anchoredPosition = -this._treeContainer.anchoredPosition;
    this._cursor.CanvasGroup.DOFade(1f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._cursor.RectTransform.localScale = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null ? MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.transform.localScale : Vector3.one));
    this._cursor.enabled = true;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || this.IsShowing)
      return;
    if (!this._didUpgraded)
      this._didCancel = true;
    this.Hide();
  }

  protected override void OnHideStarted()
  {
    AudioManager.Instance.ResumeActiveLoops();
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
      BiomeConstants.Instance.GoopFadeOut(1f, UseDeltaTime: false);
    this._cursor.enabled = false;
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_statue_close");
  }

  protected override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
