// Decompiled with JetBrains decompiler
// Type: FollowerDirectionChanger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerDirectionChanger : BaseMonoBehaviour
{
  public StateMachine s;
  public GameObject Up;
  public GameObject Diagonal;
  public GameObject Side;
  public Vector2 LeftSideRange = new Vector2(80f, 100f);
  public Vector2 RightSideRange = new Vector2(80f, 100f);
  public Vector2 UpRange = new Vector2(80f, 100f);
  public Vector2 DownRange = new Vector2(80f, 100f);

  public void Start()
  {
  }

  public void chooseDirection()
  {
    this.Up.SetActive(false);
    this.Diagonal.SetActive(false);
    this.Side.SetActive(false);
    Debug.Log((object) "ChoosingDirection");
    if ((double) this.s.facingAngle >= (double) this.LeftSideRange.x && (double) this.s.facingAngle <= (double) this.LeftSideRange.y)
      this.Side.SetActive(true);
    else if ((double) this.s.facingAngle >= (double) this.RightSideRange.x && (double) this.s.facingAngle <= (double) this.RightSideRange.y)
      this.Side.SetActive(true);
    else if ((double) this.s.facingAngle >= (double) this.UpRange.x && (double) this.s.facingAngle <= (double) this.UpRange.y)
      this.Up.SetActive(true);
    else if ((double) this.s.facingAngle >= (double) this.DownRange.x && (double) this.s.facingAngle <= (double) this.DownRange.y)
      this.Up.SetActive(true);
    else
      this.Diagonal.SetActive(true);
  }
}
