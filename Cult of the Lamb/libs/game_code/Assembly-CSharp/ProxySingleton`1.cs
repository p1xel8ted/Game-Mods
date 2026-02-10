// Decompiled with JetBrains decompiler
// Type: ProxySingleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class ProxySingleton<T> : MonoSingleton<T> where T : MonoBehaviour
{
  public override void Awake()
  {
    T component;
    if (!this.TryGetComponent<T>(out component))
      return;
    if ((Object) MonoSingleton<T>.Instance != (Object) component)
      Object.Destroy((Object) this.gameObject);
    else
      Object.DontDestroyOnLoad((Object) this);
  }
}
