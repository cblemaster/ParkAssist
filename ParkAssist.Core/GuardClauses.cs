﻿namespace ParkAssist.Core
{
    public static class GuardClauses
    {
        public static bool IsNotNull<T>(T obj) where T : class => obj != null;
        public static bool StringContainsChars(string str) => !string.IsNullOrWhiteSpace(str);
        public static bool StringLengthInValidRangeInclusive(int min, int max, string str) => str.Length >= min && str.Length <= max;
        public static bool StringLengthInValidRangeExclusive(int min, int max, string str) => str.Length > min && str.Length < max;
        public static bool IdGreaterThanZero(int id) => id > 0;
        public static bool DecimalGreaterThanZero(decimal dec) => dec > 0M;
    }
}
