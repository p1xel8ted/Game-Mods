// Decompiled with JetBrains decompiler
// Type: AlertCategory`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
[Serializable]
public abstract class AlertCategory<T>
{
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public Action<T> OnAlertAdded;
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public Action<T> OnAlertRemoved;
  [Key(0)]
  public List<T> _alerts = new List<T>();
  [Key(1)]
  public List<T> _singleAlerts = new List<T>();

  [IgnoreMember]
  public virtual int Total => this._alerts.Count;

  public virtual bool IsValidAlert(T alert) => true;

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
