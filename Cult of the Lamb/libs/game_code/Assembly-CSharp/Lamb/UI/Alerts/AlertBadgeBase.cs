// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.AlertBadgeBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Alerts;

public class AlertBadgeBase : BaseMonoBehaviour
{
  public Vector3 _wobble;
  [SerializeField]
  public bool _performedCTA;

  public void OnEnable()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.WobbleAlert());
  }

  public void OnDisable() => this.StopAllCoroutines();

  public IEnumerator CTA()
  {
    AlertBadgeBase alertBadgeBase = this;
    alertBadgeBase._performedCTA = true;
    float progress = 0.0f;
    float duration = 0.3f;
    float targetScale = alertBadgeBase.transform.localScale.x;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      alertBadgeBase.transform.localScale = Vector3.one * Mathf.Lerp(1f, targetScale, progress / duration);
      yield return (object) null;
    }
    alertBadgeBase.transform.localScale = Vector3.one * targetScale;
  }

  public IEnumerator WobbleAlert()
  {
    AlertBadgeBase alertBadgeBase = this;
    if (!alertBadgeBase._performedCTA)
      yield return (object) alertBadgeBase.CTA();
    float wobble = (float) Random.Range(0, 360);
    float wobbleSpeed = 4f;
    while (true)
    {
      wobble += wobbleSpeed * Time.unscaledDeltaTime;
      alertBadgeBase._wobble.z = 15f * Mathf.Cos(wobble);
      alertBadgeBase.transform.localRotation = Quaternion.Euler(alertBadgeBase._wobble);
      yield return (object) null;
    }
  }
}
