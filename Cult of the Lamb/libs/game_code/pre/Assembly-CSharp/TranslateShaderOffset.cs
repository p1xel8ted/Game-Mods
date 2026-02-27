// Decompiled with JetBrains decompiler
// Type: TranslateShaderOffset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TranslateShaderOffset : MonoBehaviour
{
  public string propertyName = "_MainTex";
  public Vector2 velocity;
  private Vector2 offset;

  private void Start()
  {
    this.offset = this.GetComponent<Renderer>().material.GetTextureOffset(this.propertyName);
  }

  private void Update()
  {
    this.offset += this.velocity * Time.deltaTime;
    this.GetComponent<Renderer>().material.SetTextureOffset(this.propertyName, this.offset);
  }
}
