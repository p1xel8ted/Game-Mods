// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.Sampled.Agent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Pathfinding.RVO.Sampled;

public class Agent : IAgent
{
  public Vector3 smoothPos;
  [CompilerGenerated]
  public Vector3 \u003CPosition\u003Ek__BackingField;
  [CompilerGenerated]
  public Vector3 \u003CDesiredVelocity\u003Ek__BackingField;
  public float radius;
  public float height;
  public float maxSpeed;
  public float neighbourDist;
  public float agentTimeHorizon;
  public float obstacleTimeHorizon;
  public float weight;
  public bool locked;
  public RVOLayer layer;
  public RVOLayer collidesWith;
  public int maxNeighbours;
  public Vector3 position;
  public Vector3 desiredVelocity;
  public Vector3 prevSmoothPos;
  [CompilerGenerated]
  public RVOLayer \u003CLayer\u003Ek__BackingField;
  [CompilerGenerated]
  public RVOLayer \u003CCollidesWith\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CLocked\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CRadius\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CHeight\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CMaxSpeed\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CNeighbourDist\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CAgentTimeHorizon\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CObstacleTimeHorizon\u003Ek__BackingField;
  [CompilerGenerated]
  public Vector3 \u003CVelocity\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CDebugDraw\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CMaxNeighbours\u003Ek__BackingField;
  public Agent next;
  public Vector3 velocity;
  public Vector3 newVelocity;
  public Simulator simulator;
  public List<Agent> neighbours = new List<Agent>();
  public List<float> neighbourDists = new List<float>();
  public List<ObstacleVertex> obstaclesBuffered = new List<ObstacleVertex>();
  public List<ObstacleVertex> obstacles = new List<ObstacleVertex>();
  public List<float> obstacleDists = new List<float>();
  public static Stopwatch watch1 = new Stopwatch();
  public static Stopwatch watch2 = new Stopwatch();
  public static float DesiredVelocityWeight = 0.02f;
  public static float DesiredVelocityScale = 0.1f;
  public static float GlobalIncompressibility = 30f;
  public const float WallWeight = 5f;

  public Vector3 Position
  {
    get => this.\u003CPosition\u003Ek__BackingField;
    set => this.\u003CPosition\u003Ek__BackingField = value;
  }

  public Vector3 InterpolatedPosition => this.smoothPos;

  public Vector3 DesiredVelocity
  {
    get => this.\u003CDesiredVelocity\u003Ek__BackingField;
    set => this.\u003CDesiredVelocity\u003Ek__BackingField = value;
  }

  public void Teleport(Vector3 pos)
  {
    this.Position = pos;
    this.smoothPos = pos;
    this.prevSmoothPos = pos;
  }

  public void SetYPosition(float yCoordinate)
  {
    this.Position = new Vector3(this.Position.x, yCoordinate, this.Position.z);
    this.smoothPos.y = yCoordinate;
    this.prevSmoothPos.y = yCoordinate;
  }

  public RVOLayer Layer
  {
    get => this.\u003CLayer\u003Ek__BackingField;
    set => this.\u003CLayer\u003Ek__BackingField = value;
  }

  public RVOLayer CollidesWith
  {
    get => this.\u003CCollidesWith\u003Ek__BackingField;
    set => this.\u003CCollidesWith\u003Ek__BackingField = value;
  }

  public bool Locked
  {
    get => this.\u003CLocked\u003Ek__BackingField;
    set => this.\u003CLocked\u003Ek__BackingField = value;
  }

  public float Radius
  {
    get => this.\u003CRadius\u003Ek__BackingField;
    set => this.\u003CRadius\u003Ek__BackingField = value;
  }

  public float Height
  {
    get => this.\u003CHeight\u003Ek__BackingField;
    set => this.\u003CHeight\u003Ek__BackingField = value;
  }

  public float MaxSpeed
  {
    get => this.\u003CMaxSpeed\u003Ek__BackingField;
    set => this.\u003CMaxSpeed\u003Ek__BackingField = value;
  }

  public float NeighbourDist
  {
    get => this.\u003CNeighbourDist\u003Ek__BackingField;
    set => this.\u003CNeighbourDist\u003Ek__BackingField = value;
  }

  public float AgentTimeHorizon
  {
    get => this.\u003CAgentTimeHorizon\u003Ek__BackingField;
    set => this.\u003CAgentTimeHorizon\u003Ek__BackingField = value;
  }

  public float ObstacleTimeHorizon
  {
    get => this.\u003CObstacleTimeHorizon\u003Ek__BackingField;
    set => this.\u003CObstacleTimeHorizon\u003Ek__BackingField = value;
  }

  public Vector3 Velocity
  {
    get => this.\u003CVelocity\u003Ek__BackingField;
    set => this.\u003CVelocity\u003Ek__BackingField = value;
  }

  public bool DebugDraw
  {
    get => this.\u003CDebugDraw\u003Ek__BackingField;
    set => this.\u003CDebugDraw\u003Ek__BackingField = value;
  }

  public int MaxNeighbours
  {
    get => this.\u003CMaxNeighbours\u003Ek__BackingField;
    set => this.\u003CMaxNeighbours\u003Ek__BackingField = value;
  }

  public List<ObstacleVertex> NeighbourObstacles => (List<ObstacleVertex>) null;

  public Agent(Vector3 pos)
  {
    this.MaxSpeed = 2f;
    this.NeighbourDist = 15f;
    this.AgentTimeHorizon = 2f;
    this.ObstacleTimeHorizon = 2f;
    this.Height = 5f;
    this.Radius = 5f;
    this.MaxNeighbours = 10;
    this.Locked = false;
    this.position = pos;
    this.Position = this.position;
    this.prevSmoothPos = this.position;
    this.smoothPos = this.position;
    this.Layer = RVOLayer.DefaultAgent;
    this.CollidesWith = (RVOLayer) -1;
  }

  public void BufferSwitch()
  {
    this.radius = this.Radius;
    this.height = this.Height;
    this.maxSpeed = this.MaxSpeed;
    this.neighbourDist = this.NeighbourDist;
    this.agentTimeHorizon = this.AgentTimeHorizon;
    this.obstacleTimeHorizon = this.ObstacleTimeHorizon;
    this.maxNeighbours = this.MaxNeighbours;
    this.desiredVelocity = this.DesiredVelocity;
    this.locked = this.Locked;
    this.collidesWith = this.CollidesWith;
    this.layer = this.Layer;
    this.Velocity = this.velocity;
    List<ObstacleVertex> obstaclesBuffered = this.obstaclesBuffered;
    this.obstaclesBuffered = this.obstacles;
    this.obstacles = obstaclesBuffered;
  }

  public void Update()
  {
    this.velocity = this.newVelocity;
    this.prevSmoothPos = this.smoothPos;
    this.position = this.prevSmoothPos;
    this.position += this.velocity * this.simulator.DeltaTime;
    this.Position = this.position;
  }

  public void Interpolate(float t)
  {
    this.smoothPos = this.prevSmoothPos + (this.Position - this.prevSmoothPos) * t;
  }

  public void CalculateNeighbours()
  {
    this.neighbours.Clear();
    this.neighbourDists.Clear();
    if (this.locked)
      return;
    if (this.MaxNeighbours > 0)
    {
      double neighbourDist1 = (double) this.neighbourDist;
      double neighbourDist2 = (double) this.neighbourDist;
      this.simulator.Quadtree.Query(new Vector2(this.position.x, this.position.z), this.neighbourDist, this);
    }
    this.obstacles.Clear();
    this.obstacleDists.Clear();
    double num = (double) this.obstacleTimeHorizon * (double) this.maxSpeed + (double) this.radius;
  }

  public float Sqr(float x) => x * x;

  public float InsertAgentNeighbour(Agent agent, float rangeSq)
  {
    if (this == agent || (agent.layer & this.collidesWith) == (RVOLayer) 0)
      return rangeSq;
    float num = this.Sqr(agent.position.x - this.position.x) + this.Sqr(agent.position.z - this.position.z);
    if ((double) num < (double) rangeSq)
    {
      if (this.neighbours.Count < this.maxNeighbours)
      {
        this.neighbours.Add(agent);
        this.neighbourDists.Add(num);
      }
      int index = this.neighbours.Count - 1;
      if ((double) num < (double) this.neighbourDists[index])
      {
        for (; index != 0 && (double) num < (double) this.neighbourDists[index - 1]; --index)
        {
          this.neighbours[index] = this.neighbours[index - 1];
          this.neighbourDists[index] = this.neighbourDists[index - 1];
        }
        this.neighbours[index] = agent;
        this.neighbourDists[index] = num;
      }
      if (this.neighbours.Count == this.maxNeighbours)
        rangeSq = this.neighbourDists[this.neighbourDists.Count - 1];
    }
    return rangeSq;
  }

  public void InsertObstacleNeighbour(ObstacleVertex ob1, float rangeSq)
  {
    ObstacleVertex next = ob1.next;
    float num = VectorMath.SqrDistancePointSegment(ob1.position, next.position, this.Position);
    if ((double) num >= (double) rangeSq)
      return;
    this.obstacles.Add(ob1);
    this.obstacleDists.Add(num);
    int index;
    for (index = this.obstacles.Count - 1; index != 0 && (double) num < (double) this.obstacleDists[index - 1]; --index)
    {
      this.obstacles[index] = this.obstacles[index - 1];
      this.obstacleDists[index] = this.obstacleDists[index - 1];
    }
    this.obstacles[index] = ob1;
    this.obstacleDists[index] = num;
  }

  public static Vector3 To3D(Vector2 p) => new Vector3(p.x, 0.0f, p.y);

  public static void DrawCircle(Vector2 _p, float radius, Color col)
  {
    Agent.DrawCircle(_p, radius, 0.0f, 6.28318548f, col);
  }

  public static void DrawCircle(Vector2 _p, float radius, float a0, float a1, Color col)
  {
    Vector3 vector3_1 = Agent.To3D(_p);
    while ((double) a0 > (double) a1)
      a0 -= 6.28318548f;
    Vector3 vector3_2 = new Vector3(Mathf.Cos(a0) * radius, 0.0f, Mathf.Sin(a0) * radius);
    for (int index = 0; (double) index <= 40.0; ++index)
    {
      Vector3 vector3_3 = new Vector3(Mathf.Cos(Mathf.Lerp(a0, a1, (float) index / 40f)) * radius, 0.0f, Mathf.Sin(Mathf.Lerp(a0, a1, (float) index / 40f)) * radius);
      UnityEngine.Debug.DrawLine(vector3_1 + vector3_2, vector3_1 + vector3_3, col);
      vector3_2 = vector3_3;
    }
  }

  public static void DrawVO(Vector2 circleCenter, float radius, Vector2 origin)
  {
    float num1 = Mathf.Atan2((origin - circleCenter).y, (origin - circleCenter).x);
    float f = radius / (origin - circleCenter).magnitude;
    float num2 = (double) f <= 1.0 ? Mathf.Abs(Mathf.Acos(f)) : 0.0f;
    Agent.DrawCircle(circleCenter, radius, num1 - num2, num1 + num2, Color.black);
    Vector2 vector2_1 = new Vector2(Mathf.Cos(num1 - num2), Mathf.Sin(num1 - num2)) * radius;
    Vector2 vector2_2 = new Vector2(Mathf.Cos(num1 + num2), Mathf.Sin(num1 + num2)) * radius;
    Vector2 p1 = -new Vector2(-vector2_1.y, vector2_1.x);
    Vector2 p2 = new Vector2(-vector2_2.y, vector2_2.x);
    Vector2 p3 = vector2_1 + circleCenter;
    Vector2 p4 = vector2_2 + circleCenter;
    UnityEngine.Debug.DrawRay(Agent.To3D(p3), Agent.To3D(p1).normalized * 100f, Color.black);
    UnityEngine.Debug.DrawRay(Agent.To3D(p4), Agent.To3D(p2).normalized * 100f, Color.black);
  }

  public static void DrawCross(Vector2 p, float size = 1f) => Agent.DrawCross(p, Color.white, size);

  public static void DrawCross(Vector2 p, Color col, float size = 1f)
  {
    size *= 0.5f;
    UnityEngine.Debug.DrawLine(new Vector3(p.x, 0.0f, p.y) - Vector3.right * size, new Vector3(p.x, 0.0f, p.y) + Vector3.right * size, col);
    UnityEngine.Debug.DrawLine(new Vector3(p.x, 0.0f, p.y) - Vector3.forward * size, new Vector3(p.x, 0.0f, p.y) + Vector3.forward * size, col);
  }

  public void CalculateVelocity(Simulator.WorkerContext context)
  {
    if (this.locked)
    {
      this.newVelocity = (Vector3) Vector2.zero;
    }
    else
    {
      if (context.vos.Length < this.neighbours.Count + this.simulator.obstacles.Count)
        context.vos = new Agent.VO[Mathf.Max(context.vos.Length * 2, this.neighbours.Count + this.simulator.obstacles.Count)];
      Vector2 vector2_1 = new Vector2(this.position.x, this.position.z);
      Agent.VO[] vos = context.vos;
      int voCount = 0;
      Vector2 vector2_2 = new Vector2(this.velocity.x, this.velocity.z);
      float inverseDt = 1f / this.agentTimeHorizon;
      float wallThickness = this.simulator.WallThickness;
      float weightFactor = this.simulator.algorithm == Simulator.SamplingAlgorithm.GradientDescent ? 1f : 5f;
      for (int index = 0; index < this.simulator.obstacles.Count; ++index)
      {
        ObstacleVertex obstacle = this.simulator.obstacles[index];
        ObstacleVertex obstacleVertex = obstacle;
        do
        {
          if (obstacleVertex.ignore || (double) this.position.y > (double) obstacleVertex.position.y + (double) obstacleVertex.height || (double) this.position.y + (double) this.height < (double) obstacleVertex.position.y || (obstacleVertex.layer & this.collidesWith) == (RVOLayer) 0)
          {
            obstacleVertex = obstacleVertex.next;
          }
          else
          {
            float f = Agent.VO.Det(new Vector2(obstacleVertex.position.x, obstacleVertex.position.z), obstacleVertex.dir, vector2_1);
            float num = Vector2.Dot(obstacleVertex.dir, vector2_1 - new Vector2(obstacleVertex.position.x, obstacleVertex.position.z));
            bool flag = (double) num <= (double) wallThickness * 0.05000000074505806 || (double) num >= (double) (new Vector2(obstacleVertex.position.x, obstacleVertex.position.z) - new Vector2(obstacleVertex.next.position.x, obstacleVertex.next.position.z)).magnitude - (double) wallThickness * 0.05000000074505806;
            if ((double) Mathf.Abs(f) < (double) this.neighbourDist)
            {
              if ((double) f <= 0.0 && !flag && (double) f > -(double) wallThickness)
              {
                vos[voCount] = new Agent.VO(vector2_1, new Vector2(obstacleVertex.position.x, obstacleVertex.position.z) - vector2_1, obstacleVertex.dir, weightFactor * 2f);
                ++voCount;
              }
              else if ((double) f > 0.0)
              {
                Vector2 p1 = new Vector2(obstacleVertex.position.x, obstacleVertex.position.z) - vector2_1;
                Vector2 p2 = new Vector2(obstacleVertex.next.position.x, obstacleVertex.next.position.z) - vector2_1;
                Vector2 normalized1 = p1.normalized;
                Vector2 normalized2 = p2.normalized;
                vos[voCount] = new Agent.VO(vector2_1, p1, p2, normalized1, normalized2, weightFactor);
                ++voCount;
              }
            }
            obstacleVertex = obstacleVertex.next;
          }
        }
        while (obstacleVertex != obstacle);
      }
      for (int index = 0; index < this.neighbours.Count; ++index)
      {
        Agent neighbour = this.neighbours[index];
        if (neighbour != this && (double) Math.Min(this.position.y + this.height, neighbour.position.y + neighbour.height) - (double) Math.Max(this.position.y, neighbour.position.y) >= 0.0)
        {
          Vector2 vector2_3 = new Vector2(neighbour.Velocity.x, neighbour.velocity.z);
          float radius = this.radius + neighbour.radius;
          Vector2 center = new Vector2(neighbour.position.x, neighbour.position.z) - vector2_1;
          Vector2 sideChooser = vector2_2 - vector2_3;
          Vector2 offset = !neighbour.locked ? (vector2_2 + vector2_3) * 0.5f : vector2_3;
          vos[voCount] = new Agent.VO(center, offset, radius, sideChooser, inverseDt, 1f);
          ++voCount;
          if (this.DebugDraw)
            Agent.DrawVO(vector2_1 + center * inverseDt + offset, radius * inverseDt, vector2_1 + offset);
        }
      }
      Vector2 zero1 = Vector2.zero;
      Vector2 vector;
      if (this.simulator.algorithm == Simulator.SamplingAlgorithm.GradientDescent)
      {
        if (this.DebugDraw)
        {
          for (int index1 = 0; index1 < 40; ++index1)
          {
            for (int index2 = 0; index2 < 40; ++index2)
            {
              Vector2 p1 = new Vector2((float) ((double) index1 * 15.0 / 40.0), (float) ((double) index2 * 15.0 / 40.0));
              Vector2 zero2 = Vector2.zero;
              float num = 0.0f;
              for (int index3 = 0; index3 < voCount; ++index3)
              {
                float weight;
                zero2 += vos[index3].Sample(p1 - vector2_1, out weight);
                if ((double) weight > (double) num)
                  num = weight;
              }
              Vector2 vector2_4 = new Vector2(this.desiredVelocity.x, this.desiredVelocity.z) - (p1 - vector2_1);
              Vector2 vector2_5 = zero2 + vector2_4 * Agent.DesiredVelocityScale;
              if ((double) vector2_4.magnitude * (double) Agent.DesiredVelocityWeight > (double) num)
                num = vector2_4.magnitude * Agent.DesiredVelocityWeight;
              if ((double) num > 0.0)
              {
                Vector2 vector2_6 = vector2_5 / num;
              }
              UnityEngine.Debug.DrawRay(Agent.To3D(p1), Agent.To3D(vector2_4 * 0.0f), Color.blue);
              float score = 0.0f;
              Vector2 p2 = p1 - Vector2.one * 15f * 0.5f;
              Vector2 vector2_7 = this.Trace(vos, voCount, p2, 0.01f, out score);
              if ((double) (p2 - vector2_7).sqrMagnitude < (double) this.Sqr(0.375f) * 2.5999999046325684)
                UnityEngine.Debug.DrawRay(Agent.To3D(vector2_7 + vector2_1), Vector3.up * 1f, Color.red);
            }
          }
        }
        float score1 = float.PositiveInfinity;
        float cutoff = new Vector2(this.velocity.x, this.velocity.z).magnitude * this.simulator.qualityCutoff;
        vector = this.Trace(vos, voCount, new Vector2(this.desiredVelocity.x, this.desiredVelocity.z), cutoff, out score1);
        if (this.DebugDraw)
          Agent.DrawCross(vector + vector2_1, Color.yellow, 0.5f);
        Vector2 velocity = (Vector2) this.Velocity;
        float score2;
        Vector2 vector2_8 = this.Trace(vos, voCount, velocity, cutoff, out score2);
        if ((double) score2 < (double) score1)
        {
          vector = vector2_8;
          score1 = score2;
        }
        if (this.DebugDraw)
          Agent.DrawCross(vector2_8 + vector2_1, Color.magenta, 0.5f);
      }
      else
      {
        Vector2[] samplePos = context.samplePos;
        float[] sampleSize = context.sampleSize;
        int index4 = 0;
        Vector2 vector2_9 = new Vector2(this.desiredVelocity.x, this.desiredVelocity.z);
        float num1 = Mathf.Max(this.radius, Mathf.Max(vector2_9.magnitude, this.Velocity.magnitude));
        samplePos[index4] = vector2_9;
        sampleSize[index4] = num1 * 0.3f;
        int index5 = index4 + 1;
        samplePos[index5] = vector2_2;
        sampleSize[index5] = num1 * 0.3f;
        int index6 = index5 + 1;
        Vector2 vector2_10 = vector2_2 * 0.5f;
        Vector2 vector2_11 = new Vector2(vector2_10.y, -vector2_10.x);
        for (int index7 = 0; index7 < 8; ++index7)
        {
          samplePos[index6] = vector2_11 * Mathf.Sin((float) ((double) index7 * 3.1415927410125732 * 2.0 / 8.0)) + vector2_10 * (1f + Mathf.Cos((float) ((double) index7 * 3.1415927410125732 * 2.0 / 8.0)));
          sampleSize[index6] = (float) ((1.0 - (double) Mathf.Abs((float) index7 - 4f) / 8.0) * (double) num1 * 0.5);
          ++index6;
        }
        Vector2 vector2_12 = vector2_10 * 0.6f;
        Vector2 vector2_13 = vector2_11 * 0.6f;
        for (int index8 = 0; index8 < 6; ++index8)
        {
          samplePos[index6] = vector2_13 * Mathf.Cos((float) (((double) index8 + 0.5) * 3.1415927410125732 * 2.0 / 6.0)) + vector2_12 * (1.66666663f + Mathf.Sin((float) (((double) index8 + 0.5) * 3.1415927410125732 * 2.0 / 6.0)));
          sampleSize[index6] = num1 * 0.3f;
          ++index6;
        }
        for (int index9 = 0; index9 < 6; ++index9)
        {
          samplePos[index6] = vector2_2 + new Vector2(num1 * 0.2f * Mathf.Cos((float) (((double) index9 + 0.5) * 3.1415927410125732 * 2.0 / 6.0)), num1 * 0.2f * Mathf.Sin((float) (((double) index9 + 0.5) * 3.1415927410125732 * 2.0 / 6.0)));
          sampleSize[index6] = (float) ((double) num1 * 0.20000000298023224 * 2.0);
          ++index6;
        }
        samplePos[index6] = vector2_2 * 0.5f;
        sampleSize[index6] = num1 * 0.4f;
        int index10 = index6 + 1;
        Vector2[] bestPos = context.bestPos;
        float[] bestSizes = context.bestSizes;
        float[] bestScores = context.bestScores;
        for (int index11 = 0; index11 < 3; ++index11)
          bestScores[index11] = float.PositiveInfinity;
        bestScores[3] = float.NegativeInfinity;
        Vector2 vector2_14 = vector2_2;
        float num2 = float.PositiveInfinity;
        for (int index12 = 0; index12 < 3; ++index12)
        {
          for (int index13 = 0; index13 < index10; ++index13)
          {
            float val1 = 0.0f;
            for (int index14 = 0; index14 < voCount; ++index14)
              val1 = Math.Max(val1, vos[index14].ScalarSample(samplePos[index13]));
            float magnitude = (samplePos[index13] - vector2_9).magnitude;
            float num3 = val1 + magnitude * Agent.DesiredVelocityWeight;
            float num4 = val1 + magnitude * (1f / 1000f);
            if (this.DebugDraw)
              Agent.DrawCross(vector2_1 + samplePos[index13], Agent.Rainbow(Mathf.Log(num4 + 1f) * 5f), sampleSize[index13] * 0.5f);
            if ((double) num3 < (double) bestScores[0])
            {
              for (int index15 = 0; index15 < 3; ++index15)
              {
                if ((double) num3 >= (double) bestScores[index15 + 1])
                {
                  bestScores[index15] = num3;
                  bestSizes[index15] = sampleSize[index13];
                  bestPos[index15] = samplePos[index13];
                  break;
                }
              }
            }
            if ((double) num4 < (double) num2)
            {
              vector2_14 = samplePos[index13];
              num2 = num4;
              if ((double) num4 == 0.0)
              {
                index12 = 100;
                break;
              }
            }
          }
          index10 = 0;
          for (int index16 = 0; index16 < 3; ++index16)
          {
            Vector2 vector2_15 = bestPos[index16];
            float num5 = bestSizes[index16];
            bestScores[index16] = float.PositiveInfinity;
            float num6 = (float) ((double) num5 * 0.60000002384185791 * 0.5);
            samplePos[index10] = vector2_15 + new Vector2(num6, num6);
            samplePos[index10 + 1] = vector2_15 + new Vector2(-num6, num6);
            samplePos[index10 + 2] = vector2_15 + new Vector2(-num6, -num6);
            samplePos[index10 + 3] = vector2_15 + new Vector2(num6, -num6);
            float num7 = num5 * (num5 * 0.6f);
            sampleSize[index10] = num7;
            sampleSize[index10 + 1] = num7;
            sampleSize[index10 + 2] = num7;
            sampleSize[index10 + 3] = num7;
            index10 += 4;
          }
        }
        vector = vector2_14;
      }
      if (this.DebugDraw)
        Agent.DrawCross(vector + vector2_1);
      this.newVelocity = Agent.To3D(Vector2.ClampMagnitude(vector, this.maxSpeed));
    }
  }

  public static Color Rainbow(float v)
  {
    Color color = new Color(v, 0.0f, 0.0f);
    if ((double) color.r > 1.0)
    {
      color.g = color.r - 1f;
      color.r = 1f;
    }
    if ((double) color.g > 1.0)
    {
      color.b = color.g - 1f;
      color.g = 1f;
    }
    return color;
  }

  public Vector2 Trace(Agent.VO[] vos, int voCount, Vector2 p, float cutoff, out float score)
  {
    score = 0.0f;
    float stepScale = this.simulator.stepScale;
    float num1 = float.PositiveInfinity;
    Vector2 vector2_1 = p;
    for (int index1 = 0; index1 < 50; ++index1)
    {
      float num2 = (float) (1.0 - (double) index1 / 50.0) * stepScale;
      Vector2 zero = Vector2.zero;
      float val1 = 0.0f;
      for (int index2 = 0; index2 < voCount; ++index2)
      {
        float weight;
        Vector2 vector2_2 = vos[index2].Sample(p, out weight);
        zero += vector2_2;
        if ((double) weight > (double) val1)
          val1 = weight;
      }
      Vector2 vector2_3 = new Vector2(this.desiredVelocity.x, this.desiredVelocity.z) - p;
      float val2 = vector2_3.magnitude * Agent.DesiredVelocityWeight;
      Vector2 vector2_4 = zero + vector2_3 * Agent.DesiredVelocityScale;
      float num3 = Math.Max(val1, val2);
      score = num3;
      if ((double) score < (double) num1)
        num1 = score;
      vector2_1 = p;
      if ((double) score > (double) cutoff || index1 <= 10)
      {
        float sqrMagnitude = vector2_4.sqrMagnitude;
        if ((double) sqrMagnitude > 0.0)
          vector2_4 *= num3 / Mathf.Sqrt(sqrMagnitude);
        Vector2 vector2_5 = vector2_4 * num2;
        Vector2 p1 = p;
        p += vector2_5;
        if (this.DebugDraw)
          UnityEngine.Debug.DrawLine(Agent.To3D(p1) + this.position, Agent.To3D(p) + this.position, Agent.Rainbow(0.1f / score) * new Color(1f, 1f, 1f, 0.2f));
      }
      else
        break;
    }
    score = num1;
    return vector2_1;
  }

  public static bool IntersectionFactor(
    Vector2 start1,
    Vector2 dir1,
    Vector2 start2,
    Vector2 dir2,
    out float factor)
  {
    float num1 = (float) ((double) dir2.y * (double) dir1.x - (double) dir2.x * (double) dir1.y);
    if ((double) num1 == 0.0)
    {
      factor = 0.0f;
      return false;
    }
    float num2 = (float) ((double) dir2.x * ((double) start1.y - (double) start2.y) - (double) dir2.y * ((double) start1.x - (double) start2.x));
    factor = num2 / num1;
    return true;
  }

  public struct VO
  {
    public Vector2 origin;
    public Vector2 center;
    public Vector2 line1;
    public Vector2 line2;
    public Vector2 dir1;
    public Vector2 dir2;
    public Vector2 cutoffLine;
    public Vector2 cutoffDir;
    public float sqrCutoffDistance;
    public bool leftSide;
    public bool colliding;
    public float radius;
    public float weightFactor;

    public VO(Vector2 offset, Vector2 p0, Vector2 dir, float weightFactor)
    {
      this.colliding = true;
      this.line1 = p0;
      this.dir1 = -dir;
      this.origin = Vector2.zero;
      this.center = Vector2.zero;
      this.line2 = Vector2.zero;
      this.dir2 = Vector2.zero;
      this.cutoffLine = Vector2.zero;
      this.cutoffDir = Vector2.zero;
      this.sqrCutoffDistance = 0.0f;
      this.leftSide = false;
      this.radius = 0.0f;
      this.weightFactor = weightFactor * 0.5f;
    }

    public VO(
      Vector2 offset,
      Vector2 p1,
      Vector2 p2,
      Vector2 tang1,
      Vector2 tang2,
      float weightFactor)
    {
      this.weightFactor = weightFactor * 0.5f;
      this.colliding = false;
      this.cutoffLine = p1;
      this.cutoffDir = (p2 - p1).normalized;
      this.line1 = p1;
      this.dir1 = tang1;
      this.line2 = p2;
      this.dir2 = tang2;
      this.dir2 = -this.dir2;
      this.cutoffDir = -this.cutoffDir;
      this.origin = Vector2.zero;
      this.center = Vector2.zero;
      this.sqrCutoffDistance = 0.0f;
      this.leftSide = false;
      this.radius = 0.0f;
      weightFactor = 5f;
    }

    public VO(
      Vector2 center,
      Vector2 offset,
      float radius,
      Vector2 sideChooser,
      float inverseDt,
      float weightFactor)
    {
      this.weightFactor = weightFactor * 0.5f;
      this.origin = offset;
      weightFactor = 0.5f;
      if ((double) center.magnitude < (double) radius)
      {
        this.colliding = true;
        this.leftSide = false;
        this.line1 = center.normalized * (center.magnitude - radius);
        this.dir1 = new Vector2(this.line1.y, -this.line1.x).normalized;
        this.line1 += offset;
        this.cutoffDir = Vector2.zero;
        this.cutoffLine = Vector2.zero;
        this.sqrCutoffDistance = 0.0f;
        this.dir2 = Vector2.zero;
        this.line2 = Vector2.zero;
        this.center = Vector2.zero;
        this.radius = 0.0f;
      }
      else
      {
        this.colliding = false;
        center *= inverseDt;
        radius *= inverseDt;
        Vector2 vector2_1 = center + offset;
        this.sqrCutoffDistance = center.magnitude - radius;
        this.center = center;
        this.cutoffLine = center.normalized * this.sqrCutoffDistance;
        Vector2 vector2_2 = new Vector2(-this.cutoffLine.y, this.cutoffLine.x);
        this.cutoffDir = vector2_2.normalized;
        this.cutoffLine += offset;
        this.sqrCutoffDistance *= this.sqrCutoffDistance;
        float num1 = Mathf.Atan2(-center.y, -center.x);
        float num2 = Mathf.Abs(Mathf.Acos(radius / center.magnitude));
        this.radius = radius;
        this.leftSide = VectorMath.RightOrColinear(Vector2.zero, center, sideChooser);
        this.line1 = new Vector2(Mathf.Cos(num1 + num2), Mathf.Sin(num1 + num2)) * radius;
        vector2_2 = new Vector2(this.line1.y, -this.line1.x);
        this.dir1 = vector2_2.normalized;
        this.line2 = new Vector2(Mathf.Cos(num1 - num2), Mathf.Sin(num1 - num2)) * radius;
        vector2_2 = new Vector2(this.line2.y, -this.line2.x);
        this.dir2 = vector2_2.normalized;
        this.line1 += vector2_1;
        this.line2 += vector2_1;
      }
    }

    public static bool Left(Vector2 a, Vector2 dir, Vector2 p)
    {
      return (double) dir.x * ((double) p.y - (double) a.y) - ((double) p.x - (double) a.x) * (double) dir.y <= 0.0;
    }

    public static float Det(Vector2 a, Vector2 dir, Vector2 p)
    {
      return (float) (((double) p.x - (double) a.x) * (double) dir.y - (double) dir.x * ((double) p.y - (double) a.y));
    }

    public Vector2 Sample(Vector2 p, out float weight)
    {
      if (this.colliding)
      {
        float num = Agent.VO.Det(this.line1, this.dir1, p);
        if ((double) num >= 0.0)
        {
          weight = num * this.weightFactor;
          return new Vector2(-this.dir1.y, this.dir1.x) * weight * Agent.GlobalIncompressibility;
        }
        weight = 0.0f;
        return new Vector2(0.0f, 0.0f);
      }
      float num1 = Agent.VO.Det(this.cutoffLine, this.cutoffDir, p);
      if ((double) num1 <= 0.0)
      {
        weight = 0.0f;
        return Vector2.zero;
      }
      float num2 = Agent.VO.Det(this.line1, this.dir1, p);
      float num3 = Agent.VO.Det(this.line2, this.dir2, p);
      if ((double) num2 >= 0.0 && (double) num3 >= 0.0)
      {
        if (this.leftSide)
        {
          if ((double) num1 < (double) this.radius)
          {
            weight = num1 * this.weightFactor;
            return new Vector2(-this.cutoffDir.y, this.cutoffDir.x) * weight;
          }
          weight = num2;
          return new Vector2(-this.dir1.y, this.dir1.x) * weight;
        }
        if ((double) num1 < (double) this.radius)
        {
          weight = num1 * this.weightFactor;
          return new Vector2(-this.cutoffDir.y, this.cutoffDir.x) * weight;
        }
        weight = num3 * this.weightFactor;
        return new Vector2(-this.dir2.y, this.dir2.x) * weight;
      }
      weight = 0.0f;
      return new Vector2(0.0f, 0.0f);
    }

    public float ScalarSample(Vector2 p)
    {
      if (this.colliding)
      {
        float num = Agent.VO.Det(this.line1, this.dir1, p);
        return (double) num >= 0.0 ? num * Agent.GlobalIncompressibility * this.weightFactor : 0.0f;
      }
      float num1 = Agent.VO.Det(this.cutoffLine, this.cutoffDir, p);
      if ((double) num1 <= 0.0)
        return 0.0f;
      float num2 = Agent.VO.Det(this.line1, this.dir1, p);
      float num3 = Agent.VO.Det(this.line2, this.dir2, p);
      if ((double) num2 < 0.0 || (double) num3 < 0.0)
        return 0.0f;
      return this.leftSide ? ((double) num1 < (double) this.radius ? num1 * this.weightFactor : num2 * this.weightFactor) : ((double) num1 < (double) this.radius ? num1 * this.weightFactor : num3 * this.weightFactor);
    }
  }
}
