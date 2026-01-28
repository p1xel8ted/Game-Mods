// Decompiled with JetBrains decompiler
// Type: YngyaRewardManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class YngyaRewardManager : SerializedMonoBehaviour
{
  public static YngyaRewardManager Instance;
  [SerializeField]
  public RewardDictionary rewardDictionary = new RewardDictionary();
  [SerializeField]
  public GameObject defaultReward;
  [SerializeField]
  public GameObject wolfShrine;
  [SerializeField]
  public GameObject door;
  [SerializeField]
  public UnityEvent callback;

  public UnityEvent Callback => this.callback;

  public void SpokeToYngya()
  {
    DataManager.Instance.SpokeToYngyaOnMountainTop = true;
    this.defaultReward.SetActive(true);
    this.wolfShrine.SetActive(false);
  }

  public void OnEnable()
  {
    YngyaRewardManager.Instance = this;
    if (DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Shrine)
      return;
    if (DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_Witness || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_Witness)
    {
      this.defaultReward.SetActive(true);
      this.wolfShrine.SetActive(false);
    }
    else if ((UnityEngine.Object) this.door != (UnityEngine.Object) null && (DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_Boss || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_Boss))
    {
      this.door.gameObject.SetActive(true);
      this.gameObject.SetActive(false);
      this.door.GetComponentInParent<Interaction_TempleBossDoor>().enabled = true;
    }
    else
    {
      if ((UnityEngine.Object) this.door != (UnityEngine.Object) null)
        this.door.GetComponentInParent<Interaction_TempleBossDoor>().Used = true;
      if (!DataManager.Instance.SpokeToYngyaOnMountainTop && DataManager.Instance.CurrentDLCNodeType != DungeonWorldMapIcon.NodeType.Dungeon5_Story)
      {
        foreach (KeyValuePair<DungeonWorldMapIcon.NodeType, GameObject> reward in (Dictionary<DungeonWorldMapIcon.NodeType, GameObject>) this.rewardDictionary)
          reward.Value.SetActive(false);
        this.defaultReward.SetActive(false);
        this.wolfShrine.SetActive(true);
      }
      else
      {
        this.wolfShrine.SetActive(false);
        bool flag1 = false;
        this.rewardDictionary.ForEach<KeyValuePair<DungeonWorldMapIcon.NodeType, GameObject>>((Action<KeyValuePair<DungeonWorldMapIcon.NodeType, GameObject>>) (reward => reward.Value.SetActive(false)));
        foreach (KeyValuePair<DungeonWorldMapIcon.NodeType, GameObject> reward in (Dictionary<DungeonWorldMapIcon.NodeType, GameObject>) this.rewardDictionary)
        {
          bool flag2 = DataManager.Instance.CurrentDLCNodeType == reward.Key;
          if (flag2)
          {
            reward.Value.SetActive(flag2);
            flag1 = true;
          }
        }
        this.defaultReward.SetActive(!flag1);
        if (DataManager.Instance.CurrentDLCNodeType != DungeonWorldMapIcon.NodeType.Dungeon5_Story && DataManager.Instance.CurrentDLCNodeType != DungeonWorldMapIcon.NodeType.Dungeon6_Story)
          return;
        this.DisableTeleporter();
      }
    }
  }

  public void DisableTeleporter()
  {
    if (!((UnityEngine.Object) Interaction_TeleportHome.Instance != (UnityEngine.Object) null))
      return;
    Interaction_TeleportHome.Instance.Interactable = false;
  }

  public void EnableTeleporter()
  {
    if (!((UnityEngine.Object) Interaction_TeleportHome.Instance != (UnityEngine.Object) null))
      return;
    Interaction_TeleportHome.Instance.Interactable = true;
  }
}
