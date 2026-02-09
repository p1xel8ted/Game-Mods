// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVOSimulator
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.RVO.Sampled;
using UnityEngine;

#nullable disable
namespace Pathfinding.RVO;

[AddComponentMenu("Pathfinding/Local Avoidance/RVO Simulator")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_simulator.php")]
public class RVOSimulator : MonoBehaviour
{
  [Tooltip("Calculate local avoidance in between frames.\nThis can increase jitter in the agents' movement so use it only if you really need the performance boost. It will also reduce the responsiveness of the agents to the commands you send to them.")]
  public bool doubleBuffering;
  [Tooltip("Interpolate positions between simulation timesteps")]
  public bool interpolation = true;
  [Tooltip("Desired FPS for rvo simulation. It is usually not necessary to run a crowd simulation at a very high fps.\nUsually 10-30 fps is enough, but can be increased for better quality.\nThe rvo simulation will never run at a higher fps than the game")]
  public int desiredSimulationFPS = 20;
  [Tooltip("Number of RVO worker threads. If set to None, no multithreading will be used.")]
  public ThreadCount workerThreads = ThreadCount.Two;
  [Tooltip("[GradientDescent][unitless][0...1] A higher value will result in lower quality local avoidance but faster calculations.")]
  public float qualityCutoff = 0.05f;
  [Tooltip("[GradientDescent][unitless][0...2] How large steps to take when searching for a minimum to the penalty function. Larger values will make it faster, but less accurate, too low values (near 0) can also give large inaccuracies. Values around 0.5-1.5 work the best.")]
  public float stepScale = 1.5f;
  [Tooltip("[0...infinity] Higher values will raise the penalty for agent-agent intersection")]
  public float incompressibility = 30f;
  [Tooltip("Thickness of RVO obstacle walls.\nIf obstacles are passing through obstacles, try a larger value, if they are getting stuck near small obstacles, try reducing it")]
  public float wallThickness = 1f;
  [Tooltip("[unitless][0...infinity] How much an agent will try to reach the desired velocity. A higher value will yield a more aggressive behaviour")]
  public float desiredVelocityWeight = 0.1f;
  [Tooltip("What sampling algorithm to use. GradientDescent is a bit more agressive but makes it easier for agents to intersect.")]
  public Simulator.SamplingAlgorithm algorithm = Simulator.SamplingAlgorithm.GradientDescent;
  [Tooltip("Run multiple simulation steps per step. Much slower, but may lead to slightly higher quality local avoidance.")]
  public bool oversampling;
  public Simulator simulator;

  public Simulator GetSimulator()
  {
    if (this.simulator == null)
      this.Awake();
    return this.simulator;
  }

  public void Awake()
  {
    if (this.desiredSimulationFPS < 1)
      this.desiredSimulationFPS = 1;
    if (this.simulator != null)
      return;
    this.simulator = new Simulator(AstarPath.CalculateThreadCount(this.workerThreads), this.doubleBuffering);
    this.simulator.Interpolation = this.interpolation;
    this.simulator.DesiredDeltaTime = 1f / (float) this.desiredSimulationFPS;
  }

  public void Update()
  {
    if (this.desiredSimulationFPS < 1)
      this.desiredSimulationFPS = 1;
    Agent.DesiredVelocityWeight = this.desiredVelocityWeight;
    Agent.GlobalIncompressibility = this.incompressibility;
    Simulator simulator = this.GetSimulator();
    simulator.DesiredDeltaTime = 1f / (float) this.desiredSimulationFPS;
    simulator.Interpolation = this.interpolation;
    simulator.stepScale = this.stepScale;
    simulator.qualityCutoff = this.qualityCutoff;
    simulator.algorithm = this.algorithm;
    simulator.Oversampling = this.oversampling;
    simulator.WallThickness = this.wallThickness;
    simulator.Update();
  }

  public void OnDestroy()
  {
    if (this.simulator == null)
      return;
    this.simulator.OnDestroy();
  }
}
