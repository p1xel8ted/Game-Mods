// Decompiled with JetBrains decompiler
// Type: PlayerPoisonTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerPoisonTimer : BaseMonoBehaviour
{
  [SerializeField]
  private Canvas worldCanvas;
  [SerializeField]
  private GameObject poisonParent;
  [SerializeField]
  private Image poisonWheel;
  [SerializeField]
  private Gradient colorGradient;
  private HealthPlayer health;

  private void Awake()
  {
    this.worldCanvas.worldCamera = Camera.main;
    this.health = this.GetComponentInParent<HealthPlayer>();
  }

  private void Update()
  {
    this.poisonParent.SetActive(this.health.IsPoisoned);
    if (!this.health.IsPoisoned)
      return;
    this.poisonWheel.fillAmount = this.health.PoisonNormalisedTime;
    this.poisonWheel.color = this.colorGradient.Evaluate(this.health.PoisonNormalisedTime);
  }
}
