using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;
using System.Text.RegularExpressions;

namespace Borman.ZuSharp
{
    public class Layout
    {
        public string Name { get; set; }
        public List<KeyCombination> Combinations { get; set; }
        public Dictionary<string, KeyCombination> CombinationsDict { get; set; }
        public int MaxCompinationLength { get; set; }

        public static Layout LoadLayout(string fileName)
        {
            Layout result = new Layout
            {
                Name = "Generic Layout",
                Combinations = new List<KeyCombination>()
            };

            using (StreamReader file = new System.IO.StreamReader(fileName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (line == null)
                    {
                        continue;
                    }

                    // get rid of comment
                    line = Regex.Replace(line, "//.*", "");

                    line = line.Trim();

                    // skip blank lines
                    if (String.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    // read name
                    string namePragma = "#Name";

                    if (line.StartsWith(namePragma))
                    {
                        if (line.Length > namePragma.Length)
                        {
                            result.Name = line.Substring(namePragma.Length).Trim();
                        }

                        continue;
                    }


                    Match m = Regex.Match(line, @"^(\s*(?<key>\w+))(\s*\+\s*(?<key>\w+))*\s*:\s*(?<upperChar>\S)\s*,(?<charNoCaps>,)?\s*(?<lowerChar>\S)\s*(\-\>\s*(?<escapedUpperChar>\S)\s*,(?<escapedCharNoCaps>,)?\s*(?<escapedLowerChar>\S))?\s*$", RegexOptions.Compiled);

                    if (!m.Success)
                    {
                        throw new Exception("Error parsing layout line: " + line);
                    }

                    KeyCombination combination = new KeyCombination
                    {
                        IsCharacterCapsLockSensitive = !m.Groups["charNoCaps"].Success,
                        CharacterUpperCase = m.Groups["upperChar"].Value[0],
                        CharacterLowerCase = m.Groups["lowerChar"].Value[0],
                        IsEscapedCharacterCapsLockSensitive = !m.Groups["escapedCharNoCaps"].Success,
                        EscapedCharacterUpperCase = m.Groups["escapedUpperChar"].Success ? (char?)m.Groups["escapedUpperChar"].Value[0] : null,
                        EscapedCharacterLowerCase = m.Groups["escapedLowerChar"].Success ? (char?)m.Groups["escapedLowerChar"].Value[0] : null,
                        Keys = new List<Key>()
                    };

                    foreach (var capture in m.Groups["key"].Captures)
                    {
                        string keyString = capture.ToString();
                        Key key = (Key)Enum.Parse(typeof(Key), keyString, true);
                        combination.Keys.Add(key);
                    }

                    if (combination.Keys.Count > 2)
                    {
                        throw new Exception("Key combinations of more that 2 keys are not supported.");
                    }

                    result.Combinations.Add(combination);

                }

            }


            result.MaxCompinationLength = result.Combinations.Max<KeyCombination>(k => k.Keys.Count);

            try
            {
                result.CombinationsDict = result.Combinations.ToDictionary<KeyCombination, string>(k => k.KeyCombinationHash);
            } catch{
                throw new Exception("Duplication key combinations are not allowed");
            }

            return result;
        }
    }

    public class KeyCombination
    {
        public List<Key> Keys { get; set; }

        public bool IsCharacterCapsLockSensitive { get; set; }
        public char CharacterUpperCase { get; set; }
        public char CharacterLowerCase { get; set; }

        public bool IsEscapedCharacterCapsLockSensitive { get; set; }
        public char? EscapedCharacterUpperCase { get; set; }
        public char? EscapedCharacterLowerCase { get; set; }

        public string KeyCombinationHash
        {
            get
            {
                return GetKeyCombinationHash(this.Keys);
            }
        }

        public static string GetKeyCombinationHash(List<Key> keys)
        {
            return String.Join(" + ", keys);
        }
    }
}
