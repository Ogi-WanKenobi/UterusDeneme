using UnityEngine;

namespace Uterus
{
    public enum Side { Left, Right }

    [System.Serializable]
    public struct BranchPair
    {
        public PageNode left;
        public PageNode right;
    }

    [CreateAssetMenu(menuName = "Uterus/Page Node", fileName = "PageNode")]
    public class PageNode : ScriptableObject
    {
        [Header("Display")]
        [TextArea] public string headerText;
        [TextArea] public string bodyText;
        [TextArea] public string footerText;

        [Header("Branches (where this node was chosen from)")]
        public BranchPair fromLeft;   // soldan seçildiyse: (sol, sağ)
        public BranchPair fromRight;  // sağdan seçildiyse: (sol, sağ)
    }
}
