// Decompiled with JetBrains decompiler
// Type: I2.Loc.CoroutineManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class CoroutineManager : MonoBehaviour
{
  public static CoroutineManager mInstance;

  public static CoroutineManager pInstance
  {
    get
    {
      if ((Object) CoroutineManager.mInstance == (Object) null)
      {
        GameObject target = new GameObject("_Coroutiner");
        target.hideFlags = HideFlags.HideAndDontSave;
        CoroutineManager.mInstance = target.AddComponent<CoroutineManager>();
        if (Application.isPlaying)
          Object.DontDestroyOnLoad((Object) target);
      }
      return CoroutineManager.mInstance;
    }
  }

  public void Awake()
  {
    if (!Application.isPlaying)
      return;
    Object.DontDestroyOnLoad((Object) this.gameObject);
  }

  public static Coroutine Start(IEnumerator coroutine)
  {
    return CoroutineManager.pInstance.StartCoroutine((IEnumerator) coroutine);
  }
}
