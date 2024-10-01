using UnityEngine;

namespace Class
{

    public static class MathFuncs
    {

        const float APPROXMATE = 0.001f;

        public static Vector3 VectorLerp(Vector3 startV, Vector3 targetV, float speed = 10f)
        {
            float startX = startV.x;
            float targetX = targetV.x;

            if (startX != targetX)
            {
                startX = Mathf.Lerp(startX, targetX, Time.deltaTime * speed);
                if (Mathf.Abs(startX - targetX) <= APPROXMATE)
                    startX = targetX;
            }


            float startZ = startV.z;
            float targetZ = targetV.z;

            if (startZ != targetZ)
            {
                startZ = Mathf.Lerp(startZ, targetZ, Time.deltaTime * speed);
                if (Mathf.Abs(startZ - targetZ) <= APPROXMATE)
                    startZ = targetZ;
            }

            return new Vector3(startX, startV.y, startZ);

        }

    }

}