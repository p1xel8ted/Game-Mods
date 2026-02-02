// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HighContrastTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
