// Decompiled with JetBrains decompiler
// Type: SimpleMoveObjectDown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
