// Decompiled with JetBrains decompiler
// Type: DOGames.Scripts.BasicFPS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace DOGames.Scripts;

[RequireComponent(typeof (Text))]
public class BasicFPS : MonoBehaviour
{
  public const double FpsMeasurePeriod = 0.5;
  public double m_FpsNextPeriod;
  public int m_FpsAccumulator;
  public int m_CurrentFps;
  public Text m_Text;
  public const string display = "{0:D2}";

  public void Start()
  {
    this.m_Text = this.GetComponent<Text>();
    if (this.m_Text.gameObject.activeSelf && this.m_Text.enabled)
      return;
    this.m_Text = (Text) null;
  }

  public void Update()
  {
    ++this.m_FpsAccumulator;
    if ((double) Time.realtimeSinceStartup <= this.m_FpsNextPeriod)
      return;
    this.m_CurrentFps = (int) ((double) this.m_FpsAccumulator / 0.5);
    this.m_FpsAccumulator = 0;
    this.m_FpsNextPeriod = (double) Time.realtimeSinceStartup + 0.5;
    if (!((Object) this.m_Text != (Object) null))
      return;
    this.m_Text.text = $"{this.m_CurrentFps:D2}";
  }
}
