// Decompiled with JetBrains decompiler
// Type: HUD_Specials
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine.UI;

#nullable disable
public class HUD_Specials : BaseMonoBehaviour
{
  public Image ProgressWheel;
  public TextMeshProUGUI Ammo;

  public float PLAYER_SPECIAL_CHARGE
  {
    get => DataManager.Instance.PLAYER_SPECIAL_CHARGE;
    set
    {
      if ((double) value > (double) DataManager.Instance.PLAYER_SPECIAL_CHARGE_TARGET)
        value = DataManager.Instance.PLAYER_SPECIAL_CHARGE_TARGET;
      DataManager.Instance.PLAYER_SPECIAL_CHARGE = value;
      this.ProgressWheel.fillAmount = this.PLAYER_SPECIAL_CHARGE / DataManager.Instance.PLAYER_SPECIAL_CHARGE_TARGET;
    }
  }

  public void OnEnable()
  {
    PlayerFarming.OnGetSoul += new PlayerFarming.GetSoulAction(this.OnGetSoul);
  }

  public void OnDisable()
  {
    PlayerFarming.OnGetSoul -= new PlayerFarming.GetSoulAction(this.OnGetSoul);
  }

  public void Start()
  {
    this.ProgressWheel.fillAmount = this.PLAYER_SPECIAL_CHARGE / DataManager.Instance.PLAYER_SPECIAL_CHARGE_TARGET;
    PlayerWeapon.OnSpecial += new PlayerWeapon.DoSpecialAction(this.OnSpecial);
  }

  public void OnSpecial()
  {
    this.Ammo.text = DataManager.Instance.PLAYER_SPECIAL_AMMO.ToString();
    double playerSpecialAmmo = (double) DataManager.Instance.PLAYER_SPECIAL_AMMO;
  }

  public void OnGetSoul(int DeltaValue)
  {
    if (DeltaValue <= 0 || (double) DataManager.Instance.PLAYER_SPECIAL_AMMO <= 0.0)
      return;
    this.PLAYER_SPECIAL_CHARGE += (float) DeltaValue;
  }
}
