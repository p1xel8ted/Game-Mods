// Decompiled with JetBrains decompiler
// Type: CutsceneManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public static class CutsceneManager
{
  public const string EXTRA_PATH = "DLC_stories/";

  public static void ExecuteCutscene(
    string flowscript_name,
    CustomFlowScript.OnFinishedDelegate onFinishedDelegate = null)
  {
    GS.RunFlowScript("DLC_stories/" + flowscript_name, onFinishedDelegate);
  }
}
