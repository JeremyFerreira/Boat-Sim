
using UnityEngine;
#if UNITY_EDITOR

public static class GizmoExtension
{
    public static void DrawCircle(Vector3 position, float radius, Color color, float precision = 20)
    {
        Color old = Gizmos.color;
        Gizmos.color = color;
        for(int i = 0; i < precision; i++)
        {
            float angle1 = MathExtension.TAU*i/precision;
            Vector3 point1 = new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius + position;
            float angle2 = MathExtension.TAU * (i+1) / precision;
            Vector3 point2 = new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius + position;

            Gizmos.DrawLine(point1, point2);
        }
        Gizmos.color = old;
    }
    public static void DrawArc(Vector3 position, float radius, Vector3 direction, float angle, Color color, float precision = 20)
    {
        Color old = Gizmos.color;
        Gizmos.color = color;

        //Calculate angle and normalize direction
        float halfAngle = Mathf.Deg2Rad * angle /2f;
        direction = new Vector3(direction.x, 0f, direction.z).normalized;

        //Calculate side lines to draw
        Vector3 positiveHalfRotation = new Vector3(direction.x * Mathf.Cos(halfAngle) - direction.z * Mathf.Sin(halfAngle), 0f, direction.z * Mathf.Cos(halfAngle) + direction.x * Mathf.Sin(halfAngle)).normalized;
        Vector3 negativeHalfRotation = new Vector3(direction.x * Mathf.Cos(-halfAngle) - direction.z * Mathf.Sin(-halfAngle), 0f, direction.z * Mathf.Cos(-halfAngle) + direction.x * Mathf.Sin(-halfAngle)).normalized;

        Gizmos.DrawLine(position, position + positiveHalfRotation * radius);
        Gizmos.DrawLine(position, position + negativeHalfRotation * radius);


        //Draw Arc
        for(int i = 0; i < precision; i++)
        {
            float currentAngle = halfAngle * i / precision;
            float currentNextAngle = halfAngle * (i+1) / precision;
            //positiveside
            Vector3 dir1 = new Vector3(direction.x * Mathf.Cos(currentAngle) - direction.z * Mathf.Sin(currentAngle), 0f, direction.z * Mathf.Cos(currentAngle) + direction.x * Mathf.Sin(currentAngle)).normalized;
            Vector3 dir2 = new Vector3(direction.x * Mathf.Cos(currentNextAngle) - direction.z * Mathf.Sin(currentNextAngle), 0f, direction.z * Mathf.Cos(currentNextAngle) + direction.x * Mathf.Sin(currentNextAngle)).normalized;
            Gizmos.DrawLine(position + dir1 * radius, position + dir2 * radius);


            //negativeside
            Vector3 dir3 = new Vector3(direction.x * Mathf.Cos(-currentAngle) - direction.z * Mathf.Sin(-currentAngle), 0f, direction.z * Mathf.Cos(-currentAngle) + direction.x * Mathf.Sin(-currentAngle)).normalized;
            Vector3 dir4 = new Vector3(direction.x * Mathf.Cos(-currentNextAngle) - direction.z * Mathf.Sin(-currentNextAngle), 0f, direction.z * Mathf.Cos(-currentNextAngle) + direction.x * Mathf.Sin(-currentNextAngle)).normalized;
            Gizmos.DrawLine(position + dir3 * radius, position + dir4 * radius);

        }

        Gizmos.color = old;
    }
    public static void DrawWireArc(Vector3 position, float radius, Vector3 direction, float angle, Color color, float precision = 20)
    {
        Color old = Gizmos.color;
        Gizmos.color = color;

        //Calculate angle and normalize direction
        float halfAngle = Mathf.Deg2Rad * angle / 2f;
        direction = new Vector3(direction.x, 0f, direction.z).normalized;

        //Calculate side lines to draw
        Vector3 positiveHalfRotation = MathExtension.RotateVectorOnXZPlane(direction,halfAngle).normalized;
        Vector3 negativeHalfRotation = MathExtension.RotateVectorOnXZPlane(direction,- halfAngle).normalized;

        Gizmos.DrawLine(position, position + positiveHalfRotation * radius);
        Gizmos.DrawLine(position, position + negativeHalfRotation * radius);


        //Draw Arc
        for (int i = 0; i < precision; i++)
        {
            float currentAngle = halfAngle * i / precision;
            float currentNextAngle = halfAngle * (i + 1) / precision;
            //positiveside
            Vector3 dir1 = MathExtension.RotateVectorOnXZPlane(direction, currentAngle).normalized;
            Vector3 dir2 = MathExtension.RotateVectorOnXZPlane(direction, currentNextAngle).normalized;
            Gizmos.DrawLine(position + dir1 * radius, position + dir2 * radius);
            Gizmos.DrawLine(position, position + dir1 * radius);


            //negativeside
            Vector3 dir3 = MathExtension.RotateVectorOnXZPlane(direction, -currentAngle).normalized;
            Vector3 dir4 = MathExtension.RotateVectorOnXZPlane(direction, -currentNextAngle).normalized;
            Gizmos.DrawLine(position + dir3 * radius, position + dir4 * radius);
            Gizmos.DrawLine(position, position + dir3 * radius);

        }

        Gizmos.color = old;
    }
}
#endif
