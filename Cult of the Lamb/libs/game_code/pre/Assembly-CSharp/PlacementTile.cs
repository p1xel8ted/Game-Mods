// Decompiled with JetBrains decompiler
// Type: PlacementTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityFx.Outline;

#nullable disable
public class PlacementTile : BaseMonoBehaviour
{
  protected OutlineEffect Outliner;
  public Vector3 Position;
  public SpriteRenderer spriteRenderer;
  public Vector2Int GridPosition;
  [SerializeField]
  private SpriteRenderer _blockedOutline;
  [SerializeField]
  private Sprite normalOutline;
  [SerializeField]
  private Sprite editOutline;
  private bool hadOutline;
  private int changed;

  private void OnEnable()
  {
  }

  public void SetColor(Color color, Vector3 buildingPos)
  {
    this.spriteRenderer.color = color;
    if (color == Color.red)
    {
      if (!CheatConsole.HidingUI)
        this._blockedOutline.enabled = true;
      this.spriteRenderer.color = (Color) new Vector4(1f, 1f, 1f, 1f);
    }
    else if (color == Color.white)
    {
      this._blockedOutline.enabled = false;
      this.spriteRenderer.color = (Color) new Vector4(1f, 1f, 1f, 0.5f);
      this.spriteRenderer.sprite = this.normalOutline;
    }
    else if (color == StaticColors.OrangeColor)
    {
      this._blockedOutline.enabled = false;
      this.spriteRenderer.color = StaticColors.OrangeColor;
      this.spriteRenderer.sprite = this.editOutline;
    }
    else
    {
      this.spriteRenderer.color = (Color) new Vector4(0.0f, 1f, 0.0f, 1f);
      this.spriteRenderer.sprite = this.editOutline;
    }
  }
}
