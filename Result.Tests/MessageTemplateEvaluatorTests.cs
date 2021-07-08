using NUnit.Framework;
using System;
using Wookashi.Common.Result;

namespace Wookashi.Common.Result.Tests
{
    [TestFixture]
    public class MessageTemplateEvaluatorTests
    {
        [Test]
        [Category("Unit")]
        [TestCase("Test message Test1 Test2 Test3", "Test message Test1 Test2 Test3")]
        [TestCase("Test message cold vodka is on first shelf", "Test message cold vodka is on first shelf")]
        [TestCase("Test message Test1Value 123-456-678 person@example.com", "Test message Test1Value 123-456-678 person@example.com")]
        public void RenderMessage_WithoutParameters(string message, string expected)
        {
            // arrange
            Result result = Result.SuccessWithMessage(message);

            // assert
            Assert.That(result.ToString(), Is.EqualTo(expected));
        }

        [Test]
        [Category("Unit")]
        [TestCase("Test message {test} {test} {test}", "Test1", "Test2", "Test3", "Test message Test1 Test2 Test3")]
        [TestCase("Test message {Test1} {Test2} {Test3}", "cold vodka", "is on first", "shelf", "Test message cold vodka is on first shelf")]
        [TestCase("Test message {Test1} {Test2} {Test3}", "Test1Value", "123-456-678", "person@example.com", "Test message Test1Value 123-456-678 person@example.com")]
        public void RenderMessage_WithParameters(string message, string test1, string test2, string test3, string expected)
        {
            // arrange
            var vals = new object[] { test1, test2, test3 };
            var result = Result.SuccessWithMessage(message, vals);

            // assert
            Assert.That(result.ToString(), Is.EqualTo(expected));
        }
    }
}