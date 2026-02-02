// Decompiled with JetBrains decompiler
// Type: Interaction_StructureMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_StructureMenu : Interaction
{
  public void Start()
  {
    Vector3 position = this.transform.position;
    Random.InitState((int) position.x + (int) position.y);
    Vector3 vector3 = new Vector3(0.0f, Random.Range(-0.0015f, 0.0015f), Random.Range(-0.015f, 0.015f));
    this.transform.position = position + vector3;
  }
}
