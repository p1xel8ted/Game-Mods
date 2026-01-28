// Decompiled with JetBrains decompiler
// Type: SimpleMoveObjectDown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SimpleMoveObjectDown : MonoBehaviour
{
  [SerializeField]
  public Vector3 offset = new Vector3(0.0f, 0.0f, 2f);

  public void Awake()
  {
    if (PlayerFarming.Location != FollowerLocation.Dungeon1_6)
      return;
    this.transform.position += this.offset;
  }
}
