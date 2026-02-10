// Decompiled with JetBrains decompiler
// Type: InitUnifyGlobal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Steamworks;
using System.IO;
using Unify;
using UnityEngine;

#nullable disable
public class InitUnifyGlobal : MonoBehaviour
{
  public static GameObject instance;
  public static MemoryStream _outputMemoryStream;
  public static MemoryStream _serializedMemoryStream;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void Startup()
  {
    Engagement.GlobalAllowEngagement = false;
    if (!((Object) InitUnifyGlobal.instance == (Object) null))
      return;
    Debug.Log((object) "Unify: Startup");
    if (SteamManager.Initialized)
      Debug.Log((object) ("Steam user -> " + SteamFriends.GetPersonaName()));
    else
      Debug.LogError((object) "Steam client might be not running!");
    if (InitUnifyGlobal._outputMemoryStream == null)
      InitUnifyGlobal._outputMemoryStream = (MemoryStream) new NotClosingMemoryStream(new MemoryStream(1048576 /*0x100000*/));
    if (InitUnifyGlobal._serializedMemoryStream == null)
      InitUnifyGlobal._serializedMemoryStream = (MemoryStream) new NotClosingMemoryStream(new MemoryStream(16777216 /*0x01000000*/));
    Object original = Resources.Load("Prefabs/Unify Global");
    if (original != (Object) null)
    {
      InitUnifyGlobal.instance = Object.Instantiate(original) as GameObject;
      if ((Object) InitUnifyGlobal.instance != (Object) null)
      {
        Object.DontDestroyOnLoad((Object) InitUnifyGlobal.instance);
        InitUnifyGlobal.instance.AddComponent<SkeletonAnimationLODGlobalManager>().Initialize();
        InitUnifyGlobal.instance.GetComponent<SessionManager>().syncSaveFiles = new string[0];
      }
      else
        Debug.LogError((object) "Unify: Could not instantiate Unify prefab.");
    }
    else
      Debug.LogError((object) "Unify: Unable to find game specific Unify prefab.");
  }
}
