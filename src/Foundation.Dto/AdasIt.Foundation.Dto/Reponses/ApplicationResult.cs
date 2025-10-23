namespace AdasIt.Foundation.Dto.Reponses;

public class ApplicationResult<T> where T : class?
{
    public bool IsSuccess => _errors.Count == 0;
    public bool IsFailure => !IsSuccess;
    public T? Data { get; private set; }

    private readonly ICollection<ErrorModel> _errors;
    public IReadOnlyCollection<ErrorModel> Errors => [.. _errors];

    private readonly ICollection<ErrorModel> _warnings;
    public IReadOnlyCollection<ErrorModel> Warnings => [.. _warnings];

    private readonly ICollection<ErrorModel> _information;
    public IReadOnlyCollection<ErrorModel> Information => [.. _information];

    private ApplicationResult(ICollection<ErrorModel>? errors,
        ICollection<ErrorModel>? warnings,
        ICollection<ErrorModel>? information,
        T? data)
    {
        _warnings = warnings ?? [];
        _errors = errors ?? [];
        _information = information ?? [];
        Data = data;
    }

    public static implicit operator ApplicationResult<T>(T value) => Success(Data: value);

    public static ApplicationResult<T> Success(
        ICollection<ErrorModel>? Warnings = null,
        ICollection<ErrorModel>? Information = null,
        T? Data = null) => new(null, Warnings, Information, Data);

    public static ApplicationResult<T> Failure(
        ICollection<ErrorModel>? Errors = null,
        ICollection<ErrorModel>? Warnings = null,
        ICollection<ErrorModel>? Information = null,
        T? Data = null) => new(Errors, Warnings, Information, Data);

    public ApplicationResult<T> AddError(ErrorModel Errors)
    {
        _errors.Add(Errors);
        return this;
    }
    public ApplicationResult<T> AddErrors(List<ErrorModel> Errors)
    {
        Errors.ForEach(e => _errors.Add(e));
        return this;
    }

    public ApplicationResult<T> AddWarnings(ErrorModel Errors)
    {
        _warnings.Add(Errors);
        return this;
    }

    public ApplicationResult<T> AddInformation(ErrorModel Errors)
    {
        _information.Add(Errors);
        return this;
    }

    public ApplicationResult<T> SetData(T data)
    {
        Data = data;
        return this;
    }
}
