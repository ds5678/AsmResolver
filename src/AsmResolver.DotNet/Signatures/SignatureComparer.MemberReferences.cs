using System;
using System.Collections.Generic;

namespace AsmResolver.DotNet.Signatures
{
    public partial class SignatureComparer :
        IEqualityComparer<MemberReference>,
        IEqualityComparer<IMethodDescriptor>,
        IEqualityComparer<IFieldDescriptor>,
        IEqualityComparer<MethodSpecification>
    {
        /// <inheritdoc />
        public bool Equals(MemberReference? x, MemberReference? y) => Equals(x, y, default, default);
        public bool Equals(MemberReference? x, MemberReference? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            if (x.IsMethod)
                return Equals((IMethodDescriptor) x, y, xContext, yContext);
            if (y.IsField)
                return Equals((IFieldDescriptor) x, y, xContext, yContext);
            return false;
        }

        /// <inheritdoc />
        public int GetHashCode(MemberReference obj)
        {
            if (obj.IsMethod)
                return GetHashCode((IMethodDescriptor) obj);
            if (obj.IsField)
                return GetHashCode((IFieldDescriptor) obj);
            throw new ArgumentOutOfRangeException(nameof(obj));
        }

        /// <inheritdoc />
        public bool Equals(IMethodDescriptor? x, IMethodDescriptor? y) => Equals(x, y, default, default);
        public bool Equals(IMethodDescriptor? x, IMethodDescriptor? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            if (x is MethodSpecification specification)
                return Equals(specification, y as MethodSpecification, xContext, yContext);

            return x.Name == y.Name
                   && Equals(x.DeclaringType, y.DeclaringType, xContext, yContext)
                   && Equals(x.Signature, y.Signature, xContext, yContext);
        }

        /// <inheritdoc />
        public int GetHashCode(IMethodDescriptor obj)
        {
            unchecked
            {
                int hashCode = obj.Name is null ? 0 : obj.Name.GetHashCode();
                hashCode = (hashCode * 397) ^ (obj.DeclaringType is not null ? GetHashCode(obj.DeclaringType) : 0);
                hashCode = (hashCode * 397) ^ (obj.Signature is not null ? GetHashCode(obj.Signature) : 0);
                return hashCode;
            }
        }

        /// <inheritdoc />
        public bool Equals(IFieldDescriptor? x, IFieldDescriptor? y) => Equals(x, y, default, default);
        public bool Equals(IFieldDescriptor? x, IFieldDescriptor? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            return x.Name == y.Name
                   && Equals(x.DeclaringType, y.DeclaringType, xContext, yContext)
                   && Equals(x.Signature, y.Signature, xContext, yContext);
        }

        /// <inheritdoc />
        public int GetHashCode(IFieldDescriptor obj)
        {
            unchecked
            {
                int hashCode = obj.Name is null ? 0 : obj.Name.GetHashCode();
                hashCode = (hashCode * 397) ^ (obj.DeclaringType is not null ? GetHashCode(obj.DeclaringType) : 0);
                hashCode = (hashCode * 397) ^ (obj.Signature is not null ? GetHashCode(obj.Signature) : 0);
                return hashCode;
            }
        }

        /// <inheritdoc />
        public bool Equals(MethodSpecification? x, MethodSpecification? y) => Equals(x, y, default, default);
        public bool Equals(MethodSpecification? x, MethodSpecification? y, in GenericContext xContext, in GenericContext yContext)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;

            return Equals(x.Method, y.Method, xContext, yContext)
                   && Equals(x.Signature, y.Signature, xContext, yContext);
        }

        /// <inheritdoc />
        public int GetHashCode(MethodSpecification obj)
        {
            unchecked
            {
                int hashCode = obj.Method == null ? 0 : GetHashCode(obj.Method);
                hashCode = (hashCode * 397) ^ (obj.Signature is not null ? GetHashCode(obj.Signature) : 0);
                return hashCode;
            }
        }
    }
}
