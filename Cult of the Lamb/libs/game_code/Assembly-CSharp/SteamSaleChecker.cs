// Decompiled with JetBrains decompiler
// Type: SteamSaleChecker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class SteamSaleChecker : MonoBehaviour
{
  public const string SteamStoreApi = "https://store.steampowered.com/api/appdetails?appids={0}";
  public UnityEvent<PriceOverview> OnSaleUpdated;

  public IEnumerator CheckIfOnSale(int appId)
  {
    yield break;
  }
}
