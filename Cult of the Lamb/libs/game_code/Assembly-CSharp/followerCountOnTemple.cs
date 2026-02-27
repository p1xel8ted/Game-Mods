// Decompiled with JetBrains decompiler
// Type: followerCountOnTemple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class followerCountOnTemple : BaseMonoBehaviour
{
  public GameObject[] dotsFollowerCount;
  public int FollowerCount;

  public void Start() => this.updateCircles();

  public void updateCircles()
  {
    for (int index = 0; index < this.dotsFollowerCount.Length; ++index)
      this.dotsFollowerCount[index].SetActive(false);
    this.FollowerCount = DataManager.Instance.Followers.Count;
    for (int index = 0; index < Mathf.Clamp(this.FollowerCount, 0, this.dotsFollowerCount.Length); ++index)
      this.dotsFollowerCount[index].SetActive(true);
  }

  public void Update()
  {
    if (this.FollowerCount == DataManager.Instance.Followers.Count)
      return;
    this.updateCircles();
  }
}
