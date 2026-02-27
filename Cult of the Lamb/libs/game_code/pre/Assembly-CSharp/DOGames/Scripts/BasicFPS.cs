// Decompiled with JetBrains decompiler
// Type: DOGames.Scripts.BasicFPS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace DOGames.Scripts;

[RequireComponent(typeof (Text))]
public class BasicFPS : MonoBehaviour
{
  private const double FpsMeasurePeriod = 0.5;
  private double m_FpsNextPeriod;
  private int m_FpsAccumulator;
  private int m_CurrentFps;
  private Text m_Text;
  private const string display = "{0:D2}";

  private void Start()
  {
    this.m_Text = this.GetComponent<Text>();
    if (this.m_Text.gameObject.activeSelf && this.m_Text.enabled)
      return;
    this.m_Text = (Text) null;
  }

  private void Update()
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
