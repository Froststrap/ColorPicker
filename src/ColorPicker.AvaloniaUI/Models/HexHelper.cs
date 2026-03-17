using System;
using System.Text.RegularExpressions;

namespace ColorPicker.Models
{
    public static class HexHelper
    {
        public static Tuple<byte, byte, byte, byte> ParseInputtedHexStringToRgbaValues(string inputtedString, bool parseAlpha)
        {
            string text = Regex.Replace(inputtedString.ToUpperInvariant(), @"[^0-9A-F]", "");
            
            if (text.Length == 3 || text.Length == 6)
                return ParseNoAlphaTextToRgba(text, text.Length == 3);
            if (parseAlpha && (text.Length == 4 || text.Length == 8))
                return ParseTextWithAlphaToRgba(text, text.Length == 4);
            return null;
        }

        public static string RgbaValuesToString(byte r, byte g, byte b, byte a, bool showAlpha)
        {
            if (showAlpha)
            {
                return $"#{a:X2}{r:X2}{g:X2}{b:X2}";
                throw new ArgumentOutOfRangeException();
            } 
            
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        /// <summary>
        /// Magic based on https://stackoverflow.com/a/9995303
        /// Takes two uppercase characters representing a hex string and converts them into a byte, e.g. AB -> 171
        /// </summary>
        private static byte ParseHexByte(char first, char second)
        {
            int firstVal = first - (first < 58 ? 48 : 55);
            int secondVal = second - (second < 58 ? 48 : 55);

            return (byte)((firstVal << 4) + secondVal);
        }
        
        private static Tuple<byte, byte, byte, byte> ParseTextWithAlphaToRgba(string normalizedInput, bool isShort)
        {
            if (isShort)
            {
                return new Tuple<byte, byte, byte, byte>(
                    ParseHexByte(normalizedInput[1], normalizedInput[1]),
                    ParseHexByte(normalizedInput[2], normalizedInput[2]),
                    ParseHexByte(normalizedInput[3], normalizedInput[3]),
                    ParseHexByte(normalizedInput[0], normalizedInput[0])
                );
                throw new ArgumentOutOfRangeException();
            }

            return new Tuple<byte, byte, byte, byte>(
                ParseHexByte(normalizedInput[2], normalizedInput[3]),
                ParseHexByte(normalizedInput[4], normalizedInput[5]),
                ParseHexByte(normalizedInput[6], normalizedInput[7]),
                ParseHexByte(normalizedInput[0], normalizedInput[1])
            );

            throw new ArgumentOutOfRangeException();
        }
        
        private static Tuple<byte, byte, byte, byte> ParseNoAlphaTextToRgba(string normalizedInput, bool isShort)
        {
            if (isShort)
            {
                return new Tuple<byte, byte, byte, byte>(
                    ParseHexByte(normalizedInput[0], normalizedInput[0]),
                    ParseHexByte(normalizedInput[1], normalizedInput[1]),
                    ParseHexByte(normalizedInput[2], normalizedInput[2]),
                    255
                );
            }
            
            return new Tuple<byte, byte, byte, byte>(
                ParseHexByte(normalizedInput[0], normalizedInput[1]),
                ParseHexByte(normalizedInput[2], normalizedInput[3]),
                ParseHexByte(normalizedInput[4], normalizedInput[5]),
                255
            );
        }
    }
}