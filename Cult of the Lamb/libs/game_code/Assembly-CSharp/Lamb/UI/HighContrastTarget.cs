// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HighContrastTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;

#nullable disable
namespace Lamb.UI;

public abstract class HighContrastTarget
{
  public HighContrastConfiguration _configuration;

  public HighContrastTarget(HighContrastConfiguration configuration)
  {
    this._configuration = configuration;
  }

  public abstract void Apply(bool state);

  public abstract void Init();
}
