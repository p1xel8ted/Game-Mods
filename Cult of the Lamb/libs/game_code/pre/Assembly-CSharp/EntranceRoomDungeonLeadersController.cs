// Decompiled with JetBrains decompiler
// Type: EntranceRoomDungeonLeadersController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class EntranceRoomDungeonLeadersController : MonoBehaviour
{
  private Interaction_EntranceShrine _dungeonLeaderMechanics;
  public Interaction_WeaponSelectionPodium[] Weapons;

  private void OnEnable()
  {
    this._dungeonLeaderMechanics = UnityEngine.Object.FindObjectOfType<Interaction_EntranceShrine>();
    Debug.Log((object) nameof (EntranceRoomDungeonLeadersController));
    if (!((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null) && BiomeGenerator.Instance.CurrentRoom != null && BiomeGenerator.Instance.CurrentRoom.Completed)
      return;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      this.HideWeapons();
      RoomLockController.CloseAll();
    })));
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  private void HideWeapons()
  {
    if ((UnityEngine.Object) this._dungeonLeaderMechanics != (UnityEngine.Object) null)
      this._dungeonLeaderMechanics.HideDummys();
    foreach (Interaction_WeaponSelectionPodium weapon in this.Weapons)
    {
      if ((bool) (UnityEngine.Object) weapon)
        weapon.gameObject.SetActive(false);
    }
  }

  private void RevealWeapons()
  {
    foreach (Component weapon in this.Weapons)
      weapon.gameObject.SetActive(true);
    Interaction_Chest.ChestEvent onChestRevealed = Interaction_Chest.OnChestRevealed;
    if (onChestRevealed == null)
      return;
    onChestRevealed();
  }
}
