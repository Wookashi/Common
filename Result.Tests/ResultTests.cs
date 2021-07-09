using NUnit.Framework;
using Wookashi.Common.Result;
using Wookashi.Common.Result.Enums;

namespace Wookashi.Common.Result.Tests
{
    [TestFixture]
    public class ResultTests
    {
        [Test]
        [Category("Unit")]
        public void Constructor_CreatesResultWithErrorEntry()
        {
            // arrange
            const string message = "Error";

            // act
            var result = Result.Error(message);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.MessageTemplate, Is.EqualTo(message));
                Assert.That(result.IsError, Is.EqualTo(true));
            });
        }

        [Test]
        [Category("Unit")]
        public void IsError_ReturnFalseForWarning()
        {
            // arrange

            var result = Result.Warning("Warning");

            // act
            var isError = result.IsError;

            // assert
            Assert.AreEqual(false, isError);
        }

        [Test]
        [Category("Unit")]
        public void IsError_ReturnTrueForError()
        {
            // arrange

            var result = Result.Error("Error");

            // act
            var isError = result.IsError;

            // assert
            Assert.AreEqual(true, isError);
        }

        [Test]
        [Category("Unit")]
        public void IsWarning_ReturnFalseForError()
        {
            // arrange

            var result = Result.Error("Error");

            // act
            var isWarning = result.IsWarning;

            // assert
            Assert.AreEqual(false, isWarning);
        }

        [Test]
        [Category("Unit")]
        public void IsWarning_ReturnTrueForWarning()
        {
            // arrange

            var result = Result.Warning("Warning");

            // act
            var isWarning = result.IsWarning;

            // assert
            Assert.AreEqual(true, isWarning);
        }

        [Test]
        [Category("Unit")]
        public void IsSuccess_ReturnTrueWhenNoMessages()
        {
            // arrange
            var result = Result.Success;

            // act & assert
            Assert.AreEqual(true, result.IsSuccess);
        }

        [Test]
        [Category("Unit")]
        public void IsSuccess_ReturnFalseForWarning()
        {
            // arrange
            var result = Result.Warning("uwaga");

            // act & assert
            Assert.AreEqual(false, result.IsSuccess);
        }

        [Test]
        [Category("Unit")]
        public void IsSuccess_ReturnFalseForError()
        {
            // arrange
            var result = Result.Error("Error");

            // act & assert
            Assert.AreEqual(false, result.IsSuccess);
        }

        [Test]
        [Category("Unit")]
        public void Message_CreatesResultWithMessageEntry()
        {
            // arrange
            const string message = "Informacja";

            // act
            var result = Result.SuccessWithMessage(message);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.EqualTo(true));
                Assert.That(result.ToString, Is.EqualTo(message));
            });
        }

        [Test]
        [Category("Unit")]
        public void Warning_CreatesResultWithWarningEntry()
        {
            // arrange
            const string message = "Warning";

            // act
            var result = Result.Warning(message);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsWarning, Is.EqualTo(true));
                Assert.That(result.ToString, Is.EqualTo(message));
            });
        }

        [Test]
        [Category("Unit")]
        [TestCase("Warning", "Warning")]
        public void ToString_WithoutParameters(string message, string expected)
        {
            // arrange
            var result = Result.SuccessWithMessage(message, null);

            // act
            // assert
            Assert.That(result.ToString, Is.EqualTo(expected));
        }

        [Test]
        [Category("Unit")]
        [TestCase("Test message {test} {test} {test}", "Test1Value", "Test2Value", "Test3Value", "Test message Test1Value Test2Value Test3Value")]
        [TestCase("Test message {Test2} {Test1} {Test3}", "Test1Value", "Test2Value", "Test3Value", "Test message Test1Value Test2Value Test3Value")]
        public void ToString_WithParameters(string message, string test1, string test2, string test3, string expected)
        {
            // arrange
            var result = Result.SuccessWithMessage(message, new object[] { test1, test2, test3 });

            // act
            // assert
            Assert.That(result.ToString, Is.EqualTo(expected));
        }

        [Test]
        [Category("Unit")]
        [TestCase("Test message", "Test message", ResultStatusEnum.Error)]
        [TestCase("Test", "Test", ResultStatusEnum.Success)]
        public void ToString_WithStatus_WithoutParameters(string message, string expected, ResultStatusEnum status)
        {
            // arrange
            Result result;
            expected = $"{status}: {expected}";

            // act
            // act
            switch (status)
            {
                case ResultStatusEnum.Success:
                    result = Result.SuccessWithMessage(message);
                    break;
                case ResultStatusEnum.Warning:
                    result = Result.Warning(message);
                    break;
                case ResultStatusEnum.Error:
                    result = Result.Error(message);
                    break;
                default:
                    result = Result.SuccessWithMessage(message);
                    break;
            }

            // assert
            Assert.That(result.ToString(true), Is.EqualTo(expected));
        }

        [Test]
        [Category("Unit")]
        [TestCase("Test message {test} {test} {test}", "Test1Value", "Test2Value", "Test3Value", "Test message Test1Value Test2Value Test3Value", ResultStatusEnum.Success)]
        [TestCase("Test message {Test2} {Test1} {Test3}", "Test1Value", "Test2Value", "Test3Value", "Test message Test1Value Test2Value Test3Value", ResultStatusEnum.Warning)]
        [TestCase("Test message {Test2} {Test1} {Test3}", "Test1Value", "Test2Value", "Test3Value", "Test message Test1Value Test2Value Test3Value", ResultStatusEnum.Error)]
        public void ToString_WithStatus_WithParameters(string message, string test1, string test2, string test3, string expected, ResultStatusEnum status)
        {
            // arrange
            var vals = new object[] { test1, test2, test3 };
            expected = $"{status}: {expected}";
            Result result;

            // act
            switch (status)
            {
                case ResultStatusEnum.Success:
                    result = Result.SuccessWithMessage(message, vals);
                    break;
                case ResultStatusEnum.Warning:
                    result = Result.Warning(message, vals);
                    break;
                case ResultStatusEnum.Error:
                    result = Result.Error(message, vals);
                    break;
                default:
                    result = Result.SuccessWithMessage(message, vals);
                    break;
            }

            // assert
            Assert.That(result.ToString(true), Is.EqualTo(expected));
        }
    }
}