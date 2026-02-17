// Decompiled with JetBrains decompiler
// Type: HideIfDungeon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HideIfDungeon : MonoBehaviour
{
  [SerializeField]
  public FollowerLocation dungeon;

  public void Start()
  {
    if (PlayerFarming.Location != this.dungeon)
      return;
    this.gameObject.SetActive(false);
  }
}
