using System.Linq;

namespace Etienne {
    public partial class Extensions {

        /// <summary>
        /// Remove everything afer the last specified char
        /// </summary>
        /// <param name="c">Char to remove from</param>
        /// <param name="keepChar">Do you want to keep the char</param>
        /// <returns></returns>
        public static string RemoveAfter(this string input, char c, bool keepChar = false) {
            int index = input.LastIndexOf(c);
            if(index > 0) {
                return input.Substring(0, keepChar ? index + 1 : index);
            }

            return input;
        }

        public static string[] RemoveAt(this string[] array, int index) {
            return array.Where(o => o != array[index]).ToArray();
        }
    }
}