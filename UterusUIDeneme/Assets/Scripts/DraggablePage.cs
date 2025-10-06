using UnityEngine;
using UnityEngine.EventSystems;

namespace Uterus
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggablePage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        
        
        // public GameController controller; // GameController referansı spawn’da atanacak
        RectTransform rt;
        Transform originalParent;
        Canvas rootCanvas;
        CanvasGroup cg;
        bool droppedAccepted = false;
        // GhostPageView view;

        public event OnDragDroppedHandler OnDragDropped;
        public delegate void OnDragDroppedHandler(DraggablePage draggablePage);

        void Awake()
        {
            rt = GetComponent<RectTransform>();
            cg = GetComponent<CanvasGroup>();
            // view = GetComponent<GhostPageView>();
            rootCanvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData e)
        {
            originalParent = rt.parent;
            rt.SetParent(rootCanvas.transform, true);
            cg.blocksRaycasts = false;
            cg.alpha = 0.9f;
            droppedAccepted = false;
            //
            // // Karşı tarafı gizle (iptal olursa controller geri getirecek)
            // controller?.OnPageDragBegin(view);
        }

        public void OnDrag(PointerEventData e)
        {
            rt.position = e.position;
        }

        public void OnEndDrag(PointerEventData e)
        {
            if (!droppedAccepted)
            {
                
                // Drop kabul edilmediyse: geri dön + karşı tarafı eski hâline getir
                rt.SetParent(originalParent, true);
                cg.blocksRaycasts = true;
                cg.alpha = 1f;
                
                // controller?.OnPageDragCanceled(view);
            }
        }

        public void MarkAccepted()
        {
            droppedAccepted = true;
            cg.blocksRaycasts = true;
            cg.alpha = 1f;
            enabled = false; // merkezde tekrar sürüklenmesin
            
            OnDragDropped?.Invoke(this);
        }
    }
}
