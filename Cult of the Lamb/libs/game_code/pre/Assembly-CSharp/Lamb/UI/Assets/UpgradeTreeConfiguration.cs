// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.UpgradeTreeConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Upgrade Tree Configuration", menuName = "Massive Monster/Upgrade Tree Configuration", order = 1)]
public class UpgradeTreeConfiguration : ScriptableObject
{
  [Header("Tiers")]
  [SerializeField]
  private List<UpgradeTreeConfiguration.TreeTierConfig> _tierConfigurations = new List<UpgradeTreeConfiguration.TreeTierConfig>();
  [Header("Connections")]
  [SerializeField]
  private float _tierBridgeLength;
  [SerializeField]
  private float _branchOffset;
  [SerializeField]
  private Texture _lockedTexture;
  [SerializeField]
  private Texture _unavailableTexture;
  [SerializeField]
  private Texture _availableTexture;
  [SerializeField]
  private Texture _unlockedTexture;
  [SerializeField]
  private Color _lockedConnectionColor;
  [SerializeField]
  private Color _unavailableConnectionColor;
  [SerializeField]
  private Color _availableConnectionColor;
  [SerializeField]
  private Color _unlockedConnectionColor;
  [SerializeField]
  private List<UpgradeSystem.Type> _allUpgrades = new List<UpgradeSystem.Type>();

  public float TierBridgeLength => this._tierBridgeLength;

  public float BranchOffset => this._branchOffset;

  public Texture LockedTexture => this._lockedTexture;

  public Texture UnavailableTexture => this._unavailableTexture;

  public Texture AvailableTexture => this._availableTexture;

  public Texture UnlockedTexture => this._unlockedTexture;

  public Color LockedConnectionColor => this._lockedConnectionColor;

  public Color UnavailableConnectionColor => this._unavailableConnectionColor;

  public Color AvailableConnectionColor => this._availableConnectionColor;

  public Color UnlockedConnectionColor => this._unlockedConnectionColor;

  public List<UpgradeSystem.Type> AllUpgrades => this._allUpgrades;

  public UpgradeTreeConfiguration.TreeTierConfig GetConfigForTier(UpgradeTreeNode.TreeTier tier)
  {
    foreach (UpgradeTreeConfiguration.TreeTierConfig tierConfiguration in this._tierConfigurations)
    {
      if (tierConfiguration.Tier == tier)
        return tierConfiguration;
    }
    return (UpgradeTreeConfiguration.TreeTierConfig) null;
  }

  public int NumRequiredNodesForTier(UpgradeTreeNode.TreeTier tier)
  {
    int num = 0;
    foreach (UpgradeTreeConfiguration.TreeTierConfig tierConfiguration in this._tierConfigurations)
    {
      if (tierConfiguration.Tier <= tier)
        num += tierConfiguration.NumRequiredToUnlock;
      else
        break;
    }
    return num;
  }

  public float NormalizedProgressToNextTier(UpgradeTreeNode.TreeTier tier)
  {
    return 1f - Mathf.Clamp((float) (this.NumRequiredNodesForTier(tier) - this.NumUnlockedUpgrades()) / (float) this.GetConfigForTier(tier).NumRequiredToUnlock, 0.0f, 1f);
  }

  public float TotalNormalizedProgress()
  {
    return (float) (1 - this._allUpgrades.Count / this.NumUnlockedUpgrades());
  }

  public int NumUnlockedUpgrades()
  {
    int num = 0;
    foreach (UpgradeSystem.Type allUpgrade in this._allUpgrades)
    {
      if (UpgradeSystem.GetUnlocked(allUpgrade))
        ++num;
    }
    return num;
  }

  public UpgradeTreeNode.TreeTier HighestTier()
  {
    UpgradeTreeNode.TreeTier treeTier = UpgradeTreeNode.TreeTier.Tier1;
    foreach (UpgradeTreeConfiguration.TreeTierConfig tierConfiguration in this._tierConfigurations)
    {
      if (tierConfiguration.Tier > treeTier)
        treeTier = tierConfiguration.Tier;
    }
    return treeTier;
  }

  public bool HasUnlockAvailable()
  {
    int num = 0;
    foreach (UpgradeSystem.Type allUpgrade in this._allUpgrades)
    {
      if (UpgradeSystem.GetUnlocked(allUpgrade))
        ++num;
    }
    return num < this._allUpgrades.Count;
  }

  [Serializable]
  public class TreeTierConfig
  {
    [SerializeField]
    private UpgradeTreeNode.TreeTier _tier;
    [SerializeField]
    private bool _requiresCentralTier = true;
    [SerializeField]
    private UpgradeSystem.Type _centralNode;
    [SerializeField]
    private int _numRequiredToUnlock;

    public UpgradeTreeNode.TreeTier Tier => this._tier;

    public UpgradeSystem.Type CentralNode => this._centralNode;

    public bool RequiresCentralTier => this._requiresCentralTier;

    public int NumRequiredToUnlock => this._numRequiredToUnlock;
  }
}
