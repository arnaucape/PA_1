using UnityEngine;

namespace Steerings
{

    public class LeaderFollowingBlended : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;

        /* COMPLETE */
        // parameters required by KeepPosition
        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        // parameters required by LinearRepulsion
        public string idTag;
        public float repulsionThreshold;

        //weight for LinearRepulsion
        public float wlr = 0.5f;

        public override SteeringOutput GetSteering()
        {
            SteeringOutput result = LeaderFollowingBlended.GetSteering(this.ownKS, target,
                                                                          requiredDistance, requiredAngle,
                                                                          tag, repulsionThreshold,
                                                                          wlr /* COMPLETE */);
            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS,
                                                  GameObject target, float distance, float angle,
                                                  string tag, float repulsionTh,
                                                  float wlr /* COMPLETE */)
        {
            // compute both steerings
            SteeringOutput lr = LinearRepulsion.GetSteering(ownKS, tag, repulsionTh);
            SteeringOutput kp = KeepPosition.GetSteering(ownKS, target, distance, angle);

            // blend
            // (if one is SteeringBehaviour.NULL_STEERING return the other.
            // if none is SteeringBehaviour.NULL_STEERING blend with weights wlr and 1-wlr)
            /* COMPLETE */

            if (lr == SteeringBehaviour.NULL_STEERING)
                return kp;
            if (kp == SteeringBehaviour.NULL_STEERING)
                return lr;


            // "concoct" the blending on lr
            lr.linearAcceleration = wlr * lr.linearAcceleration + (1 - wlr) * kp.linearAcceleration;
            return lr;
        }

    }
}