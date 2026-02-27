// Decompiled with JetBrains decompiler
// Type: HUD_Specials
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private void OnEnable()
  {
    PlayerFarming.OnGetSoul += new PlayerFarming.GetSoulAction(this.OnGetSoul);
  }

  private void OnDisable()
  {
    PlayerFarming.OnGetSoul -= new PlayerFarming.GetSoulAction(this.OnGetSoul);
  }

  private void Start()
  {
    this.ProgressWheel.fillAmount = this.PLAYER_SPECIAL_CHARGE / DataManager.Instance.PLAYER_SPECIAL_CHARGE_TARGET;
    PlayerWeapon.OnSpecial += new PlayerWeapon.DoSpecialAction(this.OnSpecial);
  }

  private void OnSpecial()
  {
    this.Ammo.text = DataManager.Instance.PLAYER_SPECIAL_AMMO.ToString();
    double playerSpecialAmmo = (double) DataManager.Instance.PLAYER_SPECIAL_AMMO;
  }

  private void OnGetSoul(int DeltaValue)
  {
    if (DeltaValue <= 0 || (double) DataManager.Instance.PLAYER_SPECIAL_AMMO <= 0.0)
      return;
    this.PLAYER_SPECIAL_CHARGE += (float) DeltaValue;
  }
}
