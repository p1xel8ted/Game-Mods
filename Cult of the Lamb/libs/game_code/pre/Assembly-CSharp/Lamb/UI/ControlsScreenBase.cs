// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ControlsScreenBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI;

public abstract class ControlsScreenBase : UISubmenuBase
{
  public abstract bool ValidInputType(InputType inputType);

  public abstract void Configure(InputType inputType);

  public abstract void Configure(SettingsData.ControlSettings controlSettings);

  public abstract bool ShowBindingPrompts();
}
