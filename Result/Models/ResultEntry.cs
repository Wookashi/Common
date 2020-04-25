using Result.Enums;

namespace Result.Models
{
    public class ResultEntry
    {
        public ResultEntry()
        {
        }

        public ResultEntry(ResultStatus status, string message = null)
        {
            Status = status;
            Message = message;
        }

        public int Code { get; set; }

        public ResultStatus Status { get; internal set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public string MemberName { get; set; }

        public override string ToString()
        {
            return $"[{ Status}:{ Code}] { Message}";
        }

        public string ToGuiString(bool withStatus = false)
        {
            if (withStatus)
                return $"{Status}: {Message}";
            return Message;
        }
    }
}