// Decompiled with JetBrains decompiler
// Type: CommandPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class CommandPoint : BaseMonoBehaviour
{
  public Health.Team team;
  private SpriteRenderer spriterenderer;
  public Sprite ImageNeutral;
  public Sprite ImageTeam1;
  public Sprite ImageTeam2;

  private void Start()
  {
    this.spriterenderer = this.GetComponent<SpriteRenderer>();
    this.setImage();
  }

  private void setImage()
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
