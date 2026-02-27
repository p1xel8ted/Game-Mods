// Decompiled with JetBrains decompiler
// Type: Lamb.UI.QuestsMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.PauseDetails;
using src.Extensions;
using src.UI.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class QuestsMenu : UISubmenuBase
{
  [FormerlySerializedAs("_questItemTemplate")]
  [Header("Templates")]
  [SerializeField]
  public QuestItemActive _questItemActiveTemplate;
  [SerializeField]
  public QuestItemInactive _questItemInActiveTemplate;
  [Header("Quests Menu")]
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public RectTransform _activeQuestsContent;
  [SerializeField]
  public GameObject _completedQuestsHeader;
  [SerializeField]
  public RectTransform _completedQuestsContent;
  [SerializeField]
  public GameObject _failedQuestsHeader;
  [SerializeField]
  public RectTransform _failedQuestsContent;
  [SerializeField]
  public GameObject _noActiveQuestsText;
  [SerializeField]
  public GameObject _noCompletedQuestsText;
  [SerializeField]
  public GameObject _noFailedQuestsText;
  public Dictionary<string, QuestItemActive> _activeQuestItems = new Dictionary<string, QuestItemActive>();
  public Dictionary<string, QuestItemInactive> _completedQuestItems = new Dictionary<string, QuestItemInactive>();
  public Dictionary<string, QuestItemInactive> _failedQuestItems = new Dictionary<string, QuestItemInactive>();

  public override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    this._scrollRect.normalizedPosition = (Vector2) Vector3.one;
    if (this._activeQuestItems.Count + this._completedQuestItems.Count + this._failedQuestItems.Count == 0)
    {
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (!string.IsNullOrEmpty(objective.UniqueGroupID))
        {
          QuestItemActive questItemActive1;
          if (this._activeQuestItems.TryGetValue(objective.UniqueGroupID, out questItemActive1))
          {
            questItemActive1.AddObjectivesData(objective);
          }
          else
          {
            QuestItemActive questItemActive2 = this._questItemActiveTemplate.Instantiate<QuestItemActive>((Transform) this._activeQuestsContent);
            questItemActive2.AddObjectivesData(objective);
            this._activeQuestItems.Add(objective.UniqueGroupID, questItemActive2);
          }
        }
      }
      foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
      {
        if (!string.IsNullOrEmpty(completedObjective.UniqueGroupID))
        {
          QuestItemActive questItemActive3;
          if (this._activeQuestItems.TryGetValue(completedObjective.UniqueGroupID, out questItemActive3))
          {
            questItemActive3.AddObjectivesData(completedObjective);
          }
          else
          {
            QuestItemActive questItemActive4 = this._questItemActiveTemplate.Instantiate<QuestItemActive>((Transform) this._activeQuestsContent);
            questItemActive4.AddObjectivesData(completedObjective);
            this._activeQuestItems.Add(completedObjective.UniqueGroupID, questItemActive4);
          }
        }
      }
      foreach (ObjectivesDataFinalized data in DataManager.Instance.CompletedObjectivesHistory)
      {
        if (!string.IsNullOrEmpty(data.UniqueGroupID) && !this._activeQuestItems.ContainsKey(data.UniqueGroupID))
        {
          QuestItemInactive questItemInactive1;
          if (this._completedQuestItems.TryGetValue(data.UniqueGroupID, out questItemInactive1))
          {
            questItemInactive1.AddObjectivesData(data);
          }
          else
          {
            QuestItemInactive questItemInactive2 = this._questItemInActiveTemplate.Instantiate<QuestItemInactive>((Transform) this._completedQuestsContent);
            questItemInactive2.AddObjectivesData(data);
            this._completedQuestItems.Add(data.UniqueGroupID, questItemInactive2);
          }
        }
      }
      this._completedQuestsHeader.SetActive(this._completedQuestItems.Count > 0);
      this._completedQuestsContent.gameObject.SetActive(this._completedQuestItems.Count > 0);
      foreach (ObjectivesDataFinalized data in DataManager.Instance.FailedObjectivesHistory)
      {
        if (!string.IsNullOrEmpty(data.UniqueGroupID) && !this._completedQuestItems.ContainsKey(data.UniqueGroupID))
        {
          QuestItemInactive questItemInactive3;
          if (this._failedQuestItems.TryGetValue(data.UniqueGroupID, out questItemInactive3))
          {
            questItemInactive3.AddObjectivesData(data);
          }
          else
          {
            QuestItemInactive questItemInactive4 = this._questItemInActiveTemplate.Instantiate<QuestItemInactive>((Transform) this._failedQuestsContent);
            questItemInactive4.AddObjectivesData(data);
            this._failedQuestItems.Add(data.UniqueGroupID, questItemInactive4);
          }
        }
      }
      this._failedQuestsHeader.SetActive(this._failedQuestItems.Count > 0);
      this._failedQuestsContent.gameObject.SetActive(this._failedQuestItems.Count > 0);
      foreach (QuestItemBase<ObjectivesData> questItemBase in this._activeQuestItems.Values.ToList<QuestItemActive>())
        questItemBase.Configure();
      foreach (QuestItemBase<ObjectivesDataFinalized> questItemBase in this._completedQuestItems.Values.ToList<QuestItemInactive>())
        questItemBase.Configure();
      foreach (QuestItemBase<ObjectivesDataFinalized> questItemBase in this._failedQuestItems.Values.ToList<QuestItemInactive>())
        questItemBase.Configure(true);
      this._noActiveQuestsText.SetActive(this._activeQuestItems.Count == 0);
      this._noCompletedQuestsText.SetActive(this._completedQuestItems.Count == 0);
      this._noFailedQuestsText.SetActive(this._failedQuestItems.Count == 0);
      if (this._activeQuestItems.Count > 0)
        this.OverrideDefault((Selectable) this._activeQuestItems.Values.ToList<QuestItemActive>()[0].Button);
      else if (this._completedQuestItems.Count > 0)
        this.OverrideDefaultOnce((Selectable) this._completedQuestItems.Values.ToList<QuestItemInactive>()[0].Button);
      else if (this._failedQuestItems.Count > 0)
        this.OverrideDefault((Selectable) this._failedQuestItems.Values.ToList<QuestItemInactive>()[0].Button);
    }
    this.ActivateNavigation();
    this._scrollRect.enabled = true;
  }
}
