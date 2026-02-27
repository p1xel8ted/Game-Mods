// Decompiled with JetBrains decompiler
// Type: src.UI.Testing.AdventureMapTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
