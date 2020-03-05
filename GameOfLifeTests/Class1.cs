using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GameOfLifeAppl;
using NUnit.Framework;

namespace GameOfLifeTests
{
    [TestFixture]
    public class Class1
    {
        [TestCaseSource(typeof(Class1), nameof(TestCases))]
        public void RunTest(int testNum)
        {
            string testDataPath = TestDataPath();

            string inpFileName = Path.Combine(testDataPath, $"test_{testNum}_input.txt");
            string actFileName = Path.Combine(testDataPath, $"test_{testNum}_active.txt");
            string expFileName = Path.Combine(testDataPath, $"test_{testNum}_expected.txt");

            File.Delete(actFileName);

            Program.TestCode(inpFileName, actFileName);

            FileAssert.AreEqual(expFileName, actFileName);
        }

        static IEnumerable<TestCaseData> TestCases()
        {
            var testDataPath = TestDataPath();

            Console.WriteLine(testDataPath);

            var files = Directory.GetFiles(testDataPath, "test_*_input.txt");

            foreach (var file in files)
            {
                var testNum = int.Parse(file.Split('_')[1]);

                yield return new TestCaseData(testNum);
            }
        }

        private static string TestDataPath()
        {
            Uri uri = new Uri(Assembly.GetExecutingAssembly().CodeBase, UriKind.Absolute);
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(uri.LocalPath), "..", "TestData"));
        }
    }
}
