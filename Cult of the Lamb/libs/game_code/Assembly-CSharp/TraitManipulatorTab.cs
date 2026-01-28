// Decompiled with JetBrains decompiler
// Type: TraitManipulatorTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.Alerts;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class TraitManipulatorTab : MMTab
{
  [SerializeField]
  public GameObject _alert;
  [SerializeField]
  public UITraitManipulatorMenuController.Type _category;
  [SerializeField]
  public Image _image;
  [SerializeField]
  public GameObject _lockIcon;

  public GameObject Alert => this._alert;

  public override void Configure()
  {
    if (DataManager.Instance.Alerts.TraitManipulatorAlerts.HasAlert(this._category))
    {
      this._alert.SetActive(true);
      TraitManipulatorAlerts manipulatorAlerts = DataManager.Instance.Alerts.TraitManipulatorAlerts;
      manipulatorAlerts.OnAlertRemoved = manipulatorAlerts.OnAlertRemoved + new Action<UITraitManipulatorMenuController.Type>(this.OnAlertRemoved);
    }
    else
      this._alert.SetActive(false);
  }

  public void OnAlertRemoved(UITraitManipulatorMenuController.Type type)
  {
    if (DataManager.Instance.Alerts.TraitManipulatorAlerts.HasAlert(this._category))
    {
      this._alert.SetActive(true);
    }
    else
    {
      this._alert.SetActive(false);
      TraitManipulatorAlerts manipulatorAlerts = DataManager.Instance.Alerts.TraitManipulatorAlerts;
      manipulatorAlerts.OnAlertRemoved = manipulatorAlerts.OnAlertRemoved - new Action<UITraitManipulatorMenuController.Type>(this.OnAlertRemoved);
    }
  }

  public void OnDisable()
  {
    if (DataManager.Instance == null)
      return;
    TraitManipulatorAlerts manipulatorAlerts = DataManager.Instance.Alerts.TraitManipulatorAlerts;
    manipulatorAlerts.OnAlertRemoved = manipulatorAlerts.OnAlertRemoved - new Action<UITraitManipulatorMenuController.Type>(this.OnAlertRemoved);
  }

  public void SetLocked()
  {
    this._image.color = Color.grey;
    this._lockIcon.gameObject.SetActive(true);
    this.Button.Interactable = false;
  }

  public bool TrySetInteractable(bool interactable)
  {
    if ((UnityEngine.Object) this._lockIcon != (UnityEngine.Object) null && this._lockIcon.activeSelf)
      return false;
    this.Button.interactable = interactable;
    return true;
  }
}
