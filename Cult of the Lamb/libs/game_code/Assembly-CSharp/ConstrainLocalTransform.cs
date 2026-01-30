// Decompiled with JetBrains decompiler
// Type: ConstrainLocalTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
