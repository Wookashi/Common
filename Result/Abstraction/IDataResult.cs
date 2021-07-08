namespace Wookashi.Common.Result.Abstraction
{
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
