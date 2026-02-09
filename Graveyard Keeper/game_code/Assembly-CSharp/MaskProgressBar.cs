// Decompiled with JetBrains decompiler
// Type: MaskProgressBar
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MaskProgressBar : MonoBehaviour
{
  public SpriteMask mask;
  public IngameProgressBar.ScaleType scale_type;
  public float min_scale;
  public float max_scale;
  public string game_res;
  public WorldGameObject _wgo;

  public void UpdateBar()
  {
    if (!this.HasWGO())
      return;
    float num = this.min_scale + (this.max_scale - this.min_scale) * this._wgo.GetParam(this.game_res);
    Vector3 localScale = this.mask.transform.localScale;
    switch (this.scale_type)
    {
      case IngameProgressBar.ScaleType.Vertical:
        this.mask.transform.localScale = new Vector3(localScale.x, num, localScale.z);
        break;
      case IngameProgressBar.ScaleType.Horizontal:
        this.mask.transform.localScale = new Vector3(num, localScale.y, localScale.z);
        break;
    }
  }

  public bool HasWGO()
  {
    if ((Object) this._wgo == (Object) null)
    {
      this.InitWGO();
      if ((Object) this._wgo == (Object) null)
        return false;
    }
    return true;
  }

  public void InitWGO()
  {
    WorldObjectPart component = this.GetComponent<WorldObjectPart>();
    if ((Object) component == (Object) null)
      return;
    this._wgo = component.parent;
  }
}
