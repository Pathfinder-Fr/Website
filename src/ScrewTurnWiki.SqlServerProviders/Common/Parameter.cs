namespace ScrewTurn.Wiki.Plugins.SqlCommon
{
    /// <summary>
    ///     Represents a generic database parameter.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:GenericDbParameter" /> class.
        /// </summary>
        /// <param name="type">The type of the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public Parameter(ParameterType type, string name, object value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     Gets the type of the parameter.
        /// </summary>
        public ParameterType Type { get; }

        /// <summary>
        ///     Gets the name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets or sets the value of the parameter.
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    ///     Lists parameter types.
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        ///     An Int16.
        /// </summary>
        Int16,

        /// <summary>
        ///     An Int32.
        /// </summary>
        Int32,

        /// <summary>
        ///     An Int64.
        /// </summary>
        Int64,

        /// <summary>
        ///     A unicode string.
        /// </summary>
        String,

        /// <summary>
        ///     A unicode character.
        /// </summary>
        Char,

        /// <summary>
        ///     A date/time.
        /// </summary>
        DateTime,

        /// <summary>
        ///     A boolean.
        /// </summary>
        Boolean,

        /// <summary>
        ///     A byte.
        /// </summary>
        Byte,

        /// <summary>
        ///     An array of bytes.
        /// </summary>
        ByteArray
    }
}