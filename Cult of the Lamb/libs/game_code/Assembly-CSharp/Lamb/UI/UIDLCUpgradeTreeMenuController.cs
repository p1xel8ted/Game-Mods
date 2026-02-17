// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDLCUpgradeTreeMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIDLCUpgradeTreeMenuController : UIUpgradeTreeMenuController
{
  [SerializeField]
  public Image snow;
  [SerializeField]
  public GameObject[] topPrompts;
  [SerializeField]
  public GameObject backPrompt;
  public UIUpgradeTreeMenuController _baseTree;
  public bool _animateSnow;
  public string upgradeLayerRevealedSFX = "event:/dlc/ui/divinetree/woolhaven_upgrade_layer_reveal";
  public string upgradeLayerCoveredUpSFX = "event:/dlc/ui/divinetree/woolhaven_upgrade_layer_coverup";
  public const float tier1TreeReveal = 0.37f;
  public const float tier2TreeReveal = 0.455f;
  public const float tier3TreeReveal = 0.65f;

  public void Show(UIUpgradeTreeMenuController baseTree, bool animateSnow)
  {
    this._baseTree = baseTree;
    this._animateSnow = animateSnow;
    this.Show(this._baseTree.RevealType);
    if (this._baseTree.RevealType == UpgradeSystem.Type.Count)
      return;
    this.DisableControlPrompts();
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    Shader.SetGlobalFloat("_DLCTreeReveal", 1f);
    if (DataManager.Instance.DLCUpgradeTreeSnowIncrement == 0 && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem))
      Shader.SetGlobalFloat("_DLCTreeReveal", 0.37f);
    else if (DataManager.Instance.DLCUpgradeTreeSnowIncrement == 1)
      Shader.SetGlobalFloat("_DLCTreeReveal", 0.455f);
    else if (DataManager.Instance.DLCUpgradeTreeSnowIncrement == 2)
      Shader.SetGlobalFloat("_DLCTreeReveal", 0.65f);
    Shader.SetGlobalFloat("_DLCTreeHideRanchBranch", UpgradeSystem.GetUnlocked(UpgradeSystem.Type.RanchingSystem) ? 0.0f : 1f);
    this.snow.gameObject.SetActive(true);
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    if ((UnityEngine.Object) this._baseTree != (UnityEngine.Object) null && this._baseTree.RevealType == UpgradeSystem.Type.Count)
    {
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._treeNodes[0].Button);
      this.OverrideDefault((Selectable) this._treeNodes[0].Button);
      this._treeContainer.anchoredPosition = -this._treeNodes[0].Button.GetComponent<RectTransform>().anchoredPosition;
      this._cursor.RectTransform.anchoredPosition = this._treeNodes[0].Button.GetComponent<RectTransform>().anchoredPosition;
      this._cursor.RectTransform.localScale = this._treeNodes[0].Button.transform.localScale;
    }
    if (!this._animateSnow)
      return;
    this.StartCoroutine((IEnumerator) this.DoIncrementSnowAnimation());
  }

  public override void Update()
  {
    this.ZoomUpdate();
    this.DelayTimer += Time.unscaledDeltaTime;
    if (!DataManager.Instance.MAJOR_DLC || !InputManager.UI.GetPageNavigateLeftDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || this.changingTrees || (double) this.DelayTimer <= 1.0 || !((UnityEngine.Object) this._baseTree != (UnityEngine.Object) null) || !this.CanvasGroup.interactable)
      return;
    this.ShowBaseTree();
  }

  public void ShowBaseTree()
  {
    this.changingTrees = true;
    this._baseTree.Configure();
    this._baseTree.Show();
    UIUpgradeTreeMenuController baseTree = this._baseTree;
    baseTree.OnShownCompleted = baseTree.OnShownCompleted + new System.Action(this.RemoveSelf);
    this._baseTree.DelayTimer = 0.5f;
    BuildingShrine.ShowingDLCTree = false;
  }

  public void RemoveSelf()
  {
    this.changingTrees = false;
    this._baseTree.CanvasGroup.interactable = true;
    UIUpgradeTreeMenuController baseTree = this._baseTree;
    baseTree.OnShownCompleted = baseTree.OnShownCompleted - new System.Action(this.RemoveSelf);
    UIMenuBase.ActiveMenus.Remove((UIMenuBase) this);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnHideStarted()
  {
    if (this._animateSnow)
      return;
    base.OnHideStarted();
    if (!((UnityEngine.Object) this._baseTree != (UnityEngine.Object) null))
      return;
    this._baseTree.Hide(true);
  }

  public override UpgradeTreeNode.TreeTier TreeTier()
  {
    return DataManager.Instance.DLCCurrentUpgradeTreeTier;
  }

  public override void UpdateTier(UpgradeTreeNode.TreeTier tier)
  {
    DataManager.Instance.DLCCurrentUpgradeTreeTier = tier;
  }

  public override void UpdatePointsText() => base.UpdatePointsText();

  public override IEnumerator DoDLCRevealAnimation()
  {
    yield break;
  }

  public override IEnumerator DoRevealAnimation(
    UpgradeSystem.Type typeToReveal,
    float delayHideDuration = 2f,
    bool hideOnComplete = true)
  {
    UIDLCUpgradeTreeMenuController treeMenuController = this;
    List<UpgradeTreeNode> changedNodes = new List<UpgradeTreeNode>();
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    float currentTreeReveal = Shader.GetGlobalFloat("_DLCTreeReveal");
    if (typeToReveal == UpgradeSystem.Type.WinterSystem)
      currentTreeReveal = 0.37f;
    foreach (UpgradeTreeNode treeNode in treeMenuController._treeNodes)
    {
      if (treeNode.RequiresUpgrade == typeToReveal)
        changedNodes.Add(treeNode);
    }
    UpgradeTreeNode targetNode = changedNodes[0];
    if (typeToReveal == UpgradeSystem.Type.WinterSystem)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/ui/divinetree/woolhaven_tab_open_firsttime");
      foreach (UpgradeTreeNode upgradeTreeNode in changedNodes)
      {
        if (upgradeTreeNode.Upgrade == UpgradeSystem.Type.Building_Furnace_2)
          targetNode = upgradeTreeNode;
      }
    }
    treeMenuController.OverrideDefault((Selectable) targetNode.Button);
    treeMenuController.SetActiveStateForMenu(false);
    treeMenuController._cursor.enabled = false;
    treeMenuController._cursor.CanvasGroup.DOFade(0.0f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    treeMenuController._scrollRect.enabled = false;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    foreach (UpgradeTreeNode upgradeTreeNode in changedNodes)
      upgradeTreeNode.Configure(treeMenuController.TreeTier(), true);
    foreach (UpgradeTreeNode nodeConnection in targetNode.NodeConnections)
    {
      if (nodeConnection.NodeTier <= treeMenuController.TreeTier())
        changedNodes.Add(nodeConnection);
    }
    Vector2 focalPoint = targetNode.RectTransform.anchoredPosition;
    float zoom = 0.85f;
    if (targetNode.PrerequisiteNodes != null && targetNode.PrerequisiteNodes.Length != 0)
    {
      UpgradeTreeNode prerequisiteNode = targetNode.PrerequisiteNodes[0];
      treeMenuController.DetermineFocalPointAndZoom(new List<UpgradeTreeNode>((IEnumerable<UpgradeTreeNode>) changedNodes)
      {
        prerequisiteNode
      }, out focalPoint, out zoom);
    }
    if (typeToReveal == UpgradeSystem.Type.WinterSystem)
      zoom /= 1.5f;
    UIManager.PlayAudio("event:/ui/map_location_pan");
    yield return (object) treeMenuController.DoFocusPosition(-focalPoint, 1f, zoom);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    float ranchRevealTime = 0.0f;
    if (treeMenuController.revealType == UpgradeSystem.Type.RanchingSystem)
    {
      AudioManager.Instance.PlayOneShot(treeMenuController.upgradeLayerRevealedSFX);
      DOTween.To((DOGetter<float>) (() => ranchRevealTime), (DOSetter<float>) (x => ranchRevealTime = x), 1f, 4f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => Shader.SetGlobalFloat("_DLCTreeReveal", Mathf.Lerp(currentTreeReveal, 1f, ranchRevealTime)))).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
    }
    if (treeMenuController.revealType == UpgradeSystem.Type.WinterSystem || treeMenuController.revealType == UpgradeSystem.Type.RanchingSystem)
      AudioManager.Instance.PlayOneShot("event:/dlc/ui/divinetree/woolhaven_tab_populate");
    UIManager.PlayAudio("event:/unlock_building/unlock");
    foreach (UpgradeTreeNode upgradeTreeNode in changedNodes)
    {
      upgradeTreeNode.ShowNode(hideOnComplete);
      treeMenuController.StartCoroutine((IEnumerator) upgradeTreeNode.DoUpdateStateAnimation());
      yield return (object) new WaitForSecondsRealtime(0.1f);
    }
    foreach (NodeConnectionLine nodeConnection in treeMenuController._nodeConnections)
      nodeConnection.Start();
    if (treeMenuController.revealType == UpgradeSystem.Type.RanchingSystem)
      Shader.SetGlobalFloat("_DLCTreeHideRanchBranch", 0.0f);
    Shader.SetGlobalFloat("_DLCTreeReveal", 1f);
    treeMenuController.snow.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot(treeMenuController.upgradeLayerCoveredUpSFX);
    float t = 0.0f;
    DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 4f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => Shader.SetGlobalFloat("_DLCTreeReveal", Mathf.Lerp(1f, currentTreeReveal, t)))).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(4f);
    if (treeMenuController.revealType == UpgradeSystem.Type.WinterSystem || treeMenuController.revealType == UpgradeSystem.Type.RanchingSystem)
      treeMenuController.StartCoroutine((IEnumerator) treeMenuController.DoIncrementSnowAnimation(true));
    else
      treeMenuController.Hide();
  }

  public override void OnNodeSelected(UpgradeTreeNode node)
  {
    if (node.State != UpgradeTreeNode.NodeState.Available && node.State != UpgradeTreeNode.NodeState.Unlocked)
      return;
    base.OnNodeSelected(node);
  }

  public virtual IEnumerator DoIncrementSnowAnimation(bool skipToUnlock = false)
  {
    UIDLCUpgradeTreeMenuController treeMenuController = this;
    if (UpgradeSystem.AbilityPoints <= 0)
      UpgradeSystem.AbilityPoints = 1;
    treeMenuController.UpdatePointsText();
    foreach (GameObject topPrompt in treeMenuController.topPrompts)
      topPrompt.gameObject.SetActive(false);
    treeMenuController.DisableControlPrompts();
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    treeMenuController.SetActiveStateForMenu(false);
    treeMenuController._cursor.enabled = false;
    treeMenuController._cursor.CanvasGroup.DOFade(0.0f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    treeMenuController._scrollRect.enabled = false;
    if (!skipToUnlock)
    {
      yield return (object) new WaitForSecondsRealtime(1f);
      ++DataManager.Instance.DLCUpgradeTreeSnowIncrement;
      if (DataManager.Instance.DLCUpgradeTreeSnowIncrement == 1)
        yield return (object) treeMenuController.DoFocusPosition(new Vector2(0.0f, 1000f), 1f);
      else
        yield return (object) treeMenuController.DoFocusPosition(new Vector2(0.0f, 0.0f), 1f);
      AudioManager.Instance.PlayOneShot(treeMenuController.upgradeLayerRevealedSFX);
      float t = 0.0f;
      DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 2f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        float a = 0.37f;
        float b = 0.455f;
        if (DataManager.Instance.DLCUpgradeTreeSnowIncrement == 2)
        {
          a = 0.455f;
          b = 0.65f;
        }
        else if (DataManager.Instance.DLCUpgradeTreeSnowIncrement == 3)
        {
          a = 0.535f;
          b = 0.666f;
        }
        Shader.SetGlobalFloat("_DLCTreeReveal", Mathf.Lerp(a, b, t));
      })).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(2f);
    }
    UpgradeTreeNode targetNode = (UpgradeTreeNode) null;
    foreach (UpgradeTreeNode treeNode in treeMenuController._treeNodes)
    {
      if (DataManager.Instance.DLCUpgradeTreeSnowIncrement == 0 && treeNode.Upgrade == UpgradeSystem.Type.Building_Furnace || DataManager.Instance.DLCUpgradeTreeSnowIncrement == 1 && treeNode.Upgrade == UpgradeSystem.Type.Building_Furnace_2 || DataManager.Instance.DLCUpgradeTreeSnowIncrement == 2 && treeNode.Upgrade == UpgradeSystem.Type.Building_Furnace_3 || treeMenuController.RevealType == UpgradeSystem.Type.RanchingSystem && treeNode.Upgrade == UpgradeSystem.Type.Building_Ranch)
        targetNode = treeNode;
    }
    yield return (object) treeMenuController.DoFocusPosition(-targetNode.RectTransform.anchoredPosition, 0.25f);
    targetNode.ForceState(UpgradeTreeNode.NodeState.Available);
    yield return (object) targetNode.DoUpdateStateAnimation();
    treeMenuController.SetActiveStateForMenu(true);
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) targetNode.Button);
    treeMenuController.EnableControlPrompts();
    treeMenuController.backPrompt.gameObject.SetActive(false);
    if (!skipToUnlock)
      ++DataManager.Instance.DLCCurrentUpgradeTreeTier;
    treeMenuController.changingTrees = true;
    BuildingShrine.AnimateSnowDLCTree = false;
    while (targetNode.State != UpgradeTreeNode.NodeState.Unlocked)
    {
      if ((UnityEngine.Object) treeMenuController._baseTree != (UnityEngine.Object) null)
        treeMenuController._baseTree.CanvasGroup.interactable = false;
      treeMenuController.CanvasGroup.interactable = false;
      yield return (object) null;
    }
    if ((UnityEngine.Object) treeMenuController._baseTree != (UnityEngine.Object) null)
      treeMenuController._baseTree.CanvasGroup.interactable = false;
    treeMenuController.CanvasGroup.interactable = false;
    if (targetNode.Upgrade == UpgradeSystem.Type.Building_Furnace_2)
      AudioManager.Instance.PlayOneShot("event:/dlc/building/furnace/upgrade_to_level_02");
    else if (targetNode.Upgrade == UpgradeSystem.Type.Building_Furnace_3)
      AudioManager.Instance.PlayOneShot("event:/dlc/building/furnace/upgrade_to_level_03");
    yield return (object) new WaitForSecondsRealtime(1f);
    treeMenuController._animateSnow = false;
    treeMenuController.Hide();
  }

  public override IEnumerator DoUnlockAnimation(UpgradeTreeNode targetNode)
  {
    UIDLCUpgradeTreeMenuController treeMenuController1 = this;
    List<UpgradeTreeNode> changedNodes = new List<UpgradeTreeNode>();
    float zoom = 1f;
    bool unlockedNewTier = false;
    bool unlockedCentralNode = false;
    treeMenuController1.OverrideDefault((Selectable) targetNode.Button);
    treeMenuController1.SetActiveStateForMenu(false);
    treeMenuController1._cursor.enabled = false;
    treeMenuController1._cursor.CanvasGroup.DOFade(0.0f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    treeMenuController1._scrollRect.enabled = false;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    targetNode.Configure(treeMenuController1.TreeTier(), true);
    foreach (UpgradeTreeNode nodeConnection in targetNode.NodeConnections)
    {
      if (nodeConnection.NodeTier <= treeMenuController1.TreeTier())
        changedNodes.Add(nodeConnection);
    }
    Vector2 anchoredPosition = targetNode.RectTransform.anchoredPosition;
    if (targetNode.PrerequisiteNodes != null && targetNode.PrerequisiteNodes.Length != 0)
    {
      UpgradeTreeNode prerequisiteNode = targetNode.PrerequisiteNodes[0];
      UIDLCUpgradeTreeMenuController treeMenuController2 = treeMenuController1;
      List<UpgradeTreeNode> nodes = new List<UpgradeTreeNode>();
      nodes.Add(prerequisiteNode);
      nodes.Add(targetNode);
      ref Vector2 local1 = ref anchoredPosition;
      ref float local2 = ref zoom;
      treeMenuController2.DetermineFocalPointAndZoom(nodes, out local1, out local2);
    }
    yield return (object) treeMenuController1.DoFocusPosition(-anchoredPosition, 0.25f, zoom);
    List<NodeConnectionLine> connectionLines = new List<NodeConnectionLine>();
    foreach (NodeConnectionLine nodeConnection in treeMenuController1._nodeConnections)
    {
      if (nodeConnection.Nodes.Contains(targetNode))
      {
        connectionLines.Add(nodeConnection);
        nodeConnection.PerformLineAnimation();
      }
    }
    if (connectionLines.Count > 0)
      yield return (object) treeMenuController1.YieldForConnections(connectionLines);
    else
      yield return (object) targetNode.DoUpdateStateAnimation();
    yield return (object) new WaitForSecondsRealtime(0.1f);
    zoom = 1f;
    foreach (UpgradeTreeNode treeNode in treeMenuController1._treeNodes)
      treeNode.OnStateDidChange += (Action<UpgradeTreeNode>) (changedNode =>
      {
        if (changedNodes.Contains(changedNode))
          return;
        changedNodes.Add(changedNode);
      });
    UpgradeTreeNode.TreeTier tier1 = treeMenuController1.TreeTier() + 1;
    UpgradeTreeConfiguration.TreeTierConfig configForTier = treeMenuController1._configuration.GetConfigForTier(tier1);
    if (configForTier != null)
    {
      int num1 = treeMenuController1._configuration.NumRequiredNodesForTier(tier1);
      int num2 = treeMenuController1._configuration.NumUnlockedUpgrades();
      Debug.Log((object) $"{tier1 - 1} - {num2}/{num1}".Colour(Color.yellow));
      if (num2 >= num1)
      {
        if (!configForTier.RequiresCentralTier || targetNode.Upgrade == configForTier.CentralNode)
        {
          unlockedNewTier = true;
          treeMenuController1.UpdateTier(tier1);
          treeMenuController1._rootNode.Configure(treeMenuController1.TreeTier(), true);
        }
        else
        {
          foreach (UpgradeTreeNode treeNode in treeMenuController1._treeNodes)
          {
            if (treeNode.Upgrade == configForTier.CentralNode && treeNode.State == UpgradeTreeNode.NodeState.Locked)
            {
              treeNode.Configure(treeMenuController1.TreeTier(), true);
              break;
            }
          }
        }
      }
    }
    foreach (UpgradeTreeNode treeNode in treeMenuController1._treeNodes)
      treeNode.OnStateDidChange -= (Action<UpgradeTreeNode>) (changedNode =>
      {
        if (changedNodes.Contains(changedNode))
          return;
        changedNodes.Add(changedNode);
      });
    TierLockIcon targetTierLock = (TierLockIcon) null;
    if (unlockedNewTier)
    {
      foreach (TierLockIcon tierLock in treeMenuController1._tierLocks)
      {
        if (tierLock.Tier == treeMenuController1.TreeTier())
        {
          targetTierLock = tierLock;
          break;
        }
      }
    }
    else
    {
      UpgradeTreeNode.TreeTier[] values = Enum.GetValues(typeof (UpgradeTreeNode.TreeTier)) as UpgradeTreeNode.TreeTier[];
      int num3 = treeMenuController1._configuration.NumUnlockedUpgrades();
      foreach (UpgradeTreeNode.TreeTier tier2 in values)
      {
        int num4 = treeMenuController1._configuration.NumRequiredNodesForTier(tier2);
        if (num3 == num4)
        {
          unlockedCentralNode = true;
          using (List<TierLockIcon>.Enumerator enumerator = treeMenuController1._tierLocks.GetEnumerator())
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
    if ((UnityEngine.Object) targetTierLock != (UnityEngine.Object) null && (targetTierLock.Tier == UpgradeTreeNode.TreeTier.Tier2 && DataManager.Instance.DLCUpgradeTreeSnowIncrement <= 0 || targetTierLock.Tier == UpgradeTreeNode.TreeTier.Tier3 && DataManager.Instance.DLCUpgradeTreeSnowIncrement <= 1 || targetTierLock.Tier == UpgradeTreeNode.TreeTier.Tier4 && DataManager.Instance.DLCUpgradeTreeSnowIncrement <= 2))
      unlockedCentralNode = false;
    Vector2 focalPoint;
    if (unlockedCentralNode)
    {
      yield return (object) treeMenuController1.DoFocusPosition(-targetTierLock.RectTransform.anchoredPosition, 0.25f, zoom);
      yield return (object) targetTierLock.DestroyTierLock();
      yield return (object) new WaitForSecondsRealtime(0.1f);
      if (targetTierLock.Tier - treeMenuController1.TreeTier() == 1 && changedNodes.Count > 0)
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
      foreach (UpgradeTreeNode treeNode in treeMenuController1._treeNodes)
      {
        if (treeNode.NodeTier == treeMenuController1.TreeTier())
          nodes.Add(treeNode);
      }
      treeMenuController1.DetermineFocalPointAndZoom(nodes, out focalPoint, out zoom);
    }
    else
      treeMenuController1.DetermineFocalPointAndZoom(new List<UpgradeTreeNode>((IEnumerable<UpgradeTreeNode>) changedNodes)
      {
        targetNode
      }, out focalPoint, out zoom);
    yield return (object) treeMenuController1.DoFocusPosition(-focalPoint, 0.25f, zoom);
    if (unlockedNewTier)
      yield return (object) targetTierLock.RevealTier();
    foreach (NodeConnectionLine nodeConnection in treeMenuController1._nodeConnections)
    {
      if (nodeConnection.IsDirty)
        nodeConnection.PerformLineAnimation();
    }
    yield return (object) treeMenuController1.YieldForConnections(treeMenuController1._nodeConnections);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    if ((double) zoom < 1.0)
      yield return (object) treeMenuController1.DoFocusPosition(-targetNode.RectTransform.anchoredPosition, 0.25f);
    treeMenuController1.OnUnlockAnimationCompleted();
    Action<UpgradeSystem.Type> onUpgradeUnlocked = treeMenuController1.OnUpgradeUnlocked;
    if (onUpgradeUnlocked != null)
      onUpgradeUnlocked(targetNode.Upgrade);
    UpgradeTreeNode upgradeTreeNode1 = changedNodes.Find((Predicate<UpgradeTreeNode>) (x => x.State != 0));
    if (unlockedCentralNode && (UnityEngine.Object) upgradeTreeNode1 != (UnityEngine.Object) null)
      treeMenuController1._cursor.RectTransform.anchoredPosition = upgradeTreeNode1.RectTransform.anchoredPosition;
    else
      treeMenuController1._cursor.RectTransform.anchoredPosition = targetNode.RectTransform.anchoredPosition;
  }
}
