// Decompiled with JetBrains decompiler
// Type: Pathfinding.UnityReferenceHelper
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_unity_reference_helper.php")]
[ExecuteInEditMode]
public class UnityReferenceHelper : MonoBehaviour
{
  [SerializeField]
  [HideInInspector]
  public string guid;

  public string GetGUID() => this.guid;

  public void Awake() => this.Reset();

  public void Reset()
  {
    if (string.IsNullOrEmpty(this.guid))
    {
      this.guid = Guid.NewGuid().ToString();
      Debug.Log((object) ("Created new GUID - " + this.guid));
    }
    else
    {
      foreach (UnityReferenceHelper unityReferenceHelper in Object.FindObjectsOfType(typeof (UnityReferenceHelper)) as UnityReferenceHelper[])
      {
        if ((Object) unityReferenceHelper != (Object) this && this.guid == unityReferenceHelper.guid)
        {
          this.guid = Guid.NewGuid().ToString();
          Debug.Log((object) ("Created new GUID - " + this.guid));
          break;
        }
      }
    }
  }
}
