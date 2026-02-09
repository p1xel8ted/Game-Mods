// Decompiled with JetBrains decompiler
// Type: BubbleWidgetDataOptions
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class BubbleWidgetDataOptions : BubbleWidgetData
{
  public List<BubbleWidgetDataOptions.OptionData> options = new List<BubbleWidgetDataOptions.OptionData>();
  public System.Action on_hide;

  public BubbleWidgetDataOptions()
  {
  }

  public BubbleWidgetDataOptions(string name, System.Action callback, bool enabled = true)
  {
    this.AddOption(name, callback, enabled);
  }

  public BubbleWidgetDataOptions(string[] names, System.Action[] callbacks)
  {
    for (int index = 0; index < names.Length; ++index)
      this.AddOption(names[index], callbacks[index]);
  }

  public void AddOption(string name, System.Action callback, bool enabled = true)
  {
    this.options.Add(new BubbleWidgetDataOptions.OptionData(name, callback, enabled));
  }

  public struct OptionData(string name, System.Action callback, bool enabled = true)
  {
    public string name = name;
    public System.Action callback = callback;
    public bool enabled = enabled;
  }
}
