using UnityEngine;
namespace Steerings
{
    public class KeepPosition : SteeringBehaviour
    {

        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;
        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;


        public override SteeringOutput GetSteering()
        {

            SteeringOutput result = KeepPosition.GetSteering(ownKS, target, requiredDistance, requiredAngle);
            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState me, GameObject target, float distance, float angle)
        {
            // get the target's orientation (as an angle)...
            float targetOrientation = target.transform.rotation.eulerAngles.z;

            // ... add the required angle
            float requiredOrientation = targetOrientation + angle;

            // convert the orientation into a direction (convert from angle to vector)
            Vector3 requiredDirection = Utils.OrientationToVector(requiredOrientation).normalized;

            // determine required position
            Vector3 requiredPosition = target.transform.position + requiredDirection * distance;

            // place surrogate target in required position
            SURROGATE_TARGET.transform.position = requiredPosition;

            return Seek.GetSteering(me, SURROGATE_TARGET);
            //return Arrive.GetSteering(me, SURROGATE_TARGET, 2, 5);
        }
    }

}
