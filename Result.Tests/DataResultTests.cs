using NUnit.Framework;
using System;
using System.Data;
using Wookashi.Common.Result.Enums;

namespace Wookashi.Common.Result.Tests
{
    [TestFixture]
    public class DataResultTests
    {
        [Test]
        [Category("Unit")]
        public void Constructor_NewData_AssignsData()
        {
            // arrange
            var data = new DataTable("Data");

            // act
            var result = new DataResult<DataTable>(data);

            // assert

            Assert.That(result.Data, Is.EqualTo(data));
        }

        [Test]
        [Category("Unit")]
        public void IsCorrect_DataDefined()
        {
            // arrange
            var result = new DataResult<DataTable>(new DataTable("Data"));

            // act 
            var isCorrect = result.IsCorrect;

            // assert
            Assert.That(isCorrect, Is.EqualTo(true));
        }

        [Test]
        [Category("Unit")]
        public void IsCorrect_NullData()
        {
            // arrange
            var result = new DataResult<DataTable>(null);

            // act 
            var isCorrect = result.IsCorrect;

            // assert
            Assert.That(isCorrect, Is.EqualTo(false));
        }

        [Test]
        [Category("Unit")]
        public void Constructor_DataDefined_AssignsData()
        {
            // arrange
            var data = new DataTable("Data");

            // act
            var result = new DataResult<DataTable>(data);

            // assert
            Assert.That(result.Data, Is.EqualTo(data));
        }

        [Test]
        [Category("Unit")]
        public void AddResult_Result_Merge()
        {
            // arrange
            var firstResult = new DataResult<bool>(true);
            var secondResult = new Result(ResultStatusEnum.Warning, "Warning", null);

            // act
            ResultsPack result = firstResult.AddResult(secondResult);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Results, Has.Count.EqualTo(2));
                Assert.That(result.Results, Is.Unique);
            });
        }

        [Test]
        [Category("Unit")]
        [TestCase("Warning", "Warning")]
        public void ToString_WithoutParameters(string message, string expected)
        {
            // arrange
            var result = DataResult<bool>.SuccessWithMessage(message, null);

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
            var result = DataResult<bool>.SuccessWithMessage(message, new object[] { test1, test2, test3 });

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
            DataResult<bool> result;
            expected = $"{status}: {expected}";

            // act
            // act
            switch (status)
            {
                case ResultStatusEnum.Success:
                    result = DataResult<bool>.SuccessWithMessage(message);
                    break;
                case ResultStatusEnum.Warning:
                    result = DataResult<bool>.Warning(message);
                    break;
                case ResultStatusEnum.Error:
                    result = DataResult<bool>.Error(message);
                    break;
                default:
                    result = DataResult<bool>.SuccessWithMessage(message);
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
            expected = $"{status}: {expected}";
            DataResult<bool> result;
            object[] vars = new object[] { test1, test2, test3 };

            // act
            switch (status)
            {
                case ResultStatusEnum.Success:
                    result = DataResult<bool>.SuccessWithMessage(message, vars);
                    break;
                case ResultStatusEnum.Warning:
                    result = DataResult<bool>.Warning(message, vars);
                    break;
                case ResultStatusEnum.Error:
                    result = DataResult<bool>.Error(message, vars);
                    break;
                default:
                    result = DataResult<bool>.SuccessWithMessage(message, vars);
                    break;
            }


            // assert
            Assert.That(result.ToString(true), Is.EqualTo(expected));
        }

        [Test]
        [Category("Unit")]
        public void IsCorrect_IntData_NotInitialized()
        {
            // arrange
            var result = new DataResult<int>();

            // act
            var isCorrect = result.IsCorrect;

            // assert
            Assert.That(isCorrect, Is.EqualTo(false));
        }

        [Test]
        [Category("Unit")]
        [TestCase(1, true)]
        [TestCase(0, true)]
        public void IsCorrect_IntData_Initialized(int value, bool expectedResult)
        {
            // arrange
            var result = new DataResult<int>(value);

            // act
            var isCorrect = result.IsCorrect;

            // assert
            Assert.That(isCorrect, Is.EqualTo(expectedResult));
        }

        [Test]
        [Category("Unit")]
        [TestCase(1, true)]
        [TestCase(0, true)]
        [TestCase(null, false)]
        public void IsCorrect_IntData_Initialized(int? value, bool expectedResult)
        {
            // arrange
            var result = new DataResult<int?>(value);

            // act
            var isCorrect = result.IsCorrect;

            // assert
            Assert.That(isCorrect, Is.EqualTo(expectedResult));
        }

        [Test]
        [Category("Unit")]
        public void SetException_DoesntChangeResultType()
        {
            // arrange
            var result = DataResult<bool>.Error("Test");

            // act
            result.SetException(new NullReferenceException());

            // assert
            Assert.IsTrue(result.GetType() == typeof(DataResult<bool>));
        }
    }
}