// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RewardPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RewardPath : MonoBehaviour
{
  public Action<PlayerFleeceManager.FleeceType> OnSelected;
  public Action<ScenarioData> OnConfirmed;
  [Header("Fleeces")]
  [SerializeField]
  public PlayerFleeceManager.FleeceType _fleece;
  [SerializeField]
  public SandboxFleeceItem _fleeceItem;
  [Header("Rewards")]
  [SerializeField]
  public MMUILineRenderer _topLine;
  [SerializeField]
  public MMUILineRenderer _bottomLine;
  [SerializeField]
  public RectTransform _itemContainer;
  [SerializeField]
  public RewardItem _rewardItemTemplate;
  public List<ScenarioData> _scenarioData;
  public List<RewardItem> _rewardItems = new List<RewardItem>();

  public PlayerFleeceManager.FleeceType Fleece => this._fleece;

  public SandboxFleeceItem FleeceItem => this._fleeceItem;

  public void Configure(List<ScenarioData> scenarioData)
  {
    DungeonSandboxManager.ProgressionSnapshot progressionForScenario = DungeonSandboxManager.GetProgressionForScenario(scenarioData[0].ScenarioType, this._fleece);
    int completedRuns = Mathf.Min(progressionForScenario.CompletedRuns, 5);
    this._fleeceItem.Configure((int) this._fleece);
    this._fleeceItem.Button.OnSelected += (System.Action) (() =>
    {
      Action<PlayerFleeceManager.FleeceType> onSelected = this.OnSelected;
      if (onSelected != null)
        onSelected(this._fleece);
      if (!this._fleeceItem.Unlocked)
        return;
      this._rewardItems[completedRuns].Highlight();
    });
    this._fleeceItem.Button.OnDeselected += (System.Action) (() =>
    {
      if (!this._fleeceItem.Unlocked)
        return;
      this._rewardItems[completedRuns].UnHighlight();
    });
    this._fleeceItem.Button.onClick.AddListener((UnityAction) (() =>
    {
      Action<ScenarioData> onConfirmed = this.OnConfirmed;
      if (onConfirmed == null)
        return;
      onConfirmed(this._scenarioData[completedRuns]);
    }));
    this._scenarioData = scenarioData;
    foreach (ScenarioData scenarioData1 in this._scenarioData)
    {
      if (this._rewardItems.Count != 6)
      {
        RewardItem rewardItem = this._rewardItemTemplate.Instantiate<RewardItem>((Transform) this._itemContainer);
        rewardItem.Configure(scenarioData1, progressionForScenario, (double) this.transform.rotation.z > 0.0);
        this._rewardItems.Add(rewardItem);
      }
      else
        break;
    }
    LayoutRebuilder.ForceRebuildLayoutImmediate(this._itemContainer);
    MMUILineRenderer.Branch root = this._topLine.Root;
    root.Points.Clear();
    if (completedRuns == 0)
      root.Points.Add(new MMUILineRenderer.BranchPoint((Vector2) Vector3.zero));
    else
      root.Points.Add(new MMUILineRenderer.BranchPoint((Vector2) this._rewardItems[completedRuns - 1].transform.localPosition));
    root.Points.Add(new MMUILineRenderer.BranchPoint((Vector2) this._rewardItems[completedRuns].transform.localPosition));
    this._topLine.UpdateValues();
    this._rewardItems[completedRuns].ConfigureSecondaryLine(root);
    MMUILineRenderer.Branch branch = this._bottomLine.Root;
    branch.Points.Clear();
    branch.Points.Add(new MMUILineRenderer.BranchPoint(Vector2.zero));
    foreach (RewardItem rewardItem in this._rewardItems)
    {
      branch.Points.Add(new MMUILineRenderer.BranchPoint((Vector2) rewardItem.transform.localPosition));
      rewardItem.ConfigureLine(branch);
      branch = branch.Points.LastElement<MMUILineRenderer.BranchPoint>().AddNewBranch();
    }
    this._bottomLine.UpdateValues();
  }

  public void SetIncognitoMode() => this._fleeceItem.Button.interactable = false;

  public void RemoveIncognitoMode() => this._fleeceItem.Button.interactable = true;
}
