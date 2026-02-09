// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddCameraFilterSceneInThePast
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Add Camera Filter Scene In The Past", 0)]
[Category("Game Actions")]
[Color("0bb0b0")]
public class Flow_AddCameraFilterSceneInThePast : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> par_remove = this.AddValueInput<bool>("Remove filter?");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (par_remove.value)
      {
        MainGame.me.gameObject.DestroyComponentIfExists<CameraFilterPack_Blur_Tilt_Shift_Hole>();
        MainGame.me.gameObject.DestroyComponentIfExists<CameraFilterPack_TV_WideScreenCircle>();
      }
      else
      {
        CameraFilterPack_Blur_Tilt_Shift_Hole blurTiltShiftHole = MainGame.me.gameObject.AddComponent<CameraFilterPack_Blur_Tilt_Shift_Hole>();
        blurTiltShiftHole.Amount = 3f;
        blurTiltShiftHole.FastFilter = 2;
        blurTiltShiftHole.Smooth = 0.191f;
        blurTiltShiftHole.Size = 0.661f;
        CameraFilterPack_TV_WideScreenCircle wideScreenCircle = MainGame.me.gameObject.AddComponent<CameraFilterPack_TV_WideScreenCircle>();
        wideScreenCircle.Size = 0.8f;
        wideScreenCircle.Smooth = 0.4f;
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("Remove filter?").value ? "Remove Camera Filter Scene In The Past" : base.name;
    }
    set => base.name = value;
  }
}
