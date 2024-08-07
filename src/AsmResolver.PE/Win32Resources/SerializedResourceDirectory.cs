using System;
using System.Collections.Generic;
using AsmResolver.Collections;
using AsmResolver.IO;
using AsmResolver.PE.File;

namespace AsmResolver.PE.Win32Resources
{
    /// <summary>
    /// Provides an implementation of a resource directory that was read from an existing PE file.
    /// </summary>
    public class SerializedResourceDirectory : ResourceDirectory
    {
        /// <summary>
        /// Indicates the size of a single sub-directory entry in a resource directory.
        /// </summary>
        public const uint ResourceDirectorySize = 2 * sizeof(uint) + 4 * sizeof(ushort);

        /// <summary>
        /// Indicates the maximum depth of sub directories a resource directory can have before AsmResolver aborts
        /// reading the resource tree branch.
        /// </summary>
        public const int MaxDepth = 10;

        private readonly PEReaderContext _context;
        private readonly ushort _namedEntries;
        private readonly ushort _idEntries;
        private readonly uint _entriesRva;
        private readonly int _depth;

        /// <summary>
        /// Reads a single resource directory from an input stream.
        /// </summary>
        /// <param name="context">The reader context.</param>
        /// <param name="entry">The entry to read. If this value is <c>null</c>, the root directory is assumed.</param>
        /// <param name="directoryReader">The input stream.</param>
        /// <param name="depth">
        /// The current depth of the resource directory tree structure.
        /// If this value exceeds <see cref="MaxDepth"/>, this class will not initialize any entries.
        /// </param>
        public SerializedResourceDirectory(PEReaderContext context, ResourceDirectoryEntry? entry,
            ref BinaryStreamReader directoryReader, int depth = 0)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _depth = depth;

            if (entry.HasValue)
            {
                var value = entry.Value;
                if (value.IsByName)
                    Name = value.Name;
                else
                    Id = value.IdOrNameOffset;
            }

            if (directoryReader.IsValid)
            {
                Characteristics = directoryReader.ReadUInt32();
                TimeDateStamp = directoryReader.ReadUInt32();
                MajorVersion = directoryReader.ReadUInt16();
                MinorVersion = directoryReader.ReadUInt16();

                _namedEntries = directoryReader.ReadUInt16();
                _idEntries = directoryReader.ReadUInt16();
                _entriesRva = directoryReader.Rva;

                directoryReader.Offset =
                    directoryReader.Offset + (ulong) ((_namedEntries + _idEntries) * ResourceDirectoryEntry.EntrySize);
            }
        }

        /// <inheritdoc />
        protected override IList<IResourceEntry> GetEntries()
        {
            var result = new OwnedCollection<ResourceDirectory, IResourceEntry>(this);

            // Optimisation, check for invalid resource directory offset, and prevention of self loop:
            if (_namedEntries + _idEntries == 0 || _depth >= MaxDepth)
            {
                _context.BadImage($"Reached maximum recursion depth of {_depth} sub resource directories.");
                return result;
            }

            uint baseRva = _context.File.OptionalHeader
                .GetDataDirectory(DataDirectoryIndex.ResourceDirectory)
                .VirtualAddress;

            // Create entries reader.
            uint entryListSize = (uint) ((_namedEntries + _idEntries) * ResourceDirectoryEntry.EntrySize);
            if (!_context.File.TryCreateReaderAtRva(_entriesRva, entryListSize, out var entriesReader))
            {
                _context.BadImage("Resource directory contains an invalid entry table RVA and/or entry count.");
                return result;
            }

            for (int i = 0; i < _namedEntries + _idEntries; i++)
            {
                var rawEntry = new ResourceDirectoryEntry(_context, ref entriesReader);

                // Note: Even if creating the directory reader fails, we still want to include the directory entry
                //       itself. In such a case, we expose the directory as an empty directory. This is why the
                //       following if statement does not dictate the creation of the data entry or not.

                if (!_context.File.TryCreateReaderAtRva(baseRva + rawEntry.DataOrSubDirOffset, out var entryReader))
                    _context.BadImage($"Resource directory entry {i.ToString()} has an invalid data offset.");

                result.Add(rawEntry.IsSubDirectory
                    ? new SerializedResourceDirectory(_context,  rawEntry, ref entryReader, _depth + 1)
                    : new SerializedResourceData(_context, rawEntry, ref entryReader));
            }

            return result;
        }

    }
}
