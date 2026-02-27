// Decompiled with JetBrains decompiler
// Type: FollowerDirectionSetter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerDirectionSetter : BaseMonoBehaviour
{
  public GameObject[] GameObjects;

  private void GetChildren()
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
