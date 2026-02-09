// Decompiled with JetBrains decompiler
// Type: UISnapshotPoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Snapshot Point")]
public class UISnapshotPoint : MonoBehaviour
{
  public bool isOrthographic = true;
  public float nearClip = -100f;
  public float farClip = 100f;
  [Range(10f, 80f)]
  public int fieldOfView = 35;
  public float orthoSize = 30f;
  public Texture2D thumbnail;

  public void Start()
  {
    if (!(this.tag != "EditorOnly"))
      return;
    this.tag = "EditorOnly";
  }
}
