// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.QuestItemActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public GameObject _trackingContainer;
  [SerializeField]
  public SkeletonGraphic _trackingTick;

  public void Start() => this._button.onClick.AddListener(new UnityAction(this.OnClicked));

  public void OnEnable()
  {
    ObjectiveManager.OnObjectiveUntrackedFromQue += new Action<string>(this.UpdateTick);
  }

  public void OnDisable()
  {
    ObjectiveManager.OnObjectiveUntrackedFromQue -= new Action<string>(this.UpdateTick);
  }

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
    if (this._uniqueGroupID == data.UniqueGroupID)
      this._datas.Add(data);
    this._trackingTick.color = Color.white;
    if (data.Type != Objectives.TYPES.BLIZZARD_OFFERING)
      return;
    this.Button.Confirmable = false;
    this._trackingTick.color = Color.gray;
  }

  public override void AddObjective(ObjectivesData objectivesData, bool failed = false)
  {
    this._questItemObjectiveTemplate.Instantiate<QuestItemObjective>((Transform) this._objectivesContainer).Configure(objectivesData);
  }

  public void OnClicked()
  {
    this._trackedQuest = !this._trackedQuest;
    if (this._trackedQuest)
      ObjectiveManager.TrackGroup(this._uniqueGroupID);
    else
      ObjectiveManager.UntrackGroup(this._uniqueGroupID);
    this._trackingTick.SetAnimation(this._trackedQuest ? "turn-on" : "off");
  }

  public void UpdateTick(string uniqueGroupID)
  {
    if (this._uniqueGroupID != uniqueGroupID)
      return;
    this._trackedQuest = DataManager.Instance.TrackedObjectiveGroupIDs.Contains(uniqueGroupID);
    this._trackingTick.SetAnimation(this._trackedQuest ? "turn-on" : "off");
  }
}
