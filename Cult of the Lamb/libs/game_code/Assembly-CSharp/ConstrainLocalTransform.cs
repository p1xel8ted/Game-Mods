// Decompiled with JetBrains decompiler
// Type: ConstrainLocalTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class ConstrainLocalTransform : BaseMonoBehaviour
{
  public bool ConstrainTransform = true;
  public Vector3 m_LastPosition = Vector3.zero;
  public Quaternion m_LastRotation = Quaternion.identity;
  public Vector3 m_LastScale = Vector3.zero;
}
