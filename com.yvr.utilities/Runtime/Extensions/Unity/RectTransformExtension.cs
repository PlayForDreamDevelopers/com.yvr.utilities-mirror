using UnityEngine;

namespace YVR.Utilities
{
    public static class RectTransformExtension
    {
        private static Vector3[] s_Corners = new Vector3[4];

        public static bool RayIntersects(this RectTransform target, Ray ray, out Vector3 hitWorldPos)
        {
            target.GetWorldCorners(s_Corners); // bottom left -> top Left -> top right -> bottom right

            // Using three points to define a infinite plane
            var plane = new Plane(s_Corners[0], s_Corners[1], s_Corners[2]);

            bool hit = plane.Raycast(ray, out float enter); // whether the ray hit the infinite plane
            if (hit)
            {
                Vector3 intersectionPoint = ray.GetPoint(enter);
                Vector3 rectPlaneBottomEdge = s_Corners[3] - s_Corners[0];
                Vector3 rectPlaneLeftEdge = s_Corners[1] - s_Corners[0];
                Vector3 interactionEdge = intersectionPoint - s_Corners[0];
                float bottomDot = Vector3.Dot(interactionEdge, rectPlaneBottomEdge);
                float leftDot = Vector3.Dot(interactionEdge, rectPlaneLeftEdge);

                bool withInRectPlane = bottomDot > 0 && // The Intersection point is on the up side of the bottom edge
                                       leftDot > 0 && // The Intersection point is on the right side of left edge
                                       // The distance of the projection of intersection point on the bottom edge is shorter than the bottom edge itself
                                       bottomDot < rectPlaneBottomEdge.sqrMagnitude &&
                                       // The distance of the projection of intersection point on the left edge is shorter than the left edge itself
                                       leftDot < rectPlaneLeftEdge.sqrMagnitude;

                if (withInRectPlane)
                {
                    hitWorldPos = intersectionPoint;
                    return true;
                }
            }

            hitWorldPos = Vector3.one;
            return false;
        }

        public static RectTransform AnchorPosY(this RectTransform selfRectTrans, float anchorPosY)
        {
            Vector2 anchorPos = selfRectTrans.anchoredPosition;
            anchorPos.y = anchorPosY;
            selfRectTrans.anchoredPosition = anchorPos;
            return selfRectTrans;
        }

        public static RectTransform SetSizeHeight(this RectTransform selfRectTrans, float sizeHeight)
        {
            Vector2 sizeDelta = selfRectTrans.sizeDelta;
            sizeDelta.y = sizeHeight;
            selfRectTrans.sizeDelta = sizeDelta;
            return selfRectTrans;
        }

        public static void GetWorldSpaceSize(this RectTransform canvasTrans, ref Vector3 size)
        {
            canvasTrans.GetWorldCorners(s_Corners);

            float width = Vector3.Distance(s_Corners[0], s_Corners[3]);
            float height = Vector3.Distance(s_Corners[0], s_Corners[1]);

            size.x = width;
            size.y = height;
        }
    }
}