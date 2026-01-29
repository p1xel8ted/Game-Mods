// Decompiled with JetBrains decompiler
// Type: BlunderAmmo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using MMBiomeGeneration;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BlunderAmmo : PlayerAmmo
{
  public Swipe swipe;
  [SerializeField]
  public Swipe swipeLight;
  [SerializeField]
  public Swipe swipeDark;
  public Swipe swipeHeavy;
  public bool isInitialized;
  public int blunderAmmoTotal = 3;
  public Image radialChargeImage;
  public float blunderAmmo = 3f;
  public float blunderRechargeTimerReload = 0.6f;
  public float blunderRecharge;
  public Image currentReloadingImage;
  public Color goldenBulletColor;
  public Color standardBulletColor;
  public float blunderJam;

  public void Init(PlayerFarming playerFarmingRef)
  {
    this.playerFarming = playerFarmingRef;
    PlayerWeapon.WeaponCombos combo = this.playerFarming.CurrentWeaponInfo.WeaponData.Combos[0];
    if (this.gameObject.activeSelf)
      return;
    this.gameObject.SetActive(true);
    if (combo != null)
      this.blunderAmmoTotal = combo.BlunderbussBulletCount;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Blunderbuss))
      this.blunderAmmoTotal = 4;
    if (this.playerFarming.canUseKeyboard)
      MonoSingleton<UIManager>.Instance.SetCurrentCursor(1);
    this.radialChargeImage.gameObject.SetActive(true);
    this.radialChargeImage.fillAmount = 0.0f;
    this.blunderAmmo = (float) this.blunderAmmoTotal;
    this.blunderRecharge = 0.0f;
    for (int index = 0; index < this.Images.Count; ++index)
      this.Images[index].gameObject.SetActive(false);
    this.AmmoChanged();
    Debug.Log((object) ("Current dungeon location is " + BiomeGenerator.Instance.DungeonLocation.ToString()));
    if (BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_5)
    {
      Debug.Log((object) "Set blunderbuss swipe to dark variant");
      this.swipe = this.swipeDark;
    }
    else
      this.swipe = this.swipeLight;
    this.isInitialized = true;
  }

  public void JamBlunderbuss()
  {
    AudioManager.Instance.PlayOneShot("event:/weapon/blunderbuss_heavy_load_full", this.gameObject);
    this.blunderRecharge = 0.0f;
    this.blunderJam = 0.5f;
    this.CantAfford();
    this.radialChargeImage.DOKill();
    this.radialChargeImage.DOFillAmount(0.0f, 0.25f);
    this.Images[0].transform.DOKill();
    this.Images[0].transform.localScale = Vector3.one * 1.5f;
    this.Images[0].transform.DOScale(Vector3.one, 1f);
  }

  public new void OnEnable()
  {
  }

  public void FixedUpdate()
  {
    if (!(bool) (Object) EquipmentManager.GetWeaponData(this.playerFarming.currentWeapon) || EquipmentManager.GetWeaponData(this.playerFarming.currentWeapon).PrimaryEquipmentType != EquipmentType.Blunderbuss)
    {
      this.radialChargeImage.gameObject.SetActive(false);
      MonoSingleton<UIManager>.Instance.SetCurrentCursor(0);
      this.gameObject.SetActive(false);
    }
    else
    {
      if (TimeManager.PauseGameTime)
        MonoSingleton<UIManager>.Instance.SetCurrentCursor(0);
      if ((double) this.blunderAmmo >= (double) this.blunderAmmoTotal)
      {
        this.blunderRecharge = 0.0f;
        this.radialChargeImage.gameObject.SetActive(false);
        this.radialChargeImage.fillAmount = 0.0f;
      }
      else if ((double) this.blunderJam > 0.0)
      {
        this.blunderJam -= Time.deltaTime;
      }
      else
      {
        this.blunderRecharge += Time.deltaTime * (1f + TrinketManager.GetAttackRateMultiplier(this.playerFarming));
        if ((double) this.blunderRecharge > (double) this.blunderRechargeTimerReload)
        {
          this.blunderRecharge -= this.blunderRechargeTimerReload;
          this.AddAmmo();
        }
        this.radialChargeImage.gameObject.SetActive(true);
        this.radialChargeImage.fillAmount = 1f / this.blunderRechargeTimerReload * this.blunderRecharge;
        this.radialChargeImage.transform.position = this.currentReloadingImage.transform.position;
      }
    }
  }

  public bool UseAmmo()
  {
    if ((double) this.blunderAmmo <= 0.0)
    {
      this.AmmoChanged();
      this.blunderAmmo = 0.0f;
      return false;
    }
    this.blunderRecharge = 0.0f;
    this.SetBlunderAmmo(this.blunderAmmo - 1f);
    return true;
  }

  public bool AddAmmo()
  {
    if ((double) this.blunderAmmo >= (double) this.blunderAmmoTotal)
    {
      this.blunderAmmo = (float) this.blunderAmmoTotal;
      return false;
    }
    this.SetBlunderAmmo(this.blunderAmmo + 1f);
    AudioManager.Instance.PlayOneShot("event:/weapon/blunderbuss_reload", this.gameObject);
    return true;
  }

  public void EmptyAllAmmo()
  {
    this.blunderAmmo = 0.0f;
    this.AmmoChanged();
  }

  public void SetBlunderAmmo(float value)
  {
    this.blunderAmmo = value;
    if ((double) this.blunderAmmo > (double) this.blunderAmmoTotal)
      this.blunderAmmo = (float) this.blunderAmmoTotal;
    else if ((double) this.blunderAmmo < 0.0)
      this.blunderAmmo = 0.0f;
    this.AmmoChanged();
  }

  public override void CantAfford()
  {
    this.transform.DOKill();
    this.transform.DOShakePosition(0.5f, new Vector3(0.25f, 0.0f), randomness: 0.0f);
    this.AmmoChanged();
  }

  public new virtual void AmmoChanged()
  {
    this.StopAllCoroutines();
    this.CanvasGroup.DOKill();
    this.CanvasGroup.DOFade(1f, 0.1f);
    int index = -1;
    bool flag = false;
    while (++index < this.Images.Count)
    {
      this.Images[index].gameObject.SetActive(index < this.blunderAmmoTotal);
      if ((double) index < (double) this.blunderAmmo)
      {
        if ((Object) this.Images[index].sprite == (Object) this.EmptySprite)
        {
          this.Images[index].transform.DOKill();
          this.Images[index].transform.localScale = Vector3.one * 1.5f;
          this.Images[index].transform.DOScale(Vector3.one, 1f);
        }
        this.Images[index].sprite = this.AmmoSprite;
        if (index == this.blunderAmmoTotal - 1)
          this.Images[index].color = this.goldenBulletColor;
        else
          this.Images[index].color = this.standardBulletColor;
        this.radialChargeImage.transform.position = this.Images[index].transform.position;
      }
      else
      {
        if ((Object) this.Images[index].sprite == (Object) this.AmmoSprite)
        {
          this.Images[index].transform.DOKill();
          this.Images[index].transform.localScale = Vector3.one * 1.5f;
          this.Images[index].transform.DOScale(Vector3.one, 1f);
        }
        this.Images[index].sprite = this.EmptySprite;
        this.Images[index].color = Color.white;
        if (!flag)
        {
          this.currentReloadingImage = this.Images[index];
          this.radialChargeImage.transform.position = this.Images[index].transform.position;
          this.radialChargeImage.transform.SetParent(this.Images[index].transform);
          this.radialChargeImage.transform.localScale = Vector3.one;
          flag = true;
        }
      }
    }
    this.radialChargeImage.gameObject.SetActive(flag);
    this.StartCoroutine((IEnumerator) this.FadeOut());
  }
}
