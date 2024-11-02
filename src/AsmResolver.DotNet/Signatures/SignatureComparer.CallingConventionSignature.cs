using System;
using System.Collections.Generic;

namespace AsmResolver.DotNet.Signatures
{
    public partial class SignatureComparer :
        IEqualityComparer<CallingConventionSignature>,
        IEqualityComparer<FieldSignature>,
        IEqualityComparer<MethodSignature>,
        IEqualityComparer<PropertySignature>,
        IEqualityComparer<LocalVariablesSignature>,
        IEqualityComparer<GenericInstanceMethodSignature>
    {
        /// <inheritdoc />
        public bool Equals(CallingConventionSignature? x, CallingConventionSignature? y) => Equals(x, y, default, default);
        public bool Equals(CallingConventionSignature? x, CallingConventionSignature? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            return x switch
            {
                LocalVariablesSignature localVarSig => Equals(localVarSig, y as LocalVariablesSignature, xContext, yContext),
                FieldSignature fieldSig => Equals(fieldSig, y as FieldSignature, xContext, yContext),
                MethodSignature methodSig => Equals(methodSig, y as MethodSignature, xContext, yContext),
                PropertySignature propertySig => Equals(propertySig, y as PropertySignature, xContext, yContext),
                _ => false
            };
        }

        /// <inheritdoc />
        public int GetHashCode(CallingConventionSignature obj)
        {
            return obj switch
            {
                LocalVariablesSignature localVarSig => GetHashCode(localVarSig),
                FieldSignature fieldSig => GetHashCode(fieldSig),
                MethodSignature methodSig => GetHashCode(methodSig),
                PropertySignature propertySig => GetHashCode(propertySig),
                _ => throw new ArgumentOutOfRangeException(nameof(obj))
            };
        }

        /// <inheritdoc />
        public bool Equals(FieldSignature? x, FieldSignature? y) => Equals(x, y, default, default);
        public bool Equals(FieldSignature? x, FieldSignature? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            return x.Attributes == y.Attributes
                   && Equals(x.FieldType, y.FieldType, xContext, yContext);
        }

        /// <inheritdoc />
        public int GetHashCode(FieldSignature obj)
        {
            unchecked
            {
                int hashCode = (int) obj.Attributes;
                hashCode = (hashCode * 397) ^ GetHashCode(obj.FieldType);
                return hashCode;
            }
        }

        /// <inheritdoc />
        public bool Equals(MethodSignature? x, MethodSignature? y) => Equals(x, y, default, default);
        public bool Equals(MethodSignature? x, MethodSignature? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            return x.Attributes == y.Attributes
                   && x.GenericParameterCount == y.GenericParameterCount
                   && Equals(x.ReturnType, y.ReturnType, xContext, yContext)
                   && Equals(x.ParameterTypes, y.ParameterTypes, xContext, yContext)
                   && Equals(x.SentinelParameterTypes, y.SentinelParameterTypes, xContext, yContext);
        }

        /// <inheritdoc />
        public int GetHashCode(MethodSignature obj)
        {
            unchecked
            {
                int hashCode = (int) obj.Attributes;
                hashCode = (hashCode * 397) ^ obj.GenericParameterCount;
                hashCode = (hashCode * 397) ^ GetHashCode(obj.ReturnType);
                hashCode = (hashCode * 397) ^ GetHashCode(obj.ParameterTypes);
                hashCode = (hashCode * 397) ^ GetHashCode(obj.SentinelParameterTypes);
                return hashCode;
            }
        }

        /// <inheritdoc />
        public bool Equals(LocalVariablesSignature? x, LocalVariablesSignature? y) => Equals(x, y, default, default);
        public bool Equals(LocalVariablesSignature? x, LocalVariablesSignature? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            return x.Attributes == y.Attributes
                   && Equals(x.VariableTypes, y.VariableTypes, xContext, yContext);
        }

        /// <inheritdoc />
        public int GetHashCode(LocalVariablesSignature obj)
        {
            unchecked
            {
                int hashCode = (int) obj.Attributes;
                hashCode = (hashCode * 397) ^ GetHashCode(obj.VariableTypes);
                return hashCode;
            }
        }

        /// <inheritdoc />
        public bool Equals(GenericInstanceMethodSignature? x, GenericInstanceMethodSignature? y) => Equals(x, y, default, default);
        public bool Equals(GenericInstanceMethodSignature? x, GenericInstanceMethodSignature? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            return x.Attributes == y.Attributes
                   && Equals(x.TypeArguments, y.TypeArguments, xContext, yContext);
        }

        /// <inheritdoc />
        public int GetHashCode(GenericInstanceMethodSignature obj)
        {
            unchecked
            {
                int hashCode = (int) obj.Attributes;
                hashCode = (hashCode * 397) ^ GetHashCode(obj.TypeArguments);
                return hashCode;
            }
        }

        /// <inheritdoc />
        public bool Equals(PropertySignature? x, PropertySignature? y) => Equals(x, y, default, default);
        public bool Equals(PropertySignature? x, PropertySignature? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            return x.Attributes == y.Attributes
                   && Equals(x.ReturnType, y.ReturnType, xContext, yContext)
                   && Equals(x.ParameterTypes, y.ParameterTypes, xContext, yContext);
        }

        /// <inheritdoc />
        public int GetHashCode(PropertySignature obj)
        {
            unchecked
            {
                int hashCode = (int) obj.Attributes;
                hashCode = (hashCode * 397) ^ GetHashCode(obj.ReturnType);
                hashCode = (hashCode * 397) ^ GetHashCode(obj.ParameterTypes);
                return hashCode;
            }
        }
    }
}
