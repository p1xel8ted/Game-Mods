// Decompiled with JetBrains decompiler
// Type: EnemyOrderGroupIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using UnityEngine;

#nullable disable
public class EnemyOrderGroupIndicator : MonoBehaviour
{
  public GameObject[] indicators;
  public GameObject[] indicatorsAlt;
  public GameObject redShield;
  public GameObject greenShield;
  public int _cachedOrder;
  public bool _numerals;

  public void OnEnable()
  {
    this._numerals = SettingsManager.Settings.Accessibility.RomanNumerals;
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged += new Action<bool>(this.OnRomanNumeralsChanged);
  }

  public void OnDisable()
  {
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged -= new Action<bool>(this.OnRomanNumeralsChanged);
  }

  public void SetIndicatorForOrder(int order)
  {
    Debug.Log((object) ("Setting indicator for " + order.ToString()));
    for (int index = 0; index < this.indicators.Length; ++index)
      this.indicators[index].SetActive(order == index && this._numerals);
    for (int index = 0; index < this.indicatorsAlt.Length; ++index)
      this.indicatorsAlt[index].SetActive(order == index && !this._numerals);
    this.SetVulnerable(order == 0);
    this._cachedOrder = order;
  }

  public void SetVulnerable(bool vulnerable)
  {
    this.greenShield.SetActive(vulnerable);
    this.redShield.SetActive(!vulnerable);
    if (!vulnerable)
      return;
    this.transform.localScale = Vector3.one;
    this.transform.DOKill();
    this.transform.DOPunchScale(Vector3.one * 1.25f, 0.25f);
  }

  public void OnRomanNumeralsChanged(bool state)
  {
    this._numerals = state;
    this.SetIndicatorForOrder(this._cachedOrder);
  }
}
