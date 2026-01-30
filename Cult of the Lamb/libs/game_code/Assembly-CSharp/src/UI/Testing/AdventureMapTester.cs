// Decompiled with JetBrains decompiler
// Type: src.UI.Testing.AdventureMapTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Map;
using System.Collections;
using UnityEngine;

#nullable disable
namespace src.UI.Testing;

public class AdventureMapTester : MonoBehaviour
{
  public IEnumerator Start()
  {
    yield return (object) null;
    MapManager.Instance.ShowMap();
  }
}
