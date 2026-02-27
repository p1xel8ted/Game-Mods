// Decompiled with JetBrains decompiler
// Type: DoctrineCategoryNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DoctrineCategoryNotification : MonoBehaviour
{
  private SermonCategory _sermonCategory;
  private DoctrineUpgradeSystem.DoctrineType _doctrineType;

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

  private void OnAlertRemoved(DoctrineUpgradeSystem.DoctrineType doctrineType)
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
