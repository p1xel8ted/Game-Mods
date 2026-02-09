// Decompiled with JetBrains decompiler
// Type: RumbleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RumbleManager : BaseMonoBehaviour
{
  public static RumbleManager _instance;

  public static RumbleManager Instance
  {
    get
    {
      if ((Object) RumbleManager._instance == (Object) null)
        RumbleManager._instance = (Object.Instantiate(Resources.Load("MMVibrate/RumbleManager")) as GameObject).GetComponent<RumbleManager>();
      return RumbleManager._instance;
    }
  }

  public void Awake()
  {
    if ((Object) RumbleManager._instance != (Object) null && (Object) RumbleManager._instance != (Object) this)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      RumbleManager._instance = this;
      if ((Object) this.transform.parent != (Object) null)
        this.transform.SetParent((Transform) null);
      Object.DontDestroyOnLoad((Object) this.gameObject);
    }
  }

  public void Rumble()
  {
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, coroutineSupport: (MonoBehaviour) this);
  }
}
