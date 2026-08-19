using Vendors.Domain.Abstractions;
using Xunit;

namespace Vendors.UnitTests.Domain;

public sealed class ResultTests
{
    private static Error Map(Exception exception) => Error.Failure("Test.Error", exception.Message);

    [Fact]
    public async Task Of_Returns_Success_When_Operation_Succeeds()
    {
        Result<int> result = await Result.Of(() => Task.FromResult(Result.Success(42)), Map);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task Of_Folds_Thrown_Exception_Into_Failure_Via_Mapper()
    {
        Func<Task<Result<int>>> operation = () => throw new InvalidOperationException("boom");

        Result<int> result = await Result.Of(operation, Map);

        Assert.True(result.IsFailure);
        Assert.Equal("Test.Error", result.Error.Code);
        Assert.Equal("boom", result.Error.Description);
    }

    [Fact]
    public async Task Of_Does_Not_Swallow_Cancellation()
    {
        Func<Task<Result<int>>> operation = () => throw new OperationCanceledException();

        await Assert.ThrowsAsync<OperationCanceledException>(() => Result.Of(operation, Map));
    }

    [Fact]
    public void Value_Throws_When_Result_Is_Failure()
    {
        Result<int> result = Result.Failure<int>(Error.NotFound("X", "missing"));

        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
