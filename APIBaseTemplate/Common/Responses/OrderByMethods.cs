using System.Reflection;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// </summary>
    public class OrderByMethods
    {
        public MethodInfo OrderBy { get; }
        public MethodInfo OrderByDescending { get; }
        public MethodInfo ThenBy { get; }
        public MethodInfo ThenByDescending { get; }
        public OrderByMethods(MethodInfo orderBy, MethodInfo orderByDescending, MethodInfo thenBy, MethodInfo thenByDescending)
        {
            OrderBy = orderBy;
            OrderByDescending = orderByDescending;
            ThenBy = thenBy;
            ThenByDescending = thenByDescending;
        }
    }
}
