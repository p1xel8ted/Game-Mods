// Decompiled with JetBrains decompiler
// Type: MarketplaceMusicManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using UnityEngine;

#nullable disable
public class MarketplaceMusicManager : MonoBehaviour
{
  [SerializeField]
  public MarketplaceMusicManager.Shop_ID shop_ID;

  public void Awake()
  {
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public void OnDestroy()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!this.gameObject.activeSelf)
      return;
    AudioManager.Instance.SetMusicRoomID((int) this.shop_ID, "shop_id");
  }

  public enum Shop_ID
  {
    rakshasa,
    forneus,
    helob,
    bebo,
  }
}
