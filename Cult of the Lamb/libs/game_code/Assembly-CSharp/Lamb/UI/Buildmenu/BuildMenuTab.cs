// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.BuildMenuTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class BuildMenuTab : MMTab
{
  [SerializeField]
  public GameObject _alert;
  [SerializeField]
  public UIBuildMenuController.Category _category;

  public GameObject Alert => this._alert;

  public override void Configure()
  {
    if (DataManager.Instance.Alerts.Structures.GetAlertsForCategory(this._category).Count > 0)
    {
      this._alert.SetActive(true);
      StructureAlerts structures = DataManager.Instance.Alerts.Structures;
      structures.OnAlertRemoved = structures.OnAlertRemoved + new Action<StructureBrain.TYPES>(this.OnAlertRemoved);
    }
    else
      this._alert.SetActive(false);
  }

  public void OnAlertRemoved(StructureBrain.TYPES type)
  {
    if (DataManager.Instance.Alerts.Structures.GetAlertsForCategory(this._category).Count > 0)
    {
      this._alert.SetActive(true);
    }
    else
    {
      this._alert.SetActive(false);
      StructureAlerts structures = DataManager.Instance.Alerts.Structures;
      structures.OnAlertRemoved = structures.OnAlertRemoved - new Action<StructureBrain.TYPES>(this.OnAlertRemoved);
    }
  }

  public void OnDisable()
  {
    if (DataManager.Instance == null)
      return;
    StructureAlerts structures = DataManager.Instance.Alerts.Structures;
    structures.OnAlertRemoved = structures.OnAlertRemoved - new Action<StructureBrain.TYPES>(this.OnAlertRemoved);
  }
}
