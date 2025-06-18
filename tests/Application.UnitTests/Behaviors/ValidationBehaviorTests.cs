using BackendAuthTemplate.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Shouldly;

namespace BackendAuthTemplate.Application.UnitTests.Behaviors
{
    public class ValidationBehaviorTests
    {
        private class TestRequest : IRequest<string>
        {
            public string Value { get; set; } = string.Empty;
        }

        private class TestValidator : AbstractValidator<TestRequest>
        {
            public TestValidator()
            {
                _ = RuleFor(x => x.Value).NotEmpty().WithMessage("Value must not be empty");
            }
        }

        [Fact]
        public async Task ValidationBehavior_Should_Invoke_Next_When_Request_Is_Valid()
        {
            List<IValidator<TestRequest>> validators = [new TestValidator()];
            ValidationBehavior<TestRequest, string> behavior = new(validators);
            TestRequest request = new()
            {
                Value = "Valid"
            };

            bool nextCalled = false;

            Task<string> Next(CancellationToken t)
            {
                nextCalled = true;
                return Task.FromResult("Success");
            }
            string result = await behavior.Handle(request, Next, CancellationToken.None);

            nextCalled.ShouldBeTrue();
            result.ShouldBe("Success");
        }

        [Fact]
        public async Task ValidationBehavior_Should_Throw_When_Request_Is_Invalid()
        {
            List<IValidator<TestRequest>> validators = [new TestValidator()];

            ValidationBehavior<TestRequest, string> behavior = new(validators);

            TestRequest request = new()
            {
                Value = ""
            };

            Func<Task> act = async () => await behavior.Handle(request, (CancellationToken t) => Task.FromResult("Success"), CancellationToken.None);

            _ = await act.ShouldThrowAsync<ValidationException>();
        }
    }
}
