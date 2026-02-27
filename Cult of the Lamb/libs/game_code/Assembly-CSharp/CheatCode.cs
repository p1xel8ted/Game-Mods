// Decompiled with JetBrains decompiler
// Type: CheatCode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
