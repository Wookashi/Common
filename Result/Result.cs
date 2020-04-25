using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wookashi.Common.Result.Enums;
using Wookashi.Common.Result.Models;

namespace Wookashi.Common.Result
{
    public class Result
    {
        public readonly List<ResultEntry> Entries;
        private readonly bool _preventDuplicates;

        public long? DependentEntityId { get; private set; }

        public Result()
        {
            Entries = new List<ResultEntry>();
            Status = ResultStatus.Success;
        }

        public Result(bool preventDuplicates) : this()
        {
            _preventDuplicates = preventDuplicates;
        }

        public Result(IEnumerable<ResultEntry> entries)
        {
            Entries = new List<ResultEntry>();

            if (entries == null)
            {
                return;
            }

            foreach (var entry in entries)
            {
                AddEntry(entry);
            }
        }

        public Result(ResultEntry entry)
        {
            Entries = new List<ResultEntry>();

            if (entry == null)
            {
                return;
            }

            AddEntry(entry);
        }

        public ResultStatus Status { get; internal set; }

        public static Result Success => new Result();

        /// <summary>
        /// Return <c>true</c> if result haven't error status
        /// </summary>
        public bool Succeed => Status != ResultStatus.Error;

        /// <summary>
        /// Indicate if result have status error
        /// </summary>
        public bool IsError => Status == ResultStatus.Error;

        /// <summary>
        /// Indicate if result have status warning
        /// </summary>
        public bool IsWarning => Status == ResultStatus.Warning;

        /// <summary>
        /// Return <c>true</c> if result have at least one error
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return Entries.Any(x => x.Status == ResultStatus.Error);
            }
        }

        public void SetDependentEntityId(long id)
        {
            DependentEntityId = id;
        }

        public static Result Error(string message, int code = 0, string memberName = null)
        {
            var result = new Result();
            result.AddError(message, code, memberName);
            return result;
        }

        public static Result Message(string message, int code = 0, string memberName = null)
        {
            var result = new Result();
            result.AddMessage(message, code, memberName);
            return result;
        }

        public static Result Warning(string message, int code = 0, string memberName = null)
        {
            var result = new Result();
            result.AddWarning(message, code, memberName);
            return result;
        }

        /// <summary>
        /// Add more than one result in one function
        /// </summary>
        /// <param name="entries">Entries list (IEnumerable)</param>
        public void AddEntries(IEnumerable<ResultEntry> entries)
        {
            if (entries == null)
            {
                return;
            }

            foreach (var entry1 in entries)
            {
                var entry = entry1;

                if (!_preventDuplicates || !Entries.Exists(x =>
                {
                    if (x.Message == entry.Message && x.Code == entry.Code)
                    {
                        return x.Status == entry.Status;
                    }

                    return false;
                }))
                    AddEntry(entry);
            }
        }

        /// <summary>
        /// Add new entry to result entry collection
        /// </summary>
        /// <param name="entry">Result entry</param>
        public void AddEntry(ResultEntry entry)
        {
            if (entry == null || _preventDuplicates && Entries.Exists(x =>
            {
                if (x.Message == entry.Message && x.Code == entry.Code)
                {
                    return x.Status == entry.Status;
                }

                return false;
            }))
                return;
            Entries.Add(entry);
            SetStatus(entry.Status);
        }

        /// <summary>
        /// Add error to result entry collection. Entries cannot be duplicated.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="code">Error code</param>
        /// <param name="memberName">Property name (not needed)</param>
        public void AddError(string message, int code = 0, string memberName = null)
        {
            if (_preventDuplicates && Entries.Exists(x =>
            {
                if (x.Message == message && x.Code == code)
                {
                    return x.Status == ResultStatus.Error;
                }

                return false;
            }))
                return;
            Entries.Add(new ResultEntry(ResultStatus.Error)
            {
                Code = code,
                Message = message,
                MemberName = memberName
            });

            SetStatus(ResultStatus.Error);
        }

        /// <summary>
        /// Add message to result entry collection. Entries cannot be duplicated.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="code">Error code</param>
        /// <param name="memberName">Property name (not needed)</param>
        public void AddMessage(string message, int code = 0, string memberName = null)
        {
            if (_preventDuplicates && Entries.Exists(x =>
            {
                if (x.Message == message && x.Code == code)
                {
                    return x.Status == ResultStatus.Success;
                }

                return false;
            }))
                return;

            Entries.Add(new ResultEntry(ResultStatus.Success)
            {
                Code = code,
                Message = message,
                MemberName = memberName
            });

            SetStatus(ResultStatus.Success);
        }

        /// <summary>
        /// Add result entries from parameter result and set its status.
        /// </summary>
        /// <param name="result">Added Result</param>
        public void AddResult(Result result)
        {
            if (result == null || result.Entries == null)
            {
                return;
            }

            AddEntries(result.Entries);
            SetStatus(result.Status);

            if (result.DependentEntityId.HasValue)
            {
                SetDependentEntityId(result.DependentEntityId.Value);
            }
        }

        /// <summary>
        /// Add warning to result entry collection. Entries cannot be duplicated.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="code">Error code</param>
        /// <param name="memberName">Property name (not needed)</param>
        public void AddWarning(string message, int code = 0, string memberName = null)
        {
            if (_preventDuplicates && Entries.Exists(x =>
            {
                if (x.Message == message && x.Code == code)
                {
                    return x.Status == ResultStatus.Warning;
                }

                return false;
            }))
                return;

            Entries.Add(new ResultEntry(ResultStatus.Warning)
            {
                Code = code,
                Message = message,
                MemberName = memberName
            });

            SetStatus(ResultStatus.Warning);
        }

        /// <summary>
        /// Inserts entry on index 0
        /// </summary>
        /// <param name="entry"></param>
        public void InsertEntry(ResultEntry entry)
        {
            if (entry == null || _preventDuplicates && Entries.Exists(x =>
            {
                if (x.Message == entry.Message && x.Code == entry.Code)
                {
                    return x.Status == entry.Status;
                }

                return false;
            }))
                return;
            Entries.Insert(0, entry);

            SetStatus(entry.Status);
        }

        /// <summary>
        /// Sets result status to parameter status, can be use with force boolean (optional)
        /// </summary>
        /// <param name="status">(ResultStatus) Status to set</param>
        /// <param name="force">(bool) force</param>
        /// <returns>bool result of operation</returns>
        public bool SetStatus(ResultStatus status, bool force = false)
        {
            if (!force && status <= Status)
            {
                return false;
            }

            Status = status;
            return true;
        }

        public string ToFullString()
        {
            return
                $"Status: {Status}{Environment.NewLine}---{Environment.NewLine}{ToString()}{Environment.NewLine as object}---";
        }

        public string ToGuiString(bool withStatus, bool withSubStatuses = false)
        {
            var stringBuilder = new StringBuilder();

            if (withStatus)
            {
                stringBuilder.AppendLine(Status.ToString());
            }

            for (var index = 0; index < Entries.Count; ++index)
            {
                if (index > 0)
                {
                    stringBuilder.AppendLine();
                }
                stringBuilder.Append(Entries[index].ToGuiString(withSubStatuses));
            }

            return stringBuilder.ToString();
        }

        public string ToOneLineString()
        {
            var stringBuilder = new StringBuilder();

            foreach (ResultEntry t in Entries)
            {
                stringBuilder.Append(t).Append(";");
            }

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            for (var index = 0; index < Entries.Count; ++index)
            {
                if (index > 0)
                {
                    stringBuilder.AppendLine();
                }

                stringBuilder.Append(Entries[index]);
            }

            return stringBuilder.ToString();
        }
    }

    public class Result<T> : Result
    {
        public new static Result<T> Success
        {
            get
            {
                return new Result<T>();
            }
        }

        public T Data { get; set; }

        public Result()
        {
        }

        public Result(bool preventDuplicates)
          : base(preventDuplicates)
        {
        }

        public Result(T data) : this()
        {
            Data = data;
        }

        public Result(ResultEntry entry) : this()
        {
            AddEntry(entry);
        }

        public Result(IEnumerable<ResultEntry> entries) : this()
        {
            AddEntries(entries);
        }

        public Result(T resultData, ResultEntry entry) : this(resultData)
        {
            AddEntry(entry);
        }

        /// <summary>
        /// Add result entries from parameter result and set its status. Data will be set too.
        /// </summary>
        /// <param name="result">Added Result</param>
        public void AddResult(Result<T> result)
        {
            if (result?.Entries == null)
            {
                return;
            }

            AddEntries(result.Entries);
            SetStatus(result.Status);
            Data = result.Data;
        }

        /// <summary>
        /// Returns <c>true</c> when result have status Sucess and data is not null.
        /// </summary>
        /// <returns></returns>
        public bool IsCorrect()
        {
            return Status == ResultStatus.Success && Data != null;
        }
    }
}