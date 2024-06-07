using System;
using System.Collections;
using System.Collections.Generic;
using AsmResolver.IO;

namespace AsmResolver.PE.DotNet.Metadata.Tables
{
    /// <summary>
    /// Represents a single row in the module definition metadata table.
    /// </summary>
    public struct ModuleReferenceRow : IMetadataRow
    {
        /// <summary>
        /// Reads a single module reference row from an input stream.
        /// </summary>
        /// <param name="reader">The input stream.</param>
        /// <param name="layout">The layout of the module reference table.</param>
        /// <returns>The row.</returns>
        public static ModuleReferenceRow FromReader(ref BinaryStreamReader reader, TableLayout layout)
        {
            return new ModuleReferenceRow(reader.ReadIndex((IndexSize) layout.Columns[0].Size));
        }

        /// <summary>
        /// Creates a new row for the module reference metadata table.
        /// </summary>
        /// <param name="name">The index into the #Strings heap referencing the name of the external module that was
        /// imported.</param>
        public ModuleReferenceRow(uint name)
        {
            Name = name;
        }

        /// <inheritdoc />
        public TableIndex TableIndex => TableIndex.ModuleRef;

        /// <inheritdoc />
        public int Count => 1;

        /// <inheritdoc />
        public uint this[int index] => index switch
        {
            0 => Name,
            _ => throw new IndexOutOfRangeException()
        };

        /// <summary>
        /// Gets or sets an index into the #Strings heap referencing the name of the external module that was imported.
        /// </summary>
        public uint Name
        {
            get;
            set;
        }

        /// <inheritdoc />
        public void Write(BinaryStreamWriter writer, TableLayout layout)
        {
            writer.WriteIndex(Name, (IndexSize) layout.Columns[0].Size);
        }

        /// <summary>
        /// Determines whether this row is considered equal to the provided module row.
        /// </summary>
        /// <param name="other">The other row.</param>
        /// <returns><c>true</c> if the rows are equal, <c>false</c> otherwise.</returns>
        public bool Equals(ModuleReferenceRow other)
        {
            return Name == other.Name;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is ModuleReferenceRow other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (int) Name;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({Name:X8})";
        }

        /// <inheritdoc />
        public IEnumerator<uint> GetEnumerator()
        {
            return new MetadataRowColumnEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
