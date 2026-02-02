// Decompiled with JetBrains decompiler
// Type: ControllerRecommendedSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
