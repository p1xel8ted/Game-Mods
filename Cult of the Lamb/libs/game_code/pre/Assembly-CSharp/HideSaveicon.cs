// Decompiled with JetBrains decompiler
// Type: HideSaveicon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class HideSaveicon : MonoBehaviour
{
  [SerializeField]
  private CanvasGroup _canvasGroup;
  public static HideSaveicon instance;

  private void Awake()
  {
    if ((UnityEngine.Object) HideSaveicon.instance == (UnityEngine.Object) null)
      HideSaveicon.instance = this;
    this.gameObject.SetActive(false);
  }

  public void StartRoutineSave(System.Action callback)
  {
    this.gameObject.SetActive(true);
    this._canvasGroup.DOKill();
    this._canvasGroup.alpha = 1f;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Saving(callback));
  }

  private IEnumerator Saving(System.Action callback)
  {
    HideSaveicon hideSaveicon = this;
    yield return (object) new WaitForEndOfFrame();
    callback();
    yield return (object) new WaitForSecondsRealtime(0.25f);
    hideSaveicon._canvasGroup.DOFade(0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    hideSaveicon.gameObject.SetActive(false);
  }
}
