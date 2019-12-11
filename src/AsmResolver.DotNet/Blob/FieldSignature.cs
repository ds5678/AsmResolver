namespace AsmResolver.DotNet.Blob
{
    /// <summary>
    /// Represents a signature of a field defined in a .NET executable file.
    /// </summary>
    public class FieldSignature : MemberSignature
    {
        /// <summary>
        /// Reads a single field signature from an input stream.
        /// </summary>
        /// <param name="module">The module containing the signature.</param>
        /// <param name="reader">The blob input stream.</param>
        /// <returns>The field signature.</returns>
        public static FieldSignature FromReader(ModuleDefinition module, IBinaryStreamReader reader) =>
            FromReader(module, reader, RecursionProtection.CreateNew());

        /// <summary>
        /// Reads a single field signature from an input stream.
        /// </summary>
        /// <param name="module">The module containing the signature.</param>
        /// <param name="reader">The blob input stream.</param>
        /// <param name="protection">The object responsible for detecting infinite recursion.</param>
        /// <returns>The field signature.</returns>
        public static FieldSignature FromReader(ModuleDefinition module, IBinaryStreamReader reader,
            RecursionProtection protection)
        {
            return new FieldSignature(
                (CallingConventionAttributes) reader.ReadByte(),
                TypeSignature.FromReader(module, reader, protection));
        }
        
        /// <summary>
        /// Creates a new field signature with the provided field type.
        /// </summary>
        /// <param name="fieldType">The field type.</param>
        public FieldSignature(TypeSignature fieldType)
            : this(CallingConventionAttributes.Field, fieldType)
        {
        }
        
        /// <summary>
        /// Creates a new field signature with the provided field type.
        /// </summary>
        /// <param name="attributes">The attributes of the field.</param>
        /// <param name="fieldType">The field type.</param>
        /// <remarks>
        /// This constructor automatically sets the <see cref="CallingConventionAttributes.Field"/> bit.
        /// </remarks>
        public FieldSignature(CallingConventionAttributes attributes, TypeSignature fieldType) 
            : base(attributes | CallingConventionAttributes.Field, fieldType)
        {
        }

        /// <summary>
        /// Gets the type of the object that the field stores.
        /// </summary>
        public TypeSignature FieldType
        {
            get => MemberReturnType;
            set => MemberReturnType = value;
        }
    }
}