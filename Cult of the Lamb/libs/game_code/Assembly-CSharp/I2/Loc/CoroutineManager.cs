// Decompiled with JetBrains decompiler
// Type: I2.Loc.CoroutineManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
