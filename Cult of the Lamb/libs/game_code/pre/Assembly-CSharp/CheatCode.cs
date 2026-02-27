// Decompiled with JetBrains decompiler
// Type: CheatCode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private int index;
  private float downTime;
  private float upTime;
  private float pressTime;
  private float countDown = 10f;
  private GameObject CheatPanel;
  public static bool UpdateSkeletonAnimation = true;

  private Player player
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

  private void Update()
  {
    if (this.cheatCode.Length == 0 || this.CheatUIToDisplay == "" || this.player == null || !((Object) this.CheatPanel == (Object) null) || this.player == null)
      return;
    if (this.player.GetAnyButtonDown())
    {
      if (this.player.GetButtonDown(this.cheatCode[this.index]))
      {
        ++this.index;
        this.downTime = Time.time;
        this.pressTime = this.downTime + this.countDown;
      }
      else
        this.index = 0;
    }
    if (this.index == this.cheatCode.Length)
    {
      if (!this.FindCheatUI)
      {
        GameObject original = Resources.Load(this.CheatUIToDisplay) as GameObject;
        if ((Object) original == (Object) null)
          Debug.LogError((object) "CheatPanel: unable to load prefab resource.");
        this.CheatPanel = Object.Instantiate<GameObject>(original, this.transform.Find("CanvasUnify"));
        this.index = 0;
      }
      else
      {
        GameObject gameObject = GameObject.Find(this.CheatUIToDisplay);
        if ((Object) gameObject != (Object) null)
        {
          gameObject.SetActive(true);
          if (this.SendMessage != "")
            gameObject.SendMessage(this.SendMessage);
        }
        this.index = 0;
      }
    }
    else
    {
      if (this.index <= 0 || (double) Time.time < (double) this.pressTime)
        return;
      this.index = 0;
    }
  }

  public void close() => Object.Destroy((Object) this.CheatPanel);
}
