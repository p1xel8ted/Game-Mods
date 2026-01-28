// Decompiled with JetBrains decompiler
// Type: Objectives_DefeatKnucklebones
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_DefeatKnucklebones : ObjectivesData
{
  [Key(16 /*0x10*/)]
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

  public Objectives_DefeatKnucklebones()
  {
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

  public override bool CheckComplete()
  {
    Debug.Log((object) $"{"CharacterNameTerm: ".Colour(Color.green)}  {this.CharacterNameTerm}");
    switch (this.CharacterNameTerm)
    {
      case "NAMES/Ratau":
        return DataManager.Instance.Knucklebones_Opponent_Ratau_Won && !DataManager.Instance.RatauKilled;
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

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_DefeatKnucklebones : ObjectivesDataFinalized
  {
    [Key(3)]
    public string CharacterNameTerm = "";

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/Custom/Knucklebones"), (object) $"<color=#FFD201>{LocalizationManager.GetTranslation(this.CharacterNameTerm ?? "")}</color>");
    }
  }
}
