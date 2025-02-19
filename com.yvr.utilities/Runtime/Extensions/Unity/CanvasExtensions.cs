using UnityEngine;
using UnityEngine.EventSystems;
#if USE_XRITK
using UnityEngine.XR.Interaction.Toolkit.UI;
#endif

namespace YVR.Utilities
{
    public static class CanvasExtensions
    {
        public static void GetWorldSpaceSize(this Canvas canvas, ref Vector3 size)
        {
            canvas.GetComponent<RectTransform>().GetWorldSpaceSize(ref size);
        }

        public static void SwitchCanvasUIInteractivity(this Canvas canvas, bool enable)
        {
            if (enable)
                TurnOnCanvasUIInteractivity();
            else
                TurnOffCanvasUIInteractivity();

            void TurnOnCanvasUIInteractivity()
            {
                canvas.worldCamera = Camera.main;
#if USE_XRITK
                var graphicRaycaster = canvas.transform.AutoAddingGetComponent<TrackedDeviceGraphicRaycaster>();
                //Enable to make UI be blocked by 3D objects that exist in front of it.
                graphicRaycaster.checkFor3DOcclusion = true;
#endif
                BaseRaycaster[] raycasterArray = canvas.transform.GetComponents<BaseRaycaster>();
                raycasterArray.ForEach(raycaster => raycaster.enabled = true);
            }

            void TurnOffCanvasUIInteractivity()
            {
                BaseRaycaster[] raycasterArray = canvas.transform.GetComponents<BaseRaycaster>();
                raycasterArray.ForEach(raycaster => raycaster.enabled = false);
            }
        }
    }
}