// Decompiled with JetBrains decompiler
// Type: MMTools.DoctrineResponse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace MMTools;

public class DoctrineResponse
{
  public SermonCategory SermonCategory;
  public int RewardLevel;
  public bool isFirstChoice;
  public System.Action Callback;

  public DoctrineResponse(
    SermonCategory SermonCategory,
    int RewardLevel,
    bool isFirstChoice,
    System.Action Callback)
  {
    this.SermonCategory = SermonCategory;
    this.RewardLevel = RewardLevel;
    this.isFirstChoice = isFirstChoice;
    this.Callback = Callback;
  }
}
