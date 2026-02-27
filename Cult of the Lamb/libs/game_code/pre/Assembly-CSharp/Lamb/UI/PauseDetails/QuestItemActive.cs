// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.QuestItemActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using src.Extensions;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.PauseDetails;

public class QuestItemActive : QuestItemBase<ObjectivesData>
{
  [Header("Tracking")]
  [SerializeField]
  protected GameObject _trackingContainer;
  [SerializeField]
  protected SkeletonGraphic _trackingTick;

  private void Start() => this._button.onClick.AddListener(new UnityAction(this.OnClicked));

  public override void Configure(bool failed = false)
  {
    base.Configure();
    this._datas.Sort((Comparison<ObjectivesData>) ((a, b) => a.Index.CompareTo(b.Index)));
    foreach (ObjectivesData data in this._datas)
      this.AddObjective(data, failed);
  }

  public override void AddObjectivesData(ObjectivesData data)
  {
    if (string.IsNullOrEmpty(this._uniqueGroupID))
    {
      this._uniqueGroupID = data.UniqueGroupID;
      this._title.text = LocalizationManager.GetTranslation(data.GroupId);
      this._trackedQuest = DataManager.Instance.TrackedObjectiveGroupIDs.Contains(this._uniqueGroupID);
      if (this._trackedQuest)
        this._trackingTick.SetAnimation("on");
    }
    if (!(this._uniqueGroupID == data.UniqueGroupID))
      return;
    this._datas.Add(data);
  }

  protected override void AddObjective(ObjectivesData objectivesData, bool failed = false)
  {
    this._questItemObjectiveTemplate.Instantiate<QuestItemObjective>((Transform) this._objectivesContainer).Configure(objectivesData);
  }

  private void OnClicked()
  {
    this._trackedQuest = !this._trackedQuest;
    if (this._trackedQuest)
      ObjectiveManager.TrackGroup(this._uniqueGroupID);
    else
      ObjectiveManager.UntrackGroup(this._uniqueGroupID);
    this._trackingTick.SetAnimation(this._trackedQuest ? "turn-on" : "off");
  }
}
