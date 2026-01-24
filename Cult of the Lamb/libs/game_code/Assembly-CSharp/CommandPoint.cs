// Decompiled with JetBrains decompiler
// Type: CommandPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class CommandPoint : BaseMonoBehaviour
{
  public Health.Team team;
  public SpriteRenderer spriterenderer;
  public Sprite ImageNeutral;
  public Sprite ImageTeam1;
  public Sprite ImageTeam2;

  public void Start()
  {
    this.spriterenderer = this.GetComponent<SpriteRenderer>();
    this.setImage();
  }

  public void setImage()
  {
    switch (this.team)
    {
      case Health.Team.Neutral:
        this.spriterenderer.sprite = this.ImageNeutral;
        break;
      case Health.Team.PlayerTeam:
        this.spriterenderer.sprite = this.ImageTeam1;
        break;
      case Health.Team.Team2:
        this.spriterenderer.sprite = this.ImageTeam2;
        break;
    }
  }
}
