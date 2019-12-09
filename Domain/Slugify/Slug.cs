using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Slugify
{
    public static class Slug
    {
        public static SlugOptions Config;

        public static string GenerateSlug(this string inputString)
        {
            if (Config.ForceLowerCase)
            {
                inputString = inputString.ToLower();
            }

            if (Config.TrimWhitespace)
            {
                inputString = inputString.Trim();
            }

            inputString = CleanWhiteSpace(inputString, Config.CollapseWhiteSpace);
            inputString = ApplyReplacements(inputString, Config.StringReplacements);
            inputString = RemoveDiacritics(inputString);
            inputString = DeleteCharacters(inputString, Config.DeniedCharactersRegex);

            if (Config.CollapseDashes)
            {
                inputString = Regex.Replace(inputString, "--+", "-");
            }

            return inputString;
        }

        private static string CleanWhiteSpace(string str, bool collapse)
        {
            return Regex.Replace(str, collapse ? @"\s+" : @"\s", " ");
        }

        private static string RemoveDiacritics(string str)
        {
            var stFormD = str.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string ApplyReplacements(string str, Dictionary<string, string> replacements)
        {
            var sb = new StringBuilder(str);

            foreach (KeyValuePair<string, string> replacement in replacements)
            {
                sb = sb.Replace(replacement.Key, replacement.Value);
            }

            return sb.ToString();
        }

        private static string DeleteCharacters(string str, string regex)
        {
            return Regex.Replace(str, regex, "");
        }
    }
}
