// Decompiled with JetBrains decompiler
// Type: PlayerEffectTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerEffectTimer : BaseMonoBehaviour
{
  [SerializeField]
  public PlayerEffectTimer.Effect effect;
  [SerializeField]
  public Canvas worldCanvas;
  [SerializeField]
  public GameObject effectParent;
  [SerializeField]
  public Image wheel;
  [SerializeField]
  public Gradient colorGradient;
  public HealthPlayer health;

  public void Awake()
  {
    this.worldCanvas.worldCamera = Camera.main;
    this.health = this.GetComponentInParent<HealthPlayer>();
  }

  public void Update()
  {
    bool flag;
    float time;
    switch (this.effect)
    {
      case PlayerEffectTimer.Effect.Poison:
        flag = this.health.IsPoisoned;
        time = this.health.PoisonNormalisedTime;
        break;
      case PlayerEffectTimer.Effect.Burn:
        flag = this.health.IsBurned;
        time = this.health.BurnNormalisedTime;
        break;
      default:
        flag = this.health.IsPoisoned;
        time = this.health.PoisonNormalisedTime;
        break;
    }
    this.effectParent.SetActive(flag);
    if (!flag)
      return;
    this.wheel.fillAmount = time;
    this.wheel.color = this.colorGradient.Evaluate(time);
  }

  public enum Effect
  {
    Poison,
    Burn,
  }
}
