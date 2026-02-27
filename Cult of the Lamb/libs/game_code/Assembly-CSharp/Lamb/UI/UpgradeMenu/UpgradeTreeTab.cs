// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenu.UpgradeTreeTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI.UpgradeMenu;

public class UpgradeTreeTab : MMTab
{
  [SerializeField]
  public GameObject _alert;
  [SerializeField]
  public GameObject _lock;
  [SerializeField]
  public bool _isDLC;

  public GameObject Alert => this._alert;

  public override void Configure()
  {
  }

  public void OnAlertRemoved(StructureBrain.TYPES type)
  {
  }

  public void OnDisable()
  {
    if (DataManager.Instance == null)
      return;
    StructureAlerts structures = DataManager.Instance.Alerts.Structures;
    structures.OnAlertRemoved = structures.OnAlertRemoved - new Action<StructureBrain.TYPES>(this.OnAlertRemoved);
  }
}
