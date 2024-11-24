using System;
using System.Collections.Generic;
using System.Linq;

namespace Asp_Web_Lib.Helpers
{
    public static class CultureHelper
    {
        // Lista obsługiwanych kultur
        private static readonly List<string> _cultures = new List<string> { "pl", "en" };

        // Zwraca obsługiwaną kulturę lub domyślną
        public static string GetImplementedCulture(string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
                return GetDefaultCulture();

            if (!_cultures.Contains(cultureName))
                return GetDefaultCulture();

            return cultureName;
        }

        public static string GetDefaultCulture()
        {
            return _cultures[0]; // "pl" jest domyślną kulturą
        }

        // Metoda opcjonalna: sprawdza, czy kultura jest obsługiwana
        public static bool IsValidCulture(string cultureName)
        {
            return _cultures.Contains(cultureName);
        }
    }
}