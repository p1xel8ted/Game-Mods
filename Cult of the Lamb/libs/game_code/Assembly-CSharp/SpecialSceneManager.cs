// Decompiled with JetBrains decompiler
// Type: SpecialSceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpecialSceneManager : BaseMonoBehaviour
{
  public GameObject Telescope;
  public static SpecialSceneManager Instance;

  public void Start() => SpecialSceneManager.Instance = this;
}
