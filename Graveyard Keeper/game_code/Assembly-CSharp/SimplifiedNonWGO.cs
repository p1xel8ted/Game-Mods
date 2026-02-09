// Decompiled with JetBrains decompiler
// Type: SimplifiedNonWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimplifiedNonWGO : SimplifiedObject
{
  public Object prefab;
  public static Dictionary<string, bool> _prefab_has_chunck = new Dictionary<string, bool>();

  [ContextMenu("Restore")]
  public override GameObject Restore()
  {
    GameObject prefab = (GameObject) this.prefab;
    string name = prefab.name;
    if (prefab.activeSelf)
    {
      bool flag;
      if (!SimplifiedNonWGO._prefab_has_chunck.TryGetValue(name, out flag))
      {
        flag = (Object) prefab.GetComponent<ChunkedGameObject>() != (Object) null;
        SimplifiedNonWGO._prefab_has_chunck.Add(name, flag);
      }
      if (flag)
        prefab.SetActive(false);
    }
    GameObject o = Object.Instantiate<GameObject>(prefab, this.transform.parent);
    o.transform.localPosition = this.transform.localPosition;
    o.transform.localScale = this.transform.localScale;
    o.transform.localRotation = this.transform.localRotation;
    this.CommonRestore(o);
    if (Application.isPlaying && (Object) this.chunk != (Object) null)
    {
      this.chunk.ResetAtTheBeginning();
      this.chunk.Init();
    }
    if (Application.isPlaying)
      Object.Destroy((Object) this.gameObject);
    else
      Object.DestroyImmediate((Object) this.gameObject);
    int num = Application.isPlaying ? 1 : 0;
    return o.gameObject;
  }

  public void Start()
  {
  }
}
