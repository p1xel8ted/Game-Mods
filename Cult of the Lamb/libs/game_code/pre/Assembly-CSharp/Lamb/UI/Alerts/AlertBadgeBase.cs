// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.AlertBadgeBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Alerts;

public class AlertBadgeBase : BaseMonoBehaviour
{
  private Vector3 _wobble;
  private bool _performedCTA;

  private void OnEnable()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.WobbleAlert());
  }

  private void OnDisable() => this.StopAllCoroutines();

  private IEnumerator CTA()
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

  private IEnumerator WobbleAlert()
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
      alertBadgeBase.transform.eulerAngles = alertBadgeBase._wobble;
      yield return (object) null;
    }
  }
}
