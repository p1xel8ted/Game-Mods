// Decompiled with JetBrains decompiler
// Type: InitUnifyGlobal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.IO;
using UnityEngine;

#nullable disable
public class InitUnifyGlobal : MonoBehaviour
{
  private static GameObject instance;
  public static MemoryStream _outputMemoryStream;
  public static MemoryStream _serializedMemoryStream;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void Startup()
  {
    if (!((Object) InitUnifyGlobal.instance == (Object) null))
      return;
    Debug.Log((object) "Unify: Startup");
    if (SteamManager.Initialized)
      Debug.Log((object) ("Steam user -> " + SteamFriends.GetPersonaName()));
    else
      Debug.LogError((object) "Steam client might be not running!");
    InitUnifyGlobal._outputMemoryStream = (MemoryStream) new NotClosingMemoryStream(new MemoryStream(1048576 /*0x100000*/));
    InitUnifyGlobal._serializedMemoryStream = (MemoryStream) new NotClosingMemoryStream(new MemoryStream(16777216 /*0x01000000*/));
    Object original = Resources.Load("Prefabs/Unify Global");
    if (original != (Object) null)
    {
      InitUnifyGlobal.instance = Object.Instantiate(original) as GameObject;
      if ((Object) InitUnifyGlobal.instance != (Object) null)
        Object.DontDestroyOnLoad((Object) InitUnifyGlobal.instance);
      else
        Debug.LogError((object) "Unify: Could not instantiate Unify prefab.");
    }
    else
      Debug.LogError((object) "Unify: Unable to find game specific Unify prefab.");
  }
}
