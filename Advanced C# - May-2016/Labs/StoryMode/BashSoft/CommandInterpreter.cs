﻿namespace BashSoft
{
    using System.Diagnostics;

    public static class CommandInterpreter
    {
        public static void InterpretCommand(string input)
        {
            string[] data = input.Split();
            string command = data[0];
            switch (command)
            {
                case "open":
                    TryOpenFile(data);
                    break;
                case "mkdir":
                    TryCreateDirectory(data);
                    break;
                case "ls":
                    TryTraverseFolder(data);
                    break;
                case "cmp":
                    TryCompareFiles(data);
                    break;
                case "cdRel":
                    TryChangePathRelatively(data);
                    break;
                case "cdAbs":
                    TryChangePathAbsolute(data);
                    break;
                case "readDb":
                    TryReadDataFromFile(data);
                    break;
                case "help":
                    TryGetHelp(data);
                    break;
                case "show":
                    TryShowWantedData(input, data);
                    break;
                case "filter":
                    // TODO:
                    break;
                case "order":
                    // TODO:
                    break;
                case "decOrder":
                    // TODO:
                    break;
                case "download":
                    // TODO:
                    break;
                case "downloadAsynch":
                    // TODO:
                    break;
                default:
                    DisplayInvalidCommanndMessage(command);
                    break;
            }
        }

        private static void TryShowWantedData(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string courseName = data[1];
                StudentsRepository.GetAllStudentsFromCourse(courseName);
            }
            else if (data.Length == 3)
            {
                string courseName = data[1];
                string userName = data[2];
                StudentsRepository.GetStudentScoresFromCourse(courseName, userName);
            }
            else
            {
                DisplayInvalidCommanndMessage(input);
            }
        }

        private static void TryGetHelp(string[] data)
        {
            CheckParams(data, 1);

            OutputWriter.WriteMessageOnNewLine($"{new string('_', 100)}");
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "make directory - mkdir: path "));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "traverse directory - ls: depth "));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "comparing files - cmp: path1 path2"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "change directory - changeDirREl:relative path"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "change directory - changeDir:absolute path"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "read students data base - readDb: path"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "filter {courseName} excelent/average/poor  take 2/5/all students - filterExcelent (the output is written on the console)"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "order increasing students - order {courseName} ascending/descending take 20/10/all (the output is written on the console)"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "download file - download: path of file (saved in current directory)"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "download file asinchronously - downloadAsynch: path of file (save in the current directory)"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "get help – help"));
            OutputWriter.WriteMessageOnNewLine($"{new string('_', 100)}");
            OutputWriter.WriteEmptyLine();
        }

        private static void TryReadDataFromFile(string[] data)
        {
            CheckParams(data, 2);

            string fileName = data[1];
            StudentsRepository.InitializeData(fileName);
        }

        private static void TryChangePathAbsolute(string[] data)
        {
            CheckParams(data, 2);

            string absolutePath = data[1];
            IOManager.ChangeCurentDirectoryAbsolute(absolutePath);
        }

        private static void TryChangePathRelatively(string[] data)
        {
            CheckParams(data, 2);

            string relPath = data[1];
            IOManager.ChangeCurrentDirectoryRelative(relPath);
        }

        private static void TryCompareFiles(string[] data)
        {
            CheckParams(data, 3);

            string firstPath = data[1];
            string secondPath = data[2];

            Tester.CompareContent(firstPath, secondPath);
        }

        private static void TryTraverseFolder(string[] data)
        {
            if (data.Length == 1)
            {
                IOManager.TraverseDirectory(0);
            }
            else if (data.Length == 2)
            {
                int depth;
                bool hasParsed = int.TryParse(data[1], out depth);
                if (hasParsed)
                {
                    IOManager.TraverseDirectory(depth);
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnableToParseNumber);
                }
            }
        }

        private static void TryCreateDirectory(string[] data)
        {
            CheckParams(data, 2);

            string folderName = data[1];
            IOManager.CreateDirectoryInCurretFolder(folderName);
        }

        private static void TryOpenFile(string[] data)
        {
            CheckParams(data, 2);

            string fileName = data[1];
            Process.Start(SessionData.currentPath + "\\" + fileName);
        }

        private static void DisplayInvalidCommanndMessage(string command)
        {
            OutputWriter.DisplayException(string.Format(ExceptionMessages.InvalidCommand, command));
        }

        private static void CheckParams(string[] data, int length)
        {
            if (data.Length != length)
            {
                OutputWriter.DisplayException($"Invalid number of parameters for the {data[0]} command.");
            }
        }
    }
}