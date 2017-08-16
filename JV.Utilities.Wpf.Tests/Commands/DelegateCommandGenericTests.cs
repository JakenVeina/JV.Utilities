using System;
using System.Linq;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.Commands;
using JV.Utilities.Wpf.Commands.Interfaces;

namespace JV.Utilities.Wpf.Tests.Commands
{
    [TestFixture]
    public class DelegateCommandGenericTests
    {
        /**********************************************************************/
        #region Test Context
        
        private class TestContext<T>
        {
            public TestContext()
            {
                execute = Substitute.For<Action<T>>();
                canExecute = Substitute.For<Predicate<T>>();
                commandManager = Substitute.For<ICommandManager>();
            }

            public Action<T> execute;
            public Predicate<T> canExecute;
            public ICommandManager commandManager;

            public DelegateCommand<T> ConstructUUT_Execute()
                => new DelegateCommand<T>(execute);

            public DelegateCommand<T> ConstructUUT_Execute_CanExecute()
                => new DelegateCommand<T>(execute, canExecute);

            public DelegateCommand<T> ConstructUUT_Execute_CanExecute_CommandManager()
                => new DelegateCommand<T>(execute, canExecute, commandManager);
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor(execute) Tests

        [Test]
        public void Constructor_Execute_ExecuteIsNull_ThrowsException()
        {
            var context = new TestContext<string>()
            {
                execute = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT_Execute();
            });

            result.ParamName.ShouldBe(nameof(context.execute));
        }

        [Test]
        public void Constructor_Execute_Otherwise_DoesNotThrowException()
        {
            var context = new TestContext<string>();

            Should.NotThrow(() => context.ConstructUUT_Execute());
        }

        #endregion Constructor(execute) Tests

        /**********************************************************************/
        #region Constructor(execute, canExecute) Tests

        [Test]
        public void Constructor_ExecuteCanExecute_ExecuteIsNull_ThrowsException()
        {
            var context = new TestContext<string>()
            {
                execute = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT_Execute_CanExecute();
            });

            result.ParamName.ShouldBe(nameof(context.execute));
        }

        [Test]
        public void Constructor_ExecuteCanExecute_Otherwise_DoesNotThrowException()
        {
            var context = new TestContext<string>();

            Should.NotThrow(() => context.ConstructUUT_Execute_CanExecute());
        }

        #endregion Constructor(execute, canExecute) Tests

        /**********************************************************************/
        #region Constructor(execute, canExecute, commandManager) Tests

        [Test]
        public void Constructor_ExecuteCanExecuteCommandManager_ExecuteIsNull_ThrowsException()
        {
            var context = new TestContext<string>()
            {
                execute = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();
            });

            result.ParamName.ShouldBe(nameof(context.execute));
        }

        [Test]
        public void Constructor_ExecuteCanExecuteCommandManager_Otherwise_DoesNotThrowException()
        {
            var context = new TestContext<string>();

            Should.NotThrow(() => context.ConstructUUT_Execute_CanExecute_CommandManager());
        }

        #endregion Constructor(execute, canExecute, commandManager) Tests

        /**********************************************************************/
        #region RaiseCanExecuteChanged Tests

        [Test]
        public void RaiseCanExecuteChanged_CanExecuteChangedIsNull_DoesNotThrowException()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            Should.NotThrow(() =>
            {
                uut.RaiseCanExecuteChanged();
            });
        }

        [Test]
        public void RaiseCanExecuteChanged_Always_RaisesCanExecuteChanged()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged += handler;

            uut.RaiseCanExecuteChanged();

            handler.Received(1).Invoke(uut, Arg.Any<EventArgs>());
        }

        #endregion RaiseCanExecuteChanged Tests

        /**********************************************************************/
        #region Execute Tests

        [Test]
        public void Execute_ParameterIsNullAndTIsNotNullable_ThrowsException()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var parameter = (object)null;

            var result = Should.Throw<ArgumentException>(() =>
            {
                uut.Execute(parameter);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(parameter)),
                () => result.Message.ShouldContain("null"),
                () => result.Message.ShouldContain(uut.GetType().GetGenericArguments().First().Name));
        }

        [Test]
        public void Execute_ParameterIsNotNullAndNotInstanceOfT_ThrowsException()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var parameter = 1;

            var result = Should.Throw<ArgumentException>(() =>
            {
                uut.Execute(parameter);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(parameter)),
                () => result.Message.ShouldContain(parameter.GetType().Name),
                () => result.Message.ShouldContain(uut.GetType().GetGenericArguments().First().Name));
        }

        [TestCase(null)]
        [TestCase("parameter")]
        public void Execute_Otherwise_InvokesExecuteWithParameter(string parameter)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            uut.Execute(parameter);

            context.execute.Received(1).Invoke(parameter);
        }

        #endregion Execute Tests

        /**********************************************************************/
        #region CanExecute Tests

        [Test]
        public void CanExecute_ParameterIsNullAndTIsNotNullable_ReturnsFalse()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var parameter = (string)null;

            uut.CanExecute(parameter).ShouldBeFalse();
        }

        [Test]
        public void CanExecute_ParameterIsNotNullAndNotInstanceOfT_ReturnsFalse()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var parameter = 1;

            uut.CanExecute(parameter).ShouldBeFalse();
        }

        [TestCase(null)]
        [TestCase("parameter")]
        public void CanExecute_CanExecuteIsNull_ReturnsTrue(string parameter)
        {
            var context = new TestContext<string>()
            {
                canExecute = null
            };
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            uut.CanExecute(parameter).ShouldBeTrue();
        }

        [TestCase(null)]
        [TestCase("parameter")]
        public void CanExecute_Otherwise_InvokesCanExecute(string parameter)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            uut.CanExecute(parameter);

            context.canExecute.Received(1).Invoke(parameter);
        }

        [Test, Combinatorial]
        public void CanExecute_Otherwise_ReturnsCanExecute([Values(null, "parameter")] string parameter, [Values(true, false)] bool expected)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            context.canExecute.Invoke(Arg.Any<string>()).Returns(expected);

            uut.CanExecute(parameter).ShouldBe(expected);
        }

        #endregion CanExecute Tests

        /**********************************************************************/
        #region CanExecuteChanged Tests

        [Test]
        public void CanExecuteChangedAdd_CommandManagerIsNull_DoesNotThrowException()
        {
            var context = new TestContext<string>()
            {
                commandManager = null
            };
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();

            Should.NotThrow(() => uut.CanExecuteChanged += handler);
        }

        [Test]
        public void CanExecuteChangedRemove_CommandManagerIsNull_DoesNotThrowException()
        {
            var context = new TestContext<string>()
            {
                commandManager = null
            };
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();

            Should.NotThrow(() => uut.CanExecuteChanged -= handler);
        }

        [Test]
        public void CanExecuteChangedAdd_CanExecuteIsNull_DoesNotInvokeCommandManagerRequerySuggestedAdd()
        {
            var context = new TestContext<string>()
            {
                canExecute = null
            };
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged += handler;

            context.commandManager.DidNotReceive().RequerySuggested += handler;
        }

        [Test]
        public void CanExecuteChangedRemove_CanExecuteIsNull_DoesNotInvokeCommandManagerRequerySuggestedRemove()
        {
            var context = new TestContext<string>()
            {
                canExecute = null
            };
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged -= handler;

            context.commandManager.DidNotReceive().RequerySuggested -= handler;
        }

        [Test]
        public void CanExecuteChangedAdd_CommandManagerIsNotNull_InvokesCommandManagerRequerySuggestedAdd()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged += handler;

            context.commandManager.Received(1).RequerySuggested += handler;
        }

        [Test]
        public void CanExecuteChangedRemove_CommandManagerIsNotNull_InvokesCommandManagerRequerySuggestedRemove()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged -= handler;

            context.commandManager.Received(1).RequerySuggested -= handler;
        }

        #endregion CanExecuteChanged Tests

        /**********************************************************************/
        #region ValidateParameterType Tests

        [Test]
        public void ValidateParameterType_ParameterIsNullAndTIsNotNullable_ReturnsFalse()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var parameter = (object)null;

            uut.ValidateParameterType(parameter).ShouldBeFalse();
        }

        [Test]
        public void ValidateParameterType_ParameterIsNullAndTIsNullable_ReturnsTrue()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var parameter = (object)null;

            uut.ValidateParameterType(parameter).ShouldBeTrue();
        }

        [Test]
        public void ValidateParameterType_ParameterIsNotNullAndNotInstanceOfT_ReturnsFalse()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var parameter = 1;

            uut.ValidateParameterType(parameter).ShouldBeFalse();
        }

        [TestCase(null)]
        [TestCase("parameter")]
        public void ValidateParameterType_Otherwise_ReturnsTrue(string parameter)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            uut.ValidateParameterType(parameter).ShouldBeTrue();
        }

        #endregion ValidateParameterType Tests

        /**********************************************************************/
        #region AssertParameterType Tests

        [Test]
        public void AssertParameterType_ParameterIsNullAndTIsNotNullable_ThrowsException()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var parameter = (object)null;

            var result = Should.Throw<ArgumentException>(() =>
            {
                uut.AssertParameterType(parameter);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(parameter)),
                () => result.Message.ShouldContain("null"),
                () => result.Message.ShouldContain(uut.GetType().GetGenericArguments().First().Name));
        }

        [Test]
        public void AssertParameterType_ParameterIsNotNullAndNotInstanceOfT_ThrowsException()
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var parameter = 1;

            var result = Should.Throw<ArgumentException>(() =>
            {
                uut.AssertParameterType(parameter);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(parameter)),
                () => result.Message.ShouldContain(parameter.GetType().Name),
                () => result.Message.ShouldContain(uut.GetType().GetGenericArguments().First().Name));
        }

        [TestCase(null)]
        [TestCase("parameter")]
        public void AssertParameterType_Otherwise_DoesNotThrowException(string parameter)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            Should.NotThrow(() =>
            {
                uut.AssertParameterType(parameter);
            });
        }

        #endregion AssertParameterType Tests
    }
}
