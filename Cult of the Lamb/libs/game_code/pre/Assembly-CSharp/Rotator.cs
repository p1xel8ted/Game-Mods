// Decompiled with JetBrains decompiler
// Type: Rotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Rotator : MonoBehaviour
{
  public Vector3 rotationDegrees;
  public bool localSpace;

  private void Start()
  {
  }

  private void Update()
  {
    this.transform.Rotate(this.rotationDegrees * Time.deltaTime, this.localSpace ? Space.Self : Space.World);
  }
}
