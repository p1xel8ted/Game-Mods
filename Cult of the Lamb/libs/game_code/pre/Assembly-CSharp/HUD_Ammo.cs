// Decompiled with JetBrains decompiler
// Type: HUD_Ammo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Ammo : BaseMonoBehaviour
{
  public List<HUD_AmmoIcon> Icons = new List<HUD_AmmoIcon>();
  public Image ReloadRadialProgress;
  private Coroutine cReloadingRoutine;

  private void OnEnable()
  {
    this.ReloadRadialProgress.gameObject.SetActive(false);
    PlayerArrows.OnAmmoUpdated += new PlayerArrows.AmmoUpdated(this.OnAmmoUpdated);
    PlayerArrows.OnNoAmmoShake += new PlayerArrows.AmmoUpdated(this.OnNoAmmoShake);
    PlayerArrows.OnBeginReloading += new PlayerArrows.AmmoUpdated(this.OnBeginReloading);
  }

  private void OnBeginReloading(PlayerArrows playerArrows)
  {
    if (this.cReloadingRoutine != null)
      this.StopCoroutine(this.cReloadingRoutine);
    this.cReloadingRoutine = this.StartCoroutine((IEnumerator) this.ReloadingRoutine(playerArrows));
  }

  private void TestPostion(int Num)
  {
    this.ReloadRadialProgress.rectTransform.anchoredPosition = new Vector2((float) (32 /*0x20*/ * Num + 10), 0.0f);
  }

  private IEnumerator ReloadingRoutine(PlayerArrows playerArrows)
  {
    this.ReloadRadialProgress.gameObject.SetActive(true);
    this.ReloadRadialProgress.rectTransform.anchoredPosition = new Vector2((float) (32 /*0x20*/ * (playerArrows.PLAYER_ARROW_TOTAL_AMMO + playerArrows.PLAYER_SPIRIT_TOTAL_AMMO) + 10), 0.0f);
    while (DataManager.Instance.PLAYER_ARROW_AMMO < DataManager.Instance.PLAYER_ARROW_TOTAL_AMMO)
    {
      this.ReloadRadialProgress.fillAmount = playerArrows.ReloadProgress / playerArrows.ReloadTarget;
      yield return (object) null;
    }
    this.ReloadRadialProgress.gameObject.SetActive(false);
  }

  public void Play()
  {
    PlayerArrows.OnAmmoUpdated += new PlayerArrows.AmmoUpdated(this.OnAmmoUpdated);
    PlayerArrows.OnNoAmmoShake += new PlayerArrows.AmmoUpdated(this.OnNoAmmoShake);
    PlayerArrows.OnBeginReloading += new PlayerArrows.AmmoUpdated(this.OnBeginReloading);
    this.OnAmmoUpdated(Object.FindObjectOfType<PlayerArrows>());
  }

  private void OnDisable()
  {
    PlayerArrows.OnAmmoUpdated -= new PlayerArrows.AmmoUpdated(this.OnAmmoUpdated);
    PlayerArrows.OnNoAmmoShake -= new PlayerArrows.AmmoUpdated(this.OnNoAmmoShake);
    PlayerArrows.OnBeginReloading -= new PlayerArrows.AmmoUpdated(this.OnBeginReloading);
  }

  private void OnAmmoUpdated(PlayerArrows playerArrows) => this.UpdateAmmo(playerArrows);

  private void UpdateAmmo(PlayerArrows playerArrows)
  {
    int index = -1;
    float num = -0.05f;
    while (++index < this.Icons.Count)
    {
      if (index < playerArrows.PLAYER_ARROW_TOTAL_AMMO)
      {
        if (index < playerArrows.PLAYER_ARROW_AMMO)
          this.Icons[index].SetMode(HUD_AmmoIcon.Mode.ON, num += 0.05f);
        else
          this.Icons[index].SetMode(HUD_AmmoIcon.Mode.EMPTY, num += 0.05f);
      }
      else if (index >= playerArrows.PLAYER_ARROW_TOTAL_AMMO && index < playerArrows.PLAYER_ARROW_TOTAL_AMMO + playerArrows.PLAYER_SPIRIT_TOTAL_AMMO)
      {
        if (index < playerArrows.PLAYER_ARROW_TOTAL_AMMO + playerArrows.PLAYER_SPIRIT_AMMO)
          this.Icons[index].SetMode(HUD_AmmoIcon.Mode.ON_SPIRIT, num += 0.05f);
        else
          this.Icons[index].SetMode(HUD_AmmoIcon.Mode.EMPTY_SPIRIT, num += 0.05f);
      }
      else
        this.Icons[index].SetMode(HUD_AmmoIcon.Mode.OFF, 0.0f);
    }
  }

  public void OnNoAmmoShake(PlayerArrows playerArrows)
  {
    int index = -1;
    while (++index < this.Icons.Count)
      this.Icons[index].StartShake();
  }
}
