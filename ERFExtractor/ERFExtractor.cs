using System;
using System.IO;

namespace NWNUtil
{
    /// <summary>
    /// Provides access to ERF file format. (.mod,.hak, .erf, .nwm)
    /// </summary>
    public class ErfExtractor
    {
        private char[] _cFileType;                // 4 chars
        private char[] _cVersion;                 // 4 chars
        public UInt32 LanguageCount;           // 32 bit
        public UInt32 LocalizedStringSize;     // 32 bit
        public UInt32 EntryCount;              // 32 bit
        public UInt32 OffsetToLocalizedString; // 32 bit
        public UInt32 OffsetToKeyList;         // 32 bit
        public UInt32 OffsetToResourceList;    // 32 bit
        private byte[] _bBuildYear;               // 4 Bytes
        private byte[] _bBuildDay;                // 4 Bytes
        public byte[] BDescriptionStrRef;       // 4 Bytes
        //private byte[] _reserved;                // 116 Bytes. Can be used to add additional fields to mod file

        // Provide access to the bytes, and character arrays via basic ints or strings
        public int BuildYear
        {
            get { return BuildYear = BitConverter.ToInt32(_bBuildYear, 0); }
            set { _bBuildYear = BitConverter.GetBytes(value); }
        }
        public int BuildDay
        {
            get { return BuildDay = BitConverter.ToInt32(_bBuildDay, 0); }
            set { _bBuildDay = BitConverter.GetBytes(value); }
        }
        public int DescriptionStrRef
        {
            get { return DescriptionStrRef = BitConverter.ToInt32(BDescriptionStrRef, 0); }
            set { BDescriptionStrRef = BitConverter.GetBytes(value); }
        }
        public string FileType
        {

            get { return FileType = MyConvert.CharArrayToString(_cFileType); }
            set { _cFileType = value.ToCharArray(); }
        }
        public string Version
        {
            get { return Version = MyConvert.CharArrayToString(_cVersion); }
            set { _cVersion = value.ToCharArray(); }
        }

        public string Error = "";

        /// <summary>
        /// Read header information and store it to properties from erf
        /// </summary>
        /// <param name="filePath">Path to erf file</param>
        public void ReadHeader(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var binReader =
                new BinaryReader(File.Open(filePath, FileMode.Open));
            try
            {
                // Tests to see if the file can be read.
                // Reads first three bytes of the mod file into the buffer
                var testArray = new byte[3];
                var count = binReader.Read(testArray, 0, 3);

                if (count == 0) return;
                // Resets the position in the stream to zero
                binReader.BaseStream.Seek(0, SeekOrigin.Begin);

                _cFileType = binReader.ReadChars(4);
                _cVersion = binReader.ReadChars(4);
                LanguageCount = binReader.ReadUInt32();
                LocalizedStringSize = binReader.ReadUInt32();
                EntryCount = binReader.ReadUInt32();
                OffsetToLocalizedString = binReader.ReadUInt32();
                OffsetToKeyList = binReader.ReadUInt32();
                OffsetToResourceList = binReader.ReadUInt32();
                _bBuildYear = binReader.ReadBytes(4);
                _bBuildDay = binReader.ReadBytes(4);
                BDescriptionStrRef = binReader.ReadBytes(4);
                //_reserved = binReader.ReadBytes(116);
            }
            catch (EndOfStreamException e)
            {
                Error = e.Message;
            }
            finally
            {
                binReader.Close();
            }
        }


        /// <summary>
        /// Read header information and store it to properties from erf
        /// Afterwards extract all the files stored in the erf
        /// </summary>
        /// <param name="modPath">String path to erf</param>
        /// <param name="destinationDirectory">String path to destination directory to extract to</param>
        public void ExtractMod(string modPath, string destinationDirectory)
        {
            if (_cFileType == null)
                ReadHeader(modPath);

            var binReader =
                new BinaryReader(File.Open(modPath, FileMode.Open));
            try
            {
                // Test to see if we can read from the file
                var testArray = new byte[3];
                var count = binReader.Read(testArray, 0, 3);
                if (count == 0) return;
                // Begin reading data after the header which is always 160 bytes long
                binReader.BaseStream.Seek(160, SeekOrigin.Begin);

                // Loop through the string elements and store them in an array of ERFStrings

                var erfString = new ErfString[LanguageCount];
                uint offset = 0;

                for (var i = 0; i < LanguageCount; i++)
                {
                    erfString[i].LanguageId = binReader.ReadUInt32();
                    erfString[i].StringSize = binReader.ReadUInt32();
                    offset = offset + 8;
                    // Read a number of chars equivalent to the StringSize and then convert them
                    // from a char array to a string
                    erfString[i].SString = MyConvert.CharArrayToString(binReader.ReadChars(Convert.ToInt32(erfString[i].StringSize)));
                    offset = offset + erfString[i].StringSize;
                }

                // Look through the key elements and store them in an array of Resources
                // Each key element has the same length and number of key elements is stored in EntryCount

                var resource = new Resource[EntryCount];

                while (binReader.PeekChar() == 0)
                {
                    binReader.BaseStream.Seek(1, SeekOrigin.Current);
                }

                for (var i = 0; i < EntryCount; i++)
                {
                    resource[i].ChResRef = binReader.ReadChars(16);
                    resource[i].ResId = binReader.ReadInt32();
                    resource[i].ResType = binReader.ReadInt16();
                    //resource[i].Unused = binReader.ReadBytes(2);
                    binReader.BaseStream.Seek(2, SeekOrigin.Current);
                }

                // Look through the resource list elements and store them in an array of Resources
                // Each resource element corresponds directly to the key elements

                // Seek to start of resource list. For some strange reason it loses its position after processing key list?
                binReader.BaseStream.Seek(OffsetToResourceList, SeekOrigin.Begin);

                for (var i = 0; i < EntryCount; i++)
                {
                    resource[i].OffsetToResource = binReader.ReadUInt32();
                    resource[i].ResourceSize = binReader.ReadUInt32();
                }
                for (var i = 0; i < EntryCount; i++) // Good
                {
                    var resourcePath = destinationDirectory + resource[i].ResRef + "." + resource[i].GetFileExtension(); // Define name for files

                    // Seek to start of data
                    binReader.BaseStream.Seek(resource[i].OffsetToResource, SeekOrigin.Begin);
                    // Read data into write buffer
                    var write = binReader.ReadBytes(Convert.ToInt32(resource[i].ResourceSize));

                    using (var binWriter = new BinaryWriter(File.Open(resourcePath, FileMode.Create)))
                    {
                        binWriter.Write(write);
                    }
                }
            }
            catch (EndOfStreamException e)
            {
                Error = e.Message;
            }
            finally
            {
                binReader.Close();
            }
        }
    }
}
