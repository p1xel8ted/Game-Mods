// Decompiled with JetBrains decompiler
// Type: JudgementMeter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class JudgementMeter : MonoBehaviour
{
  public static bool JudgementEnabled;
  public static JudgementMeter instance;
  public const int Max = 4;
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public Image needle;
  [SerializeField]
  public GameObject topGlow;
  [SerializeField]
  public GameObject bottomGlow;

  public int Judgement
  {
    get => DataManager.Instance.JudgementAmount;
    set => DataManager.Instance.JudgementAmount = value;
  }

  public static void Show()
  {
    if (!JudgementMeter.JudgementEnabled)
      return;
    if ((Object) JudgementMeter.instance == (Object) null)
      JudgementMeter.instance = Object.Instantiate<JudgementMeter>(UnityEngine.Resources.Load<JudgementMeter>("Prefabs/UI/UI Judgement Meter"), GameObject.FindGameObjectWithTag("Canvas").transform);
    JudgementMeter.instance.gameObject.SetActive(true);
    JudgementMeter.instance.canvasGroup.alpha = 0.0f;
    JudgementMeter.instance.canvasGroup.DOFade(1f, 0.25f);
    JudgementMeter.instance.needle.transform.localPosition = new Vector3(JudgementMeter.instance.needle.transform.localPosition.x, (float) (48 /*0x30*/ * JudgementMeter.instance.Judgement), JudgementMeter.instance.needle.transform.localPosition.z);
    JudgementMeter.instance.topGlow.SetActive(JudgementMeter.instance.Judgement >= 4);
    JudgementMeter.instance.bottomGlow.SetActive(JudgementMeter.instance.Judgement <= -4);
  }

  public void Hide()
  {
    this.canvasGroup.DOFade(0.0f, 0.25f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
  }

  public static void ShowModify(int increment)
  {
    if (!JudgementMeter.JudgementEnabled)
    {
      DataManager.Instance.JudgementAmount += increment;
    }
    else
    {
      JudgementMeter.Show();
      JudgementMeter.instance.StartCoroutine((IEnumerator) JudgementMeter.instance.ModifyIE(increment));
    }
  }

  public IEnumerator ModifyIE(int increment)
  {
    yield return (object) new WaitForSeconds(1f);
    int previous = this.Judgement;
    this.Judgement = Mathf.Clamp(this.Judgement + increment, -4, 4);
    if (previous != this.Judgement)
    {
      this.needle.transform.DOLocalMove(new Vector3(JudgementMeter.instance.needle.transform.localPosition.x, (float) (48 /*0x30*/ * JudgementMeter.instance.Judgement), JudgementMeter.instance.needle.transform.localPosition.z), 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        if (JudgementMeter.instance.Judgement >= 4)
        {
          Vector3 localScale = JudgementMeter.instance.topGlow.transform.localScale;
          JudgementMeter.instance.topGlow.SetActive(true);
          JudgementMeter.instance.topGlow.transform.localScale = Vector3.zero;
          JudgementMeter.instance.topGlow.transform.DOScale(localScale, 0.25f);
        }
        else if (JudgementMeter.instance.Judgement <= -4)
        {
          Vector3 localScale = JudgementMeter.instance.bottomGlow.transform.localScale;
          JudgementMeter.instance.bottomGlow.SetActive(true);
          JudgementMeter.instance.bottomGlow.transform.localScale = Vector3.zero;
          JudgementMeter.instance.bottomGlow.transform.DOScale(localScale, 0.25f);
        }
        else if (previous >= 4)
        {
          JudgementMeter.instance.topGlow.transform.DOScale(0.0f, 0.25f);
        }
        else
        {
          if (previous > -4)
            return;
          JudgementMeter.instance.bottomGlow.transform.DOScale(0.0f, 0.25f);
        }
      }));
    }
    else
    {
      Vector3 p = this.needle.transform.localPosition;
      this.needle.transform.DOShakePosition(0.5f, 10f).OnComplete<Tweener>((TweenCallback) (() => this.needle.transform.localPosition = p));
    }
    yield return (object) new WaitForSeconds(2f);
    this.Hide();
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__11_0() => this.gameObject.SetActive(false);
}
