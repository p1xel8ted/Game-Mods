// Decompiled with JetBrains decompiler
// Type: RandomEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
