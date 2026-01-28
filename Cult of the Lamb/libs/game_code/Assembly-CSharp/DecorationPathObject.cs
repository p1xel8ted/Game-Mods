// Decompiled with JetBrains decompiler
// Type: DecorationPathObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class DecorationPathObject : BaseMonoBehaviour
{
  [SerializeField]
  public Structure structure;

  public void Awake() => this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);

  public void OnBrainAssigned()
  {
    PathTileManager.Instance.SetTile(this.structure.Type, this.transform.position);
    this.structure.OnProgressCompleted.RemoveListener(new UnityAction(this.OnBrainAssigned));
    this.Invoke("RemoveStruct", 0.01f);
  }

  public void RemoveStruct() => this.structure.RemoveStructure();
}
