using System;
using Wookashi.Common.Result.Enums;

namespace Wookashi.Common.Result.Abstraction
{
    public interface IResult
    {
        ResultStatusEnum Status { get; }
        bool IsError { get; }
        bool IsWarning { get; }
        bool IsSuccess { get; }
        string MessageTemplate { get; }
        object[] MessageVars { get; }
        Exception Exception { get; }
        string ToString(bool withStatus = false);
    }
}
