namespace ParkAssist.Core
{
    public class GuardClauses
    {
        public bool IsNotNull<T>(T obj) where T : class
        {
            return obj != null;
        }
        public bool StringContainsChars(string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
        public bool StringLengthInValidRangeInclusive(int min, int max, string str)
        {
            return str.Length >= min && str.Length <= max;
        }
        public bool IdGreaterThanZero(int id)
        {
            return id > 0;
        }
        public bool DecimalGreaterThanZero(decimal dec)
        {
            return dec > 0M;
        }
    }
}
