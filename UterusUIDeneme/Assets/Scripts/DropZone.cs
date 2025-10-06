using UnityEngine;
using UnityEngine.EventSystems;

namespace Uterus
{
    [RequireComponent(typeof(UnityEngine.UI.Image))] // raycast için garanti
    public class DropZone : MonoBehaviour, IDropHandler
    {
        // public GameController controller;

        public void OnDrop(PointerEventData e)
        {
            // if (!controller || e.pointerDrag == null) return;

            // Drag edilen kökte olmayabilir, o yüzden InParent al
            var view = e.pointerDrag.GetComponentInParent<GhostPageView>();
            var drag = e.pointerDrag.GetComponentInParent<DraggablePage>();

            if (view && drag)
            {
                // controller.OnPageDroppedToCenter(view, drag);
                drag.MarkAccepted(); // GERİ DÖNMEYİ ENGELLEYEN KRİTİK SATIR
                Debug.Log("DROP accepted -> " + view.name);
            }
        }
    }
}
