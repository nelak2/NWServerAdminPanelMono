using System;

namespace NWNUtil
{
    public struct Resource
    {
        //----
        // All data below is from Key List
        //----

        // Resref can be a max of 16 chars
        /// <summary>
        /// Get or set ResRef as char array
        /// Use ResRef if you want to work with a string
        /// </summary>
        public char[] ChResRef;
        /// <summary>
        /// Get or set ResRef as string
        /// Use chResRef if you want to work with a char array
        /// </summary>
        public string ResRef
        {
            get { return MyConvert.CharArrayToString(ChResRef); }
            set { ChResRef = value.ToCharArray(); }
        }
        public Int32 ResId;
        /// <summary>
        /// Integer refering to file extension
        /// </summary>
        public Int32 ResType;
        /// <summary>
        /// Unused bytes in keylist
        /// </summary>
        public byte[] Unused;

        //----
        // All data below is from Resource List
        //----
        /// <summary>
        /// Offset to file data from beginning of ERF
        /// </summary>
        public UInt32 OffsetToResource;
        /// <summary>
        /// Number of bytes
        /// </summary>
        public UInt32 ResourceSize;

        public string GetFileExtension()
        {
            string fileExtension;

            switch (ResType)
            {
                case 1:
                    fileExtension = "bmp";
                    break;
                case 3:
                    fileExtension = "tga";
                    break;
                case 4:
                    fileExtension = "wav";
                    break;
                case 6:
                    fileExtension = "plt";
                    break;
                case 7:
                    fileExtension = "ini";
                    break;
                case 10:
                    fileExtension = "txt";
                    break;
                case 2002:
                    fileExtension = "mdl";
                    break;
                case 2009:
                    fileExtension = "nss";
                    break;
                case 2010:
                    fileExtension = "ncs";
                    break;
                case 2012:
                    fileExtension = "are";
                    break;
                case 2013:
                    fileExtension = "set";
                    break;
                case 2014:
                    fileExtension = "ifo";
                    break;
                case 2015:
                    fileExtension = "bic";
                    break;
                case 2016:
                    fileExtension = "wok";
                    break;
                case 2017:
                    fileExtension = "2da";
                    break;
                case 2022:
                    fileExtension = "txi";
                    break;
                case 2023:
                    fileExtension = "git";
                    break;
                case 2025:
                    fileExtension = "uti";
                    break;
                case 2027:
                    fileExtension = "utc";
                    break;
                case 2029:
                    fileExtension = "dlg";
                    break;
                case 2030:
                    fileExtension = "itp";
                    break;
                case 2032:
                    fileExtension = "utt";
                    break;
                case 2033:
                    fileExtension = "dds";
                    break;
                case 2035:
                    fileExtension = "uts";
                    break;
                case 2036:
                    fileExtension = "ltr";
                    break;
                case 2037:
                    fileExtension = "gff";
                    break;
                case 2038:
                    fileExtension = "fac";
                    break;
                case 2040:
                    fileExtension = "ute";
                    break;
                case 2042:
                    fileExtension = "utd";
                    break;
                case 2044:
                    fileExtension = "utp";
                    break;
                case 2045:
                    fileExtension = "dft";
                    break;
                case 2046:
                    fileExtension = "gic";
                    break;
                case 2047:
                    fileExtension = "gui";
                    break;
                case 2051:
                    fileExtension = "utm";
                    break;
                case 2052:
                    fileExtension = "dwk";
                    break;
                case 2053:
                    fileExtension = "pwk";
                    break;
                case 2056:
                    fileExtension = "jrl";
                    break;
                case 2058:
                    fileExtension = "utw";
                    break;
                case 2060:
                    fileExtension = "ssf";
                    break;
                case 2064:
                    fileExtension = "ndb";
                    break;
                case 2065:
                    fileExtension = "ptm";
                    break;
                case 2066:
                    fileExtension = "ptt";
                    break;
                default:
                    fileExtension = "err";
                    break;
            }
            return fileExtension;
        }
    }
}
