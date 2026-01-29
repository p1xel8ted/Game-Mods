// Decompiled with JetBrains decompiler
// Type: DelayHealthFrameOnSpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class DelayHealthFrameOnSpawn : BaseMonoBehaviour
{
  public Health health;

  public void Awake()
  {
    this.health = this.GetComponent<Health>();
    this.health.enabled = false;
    this.StartCoroutine((IEnumerator) this.EnableHealthAfterFrameRoutine());
  }

  public IEnumerator EnableHealthAfterFrameRoutine()
  {
    yield return (object) null;
    this.health.enabled = true;
  }
}
