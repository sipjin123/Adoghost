namespace CustomAIBT.Utilities
{
    using UnityEngine;

    public static class CustTransformUtils
    {
        /// <summary>
        /// Instantly rotates the source to face the target on the Y axis only (no pitch/tilt).
        /// </summary>
        /// <param name="source">Transform that will rotate</param>
        /// <param name="target">Target to face</param>
        public static void FaceTarget(Transform source, Transform target)
        {
            if (source == null || target == null) return;

            Vector3 direction = (target.position - source.position).normalized;
            direction.y = 0f; // Optional: lock to horizontal rotation only

            if (direction != Vector3.zero)
            {
                source.rotation = Quaternion.LookRotation(direction);
            }
        }

        /// <summary>
        /// Instantly rotates the source to face the given world position on the Y axis only.
        /// </summary>
        /// <param name="source">Transform that will rotate</param>
        /// <param name="worldPosition">World position to face</param>
        public static void FacePosition(Transform source, Vector3 worldPosition)
        {
            if (source == null) return;

            Vector3 direction = (worldPosition - source.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                source.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}