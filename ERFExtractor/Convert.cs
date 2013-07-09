namespace NWNUtil
{
    class MyConvert
    {
        /// <summary>
        /// Utility method to convert character arrays to string
        /// </summary>
        /// <param name="array">4 Char array</param>
        /// <returns>Returns a string composed of char array</returns>
        public static string CharArrayToString(char[] array)
        {
            var myString = "";
            var length = array.Length;
            for (var i = 0; i < length; i++)
            {
                myString = myString + array[i];
            }

            return myString.Trim('\0');
        }
    }
}
