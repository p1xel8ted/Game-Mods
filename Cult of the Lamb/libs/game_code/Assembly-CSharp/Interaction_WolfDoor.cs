// Decompiled with JetBrains decompiler
// Type: Interaction_WolfDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_WolfDoor : Interaction
{
  [SerializeField]
  public GameObject door;
  public const int requiredFollowers = 5;

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = false;
    if (DataManager.Instance.Followers.Count < 5)
    {
      if (LocalizeIntegration.IsArabic())
      {
        string[] strArray = new string[6];
        strArray[0] = ScriptLocalization.Interactions.Requires;
        int num = 5;
        strArray[1] = num.ToString();
        strArray[2] = " / <color=red> ";
        num = DataManager.Instance.Followers.Count;
        strArray[3] = num.ToString();
        strArray[4] = "</color> ";
        strArray[5] = FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS);
        this.Label = string.Concat(strArray);
      }
      else
        this.Label = $"{ScriptLocalization.Interactions.Requires}<color=red> {DataManager.Instance.Followers.Count.ToString()}</color> / {5.ToString()} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
    }
    else if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_6))
    {
      this.Label = LocalizationManager.GetTranslation("Interactions/RequiresExecutionerAxe");
    }
    else
    {
      this.Interactable = true;
      this.Label = ScriptLocalization.FollowerInteractions.MakeDemand;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.door.gameObject.SetActive(false);
    this.Interactable = false;
    this.HasChanged = true;
  }
}
