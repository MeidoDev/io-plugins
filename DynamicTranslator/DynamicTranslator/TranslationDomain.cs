using BepInEx.Logging;
using Logger = BepInEx.Logger;
using System.Collections.Generic;

namespace DynamicTranslator
{
    public static class TranslationDomain
    {
        public const string D_DISABLED = "#disabled";
        public const string D_ADV = "adv";
        public const string D_LABEL = "label";

        public static readonly string[] ValidDomains = {
            D_ADV,
            D_LABEL
        };

        /// <summary>
        /// Dictionaries that map UILabel components to a domain
        /// </summary>
        private static Dictionary<int, string> LabelHashMap;
        private static Dictionary<string, string> LabelDomainMap;

        public static void Initialize()
        {
            LabelHashMap = new Dictionary<int, string>();

            // List all known labels here. Those that should not get
            // auto-translated (i.e. ADV or numeric labels) should
            // get the DISABLED domain assigned
            LabelDomainMap = new Dictionary<string, string>() {
                // Blacklisted Labels go here
                { @"Slider\Label", D_DISABLED},                 // Text Progress indicator
                { @"Conf_TextBG\Text_Sampl", D_DISABLED},       // Config:TextSettings:ExampleText
                { @"UI Root (ADV)\Text_Name", D_DISABLED},      // ADV Screen Name
                { @"UI Root (ADV)\Text_Text", D_DISABLED},      // ADV Screen Text
            };
        }

        public static string GetLabelTranslationDomain(UILabel label)
        {
            string domain;

            // Try to find the domain via reference
            if (!LabelHashMap.TryGetValue(label.GetHashCode(), out domain))
            {
                // Generate the identifier
                string labelIdentifier = string.Format("{0}\\{1}", (label.parent ? label.parent.name : ""), label.name);

                if (!LabelDomainMap.TryGetValue(labelIdentifier, out domain))
                {
                    // Assign the default label domain
                    domain = D_LABEL;
                }

                // Store the lookup in the hash dictionary
                LabelHashMap.Add(label.GetHashCode(), domain);
            }

            return domain;
        }
    }
}
