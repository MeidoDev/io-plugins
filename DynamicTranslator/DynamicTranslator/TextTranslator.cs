﻿using BepInEx.Logging;
using Logger = BepInEx.Logger;
using System;
using System.Collections.Generic;
using System.IO;

namespace DynamicTranslator
{
    public sealed class TranslationDomains
    {
        public const string ADV = "adv";

        public static readonly string[] ValidDomains = {
            ADV
        };
    }

    public class TranslatorException : Exception
    {
        public TranslatorException(string message) : base(message)
        {
        }
    }

    public static class TextTranslator
    {
        private const string TF_HEADER = @"###IOTF:1.0###";
        private const string T_SL_DELIM = @" = ";
        private const string T_ML_STARTEND = @"```";
        private const string T_ML_DELIM = @"===";

        private static bool is_initialized = false;
        private static string trans_dir;

        /// <summary>
        /// The translation dictionary.
        /// Format: {domain} => ({original text} => {translated text})
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> dict;

        /// <summary>
        /// A dictionary containing missing translation strings
        /// </summary>
        private static Dictionary<string, List<string>> missing;


        public static void Initialize(string transDir)
        {
            is_initialized = false;

            if (!Directory.Exists(transDir))
                throw new TranslatorException("Invalid translation dictionary path");

            trans_dir = transDir;
            dict = new Dictionary<string, Dictionary<string, string>>();
            missing = new Dictionary<string, List<string>>();

            is_initialized = true;

            LoadTranslations();
        }

        public static void Reload()
        {
            if (!is_initialized)
                throw new TranslatorException("Translator not initialized");

            dict = new Dictionary<string, Dictionary<string, string>>();
            missing = new Dictionary<string, List<string>>();

            LoadTranslations();
        }

        public static string Translate(string domain, string text)
        {
            string translated = text;

            if (is_initialized && text.Length > 0)
            {
                if (dict.ContainsKey(domain) && dict[domain].ContainsKey(text))
                {
                    translated = dict[domain][text];
                }
                else
                {
                    Logger.Log(LogLevel.Debug, string.Format("Missing translation: `{0}`", text));
                    // Save the missing translation if needed
                    if (!missing.ContainsKey(domain))
                    {
                        missing.Add(domain, new List<string>() { text });
                    }
                    else if (!missing[domain].Contains(text))
                    {
                        missing[domain].Add(text);
                    }
                }
            }

            return translated;
        }

        private static void LoadTranslations()
        {
            foreach (string fileName in Directory.GetFiles(trans_dir, "*.txt"))
            {
                ParseTranslationFile(fileName);
            }
        }

        private static void ParseTranslationFile(string filePath)
        {
            Dictionary<string, string> domainDict = new Dictionary<string, string>();

            try
            {
                string domain = Path.GetFileNameWithoutExtension(filePath).ToLower();
                if (Array.IndexOf(TranslationDomains.ValidDomains, domain) == -1)
                {
                    throw new TranslatorException("Invalid domain (filename)");
                }

                using (var reader = new StreamReader(filePath))
                {
                    // Check if the header is present
                    var currentLine = reader.ReadLine();

                    if (currentLine != TF_HEADER)
                        throw new TranslatorException("Missing file header");

                    bool isMultiline = false;
                    bool doCommit = false;
                    int lineNumber = 1;
                    string transFrom = "";
                    string transTo = "";
                    string buffer = "";

                    while ((currentLine = reader.ReadLine())!= null)
                    {
                        lineNumber++;

                        if (currentLine.Length > 0 && currentLine[0] == '#')
                        {
                            // Skip comment lines
                            continue;
                        }

                        if (currentLine.Length == 0)
                        {
                            if (isMultiline)
                                buffer += "\n";
                        }
                        else if (currentLine.StartsWith(T_ML_STARTEND))
                        {
                            if (isMultiline)
                            {
                                // End of translation indicator
                                if (buffer.Length == 0)
                                    throw new TranslatorException(string.Format("Empty multiline block at line {0}", lineNumber));

                                transTo = buffer;
                                buffer = "";
                                isMultiline = false;
                                doCommit = true;
                            }
                            else
                            {
                                // Start of a new multiline block
                                isMultiline = true;
                            }
                        }
                        else if (currentLine.StartsWith(T_ML_DELIM))
                        {
                            if (buffer.Length == 0)
                                throw new TranslatorException(string.Format("Empty multiline block at line {0}", lineNumber));

                            transFrom = buffer;
                            buffer = "";
                        }
                        else if (isMultiline)
                        {
                            buffer += "\n" + currentLine;
                        }
                        else
                        {
                            // Check if it's a valid single line pattern
                            int delimPos = currentLine.IndexOf(T_SL_DELIM);

                            if (delimPos == -1)
                                throw new TranslatorException(string.Format("Invalid data at line {0}", lineNumber));

                            string[] tokens = currentLine.Split(new[] { T_SL_DELIM }, StringSplitOptions.RemoveEmptyEntries);
                            if (tokens.Length != 2)
                                throw new TranslatorException(string.Format("Invalid data at line {0}", lineNumber));

                            transFrom = tokens[0];
                            transTo = tokens[1];
                            doCommit = true;
                        }

                        if (doCommit)
                        {
                            // Write the current multiline data to the dictionary
                            domainDict.Add(transFrom, transTo);
                            transFrom = "";
                            transTo = "";
                            doCommit = false;
                        }
                    }

                    reader.Close();

                    Logger.Log(LogLevel.Debug, string.Format("Loaded {0} translations from file {1}", domainDict.Count, Path.GetFileName(filePath)));

                    dict.Add(domain, domainDict);
                }
            }
            catch (IOException)
            {
                Logger.Log(LogLevel.Error, string.Format("Unable to read translation file '{0}'", Path.GetFileName(filePath)));
            }
            catch (TranslatorException tEx)
            {
                Logger.Log(LogLevel.Debug, string.Format("An error occured while processing translation file '{0}'.\nMessage: {1}", Path.GetFileName(filePath), tEx.Message));
            }
        }
    }
}
