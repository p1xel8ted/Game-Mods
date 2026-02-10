// Decompiled with JetBrains decompiler
// Type: DOGames.Scripts.Bootstrap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
