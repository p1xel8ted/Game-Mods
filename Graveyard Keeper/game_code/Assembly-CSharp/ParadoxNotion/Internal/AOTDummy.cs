// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Internal.AOTDummy
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using FlowCanvas.Nodes;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.Tasks.Conditions;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Internal;

public class AOTDummy
{
  public object o;

  public void FlowCanvas_ValueHandler_Delegate()
  {
  }

  public void FlowCanvas_Flow_ReadParameter_1()
  {
    Flow flow = new Flow();
    flow.ReadParameter<bool>((string) this.o);
    double num1 = (double) flow.ReadParameter<float>((string) this.o);
    flow.ReadParameter<int>((string) this.o);
    flow.ReadParameter<Vector2>((string) this.o);
    flow.ReadParameter<Vector3>((string) this.o);
    flow.ReadParameter<Vector4>((string) this.o);
    flow.ReadParameter<Quaternion>((string) this.o);
    flow.ReadParameter<Keyframe>((string) this.o);
    flow.ReadParameter<Bounds>((string) this.o);
    flow.ReadParameter<Color>((string) this.o);
    flow.ReadParameter<Rect>((string) this.o);
    flow.ReadParameter<ContactPoint>((string) this.o);
    flow.ReadParameter<ContactPoint2D>((string) this.o);
    flow.ReadParameter<Collision>((string) this.o);
    flow.ReadParameter<Collision2D>((string) this.o);
    flow.ReadParameter<RaycastHit>((string) this.o);
    flow.ReadParameter<RaycastHit2D>((string) this.o);
    flow.ReadParameter<Ray>((string) this.o);
    int num2 = (int) flow.ReadParameter<Space>((string) this.o);
    int num3 = (int) flow.ReadParameter<Direction>((string) this.o);
    int num4 = (int) flow.ReadParameter<ItemDefinition.EquipmentType>((string) this.o);
    int num5 = (int) flow.ReadParameter<MovementComponent.GoToMethod>((string) this.o);
    int num6 = (int) flow.ReadParameter<GDPoint.IdlePointPrefix>((string) this.o);
    flow.ReadParameter<LayerMask>((string) this.o);
  }

  public void FlowCanvas_Flow_WriteParameter_2()
  {
    Flow flow = new Flow();
    flow.WriteParameter<bool>((string) this.o, (bool) this.o);
    flow.WriteParameter<float>((string) this.o, (float) this.o);
    flow.WriteParameter<int>((string) this.o, (int) this.o);
    flow.WriteParameter<Vector2>((string) this.o, (Vector2) this.o);
    flow.WriteParameter<Vector3>((string) this.o, (Vector3) this.o);
    flow.WriteParameter<Vector4>((string) this.o, (Vector4) this.o);
    flow.WriteParameter<Quaternion>((string) this.o, (Quaternion) this.o);
    flow.WriteParameter<Keyframe>((string) this.o, (Keyframe) this.o);
    flow.WriteParameter<Bounds>((string) this.o, (Bounds) this.o);
    flow.WriteParameter<Color>((string) this.o, (Color) this.o);
    flow.WriteParameter<Rect>((string) this.o, (Rect) this.o);
    flow.WriteParameter<ContactPoint>((string) this.o, (ContactPoint) this.o);
    flow.WriteParameter<ContactPoint2D>((string) this.o, (ContactPoint2D) this.o);
    flow.WriteParameter<Collision>((string) this.o, (Collision) this.o);
    flow.WriteParameter<Collision2D>((string) this.o, (Collision2D) this.o);
    flow.WriteParameter<RaycastHit>((string) this.o, (RaycastHit) this.o);
    flow.WriteParameter<RaycastHit2D>((string) this.o, (RaycastHit2D) this.o);
    flow.WriteParameter<Ray>((string) this.o, (Ray) this.o);
    flow.WriteParameter<Space>((string) this.o, (Space) this.o);
    flow.WriteParameter<Direction>((string) this.o, (Direction) this.o);
    flow.WriteParameter<ItemDefinition.EquipmentType>((string) this.o, (ItemDefinition.EquipmentType) this.o);
    flow.WriteParameter<MovementComponent.GoToMethod>((string) this.o, (MovementComponent.GoToMethod) this.o);
    flow.WriteParameter<GDPoint.IdlePointPrefix>((string) this.o, (GDPoint.IdlePointPrefix) this.o);
    flow.WriteParameter<LayerMask>((string) this.o, (LayerMask) this.o);
  }

  public void FlowCanvas_FlowNode_AddValueInput_1()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((FlowNode) local).AddValueInput<bool>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<float>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<int>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Vector2>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Vector3>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Vector4>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Quaternion>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Keyframe>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Bounds>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Color>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Rect>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<ContactPoint>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<ContactPoint2D>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Collision>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Collision2D>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<RaycastHit>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<RaycastHit2D>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Ray>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Space>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<Direction>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<ItemDefinition.EquipmentType>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<MovementComponent.GoToMethod>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<GDPoint.IdlePointPrefix>((string) this.o, (string) this.o);
    ((FlowNode) local).AddValueInput<LayerMask>((string) this.o, (string) this.o);
  }

  public void FlowCanvas_FlowNode_AddVerticalValueInput_2()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((FlowNode) local).AddVerticalValueInput<bool>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<float>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<int>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Vector2>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Vector3>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Vector4>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Quaternion>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Keyframe>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Bounds>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Color>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Rect>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<ContactPoint>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<ContactPoint2D>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Collision>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Collision2D>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<RaycastHit>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<RaycastHit2D>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Ray>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Space>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<Direction>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<ItemDefinition.EquipmentType>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<MovementComponent.GoToMethod>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<GDPoint.IdlePointPrefix>((string) this.o, (string) this.o);
    ((FlowNode) local).AddVerticalValueInput<LayerMask>((string) this.o, (string) this.o);
  }

  public void FlowCanvas_FlowNode_AddValueOutput_3()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((FlowNode) local).AddValueOutput<bool>((string) this.o, (string) this.o, (ValueHandler<bool>) this.o);
    ((FlowNode) local).AddValueOutput<float>((string) this.o, (string) this.o, (ValueHandler<float>) this.o);
    ((FlowNode) local).AddValueOutput<int>((string) this.o, (string) this.o, (ValueHandler<int>) this.o);
    ((FlowNode) local).AddValueOutput<Vector2>((string) this.o, (string) this.o, (ValueHandler<Vector2>) this.o);
    ((FlowNode) local).AddValueOutput<Vector3>((string) this.o, (string) this.o, (ValueHandler<Vector3>) this.o);
    ((FlowNode) local).AddValueOutput<Vector4>((string) this.o, (string) this.o, (ValueHandler<Vector4>) this.o);
    ((FlowNode) local).AddValueOutput<Quaternion>((string) this.o, (string) this.o, (ValueHandler<Quaternion>) this.o);
    ((FlowNode) local).AddValueOutput<Keyframe>((string) this.o, (string) this.o, (ValueHandler<Keyframe>) this.o);
    ((FlowNode) local).AddValueOutput<Bounds>((string) this.o, (string) this.o, (ValueHandler<Bounds>) this.o);
    ((FlowNode) local).AddValueOutput<Color>((string) this.o, (string) this.o, (ValueHandler<Color>) this.o);
    ((FlowNode) local).AddValueOutput<Rect>((string) this.o, (string) this.o, (ValueHandler<Rect>) this.o);
    ((FlowNode) local).AddValueOutput<ContactPoint>((string) this.o, (string) this.o, (ValueHandler<ContactPoint>) this.o);
    ((FlowNode) local).AddValueOutput<ContactPoint2D>((string) this.o, (string) this.o, (ValueHandler<ContactPoint2D>) this.o);
    ((FlowNode) local).AddValueOutput<Collision>((string) this.o, (string) this.o, (ValueHandler<Collision>) this.o);
    ((FlowNode) local).AddValueOutput<Collision2D>((string) this.o, (string) this.o, (ValueHandler<Collision2D>) this.o);
    ((FlowNode) local).AddValueOutput<RaycastHit>((string) this.o, (string) this.o, (ValueHandler<RaycastHit>) this.o);
    ((FlowNode) local).AddValueOutput<RaycastHit2D>((string) this.o, (string) this.o, (ValueHandler<RaycastHit2D>) this.o);
    ((FlowNode) local).AddValueOutput<Ray>((string) this.o, (string) this.o, (ValueHandler<Ray>) this.o);
    ((FlowNode) local).AddValueOutput<Space>((string) this.o, (string) this.o, (ValueHandler<Space>) this.o);
    ((FlowNode) local).AddValueOutput<Direction>((string) this.o, (string) this.o, (ValueHandler<Direction>) this.o);
    ((FlowNode) local).AddValueOutput<ItemDefinition.EquipmentType>((string) this.o, (string) this.o, (ValueHandler<ItemDefinition.EquipmentType>) this.o);
    ((FlowNode) local).AddValueOutput<MovementComponent.GoToMethod>((string) this.o, (string) this.o, (ValueHandler<MovementComponent.GoToMethod>) this.o);
    ((FlowNode) local).AddValueOutput<GDPoint.IdlePointPrefix>((string) this.o, (string) this.o, (ValueHandler<GDPoint.IdlePointPrefix>) this.o);
    ((FlowNode) local).AddValueOutput<LayerMask>((string) this.o, (string) this.o, (ValueHandler<LayerMask>) this.o);
  }

  public void FlowCanvas_FlowNode_AddValueOutput_4()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((FlowNode) local).AddValueOutput<bool>((string) this.o, (ValueHandler<bool>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<float>((string) this.o, (ValueHandler<float>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<int>((string) this.o, (ValueHandler<int>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Vector2>((string) this.o, (ValueHandler<Vector2>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Vector3>((string) this.o, (ValueHandler<Vector3>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Vector4>((string) this.o, (ValueHandler<Vector4>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Quaternion>((string) this.o, (ValueHandler<Quaternion>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Keyframe>((string) this.o, (ValueHandler<Keyframe>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Bounds>((string) this.o, (ValueHandler<Bounds>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Color>((string) this.o, (ValueHandler<Color>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Rect>((string) this.o, (ValueHandler<Rect>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<ContactPoint>((string) this.o, (ValueHandler<ContactPoint>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<ContactPoint2D>((string) this.o, (ValueHandler<ContactPoint2D>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Collision>((string) this.o, (ValueHandler<Collision>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Collision2D>((string) this.o, (ValueHandler<Collision2D>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<RaycastHit>((string) this.o, (ValueHandler<RaycastHit>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<RaycastHit2D>((string) this.o, (ValueHandler<RaycastHit2D>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Ray>((string) this.o, (ValueHandler<Ray>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Space>((string) this.o, (ValueHandler<Space>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<Direction>((string) this.o, (ValueHandler<Direction>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<ItemDefinition.EquipmentType>((string) this.o, (ValueHandler<ItemDefinition.EquipmentType>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<MovementComponent.GoToMethod>((string) this.o, (ValueHandler<MovementComponent.GoToMethod>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<GDPoint.IdlePointPrefix>((string) this.o, (ValueHandler<GDPoint.IdlePointPrefix>) this.o, (string) this.o);
    ((FlowNode) local).AddValueOutput<LayerMask>((string) this.o, (ValueHandler<LayerMask>) this.o, (string) this.o);
  }

  public void FlowCanvas_ValueInput_CreateInstance_1()
  {
    ValueInput.CreateInstance<bool>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<float>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<int>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Vector2>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Vector3>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Vector4>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Quaternion>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Keyframe>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Bounds>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Color>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Rect>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<ContactPoint>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<ContactPoint2D>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Collision>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Collision2D>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<RaycastHit>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<RaycastHit2D>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Ray>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Space>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<Direction>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<ItemDefinition.EquipmentType>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<MovementComponent.GoToMethod>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<GDPoint.IdlePointPrefix>((FlowNode) this.o, (string) this.o, (string) this.o);
    ValueInput.CreateInstance<LayerMask>((FlowNode) this.o, (string) this.o, (string) this.o);
  }

  public void FlowCanvas_ValueOutput_CreateInstance_1()
  {
    ValueOutput.CreateInstance<bool>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<bool>) this.o);
    ValueOutput.CreateInstance<float>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<float>) this.o);
    ValueOutput.CreateInstance<int>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<int>) this.o);
    ValueOutput.CreateInstance<Vector2>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Vector2>) this.o);
    ValueOutput.CreateInstance<Vector3>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Vector3>) this.o);
    ValueOutput.CreateInstance<Vector4>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Vector4>) this.o);
    ValueOutput.CreateInstance<Quaternion>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Quaternion>) this.o);
    ValueOutput.CreateInstance<Keyframe>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Keyframe>) this.o);
    ValueOutput.CreateInstance<Bounds>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Bounds>) this.o);
    ValueOutput.CreateInstance<Color>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Color>) this.o);
    ValueOutput.CreateInstance<Rect>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Rect>) this.o);
    ValueOutput.CreateInstance<ContactPoint>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<ContactPoint>) this.o);
    ValueOutput.CreateInstance<ContactPoint2D>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<ContactPoint2D>) this.o);
    ValueOutput.CreateInstance<Collision>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Collision>) this.o);
    ValueOutput.CreateInstance<Collision2D>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Collision2D>) this.o);
    ValueOutput.CreateInstance<RaycastHit>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<RaycastHit>) this.o);
    ValueOutput.CreateInstance<RaycastHit2D>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<RaycastHit2D>) this.o);
    ValueOutput.CreateInstance<Ray>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Ray>) this.o);
    ValueOutput.CreateInstance<Space>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Space>) this.o);
    ValueOutput.CreateInstance<Direction>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<Direction>) this.o);
    ValueOutput.CreateInstance<ItemDefinition.EquipmentType>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<ItemDefinition.EquipmentType>) this.o);
    ValueOutput.CreateInstance<MovementComponent.GoToMethod>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<MovementComponent.GoToMethod>) this.o);
    ValueOutput.CreateInstance<GDPoint.IdlePointPrefix>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<GDPoint.IdlePointPrefix>) this.o);
    ValueOutput.CreateInstance<LayerMask>((FlowNode) this.o, (string) this.o, (string) this.o, (ValueHandler<LayerMask>) this.o);
  }

  public void FlowCanvas_Nodes_ReflectedDelegateEvent_Callback1_1()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((ReflectedDelegateEvent) local).Callback1<bool>((bool) this.o);
    ((ReflectedDelegateEvent) local).Callback1<float>((float) this.o);
    ((ReflectedDelegateEvent) local).Callback1<int>((int) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Vector2>((Vector2) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Vector3>((Vector3) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Vector4>((Vector4) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Quaternion>((Quaternion) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Keyframe>((Keyframe) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Bounds>((Bounds) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Color>((Color) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Rect>((Rect) this.o);
    ((ReflectedDelegateEvent) local).Callback1<ContactPoint>((ContactPoint) this.o);
    ((ReflectedDelegateEvent) local).Callback1<ContactPoint2D>((ContactPoint2D) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Collision>((Collision) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Collision2D>((Collision2D) this.o);
    ((ReflectedDelegateEvent) local).Callback1<RaycastHit>((RaycastHit) this.o);
    ((ReflectedDelegateEvent) local).Callback1<RaycastHit2D>((RaycastHit2D) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Ray>((Ray) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Space>((Space) this.o);
    ((ReflectedDelegateEvent) local).Callback1<Direction>((Direction) this.o);
    ((ReflectedDelegateEvent) local).Callback1<ItemDefinition.EquipmentType>((ItemDefinition.EquipmentType) this.o);
    ((ReflectedDelegateEvent) local).Callback1<MovementComponent.GoToMethod>((MovementComponent.GoToMethod) this.o);
    ((ReflectedDelegateEvent) local).Callback1<GDPoint.IdlePointPrefix>((GDPoint.IdlePointPrefix) this.o);
    ((ReflectedDelegateEvent) local).Callback1<LayerMask>((LayerMask) this.o);
  }

  public void FlowCanvas_Nodes_ReflectedUnityEvent_CallbackMethod1_1()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((ReflectedUnityEvent) local).CallbackMethod1<bool>((bool) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<float>((float) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<int>((int) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Vector2>((Vector2) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Vector3>((Vector3) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Vector4>((Vector4) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Quaternion>((Quaternion) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Keyframe>((Keyframe) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Bounds>((Bounds) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Color>((Color) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Rect>((Rect) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<ContactPoint>((ContactPoint) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<ContactPoint2D>((ContactPoint2D) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Collision>((Collision) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Collision2D>((Collision2D) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<RaycastHit>((RaycastHit) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<RaycastHit2D>((RaycastHit2D) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Ray>((Ray) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Space>((Space) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<Direction>((Direction) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<ItemDefinition.EquipmentType>((ItemDefinition.EquipmentType) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<MovementComponent.GoToMethod>((MovementComponent.GoToMethod) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<GDPoint.IdlePointPrefix>((GDPoint.IdlePointPrefix) this.o);
    ((ReflectedUnityEvent) local).CallbackMethod1<LayerMask>((LayerMask) this.o);
  }

  public void NodeCanvas_Framework_Blackboard_GetVariable_1()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((Blackboard) local).GetVariable<bool>((string) this.o);
    ((Blackboard) local).GetVariable<float>((string) this.o);
    ((Blackboard) local).GetVariable<int>((string) this.o);
    ((Blackboard) local).GetVariable<Vector2>((string) this.o);
    ((Blackboard) local).GetVariable<Vector3>((string) this.o);
    ((Blackboard) local).GetVariable<Vector4>((string) this.o);
    ((Blackboard) local).GetVariable<Quaternion>((string) this.o);
    ((Blackboard) local).GetVariable<Keyframe>((string) this.o);
    ((Blackboard) local).GetVariable<Bounds>((string) this.o);
    ((Blackboard) local).GetVariable<Color>((string) this.o);
    ((Blackboard) local).GetVariable<Rect>((string) this.o);
    ((Blackboard) local).GetVariable<ContactPoint>((string) this.o);
    ((Blackboard) local).GetVariable<ContactPoint2D>((string) this.o);
    ((Blackboard) local).GetVariable<Collision>((string) this.o);
    ((Blackboard) local).GetVariable<Collision2D>((string) this.o);
    ((Blackboard) local).GetVariable<RaycastHit>((string) this.o);
    ((Blackboard) local).GetVariable<RaycastHit2D>((string) this.o);
    ((Blackboard) local).GetVariable<Ray>((string) this.o);
    ((Blackboard) local).GetVariable<Space>((string) this.o);
    ((Blackboard) local).GetVariable<Direction>((string) this.o);
    ((Blackboard) local).GetVariable<ItemDefinition.EquipmentType>((string) this.o);
    ((Blackboard) local).GetVariable<MovementComponent.GoToMethod>((string) this.o);
    ((Blackboard) local).GetVariable<GDPoint.IdlePointPrefix>((string) this.o);
    ((Blackboard) local).GetVariable<LayerMask>((string) this.o);
  }

  public void NodeCanvas_Framework_Blackboard_GetValue_2()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((Blackboard) local).GetValue<bool>((string) this.o);
    double num1 = (double) ((Blackboard) local).GetValue<float>((string) this.o);
    ((Blackboard) local).GetValue<int>((string) this.o);
    ((Blackboard) local).GetValue<Vector2>((string) this.o);
    ((Blackboard) local).GetValue<Vector3>((string) this.o);
    ((Blackboard) local).GetValue<Vector4>((string) this.o);
    ((Blackboard) local).GetValue<Quaternion>((string) this.o);
    ((Blackboard) local).GetValue<Keyframe>((string) this.o);
    ((Blackboard) local).GetValue<Bounds>((string) this.o);
    ((Blackboard) local).GetValue<Color>((string) this.o);
    ((Blackboard) local).GetValue<Rect>((string) this.o);
    ((Blackboard) local).GetValue<ContactPoint>((string) this.o);
    ((Blackboard) local).GetValue<ContactPoint2D>((string) this.o);
    ((Blackboard) local).GetValue<Collision>((string) this.o);
    ((Blackboard) local).GetValue<Collision2D>((string) this.o);
    ((Blackboard) local).GetValue<RaycastHit>((string) this.o);
    ((Blackboard) local).GetValue<RaycastHit2D>((string) this.o);
    ((Blackboard) local).GetValue<Ray>((string) this.o);
    int num2 = (int) ((Blackboard) local).GetValue<Space>((string) this.o);
    int num3 = (int) ((Blackboard) local).GetValue<Direction>((string) this.o);
    int num4 = (int) ((Blackboard) local).GetValue<ItemDefinition.EquipmentType>((string) this.o);
    int num5 = (int) ((Blackboard) local).GetValue<MovementComponent.GoToMethod>((string) this.o);
    int num6 = (int) ((Blackboard) local).GetValue<GDPoint.IdlePointPrefix>((string) this.o);
    ((Blackboard) local).GetValue<LayerMask>((string) this.o);
  }

  public void NodeCanvas_Framework_IBlackboard_GetVariable_1()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((IBlackboard) local).GetVariable<bool>((string) this.o);
    ((IBlackboard) local).GetVariable<float>((string) this.o);
    ((IBlackboard) local).GetVariable<int>((string) this.o);
    ((IBlackboard) local).GetVariable<Vector2>((string) this.o);
    ((IBlackboard) local).GetVariable<Vector3>((string) this.o);
    ((IBlackboard) local).GetVariable<Vector4>((string) this.o);
    ((IBlackboard) local).GetVariable<Quaternion>((string) this.o);
    ((IBlackboard) local).GetVariable<Keyframe>((string) this.o);
    ((IBlackboard) local).GetVariable<Bounds>((string) this.o);
    ((IBlackboard) local).GetVariable<Color>((string) this.o);
    ((IBlackboard) local).GetVariable<Rect>((string) this.o);
    ((IBlackboard) local).GetVariable<ContactPoint>((string) this.o);
    ((IBlackboard) local).GetVariable<ContactPoint2D>((string) this.o);
    ((IBlackboard) local).GetVariable<Collision>((string) this.o);
    ((IBlackboard) local).GetVariable<Collision2D>((string) this.o);
    ((IBlackboard) local).GetVariable<RaycastHit>((string) this.o);
    ((IBlackboard) local).GetVariable<RaycastHit2D>((string) this.o);
    ((IBlackboard) local).GetVariable<Ray>((string) this.o);
    ((IBlackboard) local).GetVariable<Space>((string) this.o);
    ((IBlackboard) local).GetVariable<Direction>((string) this.o);
    ((IBlackboard) local).GetVariable<ItemDefinition.EquipmentType>((string) this.o);
    ((IBlackboard) local).GetVariable<MovementComponent.GoToMethod>((string) this.o);
    ((IBlackboard) local).GetVariable<GDPoint.IdlePointPrefix>((string) this.o);
    ((IBlackboard) local).GetVariable<LayerMask>((string) this.o);
  }

  public void NodeCanvas_Framework_IBlackboard_GetValue_2()
  {
    // ISSUE: variable of the null type
    __Null local = null;
    ((IBlackboard) local).GetValue<bool>((string) this.o);
    double num1 = (double) ((IBlackboard) local).GetValue<float>((string) this.o);
    ((IBlackboard) local).GetValue<int>((string) this.o);
    ((IBlackboard) local).GetValue<Vector2>((string) this.o);
    ((IBlackboard) local).GetValue<Vector3>((string) this.o);
    ((IBlackboard) local).GetValue<Vector4>((string) this.o);
    ((IBlackboard) local).GetValue<Quaternion>((string) this.o);
    ((IBlackboard) local).GetValue<Keyframe>((string) this.o);
    ((IBlackboard) local).GetValue<Bounds>((string) this.o);
    ((IBlackboard) local).GetValue<Color>((string) this.o);
    ((IBlackboard) local).GetValue<Rect>((string) this.o);
    ((IBlackboard) local).GetValue<ContactPoint>((string) this.o);
    ((IBlackboard) local).GetValue<ContactPoint2D>((string) this.o);
    ((IBlackboard) local).GetValue<Collision>((string) this.o);
    ((IBlackboard) local).GetValue<Collision2D>((string) this.o);
    ((IBlackboard) local).GetValue<RaycastHit>((string) this.o);
    ((IBlackboard) local).GetValue<RaycastHit2D>((string) this.o);
    ((IBlackboard) local).GetValue<Ray>((string) this.o);
    int num2 = (int) ((IBlackboard) local).GetValue<Space>((string) this.o);
    int num3 = (int) ((IBlackboard) local).GetValue<Direction>((string) this.o);
    int num4 = (int) ((IBlackboard) local).GetValue<ItemDefinition.EquipmentType>((string) this.o);
    int num5 = (int) ((IBlackboard) local).GetValue<MovementComponent.GoToMethod>((string) this.o);
    int num6 = (int) ((IBlackboard) local).GetValue<GDPoint.IdlePointPrefix>((string) this.o);
    ((IBlackboard) local).GetValue<LayerMask>((string) this.o);
  }

  public void CustomSpoof()
  {
  }

  public class FlowCanvas_BinderConnection_System_Boolean : BinderConnection<bool>
  {
  }

  public class FlowCanvas_BinderConnection_System_Single : BinderConnection<float>
  {
  }

  public class FlowCanvas_BinderConnection_System_Int32 : BinderConnection<int>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Vector2 : BinderConnection<Vector2>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Vector3 : BinderConnection<Vector3>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Vector4 : BinderConnection<Vector4>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Quaternion : BinderConnection<Quaternion>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Keyframe : BinderConnection<Keyframe>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Bounds : BinderConnection<Bounds>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Color : BinderConnection<Color>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Rect : BinderConnection<Rect>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_ContactPoint : BinderConnection<ContactPoint>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_ContactPoint2D : 
    BinderConnection<ContactPoint2D>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Collision : BinderConnection<Collision>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Collision2D : BinderConnection<Collision2D>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_RaycastHit : BinderConnection<RaycastHit>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_RaycastHit2D : BinderConnection<RaycastHit2D>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Ray : BinderConnection<Ray>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_Space : BinderConnection<Space>
  {
  }

  public class FlowCanvas_BinderConnection_Direction : BinderConnection<Direction>
  {
  }

  public class FlowCanvas_BinderConnection_ItemDefinition_EquipmentType : 
    BinderConnection<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_BinderConnection_MovementComponent_GoToMethod : 
    BinderConnection<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_BinderConnection_GDPoint_IdlePointPrefix : 
    BinderConnection<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_BinderConnection_UnityEngine_LayerMask : BinderConnection<LayerMask>
  {
  }

  public class FlowCanvas_ValueInput_System_Boolean : ValueInput<bool>
  {
  }

  public class FlowCanvas_ValueInput_System_Single : ValueInput<float>
  {
  }

  public class FlowCanvas_ValueInput_System_Int32 : ValueInput<int>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Vector2 : ValueInput<Vector2>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Vector3 : ValueInput<Vector3>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Vector4 : ValueInput<Vector4>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Quaternion : ValueInput<Quaternion>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Keyframe : ValueInput<Keyframe>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Bounds : ValueInput<Bounds>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Color : ValueInput<Color>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Rect : ValueInput<Rect>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_ContactPoint : ValueInput<ContactPoint>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_ContactPoint2D : ValueInput<ContactPoint2D>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Collision : ValueInput<Collision>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Collision2D : ValueInput<Collision2D>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_RaycastHit : ValueInput<RaycastHit>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_RaycastHit2D : ValueInput<RaycastHit2D>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Ray : ValueInput<Ray>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_Space : ValueInput<Space>
  {
  }

  public class FlowCanvas_ValueInput_Direction : ValueInput<Direction>
  {
  }

  public class FlowCanvas_ValueInput_ItemDefinition_EquipmentType : 
    ValueInput<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_ValueInput_MovementComponent_GoToMethod : 
    ValueInput<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_ValueInput_GDPoint_IdlePointPrefix : ValueInput<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_ValueInput_UnityEngine_LayerMask : ValueInput<LayerMask>
  {
  }

  public class FlowCanvas_ValueOutput_System_Boolean : ValueOutput<bool>
  {
  }

  public class FlowCanvas_ValueOutput_System_Single : ValueOutput<float>
  {
  }

  public class FlowCanvas_ValueOutput_System_Int32 : ValueOutput<int>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Vector2 : ValueOutput<Vector2>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Vector3 : ValueOutput<Vector3>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Vector4 : ValueOutput<Vector4>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Quaternion : ValueOutput<Quaternion>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Keyframe : ValueOutput<Keyframe>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Bounds : ValueOutput<Bounds>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Color : ValueOutput<Color>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Rect : ValueOutput<Rect>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_ContactPoint : ValueOutput<ContactPoint>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_ContactPoint2D : ValueOutput<ContactPoint2D>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Collision : ValueOutput<Collision>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Collision2D : ValueOutput<Collision2D>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_RaycastHit : ValueOutput<RaycastHit>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_RaycastHit2D : ValueOutput<RaycastHit2D>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Ray : ValueOutput<Ray>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_Space : ValueOutput<Space>
  {
  }

  public class FlowCanvas_ValueOutput_Direction : ValueOutput<Direction>
  {
  }

  public class FlowCanvas_ValueOutput_ItemDefinition_EquipmentType : 
    ValueOutput<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_ValueOutput_MovementComponent_GoToMethod : 
    ValueOutput<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_ValueOutput_GDPoint_IdlePointPrefix : ValueOutput<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_ValueOutput_UnityEngine_LayerMask : ValueOutput<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_System_Boolean : AddDictionaryItem<bool>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_System_Single : AddDictionaryItem<float>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_System_Int32 : AddDictionaryItem<int>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Vector2 : AddDictionaryItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Vector3 : AddDictionaryItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Vector4 : AddDictionaryItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Quaternion : 
    AddDictionaryItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Keyframe : AddDictionaryItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Bounds : AddDictionaryItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Color : AddDictionaryItem<Color>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Rect : AddDictionaryItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_ContactPoint : 
    AddDictionaryItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_ContactPoint2D : 
    AddDictionaryItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Collision : 
    AddDictionaryItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Collision2D : 
    AddDictionaryItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_RaycastHit : 
    AddDictionaryItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_RaycastHit2D : 
    AddDictionaryItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Ray : AddDictionaryItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Space : AddDictionaryItem<Space>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_Direction : AddDictionaryItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_ItemDefinition_EquipmentType : 
    AddDictionaryItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_MovementComponent_GoToMethod : 
    AddDictionaryItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_GDPoint_IdlePointPrefix : 
    AddDictionaryItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_LayerMask : 
    AddDictionaryItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_System_Boolean : AddListItem<bool>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_System_Single : AddListItem<float>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_System_Int32 : AddListItem<int>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Vector2 : AddListItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Vector3 : AddListItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Vector4 : AddListItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Quaternion : AddListItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Keyframe : AddListItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Bounds : AddListItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Color : AddListItem<Color>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Rect : AddListItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_ContactPoint : AddListItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_ContactPoint2D : AddListItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Collision : AddListItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Collision2D : AddListItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_RaycastHit : AddListItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_RaycastHit2D : AddListItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Ray : AddListItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_Space : AddListItem<Space>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_Direction : AddListItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_ItemDefinition_EquipmentType : 
    AddListItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_MovementComponent_GoToMethod : 
    AddListItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_GDPoint_IdlePointPrefix : 
    AddListItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_AddListItem_UnityEngine_LayerMask : AddListItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_Cache_System_Boolean : Cache<bool>
  {
  }

  public class FlowCanvas_Nodes_Cache_System_Single : Cache<float>
  {
  }

  public class FlowCanvas_Nodes_Cache_System_Int32 : Cache<int>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Vector2 : Cache<Vector2>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Vector3 : Cache<Vector3>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Vector4 : Cache<Vector4>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Quaternion : Cache<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Keyframe : Cache<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Bounds : Cache<Bounds>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Color : Cache<Color>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Rect : Cache<Rect>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_ContactPoint : Cache<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_ContactPoint2D : Cache<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Collision : Cache<Collision>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Collision2D : Cache<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_RaycastHit : Cache<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_RaycastHit2D : Cache<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Ray : Cache<Ray>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_Space : Cache<Space>
  {
  }

  public class FlowCanvas_Nodes_Cache_Direction : Cache<Direction>
  {
  }

  public class FlowCanvas_Nodes_Cache_ItemDefinition_EquipmentType : 
    Cache<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_Cache_MovementComponent_GoToMethod : 
    Cache<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_Cache_GDPoint_IdlePointPrefix : Cache<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_Cache_UnityEngine_LayerMask : Cache<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_System_Boolean : CreateCollection<bool>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_System_Single : CreateCollection<float>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_System_Int32 : CreateCollection<int>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Vector2 : CreateCollection<Vector2>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Vector3 : CreateCollection<Vector3>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Vector4 : CreateCollection<Vector4>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Quaternion : 
    CreateCollection<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Keyframe : CreateCollection<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Bounds : CreateCollection<Bounds>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Color : CreateCollection<Color>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Rect : CreateCollection<Rect>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_ContactPoint : 
    CreateCollection<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_ContactPoint2D : 
    CreateCollection<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Collision : CreateCollection<Collision>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Collision2D : 
    CreateCollection<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_RaycastHit : 
    CreateCollection<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_RaycastHit2D : 
    CreateCollection<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Ray : CreateCollection<Ray>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_Space : CreateCollection<Space>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_Direction : CreateCollection<Direction>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_ItemDefinition_EquipmentType : 
    CreateCollection<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_MovementComponent_GoToMethod : 
    CreateCollection<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_GDPoint_IdlePointPrefix : 
    CreateCollection<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_CreateCollection_UnityEngine_LayerMask : CreateCollection<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_System_Boolean : CreateDictionary<bool>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_System_Single : CreateDictionary<float>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_System_Int32 : CreateDictionary<int>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Vector2 : CreateDictionary<Vector2>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Vector3 : CreateDictionary<Vector3>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Vector4 : CreateDictionary<Vector4>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Quaternion : 
    CreateDictionary<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Keyframe : CreateDictionary<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Bounds : CreateDictionary<Bounds>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Color : CreateDictionary<Color>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Rect : CreateDictionary<Rect>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_ContactPoint : 
    CreateDictionary<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_ContactPoint2D : 
    CreateDictionary<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Collision : CreateDictionary<Collision>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Collision2D : 
    CreateDictionary<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_RaycastHit : 
    CreateDictionary<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_RaycastHit2D : 
    CreateDictionary<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Ray : CreateDictionary<Ray>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_Space : CreateDictionary<Space>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_Direction : CreateDictionary<Direction>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_ItemDefinition_EquipmentType : 
    CreateDictionary<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_MovementComponent_GoToMethod : 
    CreateDictionary<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_GDPoint_IdlePointPrefix : 
    CreateDictionary<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_CreateDictionary_UnityEngine_LayerMask : CreateDictionary<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_System_Boolean : CustomEvent<bool>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_System_Single : CustomEvent<float>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_System_Int32 : CustomEvent<int>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Vector2 : CustomEvent<Vector2>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Vector3 : CustomEvent<Vector3>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Vector4 : CustomEvent<Vector4>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Quaternion : CustomEvent<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Keyframe : CustomEvent<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Bounds : CustomEvent<Bounds>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Color : CustomEvent<Color>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Rect : CustomEvent<Rect>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_ContactPoint : CustomEvent<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_ContactPoint2D : CustomEvent<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Collision : CustomEvent<Collision>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Collision2D : CustomEvent<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_RaycastHit : CustomEvent<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_RaycastHit2D : CustomEvent<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Ray : CustomEvent<Ray>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_Space : CustomEvent<Space>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_Direction : CustomEvent<Direction>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_ItemDefinition_EquipmentType : 
    CustomEvent<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_MovementComponent_GoToMethod : 
    CustomEvent<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_GDPoint_IdlePointPrefix : 
    CustomEvent<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_CustomEvent_UnityEngine_LayerMask : CustomEvent<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_System_Boolean : DictionaryContainsKey<bool>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_System_Single : DictionaryContainsKey<float>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_System_Int32 : DictionaryContainsKey<int>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Vector2 : 
    DictionaryContainsKey<Vector2>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Vector3 : 
    DictionaryContainsKey<Vector3>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Vector4 : 
    DictionaryContainsKey<Vector4>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Quaternion : 
    DictionaryContainsKey<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Keyframe : 
    DictionaryContainsKey<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Bounds : 
    DictionaryContainsKey<Bounds>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Color : 
    DictionaryContainsKey<Color>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Rect : DictionaryContainsKey<Rect>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_ContactPoint : 
    DictionaryContainsKey<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_ContactPoint2D : 
    DictionaryContainsKey<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Collision : 
    DictionaryContainsKey<Collision>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Collision2D : 
    DictionaryContainsKey<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_RaycastHit : 
    DictionaryContainsKey<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_RaycastHit2D : 
    DictionaryContainsKey<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Ray : DictionaryContainsKey<Ray>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Space : 
    DictionaryContainsKey<Space>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_Direction : DictionaryContainsKey<Direction>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_ItemDefinition_EquipmentType : 
    DictionaryContainsKey<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_MovementComponent_GoToMethod : 
    DictionaryContainsKey<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_GDPoint_IdlePointPrefix : 
    DictionaryContainsKey<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_LayerMask : 
    DictionaryContainsKey<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_System_Boolean : Flow_WaitObj<bool>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_System_Single : Flow_WaitObj<float>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_System_Int32 : Flow_WaitObj<int>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Vector2 : Flow_WaitObj<Vector2>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Vector3 : Flow_WaitObj<Vector3>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Vector4 : Flow_WaitObj<Vector4>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Quaternion : Flow_WaitObj<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Keyframe : Flow_WaitObj<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Bounds : Flow_WaitObj<Bounds>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Color : Flow_WaitObj<Color>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Rect : Flow_WaitObj<Rect>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_ContactPoint : Flow_WaitObj<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_ContactPoint2D : 
    Flow_WaitObj<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Collision : Flow_WaitObj<Collision>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Collision2D : Flow_WaitObj<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_RaycastHit : Flow_WaitObj<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_RaycastHit2D : Flow_WaitObj<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Ray : Flow_WaitObj<Ray>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_Space : Flow_WaitObj<Space>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_Direction : Flow_WaitObj<Direction>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_ItemDefinition_EquipmentType : 
    Flow_WaitObj<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_MovementComponent_GoToMethod : 
    Flow_WaitObj<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_GDPoint_IdlePointPrefix : 
    Flow_WaitObj<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_Flow_WaitObj_UnityEngine_LayerMask : Flow_WaitObj<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_ForEach_System_Boolean : ForEach<bool>
  {
  }

  public class FlowCanvas_Nodes_ForEach_System_Single : ForEach<float>
  {
  }

  public class FlowCanvas_Nodes_ForEach_System_Int32 : ForEach<int>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Vector2 : ForEach<Vector2>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Vector3 : ForEach<Vector3>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Vector4 : ForEach<Vector4>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Quaternion : ForEach<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Keyframe : ForEach<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Bounds : ForEach<Bounds>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Color : ForEach<Color>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Rect : ForEach<Rect>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_ContactPoint : ForEach<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_ContactPoint2D : ForEach<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Collision : ForEach<Collision>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Collision2D : ForEach<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_RaycastHit : ForEach<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_RaycastHit2D : ForEach<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Ray : ForEach<Ray>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_Space : ForEach<Space>
  {
  }

  public class FlowCanvas_Nodes_ForEach_Direction : ForEach<Direction>
  {
  }

  public class FlowCanvas_Nodes_ForEach_ItemDefinition_EquipmentType : 
    ForEach<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_ForEach_MovementComponent_GoToMethod : 
    ForEach<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_ForEach_GDPoint_IdlePointPrefix : ForEach<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_ForEach_UnityEngine_LayerMask : ForEach<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_System_Boolean : GetDictionaryItem<bool>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_System_Single : GetDictionaryItem<float>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_System_Int32 : GetDictionaryItem<int>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Vector2 : GetDictionaryItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Vector3 : GetDictionaryItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Vector4 : GetDictionaryItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Quaternion : 
    GetDictionaryItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Keyframe : GetDictionaryItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Bounds : GetDictionaryItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Color : GetDictionaryItem<Color>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Rect : GetDictionaryItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_ContactPoint : 
    GetDictionaryItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_ContactPoint2D : 
    GetDictionaryItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Collision : 
    GetDictionaryItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Collision2D : 
    GetDictionaryItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_RaycastHit : 
    GetDictionaryItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_RaycastHit2D : 
    GetDictionaryItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Ray : GetDictionaryItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Space : GetDictionaryItem<Space>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_Direction : GetDictionaryItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_ItemDefinition_EquipmentType : 
    GetDictionaryItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_MovementComponent_GoToMethod : 
    GetDictionaryItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_GDPoint_IdlePointPrefix : 
    GetDictionaryItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_LayerMask : 
    GetDictionaryItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_System_Boolean : GetFirstListItem<bool>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_System_Single : GetFirstListItem<float>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_System_Int32 : GetFirstListItem<int>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Vector2 : GetFirstListItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Vector3 : GetFirstListItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Vector4 : GetFirstListItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Quaternion : 
    GetFirstListItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Keyframe : GetFirstListItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Bounds : GetFirstListItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Color : GetFirstListItem<Color>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Rect : GetFirstListItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_ContactPoint : 
    GetFirstListItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_ContactPoint2D : 
    GetFirstListItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Collision : GetFirstListItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Collision2D : 
    GetFirstListItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_RaycastHit : 
    GetFirstListItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_RaycastHit2D : 
    GetFirstListItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Ray : GetFirstListItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Space : GetFirstListItem<Space>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_Direction : GetFirstListItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_ItemDefinition_EquipmentType : 
    GetFirstListItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_MovementComponent_GoToMethod : 
    GetFirstListItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_GDPoint_IdlePointPrefix : 
    GetFirstListItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_GetFirstListItem_UnityEngine_LayerMask : GetFirstListItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_System_Boolean : GetLastListItem<bool>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_System_Single : GetLastListItem<float>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_System_Int32 : GetLastListItem<int>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Vector2 : GetLastListItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Vector3 : GetLastListItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Vector4 : GetLastListItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Quaternion : GetLastListItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Keyframe : GetLastListItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Bounds : GetLastListItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Color : GetLastListItem<Color>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Rect : GetLastListItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_ContactPoint : 
    GetLastListItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_ContactPoint2D : 
    GetLastListItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Collision : GetLastListItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Collision2D : 
    GetLastListItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_RaycastHit : GetLastListItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_RaycastHit2D : 
    GetLastListItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Ray : GetLastListItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_Space : GetLastListItem<Space>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_Direction : GetLastListItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_ItemDefinition_EquipmentType : 
    GetLastListItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_MovementComponent_GoToMethod : 
    GetLastListItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_GDPoint_IdlePointPrefix : 
    GetLastListItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_GetLastListItem_UnityEngine_LayerMask : GetLastListItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_System_Boolean : GetListItem<bool>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_System_Single : GetListItem<float>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_System_Int32 : GetListItem<int>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Vector2 : GetListItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Vector3 : GetListItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Vector4 : GetListItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Quaternion : GetListItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Keyframe : GetListItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Bounds : GetListItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Color : GetListItem<Color>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Rect : GetListItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_ContactPoint : GetListItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_ContactPoint2D : GetListItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Collision : GetListItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Collision2D : GetListItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_RaycastHit : GetListItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_RaycastHit2D : GetListItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Ray : GetListItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_Space : GetListItem<Space>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_Direction : GetListItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_ItemDefinition_EquipmentType : 
    GetListItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_MovementComponent_GoToMethod : 
    GetListItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_GDPoint_IdlePointPrefix : 
    GetListItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_GetListItem_UnityEngine_LayerMask : GetListItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_System_Boolean : GetOtherVariable<bool>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_System_Single : GetOtherVariable<float>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_System_Int32 : GetOtherVariable<int>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Vector2 : GetOtherVariable<Vector2>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Vector3 : GetOtherVariable<Vector3>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Vector4 : GetOtherVariable<Vector4>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Quaternion : 
    GetOtherVariable<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Keyframe : GetOtherVariable<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Bounds : GetOtherVariable<Bounds>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Color : GetOtherVariable<Color>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Rect : GetOtherVariable<Rect>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_ContactPoint : 
    GetOtherVariable<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_ContactPoint2D : 
    GetOtherVariable<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Collision : GetOtherVariable<Collision>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Collision2D : 
    GetOtherVariable<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_RaycastHit : 
    GetOtherVariable<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_RaycastHit2D : 
    GetOtherVariable<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Ray : GetOtherVariable<Ray>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Space : GetOtherVariable<Space>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_Direction : GetOtherVariable<Direction>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_ItemDefinition_EquipmentType : 
    GetOtherVariable<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_MovementComponent_GoToMethod : 
    GetOtherVariable<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_GDPoint_IdlePointPrefix : 
    GetOtherVariable<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_GetOtherVariable_UnityEngine_LayerMask : GetOtherVariable<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_System_Boolean : GetRandomListItem<bool>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_System_Single : GetRandomListItem<float>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_System_Int32 : GetRandomListItem<int>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Vector2 : GetRandomListItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Vector3 : GetRandomListItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Vector4 : GetRandomListItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Quaternion : 
    GetRandomListItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Keyframe : GetRandomListItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Bounds : GetRandomListItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Color : GetRandomListItem<Color>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Rect : GetRandomListItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_ContactPoint : 
    GetRandomListItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_ContactPoint2D : 
    GetRandomListItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Collision : 
    GetRandomListItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Collision2D : 
    GetRandomListItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_RaycastHit : 
    GetRandomListItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_RaycastHit2D : 
    GetRandomListItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Ray : GetRandomListItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Space : GetRandomListItem<Space>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_Direction : GetRandomListItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_ItemDefinition_EquipmentType : 
    GetRandomListItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_MovementComponent_GoToMethod : 
    GetRandomListItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_GDPoint_IdlePointPrefix : 
    GetRandomListItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_GetRandomListItem_UnityEngine_LayerMask : 
    GetRandomListItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_System_Boolean : GetVariable<bool>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_System_Single : GetVariable<float>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_System_Int32 : GetVariable<int>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Vector2 : GetVariable<Vector2>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Vector3 : GetVariable<Vector3>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Vector4 : GetVariable<Vector4>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Quaternion : GetVariable<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Keyframe : GetVariable<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Bounds : GetVariable<Bounds>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Color : GetVariable<Color>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Rect : GetVariable<Rect>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_ContactPoint : GetVariable<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_ContactPoint2D : GetVariable<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Collision : GetVariable<Collision>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Collision2D : GetVariable<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_RaycastHit : GetVariable<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_RaycastHit2D : GetVariable<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Ray : GetVariable<Ray>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_Space : GetVariable<Space>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_Direction : GetVariable<Direction>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_ItemDefinition_EquipmentType : 
    GetVariable<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_MovementComponent_GoToMethod : 
    GetVariable<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_GDPoint_IdlePointPrefix : 
    GetVariable<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_GetVariable_UnityEngine_LayerMask : GetVariable<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_Identity_System_Boolean : Identity<bool>
  {
  }

  public class FlowCanvas_Nodes_Identity_System_Single : Identity<float>
  {
  }

  public class FlowCanvas_Nodes_Identity_System_Int32 : Identity<int>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Vector2 : Identity<Vector2>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Vector3 : Identity<Vector3>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Vector4 : Identity<Vector4>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Quaternion : Identity<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Keyframe : Identity<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Bounds : Identity<Bounds>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Color : Identity<Color>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Rect : Identity<Rect>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_ContactPoint : Identity<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_ContactPoint2D : Identity<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Collision : Identity<Collision>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Collision2D : Identity<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_RaycastHit : Identity<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_RaycastHit2D : Identity<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Ray : Identity<Ray>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_Space : Identity<Space>
  {
  }

  public class FlowCanvas_Nodes_Identity_Direction : Identity<Direction>
  {
  }

  public class FlowCanvas_Nodes_Identity_ItemDefinition_EquipmentType : 
    Identity<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_Identity_MovementComponent_GoToMethod : 
    Identity<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_Identity_GDPoint_IdlePointPrefix : Identity<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_Identity_UnityEngine_LayerMask : Identity<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_System_Boolean : InsertListItem<bool>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_System_Single : InsertListItem<float>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_System_Int32 : InsertListItem<int>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Vector2 : InsertListItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Vector3 : InsertListItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Vector4 : InsertListItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Quaternion : InsertListItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Keyframe : InsertListItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Bounds : InsertListItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Color : InsertListItem<Color>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Rect : InsertListItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_ContactPoint : 
    InsertListItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_ContactPoint2D : 
    InsertListItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Collision : InsertListItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Collision2D : InsertListItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_RaycastHit : InsertListItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_RaycastHit2D : 
    InsertListItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Ray : InsertListItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_Space : InsertListItem<Space>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_Direction : InsertListItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_ItemDefinition_EquipmentType : 
    InsertListItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_MovementComponent_GoToMethod : 
    InsertListItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_GDPoint_IdlePointPrefix : 
    InsertListItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_InsertListItem_UnityEngine_LayerMask : InsertListItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_System_Boolean : ReadFlowParameter<bool>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_System_Single : ReadFlowParameter<float>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_System_Int32 : ReadFlowParameter<int>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Vector2 : ReadFlowParameter<Vector2>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Vector3 : ReadFlowParameter<Vector3>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Vector4 : ReadFlowParameter<Vector4>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Quaternion : 
    ReadFlowParameter<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Keyframe : ReadFlowParameter<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Bounds : ReadFlowParameter<Bounds>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Color : ReadFlowParameter<Color>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Rect : ReadFlowParameter<Rect>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_ContactPoint : 
    ReadFlowParameter<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_ContactPoint2D : 
    ReadFlowParameter<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Collision : 
    ReadFlowParameter<Collision>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Collision2D : 
    ReadFlowParameter<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_RaycastHit : 
    ReadFlowParameter<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_RaycastHit2D : 
    ReadFlowParameter<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Ray : ReadFlowParameter<Ray>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Space : ReadFlowParameter<Space>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_Direction : ReadFlowParameter<Direction>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_ItemDefinition_EquipmentType : 
    ReadFlowParameter<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_MovementComponent_GoToMethod : 
    ReadFlowParameter<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_GDPoint_IdlePointPrefix : 
    ReadFlowParameter<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_LayerMask : 
    ReadFlowParameter<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_System_Boolean : 
    ReflectedExtractorNodeWrapper<bool>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_System_Single : 
    ReflectedExtractorNodeWrapper<float>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_System_Int32 : 
    ReflectedExtractorNodeWrapper<int>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Vector2 : 
    ReflectedExtractorNodeWrapper<Vector2>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Vector3 : 
    ReflectedExtractorNodeWrapper<Vector3>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Vector4 : 
    ReflectedExtractorNodeWrapper<Vector4>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Quaternion : 
    ReflectedExtractorNodeWrapper<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Keyframe : 
    ReflectedExtractorNodeWrapper<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Bounds : 
    ReflectedExtractorNodeWrapper<Bounds>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Color : 
    ReflectedExtractorNodeWrapper<Color>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Rect : 
    ReflectedExtractorNodeWrapper<Rect>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_ContactPoint : 
    ReflectedExtractorNodeWrapper<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_ContactPoint2D : 
    ReflectedExtractorNodeWrapper<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Collision : 
    ReflectedExtractorNodeWrapper<Collision>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Collision2D : 
    ReflectedExtractorNodeWrapper<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_RaycastHit : 
    ReflectedExtractorNodeWrapper<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_RaycastHit2D : 
    ReflectedExtractorNodeWrapper<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Ray : 
    ReflectedExtractorNodeWrapper<Ray>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Space : 
    ReflectedExtractorNodeWrapper<Space>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_Direction : 
    ReflectedExtractorNodeWrapper<Direction>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_ItemDefinition_EquipmentType : 
    ReflectedExtractorNodeWrapper<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_MovementComponent_GoToMethod : 
    ReflectedExtractorNodeWrapper<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_GDPoint_IdlePointPrefix : 
    ReflectedExtractorNodeWrapper<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_LayerMask : 
    ReflectedExtractorNodeWrapper<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_System_Boolean : RelayValueInput<bool>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_System_Single : RelayValueInput<float>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_System_Int32 : RelayValueInput<int>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Vector2 : RelayValueInput<Vector2>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Vector3 : RelayValueInput<Vector3>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Vector4 : RelayValueInput<Vector4>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Quaternion : RelayValueInput<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Keyframe : RelayValueInput<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Bounds : RelayValueInput<Bounds>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Color : RelayValueInput<Color>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Rect : RelayValueInput<Rect>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_ContactPoint : 
    RelayValueInput<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_ContactPoint2D : 
    RelayValueInput<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Collision : RelayValueInput<Collision>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Collision2D : 
    RelayValueInput<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_RaycastHit : RelayValueInput<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_RaycastHit2D : 
    RelayValueInput<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Ray : RelayValueInput<Ray>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_Space : RelayValueInput<Space>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_Direction : RelayValueInput<Direction>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_ItemDefinition_EquipmentType : 
    RelayValueInput<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_MovementComponent_GoToMethod : 
    RelayValueInput<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_GDPoint_IdlePointPrefix : 
    RelayValueInput<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_RelayValueInput_UnityEngine_LayerMask : RelayValueInput<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_System_Boolean : RelayValueOutput<bool>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_System_Single : RelayValueOutput<float>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_System_Int32 : RelayValueOutput<int>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Vector2 : RelayValueOutput<Vector2>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Vector3 : RelayValueOutput<Vector3>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Vector4 : RelayValueOutput<Vector4>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Quaternion : 
    RelayValueOutput<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Keyframe : RelayValueOutput<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Bounds : RelayValueOutput<Bounds>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Color : RelayValueOutput<Color>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Rect : RelayValueOutput<Rect>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_ContactPoint : 
    RelayValueOutput<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_ContactPoint2D : 
    RelayValueOutput<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Collision : RelayValueOutput<Collision>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Collision2D : 
    RelayValueOutput<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_RaycastHit : 
    RelayValueOutput<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_RaycastHit2D : 
    RelayValueOutput<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Ray : RelayValueOutput<Ray>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Space : RelayValueOutput<Space>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_Direction : RelayValueOutput<Direction>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_ItemDefinition_EquipmentType : 
    RelayValueOutput<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_MovementComponent_GoToMethod : 
    RelayValueOutput<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_GDPoint_IdlePointPrefix : 
    RelayValueOutput<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_RelayValueOutput_UnityEngine_LayerMask : RelayValueOutput<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_System_Boolean : RemoveDictionaryKey<bool>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_System_Single : RemoveDictionaryKey<float>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_System_Int32 : RemoveDictionaryKey<int>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Vector2 : 
    RemoveDictionaryKey<Vector2>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Vector3 : 
    RemoveDictionaryKey<Vector3>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Vector4 : 
    RemoveDictionaryKey<Vector4>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Quaternion : 
    RemoveDictionaryKey<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Keyframe : 
    RemoveDictionaryKey<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Bounds : RemoveDictionaryKey<Bounds>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Color : RemoveDictionaryKey<Color>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Rect : RemoveDictionaryKey<Rect>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_ContactPoint : 
    RemoveDictionaryKey<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_ContactPoint2D : 
    RemoveDictionaryKey<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Collision : 
    RemoveDictionaryKey<Collision>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Collision2D : 
    RemoveDictionaryKey<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_RaycastHit : 
    RemoveDictionaryKey<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_RaycastHit2D : 
    RemoveDictionaryKey<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Ray : RemoveDictionaryKey<Ray>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Space : RemoveDictionaryKey<Space>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_Direction : RemoveDictionaryKey<Direction>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_ItemDefinition_EquipmentType : 
    RemoveDictionaryKey<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_MovementComponent_GoToMethod : 
    RemoveDictionaryKey<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_GDPoint_IdlePointPrefix : 
    RemoveDictionaryKey<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_LayerMask : 
    RemoveDictionaryKey<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_System_Boolean : RemoveListItem<bool>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_System_Single : RemoveListItem<float>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_System_Int32 : RemoveListItem<int>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Vector2 : RemoveListItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Vector3 : RemoveListItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Vector4 : RemoveListItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Quaternion : RemoveListItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Keyframe : RemoveListItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Bounds : RemoveListItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Color : RemoveListItem<Color>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Rect : RemoveListItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_ContactPoint : 
    RemoveListItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_ContactPoint2D : 
    RemoveListItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Collision : RemoveListItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Collision2D : RemoveListItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_RaycastHit : RemoveListItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_RaycastHit2D : 
    RemoveListItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Ray : RemoveListItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_Space : RemoveListItem<Space>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_Direction : RemoveListItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_ItemDefinition_EquipmentType : 
    RemoveListItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_MovementComponent_GoToMethod : 
    RemoveListItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_GDPoint_IdlePointPrefix : 
    RemoveListItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItem_UnityEngine_LayerMask : RemoveListItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_System_Boolean : RemoveListItemAt<bool>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_System_Single : RemoveListItemAt<float>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_System_Int32 : RemoveListItemAt<int>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Vector2 : RemoveListItemAt<Vector2>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Vector3 : RemoveListItemAt<Vector3>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Vector4 : RemoveListItemAt<Vector4>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Quaternion : 
    RemoveListItemAt<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Keyframe : RemoveListItemAt<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Bounds : RemoveListItemAt<Bounds>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Color : RemoveListItemAt<Color>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Rect : RemoveListItemAt<Rect>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_ContactPoint : 
    RemoveListItemAt<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_ContactPoint2D : 
    RemoveListItemAt<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Collision : RemoveListItemAt<Collision>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Collision2D : 
    RemoveListItemAt<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_RaycastHit : 
    RemoveListItemAt<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_RaycastHit2D : 
    RemoveListItemAt<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Ray : RemoveListItemAt<Ray>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Space : RemoveListItemAt<Space>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_Direction : RemoveListItemAt<Direction>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_ItemDefinition_EquipmentType : 
    RemoveListItemAt<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_MovementComponent_GoToMethod : 
    RemoveListItemAt<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_GDPoint_IdlePointPrefix : 
    RemoveListItemAt<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_LayerMask : RemoveListItemAt<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_System_Boolean : FlowCanvas.Nodes.SendEvent<bool>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_System_Single : FlowCanvas.Nodes.SendEvent<float>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_System_Int32 : FlowCanvas.Nodes.SendEvent<int>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Vector2 : FlowCanvas.Nodes.SendEvent<Vector2>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Vector3 : FlowCanvas.Nodes.SendEvent<Vector3>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Vector4 : FlowCanvas.Nodes.SendEvent<Vector4>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Quaternion : FlowCanvas.Nodes.SendEvent<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Keyframe : FlowCanvas.Nodes.SendEvent<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Bounds : FlowCanvas.Nodes.SendEvent<Bounds>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Color : FlowCanvas.Nodes.SendEvent<Color>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Rect : FlowCanvas.Nodes.SendEvent<Rect>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_ContactPoint : FlowCanvas.Nodes.SendEvent<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_ContactPoint2D : FlowCanvas.Nodes.SendEvent<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Collision : FlowCanvas.Nodes.SendEvent<Collision>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Collision2D : FlowCanvas.Nodes.SendEvent<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_RaycastHit : FlowCanvas.Nodes.SendEvent<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_RaycastHit2D : FlowCanvas.Nodes.SendEvent<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Ray : FlowCanvas.Nodes.SendEvent<Ray>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_Space : FlowCanvas.Nodes.SendEvent<Space>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_Direction : FlowCanvas.Nodes.SendEvent<Direction>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_ItemDefinition_EquipmentType : 
    FlowCanvas.Nodes.SendEvent<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_MovementComponent_GoToMethod : 
    FlowCanvas.Nodes.SendEvent<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_GDPoint_IdlePointPrefix : 
    FlowCanvas.Nodes.SendEvent<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_SendEvent_UnityEngine_LayerMask : FlowCanvas.Nodes.SendEvent<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_System_Boolean : SendGlobalEvent<bool>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_System_Single : SendGlobalEvent<float>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_System_Int32 : SendGlobalEvent<int>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Vector2 : SendGlobalEvent<Vector2>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Vector3 : SendGlobalEvent<Vector3>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Vector4 : SendGlobalEvent<Vector4>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Quaternion : SendGlobalEvent<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Keyframe : SendGlobalEvent<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Bounds : SendGlobalEvent<Bounds>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Color : SendGlobalEvent<Color>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Rect : SendGlobalEvent<Rect>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_ContactPoint : 
    SendGlobalEvent<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_ContactPoint2D : 
    SendGlobalEvent<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Collision : SendGlobalEvent<Collision>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Collision2D : 
    SendGlobalEvent<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_RaycastHit : SendGlobalEvent<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_RaycastHit2D : 
    SendGlobalEvent<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Ray : SendGlobalEvent<Ray>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Space : SendGlobalEvent<Space>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_Direction : SendGlobalEvent<Direction>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_ItemDefinition_EquipmentType : 
    SendGlobalEvent<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_MovementComponent_GoToMethod : 
    SendGlobalEvent<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_GDPoint_IdlePointPrefix : 
    SendGlobalEvent<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_LayerMask : SendGlobalEvent<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_System_Boolean : SetListItem<bool>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_System_Single : SetListItem<float>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_System_Int32 : SetListItem<int>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Vector2 : SetListItem<Vector2>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Vector3 : SetListItem<Vector3>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Vector4 : SetListItem<Vector4>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Quaternion : SetListItem<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Keyframe : SetListItem<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Bounds : SetListItem<Bounds>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Color : SetListItem<Color>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Rect : SetListItem<Rect>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_ContactPoint : SetListItem<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_ContactPoint2D : SetListItem<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Collision : SetListItem<Collision>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Collision2D : SetListItem<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_RaycastHit : SetListItem<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_RaycastHit2D : SetListItem<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Ray : SetListItem<Ray>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_Space : SetListItem<Space>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_Direction : SetListItem<Direction>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_ItemDefinition_EquipmentType : 
    SetListItem<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_MovementComponent_GoToMethod : 
    SetListItem<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_GDPoint_IdlePointPrefix : 
    SetListItem<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_SetListItem_UnityEngine_LayerMask : SetListItem<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_System_Boolean : SetOtherVariable<bool>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_System_Single : SetOtherVariable<float>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_System_Int32 : SetOtherVariable<int>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Vector2 : SetOtherVariable<Vector2>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Vector3 : SetOtherVariable<Vector3>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Vector4 : SetOtherVariable<Vector4>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Quaternion : 
    SetOtherVariable<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Keyframe : SetOtherVariable<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Bounds : SetOtherVariable<Bounds>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Color : SetOtherVariable<Color>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Rect : SetOtherVariable<Rect>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_ContactPoint : 
    SetOtherVariable<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_ContactPoint2D : 
    SetOtherVariable<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Collision : SetOtherVariable<Collision>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Collision2D : 
    SetOtherVariable<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_RaycastHit : 
    SetOtherVariable<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_RaycastHit2D : 
    SetOtherVariable<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Ray : SetOtherVariable<Ray>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Space : SetOtherVariable<Space>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_Direction : SetOtherVariable<Direction>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_ItemDefinition_EquipmentType : 
    SetOtherVariable<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_MovementComponent_GoToMethod : 
    SetOtherVariable<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_GDPoint_IdlePointPrefix : 
    SetOtherVariable<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_SetOtherVariable_UnityEngine_LayerMask : SetOtherVariable<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_System_Boolean : FlowCanvas.Nodes.SetVariable<bool>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_System_Single : FlowCanvas.Nodes.SetVariable<float>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_System_Int32 : FlowCanvas.Nodes.SetVariable<int>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Vector2 : FlowCanvas.Nodes.SetVariable<Vector2>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Vector3 : FlowCanvas.Nodes.SetVariable<Vector3>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Vector4 : FlowCanvas.Nodes.SetVariable<Vector4>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Quaternion : FlowCanvas.Nodes.SetVariable<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Keyframe : FlowCanvas.Nodes.SetVariable<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Bounds : FlowCanvas.Nodes.SetVariable<Bounds>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Color : FlowCanvas.Nodes.SetVariable<Color>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Rect : FlowCanvas.Nodes.SetVariable<Rect>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_ContactPoint : FlowCanvas.Nodes.SetVariable<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_ContactPoint2D : FlowCanvas.Nodes.SetVariable<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Collision : FlowCanvas.Nodes.SetVariable<Collision>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Collision2D : FlowCanvas.Nodes.SetVariable<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_RaycastHit : FlowCanvas.Nodes.SetVariable<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_RaycastHit2D : FlowCanvas.Nodes.SetVariable<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Ray : FlowCanvas.Nodes.SetVariable<Ray>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_Space : FlowCanvas.Nodes.SetVariable<Space>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_Direction : FlowCanvas.Nodes.SetVariable<Direction>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_ItemDefinition_EquipmentType : 
    FlowCanvas.Nodes.SetVariable<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_MovementComponent_GoToMethod : 
    FlowCanvas.Nodes.SetVariable<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_GDPoint_IdlePointPrefix : 
    FlowCanvas.Nodes.SetVariable<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_SetVariable_UnityEngine_LayerMask : FlowCanvas.Nodes.SetVariable<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_System_Boolean : ShuffleList<bool>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_System_Single : ShuffleList<float>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_System_Int32 : ShuffleList<int>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Vector2 : ShuffleList<Vector2>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Vector3 : ShuffleList<Vector3>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Vector4 : ShuffleList<Vector4>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Quaternion : ShuffleList<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Keyframe : ShuffleList<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Bounds : ShuffleList<Bounds>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Color : ShuffleList<Color>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Rect : ShuffleList<Rect>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_ContactPoint : ShuffleList<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_ContactPoint2D : ShuffleList<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Collision : ShuffleList<Collision>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Collision2D : ShuffleList<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_RaycastHit : ShuffleList<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_RaycastHit2D : ShuffleList<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Ray : ShuffleList<Ray>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_Space : ShuffleList<Space>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_Direction : ShuffleList<Direction>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_ItemDefinition_EquipmentType : 
    ShuffleList<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_MovementComponent_GoToMethod : 
    ShuffleList<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_GDPoint_IdlePointPrefix : 
    ShuffleList<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_ShuffleList_UnityEngine_LayerMask : ShuffleList<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_System_Boolean : SwitchValue<bool>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_System_Single : SwitchValue<float>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_System_Int32 : SwitchValue<int>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Vector2 : SwitchValue<Vector2>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Vector3 : SwitchValue<Vector3>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Vector4 : SwitchValue<Vector4>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Quaternion : SwitchValue<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Keyframe : SwitchValue<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Bounds : SwitchValue<Bounds>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Color : SwitchValue<Color>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Rect : SwitchValue<Rect>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_ContactPoint : SwitchValue<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_ContactPoint2D : SwitchValue<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Collision : SwitchValue<Collision>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Collision2D : SwitchValue<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_RaycastHit : SwitchValue<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_RaycastHit2D : SwitchValue<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Ray : SwitchValue<Ray>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_Space : SwitchValue<Space>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_Direction : SwitchValue<Direction>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_ItemDefinition_EquipmentType : 
    SwitchValue<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_MovementComponent_GoToMethod : 
    SwitchValue<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_GDPoint_IdlePointPrefix : 
    SwitchValue<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_SwitchValue_UnityEngine_LayerMask : SwitchValue<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_System_Boolean : FlowCanvas.Nodes.TryGetValue<bool>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_System_Single : FlowCanvas.Nodes.TryGetValue<float>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_System_Int32 : FlowCanvas.Nodes.TryGetValue<int>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Vector2 : FlowCanvas.Nodes.TryGetValue<Vector2>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Vector3 : FlowCanvas.Nodes.TryGetValue<Vector3>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Vector4 : FlowCanvas.Nodes.TryGetValue<Vector4>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Quaternion : FlowCanvas.Nodes.TryGetValue<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Keyframe : FlowCanvas.Nodes.TryGetValue<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Bounds : FlowCanvas.Nodes.TryGetValue<Bounds>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Color : FlowCanvas.Nodes.TryGetValue<Color>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Rect : FlowCanvas.Nodes.TryGetValue<Rect>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_ContactPoint : FlowCanvas.Nodes.TryGetValue<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_ContactPoint2D : FlowCanvas.Nodes.TryGetValue<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Collision : FlowCanvas.Nodes.TryGetValue<Collision>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Collision2D : FlowCanvas.Nodes.TryGetValue<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_RaycastHit : FlowCanvas.Nodes.TryGetValue<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_RaycastHit2D : FlowCanvas.Nodes.TryGetValue<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Ray : FlowCanvas.Nodes.TryGetValue<Ray>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_Space : FlowCanvas.Nodes.TryGetValue<Space>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_Direction : FlowCanvas.Nodes.TryGetValue<Direction>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_ItemDefinition_EquipmentType : 
    FlowCanvas.Nodes.TryGetValue<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_MovementComponent_GoToMethod : 
    FlowCanvas.Nodes.TryGetValue<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_GDPoint_IdlePointPrefix : 
    FlowCanvas.Nodes.TryGetValue<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_TryGetValue_UnityEngine_LayerMask : FlowCanvas.Nodes.TryGetValue<LayerMask>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_System_Boolean : WriteFlowParameter<bool>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_System_Single : WriteFlowParameter<float>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_System_Int32 : WriteFlowParameter<int>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Vector2 : WriteFlowParameter<Vector2>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Vector3 : WriteFlowParameter<Vector3>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Vector4 : WriteFlowParameter<Vector4>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Quaternion : 
    WriteFlowParameter<Quaternion>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Keyframe : 
    WriteFlowParameter<Keyframe>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Bounds : WriteFlowParameter<Bounds>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Color : WriteFlowParameter<Color>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Rect : WriteFlowParameter<Rect>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_ContactPoint : 
    WriteFlowParameter<ContactPoint>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_ContactPoint2D : 
    WriteFlowParameter<ContactPoint2D>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Collision : 
    WriteFlowParameter<Collision>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Collision2D : 
    WriteFlowParameter<Collision2D>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_RaycastHit : 
    WriteFlowParameter<RaycastHit>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_RaycastHit2D : 
    WriteFlowParameter<RaycastHit2D>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Ray : WriteFlowParameter<Ray>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Space : WriteFlowParameter<Space>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_Direction : WriteFlowParameter<Direction>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_ItemDefinition_EquipmentType : 
    WriteFlowParameter<ItemDefinition.EquipmentType>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_MovementComponent_GoToMethod : 
    WriteFlowParameter<MovementComponent.GoToMethod>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_GDPoint_IdlePointPrefix : 
    WriteFlowParameter<GDPoint.IdlePointPrefix>
  {
  }

  public class FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_LayerMask : 
    WriteFlowParameter<LayerMask>
  {
  }

  public class NodeCanvas_Framework_BBParameter_System_Boolean : BBParameter<bool>
  {
  }

  public class NodeCanvas_Framework_BBParameter_System_Single : BBParameter<float>
  {
  }

  public class NodeCanvas_Framework_BBParameter_System_Int32 : BBParameter<int>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Vector2 : BBParameter<Vector2>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Vector3 : BBParameter<Vector3>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Vector4 : BBParameter<Vector4>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Quaternion : BBParameter<Quaternion>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Keyframe : BBParameter<Keyframe>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Bounds : BBParameter<Bounds>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Color : BBParameter<Color>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Rect : BBParameter<Rect>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_ContactPoint : BBParameter<ContactPoint>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_ContactPoint2D : 
    BBParameter<ContactPoint2D>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Collision : BBParameter<Collision>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Collision2D : BBParameter<Collision2D>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_RaycastHit : BBParameter<RaycastHit>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_RaycastHit2D : BBParameter<RaycastHit2D>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Ray : BBParameter<Ray>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_Space : BBParameter<Space>
  {
  }

  public class NodeCanvas_Framework_BBParameter_Direction : BBParameter<Direction>
  {
  }

  public class NodeCanvas_Framework_BBParameter_ItemDefinition_EquipmentType : 
    BBParameter<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Framework_BBParameter_MovementComponent_GoToMethod : 
    BBParameter<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Framework_BBParameter_GDPoint_IdlePointPrefix : 
    BBParameter<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Framework_BBParameter_UnityEngine_LayerMask : BBParameter<LayerMask>
  {
  }

  public class NodeCanvas_Framework_Variable_System_Boolean : Variable<bool>
  {
  }

  public class NodeCanvas_Framework_Variable_System_Single : Variable<float>
  {
  }

  public class NodeCanvas_Framework_Variable_System_Int32 : Variable<int>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Vector2 : Variable<Vector2>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Vector3 : Variable<Vector3>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Vector4 : Variable<Vector4>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Quaternion : Variable<Quaternion>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Keyframe : Variable<Keyframe>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Bounds : Variable<Bounds>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Color : Variable<Color>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Rect : Variable<Rect>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_ContactPoint : Variable<ContactPoint>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_ContactPoint2D : Variable<ContactPoint2D>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Collision : Variable<Collision>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Collision2D : Variable<Collision2D>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_RaycastHit : Variable<RaycastHit>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_RaycastHit2D : Variable<RaycastHit2D>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Ray : Variable<Ray>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_Space : Variable<Space>
  {
  }

  public class NodeCanvas_Framework_Variable_Direction : Variable<Direction>
  {
  }

  public class NodeCanvas_Framework_Variable_ItemDefinition_EquipmentType : 
    Variable<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Framework_Variable_MovementComponent_GoToMethod : 
    Variable<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Framework_Variable_GDPoint_IdlePointPrefix : 
    Variable<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Framework_Variable_UnityEngine_LayerMask : Variable<LayerMask>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_System_Boolean : ReflectedAction<bool>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_System_Single : ReflectedAction<float>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_System_Int32 : ReflectedAction<int>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Vector2 : 
    ReflectedAction<Vector2>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Vector3 : 
    ReflectedAction<Vector3>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Vector4 : 
    ReflectedAction<Vector4>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Quaternion : 
    ReflectedAction<Quaternion>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Keyframe : 
    ReflectedAction<Keyframe>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Bounds : 
    ReflectedAction<Bounds>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Color : 
    ReflectedAction<Color>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Rect : ReflectedAction<Rect>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_ContactPoint : 
    ReflectedAction<ContactPoint>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_ContactPoint2D : 
    ReflectedAction<ContactPoint2D>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Collision : 
    ReflectedAction<Collision>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Collision2D : 
    ReflectedAction<Collision2D>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_RaycastHit : 
    ReflectedAction<RaycastHit>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_RaycastHit2D : 
    ReflectedAction<RaycastHit2D>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Ray : ReflectedAction<Ray>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Space : 
    ReflectedAction<Space>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_Direction : ReflectedAction<Direction>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_ItemDefinition_EquipmentType : 
    ReflectedAction<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_MovementComponent_GoToMethod : 
    ReflectedAction<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_GDPoint_IdlePointPrefix : 
    ReflectedAction<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_LayerMask : 
    ReflectedAction<LayerMask>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_System_Boolean : 
    ReflectedFunction<bool>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_System_Single : 
    ReflectedFunction<float>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_System_Int32 : ReflectedFunction<int>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Vector2 : 
    ReflectedFunction<Vector2>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Vector3 : 
    ReflectedFunction<Vector3>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Vector4 : 
    ReflectedFunction<Vector4>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Quaternion : 
    ReflectedFunction<Quaternion>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Keyframe : 
    ReflectedFunction<Keyframe>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Bounds : 
    ReflectedFunction<Bounds>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Color : 
    ReflectedFunction<Color>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Rect : 
    ReflectedFunction<Rect>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_ContactPoint : 
    ReflectedFunction<ContactPoint>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_ContactPoint2D : 
    ReflectedFunction<ContactPoint2D>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Collision : 
    ReflectedFunction<Collision>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Collision2D : 
    ReflectedFunction<Collision2D>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_RaycastHit : 
    ReflectedFunction<RaycastHit>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_RaycastHit2D : 
    ReflectedFunction<RaycastHit2D>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Ray : 
    ReflectedFunction<Ray>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Space : 
    ReflectedFunction<Space>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_Direction : 
    ReflectedFunction<Direction>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_ItemDefinition_EquipmentType : 
    ReflectedFunction<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_MovementComponent_GoToMethod : 
    ReflectedFunction<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_GDPoint_IdlePointPrefix : 
    ReflectedFunction<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_LayerMask : 
    ReflectedFunction<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_System_Boolean : 
    AddElementToDictionary<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_System_Single : 
    AddElementToDictionary<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_System_Int32 : 
    AddElementToDictionary<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Vector2 : 
    AddElementToDictionary<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Vector3 : 
    AddElementToDictionary<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Vector4 : 
    AddElementToDictionary<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Quaternion : 
    AddElementToDictionary<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Keyframe : 
    AddElementToDictionary<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Bounds : 
    AddElementToDictionary<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Color : 
    AddElementToDictionary<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Rect : 
    AddElementToDictionary<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_ContactPoint : 
    AddElementToDictionary<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_ContactPoint2D : 
    AddElementToDictionary<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Collision : 
    AddElementToDictionary<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Collision2D : 
    AddElementToDictionary<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_RaycastHit : 
    AddElementToDictionary<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_RaycastHit2D : 
    AddElementToDictionary<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Ray : 
    AddElementToDictionary<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Space : 
    AddElementToDictionary<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_Direction : 
    AddElementToDictionary<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_ItemDefinition_EquipmentType : 
    AddElementToDictionary<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_MovementComponent_GoToMethod : 
    AddElementToDictionary<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_GDPoint_IdlePointPrefix : 
    AddElementToDictionary<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_LayerMask : 
    AddElementToDictionary<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_System_Boolean : AddElementToList<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_System_Single : AddElementToList<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_System_Int32 : AddElementToList<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Vector2 : 
    AddElementToList<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Vector3 : 
    AddElementToList<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Vector4 : 
    AddElementToList<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Quaternion : 
    AddElementToList<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Keyframe : 
    AddElementToList<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Bounds : 
    AddElementToList<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Color : AddElementToList<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Rect : AddElementToList<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_ContactPoint : 
    AddElementToList<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_ContactPoint2D : 
    AddElementToList<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Collision : 
    AddElementToList<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Collision2D : 
    AddElementToList<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_RaycastHit : 
    AddElementToList<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_RaycastHit2D : 
    AddElementToList<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Ray : AddElementToList<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Space : AddElementToList<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_Direction : AddElementToList<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_ItemDefinition_EquipmentType : 
    AddElementToList<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_MovementComponent_GoToMethod : 
    AddElementToList<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_GDPoint_IdlePointPrefix : 
    AddElementToList<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_LayerMask : 
    AddElementToList<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_System_Boolean : 
    GetDictionaryElement<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_System_Single : 
    GetDictionaryElement<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_System_Int32 : GetDictionaryElement<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Vector2 : 
    GetDictionaryElement<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Vector3 : 
    GetDictionaryElement<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Vector4 : 
    GetDictionaryElement<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Quaternion : 
    GetDictionaryElement<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Keyframe : 
    GetDictionaryElement<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Bounds : 
    GetDictionaryElement<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Color : 
    GetDictionaryElement<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Rect : 
    GetDictionaryElement<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_ContactPoint : 
    GetDictionaryElement<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_ContactPoint2D : 
    GetDictionaryElement<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Collision : 
    GetDictionaryElement<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Collision2D : 
    GetDictionaryElement<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_RaycastHit : 
    GetDictionaryElement<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_RaycastHit2D : 
    GetDictionaryElement<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Ray : 
    GetDictionaryElement<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Space : 
    GetDictionaryElement<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_Direction : 
    GetDictionaryElement<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_ItemDefinition_EquipmentType : 
    GetDictionaryElement<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_MovementComponent_GoToMethod : 
    GetDictionaryElement<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_GDPoint_IdlePointPrefix : 
    GetDictionaryElement<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_LayerMask : 
    GetDictionaryElement<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_System_Boolean : GetIndexOfElement<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_System_Single : GetIndexOfElement<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_System_Int32 : GetIndexOfElement<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Vector2 : 
    GetIndexOfElement<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Vector3 : 
    GetIndexOfElement<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Vector4 : 
    GetIndexOfElement<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Quaternion : 
    GetIndexOfElement<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Keyframe : 
    GetIndexOfElement<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Bounds : 
    GetIndexOfElement<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Color : 
    GetIndexOfElement<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Rect : GetIndexOfElement<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_ContactPoint : 
    GetIndexOfElement<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_ContactPoint2D : 
    GetIndexOfElement<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Collision : 
    GetIndexOfElement<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Collision2D : 
    GetIndexOfElement<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_RaycastHit : 
    GetIndexOfElement<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_RaycastHit2D : 
    GetIndexOfElement<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Ray : GetIndexOfElement<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Space : 
    GetIndexOfElement<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_Direction : GetIndexOfElement<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_ItemDefinition_EquipmentType : 
    GetIndexOfElement<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_MovementComponent_GoToMethod : 
    GetIndexOfElement<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_GDPoint_IdlePointPrefix : 
    GetIndexOfElement<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_LayerMask : 
    GetIndexOfElement<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_System_Boolean : 
    InsertElementToList<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_System_Single : 
    InsertElementToList<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_System_Int32 : InsertElementToList<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Vector2 : 
    InsertElementToList<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Vector3 : 
    InsertElementToList<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Vector4 : 
    InsertElementToList<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Quaternion : 
    InsertElementToList<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Keyframe : 
    InsertElementToList<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Bounds : 
    InsertElementToList<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Color : 
    InsertElementToList<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Rect : 
    InsertElementToList<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_ContactPoint : 
    InsertElementToList<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_ContactPoint2D : 
    InsertElementToList<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Collision : 
    InsertElementToList<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Collision2D : 
    InsertElementToList<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_RaycastHit : 
    InsertElementToList<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_RaycastHit2D : 
    InsertElementToList<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Ray : 
    InsertElementToList<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Space : 
    InsertElementToList<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_Direction : 
    InsertElementToList<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_ItemDefinition_EquipmentType : 
    InsertElementToList<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_MovementComponent_GoToMethod : 
    InsertElementToList<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_GDPoint_IdlePointPrefix : 
    InsertElementToList<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_LayerMask : 
    InsertElementToList<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_System_Boolean : PickListElement<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_System_Single : PickListElement<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_System_Int32 : PickListElement<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Vector2 : 
    PickListElement<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Vector3 : 
    PickListElement<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Vector4 : 
    PickListElement<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Quaternion : 
    PickListElement<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Keyframe : 
    PickListElement<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Bounds : PickListElement<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Color : PickListElement<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Rect : PickListElement<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_ContactPoint : 
    PickListElement<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_ContactPoint2D : 
    PickListElement<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Collision : 
    PickListElement<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Collision2D : 
    PickListElement<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_RaycastHit : 
    PickListElement<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_RaycastHit2D : 
    PickListElement<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Ray : PickListElement<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Space : PickListElement<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_Direction : PickListElement<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_ItemDefinition_EquipmentType : 
    PickListElement<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_MovementComponent_GoToMethod : 
    PickListElement<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_GDPoint_IdlePointPrefix : 
    PickListElement<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_LayerMask : 
    PickListElement<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_System_Boolean : 
    PickRandomListElement<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_System_Single : 
    PickRandomListElement<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_System_Int32 : 
    PickRandomListElement<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Vector2 : 
    PickRandomListElement<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Vector3 : 
    PickRandomListElement<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Vector4 : 
    PickRandomListElement<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Quaternion : 
    PickRandomListElement<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Keyframe : 
    PickRandomListElement<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Bounds : 
    PickRandomListElement<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Color : 
    PickRandomListElement<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Rect : 
    PickRandomListElement<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_ContactPoint : 
    PickRandomListElement<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_ContactPoint2D : 
    PickRandomListElement<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Collision : 
    PickRandomListElement<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Collision2D : 
    PickRandomListElement<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_RaycastHit : 
    PickRandomListElement<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_RaycastHit2D : 
    PickRandomListElement<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Ray : 
    PickRandomListElement<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Space : 
    PickRandomListElement<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_Direction : 
    PickRandomListElement<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_ItemDefinition_EquipmentType : 
    PickRandomListElement<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_MovementComponent_GoToMethod : 
    PickRandomListElement<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_GDPoint_IdlePointPrefix : 
    PickRandomListElement<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_LayerMask : 
    PickRandomListElement<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_System_Boolean : 
    RemoveElementFromList<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_System_Single : 
    RemoveElementFromList<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_System_Int32 : 
    RemoveElementFromList<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Vector2 : 
    RemoveElementFromList<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Vector3 : 
    RemoveElementFromList<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Vector4 : 
    RemoveElementFromList<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Quaternion : 
    RemoveElementFromList<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Keyframe : 
    RemoveElementFromList<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Bounds : 
    RemoveElementFromList<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Color : 
    RemoveElementFromList<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Rect : 
    RemoveElementFromList<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_ContactPoint : 
    RemoveElementFromList<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_ContactPoint2D : 
    RemoveElementFromList<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Collision : 
    RemoveElementFromList<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Collision2D : 
    RemoveElementFromList<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_RaycastHit : 
    RemoveElementFromList<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_RaycastHit2D : 
    RemoveElementFromList<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Ray : 
    RemoveElementFromList<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Space : 
    RemoveElementFromList<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_Direction : 
    RemoveElementFromList<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_ItemDefinition_EquipmentType : 
    RemoveElementFromList<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_MovementComponent_GoToMethod : 
    RemoveElementFromList<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_GDPoint_IdlePointPrefix : 
    RemoveElementFromList<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_LayerMask : 
    RemoveElementFromList<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_System_Boolean : NodeCanvas.Tasks.Actions.SendEvent<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_System_Single : NodeCanvas.Tasks.Actions.SendEvent<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_System_Int32 : NodeCanvas.Tasks.Actions.SendEvent<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Vector2 : NodeCanvas.Tasks.Actions.SendEvent<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Vector3 : NodeCanvas.Tasks.Actions.SendEvent<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Vector4 : NodeCanvas.Tasks.Actions.SendEvent<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Quaternion : NodeCanvas.Tasks.Actions.SendEvent<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Keyframe : NodeCanvas.Tasks.Actions.SendEvent<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Bounds : NodeCanvas.Tasks.Actions.SendEvent<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Color : NodeCanvas.Tasks.Actions.SendEvent<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Rect : NodeCanvas.Tasks.Actions.SendEvent<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_ContactPoint : NodeCanvas.Tasks.Actions.SendEvent<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_ContactPoint2D : 
    NodeCanvas.Tasks.Actions.SendEvent<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Collision : NodeCanvas.Tasks.Actions.SendEvent<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Collision2D : NodeCanvas.Tasks.Actions.SendEvent<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_RaycastHit : NodeCanvas.Tasks.Actions.SendEvent<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_RaycastHit2D : NodeCanvas.Tasks.Actions.SendEvent<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Ray : NodeCanvas.Tasks.Actions.SendEvent<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Space : NodeCanvas.Tasks.Actions.SendEvent<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_Direction : NodeCanvas.Tasks.Actions.SendEvent<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_ItemDefinition_EquipmentType : 
    NodeCanvas.Tasks.Actions.SendEvent<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_MovementComponent_GoToMethod : 
    NodeCanvas.Tasks.Actions.SendEvent<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_GDPoint_IdlePointPrefix : 
    NodeCanvas.Tasks.Actions.SendEvent<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_LayerMask : NodeCanvas.Tasks.Actions.SendEvent<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_System_Boolean : SendEventToObjects<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_System_Single : SendEventToObjects<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_System_Int32 : SendEventToObjects<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Vector2 : 
    SendEventToObjects<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Vector3 : 
    SendEventToObjects<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Vector4 : 
    SendEventToObjects<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Quaternion : 
    SendEventToObjects<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Keyframe : 
    SendEventToObjects<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Bounds : 
    SendEventToObjects<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Color : 
    SendEventToObjects<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Rect : 
    SendEventToObjects<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_ContactPoint : 
    SendEventToObjects<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_ContactPoint2D : 
    SendEventToObjects<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Collision : 
    SendEventToObjects<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Collision2D : 
    SendEventToObjects<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_RaycastHit : 
    SendEventToObjects<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_RaycastHit2D : 
    SendEventToObjects<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Ray : SendEventToObjects<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Space : 
    SendEventToObjects<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_Direction : SendEventToObjects<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_ItemDefinition_EquipmentType : 
    SendEventToObjects<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_MovementComponent_GoToMethod : 
    SendEventToObjects<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_GDPoint_IdlePointPrefix : 
    SendEventToObjects<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_LayerMask : 
    SendEventToObjects<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_System_Boolean : SendMessage<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_System_Single : SendMessage<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_System_Int32 : SendMessage<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Vector2 : SendMessage<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Vector3 : SendMessage<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Vector4 : SendMessage<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Quaternion : SendMessage<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Keyframe : SendMessage<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Bounds : SendMessage<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Color : SendMessage<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Rect : SendMessage<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_ContactPoint : 
    SendMessage<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_ContactPoint2D : 
    SendMessage<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Collision : SendMessage<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Collision2D : 
    SendMessage<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_RaycastHit : SendMessage<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_RaycastHit2D : 
    SendMessage<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Ray : SendMessage<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Space : SendMessage<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_Direction : SendMessage<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_ItemDefinition_EquipmentType : 
    SendMessage<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_MovementComponent_GoToMethod : 
    SendMessage<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_GDPoint_IdlePointPrefix : 
    SendMessage<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_LayerMask : SendMessage<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_System_Boolean : SetListElement<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_System_Single : SetListElement<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_System_Int32 : SetListElement<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Vector2 : SetListElement<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Vector3 : SetListElement<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Vector4 : SetListElement<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Quaternion : 
    SetListElement<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Keyframe : 
    SetListElement<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Bounds : SetListElement<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Color : SetListElement<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Rect : SetListElement<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_ContactPoint : 
    SetListElement<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_ContactPoint2D : 
    SetListElement<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Collision : 
    SetListElement<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Collision2D : 
    SetListElement<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_RaycastHit : 
    SetListElement<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_RaycastHit2D : 
    SetListElement<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Ray : SetListElement<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Space : SetListElement<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_Direction : SetListElement<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_ItemDefinition_EquipmentType : 
    SetListElement<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_MovementComponent_GoToMethod : 
    SetListElement<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_GDPoint_IdlePointPrefix : 
    SetListElement<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_LayerMask : 
    SetListElement<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_System_Boolean : NodeCanvas.Tasks.Actions.SetVariable<bool>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_System_Single : NodeCanvas.Tasks.Actions.SetVariable<float>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_System_Int32 : NodeCanvas.Tasks.Actions.SetVariable<int>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Vector2 : NodeCanvas.Tasks.Actions.SetVariable<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Vector3 : NodeCanvas.Tasks.Actions.SetVariable<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Vector4 : NodeCanvas.Tasks.Actions.SetVariable<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Quaternion : NodeCanvas.Tasks.Actions.SetVariable<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Keyframe : NodeCanvas.Tasks.Actions.SetVariable<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Bounds : NodeCanvas.Tasks.Actions.SetVariable<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Color : NodeCanvas.Tasks.Actions.SetVariable<Color>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Rect : NodeCanvas.Tasks.Actions.SetVariable<Rect>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_ContactPoint : 
    NodeCanvas.Tasks.Actions.SetVariable<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_ContactPoint2D : 
    NodeCanvas.Tasks.Actions.SetVariable<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Collision : NodeCanvas.Tasks.Actions.SetVariable<Collision>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Collision2D : 
    NodeCanvas.Tasks.Actions.SetVariable<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_RaycastHit : NodeCanvas.Tasks.Actions.SetVariable<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_RaycastHit2D : 
    NodeCanvas.Tasks.Actions.SetVariable<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Ray : NodeCanvas.Tasks.Actions.SetVariable<Ray>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Space : NodeCanvas.Tasks.Actions.SetVariable<Space>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_Direction : NodeCanvas.Tasks.Actions.SetVariable<Direction>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_ItemDefinition_EquipmentType : 
    NodeCanvas.Tasks.Actions.SetVariable<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_MovementComponent_GoToMethod : 
    NodeCanvas.Tasks.Actions.SetVariable<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_GDPoint_IdlePointPrefix : 
    NodeCanvas.Tasks.Actions.SetVariable<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_LayerMask : NodeCanvas.Tasks.Actions.SetVariable<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_System_Boolean : CheckCSharpEvent<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_System_Single : CheckCSharpEvent<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_System_Int32 : CheckCSharpEvent<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Vector2 : 
    CheckCSharpEvent<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Vector3 : 
    CheckCSharpEvent<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Vector4 : 
    CheckCSharpEvent<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Quaternion : 
    CheckCSharpEvent<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Keyframe : 
    CheckCSharpEvent<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Bounds : 
    CheckCSharpEvent<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Color : 
    CheckCSharpEvent<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Rect : CheckCSharpEvent<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_ContactPoint : 
    CheckCSharpEvent<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_ContactPoint2D : 
    CheckCSharpEvent<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Collision : 
    CheckCSharpEvent<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Collision2D : 
    CheckCSharpEvent<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_RaycastHit : 
    CheckCSharpEvent<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_RaycastHit2D : 
    CheckCSharpEvent<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Ray : CheckCSharpEvent<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Space : 
    CheckCSharpEvent<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_Direction : CheckCSharpEvent<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_ItemDefinition_EquipmentType : 
    CheckCSharpEvent<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_MovementComponent_GoToMethod : 
    CheckCSharpEvent<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_GDPoint_IdlePointPrefix : 
    CheckCSharpEvent<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_LayerMask : 
    CheckCSharpEvent<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_System_Boolean : 
    CheckCSharpEventValue<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_System_Single : 
    CheckCSharpEventValue<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_System_Int32 : 
    CheckCSharpEventValue<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Vector2 : 
    CheckCSharpEventValue<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Vector3 : 
    CheckCSharpEventValue<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Vector4 : 
    CheckCSharpEventValue<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Quaternion : 
    CheckCSharpEventValue<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Keyframe : 
    CheckCSharpEventValue<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Bounds : 
    CheckCSharpEventValue<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Color : 
    CheckCSharpEventValue<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Rect : 
    CheckCSharpEventValue<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_ContactPoint : 
    CheckCSharpEventValue<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_ContactPoint2D : 
    CheckCSharpEventValue<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Collision : 
    CheckCSharpEventValue<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Collision2D : 
    CheckCSharpEventValue<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_RaycastHit : 
    CheckCSharpEventValue<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_RaycastHit2D : 
    CheckCSharpEventValue<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Ray : 
    CheckCSharpEventValue<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Space : 
    CheckCSharpEventValue<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_Direction : 
    CheckCSharpEventValue<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_ItemDefinition_EquipmentType : 
    CheckCSharpEventValue<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_MovementComponent_GoToMethod : 
    CheckCSharpEventValue<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_GDPoint_IdlePointPrefix : 
    CheckCSharpEventValue<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_LayerMask : 
    CheckCSharpEventValue<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_System_Boolean : CheckEvent<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_System_Single : CheckEvent<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_System_Int32 : CheckEvent<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Vector2 : CheckEvent<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Vector3 : CheckEvent<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Vector4 : CheckEvent<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Quaternion : CheckEvent<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Keyframe : CheckEvent<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Bounds : CheckEvent<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Color : CheckEvent<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Rect : CheckEvent<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_ContactPoint : 
    CheckEvent<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_ContactPoint2D : 
    CheckEvent<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Collision : CheckEvent<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Collision2D : 
    CheckEvent<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_RaycastHit : CheckEvent<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_RaycastHit2D : 
    CheckEvent<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Ray : CheckEvent<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Space : CheckEvent<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_Direction : CheckEvent<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_ItemDefinition_EquipmentType : 
    CheckEvent<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_MovementComponent_GoToMethod : 
    CheckEvent<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_GDPoint_IdlePointPrefix : 
    CheckEvent<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_LayerMask : CheckEvent<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_System_Boolean : CheckEventValue<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_System_Single : CheckEventValue<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_System_Int32 : CheckEventValue<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Vector2 : 
    CheckEventValue<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Vector3 : 
    CheckEventValue<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Vector4 : 
    CheckEventValue<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Quaternion : 
    CheckEventValue<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Keyframe : 
    CheckEventValue<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Bounds : 
    CheckEventValue<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Color : CheckEventValue<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Rect : CheckEventValue<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_ContactPoint : 
    CheckEventValue<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_ContactPoint2D : 
    CheckEventValue<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Collision : 
    CheckEventValue<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Collision2D : 
    CheckEventValue<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_RaycastHit : 
    CheckEventValue<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_RaycastHit2D : 
    CheckEventValue<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Ray : CheckEventValue<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Space : CheckEventValue<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_Direction : CheckEventValue<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_ItemDefinition_EquipmentType : 
    CheckEventValue<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_MovementComponent_GoToMethod : 
    CheckEventValue<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_GDPoint_IdlePointPrefix : 
    CheckEventValue<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_LayerMask : 
    CheckEventValue<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_System_Boolean : 
    CheckStaticCSharpEvent<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_System_Single : 
    CheckStaticCSharpEvent<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_System_Int32 : 
    CheckStaticCSharpEvent<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Vector2 : 
    CheckStaticCSharpEvent<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Vector3 : 
    CheckStaticCSharpEvent<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Vector4 : 
    CheckStaticCSharpEvent<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Quaternion : 
    CheckStaticCSharpEvent<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Keyframe : 
    CheckStaticCSharpEvent<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Bounds : 
    CheckStaticCSharpEvent<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Color : 
    CheckStaticCSharpEvent<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Rect : 
    CheckStaticCSharpEvent<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_ContactPoint : 
    CheckStaticCSharpEvent<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_ContactPoint2D : 
    CheckStaticCSharpEvent<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Collision : 
    CheckStaticCSharpEvent<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Collision2D : 
    CheckStaticCSharpEvent<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_RaycastHit : 
    CheckStaticCSharpEvent<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_RaycastHit2D : 
    CheckStaticCSharpEvent<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Ray : 
    CheckStaticCSharpEvent<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_Space : 
    CheckStaticCSharpEvent<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_Direction : 
    CheckStaticCSharpEvent<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_ItemDefinition_EquipmentType : 
    CheckStaticCSharpEvent<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_MovementComponent_GoToMethod : 
    CheckStaticCSharpEvent<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_GDPoint_IdlePointPrefix : 
    CheckStaticCSharpEvent<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckStaticCSharpEvent_UnityEngine_LayerMask : 
    CheckStaticCSharpEvent<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_System_Boolean : CheckUnityEvent<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_System_Single : CheckUnityEvent<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_System_Int32 : CheckUnityEvent<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Vector2 : 
    CheckUnityEvent<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Vector3 : 
    CheckUnityEvent<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Vector4 : 
    CheckUnityEvent<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Quaternion : 
    CheckUnityEvent<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Keyframe : 
    CheckUnityEvent<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Bounds : 
    CheckUnityEvent<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Color : CheckUnityEvent<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Rect : CheckUnityEvent<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_ContactPoint : 
    CheckUnityEvent<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_ContactPoint2D : 
    CheckUnityEvent<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Collision : 
    CheckUnityEvent<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Collision2D : 
    CheckUnityEvent<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_RaycastHit : 
    CheckUnityEvent<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_RaycastHit2D : 
    CheckUnityEvent<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Ray : CheckUnityEvent<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Space : CheckUnityEvent<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_Direction : CheckUnityEvent<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_ItemDefinition_EquipmentType : 
    CheckUnityEvent<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_MovementComponent_GoToMethod : 
    CheckUnityEvent<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_GDPoint_IdlePointPrefix : 
    CheckUnityEvent<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_LayerMask : 
    CheckUnityEvent<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_System_Boolean : 
    CheckUnityEventValue<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_System_Single : 
    CheckUnityEventValue<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_System_Int32 : 
    CheckUnityEventValue<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Vector2 : 
    CheckUnityEventValue<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Vector3 : 
    CheckUnityEventValue<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Vector4 : 
    CheckUnityEventValue<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Quaternion : 
    CheckUnityEventValue<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Keyframe : 
    CheckUnityEventValue<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Bounds : 
    CheckUnityEventValue<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Color : 
    CheckUnityEventValue<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Rect : 
    CheckUnityEventValue<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_ContactPoint : 
    CheckUnityEventValue<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_ContactPoint2D : 
    CheckUnityEventValue<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Collision : 
    CheckUnityEventValue<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Collision2D : 
    CheckUnityEventValue<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_RaycastHit : 
    CheckUnityEventValue<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_RaycastHit2D : 
    CheckUnityEventValue<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Ray : 
    CheckUnityEventValue<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Space : 
    CheckUnityEventValue<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_Direction : 
    CheckUnityEventValue<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_ItemDefinition_EquipmentType : 
    CheckUnityEventValue<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_MovementComponent_GoToMethod : 
    CheckUnityEventValue<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_GDPoint_IdlePointPrefix : 
    CheckUnityEventValue<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_LayerMask : 
    CheckUnityEventValue<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_System_Boolean : CheckVariable<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_System_Single : CheckVariable<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_System_Int32 : CheckVariable<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Vector2 : CheckVariable<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Vector3 : CheckVariable<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Vector4 : CheckVariable<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Quaternion : 
    CheckVariable<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Keyframe : 
    CheckVariable<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Bounds : CheckVariable<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Color : CheckVariable<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Rect : CheckVariable<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_ContactPoint : 
    CheckVariable<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_ContactPoint2D : 
    CheckVariable<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Collision : 
    CheckVariable<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Collision2D : 
    CheckVariable<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_RaycastHit : 
    CheckVariable<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_RaycastHit2D : 
    CheckVariable<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Ray : CheckVariable<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Space : CheckVariable<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_Direction : CheckVariable<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_ItemDefinition_EquipmentType : 
    CheckVariable<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_MovementComponent_GoToMethod : 
    CheckVariable<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_GDPoint_IdlePointPrefix : 
    CheckVariable<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_LayerMask : 
    CheckVariable<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_System_Boolean : 
    ListContainsElement<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_System_Single : 
    ListContainsElement<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_System_Int32 : 
    ListContainsElement<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Vector2 : 
    ListContainsElement<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Vector3 : 
    ListContainsElement<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Vector4 : 
    ListContainsElement<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Quaternion : 
    ListContainsElement<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Keyframe : 
    ListContainsElement<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Bounds : 
    ListContainsElement<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Color : 
    ListContainsElement<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Rect : 
    ListContainsElement<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_ContactPoint : 
    ListContainsElement<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_ContactPoint2D : 
    ListContainsElement<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Collision : 
    ListContainsElement<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Collision2D : 
    ListContainsElement<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_RaycastHit : 
    ListContainsElement<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_RaycastHit2D : 
    ListContainsElement<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Ray : 
    ListContainsElement<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Space : 
    ListContainsElement<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_Direction : 
    ListContainsElement<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_ItemDefinition_EquipmentType : 
    ListContainsElement<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_MovementComponent_GoToMethod : 
    ListContainsElement<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_GDPoint_IdlePointPrefix : 
    ListContainsElement<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_LayerMask : 
    ListContainsElement<LayerMask>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_System_Boolean : NodeCanvas.Tasks.Conditions.TryGetValue<bool>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_System_Single : NodeCanvas.Tasks.Conditions.TryGetValue<float>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_System_Int32 : NodeCanvas.Tasks.Conditions.TryGetValue<int>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Vector2 : NodeCanvas.Tasks.Conditions.TryGetValue<Vector2>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Vector3 : NodeCanvas.Tasks.Conditions.TryGetValue<Vector3>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Vector4 : NodeCanvas.Tasks.Conditions.TryGetValue<Vector4>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Quaternion : 
    NodeCanvas.Tasks.Conditions.TryGetValue<Quaternion>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Keyframe : NodeCanvas.Tasks.Conditions.TryGetValue<Keyframe>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Bounds : NodeCanvas.Tasks.Conditions.TryGetValue<Bounds>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Color : NodeCanvas.Tasks.Conditions.TryGetValue<Color>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Rect : NodeCanvas.Tasks.Conditions.TryGetValue<Rect>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_ContactPoint : 
    NodeCanvas.Tasks.Conditions.TryGetValue<ContactPoint>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_ContactPoint2D : 
    NodeCanvas.Tasks.Conditions.TryGetValue<ContactPoint2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Collision : NodeCanvas.Tasks.Conditions.TryGetValue<Collision>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Collision2D : 
    NodeCanvas.Tasks.Conditions.TryGetValue<Collision2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_RaycastHit : 
    NodeCanvas.Tasks.Conditions.TryGetValue<RaycastHit>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_RaycastHit2D : 
    NodeCanvas.Tasks.Conditions.TryGetValue<RaycastHit2D>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Ray : NodeCanvas.Tasks.Conditions.TryGetValue<Ray>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Space : NodeCanvas.Tasks.Conditions.TryGetValue<Space>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_Direction : NodeCanvas.Tasks.Conditions.TryGetValue<Direction>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_ItemDefinition_EquipmentType : 
    NodeCanvas.Tasks.Conditions.TryGetValue<ItemDefinition.EquipmentType>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_MovementComponent_GoToMethod : 
    NodeCanvas.Tasks.Conditions.TryGetValue<MovementComponent.GoToMethod>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_GDPoint_IdlePointPrefix : 
    NodeCanvas.Tasks.Conditions.TryGetValue<GDPoint.IdlePointPrefix>
  {
  }

  public class NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_LayerMask : NodeCanvas.Tasks.Conditions.TryGetValue<LayerMask>
  {
  }
}
