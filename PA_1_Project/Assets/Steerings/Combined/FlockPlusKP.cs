using UnityEngine;

namespace Steerings
{
    public class FlockPlusKP : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;
        
        // Parameters for flocking...
        public string idTag = "BOID";
        public float cohesionThreshold = 40f;
        public float repulsionThreshold = 10f;
        public float wanderRate = 10f;
        public float vmWeight = 0.08f;
        public float rpWeight = 0.46f;
        public float coWeight = 0.23f;
        public float wdWeight = 0.23f;

        // Parameters for KeepPosition
        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        // the weight for kp
        public float kpWeight = 0.5f;

        public override SteeringOutput GetSteering()
        {
            SteeringOutput result = FlockPlusKP.GetSteering(this.ownKS, idTag, cohesionThreshold, repulsionThreshold,
                                                            wanderRate, vmWeight, rpWeight, coWeight, wdWeight,
                                                            target, requiredDistance, requiredAngle,
                                                            kpWeight);
            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKs, string idTag, float cohesionThreshold, float repulsionThreshold,
                                                 float wanderRate, float vmWeight, float rpWeight, float coWeight, float wdWeight,
                                                 GameObject target, float distance, float angle,
                                                 float kpWeight)
        {
            SteeringOutput fl = Flocking.GetSteering(ownKs, idTag, cohesionThreshold, repulsionThreshold,
                                                     wanderRate,  vmWeight, rpWeight, coWeight, wdWeight);
            SteeringOutput kp = KeepPosition.GetSteering(ownKs, target, distance, angle);

            if (kp == NULL_STEERING) return fl;

            fl.linearAcceleration = fl.linearAcceleration * (1 - kpWeight) + kp.linearAcceleration * kpWeight;
            // fl.linearAcceleration = fl.linearAcceleration.normalized * ownKs.maxAcceleration;

            return fl;
        }

    }
}