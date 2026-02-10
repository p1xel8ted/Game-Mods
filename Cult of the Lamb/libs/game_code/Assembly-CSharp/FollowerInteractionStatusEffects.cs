// Decompiled with JetBrains decompiler
// Type: FollowerInteractionStatusEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerInteractionStatusEffects : BaseMonoBehaviour
{
  public Follower follower;
  public GameObject IconPrefab;
  public RectTransform _rectTransform;

  public RectTransform rectTransform
  {
    set => this._rectTransform = value;
    get
    {
      if ((Object) this._rectTransform == (Object) null)
        this._rectTransform = this.GetComponent<RectTransform>();
      return this._rectTransform;
    }
  }

  public void Init(Follower follower)
  {
    float num1 = 0.1f;
    this.follower = follower;
    if (!FollowerBrainStats.Fasting)
      return;
    float num2;
    this.AddEffect(true, num2 = num1 + 0.1f, FollowerInteractionStatusEffects.EffectIcon.Fasting);
  }

  public void AddEffect(bool Instant, FollowerInteractionStatusEffects.EffectIcon Effect)
  {
    this.AddEffect(Instant, 0.0f, Effect);
  }

  public void AddEffect(
    bool Instant,
    float Delay,
    FollowerInteractionStatusEffects.EffectIcon Effect)
  {
    this.StartCoroutine((IEnumerator) this.AddEffectRoutine(Instant, Delay, Effect));
  }

  public IEnumerator AddEffectRoutine(
    bool Instant,
    float Delay,
    FollowerInteractionStatusEffects.EffectIcon Effect)
  {
    FollowerInteractionStatusEffects interactionStatusEffects = this;
    yield return (object) new WaitForSeconds(Delay);
    GameObject gameObject = Object.Instantiate<GameObject>(interactionStatusEffects.IconPrefab, interactionStatusEffects.transform);
    gameObject.SetActive(true);
    FollowerInteractionStatusEffectIcon component = gameObject.GetComponent<FollowerInteractionStatusEffectIcon>();
    if (Effect == FollowerInteractionStatusEffects.EffectIcon.Fasting)
    {
      component.Text.text = "Fasting";
    }
    else
    {
      component.Text.text = "Test Effect!";
      component.RadialProgress.fillAmount = 0.2f;
    }
    interactionStatusEffects.StartCoroutine((IEnumerator) interactionStatusEffects.ScaleIcon(gameObject.transform));
  }

  public IEnumerator ScaleIcon(Transform t)
  {
    float Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      t.localScale = Vector3.Lerp(Vector3.one * 0.0f, Vector3.one, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
  }

  public enum EffectIcon
  {
    Fasting,
  }
}
