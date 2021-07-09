using System;
using System.Collections.Generic;
using Wookashi.Common.Result.Abstraction;
using Wookashi.Common.Result.Enums;
using Wookashi.Common.Result.Extensions;

namespace Wookashi.Common.Result
{
    public class Result : IResult
    {
        #region Constructors
        protected Result()
        {
            Status = ResultStatusEnum.Success;
        }

        internal Result(ResultStatusEnum status, string messageTemplate, params object[] messageVars) : base()
        {
            Status = status;
            MessageTemplate = messageTemplate;
            MessageVars = messageVars;
        }

        /// <summary>
        /// Creates new Result with ResultStatusEnum.Sucess without any message.
        /// </summary>
        public static Result Success => new();

        /// <summary>
        /// Creates new Result with ResultStatusEnum.Sucess status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public static Result SuccessWithMessage(string messageTemplate, params object[] messageVars) => new(ResultStatusEnum.Success, messageTemplate, messageVars);
        /// <summary>
        /// Creates new Result with ResultStatusEnum.Warning status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public static Result Warning(string messageTemplate, params object[] messageVars) => new(ResultStatusEnum.Warning, messageTemplate, messageVars);
        /// <summary>
        /// Creates new Result with ResultStatusEnum.Error status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public static Result Error(string messageTemplate, params object[] messageVars) => new(ResultStatusEnum.Error, messageTemplate, messageVars);

        #endregion

        #region Properties

        public ResultStatusEnum Status { get; }
        public bool IsError => Status == ResultStatusEnum.Error;
        public bool IsWarning => Status == ResultStatusEnum.Warning;
        public bool IsSuccess => Status == ResultStatusEnum.Success;

        public string MessageTemplate { get; }
        public object[] MessageVars { get; }
        public Exception Exception { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets Exception field. You can use all System.Exception type.
        /// </summary>
        /// <param name="exception"></param>
        public Result SetException(Exception exception)
        {
            Exception = exception;
            return this;
        }

        public override string ToString()
        {
            return MessageTemplateEvaluator.RenderMessage(MessageTemplate, MessageVars);
        }

        public string ToString(bool withStatus)
        {
            if (withStatus)
            {
                return $"{Status}: {this}";
            }

            return ToString();
        }

        #endregion
    }
}