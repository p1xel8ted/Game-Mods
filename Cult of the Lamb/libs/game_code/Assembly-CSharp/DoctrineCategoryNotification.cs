// Decompiled with JetBrains decompiler
// Type: DoctrineCategoryNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DoctrineCategoryNotification : MonoBehaviour
{
  public SermonCategory _sermonCategory;
  public DoctrineUpgradeSystem.DoctrineType _doctrineType;

  public void Configure(SermonCategory sermonCategory)
  {
    this._sermonCategory = sermonCategory;
    this.gameObject.SetActive(DataManager.Instance.Alerts.Doctrine.GetAlertsForCategory(this._sermonCategory).Count > 0);
  }

  public void Configure(DoctrineUpgradeSystem.DoctrineType type)
  {
    this._doctrineType = type;
    this.gameObject.SetActive(DataManager.Instance.Alerts.Doctrine.HasAlert(type));
  }

  public void OnEnable()
  {
    DoctrineAlerts doctrine = DataManager.Instance.Alerts.Doctrine;
    doctrine.OnAlertRemoved = doctrine.OnAlertRemoved + new Action<DoctrineUpgradeSystem.DoctrineType>(this.OnAlertRemoved);
  }

  public void OnDisable()
  {
    if (DataManager.Instance == null)
      return;
    DoctrineAlerts doctrine = DataManager.Instance.Alerts.Doctrine;
    doctrine.OnAlertRemoved = doctrine.OnAlertRemoved - new Action<DoctrineUpgradeSystem.DoctrineType>(this.OnAlertRemoved);
  }

  public void OnAlertRemoved(DoctrineUpgradeSystem.DoctrineType doctrineType)
  {
    if (this._sermonCategory != SermonCategory.None)
    {
      this.Configure(this._sermonCategory);
    }
    else
    {
      if (this._doctrineType == DoctrineUpgradeSystem.DoctrineType.None)
        return;
      this.Configure(this._doctrineType);
    }
  }

  public void TryRemoveAlert() => DataManager.Instance.Alerts.Doctrine.Remove(this._doctrineType);
}
