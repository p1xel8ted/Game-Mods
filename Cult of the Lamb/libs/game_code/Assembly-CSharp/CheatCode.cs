// Decompiled with JetBrains decompiler
// Type: CheatCode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using Unify.Input;
using UnityEngine;

#nullable disable
public class CheatCode : MonoBehaviour
{
  public string[] cheatCode = new string[8]
  {
    "UI_Up",
    "UI_Up",
    "UI_Down",
    "UI_Down",
    "UI_Left",
    "UI_Right",
    "UI_Left",
    "UI_Right"
  };
  public bool FindCheatUI;
  public string CheatUIToDisplay = "Prefabs/PerformanceTestUI";
  public string SendMessage = "";
  public int index;
  public float downTime;
  public float upTime;
  public float pressTime;
  public float countDown = 10f;
  public GameObject CheatPanel;
  public static bool UpdateSkeletonAnimation = true;

  public Player player
  {
    get
    {
      try
      {
        return RewiredInputManager.MainPlayer;
      }
      catch
      {
        return (Player) null;
      }
    }
  }
}
