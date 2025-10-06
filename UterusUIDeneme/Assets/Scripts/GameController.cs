using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Uterus
{
    public class GameController : MonoBehaviour
    {
        [Header("Areas")]
        public Transform leftGhostArea;
        public Transform rightGhostArea;
        public RectTransform centerSlot;

        [Header("Prefabs")]
        public GhostPageView ghostPagePrefab;

        [Header("Start Options")]
        public PageNode[] initialLeftOptions  = new PageNode[1];  // ana sayfada soldaki(ler)
        public PageNode[] initialRightOptions = new PageNode[1];  // ana sayfada sağdaki(ler)

        // drag iptali için ekranda tuttuğumuz son durum
        PageNode[] currentLeftOptions;
        PageNode[] currentRightOptions;

        void Start()
        {
            RefreshSide(Side.Left,  initialLeftOptions);
            RefreshSide(Side.Right, initialRightOptions);
        }

        // === Drag lifecycle ===
        public void OnPageDragBegin(GhostPageView fromView)
        {
            if (fromView == null) return;
            var other = fromView.side == Side.Left ? Side.Right : Side.Left;
            ClearSideVisuals(other, fade:true); // çekilmeyen tarafı gizle
        }

        public void OnPageDragCanceled(GhostPageView fromView)
        {
            if (fromView == null) return;
            var other = fromView.side == Side.Left ? Side.Right : Side.Left;
            // çekilmeyen tarafı eski state ile geri getir
            if (other == Side.Left)  RefreshSide(Side.Left,  currentLeftOptions);
            else                     RefreshSide(Side.Right, currentRightOptions);
        }

        // === Drop kabul ===
        public void OnPageDroppedToCenter(GhostPageView chosen, DraggablePage drag)
        {
            if (!chosen || chosen.node == null) return;

            // 1) merkez slotu temizle
            for (int i = centerSlot.childCount - 1; i >= 0; i--)
                Destroy(centerSlot.GetChild(i).gameObject);

            // 2) seçileni merkeze oturt (snap) + küçük pop
            SnapToCenter(chosen);
            StartCoroutine(PopAnimate(chosen.transform as RectTransform));

            // 3) body/footer görünür
            chosen.RevealForCenter();

            // 4) drag kabul (geri dönmesin)
            drag?.MarkAccepted();

            // 5) DAL: iki tarafı da seçilen düğüme göre doldur
BranchPair pair = (chosen.side == Side.Left)
    ? chosen.node.fromLeft
    : chosen.node.fromRight;

var nextLeft  = pair.left  ? new[] { pair.left  } : System.Array.Empty<PageNode>();
var nextRight = pair.right ? new[] { pair.right } : System.Array.Empty<PageNode>();

RefreshSide(Side.Left,  nextLeft);
RefreshSide(Side.Right, nextRight);
        }

        // === Yardımcılar ===
        void RefreshSide(Side side, PageNode[] nodes)
        {
            var parent = side == Side.Left ? leftGhostArea : rightGhostArea;

            // eski görselleri sil
            for (int i = parent.childCount - 1; i >= 0; i--)
                Destroy(parent.GetChild(i).gameObject);

            // state’i güncelle (drag cancel için)
            if (side == Side.Left)  currentLeftOptions  = nodes?.ToArray();
            else                    currentRightOptions = nodes?.ToArray();

            if (nodes == null) return;

            foreach (var n in nodes.Where(x => x != null))
            {
                var v = Instantiate(ghostPagePrefab, parent);
                v.Bind(n, side);

                // drag eventi için controller referansı ver
                var drag = v.GetComponent<DraggablePage>();
                if (drag) drag.controller = this;
            }
        }

        void ClearSideVisuals(Side side, bool fade)
        {
            var parent = side == Side.Left ? leftGhostArea : rightGhostArea;
            if (!fade)
            {
                for (int i = parent.childCount - 1; i >= 0; i--)
                    Destroy(parent.GetChild(i).gameObject);
                return;
            }
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                var t = parent.GetChild(i);
                var cg = t.GetComponent<CanvasGroup>() ?? t.gameObject.AddComponent<CanvasGroup>();
                StartCoroutine(FadeAndDestroy(cg, 0.12f));
            }
        }

        IEnumerator FadeAndDestroy(CanvasGroup cg, float dur)
        {
            float t = 0f, a0 = cg.alpha;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                cg.alpha = Mathf.Lerp(a0, 0f, t / dur);
                yield return null;
            }
            if (cg) Destroy(cg.gameObject);
        }

        void SnapToCenter(GhostPageView view)
        {
            var rt = view.GetComponent<RectTransform>();
            var le = view.GetComponent<LayoutElement>();
            if (le) le.ignoreLayout = true;

            rt.SetParent(centerSlot, false);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.localScale = Vector3.one;
        }

        IEnumerator PopAnimate(RectTransform rt)
        {
            float t = 0f, dur = 0.18f;
            Vector3 a = Vector3.one * 0.94f, b = Vector3.one;
            rt.localScale = a;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                float k = Mathf.SmoothStep(0f, 1f, t / dur);
                rt.localScale = Vector3.LerpUnclamped(a, b, k);
                yield return null;
            }
            rt.localScale = b;
        }
    }
}
