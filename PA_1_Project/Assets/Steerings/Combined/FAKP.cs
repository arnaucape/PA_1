using UnityEngine;

namespace Steerings
{
    public class FAKP : SteeringBehaviour
    {

        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;

        // the parameters for flocking
        public string idTag = "BOID";
        public float cohesionThreshold = 40f;
        public float repulsionThreshold = 10f;
        public float wanderRate = 10f;

        public float vmWeight = 0.08f;
        public float rpWeight = 0.46f;
        public float coWeight = 0.23f;
        public float wdWeight = 0.23f;

        // the target for seek
        public GameObject attractor;

        // this is for KP
        public float distance;
        public float angle;

        public float seekWeight = 0.2f; // weight of the seek behaviour

        private bool previous = false;
        public float safe;
        public float tooClose;

        public override SteeringOutput GetSteering()
        {
            SteeringOutput result = FAKP.GetSteering(this.ownKS, attractor, seekWeight, idTag, cohesionThreshold, repulsionThreshold, wanderRate,
                                                                vmWeight, rpWeight, coWeight, wdWeight, distance, angle,
                                                                ref previous, gameObject, tooClose, safe);
            base.applyRotationalPolicy(rotationalPolicy, result, attractor);
            return result;

        }

        public static SteeringOutput GetSteering(KinematicState ownKS, GameObject attractor, float seekWeight, string idTag,
            float cohesionThreshold , float repulsionThreshold ,
            float wanderRate ,
            float vmWeight , float rpWeight , float coWeight, float wdWeight,
            float distance, float angle, ref bool previous, GameObject self, float tooClose, float safe)
        {
            
            if (previous)
            {
                if (SensingUtils.DistanceToTarget(self,attractor)>=safe)
                {
                    previous = false;

                    //Vector3 pepe = Utils.OrientationToVector(attractor.GetComponent<KinematicState>().orientation + angle).normalized * distance;
                    //SURROGATE_TARGET.transform.position = attractor.transform.position + pepe;
                    return FlockingAround.GetSteering(ownKS, attractor, seekWeight, idTag, cohesionThreshold, repulsionThreshold, wanderRate, vmWeight,
                        rpWeight, coWeight, wdWeight);
                }
                else
                {
                    return Flee.GetSteering(ownKS, attractor);
                }
            }
            else
            {
                if (SensingUtils.DistanceToTarget(self,attractor)<tooClose)
                {
                    previous = true;
                    return Flee.GetSteering(ownKS, attractor);
                }
                else
                {
                    //Vector3 pepe = Utils.OrientationToVector(attractor.GetComponent<KinematicState>().orientation + angle).normalized * distance;
                    //SURROGATE_TARGET.transform.position = attractor.transform.position + pepe;
                    return FlockingAround.GetSteering(ownKS, attractor, seekWeight, idTag, cohesionThreshold, repulsionThreshold, wanderRate, vmWeight,
                        rpWeight, coWeight, wdWeight);
                }

            }

              
            
           
        }


    }
}
