// Decompiled with JetBrains decompiler
// Type: SpriteLine
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class SpriteLine : MonoBehaviour
{
  public Vector3 pos1;
  public Vector3 pos2;
  public UI2DSprite spr;

  public void Start() => this.spr = this.GetComponent<UI2DSprite>();

  public void Draw(Vector3 from, Vector3 to)
  {
    this.pos1 = from;
    this.pos2 = to;
    this.Redraw();
  }

  public int width
  {
    set => this.spr.width = value;
  }

  public void SetDepth(int d) => this.spr.depth = d;

  public void Redraw()
  {
    this.transform.rotation = Quaternion.AngleAxis((float) ((double) Mathf.Atan2(this.pos1.y - this.pos2.y, this.pos1.x - this.pos2.x) / 3.1415927410125732 * 180.0 + 90.0), Vector3.forward);
    if ((Object) this.spr == (Object) null)
      this.Start();
    this.spr.height = (int) (this.pos2 - this.pos1).magnitude;
    this.transform.localPosition = this.pos1 + (this.pos2 - this.pos1) / 2f;
  }

  public void SetColor(Color c) => this.spr.color = c;
}
