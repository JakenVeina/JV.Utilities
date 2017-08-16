using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Extensions;

namespace JV.Utilities.Tests.Extensions
{
    [TestFixture]
    public class PathExtensionsTests
    {
        /**********************************************************************/
        #region Test Data

        private static readonly string[] TestCases_SinglePaths =
        {
            @"",
            @"C:\Directory1",
            @"C:\Directory1\file1",
            @"C:\Directory1\file1.txt"
        };

        private static readonly string[][] TestCases_TwoPaths = new string[][]
        {
            new string[] { @"C:\Directory1\Directory2\file1", @"C:\Directory1\Directory2\file2", @"C:\Directory1\Directory2" },
            new string[] { @"C:\Directory1\Directory2\file2", @"C:\Directory1\Directory2\file1", @"C:\Directory1\Directory2" },
            new string[] { @"C:\Directory1\Directory2\file1", @"C:\Directory1\Directory3\file1", @"C:\Directory1" },
            new string[] { @"C:\Directory1\Directory3\file1", @"C:\Directory1\Directory2\file1", @"C:\Directory1" },
            new string[] { @"C:\Directory1\Directory2\file1", @"C:\Directory3\Directory2\file1", @"C:\" },
            new string[] { @"C:\Directory3\Directory2\file1", @"C:\Directory1\Directory2\file1", @"C:\" },
            new string[] { @"C:\Directory1\Directory2\file1", @"D:\Directory1\Directory2\file1", @"" },
            new string[] { @"D:\Directory1\Directory2\file1", @"C:\Directory1\Directory2\file1", @"" },
            new string[] { @"C:\Directory1\Directory2\file1", @"C:\Directory1\Directory2", @"C:\Directory1\Directory2" },
            new string[] { @"C:\Directory1\Directory2", @"C:\Directory1\Directory2\file1", @"C:\Directory1\Directory2" },
            new string[] { @"C:\Directory1\Directory2\file1", @"C:\Directory1", @"C:\Directory1" },
            new string[] { @"C:\Directory1", @"C:\Directory1\Directory2\file1", @"C:\Directory1" },
            new string[] { @"C:\Directory1\Directory2\file1", @"C:\", @"C:\" },
            new string[] { @"C:\", @"C:\Directory1\Directory2\file1", @"C:\" },
            new string[] { @"C:\Directory1\Directory2\file1", @"", @"" },
            new string[] { @"", @"C:\Directory1\Directory2\file1", @"" }
        };

        private static readonly string[][] TestCases_ThreePathsWithExpectedResult = new string[][]
        {
            new string[] { @"C:\Directory1\Directory2\file1", @"C:\Directory1\Directory2\file2", @"C:\Directory1\Directory3\file1", @"C:\Directory1" },
            new string[] { @"C:\Directory1\Directory3\file1", @"C:\Directory1\Directory2\file2", @"C:\Directory1\Directory2\file1", @"C:\Directory1" },
        };

        private static readonly string[][] TestCases_PathsContainsNull =
        {
            new string[] { null, null, null },
            new string[] { "A", null, "B" }
        };

        #endregion Test Data

        /**********************************************************************/
        #region Split Tests

        [Test]
        public void Split_PathIsNull_ThrowsException()
        {
            var path = (string)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                PathExtensions.Split(path);
            });

            result.ParamName.ShouldBe(nameof(path));
        }

        [TestCase(@"A<")]
        [TestCase(@"A>")]
        public void Split_PathContainsInvalidChars_ThrowsException(string path)
        {
            var result = Should.Throw<ArgumentException>(() =>
            {
                PathExtensions.Split(path);
            });

            result.ParamName.ShouldBe(nameof(path));
        }

        [TestCaseSource(nameof(TestCases_SinglePaths))]
        public void Split_PathIsValid_PathCombineOnResultMatchesPath(string path)
        {
            var result = PathExtensions.Split(path);

            Path.Combine(result).ShouldBe(path);
        }

        #endregion Split Tests

        /**********************************************************************/
        #region GetCommonPath Tests

        [Test]
        public void GetCommonPath_Enumerable_PathsIsNull_ThrowsException()
        {
            var paths = (IEnumerable<string>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                PathExtensions.GetCommonPath(paths);
            });

            result.ParamName.ShouldBe(nameof(paths));
        }

        [Test]
        public void GetCommonPath_Enumerable_PathsIsEmpty_ReturnsEmpty()
        {
            var paths = Enumerable.Empty<string>();

            PathExtensions.GetCommonPath(paths).ShouldBeEmpty();
        }

        [TestCaseSource(nameof(TestCases_PathsContainsNull))]
        public void GetCommonPath_Enumerable_PathsContainsNull_ThrowsException(string x, string y, string z)
        {
            var paths = (new string[] { x, y, z }).AsEnumerable();

            var result = Should.Throw<ArgumentException>(() =>
            {
                PathExtensions.GetCommonPath(paths);
            });

            result.ParamName.ShouldBe(nameof(paths));
        }

        [TestCaseSource(nameof(TestCases_SinglePaths))]
        public void GetCommonPath_Enumerable_PathsContainsSingle_ReturnsSingle(string path)
        {
            var paths = path.MakeEnumerable();

            PathExtensions.GetCommonPath(paths).ShouldBe(path);
        }

        [TestCaseSource(nameof(TestCases_SinglePaths))]
        public void GetCommonPath_Enumerable_PathsAreEqual_ReturnsSamePath(string path)
        {
            var paths = (new[] { path, path }).AsEnumerable();

            PathExtensions.GetCommonPath(paths).ShouldBe(path);
        }

        [TestCaseSource(nameof(TestCases_TwoPaths))]
        public void GetCommonPath_Enumerable_TwoDifferentPaths_ReturnsExpected(string path1, string path2, string expectedResult)
        {
            var paths = (new[] { path1, path2 }).AsEnumerable();

            PathExtensions.GetCommonPath(paths).ShouldBe(expectedResult);
        }

        [TestCaseSource(nameof(TestCases_ThreePathsWithExpectedResult))]
        public void GetCommonPath_Enumerable_ThreeDifferentPaths_ReturnsExpected(string path1, string path2, string path3, string expectedResult)
        {
            var paths = (new[] { path1, path2, path3 }).AsEnumerable();

            PathExtensions.GetCommonPath(paths).ShouldBe(expectedResult);
        }

        [Test]
        public void GetCommonPath_Array_PathsIsNull_ThrowsException()
        {
            var paths = (string[])null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                PathExtensions.GetCommonPath(paths);
            });

            result.ParamName.ShouldBe(nameof(paths));
        }

        [Test]
        public void GetCommonPath_Array_PathsIsEmpty_ReturnsEmpty()
        {
            var paths = new string[0];

            PathExtensions.GetCommonPath(paths).ShouldBeEmpty();
        }

        [TestCaseSource(nameof(TestCases_PathsContainsNull))]
        public void GetCommonPath_Array_PathsContainsNull_ThrowsException(string x, string y, string z)
        {
            var paths = new string[] { x, y, z };

            var result = Should.Throw<ArgumentException>(() =>
            {
                PathExtensions.GetCommonPath(paths);
            });

            result.ParamName.ShouldBe(nameof(paths));
        }

        [TestCaseSource(nameof(TestCases_SinglePaths))]
        public void GetCommonPath_Array_PathsContainsSingle_ReturnsSingle(string path)
        {
            var paths = new[] { path };

            PathExtensions.GetCommonPath(paths).ShouldBe(path);
        }

        [TestCaseSource(nameof(TestCases_SinglePaths))]
        public void GetCommonPath_Array_PathsAreEqual_ReturnsSamePath(string path)
        {
            var paths = new[] { path, path };

            PathExtensions.GetCommonPath(paths).ShouldBe(path);
        }

        [TestCaseSource(nameof(TestCases_TwoPaths))]
        public void GetCommonPath_Array_TwoDifferentPaths_ReturnsExpected(string path1, string path2, string expectedResult)
        {
            var paths = new[] { path1, path2 };

            PathExtensions.GetCommonPath(paths).ShouldBe(expectedResult);
        }

        [TestCaseSource(nameof(TestCases_ThreePathsWithExpectedResult))]
        public void GetCommonPath_Array_ThreeDifferentPaths_ReturnsExpected(string path1, string path2, string path3, string expectedResult)
        {
            var paths = new[] { path1, path2, path3 };

            PathExtensions.GetCommonPath(paths).ShouldBe(expectedResult);
        }

        #endregion GetCommonPath Tests
    }
}
