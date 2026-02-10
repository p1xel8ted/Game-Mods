// Decompiled with JetBrains decompiler
// Type: HUD_AbilityIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class HUD_AbilityIcon : MonoBehaviour
{
  public Transform Container;

  public void Play(float Delay) => this.StartCoroutine((IEnumerator) this.TweenInRoutine(Delay));

  public IEnumerator TweenInRoutine(float Delay)
  {
    this.Container.transform.localPosition = new Vector3(0.0f, -200f);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForSeconds(Delay);
    DG.Tweening.Sequence s = DOTween.Sequence();
    s.Append((Tween) this.Container.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    s.Append((Tween) this.Container.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    s.Append((Tween) this.Container.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
  }
}
