# Flocking System — Unity DOTS/ECS

<p align="center">
  <img src="https://img.shields.io/badge/Unity-DOTS%20%2F%20ECS-000000?style=for-the-badge&logo=unity&logoColor=white" />
  <img src="https://img.shields.io/badge/C%23-61%25-239120?style=for-the-badge&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/Algorithm-Boids-blueviolet?style=for-the-badge" />
  <img src="https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge" />
</p>

> Implementation of a **flocking simulation** using Unity DOTS (Data-Oriented Technology Stack) and the ECS (Entity Component System) architecture, based on **Craig Reynolds' Boids algorithm**, to simulate realistic bird flocking behavior at scale.

---

## 🎬 Demo

<p align="center">
  <a href="https://www.youtube.com/watch?v=JRo6zheqLxA">
    <img src="https://img.youtube.com/vi/JRo6zheqLxA/0.jpg" alt="Watch the demo" width="640"/>
  </a>
</p>

<p align="center">
  <a href="https://www.youtube.com/watch?v=JRo6zheqLxA">▶ Watch the presentation video on YouTube</a>
</p>

---

## 📖 Overview

This project implements a high-performance **flocking simulation** leveraging Unity's **DOTS** framework. By representing each boid as a lightweight ECS entity rather than a traditional `MonoBehaviour`, the system achieves significant performance gains — allowing thousands of agents to be simulated in real time.

The simulation faithfully reproduces the three classic rules defined by Craig Reynolds:

| Rule | Description |
|------|-------------|
| **Separation** | Steer to avoid crowding nearby flockmates |
| **Alignment** | Steer towards the average heading of nearby flockmates |
| **Cohesion** | Steer towards the average position of nearby flockmates |

---

## ✨ Features

- 🐦 **Large-scale simulation** — thousands of boids simulated in real time thanks to ECS and the Job System.
- ⚡ **Data-Oriented Design** — cache-friendly memory layout via ECS components for maximum CPU throughput.
- 🧵 **Multithreaded Jobs** — flocking logic runs in parallel using Unity's Burst-compiled Jobs.
- 🎛️ **Configurable Parameters** — separation, alignment, cohesion weights, perception radius, and speed all tunable at runtime.
- 🌐 **Spatial Partitioning** — neighbor lookups optimized to avoid O(n²) brute-force comparisons.

---

## 🗂️ Project Structure

```
Flocking-System-Ecs/
├── Assets/
│   ├── Scripts/          # ECS Systems, Components, and Authoring MonoBehaviours
│   └── Scenes/           # Unity scenes
├── Packages/             # Unity package manifest (DOTS, Burst, Collections, etc.)
├── ProjectSettings/      # Unity project settings
└── LICENSE
```

---

## 🧠 How It Works

The simulation is broken into ECS **Systems** that run every frame:

1. **BoidSpawnerSystem** — spawns boid entities at startup, assigning initial position, velocity, and flocking component data.
2. **BoidPerceptionSystem** — for each boid, queries nearby entities within a configurable perception radius and accumulates neighbor data (average position, average velocity, separation vector).
3. **BoidSteeringSystem** — applies the three Reynolds rules using the accumulated neighbor data to compute a steering force for each boid.
4. **BoidMovementSystem** — integrates velocity and updates each entity's `LocalTransform` accordingly.

All heavy computation runs inside **Burst-compiled jobs**, keeping the main thread free and maximising multi-core utilisation.

---

## 🚀 Getting Started

### Requirements

- Unity **2022.x** or newer (with DOTS packages support)
- Packages: `com.unity.entities`, `com.unity.burst`, `com.unity.collections`, `com.unity.mathematics`

### Setup

1. **Clone** the repository:
   ```bash
   git clone https://github.com/matteomarca99/Flocking-System-Ecs.git
   ```
2. **Open** the project in Unity Hub (Unity 2022.x+).
3. Unity will automatically restore packages from `Packages/manifest.json`.
4. **Open** the main scene from `Assets/Scenes/`.
5. Press **Play** to run the simulation.

---

## ⚙️ Configuration

Flocking parameters can be adjusted directly in the Inspector on the **Boid Spawner** authoring component:

| Parameter | Description |
|-----------|-------------|
| `Boid Count` | Number of boids to spawn |
| `Perception Radius` | How far each boid can "see" neighbours |
| `Separation Weight` | Influence of the separation rule |
| `Alignment Weight` | Influence of the alignment rule |
| `Cohesion Weight` | Influence of the cohesion rule |
| `Max Speed` | Maximum movement speed per boid |
| `Max Force` | Maximum steering force applied per frame |

---

## 📚 References

- Craig Reynolds — *[Flocks, Herds, and Schools: A Distributed Behavioral Model](https://www.cs.toronto.edu/~dt/siggraph97-course/cwr87/)* (SIGGRAPH 1987)
- Unity DOTS documentation — [docs.unity3d.com/Packages/com.unity.entities](https://docs.unity3d.com/Packages/com.unity.entities@latest)
- Unity Burst Compiler — [docs.unity3d.com/Packages/com.unity.burst](https://docs.unity3d.com/Packages/com.unity.burst@latest)

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Matteo Marcantoni** — [GitHub](https://github.com/matteomarca99)

---

<p align="center">
  Made with ❤️ for the Unity & game development community
</p>
