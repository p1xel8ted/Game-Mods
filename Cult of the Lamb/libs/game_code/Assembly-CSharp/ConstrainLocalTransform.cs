// Decompiled with JetBrains decompiler
// Type: ConstrainLocalTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
