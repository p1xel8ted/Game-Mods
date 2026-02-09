// Decompiled with JetBrains decompiler
// Type: AstarPathTester
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using UnityEngine;

#nullable disable
public class AstarPathTester : MonoBehaviour
{
  public GameObject to;

  [ContextMenu("Search path")]
  public void SearchPath()
  {
    Seeker seeker = this.gameObject.GetComponent<Seeker>();
    if ((Object) seeker == (Object) null)
      seeker = this.gameObject.AddComponent<Seeker>();
    seeker.StartPath(this.transform.position, this.to.transform.position, new OnPathDelegate(this.OnPathComplete));
  }

  public void OnPathComplete(Path p)
  {
    Debug.Log((object) $"On path complete. Error = {p.error.ToString()}, state = {p.CompleteState.ToString()}");
  }
}
