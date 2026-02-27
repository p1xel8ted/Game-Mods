// Decompiled with JetBrains decompiler
// Type: MaterialImagePropertyController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public abstract class MaterialImagePropertyController : MaterialPropertyController<Image>
{
  protected override void SetupSource()
  {
    if (!Application.isPlaying)
    {
      this._material = this._sourceComponent.materialForRendering;
    }
    else
    {
      this._material = new Material(this._sourceComponent.material);
      this._sourceComponent.material = this._material;
    }
    this.UpdateMaterialProperties();
  }
}
