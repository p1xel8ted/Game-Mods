// Decompiled with JetBrains decompiler
// Type: UIRacingTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class UIRacingTimer : MonoBehaviour
{
  [SerializeField]
  public TMP_Text timerText;
  [SerializeField]
  public TMP_Text gatesText;

  public void Awake()
  {
    Interaction_RacingGate.OnStartRace += new System.Action(this.OnStartRace);
    Interaction_RacingGate.OnFinishRace += new Action<float>(this.OnFinishRace);
    Interaction_RacingGate.OnGatePassed += new Action<int>(this.OnGatePassed);
    this.gameObject.SetActive(false);
  }

  public void OnDestroy()
  {
    Interaction_RacingGate.OnStartRace -= new System.Action(this.OnStartRace);
    Interaction_RacingGate.OnFinishRace -= new Action<float>(this.OnFinishRace);
    Interaction_RacingGate.OnGatePassed -= new Action<int>(this.OnGatePassed);
  }

  public void Update()
  {
    if (!Interaction_RacingGate.IsRaceActive)
      return;
    this.timerText.text = Math.Round((double) Interaction_RacingGate.RaceTimer, 2).ToString();
  }

  public void OnStartRace()
  {
    if (PlayerFarming.playersCount >= 2)
      return;
    this.gameObject.SetActive(true);
    this.transform.DOKill();
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutElastic);
  }

  public void OnGatePassed(int gatesPassed)
  {
    this.gatesText.transform.DOKill();
    this.gatesText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
    TMP_Text gatesText = this.gatesText;
    int num = gatesPassed + 1;
    string str1 = num.ToString();
    num = Interaction_RacingGate.RacingGates.Count;
    string str2 = num.ToString();
    string str3 = $"{str1}/{str2}";
    gatesText.text = str3;
  }

  public void OnFinishRace(float time)
  {
    float duration = 0.5f;
    float delay = 1.5f;
    if (!float.IsInfinity(time))
    {
      this.timerText.text = Math.Round((double) time, 2).ToString();
    }
    else
    {
      duration = 0.0f;
      delay = 0.0f;
    }
    this.transform.DOKill();
    this.transform.localScale = Vector3.one;
    this.transform.DOScale(Vector3.zero, duration).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(delay).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBounce).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
  }

  [CompilerGenerated]
  public void \u003COnFinishRace\u003Eb__7_0() => this.gameObject.SetActive(false);
}
