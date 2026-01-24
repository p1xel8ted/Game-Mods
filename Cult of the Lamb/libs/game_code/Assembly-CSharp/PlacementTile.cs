// Decompiled with JetBrains decompiler
// Type: PlacementTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityFx.Outline;

#nullable disable
public class PlacementTile : BaseMonoBehaviour
{
  public OutlineEffect Outliner;
  public Vector3 Position;
  public SpriteRenderer spriteRenderer;
  public Vector2Int GridPosition;
  [SerializeField]
  public SpriteRenderer _blockedOutline;
  [SerializeField]
  public SpriteRenderer _goodOutline;
  [SerializeField]
  public Sprite normalOutline;
  [SerializeField]
  public Sprite editOutline;
  public bool hadOutline;
  public int changed;

  public void OnEnable()
  {
  }

  public void SetColor(Color color, Vector3 buildingPos)
  {
    this.spriteRenderer.color = color;
    if (color == Color.red)
    {
      if (!CheatConsole.HidingUI)
        this._blockedOutline.enabled = true;
      this._goodOutline.enabled = false;
      this.spriteRenderer.color = (Color) new Vector4(1f, 1f, 1f, 1f);
    }
    else if (color == Color.white)
    {
      this._goodOutline.enabled = false;
      this._blockedOutline.enabled = false;
      this.spriteRenderer.color = (Color) new Vector4(1f, 1f, 1f, 0.5f);
      if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
        this.spriteRenderer.color = (Color) new Vector4(0.25f, 0.25f, 0.25f, 0.5f);
      this.spriteRenderer.sprite = this.normalOutline;
    }
    else if (color == StaticColors.OrangeColor)
    {
      this._goodOutline.enabled = false;
      this._blockedOutline.enabled = false;
      this.spriteRenderer.color = StaticColors.OrangeColor;
      this.spriteRenderer.sprite = this.editOutline;
    }
    else
    {
      this._goodOutline.enabled = true;
      this.spriteRenderer.color = (Color) new Vector4(0.0f, 1f, 0.0f, 1f);
      this.spriteRenderer.sprite = this.editOutline;
    }
  }
}
