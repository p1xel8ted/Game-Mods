// Decompiled with JetBrains decompiler
// Type: WorldGen_Atmosphere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WorldGen_Atmosphere : BaseMonoBehaviour
{
  private void Start()
  {
    int num = 0;
    while (++num < 10)
      Object.Instantiate<GameObject>(Resources.Load("Prefabs/Particles/Mist") as GameObject, this.transform.parent, true).transform.position = new Vector3((float) Random.Range(-20, 20), (float) Random.Range(-20, 20), 0.0f);
  }

  private void Update()
  {
  }
}
