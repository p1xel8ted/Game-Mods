// Decompiled with JetBrains decompiler
// Type: WanderingSoulSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WanderingSoulSpawner : MonoBehaviour
{
  [SerializeField]
  public GameObject[] _wanderingSouls;

  public void OnEnable()
  {
    if (DataManager.Instance.TotalShrineGhostJuice > 4)
    {
      this.UpdateGhosts();
    }
    else
    {
      foreach (GameObject wanderingSoul in this._wanderingSouls)
        wanderingSoul.gameObject.SetActive(false);
    }
  }

  public void UpdateGhosts()
  {
    int num = DataManager.Instance.TotalShrineGhostJuice / 4;
    Debug.Log((object) $"{"JUICE! ".Colour(Color.yellow)}  {num.ToString()}  {this._wanderingSouls.Length.ToString()}");
    Debug.Log((object) ("DataManager.Instance.TotalShrineGhostJuice  " + DataManager.Instance.TotalShrineGhostJuice.ToString()));
    for (int index = 0; index < this._wanderingSouls.Length; ++index)
      this._wanderingSouls[index].gameObject.SetActive(index < num);
  }
}
