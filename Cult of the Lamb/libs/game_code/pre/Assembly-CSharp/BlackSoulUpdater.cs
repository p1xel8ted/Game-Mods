// Decompiled with JetBrains decompiler
// Type: BlackSoulUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BlackSoulUpdater : MonoBehaviour
{
  private static BlackSoulUpdater instance;
  private static GameObject blacksoulupdateGO;

  public static BlackSoulUpdater Instance
  {
    get
    {
      if ((Object) BlackSoulUpdater.instance == (Object) null)
      {
        BlackSoulUpdater.blacksoulupdateGO = new GameObject("blacksoulupdate");
        BlackSoulUpdater.instance = BlackSoulUpdater.blacksoulupdateGO.AddComponent<BlackSoulUpdater>();
      }
      return BlackSoulUpdater.instance;
    }
  }
}
