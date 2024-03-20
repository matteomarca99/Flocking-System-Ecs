using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SettingsAuthoring : MonoBehaviour
{
    public bool simulationStarted;
    public GameObject cubePrefab;
    public GameObject terrainPrefab;
    public int amountToSpawn;
    public float separationDistance;
    public float alignmentDistance;
    public float cohesionDistance;
    public float separationWeight;
    public float alignmentWeight;
    public float cohesionWeight;
    public float birdSpeed;
    public float2 simulationArea;

    public class Baker : Baker<SettingsAuthoring>
    {
        public override void Bake(SettingsAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new Settings
            {
                simulationStarted = authoring.simulationStarted,
                cubePrefabEntity = GetEntity(authoring.cubePrefab, TransformUsageFlags.Dynamic),
                terrainPrefabEntity = GetEntity(authoring.terrainPrefab, TransformUsageFlags.None),
                amountToSpawn = authoring.amountToSpawn,
                separationDistance = authoring.separationDistance,
                alignmentDistance = authoring.alignmentDistance,
                cohesionDistance = authoring.cohesionDistance,
                separationWeight = authoring.separationWeight,
                alignmentWeight = authoring.alignmentWeight,
                cohesionWeight = authoring.cohesionWeight,
                birdSpeed = authoring.birdSpeed,
                simulationArea = authoring.simulationArea
            });
        }
    }
}

public struct Settings : IComponentData
{
    public bool simulationStarted;
    public Entity cubePrefabEntity;
    public Entity terrainPrefabEntity;
    public int amountToSpawn;
    public float separationDistance;
    public float alignmentDistance;
    public float cohesionDistance;
    public float separationWeight;
    public float alignmentWeight;
    public float cohesionWeight;
    public float birdSpeed;
    public float2 simulationArea;
}
