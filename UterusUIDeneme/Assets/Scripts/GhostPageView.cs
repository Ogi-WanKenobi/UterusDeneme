using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uterus
{
    public class GhostPageView : MonoBehaviour
    {
        [Header("UI")]
        public TMP_Text headerText;
        public TMP_Text bodyText;
        public TMP_Text footerText;

        [HideInInspector] public PageNode node;
        [HideInInspector] public Side side;

        RectTransform rt;
        LayoutElement layout;
        CanvasGroup cg;

        void Awake()
        {
            rt = GetComponent<RectTransform>();
            layout = GetComponent<LayoutElement>();
            cg = GetComponent<CanvasGroup>();
            SetGhostMode(true);
        }

        public void Bind(PageNode n, Side ownerSide)
        {
            node = n; side = ownerSide;
            headerText.text = n.headerText;
            bodyText.text   = n.bodyText;
            footerText.text = n.footerText;
            SetGhostMode(true);
        }

        public void RevealForCenter()
        {
            SetGhostMode(false);
            // Layout’ı mecburen güncelle (bazı cihazlarda TMP refresh istemezse görünmez gibi durur)
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        }

        void SetGhostMode(bool ghostMode)
        {
            if (bodyText)   bodyText.gameObject.SetActive(!ghostMode);
            if (footerText) footerText.gameObject.SetActive(!ghostMode);
            // Ghost alanında layout kontrollü kalsın, merkezde animasyonda ignore’ları açacağız.
            if (layout) layout.ignoreLayout = false;
            if (cg) cg.alpha = 1f;
        }
    }
}
