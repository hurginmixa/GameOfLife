using System;
using System.IO;
using System.Reflection;
using GameOfLifeAppl;
using NUnit.Framework;

namespace GameOfLifeTests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Test()
        {
            var testDataPath = TestDataPath();

            Console.WriteLine(testDataPath);

            var files = Directory.GetFiles(testDataPath, "test_*_input.txt");

            foreach (var file in files)
            {
                var testNum = int.Parse(file.Split('_')[1]);

                TestCase(testDataPath, testNum);
            }
        }

        private static string TestDataPath()
        {
            Uri uri = new Uri(Assembly.GetExecutingAssembly().CodeBase, UriKind.Absolute);
            var testDataPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(uri.LocalPath), "..", "TestData"));
            return testDataPath;
        }

        [Test]
        public void TestCases([Values(13, 92, 93)] int testNum)
        {
            var testDataPath = TestDataPath();

            TestCase(testDataPath, testNum);
        }

        private static void TestCase(string testDataPath, int testNum)
        {
            string inpFileName = Path.Combine(testDataPath, $"test_{testNum}_input.txt");
            string actFileName = Path.Combine(testDataPath, $"test_{testNum}_active.txt");
            string expFileName = Path.Combine(testDataPath, $"test_{testNum}_expected.txt");

            File.Delete(actFileName);

            Program.TestCode(inpFileName, actFileName);

            FileAssert.AreEqual(expFileName, actFileName, $"Test Num : {testNum}");
        }
    }
}
