using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biletall.Helper
{
    public static class ApiHelper
    {
        public static string ApiUrl = "http://biletonly.com/api"; //"http://localhost:2230/api"; //"https://biletonly.azurewebsites.net/api";


        /*
        static string[] charArray = new string[]{
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", 
            "a", "b", "c", "ç", "d", "e", "f", "g", "ğ", "h", "ı", "i", "j", "k", "l", "m", 
            "n", "o", "ö", "p", "q", "r", "s", "ş", "t", "u", "ü", "v", "w", "x", "y", "z",  
            "A", "B", "C", "Ç", "D", "E", "F", "G", "Ğ", "H", "I", "İ", "J", "K", "L", "M", 
            "N", "O", "Ö", "P", "Q", "R", "S", "Ş", "T", "U", "Ü", "V", "W", "X", "Y", "Z",
            "!", "'", "^" , "+", "%", "&", "/", "(", ")", "=", "?", "_", "£", "#", "$", "½",
            "{", "[", "]", "}", "\\", "|", "*", "-", "~", "`", ",", ";", ".", ":", "<", ">",
            "|", "@", "\"", "é", "€", "i", "¨", "æ", "ß", " "
        };
        public static string Encrypt(string str)
        {
            Random rnd = new Random();
            int sabit = rnd.Next(100, 870);
            char[] dizi = str.ToCharArray();
            string sonuc = "";
            try
            {
                for (int i = 0; i < dizi.Length; i++)
                {
                    int index = Array.IndexOf(charArray, dizi[i].ToString());
                    sonuc += (sabit + index).ToString();
                }
                sonuc += sabit.ToString();
            }
            catch
            {
                return "@HatalıKarakterDizisi";
            }
            return sonuc;
        }
        
        public static string Decrypt(string str)
        {
            string sonuc = "";
            char[] dizi = str.ToCharArray();
            int sabit = Convert.ToInt16(dizi[dizi.Length - 3].ToString() + dizi[dizi.Length - 2].ToString() + dizi[dizi.Length - 1].ToString());
            try
            {
                for (int i = 0; i < dizi.Length - 3; i = i + 3)
                {
                    int index = Convert.ToInt16(dizi[i].ToString() + dizi[i + 1].ToString() + dizi[i + 2].ToString());
                    index -= sabit;
                    if (index != -1)
                        sonuc += charArray[index];
                }
            }
            catch
            {
                return "@HatalıKarakterDizisi";
            }
            return sonuc;
        }
        */

        static string[] charArray_2 = new string[] {
            "1", "a", "i", "r", "z", "H", "P", "X",
            "b", "2", "j", "s", "A", "I", "Q", "Y",
            "c", "k", "3", "ş", "B", "İ", "R", "Z",
            "ç", "l", "t", "4", "C", "J", "S", "ğ",
            "d", "m", "u", "Ç", "5", "K", "Ş",
            "e", "n", "ü", "D", "L", "6", "T",
            "f", "o", "v", "E", "7", "M", "U",
            "g", "ö", "w", "8", "F", "N", "Ü",
            "h", "p", "9", "x", "G", "O", "V",
            "ı", "0", "q", "y", "Ğ", "Ö", "W",
            "!", "'", "^" , "+", "%", "&", "/", "(", ")", "=", "?", "_", "£", "#", "$", "½",
            "{", "[", "]", "}", "\\", "|", "*", "-", "~", "`", ",", ";", ".", ":", "<", ">",
            "|", "@", "\"", "é", "€", "i", "¨", "æ", "ß", " "
        };
        internal static string Encrypt(string str)
        {
            try
            {
                str = str.Trim();

                string result = string.Empty;
                char[] array = str.ToCharArray();
                int random1 = CreateKey();
                int firstIndex = -1;
                int secondIndex = -1;
                int thirdIndex = -1;
                char[] randomArray = random1.ToString().ToCharArray();

                for (int i = 0; i < array.Count(); i++)
                {
                    string chr = array[i].ToString();
                    if (i == firstIndex)
                        result += randomArray[0];
                    else if (i == secondIndex)
                        result += randomArray[1];
                    else if (i == thirdIndex)
                        result += randomArray[2];

                    int index = Array.IndexOf(charArray_2, chr);
                    if (index == -1)
                        return "@Error";
                    index += random1;
                    result += index.ToString();

                    if (i == 0)
                    {
                        firstIndex = Convert.ToInt32(index.ToString().ToCharArray()[0].ToString());
                        secondIndex = Convert.ToInt32(randomArray[0].ToString()) + firstIndex;
                        thirdIndex = Convert.ToInt32(randomArray[1].ToString()) + secondIndex;
                    }
                }

                return result;
            }
            catch
            {
                return "@Error";
            }
        }

        private static int CreateKey()
        {
            try
            {
                int random = new Random().Next(100, 870);
                var array = random.ToString().ToCharArray();
                if (array[0] == '0' || array[1] == '0' || array[2] == '0')
                    return CreateKey();
                return random;
            }
            catch
            {
                return 958;
            }
        }
        /*
        internal static string Decrypt(string str)
        {
            try
            {
                str = str.Trim();

                string result = string.Empty;
                char[] array = str.ToCharArray();

                int firstIndex = Convert.ToInt32(array[0].ToString()) * 3;
                int secondIndex = (Convert.ToInt32(array[firstIndex].ToString()) * 3 + firstIndex) + 1;
                int thirdIndex = (Convert.ToInt32(array[secondIndex].ToString()) * 3 + secondIndex) + 1;

                int random1 = Convert.ToInt16(array[firstIndex].ToString() + array[secondIndex].ToString() + array[thirdIndex].ToString());

                for (int i = 0; i < array.Count(); i += 3)
                {
                    if (i == firstIndex || i == secondIndex || i == thirdIndex)
                    {
                        int index = Convert.ToInt16(array[i + 1].ToString() + array[i + 2].ToString() + array[i + 3].ToString());
                        index -= random1;
                        if (index != -1)
                            result += charArray_2[index].ToString();
                        i += 1;
                    }
                    else if (i + 1 == firstIndex || i + 1 == secondIndex || i + 1 == thirdIndex)
                    {
                        int index = Convert.ToInt16(array[i].ToString() + array[i + 2].ToString() + array[i + 3].ToString());
                        index -= random1;
                        if (index != -1)
                            result += charArray_2[index].ToString();
                        i += 1;
                    }
                    else if (i + 2 == firstIndex || i + 2 == secondIndex || i + 2 == thirdIndex)
                    {
                        int index = Convert.ToInt16(array[i].ToString() + array[i + 1].ToString() + array[i + 3].ToString());
                        index -= random1;
                        if (index != -1)
                            result += charArray_2[index].ToString();
                        i += 1;
                    }
                    else
                    {
                        int index = Convert.ToInt16(array[i].ToString() + array[i + 1].ToString() + array[i + 2].ToString());
                        index -= random1;
                        if (index != -1)
                            result += charArray_2[index].ToString();
                    }
                }
                return result;
            }
            catch
            {
                return "@Error";
            }
        }*/
    }
}
