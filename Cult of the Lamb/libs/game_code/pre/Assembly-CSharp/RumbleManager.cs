// Decompiled with JetBrains decompiler
// Type: RumbleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RumbleManager : BaseMonoBehaviour
{
  private static RumbleManager _instance;

  public static RumbleManager Instance
  {
    get
    {
      if ((Object) RumbleManager._instance == (Object) null)
        RumbleManager._instance = (Object.Instantiate(Resources.Load("MMVibrate/RumbleManager")) as GameObject).GetComponent<RumbleManager>();
      return RumbleManager._instance;
    }
  }

  private void Awake()
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
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) this);
  }
}
