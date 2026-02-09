// Decompiled with JetBrains decompiler
// Type: SimplePhaseObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class SimplePhaseObject : MonoBehaviour
{
  [SerializeField]
  public UnityEvent onDefault;
  [SerializeField]
  public UnityEvent onDawn;
  [SerializeField]
  public UnityEvent onMorning;
  [SerializeField]
  public UnityEvent onAfternoon;
  [SerializeField]
  public UnityEvent onDusk;
  [SerializeField]
  public UnityEvent onNight;

  public void Awake()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.onDefault?.Invoke();
    this.OnNewPhaseStarted();
  }

  public void OnDestroy() => TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);

  public void OnNewPhaseStarted()
  {
    switch (TimeManager.CurrentPhase)
    {
      case DayPhase.Dawn:
        this.onDawn?.Invoke();
        break;
      case DayPhase.Morning:
        this.onMorning?.Invoke();
        break;
      case DayPhase.Afternoon:
        this.onAfternoon?.Invoke();
        break;
      case DayPhase.Dusk:
        this.onDusk?.Invoke();
        break;
      case DayPhase.Night:
        this.onNight?.Invoke();
        break;
    }
  }
}
