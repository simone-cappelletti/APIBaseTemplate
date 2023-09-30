namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Order by Asc/Desc
    /// </summary>
    public enum EnmOrderBy
    {
        Asc,
        Desc
    }

    /// <summary>
    /// Sort field Asc/Desc
    /// </summary>
    public class OrderByOption
    {
        public OrderByOption()
        {
            Field = string.Empty;
        }

        public OrderByOption(string field, EnmOrderBy direction)
        {
            Field = field ?? string.Empty;
            Direction = direction;
        }

        public static readonly OrderByOption[] EmptyOptionList = new OrderByOption[] { };

        /// <summary>
        /// Field label to be ordered
        /// </summary>
        public string Field { set; get; }

        /// <summary>
        /// Sort direction
        /// </summary>
        public EnmOrderBy Direction { set; get; }

        public override string ToString()
        {
            return $"{Field}:" + Enum.GetName(Direction.GetType(), Direction);
        }
    }
}
