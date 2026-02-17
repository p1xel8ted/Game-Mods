// Decompiled with JetBrains decompiler
// Type: SteamSaleChecker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
