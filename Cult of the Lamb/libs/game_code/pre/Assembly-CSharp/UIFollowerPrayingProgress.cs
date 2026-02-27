// Decompiled with JetBrains decompiler
// Type: UIFollowerPrayingProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIFollowerPrayingProgress : BaseMonoBehaviour
{
  [SerializeField]
  private Image _radialProgress;
  [SerializeField]
  private Follower _follower;
  private bool _shown;
  private bool _flashing;

  private void Awake()
  {
    this.transform.localScale = Vector3.zero;
    this.gameObject.SetActive(false);
  }

  public void Show()
  {
    if (this._shown)
      return;
    this.gameObject.SetActive(true);
    this._shown = true;
    this.transform.DOKill();
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void Hide()
  {
    if (!this._shown)
      return;
    this._shown = false;
    this.transform.DOKill();
    this.transform.localScale = Vector3.one;
    this.transform.DOScale(Vector3.zero, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
  }

  private void Update()
  {
    if (!this._shown || this._flashing || (Object) this._follower == (Object) null || this._follower.Brain == null || this._follower.Brain.Info == null || this._follower.Brain.CurrentTask == null)
      return;
    float num = 0.0f;
    if (this._follower.Brain.CurrentTask is FollowerTask_Pray currentTask2)
      num = currentTask2.GetDurationPerDevotion(this._follower);
    else if (this._follower.Brain.CurrentTask is FollowerTask_PrayPassive currentTask1)
      num = currentTask1.GetDurationPerDevotion(this._follower);
    this._radialProgress.fillAmount = this._follower.Brain.Info.PrayProgress / num;
  }

  public void Flash()
  {
    if (!this.gameObject.activeInHierarchy)
      this.gameObject.gameObject.SetActive(true);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoFlash());
  }

  private IEnumerator DoFlash()
  {
    this._flashing = true;
    Color transparent = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    this._radialProgress.color = Color.white;
    yield return (object) new WaitForSeconds(0.2f);
    DOTweenModuleUI.DOColor(this._radialProgress, transparent, 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    this._radialProgress.fillAmount = 0.0f;
    this._radialProgress.color = StaticColors.RedColor;
    this._flashing = false;
  }
}
