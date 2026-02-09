// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenu.UpgradeTreeTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
