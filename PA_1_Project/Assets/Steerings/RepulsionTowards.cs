using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings
{ 
    public class RepulsionTowards : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;
        // FT and FTI make no sense in this behaviour

        public string idTag = "REPULSIVE";
        public float repulsionThreshold = 20f;   // at which distance does repulsion start?

        public GameObject target;

        public float repulsionForce;

        public override SteeringOutput GetSteering()
        {
            // no KS? get it
            if (this.ownKS == null) this.ownKS = GetComponent<KinematicState>();

            SteeringOutput result = RepulsionTowards.GetSteering(ownKS, idTag, repulsionThreshold,  target, repulsionForce);
            base.applyRotationalPolicy(rotationalPolicy, result);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, string tag, float repulsionThreshold, GameObject target, float repulsionForce)
        {
            SteeringOutput result;
            SteeringOutput seek;
            SteeringOutput repulsion;


            seek = Steerings.Seek.GetSteering(ownKS, target);
            repulsion = LinearRepulsion.GetSteering(ownKS, tag, repulsionThreshold);

            if (seek == null && repulsion == null)
                return null;
            if (seek == null)
                return repulsion;
            if (repulsion == null)
                return seek;

            result = new SteeringOutput();
            result.linearAcceleration = seek.linearAcceleration * (1 - repulsionForce) + repulsion.linearAcceleration * repulsionForce;
            result.angularAcceleration = seek.angularAcceleration * (1 - repulsionForce) + repulsion.angularAcceleration * repulsionForce;

            return result;
        }
    }
}

