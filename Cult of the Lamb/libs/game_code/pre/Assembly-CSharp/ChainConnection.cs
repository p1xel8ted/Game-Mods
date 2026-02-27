// Decompiled with JetBrains decompiler
// Type: ChainConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
