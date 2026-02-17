// Decompiled with JetBrains decompiler
// Type: CultUpgradeProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CultUpgradeProgress : MonoBehaviour
{
  public List<Image> Images;
  public Image WhiteFade;
  public Color ActiveColor;
  public Color InActiveColor;
  public DG.Tweening.Sequence sequence;

  public void Configure(bool Animate)
  {
    this.WhiteFade.enabled = false;
    int num = 1 + Mathf.Max(0, DataManager.Instance.TempleLevel) - 1;
    int index = -1;
    while (++index < this.Images.Count)
    {
      if (index < num)
        this.Images[index].color = this.ActiveColor;
      else
        this.Images[index].color = this.InActiveColor;
    }
    if (!Animate)
      return;
    Debug.Log((object) ("TempleLevel - 1:" + (num - 1).ToString()));
    this.sequence.Kill();
    this.Images[num - 1].transform.localScale = Vector3.one * 2f;
    this.sequence = DOTween.Sequence();
    this.sequence.AppendInterval(0.2f);
    this.sequence.Append((Tween) this.Images[num - 1].transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    this.WhiteFade.enabled = true;
    this.WhiteFade.color = this.InActiveColor;
    this.WhiteFade.transform.position = this.Images[num - 1].transform.position;
    this.WhiteFade.transform.DOKill();
    this.WhiteFade.transform.DOScale(Vector3.one * 3f, 0.5f);
    this.WhiteFade.DOKill();
    DOTweenModuleUI.DOFade(this.WhiteFade, 0.0f, 0.5f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
    {
      this.WhiteFade.transform.localScale = Vector3.one;
      this.WhiteFade.enabled = false;
    }));
  }

  [CompilerGenerated]
  public void \u003CConfigure\u003Eb__5_0()
  {
    this.WhiteFade.transform.localScale = Vector3.one;
    this.WhiteFade.enabled = false;
  }
}
