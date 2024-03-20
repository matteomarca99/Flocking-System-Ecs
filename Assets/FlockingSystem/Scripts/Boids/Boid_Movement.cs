using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public partial class Boid_Movement : SystemBase
{
    public NativeParallelMultiHashMap<int, Boid_ComponentData> cellVsEntityPositions;

    public static int GetUniqueKeyForPosition(float3 position, int cellSize)
    {
        return (int)((15 * math.floor(position.x / cellSize)) + (17 * math.floor(position.y / cellSize)) + (19 * math.floor(position.z / cellSize)));
    }

    protected override void OnCreate()
    {
        cellVsEntityPositions = new NativeParallelMultiHashMap<int, Boid_ComponentData>(0, Allocator.Persistent);
    }

    protected override void OnUpdate()
    {
        EntityQuery eq = GetEntityQuery(typeof(Boid_ComponentData));
        cellVsEntityPositions.Clear();
        if (eq.CalculateEntityCount() > cellVsEntityPositions.Capacity)
        {
            cellVsEntityPositions.Capacity = eq.CalculateEntityCount();
        }

        NativeParallelMultiHashMap<int, Boid_ComponentData>.ParallelWriter cellVsEntityPositionsParallel = cellVsEntityPositions.AsParallelWriter();
        Entities.ForEach((ref Boid_ComponentData bc, ref LocalTransform trans) =>
        {
            Boid_ComponentData bcValues = new Boid_ComponentData();
            bcValues = bc;
            bcValues.currentPosition = trans.Position;
            cellVsEntityPositionsParallel.Add(GetUniqueKeyForPosition(trans.Position, bc.cellSize), bcValues);
        }).ScheduleParallel();

        float deltaTime = SystemAPI.Time.DeltaTime;
        NativeParallelMultiHashMap<int, Boid_ComponentData> cellVsEntityPositionsForJob = cellVsEntityPositions;
        Entities.WithBurst().WithReadOnly(cellVsEntityPositionsForJob).ForEach((ref Boid_ComponentData bc, ref LocalTransform trans) =>
        {
            int key = GetUniqueKeyForPosition(trans.Position, bc.cellSize);
            NativeParallelMultiHashMapIterator<int> nmhKeyIterator;
            Boid_ComponentData neighbour;
            int total = 0;
            float3 separation = float3.zero;
            float3 alignment = float3.zero;
            float3 coheshion = float3.zero;

            if (cellVsEntityPositionsForJob.TryGetFirstValue(key, out neighbour, out nmhKeyIterator))
            {
                do
                {
                    if (!trans.Position.Equals(neighbour.currentPosition) && math.distance(trans.Position, neighbour.currentPosition) < bc.perceptionRadius)
                    {
                        float3 distanceFromTo = trans.Position - neighbour.currentPosition;
                        separation += (distanceFromTo / math.distance(trans.Position, neighbour.currentPosition));
                        coheshion += neighbour.currentPosition;
                        alignment += neighbour.velocity;
                        total++;
                    }
                } while (cellVsEntityPositionsForJob.TryGetNextValue(out neighbour, ref nmhKeyIterator));
                if (total > 0)
                {
                    coheshion = coheshion / total;
                    coheshion = coheshion - (trans.Position + bc.velocity);
                    coheshion = math.normalize(coheshion) * bc.cohesionBias;

                    separation = separation / total;
                    separation = separation - bc.velocity;
                    separation = math.normalize(separation) * bc.separationBias;

                    alignment = alignment / total;
                    alignment = alignment - bc.velocity;
                    alignment = math.normalize(alignment) * bc.alignmentBias;

                }
                bc.boidsInCell = total;
                bc.acceleration += (coheshion + alignment + separation);
                trans.Rotation = math.slerp(trans.Rotation, quaternion.LookRotation(math.normalize(bc.velocity), math.up()), deltaTime * 10);
                bc.velocity = bc.velocity + bc.acceleration;
                bc.velocity = math.normalize(bc.velocity) * bc.speed;
                trans.Position = math.lerp(trans.Position, (trans.Position + bc.velocity), deltaTime * bc.step);
                bc.acceleration = math.normalize(bc.target - trans.Position) * bc.targetBias;
            }
        }).ScheduleParallel();
    }

    protected override void OnDestroy()
    {
        cellVsEntityPositions.Dispose();
    }
}