using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using Wookashi.Common.Result;
using Wookashi.Common.Result.Enums;

namespace Wookashi.Common.Result.Tests
{
    class ResultsPackTests
    {
        [Test]
        [Category("Unit")]
        public void Constructor_EmptyCollection_DoesNotAddToList()
        {
            // arrange
            var entries = new List<Result>();

            // act
            var resPack = new ResultsPack(entries);

            // assert
            Assert.That(resPack.Results, Has.Count.EqualTo(0));
        }

        [Test]
        [Category("Unit")]
        public void Constructor_EntryCollection_AddsToList()
        {
            // arrange
            var entries = new List<Result>
                {
                    new Result(ResultStatusEnum.Success, "Success", null),
                    new Result(ResultStatusEnum.Warning, "Warning", null),
                    new Result(ResultStatusEnum.Error, "Error", null)
                };

            // act
            var result = new ResultsPack(entries);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Results, Has.Count.EqualTo(3));
                Assert.That(result.Results, Is.Unique);
            });
        }

        [Test]
        [Category("Unit")]
        public void Constructor_NewEntry_AddsToList()
        {
            // arrange
            var entry = new Result(ResultStatusEnum.Success, "Success", null);

            // act
            var resPack = new ResultsPack(entry);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(resPack.Results, Has.Count.EqualTo(1));
                Assert.That(resPack.Results, Has.One.Matches<Result>(r => r.IsSuccess));
            });
        }

        [Test]
        [Category("Unit")]
        public void Add_DataResultWithData_AddToResults()
        {
            // arrange
            var resPack = new ResultsPack();
            var resultWithValue = DataResult<string>.SuccessWithData("tuti-fruti");

            // act
            resPack.Add(resultWithValue);

            // assert
            Assert.That(resPack.Results, Has.Count.EqualTo(1));
        }

        [Test]
        [Category("Unit")]
        public void Add_DataResultWithoutData_AddToResults()
        {
            // arrange
            var resPack = new ResultsPack();
            var resultWithoutValue = DataResult<string>.Success;

            // act
            resPack.Add(resultWithoutValue);

            // assert
            Assert.That(resPack.Results, Has.Count.EqualTo(1));
        }

        [Test]
        [Category("Unit")]
        public void Constructor_NullCollection_DoesNotAddToList()
        {
            // arrange
            List<Result> entries = null;

            // act
            var resPack = new ResultsPack(entries);

            // assert
            Assert.That(resPack.Results, Has.Count.EqualTo(0));
        }

        [Test]
        [Category("Unit")]
        public void Constructor_NullEntry_DoesNotAddToList()
        {
            // arrange
            Result entry = null;

            // act
            var resPack = new ResultsPack(entry);

            // assert
            Assert.That(resPack.Results, Has.Count.EqualTo(0));
        }

        [Test]
        [Category("Unit")]
        public void Add_EmptyCollection_DoesNotAddToList()
        {
            // arrange
            var resPack = new ResultsPack();
            var entries = new List<Result>();

            // act
            resPack.Add(entries);

            // assert
            Assert.That(resPack.Results, Has.Count.EqualTo(0));
        }

        [Test]
        [Category("Unit")]
        public void Add_EntryCollection_AddsToList()
        {
            // arrange
            var result = new ResultsPack();
            var entries = new List<Result>
                {
                    new Result(ResultStatusEnum.Success, "Success", null),
                    new Result(ResultStatusEnum.Warning, "Warning", null),
                    new Result(ResultStatusEnum.Error, "Error", null)
                };

            // act
            result.Add(entries);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Results, Has.Count.EqualTo(3));
                Assert.That(result.Results, Is.Unique);
            });
        }

        [Test]
        [Category("Unit")]
        public void Add_NewEntry_AddsToList()
        {
            // arrange
            var result = new ResultsPack();
            var entry = new Result(ResultStatusEnum.Success, "Success", null);

            // act
            result.Add(entry);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Results, Has.Count.EqualTo(1));
                Assert.That(result.Results, Has.One.Matches<Result>(r => r.IsSuccess));
            });
        }

        [Test]
        [Category("Unit")]
        public void Add_NewResultsPack_AddsEntriesAndSetsStatus()
        {
            // arrange
            var parentResult = new ResultsPack();
            parentResult.Add(new List<Result>
                {
                    new Result(ResultStatusEnum.Success, "Success", null),
                    new Result(ResultStatusEnum.Warning, "Warning", null),
                    new Result(ResultStatusEnum.Error, "Error", null),
                });

            var childResult = new ResultsPack();
            childResult.Results.Add(new Result(ResultStatusEnum.Success, "Informacja", null));

            // act
            childResult.Add(parentResult);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(childResult.Results, Has.Count.EqualTo(4));
                Assert.That(childResult.Results, Is.Unique);
                Assert.That(childResult.HasErrors);
            });
        }

        [Test]
        [Category("Unit")]
        public void HasErrors_EntriesWithError_True()
        {
            // arrange
            var result = new ResultsPack();

            result.Add(new List<Result>
                {
                    new Result(ResultStatusEnum.Success, "Success", null),
                    new Result(ResultStatusEnum.Warning, "Warning", null),
                    new Result(ResultStatusEnum.Error, "Error", null)
                });

            // act & assert
            Assert.That(result.HasErrors, Is.True);
        }

        [Test]
        [Category("Unit")]
        public void HasErrors_EntriesWithoutErrors_False()
        {
            // arrange
            var result = new ResultsPack();
            result.Add(new List<Result>
                    {
                        new Result(ResultStatusEnum.Success, "Success", null),
                        new Result(ResultStatusEnum.Warning, "Warning", null),
                        new Result(ResultStatusEnum.Warning, "Uwaga", null)
                    });

            // act & assert
            Assert.That(result.HasErrors, Is.False);
        }

        [Test]
        [Category("Unit")]
        public void HasWarnings_EntriesWithWarning_True()
        {
            // arrange
            var result = new ResultsPack();
            result.Add(new List<Result>
                {
                    new Result(ResultStatusEnum.Success, "Success", null),
                    new Result(ResultStatusEnum.Warning, "Warning", null)
                });

            // act & assert
            Assert.That(result.HasWarnings);
        }

        [Test]
        [Category("Unit")]
        public void HasWarnings_EntriesWithoutWarning_False()
        {
            // arrange
            var result = new ResultsPack();
            result.Add(new List<Result>
                {
                    new Result(ResultStatusEnum.Success, "Success", null),
                    new Result(ResultStatusEnum.Error, "Error", null),
                    new Result(ResultStatusEnum.Error, "Error", null)
                });

            // act & assert
            Assert.That(result.HasWarnings, Is.False);
        }

        [Test]
        [Category("Unit")]
        public void IsSuccess_ReturnTrueForAddSucess()
        {
            // arrange
            var result = ResultsPack.Success;
            result.Add(Result.SuccessWithMessage("ok"));

            // act & assert
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        [Category("Unit")]
        public void IsSuccessOrWarning_ReturnFalseForError()
        {
            // arrange

            var result = new ResultsPack().Add(Result.Error("Error"));

            // act
            var succeed = result.IsSuccess;

            // assert
            Assert.AreEqual(false, succeed);
        }

        [Test]
        [Category("Unit")]
        public void IsSuccess_ReturnTrueForSuccess()
        {
            // arrange

            var result = ResultsPack.Success;

            // act
            var succeed = result.IsSuccess;

            // assert
            Assert.AreEqual(true, succeed);
        }

        [Test]
        [Category("Unit")]
        public void IsSuccess_ReturnFalseForWarning()
        {
            // arrange

            var result = new ResultsPack().Add(Result.Warning("Warning"));

            // assert
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        [Category("Unit")]
        public void ToString_WithoutParameters()
        {
            // arrange
            var resPack = new ResultsPack().Add(Result.SuccessWithMessage("test", null));
            var expected = $"Overall status: Error, All statuses: 6, Successes count: 1, Warnings count: 2, Errors count: 3";

            resPack.Add(Result.Warning("test"));
            resPack.Add(Result.Warning("test"));
            resPack.Add(Result.Error("test"));
            resPack.Add(Result.Error("test"));
            resPack.Add(Result.Error("test"));

            // act
            // assert
            Assert.That(resPack.ToString, Is.EqualTo(expected));
        }

        [Test]
        [Category("Unit")]
        [TestCase("Test1", "Test2", "Test3", "Test1\r\nTest2\r\nTest3\r\n")]
        [TestCase("Test1Value", "Test2Value", "Test3Value", "Test1Value\r\nTest2Value\r\nTest3Value\r\n")]
        public void ToString_Extended(string message1, string message2, string message3, string expected)
        {
            // arrange
            var results = new List<Result>()
            {
                Result.SuccessWithMessage(message1),
                Result.Warning(message2),
                Result.Error(message3)
            };

            var resPack = new ResultsPack(results);

            // act
            // assert
            Assert.That(resPack.ToString(false), Is.EqualTo(expected));
        }

        [Test]
        [Category("Unit")]
        [TestCase("Test1", "Test2", "Test3", "Overall status: Error\r\n\r\nSuccess: Test1\r\nWarning: Test2\r\nError: Test3\r\n")]
        [TestCase("Test1Value", "Test2Value", "Test3Value", "Overall status: Error\r\n\r\nSuccess: Test1Value\r\nWarning: Test2Value\r\nError: Test3Value\r\n")]
        public void ToString_WithStatuses(string message1, string message2, string message3, string expected)
        {
            // arrange
            var results = new List<Result>()
            {
                Result.SuccessWithMessage(message1),
                Result.Warning(message2),
                Result.Error(message3)
            };

            var resPack = new ResultsPack(results);

            // act
            // assert
            Assert.That(resPack.ToString(true), Is.EqualTo(expected));
        }

        [Test]
        [Category("Unit")]
        [TestCase("Test1", "Test2", "Test3", "Overall status: Error<br><br>Success: Test1<br>Warning: Test2<br>Error: Test3<br>")]
        [TestCase("Test1Value", "Test2Value", "Test3Value", "Overall status: Error<br><br>Success: Test1Value<br>Warning: Test2Value<br>Error: Test3Value<br>")]
        public void ToString_WithStatuses_CustomSeparator(string message1, string message2, string message3, string expected)
        {
            // arrange
            var results = new List<Result>()
            {
                Result.SuccessWithMessage(message1),
                Result.Warning(message2),
                Result.Error(message3)
            };

            var resPack = new ResultsPack(results);

            // act
            // assert
            Assert.That(resPack.ToString(true, "<br>"), Is.EqualTo(expected));
        }

        [Test]
        [Category("Unit")]
        public void AddResult_NewResultAndData_AddsEntriesAndAssignsData()
        {
            // arrange
            var data = new DataTable("Data");
            var parentResult = new ResultsPack();
            parentResult.Results.AddRange(new List<Result>
                {
                    new Result(ResultStatusEnum.Success, "Success", null),
                    new Result(ResultStatusEnum.Warning, "Warning", null),
                    new Result(ResultStatusEnum.Error, "Error", null)
                });

            var childResult = new ResultsPack();
            childResult.Results.Add(new Result(ResultStatusEnum.Success, "Informacja", null));

            // act
            childResult.Add(parentResult);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(childResult.Results, Has.Count.EqualTo(4));
                Assert.That(childResult.Results, Is.Unique);
                Assert.That(childResult.HasWarnings);
                Assert.That(childResult.HasErrors);
            });
        }
    }
}
