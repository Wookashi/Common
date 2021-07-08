using System;
using System.Text.RegularExpressions;

namespace Wookashi.Common.Result.Extensions
{
    internal static class MessageTemplateEvaluator
    {
        private const string MessageTemplatePattern = "({[^{}]+})";

        private static Regex _regex;
        private static Regex RegexInstance
        {
            get
            {
                if (_regex == null)
                {
                    _regex = new Regex(MessageTemplatePattern, RegexOptions.IgnorePatternWhitespace, TimeSpan.FromSeconds(.25));
                }
                return _regex;
            }
        }

        /// <summary>
        /// Renders message from template
        /// </summary>
        /// <param name="messageTemplate">Template for messagge</param>
        /// <param name="messageTemplateValues">Message variables</param>
        /// <returns></returns>
        static public string RenderMessage(string messageTemplate, object[] messageTemplateValues)
        {
            if (messageTemplateValues == null || messageTemplateValues.Length == 0)
            {
                return messageTemplate;
            }

            int valueIndex = 0;
            var evaluator = new MatchEvaluator(_ => $"{{{valueIndex++}}}");
            var formatedMessage = RegexInstance.Replace(messageTemplate, evaluator);
            return string.Format(formatedMessage, messageTemplateValues);
        }
    }
}
