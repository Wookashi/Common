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

        #region Deprecated
        //TODO ŁH 2021-14-05 To delete after new methods implementation

        [Obsolete("Except of that you should use DataResult with Id type")]
        Result SetDependentEntityId(long id);

        [Obsolete("Method is Obsolete and will be deleted in future releases")]
        bool IsSuccessOrWarning { get; }

        [Obsolete("Except of that you should use ToString() method")]
        string ToGuiString(bool withStatus = false);

        #endregion
    }
}
