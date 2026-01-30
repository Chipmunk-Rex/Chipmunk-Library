namespace Chipmunk.Library.Utility.ComponentContainers
{
    public static class ComponentContainerExtension
    {
        public static T GetContainerComponent<T>(this IContainerComponent component, bool isDerived = false) where T : IContainerComponent
        {
            return component.ComponentContainer.GetComponent<T>(isDerived);
        }
        public static T GetCompo<T>(this IContainerComponent component, bool isDerived = false) where T : IContainerComponent
        {
            return component.ComponentContainer.GetComponent<T>(isDerived);
        }
        public static T Get<T>(this IContainerComponent component, bool isDerived = false) where T : IContainerComponent
        {
            return component.ComponentContainer.GetComponent<T>(isDerived);
        }
        public static T GetContainerComponent<T>(this IExcludeContainerComponent component, bool isDerived = false) where T : IContainerComponent
        {
            return component.ComponentContainer.GetComponent<T>(isDerived);
        }
        public static T GetCompo<T>(this IExcludeContainerComponent component, bool isDerived = false) where T : IContainerComponent
        {
            return component.ComponentContainer.GetComponent<T>(isDerived);
        }
        public static T Get<T>(this IExcludeContainerComponent component, bool isDerived = false) where T : IContainerComponent
        {
            return component.ComponentContainer.GetComponent<T>(isDerived);
        }
    }
}