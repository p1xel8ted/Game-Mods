// Decompiled with JetBrains decompiler
// Type: FollowerDirectionSetter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
