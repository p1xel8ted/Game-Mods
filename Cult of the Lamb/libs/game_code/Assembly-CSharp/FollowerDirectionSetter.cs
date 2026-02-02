// Decompiled with JetBrains decompiler
// Type: FollowerDirectionSetter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerDirectionSetter : BaseMonoBehaviour
{
  public GameObject[] GameObjects;

  public void GetChildren()
  {
    this.GameObjects = new GameObject[this.transform.childCount];
    int index = -1;
    while (++index < this.transform.childCount)
      this.GameObjects[index] = this.transform.GetChild(index).gameObject;
  }

  public void SetDirection()
  {
    foreach (GameObject gameObject in this.GameObjects)
      gameObject.GetComponent<FollowerDirectionChanger>().chooseDirection();
  }
}
