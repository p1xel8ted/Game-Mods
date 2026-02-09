// Decompiled with JetBrains decompiler
// Type: WorldGen_Atmosphere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WorldGen_Atmosphere : BaseMonoBehaviour
{
  public void Start()
  {
    int num = 0;
    while (++num < 10)
      Object.Instantiate<GameObject>(Resources.Load("Prefabs/Particles/Mist") as GameObject, this.transform.parent, true).transform.position = new Vector3((float) Random.Range(-20, 20), (float) Random.Range(-20, 20), 0.0f);
  }

  public void Update()
  {
  }
}
