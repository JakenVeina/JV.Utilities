using System;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.Commands;
using JV.Utilities.Wpf.Commands.Interfaces;

namespace JV.Utilities.Wpf.Tests.Commands
{
    [TestFixture]
    public class AsyncDelegateCommandGenericTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext<T>
        {
            public TestContext()
            {
                executeAsync = Substitute.For<Func<T, Task>>();
                canExecute = Substitute.For<Predicate<T>>();
                commandManager = Substitute.For<ICommandManager>();
            }

            public Func<T, Task> executeAsync;
            public Predicate<T> canExecute;
            public ICommandManager commandManager;

            public AsyncDelegateCommand<T> ConstructUUT_ExecuteAsync()
                => new AsyncDelegateCommand<T>(executeAsync);

            public AsyncDelegateCommand<T> ConstructUUT_ExecuteAsync_CanExecute()
                => new AsyncDelegateCommand<T>(executeAsync, canExecute);

            public AsyncDelegateCommand<T> ConstructUUT_ExecuteAsync_CanExecute_CommandManager()
                => new AsyncDelegateCommand<T>(executeAsync, canExecute, commandManager);
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor(executeAsync) Tests

        [Test]
        public void Constructor_ExecuteAsync_ExecuteAsyncIsNull_ThrowsException()
        {
            var context = new TestContext<string>()
            {
                executeAsync = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT_ExecuteAsync();
            });

            result.ParamName.ShouldBe(nameof(context.executeAsync));
        }

        [Test]
        public void Constructor_ExecuteAsync_Otherwise_InvokesBaseConstructor()
        {
            var context = new TestContext<string>();

            var uut = context.ConstructUUT_ExecuteAsync();

            var parameter = "parameter";

            uut.Execute(parameter);

            context.executeAsync.Received(1).Invoke(parameter);
        }

        #endregion Constructor(executeAsync) Tests

        /**********************************************************************/
        #region Constructor(executeAsync, canExecute) Tests

        [Test]
        public void Constructor_ExecuteAsyncCanExecute_ExecuteAsyncIsNull_ThrowsException()
        {
            var context = new TestContext<string>()
            {
                executeAsync = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT_ExecuteAsync_CanExecute();
            });

            result.ParamName.ShouldBe(nameof(context.executeAsync));
        }

        [Test]
        public void Constructor_ExecuteAsyncCanExecute_Otherwise_InvokesBaseConstructor()
        {
            var context = new TestContext<string>();

            var uut = context.ConstructUUT_ExecuteAsync_CanExecute();

            var parameter = "parameter";

            uut.Execute(parameter);
            uut.CanExecute(parameter);

            context.ShouldSatisfyAllConditions(
                () => context.executeAsync.Received(1).Invoke(parameter),
                () => context.canExecute.Received(1).Invoke(parameter));
        }

        #endregion Constructor(executeAsync, canExecute) Tests

        /**********************************************************************/
        #region Constructor(executeAsync, canExecute, commandManager) Tests

        [Test]
        public void Constructor_ExecuteAsyncCanExecuteCommandManager_ExecuteAsyncIsNull_ThrowsException()
        {
            var context = new TestContext<string>()
            {
                executeAsync = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT_ExecuteAsync_CanExecute_CommandManager();
            });

            result.ParamName.ShouldBe(nameof(context.executeAsync));
        }

        [Test]
        public void Constructor_ExecuteAsyncCanExecuteCommandManager_Otherwise_InvokesBaseConstructor()
        {
            var context = new TestContext<string>();

            var uut = context.ConstructUUT_ExecuteAsync_CanExecute_CommandManager();

            var parameter = "parameter";
            var handler = Substitute.For<EventHandler>();

            uut.Execute(parameter);
            uut.CanExecute(parameter);
            uut.CanExecuteChanged += handler;

            context.ShouldSatisfyAllConditions(
                () => context.executeAsync.Received(1).Invoke(parameter),
                () => context.canExecute.Received(1).Invoke(parameter),
                () => context.commandManager.Received(1).RequerySuggested += handler);
        }

        #endregion Constructor(executeAsync, canExecute, commandManager) Tests

        /**********************************************************************/
        #region ExecuteAsync Tests

        [Test]
        public async Task ExecuteAsync_ParameterIsNullAndTIsNotNullable_ThrowsException()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT_ExecuteAsync_CanExecute_CommandManager();

            var parameter = (object)null;

            var result = await Should.ThrowAsync<ArgumentException>(async () =>
            {
                await uut.ExecuteAsync(parameter);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(parameter)),
                () => result.Message.ShouldContain("null"),
                () => result.Message.ShouldContain(uut.GetType().GetGenericArguments().First().Name));
        }

        [Test]
        public async Task ExecuteAsync_ParameterIsNotNullAndNotInstanceOfT_ThrowsException()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_ExecuteAsync_CanExecute_CommandManager();

            var parameter = 1;

            var result = await Should.ThrowAsync<ArgumentException>(async () =>
            {
                await uut.ExecuteAsync(parameter);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(parameter)),
                () => result.Message.ShouldContain(parameter.GetType().Name),
                () => result.Message.ShouldContain(uut.GetType().GetGenericArguments().First().Name));
        }

        [TestCase(null)]
        [TestCase("parameter")]
        public void ExecuteAsync_Otherwise_InvokesExecuteAsync(string parameter)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_ExecuteAsync_CanExecute_CommandManager();

            uut.ExecuteAsync(parameter);

            context.executeAsync.Received(1).Invoke(parameter);
        }

        [TestCase(null)]
        [TestCase("parameter")]
        public void ExecuteAsync_Otherwise_ReturnsExecuteAsync(string parameter)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_ExecuteAsync_CanExecute_CommandManager();

            var task = Task.FromResult(0);
            context.executeAsync.Invoke(Arg.Any<string>()).Returns(task);

            uut.ExecuteAsync(parameter).ShouldBeSameAs(task);
        }

        #endregion ExecuteAsync Tests
    }
}
