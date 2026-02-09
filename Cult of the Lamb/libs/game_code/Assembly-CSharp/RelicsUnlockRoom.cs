// Decompiled with JetBrains decompiler
// Type: RelicsUnlockRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MMRoomGeneration;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RelicsUnlockRoom : MonoBehaviour
{
  [SerializeField]
  public Interaction_WeaponChoice[] weaponChoices;

  public void Start()
  {
    foreach (Interaction_WeaponChoice weaponChoice in this.weaponChoices)
    {
      if ((UnityEngine.Object) weaponChoice != (UnityEngine.Object) null)
        weaponChoice.gameObject.SetActive(false);
    }
    if (DataManager.Instance.OnboardedRelics)
      return;
    RoomLockController.CloseAll();
    this.GetComponentInParent<GenerateRoom>().LockingDoors = true;
  }

  public void OnEnable() => CoopManager.Instance.transferRelicOnRemovePlayer = true;

  public void OnDisable() => CoopManager.Instance.transferRelicOnRemovePlayer = false;

  public void OnDestroy()
  {
    CoopManager.Instance.transferRelicOnRemovePlayer = false;
    foreach (Interaction_WeaponChoice weaponChoice in this.weaponChoices)
    {
      if ((UnityEngine.Object) weaponChoice != (UnityEngine.Object) null && (bool) (UnityEngine.Object) weaponChoice.gameObject)
        weaponChoice.gameObject.SetActive(true);
    }
  }

  public void ConversationCallback()
  {
    PlayerReturnToBase.Disabled = true;
    RoomLockController.CloseAll(false);
    Time.timeScale = 0.0f;
    DataManager.Instance.OnboardedRelics = true;
    UIRelicMenuController relicMenuController1 = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>();
    List<RelicType> relicTypes = new List<RelicType>();
    foreach (RelicData relicData in EquipmentManager.RelicData)
    {
      if (relicData.UpgradeType == UpgradeSystem.Type.Relic_Pack_Default)
        relicTypes.Add(relicData.RelicType);
      else if (CoopManager.CoopActive && relicData.UpgradeType == UpgradeSystem.Type.Relic_Pack_Coop)
        relicTypes.Add(relicData.RelicType);
    }
    if (CoopManager.CoopActive)
    {
      DataManager.Instance.UnlockedCoopRelics = true;
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relic_Pack_Coop, true);
      if (DataManager.Instance.UnlockedCoopTarots && DataManager.Instance.UnlockedCoopRelics)
        DataManager.Instance.UnlockedCoopRelicsAndTarots = true;
    }
    relicMenuController1.Show(relicTypes);
    UIRelicMenuController relicMenuController2 = relicMenuController1;
    relicMenuController2.OnHidden = relicMenuController2.OnHidden + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      this.weaponChoices[0].transform.localPosition = new Vector3(-1.21f, -1.96f, 0.0f);
      this.weaponChoices[0].RelicStartCharged = false;
      this.weaponChoices[0].OnInteraction += new Interaction.InteractionEvent(this.OnPickUpRelic);
      this.weaponChoices[0].CanOpenDoors = false;
      this.weaponChoices[0].Reveal();
    });
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relic_Pack_Default);
    GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
  }

  public void OnPickUpRelic(StateMachine state)
  {
    RelicRoomManager.Instance.EquippedRelicConversation.Play();
  }

  [CompilerGenerated]
  public void \u003CConversationCallback\u003Eb__5_0()
  {
    Time.timeScale = 1f;
    this.weaponChoices[0].transform.localPosition = new Vector3(-1.21f, -1.96f, 0.0f);
    this.weaponChoices[0].RelicStartCharged = false;
    this.weaponChoices[0].OnInteraction += new Interaction.InteractionEvent(this.OnPickUpRelic);
    this.weaponChoices[0].CanOpenDoors = false;
    this.weaponChoices[0].Reveal();
  }
}
