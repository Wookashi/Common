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
        public static Result Success => new Result();

        /// <summary>
        /// Creates new Result with ResultStatusEnum.Sucess status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public static Result SuccessWithMessage(string messageTemplate, params object[] messageVars) => new Result(ResultStatusEnum.Success, messageTemplate, messageVars);
        /// <summary>
        /// Creates new Result with ResultStatusEnum.Warning status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public static Result Warning(string messageTemplate, params object[] messageVars) => new Result(ResultStatusEnum.Warning, messageTemplate, messageVars);
        /// <summary>
        /// Creates new Result with ResultStatusEnum.Error status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public static Result Error(string messageTemplate, params object[] messageVars) => new Result(ResultStatusEnum.Error, messageTemplate, messageVars);

        #endregion

        #region Properties

        public ResultStatusEnum Status { get; private set; }
        public bool IsError => Status == ResultStatusEnum.Error;
        public bool IsWarning => Status == ResultStatusEnum.Warning;
        public bool IsSuccess => Status == ResultStatusEnum.Success;

        public string MessageTemplate { get; private set; }
        public object[] MessageVars { get; private set; }
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

        #region Deprecated
        //TODO ŁH 2021-14-05 To delete after new methods implementation

        [Obsolete("Method is Obsolete and will be deleted in future releases")]
        public bool IsSuccessOrWarning => Status != ResultStatusEnum.Error;

        [Obsolete("Except of that you should use DataResult with Id type")]
        public long? DependentEntityId { get; private set; }

        [Obsolete("Except of that you should use DataResult with Id type")]
        public Result SetDependentEntityId(long id)
        {
            DependentEntityId = id;
            return this;
        }

        [Obsolete("Except of that you should use ToString() method")]
        public string ToGuiString(bool withStatus = false)
        {
            return ToString(withStatus);
        }

        [Obsolete("constructor is obsolete, use dedicated constructor: SuccessWithMessage")]
        public static Result Message(string message, (string Name, object Value)[] messageVars = null) => new Result(ResultStatusEnum.Success, message, messageVars);

        [Obsolete("AddResult is obsolete and will be deleted in future releases")]
        public ResultsPack AddResult(Result result)
        {
            if (result == null)
            {
                throw new ArgumentNullException($"{nameof(result)} nie może być nullem");
            }

            var results = new List<Result>() {
                result,
                this
            };
            return new ResultsPack(results);
        }

        [Obsolete("Method is Obsolete and will be deleted in future releases")]
        public bool HasWarnings => IsWarning;
        [Obsolete("Method is Obsolete and will be deleted in future releases")]
        public bool HasErrors => IsError;

        #endregion
    }
}