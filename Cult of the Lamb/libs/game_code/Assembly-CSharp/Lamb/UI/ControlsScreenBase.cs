// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ControlsScreenBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI;

public abstract class ControlsScreenBase : UISubmenuBase
{
  public abstract bool ValidInputType(InputType inputType);

  public abstract void Configure(InputType inputType);

  public abstract void Configure(SettingsData.ControlSettings controlSettings);

  public abstract bool ShowBindingPrompts();
}
