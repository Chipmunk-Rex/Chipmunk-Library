using UnityEngine.UIElements;

namespace Chipmunk.Library.MaterialConverter.Editor
{
    [UxmlElement]
    public partial class SplitView : TwoPaneSplitView
    {
        [UxmlAttribute] public bool centerPane = false;

        public SplitView()
        {
        }
    }
}