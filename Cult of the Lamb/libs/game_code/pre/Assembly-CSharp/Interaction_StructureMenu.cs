// Decompiled with JetBrains decompiler
// Type: Interaction_StructureMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_StructureMenu : Interaction
{
  private void Start()
  {
    Vector3 position = this.transform.position;
    Random.InitState((int) position.x + (int) position.y);
    Vector3 vector3 = new Vector3(0.0f, 0.0f, Random.Range(-0.015f, 0.015f));
    this.transform.position = position + vector3;
  }
}
