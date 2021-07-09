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

        public static ResultsPack Success => new();

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
                 $"Successes count: {Results.Count(res => res.IsSuccess)}, " +
                 $"Warnings count: {Results.Count(res => res.IsWarning)}, " +
                 $"Errors count: {Results.Count(res => res.IsError)}";
        }

        public string ToString(bool withStatuses, string newLineSeparator = null)
        {
            newLineSeparator ??= Environment.NewLine;
            StringBuilder sBuilder = new();

            if (withStatuses)
            {
                sBuilder.Append("Overall status: ").Append(Results.Any() ? Results.Max(res => res.Status) : ResultStatusEnum.Success).Append(newLineSeparator).Append(newLineSeparator);
            }
            foreach (var result in Results)
            {
                if (withStatuses)
                {
                    sBuilder.Append(result.ToString(true)).Append(newLineSeparator);
                }
                else
                {
                    sBuilder.Append(result).Append(newLineSeparator);
                }
            }
            return sBuilder.ToString();
        }

        #endregion
    }
}