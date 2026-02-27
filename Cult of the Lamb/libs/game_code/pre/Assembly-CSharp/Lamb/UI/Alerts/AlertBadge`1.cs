// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.AlertBadge`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Alerts;

public abstract class AlertBadge<T> : AlertBadgeBase
{
  [SerializeField]
  protected AlertBadge<T>.AlertMode _alertMode;
  protected T _alert;
  private bool _configured;

  protected abstract AlertCategory<T> _source { get; }

  private void Start()
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

  public void Configure(T alert)
  {
    if (this._configured || this._alertMode != AlertBadge<T>.AlertMode.Single)
      return;
    this._alert = alert;
    if (this.HasAlertSingle())
    {
      this.gameObject.SetActive(true);
      this._source.OnAlertRemoved += new Action<T>(this.OnAlertRemoved);
    }
    else
      this.gameObject.SetActive(false);
    this._configured = true;
  }

  protected virtual bool HasAlertSingle() => this._source.HasAlert(this._alert);

  protected virtual bool HasAlertTotal() => this._source.Total > 0;

  private void OnDisable() => this.StopAllCoroutines();

  private void OnDestroy() => this._source.OnAlertRemoved -= new Action<T>(this.OnAlertRemoved);

  private void OnAlertRemoved(T alert)
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

  [Serializable]
  public enum AlertMode
  {
    Single,
    Total,
  }
}
