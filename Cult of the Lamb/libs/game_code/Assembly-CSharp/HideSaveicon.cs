// Decompiled with JetBrains decompiler
// Type: HideSaveicon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class HideSaveicon : MonoBehaviour
{
  [SerializeField]
  public CanvasGroup _canvasGroup;
  public static HideSaveicon instance;
  public bool CallbackCalled;

  public static bool IsSaving()
  {
    return (UnityEngine.Object) HideSaveicon.instance != (UnityEngine.Object) null && (UnityEngine.Object) HideSaveicon.instance.gameObject != (UnityEngine.Object) null && HideSaveicon.instance.gameObject.activeSelf;
  }

  public void Awake()
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

  public IEnumerator Saving(System.Action callback)
  {
    HideSaveicon hideSaveicon = this;
    yield return (object) new WaitForEndOfFrame();
    if (!hideSaveicon.CallbackCalled)
    {
      callback();
      hideSaveicon.CallbackCalled = true;
    }
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSecondsRealtime(0.25f);
    hideSaveicon._canvasGroup.DOFade(0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    hideSaveicon.gameObject.SetActive(false);
    hideSaveicon.CallbackCalled = false;
  }
}
