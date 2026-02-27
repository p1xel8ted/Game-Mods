// Decompiled with JetBrains decompiler
// Type: ProxySingleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
