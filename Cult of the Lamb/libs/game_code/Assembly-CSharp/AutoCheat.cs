// Decompiled with JetBrains decompiler
// Type: AutoCheat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
