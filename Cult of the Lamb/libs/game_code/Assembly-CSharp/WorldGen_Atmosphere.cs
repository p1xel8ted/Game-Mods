// Decompiled with JetBrains decompiler
// Type: WorldGen_Atmosphere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
