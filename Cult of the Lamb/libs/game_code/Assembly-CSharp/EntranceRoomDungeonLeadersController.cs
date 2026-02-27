// Decompiled with JetBrains decompiler
// Type: EntranceRoomDungeonLeadersController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EntranceRoomDungeonLeadersController : MonoBehaviour
{
  public Interaction_EntranceShrine _dungeonLeaderMechanics;
  public Interaction_WeaponSelectionPodium[] Weapons;

  public void OnEnable()
  {
    this._dungeonLeaderMechanics = UnityEngine.Object.FindObjectOfType<Interaction_EntranceShrine>();
    Debug.Log((object) nameof (EntranceRoomDungeonLeadersController));
    if (DungeonSandboxManager.Active)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      if (!((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null) && BiomeGenerator.Instance.CurrentRoom != null && BiomeGenerator.Instance.CurrentRoom.Completed)
        return;
      GameManager.GetInstance().StartCoroutine(this.FrameDelay((System.Action) (() =>
      {
        this.HideWeapons();
        RoomLockController.CloseAll();
      })));
    }
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void HideWeapons()
  {
    if ((UnityEngine.Object) this._dungeonLeaderMechanics != (UnityEngine.Object) null)
      this._dungeonLeaderMechanics.HideDummys();
    foreach (Interaction_WeaponSelectionPodium weapon in this.Weapons)
    {
      if ((bool) (UnityEngine.Object) weapon)
        weapon.gameObject.SetActive(false);
    }
  }

  public void RevealWeapons()
  {
    foreach (Component weapon in this.Weapons)
      weapon.gameObject.SetActive(true);
    Interaction_Chest.ChestEvent onChestRevealed = Interaction_Chest.OnChestRevealed;
    if (onChestRevealed == null)
      return;
    onChestRevealed();
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__1_0()
  {
    this.HideWeapons();
    RoomLockController.CloseAll();
  }
}
