// Decompiled with JetBrains decompiler
// Type: FriendlyEnemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FriendlyEnemy : MonoBehaviour
{
  public static List<FriendlyEnemy> FriendlyEnemies = new List<FriendlyEnemy>();
  [SerializeField]
  public EnemySwordsman swordsman;
  [SerializeField]
  public GameObject container;
  public ShowHPBar showHpBar;

  public void OnEnable()
  {
    if ((Object) this.showHpBar == (Object) null)
      this.showHpBar = this.GetComponent<ShowHPBar>();
    FriendlyEnemy.FriendlyEnemies.Add(this);
  }

  public void OnDisable() => FriendlyEnemy.FriendlyEnemies.Remove(this);

  public void Awake()
  {
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public void Update()
  {
    if ((Object) RespawnRoomManager.Instance != (Object) null && RespawnRoomManager.Instance.gameObject.activeSelf || (Object) DeathCatRoomManager.Instance != (Object) null && DeathCatRoomManager.Instance.gameObject.activeSelf || (Object) MysticShopKeeperManager.Instance != (Object) null && MysticShopKeeperManager.Instance.gameObject.activeSelf)
    {
      this.container.gameObject.SetActive(false);
      this.swordsman.enabled = false;
    }
    else
    {
      this.container.gameObject.SetActive(true);
      this.swordsman.enabled = true;
    }
  }

  public void OnDestroy()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((Object) this.showHpBar != (Object) null) || !((Object) this.showHpBar.hpBar == (Object) null))
      return;
    this.showHpBar.InstantiateHpBar();
  }
}
