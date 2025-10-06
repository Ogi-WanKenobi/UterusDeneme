// Scripts/Uterus/CenterPageView.cs
using TMPro;
using UnityEngine;

namespace Uterus
{
    public class CenterPageView : MonoBehaviour
    {
        public TMP_Text headerText;
        public TMP_Text bodyText;
        public TMP_Text footerText;

        public void Show(PageNode n)
        {
            if (!n) return;
            if (headerText) headerText.text = n.headerText;
            if (bodyText)  bodyText.text  = n.bodyText;
            if (footerText) footerText.text = n.footerText;
        }
    }
}
