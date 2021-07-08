using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Wookashi.Common.Result.Abstraction;
using Wookashi.Common.Result.Enums;

[assembly: InternalsVisibleTo("Wookashi.Common.Result.Tests")]

namespace Wookashi.Common.Result
{
    public class ResultsPack
    {

        #region Constructors

        public ResultsPack()
        {
            Results = new List<IResult>();
        }

        public static ResultsPack Success => new ResultsPack();

        public ResultsPack(Result entry)
        {
            Results = new List<IResult>();

            if (entry is { })
            {
                Results.Add(entry);
            }
        }

        public ResultsPack(IEnumerable<IResult> results)
        {
            Results = results?.ToList() ?? new List<IResult>();
        }

        #endregion

        #region Properties
        internal List<IResult> Results;


        public bool HasErrors => Results.Any(res => res.IsError);
        public bool HasWarnings => Results.Any(res => res.IsWarning);

        public bool IsSuccess => !Results.Any(res => res.IsError || res.IsWarning);

        #endregion

        #region Methods

        /// <summary>
        ///     Add more than one result in one function
        /// </summary>
        /// <param name="entries">Result list (IEnumerable)</param>
        public ResultsPack Add(IEnumerable<IResult> results)
        {
            if (results == null)
            {
                return this;
            }

            foreach (var entry in results)
            {
                Add(entry);
            }
            return this;
        }

        public ResultsPack Add(IResult entry)
        {
            if (entry == null)
            {
                return this;
            }

            Results.Add(entry);
            return this;
        }

        public ResultsPack Add(ResultsPack resPack)
        {
            if (resPack == null)
            {
                return this;
            }

            Add(resPack.Results);
            return this;
        }

        public override string ToString()
        {
            return $"Overall status: {(Results.Any() ? Results.Max(res => res.Status) : ResultStatusEnum.Success)}, All statuses: {Results.Count}, " +
                 $"Successes count: {Results.Where(res => res.IsSuccess).Count()}, " +
                 $"Warnings count: {Results.Where(res => res.IsWarning).Count()}, " +
                 $"Errors count: {Results.Where(res => res.IsError).Count()}";
        }

        public string ToString(bool withStatuses, string newLineSeparator = null)
        {
            newLineSeparator ??= Environment.NewLine;
            StringBuilder sBuilder = new StringBuilder();

            if (withStatuses)
            {
                sBuilder.Append($"Overall status: {(Results.Any() ? Results.Max(res => res.Status) : ResultStatusEnum.Success)}{newLineSeparator}{newLineSeparator}");
            }
            foreach (var result in Results)
            {
                if (withStatuses)
                {
                    sBuilder.Append($"{result.ToString(true)}{newLineSeparator}");
                }
                else
                {
                    sBuilder.Append($"{result}{ newLineSeparator}");
                }
            }
            return sBuilder.ToString();
        }

        #endregion

        #region Deprecated
        //TODO ŁH 2021-14-05 To delete after new methods implementation

        [Obsolete("Method is Obsolete and will be deleted in future releases")]
        public bool IsSuccessOrWarning => Status != ResultStatusEnum.Error;

        [Obsolete("Method is Obsolete and will be replace by AddResults")]
        public ResultsPack AddEntries(IEnumerable<Result> entries)
        {
            return AddResults(entries);
        }

        [Obsolete("Method is Obsolete and will be deleted in future releases")]
        public ResultsPack AddEntry(Result entry) => Add(entry);

        [Obsolete("Method is Obsolete and will be deleted in future releases")]
        public ResultsPack AddResult(ResultsPack resultPack)
        {
            if (resultPack == null || resultPack.Entries == null)
            {
                return this;
            }

            AddEntries(resultPack.Entries);

            return this;
        }

        [Obsolete("Method is Obsolete and will be replace by Results")]
        public List<Result> Entries => Results.Cast<Result>().ToList();

        [Obsolete("Method is Obsolete and will be replace by ToString (true, true)")]
        public string ToFullString() => ToString();

        [Obsolete("Method is Obsolete and will be replace by ToString()")]
        public string ToOneLineString() => ToString();

        [Obsolete("Method is Obsolete, You should use AddSuccess instead")]
        public ResultsPack AddMessage(string message) => AddSuccess(message, null);

        [Obsolete("Method is Obsolete and will be replace by ToString() with parameters")]
        public string ToGuiString(bool withStatus, bool withSubStatuses = false)
        {
            return ToString(withSubStatuses);
        }

        [Obsolete("Property is Obsolete and will be deleted in future releases")]
        public ResultStatusEnum Status => Results.Any() ? Results.Max(res => res.Status) : ResultStatusEnum.Success;

        [Obsolete("Method is Obsolete use Add method instead")]
        public ResultsPack AddResults(IEnumerable<Result> results)
        {
            return Add(results);
        }

        [Obsolete("Method is Obsolete use Add method instead")]
        public ResultsPack AddResultItem(IResult entry)
        {
            return Add(entry);
        }

        /// <summary>
        /// Add Error entry to ResultPack with specified message
        /// </summary>
        /// <param name="message">Message template</param>
        /// <param name="messageVars">Message Vars (optional)</param>
        /// <returns></returns>
        [Obsolete("Method is Obsolete and will be deleted in future releases. Use Add method with proper Result instead")]
        public ResultsPack AddError(string message, params object[] messageVars)
        {
            Results.Add(Result.Error(message, messageVars));
            return this;
        }

        /// <summary>
        /// Add Warning entry to ResultPack with specified message
        /// </summary>
        /// <param name="message">Message template</param>
        /// <param name="messageVars">Message Vars (optional)</param>
        [Obsolete("Method is Obsolete and will be deleted in future releases. Use Add method with proper Result instead")]
        public ResultsPack AddWarning(string message, params object[] messageVars)
        {
            Results.Add(Result.Warning(message, messageVars));
            return this;
        }

        /// <summary>
        /// Add Sucsess entry to ResultPack with specified message
        /// </summary>
        /// <param name="message">Message template</param>
        /// <param name="messageVars">Message Vars (optional)</param>
        [Obsolete("Method is Obsolete and will be deleted in future releases. Use Add method with proper Result instead")]
        public ResultsPack AddSuccess(string message, params object[] messageVars)
        {
            Results.Add(Result.SuccessWithMessage(message, messageVars));
            return this;
        }
        #endregion
    }
}