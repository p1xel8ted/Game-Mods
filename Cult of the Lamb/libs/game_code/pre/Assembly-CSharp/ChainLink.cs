// Decompiled with JetBrains decompiler
// Type: ChainLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ChainLink : BaseMonoBehaviour
{
  public float xSpeed;
  private float xTension = 0.1f;
  private float xDampening = 0.3f;
  public float ySpeed;
  private float yTension = 0.1f;
  private float yDampening = 0.3f;
  public float zSpeed;
  private float zTension = 0.1f;
  private float zDampening = 0.3f;
  private float grav = 0.01f;

  public float x
  {
    get => this.transform.position.x;
    set
    {
      this.transform.position = new Vector3(value, this.transform.position.y, this.transform.position.z);
    }
  }

  public float y
  {
    get => this.transform.position.y;
    set
    {
      this.transform.position = new Vector3(this.transform.position.x, value, this.transform.position.z);
    }
  }

  public float z
  {
    get => this.transform.position.z;
    set
    {
      this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, value);
    }
  }

  public void Init(float x, float y, float z) => this.transform.position = new Vector3(x, y, z);

  public void UpdatePositions(Vector3 TargetPosition)
  {
    this.ySpeed += (float) ((double) this.yTension * (double) (this.transform.position.y - TargetPosition.y) - (double) this.ySpeed * (double) this.yDampening);
    this.y -= this.ySpeed;
    this.xSpeed += (float) ((double) this.xTension * (double) (this.x - TargetPosition.x) - (double) this.xSpeed * (double) this.xDampening);
    this.x -= this.xSpeed;
    this.zSpeed += (float) ((double) this.zTension * (double) (this.z - TargetPosition.z) - (double) this.zSpeed * (double) this.xDampening);
    this.z -= this.zSpeed - this.grav;
    this.transform.position = new Vector3(this.x, this.y, this.z);
  }
}
