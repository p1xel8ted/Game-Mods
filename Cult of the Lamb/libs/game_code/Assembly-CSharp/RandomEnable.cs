// Decompiled with JetBrains decompiler
// Type: RandomEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RandomEnable : BaseMonoBehaviour
{
  public void OnEnable()
  {
    if (Random.Range(0, 100) >= 50)
      return;
    this.gameObject.SetActive(false);
  }
}
