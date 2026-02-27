// Decompiled with JetBrains decompiler
// Type: DOGames.Scripts.Bootstrap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace DOGames.Scripts;

internal class Bootstrap
{
  private static bool initialised;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void Startup()
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
