// Decompiled with JetBrains decompiler
// Type: src.UI.Testing.AdventureMapTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
