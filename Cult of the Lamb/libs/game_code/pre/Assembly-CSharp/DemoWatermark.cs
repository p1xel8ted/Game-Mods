// Decompiled with JetBrains decompiler
// Type: DemoWatermark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class DemoWatermark : MonoBehaviour
{
  public static GameObject Instance;
  public TextMeshProUGUI Text;

  public static void Play()
  {
    if (!((Object) DemoWatermark.Instance == (Object) null))
      return;
    DemoWatermark.Instance = Object.Instantiate(UnityEngine.Resources.Load("DemoWatermark/DemoWatermark")) as GameObject;
  }

  private void Update()
  {
    if (!CheatConsole.IN_DEMO)
      Object.Destroy((Object) DemoWatermark.Instance);
    else if ((double) CheatConsole.DemoBeginTime == 0.0 || (double) CheatConsole.DemoBeginTime >= 1200.0)
      this.Text.text = "Work In Progress Gameplay";
    else
      this.Text.text = "Work In Progress Gameplay | " + this.GetTime();
  }

  private string GetTime()
  {
    float num1 = 1200f - CheatConsole.DemoBeginTime;
    int num2 = Mathf.FloorToInt(num1 / 60f);
    int num3 = 0;
    for (; num2 > 60; num2 -= 60)
      ++num3;
    int num4 = Mathf.FloorToInt(num1 % 60f);
    if (num3 > 0 && num3 < 10)
    {
      string str = "0" + (object) num3;
    }
    if (num3 >= 10)
      num3.ToString();
    return $"{(num2 < 10 ? (object) "0" : (object) "")}{(object) num2}:{(num4 < 10 ? (object) "0" : (object) "")}{(object) num4}";
  }
}
