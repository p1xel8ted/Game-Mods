// Decompiled with JetBrains decompiler
// Type: WeatherVane
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WeatherVane : MonoBehaviour
{
  [SerializeField]
  public Animator animator;
  [SerializeField]
  public Animator ballRotationAnimator;
  [SerializeField]
  public RuntimeAnimatorController[] winterControllers;
  [SerializeField]
  public RuntimeAnimatorController[] blizzardControllers;
  public float state;

  public void OnEnable()
  {
    this.state = 0.0f;
    this.ResetAniamtor(this.ballRotationAnimator);
  }

  public void Update()
  {
    this.state = SeasonsManager.CurrentSeason != SeasonsManager.Season.Spring || !SeasonsManager.Active || (double) SeasonsManager.SEASON_NORMALISED_PROGRESS < 0.89999997615814209 && TimeManager.CurrentDay + 1 <= SeasonsManager.SeasonTimestamp ? (!SeasonsManager.NextPhaseIsWeatherEvent || SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.None ? (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard ? 1f : 0.0f) : 0.5f) : 0.5f;
    this.animator.SetFloat("state", this.state);
    int num = SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter ? DataManager.Instance.WinterServerity : DataManager.Instance.NextWinterServerity;
    bool flag = SeasonsManager.NextPhaseIsWeatherEvent && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter;
    int index = num > 1 ? (num < 3 ? 1 : 2) : 0;
    this.animator.runtimeAnimatorController = flag || SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard ? this.blizzardControllers[index] : this.winterControllers[index];
    this.animator.SetBool("blizzardComing", flag);
  }

  public void ResetAniamtor(Animator anim)
  {
    anim.enabled = false;
    anim.enabled = true;
  }
}
