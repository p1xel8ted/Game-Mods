// Decompiled with JetBrains decompiler
// Type: Data.Serialization.MPSerialization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using System.Collections.Generic;

#nullable disable
namespace Data.Serialization;

public class MPSerialization
{
  public static MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard.WithResolver(CompositeResolver.Create((IReadOnlyList<IMessagePackFormatter>) new IMessagePackFormatter[16 /*0x10*/]
  {
    (IMessagePackFormatter) new Vector2Formatter(),
    (IMessagePackFormatter) new Vector2IntFormatter(),
    (IMessagePackFormatter) new Vector3Formatter(),
    (IMessagePackFormatter) new Vector3IntFormatter(),
    (IMessagePackFormatter) new Vector4Formatter(),
    (IMessagePackFormatter) new QuaternionFormatter(),
    (IMessagePackFormatter) new ColorFormatter(),
    (IMessagePackFormatter) new Color32Formatter(),
    (IMessagePackFormatter) new BoundsFormatter(),
    (IMessagePackFormatter) new RectFormatter(),
    (IMessagePackFormatter) new Matrix4x4Formatter(),
    (IMessagePackFormatter) new StoryObjectiveDataFormatter(),
    (IMessagePackFormatter) new EnemyDataFormatter(),
    (IMessagePackFormatter) new DungeonCompletedFleecesFormatter(),
    (IMessagePackFormatter) new RanchableAnimalFormatter(),
    (IMessagePackFormatter) new FinalizedNotificationPolymorphicFormatter()
  }, (IReadOnlyList<IFormatterResolver>) new IFormatterResolver[1]
  {
    (IFormatterResolver) StandardResolver.Instance
  })).WithCompression(MessagePackCompression.Lz4BlockArray);
}
