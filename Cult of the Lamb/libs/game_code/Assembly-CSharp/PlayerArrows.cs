// Decompiled with JetBrains decompiler
// Type: PlayerArrows
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlayerArrows : BaseMonoBehaviour
{
  public GrowAndFade growAndFade;
  public PlayerFarming playerFarming;
  public bool _Reloading;
  public float ReloadProgress;
  public float ReloadTarget = 2f;
  public bool GiveExtraSpirit;
  public Vector2 TargetRange = new Vector2(0.2f, 0.35f);
  public float ReloadSpeed = 1f;
  public int _totalSpiritAmmo;

  public static event PlayerArrows.AmmoUpdated OnAmmoUpdated;

  public static event PlayerArrows.AmmoUpdated OnNoAmmoShake;

  public static event PlayerArrows.AmmoUpdated OnBeginReloading;

  public void OnEnable()
  {
    this.playerFarming = this.GetComponent<PlayerFarming>();
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    TrinketManager.OnTrinketRemoved += new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
  }

  public void OnDisable()
  {
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    TrinketManager.OnTrinketRemoved -= new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
  }

  public void OnTrinketsChanged(TarotCards.Card trinket, PlayerFarming playerFarming = null)
  {
    this.PLAYER_SPIRIT_TOTAL_AMMO = DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO;
  }

  public int PLAYER_ARROW_AMMO
  {
    get => DataManager.Instance.PLAYER_ARROW_AMMO;
    set
    {
      DataManager.Instance.PLAYER_ARROW_AMMO = value;
      this.ReloadSpeed = UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Combat_Arrows2) ? 2f : 1f;
      if (DataManager.Instance.PLAYER_ARROW_AMMO > DataManager.Instance.PLAYER_ARROW_TOTAL_AMMO)
        DataManager.Instance.PLAYER_ARROW_AMMO = DataManager.Instance.PLAYER_ARROW_TOTAL_AMMO;
      if (DataManager.Instance.PLAYER_ARROW_AMMO <= 0)
      {
        DataManager.Instance.PLAYER_ARROW_AMMO = 0;
        PlayerArrows.AmmoUpdated onNoAmmoShake = PlayerArrows.OnNoAmmoShake;
        if (onNoAmmoShake != null)
          onNoAmmoShake(this);
        if (!this.GiveExtraSpirit)
          return;
        this.GiveExtraSpirit = false;
        --this.PLAYER_SPIRIT_TOTAL_AMMO;
        PlayerArrows.AmmoUpdated onAmmoUpdated = PlayerArrows.OnAmmoUpdated;
        if (onAmmoUpdated == null)
          return;
        onAmmoUpdated(this);
      }
      else
      {
        this.Reloading = false;
        if (DataManager.Instance.PLAYER_ARROW_AMMO < this.PLAYER_ARROW_TOTAL_AMMO)
          this.Reloading = true;
        PlayerArrows.AmmoUpdated onAmmoUpdated = PlayerArrows.OnAmmoUpdated;
        if (onAmmoUpdated == null)
          return;
        onAmmoUpdated(this);
      }
    }
  }

  public int PLAYER_ARROW_TOTAL_AMMO
  {
    get => DataManager.Instance.PLAYER_ARROW_TOTAL_AMMO;
    set
    {
      DataManager.Instance.PLAYER_ARROW_TOTAL_AMMO = value;
      PlayerArrows.AmmoUpdated onAmmoUpdated = PlayerArrows.OnAmmoUpdated;
      if (onAmmoUpdated == null)
        return;
      onAmmoUpdated(this);
    }
  }

  public void RestockArrow()
  {
    if (this.PLAYER_ARROW_AMMO <= 0)
      this.growAndFade.Play();
    if (this.PLAYER_ARROW_AMMO < this.PLAYER_ARROW_TOTAL_AMMO)
      ++this.PLAYER_ARROW_AMMO;
    if (this.PLAYER_SPIRIT_AMMO < this.PLAYER_SPIRIT_TOTAL_AMMO)
      ++this.PLAYER_SPIRIT_AMMO;
    PlayerArrows.AmmoUpdated onAmmoUpdated = PlayerArrows.OnAmmoUpdated;
    if (onAmmoUpdated != null)
      onAmmoUpdated(this);
    if (this.PLAYER_ARROW_AMMO >= this.PLAYER_ARROW_TOTAL_AMMO && this.PLAYER_SPIRIT_AMMO >= this.PLAYER_SPIRIT_TOTAL_AMMO)
    {
      Debug.Log((object) "Ammo full");
      this.Reloading = false;
    }
    else
    {
      this.ReloadProgress = 0.0f;
      PlayerArrows.AmmoUpdated onBeginReloading = PlayerArrows.OnBeginReloading;
      if (onBeginReloading == null)
        return;
      onBeginReloading(this);
    }
  }

  public void RestockAllArrows()
  {
    this.Reloading = false;
    if (this.PLAYER_ARROW_AMMO <= 0)
      this.growAndFade.Play();
    this.PLAYER_ARROW_AMMO = this.PLAYER_ARROW_TOTAL_AMMO;
    this.PLAYER_SPIRIT_AMMO = this.PLAYER_SPIRIT_TOTAL_AMMO;
    PlayerArrows.AmmoUpdated onAmmoUpdated = PlayerArrows.OnAmmoUpdated;
    if (onAmmoUpdated == null)
      return;
    onAmmoUpdated(this);
  }

  public bool Reloading
  {
    set
    {
      if (this._Reloading != value & value)
      {
        this.ReloadProgress = 0.0f;
        this.GiveExtraSpirit = false;
        PlayerArrows.AmmoUpdated onBeginReloading = PlayerArrows.OnBeginReloading;
        if (onBeginReloading != null)
          onBeginReloading(this);
      }
      this._Reloading = value;
    }
    get => this._Reloading;
  }

  public void Update()
  {
    if (!this.Reloading || (double) (this.ReloadProgress += Time.deltaTime * this.ReloadSpeed) <= (double) this.ReloadTarget)
      return;
    this.RestockArrow();
  }

  public int PLAYER_SPIRIT_AMMO
  {
    get => DataManager.Instance.PLAYER_SPIRIT_AMMO;
    set
    {
      DataManager.Instance.PLAYER_SPIRIT_AMMO = value;
      if (DataManager.Instance.PLAYER_SPIRIT_AMMO <= 0)
        DataManager.Instance.PLAYER_SPIRIT_AMMO = 0;
      if (DataManager.Instance.PLAYER_SPIRIT_AMMO > DataManager.Instance.PLAYER_ARROW_TOTAL_AMMO)
        DataManager.Instance.PLAYER_SPIRIT_AMMO = DataManager.Instance.PLAYER_ARROW_TOTAL_AMMO;
      PlayerArrows.AmmoUpdated onAmmoUpdated = PlayerArrows.OnAmmoUpdated;
      if (onAmmoUpdated == null)
        return;
      onAmmoUpdated(this);
    }
  }

  public int PLAYER_SPIRIT_TOTAL_AMMO
  {
    get => this._totalSpiritAmmo;
    set
    {
      int totalSpiritAmmo = this._totalSpiritAmmo;
      DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = value;
      this._totalSpiritAmmo = value + TrinketManager.GetSpiritAmmo(this.playerFarming);
      if (this._totalSpiritAmmo == totalSpiritAmmo)
        return;
      if (this._totalSpiritAmmo > totalSpiritAmmo)
        this.PLAYER_SPIRIT_AMMO += this._totalSpiritAmmo - totalSpiritAmmo;
      else if (this.PLAYER_SPIRIT_AMMO > this._totalSpiritAmmo)
        this.PLAYER_SPIRIT_AMMO = this._totalSpiritAmmo;
      PlayerArrows.AmmoUpdated onAmmoUpdated = PlayerArrows.OnAmmoUpdated;
      if (onAmmoUpdated == null)
        return;
      onAmmoUpdated(this);
    }
  }

  public delegate void AmmoUpdated(PlayerArrows playerArrows);
}
