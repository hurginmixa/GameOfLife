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

            Assert.Multiple(() =>
            {
                foreach (var file in files)
                {
                    var testNum = int.Parse(file.Split('_')[1]);

                    Console.Write($"Test {testNum}");

                    try
                    {
                        TestCase(testDataPath, testNum);
                        Console.WriteLine(" - Ok");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(" - Fail");
                        throw;
                    }

                }
            });
        }

        private static string TestDataPath()
        {
            Uri uri = new Uri(Assembly.GetExecutingAssembly().CodeBase, UriKind.Absolute);
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(uri.LocalPath), "..", "TestData"));
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
