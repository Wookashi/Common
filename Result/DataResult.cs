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
        public new static DataResult<T> Success => new DataResult<T>();

        /// <summary>
        /// Creates new Result<typeparamref name="T"/> with ResultStatusEnum.Success with Data
        /// </summary>
        /// <param name="data">Data object</param>
        /// <returns>new Result<typeparamref name="T"/> with data included</returns>
        public static DataResult<T> SuccessWithData(T data) => new DataResult<T>(data);

        /// <summary>
        /// Creates new Result<typeparamref name="T"/> with ResultStatusEnum.Success status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public new static DataResult<T> SuccessWithMessage(string messageTemplate, params object[] messageVars) => new DataResult<T>(ResultStatusEnum.Success, messageTemplate, messageVars);

        /// <summary>
        /// Creates new Result<typeparamref name="T"/> with ResultStatusEnum.Warning status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public new static DataResult<T> Warning(string messageTemplate, params object[] messageVars) => new DataResult<T>(ResultStatusEnum.Warning, messageTemplate, messageVars);

        /// <summary>
        /// Creates new Result<typeparamref name="T"/> with ResultStatusEnum.Error status and message.
        /// </summary>
        /// <param name="messageTemplate">Templlte message with parameters names</param>
        /// <param name="messageVars">Template variable values and names</param>
        /// <returns>Newly created result</returns>
        public new static DataResult<T> Error(string messageTemplate, params object[] messageVars) => new DataResult<T>(ResultStatusEnum.Error, messageTemplate, messageVars);


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

        #region Deprecated
        //TODO ŁH 2021-14-05 To delete after new methods implementation

        [Obsolete("AddResult is obsolete and will be deleted in future releases")]
        public ResultsPack AddResult(DataResult<T> result)
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

        [Obsolete("Method is Obsolete, you can use direct value instead")]
        public T GetData()
        {
            return Data;
        }

        #endregion

    }
}
