namespace AstroCue.Test.Utilities
{
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Server.Utilities;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StringUtilitiesTests
    {
        /// <summary>
        /// Tests the <see cref="StringUtilities.TrimToUpperFirstChar"/> method
        /// </summary>
        [TestMethod]
        public void TrimToUpperFirstCharTest()
        {
            // Arrange
            string emptyTest = string.Empty;
            const string test2 = "   test case 2 ";
            const string test3 = "Test 3";

            string emptyTestExpected = string.Empty;
            const string test2Expected = "Test case 2";
            const string test3Expected = "Test 3";

            // Act
            string emptyTestResult = StringUtilities.TrimToUpperFirstChar(emptyTest);
            string test2Result = StringUtilities.TrimToUpperFirstChar(test2);
            string test3Result = StringUtilities.TrimToUpperFirstChar(test3);

            // Assert
            emptyTestResult.Should().Be(emptyTestExpected);
            test2Result.Should().Be(test2Expected);
            test3Result.Should().Be(test3Expected);
        }

        /// <summary>
        /// Tests the <see cref="StringUtilities.TrimToLowerAll"/> method
        /// </summary>
        [TestMethod]
        public void TrimToLowerAllTest()
        {
            // Arrange
            string emptyTest = string.Empty;
            const string test2 = "     EXAmplE@teSt.Com    ";
            const string test3 = "example@test.com";

            string emptyTestExpected = string.Empty;
            const string test2Expected = "example@test.com";
            const string test3Expected = "example@test.com";

            // Act
            string emptyTestResult = StringUtilities.TrimToLowerAll(emptyTest);
            string test2Result = StringUtilities.TrimToLowerAll(test2);
            string test3Result = StringUtilities.TrimToLowerAll(test3);

            // Assert
            emptyTestResult.Should().Be(emptyTestExpected);
            test2Result.Should().Be(test2Expected);
            test3Result.Should().Be(test3Expected);
        }
    }
}
