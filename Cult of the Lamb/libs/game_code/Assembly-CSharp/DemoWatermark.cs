// Decompiled with JetBrains decompiler
// Type: DemoWatermark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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

  public void Update() => this.Text.text = "Beta Squadron - Do Not Share!";

  public string GetTime()
  {
    float num1 = 1200f - CheatConsole.DemoBeginTime;
    int num2 = Mathf.FloorToInt(num1 / 60f);
    int num3 = 0;
    for (; num2 > 60; num2 -= 60)
      ++num3;
    int num4 = Mathf.FloorToInt(num1 % 60f);
    if (num3 > 0 && num3 < 10)
    {
      string str = "0" + num3.ToString();
    }
    if (num3 >= 10)
      num3.ToString();
    return $"{(num2 < 10 ? "0" : "")}{num2.ToString()}:{(num4 < 10 ? "0" : "")}{num4.ToString()}";
  }
}
