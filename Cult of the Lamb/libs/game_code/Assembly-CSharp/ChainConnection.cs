// Decompiled with JetBrains decompiler
// Type: ChainConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ChainConnection : BaseMonoBehaviour
{
  public void UpdatePosition(Vector3 Position1, Vector3 Position2)
  {
    this.transform.position = Position1;
    this.transform.eulerAngles = new Vector3(-60f, 0.0f, Vector2.Angle((Vector2) Position1, (Vector2) Position2) * 57.29578f);
  }
}
