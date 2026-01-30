// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.UpgradeTreeConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Upgrade Tree Configuration", menuName = "Massive Monster/Upgrade Tree Configuration", order = 1)]
public class UpgradeTreeConfiguration : ScriptableObject
{
  [Header("Tiers")]
  [SerializeField]
  public List<UpgradeTreeConfiguration.TreeTierConfig> _tierConfigurations = new List<UpgradeTreeConfiguration.TreeTierConfig>();
  [Header("Connections")]
  [SerializeField]
  public float _tierBridgeLength;
  [SerializeField]
  public float _branchOffset;
  [SerializeField]
  public Texture _lockedTexture;
  [SerializeField]
  public Texture _unavailableTexture;
  [SerializeField]
  public Texture _availableTexture;
  [SerializeField]
  public Texture _unlockedTexture;
  [SerializeField]
  public Color _lockedConnectionColor;
  [SerializeField]
  public Color _unavailableConnectionColor;
  [SerializeField]
  public Color _availableConnectionColor;
  [SerializeField]
  public Color _unlockedConnectionColor;
  [SerializeField]
  public List<UpgradeSystem.Type> _allUpgrades = new List<UpgradeSystem.Type>();
  [SerializeField]
  public List<UpgradeTreeConfiguration.RequiresUpgrade> _allUpgradesRequiringUpgrade = new List<UpgradeTreeConfiguration.RequiresUpgrade>();

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

  public List<UpgradeTreeConfiguration.TreeTierConfig> TierConfigurations
  {
    get => this._tierConfigurations;
  }

  public List<UpgradeSystem.Type> AllUpgrades => this._allUpgrades;

  public List<UpgradeTreeConfiguration.RequiresUpgrade> AllUpgradesRequiringUpgrade
  {
    get => this._allUpgradesRequiringUpgrade;
  }

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

  public bool HasUnlockAvailable(bool sermon = false)
  {
    int num1 = 0;
    int num2 = 0;
    for (int index1 = 0; index1 < this._allUpgrades.Count; ++index1)
    {
      UpgradeSystem.Type allUpgrade = this._allUpgrades[index1];
      bool flag1 = false;
      bool flag2 = false;
      for (int index2 = 0; index2 < this._allUpgradesRequiringUpgrade.Count; ++index2)
      {
        UpgradeTreeConfiguration.RequiresUpgrade requiresUpgrade = this._allUpgradesRequiringUpgrade[index2];
        if (requiresUpgrade.Children.Contains(allUpgrade))
        {
          if (UpgradeSystem.GetUnlocked(requiresUpgrade.Upgrade))
          {
            if (UpgradeSystem.GetUnlocked(allUpgrade))
              ++num1;
          }
          else
          {
            ++num2;
            flag2 = true;
          }
          flag1 = true;
          break;
        }
      }
      if (UpgradeSystem.GetUnlocked(allUpgrade) && !flag1)
        ++num1;
      if (!flag2 && DataManager.MajorDLCStructures.Contains(UpgradeSystem.GetStructureTypeFromUpgrade(allUpgrade)))
      {
        UpgradeTreeNode.TreeTier tierFromUpgrade = this.GetTierFromUpgrade(allUpgrade);
        if (tierFromUpgrade == UpgradeTreeNode.TreeTier.Tier2 && DataManager.Instance.DLCUpgradeTreeSnowIncrement < 1 || tierFromUpgrade == UpgradeTreeNode.TreeTier.Tier3 && DataManager.Instance.DLCUpgradeTreeSnowIncrement < 2 || tierFromUpgrade == UpgradeTreeNode.TreeTier.Tier4 && DataManager.Instance.DLCUpgradeTreeSnowIncrement < 3)
          ++num2;
      }
    }
    return sermon && SeasonsManager.Active && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Major_DLC_Sermon_Packs) || num1 < this._allUpgrades.Count - num2;
  }

  public UpgradeTreeNode.TreeTier GetTierFromUpgrade(UpgradeSystem.Type upgrade)
  {
    foreach (UpgradeTreeConfiguration.TreeTierConfig tierConfiguration in this.TierConfigurations)
    {
      if (tierConfiguration.AllUpgradesInTier.Contains(upgrade))
        return tierConfiguration.Tier;
    }
    return UpgradeTreeNode.TreeTier.Tier1;
  }

  public UpgradeSystem.Type[] GetAllCentralUpgrades()
  {
    UpgradeSystem.Type[] allCentralUpgrades = new UpgradeSystem.Type[this._tierConfigurations.Count];
    for (int index = 0; index < allCentralUpgrades.Length; ++index)
      allCentralUpgrades[index] = this._tierConfigurations[index].CentralNode;
    return allCentralUpgrades;
  }

  public void PrintUpgrades()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("UpgradeSystem.Type treeUpgrades = new UpgradeSystem.Type[] {\n");
    foreach (UpgradeSystem.Type allUpgrade in this._allUpgrades)
      stringBuilder.Append($"UpgradeSystem.Type.{allUpgrade.ToString()},\n");
    stringBuilder.Append("\n};");
    Debug.Log((object) stringBuilder.ToString());
  }

  [Serializable]
  public class TreeTierConfig
  {
    [SerializeField]
    public UpgradeTreeNode.TreeTier _tier;
    [SerializeField]
    public bool _requiresCentralTier = true;
    [SerializeField]
    public UpgradeSystem.Type _centralNode;
    [SerializeField]
    public int _numRequiredToUnlock;
    [SerializeField]
    public List<UpgradeSystem.Type> _allUpgradesInTier = new List<UpgradeSystem.Type>();

    public UpgradeTreeNode.TreeTier Tier => this._tier;

    public UpgradeSystem.Type CentralNode => this._centralNode;

    public bool RequiresCentralTier => this._requiresCentralTier;

    public int NumRequiredToUnlock => this._numRequiredToUnlock;

    public List<UpgradeSystem.Type> AllUpgradesInTier => this._allUpgradesInTier;
  }

  [Serializable]
  public struct RequiresUpgrade
  {
    public UpgradeSystem.Type Upgrade;
    public List<UpgradeSystem.Type> Children;
  }
}
