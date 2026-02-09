// Decompiled with JetBrains decompiler
// Type: SceneDescription
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SceneDescription : MonoBehaviour
{
  public SceneDescription.SceneType scene_type;
  public MainGame main_camera;
  public UIRoot ui_root;
  public SmartAudioEngine smart_audio_engine;

  public enum SceneType
  {
    WorldMap,
    MainScene,
  }
}
