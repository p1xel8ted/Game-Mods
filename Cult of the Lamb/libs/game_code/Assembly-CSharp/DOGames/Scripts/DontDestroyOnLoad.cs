// Decompiled with JetBrains decompiler
// Type: DOGames.Scripts.DontDestroyOnLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace DOGames.Scripts;

public class DontDestroyOnLoad : MonoBehaviour
{
  public void Start() => Object.DontDestroyOnLoad((Object) this.gameObject);
}
