using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Procezor.PayrolexTestData
{
    class TestSpec
    {
        public string TestCase { get; private set; }
        public string TestName { get; private set; }

        public decimal ParseTestCase(string val)
        {
            TestCase = val;
            return 0m;
        }
        public decimal ParseTestName(string val)
        {
            TestName = val;
            return 0m;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            const string TEST_FOLDER_NAME = "test_data";
            string testFolderNameFull = ExecutableTestFolder(TEST_FOLDER_NAME);
            Console.WriteLine(testFolderNameFull);

            const string TEST_TARGET_FILE_NAME = "TestPayrolex-ExportVstupy-202001.csv";
            string testTargetFileNameFull = Path.Combine(testFolderNameFull, TEST_TARGET_FILE_NAME);
            const string TEST_RESULT_PRAC_FILE_NAME = "TestPayrolex-PracVysledky-202001.csv";
            string testResultPracFileNameFull = Path.Combine(testFolderNameFull, TEST_RESULT_PRAC_FILE_NAME);
            const string TEST_RESULT_PPOM_FILE_NAME = "TestPayrolex-PPomVysledky-202001.csv";
            string testResultPPomFileNameFull = Path.Combine(testFolderNameFull, TEST_RESULT_PPOM_FILE_NAME);
            const string PROT_FILE_NAME = "TestPuzzle-CodeLines-202001.txt";
            string testProtNameFull = Path.Combine(testFolderNameFull, PROT_FILE_NAME);

            var testTargetList = new List<Tuple<string, string, string[]>>();
            var testResultList = new List<Tuple<string, string, string[]>>();

            using (var streamTargets = new FileStream(testTargetFileNameFull, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var testTargets = new StreamReader(streamTargets, Encoding.GetEncoding("windows-1250")))
                {
                    while (testTargets.EndOfStream == false)
                    {
                        string targetString = testTargets.ReadLine();
                        string[] targetDefValues = targetString.Split(';');

                        if (targetDefValues.Length == 0)
                        {
                            continue;
                        }
                        if (targetDefValues.Length > 0 && targetDefValues[0] == "EmployeeNumb")
                        {
                            continue;
                        }
                        TestSpec testSpec = new TestSpec();
                        Func<string, decimal>[] targetParser = new Func<string, decimal>[]
                        {
                            testSpec.ParseTestCase, //Evideční číslo  	101
                            testSpec.ParseTestName, //Jméno a příjmení 	Drahota Jakub
                            ParseDecimal,
                        };
                        decimal[] targetDecValues = targetDefValues.Zip(targetParser).Select((x) => x.Second(x.First)).ToArray();
                        Func<decimal, string>[] targetGenerator = new Func<decimal, string>[]
                        {
                            CodeTargetStarts,          // A0 Evideční číslo  	101
                            WithNADecimalVal,          // B0 Jméno a příjmení 	Drahota Jakub
                            WithTargetTestsPeriodCode, // C0 Mzdové období 	    202201
                        };
                        var targetCodeLines = targetDecValues.Zip(targetGenerator)
                            .Select((x) => x.Second(x.First))
                            .Where((x) => (string.IsNullOrEmpty(x) == false));

                        testTargetList.Add(new Tuple<string, string, string[]>(testSpec.TestCase, testSpec.TestName,
                            targetCodeLines.Where((x) => (string.IsNullOrEmpty(x) == false)).ToArray()));
                    }
                }
            }

            using (var streamResults = new FileStream(testResultPracFileNameFull, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var testResults = new StreamReader(streamResults, Encoding.GetEncoding("windows-1250")))
                {
                    while (testResults.EndOfStream == false)
                    {
                        string resultString = testResults.ReadLine();
                        string[] resultDefValues = resultString.Split(';');

                        if (resultDefValues.Length == 0)
                        {
                            continue;
                        }
                        if (resultDefValues.Length > 0 && resultDefValues[0] == "EmployeeNumb")
                        {
                            continue;
                        }
                        TestSpec testSpec = new TestSpec();
                        Func<string, decimal>[] resultParser = new Func<string, decimal>[]
                        {
                        testSpec.ParseTestCase, //Evideční číslo  	101
                        testSpec.ParseTestName, //Jméno a příjmení 	Drahota Jakub
                        ParseDecimal,   //Mzdové období 	202201
                        };
                        decimal[] resultDecValues = resultDefValues.Zip(resultParser).Select((x) => x.Second(x.First)).ToArray();

                        Func<decimal, string>[] resultGenerator = new Func<decimal, string>[]
                        {
                        CodeResultStarts,          //Evideční číslo  	101
                        WithNADecimalVal,          //Jméno a příjmení 	Drahota Jakub
                        WithResultTestsPeriodCode,   //Mzdové období 	    202201
                        };
                        var resultCodeLines = resultDecValues.Zip(resultGenerator).Select((x) => x.Second(x.First)).ToArray();

                        testResultList.Add(new Tuple<string, string, string[]>(testSpec.TestCase, testSpec.TestName,
                            resultCodeLines.Where((x) => (string.IsNullOrEmpty(x) == false)).ToArray()));
                    }
                }
            }
            using (var streamProtokol = new FileInfo(testProtNameFull).Create())
            {
                var testCaseList = testTargetList.Zip(testResultList).Select((x, idx) =>
                    (idx + 1, x.First.Item1, x.First.Item2, x.First.Item3, x.Second.Item3)).ToArray();

                using (var testProtokol = new StreamWriter(streamProtokol, System.Text.Encoding.GetEncoding("windows-1250")))
                {
                    foreach (var testCase in testCaseList)
                    {
                        testProtokol.WriteLine($"PayrolexGenerator.Spec({testCase.Item1}, \"{testCase.Item3}\", \"{testCase.Item2}\")");

                        testProtokol.WriteLine(string.Join('\n', testCase.Item4));
                        testProtokol.WriteLine(string.Join('\n', testCase.Item5) + ",");
                    }
                }
            }
        }

        private static string CodeTargetStarts(decimal val)
        {
            return "// Begin Test's Targets";
        }
        private static string CodeResultStarts(decimal val)
        {
            return "// Begin Test's Results";
        }
        private static string WithNADecimalVal(decimal val)
        {
            return "";
        }
        private static string WithTargetTestsPeriodCode(decimal val) { return ""; }
        private static string WithResultTestsPeriodCode(decimal val) { return ""; }

        private static string TestResultCode(string function, decimal val, bool always = false)
        {
            if (always == false && val == 0)
            {
                return "";
            }
            var valCode = $"{val}".Replace(',', '.');
            return $"{function}({valCode}m)";
        }

        private static string TestTargetCode(string function, decimal val, bool always = false)
        {
            if (always == false && val == 0)
            {
                return "";
            }
            Int32 valParam = decimal.ToInt32(val);
            return $"{function}({valParam})";
        }

        private static string TestTargetX100(string function, decimal val, bool always = false)
        {
            if (always == false && val == 0)
            {
                return "";
            }
            Int32 valInt = decimal.ToInt32(val);
            Int32 val100 = decimal.ToInt32(val * 100);
            Int32 valDec = val100 - (valInt * 100);
            if (valDec != 0)
            {
                return $"{function}({valInt} * 100 + {valDec})";
            }
            return $"{function}({valInt} * 100)";
        }
        private static string TestTargetX060(string function, decimal val, bool always = false)
        {
            if (always == false && val == 0)
            {
                return "";
            }
            Int32 valInt = decimal.ToInt32(val);
            Int32 val060 = decimal.ToInt32(val * 60);
            Int32 valDec = val060 - (valInt * 60);
            if (valDec != 0)
            {
                return $"{function}({valInt} * 60 + {valDec})";
            }
            return $"{function}({valInt} * 60)";
        }

        private static decimal ParseDecimal(string valString)
        {
            if (valString.Trim().Equals(""))
            {
                return 0;
            }
            string numberToParse = valString.Replace('.', ',').TrimEnd('%').Replace("Kč", "").TrimEnd(' ');
            decimal numberValue = 0;
            try
            {
                numberValue = decimal.Parse(numberToParse);
            }
            catch (Exception e)
            {
            }
            return (numberValue);
        }

        private static string ExecutableTestFolder(string folderName)
        {
            const string PARENT_FOLDER_NAME = "Procezor.PayrolexTestData";

            string[] args = Environment.GetCommandLineArgs();

            string appExecutableFileNm = args[0];

            string currPath = Path.GetDirectoryName(appExecutableFileNm);
            if (string.IsNullOrEmpty(currPath))
            {
                return "";
            }
            int nameCount = currPath.Split(Path.DirectorySeparatorChar).Length;

            while (!currPath.EndsWith(PARENT_FOLDER_NAME) && nameCount != 1)
            {
                currPath = Path.GetDirectoryName(currPath);
            }
            string basePath = Path.Combine(currPath, folderName);
            if (nameCount <= 1)
            {
                basePath = Path.Combine(Path.GetFullPath("."), folderName);
            }
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            string normPath = Path.GetFullPath(basePath);

            return normPath;
        }
    }
}
