using System;
using System.Threading.Tasks;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.Commands;
using JV.Utilities.Wpf.Commands.Interfaces;

namespace JV.Utilities.Wpf.Tests.Commands
{
    [TestFixture]
    public class AsyncDelegateCommandTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext
        {
            public TestContext()
            {
                executeAsync = Substitute.For<Func<Task>>();
                canExecute = Substitute.For<Func<bool>>();
                commandManager = Substitute.For<ICommandManager>();
            }

            public Func<Task> executeAsync;
            public Func<bool> canExecute;
            public ICommandManager commandManager;

            public AsyncDelegateCommand ConstructUUT_ExecuteAsync()
                => new AsyncDelegateCommand(executeAsync);

            public AsyncDelegateCommand ConstructUUT_ExecuteAsync_CanExecute()
                => new AsyncDelegateCommand(executeAsync, canExecute);

            public AsyncDelegateCommand ConstructUUT_ExecuteAsync_CanExecute_CommandManager()
                => new AsyncDelegateCommand(executeAsync, canExecute, commandManager);
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor(executeAsync) Tests

        [Test]
        public void Constructor_ExecuteAsync_ExecuteAsyncIsNull_ThrowsException()
        {
            var context = new TestContext()
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
            var context = new TestContext();

            var uut = context.ConstructUUT_ExecuteAsync();

            var parameter = (object)null;

            uut.Execute(parameter);

            context.executeAsync.Received(1).Invoke();
        }

        #endregion Constructor(executeAsync) Tests

        /**********************************************************************/
        #region Constructor(executeAsync, canExecute) Tests

        [Test]
        public void Constructor_ExecuteAsyncCanExecute_ExecuteAsyncIsNull_ThrowsException()
        {
            var context = new TestContext()
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
            var context = new TestContext();

            var uut = context.ConstructUUT_ExecuteAsync_CanExecute();

            var parameter = (object)null;

            uut.Execute(parameter);
            uut.CanExecute(parameter);

            context.ShouldSatisfyAllConditions(
                () => context.executeAsync.Received(1).Invoke(),
                () => context.canExecute.Received(1).Invoke());
        }

        #endregion Constructor(executeAsync, canExecute) Tests

        /**********************************************************************/
        #region Constructor(executeAsync, canExecute, commandManager) Tests

        [Test]
        public void Constructor_ExecuteAsyncCanExecuteCommandManager_ExecuteAsyncIsNull_ThrowsException()
        {
            var context = new TestContext()
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
            var context = new TestContext();

            var uut = context.ConstructUUT_ExecuteAsync_CanExecute_CommandManager();

            var parameter = (object)null;
            var handler = Substitute.For<EventHandler>();

            uut.Execute(parameter);
            uut.CanExecute(parameter);
            uut.CanExecuteChanged += handler;

            context.ShouldSatisfyAllConditions(
                () => context.executeAsync.Received(1).Invoke(),
                () => context.canExecute.Received(1).Invoke(),
                () => context.commandManager.Received(1).RequerySuggested += handler);
        }

        #endregion Constructor(executeAsync, canExecute, commandManager) Tests

        /**********************************************************************/
        #region ExecuteAsync Tests

        [TestCase(null)]
        [TestCase("parameter")]
        [TestCase(1)]
        public void ExecuteAsync_Always_InvokesExecuteAsync(object parameter)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_ExecuteAsync_CanExecute_CommandManager();

            uut.ExecuteAsync(parameter);

            context.executeAsync.Received(1).Invoke();
        }

        [TestCase(null)]
        [TestCase("parameter")]
        [TestCase(1)]
        public void ExecuteAsync_Always_ReturnsExecuteAsync(object parameter)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_ExecuteAsync_CanExecute_CommandManager();

            var task = Task.FromResult(0);
            context.executeAsync.Invoke().Returns(task);

            uut.ExecuteAsync(parameter).ShouldBeSameAs(task);
        }

        #endregion ExecuteAsync Tests
    }
}
