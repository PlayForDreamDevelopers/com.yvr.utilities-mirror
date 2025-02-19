using UnityEngine;

namespace YVR.Utilities
{
    public static class GameObjectExtensions
    {
        public static GameObject Show(this GameObject selfObj)
        {
            selfObj.SetActive(true);
            return selfObj;
        }

        public static GameObject Hide(this GameObject selfObj)
        {
            selfObj.SetActive(false);
            return selfObj;
        }


        public static void GetCanvasWorldSize(this GameObject go, ref Vector3 size)
        {
            var canvas = go.GetComponentInChildren<Canvas>();
            if (!canvas)
                throw new System.Exception($"No Canvas component found of go {go.name} while get canvas world size.");

            canvas.GetWorldSpaceSize(ref size);
        }

    }
}