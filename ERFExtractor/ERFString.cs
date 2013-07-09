using System;

namespace NWNUtil
{
    public struct ErfString
    {
        public UInt32 LanguageId;
        public UInt32 StringSize;
        public string SString;

        /// <summary>
        /// Used to store NWN strings
        /// </summary>
        /// <param name="languageId">Language ID. Reference to table of language IDs</param>
        /// <param name="stringSize">Length of string in chars</param>
        /// <param name="sString">Actual string value</param>
        public ErfString(UInt32 languageId, UInt32 stringSize, string sString)
        {
            LanguageId = languageId;
            StringSize = stringSize;
            SString = sString;
        }
    }
}
