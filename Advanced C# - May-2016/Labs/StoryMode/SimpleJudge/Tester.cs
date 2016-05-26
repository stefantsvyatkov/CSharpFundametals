﻿namespace SimpleJudge
{
    using BashSoft;
    using System.IO;
    using System;

    public static class Tester
    {
        public static void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            OutputWriter.WriteMessageOnNewLine("Reading files...");

            string mismatchPath = GetMismatchPath(expectedOutputPath);

            string[] actualOutput = File.ReadAllLines(userOutputPath);
            string[] expectedOutput = File.ReadAllLines(expectedOutputPath);

            bool hasMismatch;
            string[] mismatches = GetLinesWithPossibleMismatches(actualOutput, expectedOutput, out hasMismatch);

            PrintOutput(mismatches, hasMismatch, mismatchPath);
        }

        private static void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchPath)
        {
            if (hasMismatch)
            {
                foreach (var line in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(line);
                }

                File.WriteAllLines(mismatchPath, mismatches);
                return;
            }

            OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
        }

        private static string[] GetLinesWithPossibleMismatches(string[] actualOutput, string[] expectedOutput, out bool hasMismatch)
        {
            hasMismatch = false;
            string output = string.Empty;

            string[] mismatches = new string[actualOutput.Length];
            OutputWriter.WriteMessageOnNewLine("Comparing files...");

            for (int i = 0; i < actualOutput.Length; i++)
            {
                string actualLine = actualOutput[i];
                string expectedLine = expectedOutput[i];

                if (actualLine.Equals(expectedLine))
                {
                    output = string.Format("Mismatch at line {0} -- expected \"{1}\", actual: \" {2}\"",
                        i, expectedLine, actualLine);
                    output += Environment.NewLine;

                    hasMismatch = true;
                }
                else
                {
                    output = actualLine;
                    output += Environment.NewLine;
                }

                mismatches[i] = output;
            }

            return mismatches;
        }

        public static string GetMismatchPath(string expectedOutputPath)
        {
            int indexOf = expectedOutputPath.IndexOf('\\');
            string directoryPath = expectedOutputPath.Substring(0, indexOf);
            string finalPath = directoryPath + @"\Mismatches.txt";

            return finalPath;
        }
    }
}