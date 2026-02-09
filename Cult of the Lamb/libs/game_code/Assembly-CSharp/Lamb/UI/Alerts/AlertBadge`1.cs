// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.AlertBadge`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Alerts;

public abstract class AlertBadge<T> : AlertBadgeBase
{
  [SerializeField]
  public AlertBadge<T>.AlertMode _alertMode;
  public T _alert;
  public bool _configured;

  public abstract AlertCategory<T> _source { get; }

  public void Start() => this.ConfigureSingle();

  public void Configure(T alert)
  {
    if (this._configured || this._alertMode != AlertBadge<T>.AlertMode.Single)
      return;
    this._alert = alert;
    if (this.HasAlertSingle())
      this.gameObject.SetActive(true);
    else
      this.gameObject.SetActive(false);
    this._source.OnAlertRemoved += new Action<T>(this.OnAlertRemoved);
    this._configured = true;
  }

  public void ConfigureSingle()
  {
    if (this._configured || this._alertMode != AlertBadge<T>.AlertMode.Total)
      return;
    if (this.HasAlertTotal())
    {
      this.gameObject.SetActive(true);
      this._source.OnAlertRemoved += new Action<T>(this.OnAlertRemoved);
    }
    else
      this.gameObject.SetActive(false);
    this._configured = true;
  }

  public virtual bool HasAlertSingle() => this._source.HasAlert(this._alert);

  public virtual bool HasAlertTotal() => this._source.Total > 0;

  public new void OnDisable() => this.StopAllCoroutines();

  public void OnDestroy()
  {
    if (this._source == null)
      return;
    Delegate[] invocationList = this._source.OnAlertRemoved?.GetInvocationList();
    if (invocationList == null || invocationList.Length == 0)
      return;
    this._source.OnAlertRemoved -= new Action<T>(this.OnAlertRemoved);
  }

  public void OnAlertRemoved(T alert)
  {
    if (this._alertMode == AlertBadge<T>.AlertMode.Single)
    {
      if (!alert.Equals((object) this._alert))
        return;
      if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
        this.gameObject.SetActive(false);
      this._source.OnAlertRemoved -= new Action<T>(this.OnAlertRemoved);
    }
    else
      this.gameObject.SetActive(this._source.Total != 0);
  }

  public bool TryRemoveAlert() => this._source.Remove(this._alert);

  public bool HasAlert() => this._source.HasAlert(this._alert);

  public void ResetAlert()
  {
    this._source.OnAlertRemoved -= new Action<T>(this.OnAlertRemoved);
    this._configured = false;
  }

  [Serializable]
  public enum AlertMode
  {
    Single,
    Total,
  }
}
