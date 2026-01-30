// Decompiled with JetBrains decompiler
// Type: HideIfDungeon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
