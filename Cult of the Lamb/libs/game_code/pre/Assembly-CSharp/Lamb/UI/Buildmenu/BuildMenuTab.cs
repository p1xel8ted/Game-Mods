// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.BuildMenuTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class BuildMenuTab : MMTab
{
  [SerializeField]
  private GameObject _alert;
  [SerializeField]
  private UIBuildMenuController.Category _category;

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

  private void OnAlertRemoved(StructureBrain.TYPES type)
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

  private void OnDisable()
  {
    if (DataManager.Instance == null)
      return;
    StructureAlerts structures = DataManager.Instance.Alerts.Structures;
    structures.OnAlertRemoved = structures.OnAlertRemoved - new Action<StructureBrain.TYPES>(this.OnAlertRemoved);
  }
}
