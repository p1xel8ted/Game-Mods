// Decompiled with JetBrains decompiler
// Type: ControllerRecommendedSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class ControllerRecommendedSfx : MonoBehaviour
{
  [SerializeField]
  public GameObject deceasedWarning;

  public void PlayDing()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/glass_ball_ding");
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact);
  }

  public void OnEnable()
  {
    if (!(LocalizationManager.CurrentLanguage != "English"))
      return;
    this.deceasedWarning.gameObject.SetActive(false);
  }
}
