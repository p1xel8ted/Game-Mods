// Decompiled with JetBrains decompiler
// Type: src.UI.Items.QuestItemInactive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using UnityEngine;

#nullable disable
namespace src.UI.Items;

public class QuestItemInactive : QuestItemBase<ObjectivesDataFinalized>
{
  public override void Configure(bool failed = false)
  {
    base.Configure(failed);
    this._button.Confirmable = false;
    this._datas.Sort((Comparison<ObjectivesDataFinalized>) ((a, b) => a.Index.CompareTo(b.Index)));
    foreach (ObjectivesDataFinalized data in this._datas)
      this.AddObjective(data, failed);
  }

  public override void AddObjectivesData(ObjectivesDataFinalized data)
  {
    if (string.IsNullOrEmpty(this._uniqueGroupID))
    {
      this._uniqueGroupID = data.UniqueGroupID;
      this._title.text = LocalizationManager.GetTranslation(data.GroupId);
    }
    if (!(this._uniqueGroupID == data.UniqueGroupID))
      return;
    this._datas.Add(data);
  }

  public override void AddObjective(ObjectivesDataFinalized objectivesData, bool failed = false)
  {
    this._questItemObjectiveTemplate.Instantiate<QuestItemObjective>((Transform) this._objectivesContainer).Configure(objectivesData, failed);
  }
}
