using System.Runtime.CompilerServices;

namespace DatesAndStuff
{
    public static class DateTimeStringFormatter
    {
        /// <summary>
        /// Use the ISO 8601 date format.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToIsoStringFast(this DateTime dt)
        {
            // yyyy-MM-ddTHH:mm:ss
            Span<char> chars = stackalloc char[19];
            Write4Chars(chars, 0, dt.Year);
            chars[4] = '-';
            Write2Chars(chars, 5, dt.Month);
            chars[7] = '-';
            Write2Chars(chars, 8, dt.Day);
            chars[10] = 'T';
            Write2Chars(chars, 11, dt.Hour);
            chars[13] = ':';
            Write2Chars(chars, 14, dt.Minute);
            chars[16] = ':';
            Write2Chars(chars, 17, dt.Second);
            return new string(chars);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Write4Chars(Span<char> chars, int offset, int value)
        {
            chars[offset] = Digit(value / 1000);
            chars[offset + 1] = Digit(value / 100 % 10);
            chars[offset + 2] = Digit(value / 10 % 10);
            chars[offset + 3] = Digit(value % 10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Write2Chars(Span<char> chars, int offset, int value)
        {
            chars[offset] = Digit(value / 10);
            chars[offset + 1] = Digit(value % 10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char Digit(int value) => (char)(value + '0');
    }
}