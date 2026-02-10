// Decompiled with JetBrains decompiler
// Type: RelicTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RelicTester : MonoBehaviour
{
  public RelicType relicType = RelicType.LightningStrike;
  public RelicType secondaryRelicType = RelicType.RerollWeapon;

  public void RelicsUnlocked() => DataManager.Instance.OnboardedRelics = true;

  public void ChangeRelic()
  {
    DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Relics);
    PlayerFarming.Instance.playerRelic.EquipRelic(EquipmentManager.GetRelicData(this.relicType));
  }

  public void ChargeRelic() => PlayerFarming.Instance.playerRelic.FullyCharge();

  public void RemoveRelicUpgrades()
  {
    UpgradeSystem.UnlockedUpgrades.Remove(UpgradeSystem.Type.Relics_Blessed_1);
    UpgradeSystem.UnlockedUpgrades.Remove(UpgradeSystem.Type.Relics_Dammed_1);
    UpgradeSystem.UnlockedUpgrades.Remove(UpgradeSystem.Type.Relic_Pack1);
    UpgradeSystem.UnlockedUpgrades.Remove(UpgradeSystem.Type.Relic_Pack2);
  }

  public void AddRelicUpgrades()
  {
    UpgradeSystem.UnlockedUpgrades.Add(UpgradeSystem.Type.Relic_Pack_Default);
    UpgradeSystem.UnlockedUpgrades.Add(UpgradeSystem.Type.Relics_Blessed_1);
    UpgradeSystem.UnlockedUpgrades.Add(UpgradeSystem.Type.Relics_Dammed_1);
    UpgradeSystem.UnlockedUpgrades.Add(UpgradeSystem.Type.Relic_Pack1);
    UpgradeSystem.UnlockedUpgrades.Add(UpgradeSystem.Type.Relic_Pack2);
  }

  public void Update()
  {
    if (Input.GetKeyUp(KeyCode.X))
    {
      RelicType relicType = this.relicType;
      this.relicType = this.secondaryRelicType;
      this.secondaryRelicType = relicType;
      this.ChangeRelic();
    }
    if (!Input.GetKeyUp(KeyCode.C))
      return;
    Debug.Log((object) ("PlayerFarming.Instance.playerRelic: " + ((object) PlayerFarming.Instance.playerRelic)?.ToString()));
    if (PlayerFarming.Instance.currentRelicType == RelicType.None)
      this.ChangeRelic();
    else
      this.ChargeRelic();
  }
}
