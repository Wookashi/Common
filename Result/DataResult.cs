using System;
using System.Collections.Generic;
using Wookashi.Common.Result.Abstraction;
using Wookashi.Common.Result.Enums;

namespace Wookashi.Common.Result
{
    public class DataResult<T> : Result, IResult
    {
        #region Construtors
        internal DataResult() : base()
        { }

        public DataResult(T data) : this()
        {
            Data = data;
        }
        private DataResult(ResultStatusEnum status, string message, object[] messageVars) : base(status, message, messageVars)
        {
        }

        /// <summary>
        /// Creates new Result<typeparamref name="T"/> with ResultStatusEnum.Sucess without any message.
        /// </summary>
        public new static DataResult<T> Success => new();

        /// <summary>
        /// Creates new Result<typeparamref name="T"/> with ResultStatusEnum.Success with Data
        /// </summary>
        /// <param name="data">Data object</param>
        /// <returns>new Result<typeparamref name="T"/> with data included</returns>
        public static DataResult<T> SuccessWithData(T data) => new(data);

        /// <summary>
        /// Creates new Result<typeparamref name="T"/> with ResultStatusEnum.Success status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public new static DataResult<T> SuccessWithMessage(string messageTemplate, params object[] messageVars) => new(ResultStatusEnum.Success, messageTemplate, messageVars);

        /// <summary>
        /// Creates new Result<typeparamref name="T"/> with ResultStatusEnum.Warning status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public new static DataResult<T> Warning(string messageTemplate, params object[] messageVars) => new(ResultStatusEnum.Warning, messageTemplate, messageVars);

        /// <summary>
        /// Creates new Result<typeparamref name="T"/> with ResultStatusEnum.Error status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public new static DataResult<T> Error(string messageTemplate, params object[] messageVars) => new(ResultStatusEnum.Error, messageTemplate, messageVars);

        #endregion

        #region Properties

        private T _data;
        private bool _hasDataValue;

        public T Data
        {
            get
            {
                return _data;
            }
            set
            {
                if (value is { })
                {
                    _hasDataValue = true;
                }
                _data = value;
            }
        }

        public bool HasValue => _hasDataValue;

        /// <summary>
        /// Indicates that status is Success and Data is set
        /// </summary>
        public bool IsCorrect => IsSuccess && _hasDataValue;

        public new DataResult<T> SetException(Exception exception)
        {
            base.SetException(exception);
            return this;
        }

        #endregion

    }
}
