// Decompiled with JetBrains decompiler
// Type: Objectives_DefeatKnucklebones
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public class Objectives_DefeatKnucklebones : ObjectivesData
{
  public string CharacterNameTerm = "";

  public Objectives_DefeatKnucklebones(
    string groupId,
    string CharacterNameTerm,
    float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.DEFEAT_KNUCKLEBONES;
    this.CharacterNameTerm = CharacterNameTerm;
  }

  public override string Text
  {
    get
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/Custom/Knucklebones"), (object) $"<color=#FFD201>{LocalizationManager.GetTranslation(this.CharacterNameTerm ?? "")}</color>");
    }
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones finalizedData = new Objectives_DefeatKnucklebones.FinalizedData_DefeatKnucklebones();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    finalizedData.CharacterNameTerm = this.CharacterNameTerm;
    return (ObjectivesDataFinalized) finalizedData;
  }

  protected override bool CheckComplete()
  {
    Debug.Log((object) $"{"CharacterNameTerm: ".Colour(Color.green)}  {this.CharacterNameTerm}");
    switch (this.CharacterNameTerm)
    {
      case "NAMES/Ratau":
        return DataManager.Instance.Knucklebones_Opponent_Ratau_Won || DataManager.Instance.RatauKilled;
      case "NAMES/Knucklebones/Knucklebones_NPC_0":
        return DataManager.Instance.Knucklebones_Opponent_0_Won;
      case "NAMES/Knucklebones/Knucklebones_NPC_1":
        return DataManager.Instance.Knucklebones_Opponent_1_Won;
      case "NAMES/Knucklebones/Knucklebones_NPC_2":
        return DataManager.Instance.Knucklebones_Opponent_2_Won;
      default:
        return false;
    }
  }

  [Serializable]
  public class FinalizedData_DefeatKnucklebones : ObjectivesDataFinalized
  {
    public string CharacterNameTerm = "";

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/Custom/Knucklebones"), (object) $"<color=#FFD201>{LocalizationManager.GetTranslation(this.CharacterNameTerm ?? "")}</color>");
    }
  }
}
