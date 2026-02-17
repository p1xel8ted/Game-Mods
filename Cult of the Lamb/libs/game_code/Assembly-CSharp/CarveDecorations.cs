// Decompiled with JetBrains decompiler
// Type: CarveDecorations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class CarveDecorations : MonoBehaviour
{
  public GenerateRoom generateRoom;
  public Collider2D col;

  public void Awake()
  {
    this.col = this.GetComponent<Collider2D>();
    this.generateRoom = this.GetComponentInParent<GenerateRoom>();
    this.generateRoom.OnGenerated += new GenerateRoom.GenerateEvent(this.OnGenerated);
    this.generateRoom.OnRegenerated += new GenerateRoom.GenerateEvent(this.OnGenerated);
  }

  public void OnDestroy()
  {
    if (!((Object) this.generateRoom != (Object) null))
      return;
    this.generateRoom.OnGenerated -= new GenerateRoom.GenerateEvent(this.OnGenerated);
    this.generateRoom.OnRegenerated -= new GenerateRoom.GenerateEvent(this.OnGenerated);
  }

  public void OnGenerated()
  {
    for (int index = this.generateRoom.SceneryTransform.transform.childCount - 1; index >= 0; --index)
    {
      if (this.col.ClosestPoint((Vector2) this.generateRoom.SceneryTransform.transform.GetChild(index).position) == (Vector2) this.generateRoom.SceneryTransform.transform.GetChild(index).position)
        this.generateRoom.SceneryTransform.transform.GetChild(index).gameObject.SetActive(false);
    }
  }
}
