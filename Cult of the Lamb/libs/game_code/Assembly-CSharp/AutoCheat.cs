// Decompiled with JetBrains decompiler
// Type: AutoCheat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AutoCheat : MonoBehaviour
{
  [SerializeField]
  public string[] _cheats;

  public void Start()
  {
    CheatConsole objectOfType = UnityEngine.Object.FindObjectOfType<CheatConsole>();
    foreach (string cheat in this._cheats)
    {
      System.Action action;
      if (objectOfType.Cheats.TryGetValue(cheat, out action))
        action();
    }
  }
}
