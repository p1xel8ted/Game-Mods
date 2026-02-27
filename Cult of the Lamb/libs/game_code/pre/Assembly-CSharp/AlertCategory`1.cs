// Decompiled with JetBrains decompiler
// Type: AlertCategory`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
[Serializable]
public abstract class AlertCategory<T>
{
  [XmlIgnore]
  [NonSerialized]
  public Action<T> OnAlertAdded;
  [XmlIgnore]
  [NonSerialized]
  public Action<T> OnAlertRemoved;
  public List<T> _alerts = new List<T>();
  public List<T> _singleAlerts = new List<T>();

  public virtual int Total => this._alerts.Count;

  protected virtual bool IsValidAlert(T alert) => true;

  public bool Add(T alert)
  {
    if (!this.IsValidAlert(alert) || this._alerts.Contains(alert) || this._singleAlerts.Contains(alert))
      return false;
    this._alerts.Add(alert);
    Action<T> onAlertAdded = this.OnAlertAdded;
    if (onAlertAdded != null)
      onAlertAdded(alert);
    return true;
  }

  public bool AddOnce(T alert)
  {
    if (!this.Add(alert))
      return false;
    this._singleAlerts.Add(alert);
    return true;
  }

  public bool Remove(T alert)
  {
    if (!this._alerts.Contains(alert))
      return false;
    while (this._alerts.Contains(alert))
      this._alerts.Remove(alert);
    Action<T> onAlertRemoved = this.OnAlertRemoved;
    if (onAlertRemoved != null)
      onAlertRemoved(alert);
    return true;
  }

  public virtual bool HasAlert(T alert) => this._alerts.Contains(alert);

  public void Clear() => this._alerts.Clear();

  public void ClearAll()
  {
    this._alerts.Clear();
    this._singleAlerts.Clear();
  }
}
