// Decompiled with JetBrains decompiler
// Type: followerCountOnTemple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class followerCountOnTemple : BaseMonoBehaviour
{
  public GameObject[] dotsFollowerCount;
  public int FollowerCount;

  public void Start() => this.updateCircles();

  private void updateCircles()
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
