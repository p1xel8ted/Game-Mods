// Decompiled with JetBrains decompiler
// Type: ConstrainLocalTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
