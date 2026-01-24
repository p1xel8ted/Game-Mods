// Decompiled with JetBrains decompiler
// Type: Interaction_WeatherVane
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_WeatherVane : Interaction
{
  [SerializeField]
  public GameObject interactionAvailable;

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void OnEnable()
  {
    base.OnEnable();
    this.interactionAvailable.gameObject.SetActive(!DataManager.Instance.HasWeatherVaneUI);
  }

  public override void OnInteract(StateMachine state)
  {
    if (DataManager.Instance.HasWeatherVaneUI)
      return;
    base.OnInteract(state);
    HUD_Winter.Instance.Reveal();
    this.interactionAvailable.gameObject.SetActive(false);
  }

  public override void GetLabel()
  {
    if (DataManager.Instance.HasWeatherVaneUI)
    {
      this.Label = "";
      this.Interactable = false;
    }
    else
    {
      base.GetLabel();
      this.Label = LocalizationManager.GetTranslation("Interactions/ConsultTheHeavens");
    }
  }
}
