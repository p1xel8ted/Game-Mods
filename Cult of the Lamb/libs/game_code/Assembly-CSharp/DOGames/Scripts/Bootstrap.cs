// Decompiled with JetBrains decompiler
// Type: DOGames.Scripts.Bootstrap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace DOGames.Scripts;

public class Bootstrap
{
  public static bool initialised;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void Startup()
  {
    if (Bootstrap.initialised)
      return;
    Debug.Log((object) "DOGames:Bootstrap.Start()");
    Object original = Resources.Load("Prefabs/Debug Canvas");
    if (original == (Object) null)
      Debug.LogError((object) "DOGames:Bootstrap.Start, unabled to load prefab.");
    Object.Instantiate(original);
    Bootstrap.initialised = true;
  }
}
