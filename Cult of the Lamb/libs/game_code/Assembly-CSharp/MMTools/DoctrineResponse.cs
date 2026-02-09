// Decompiled with JetBrains decompiler
// Type: MMTools.DoctrineResponse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
