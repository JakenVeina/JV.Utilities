using System;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.Commands;
using JV.Utilities.Wpf.Commands.Interfaces;

namespace JV.Utilities.Wpf.Tests.Commands
{
    [TestFixture]
    public class DelegateCommandTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext
        {
            public TestContext()
            {
                execute = Substitute.For<Action>();
                canExecute = Substitute.For<Func<bool>>();
                commandManager = Substitute.For<ICommandManager>();
            }

            public Action execute;
            public Func<bool> canExecute;
            public ICommandManager commandManager;

            public DelegateCommand ConstructUUT_Execute()
                => new DelegateCommand(execute);

            public DelegateCommand ConstructUUT_Execute_CanExecute()
                => new DelegateCommand(execute, canExecute);

            public DelegateCommand ConstructUUT_Execute_CanExecute_CommandManager()
                => new DelegateCommand(execute, canExecute, commandManager);
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor(execute) Tests

        [Test]
        public void Constructor_Execute_ExecuteIsNull_ThrowsException()
        {
            var context = new TestContext()
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
            var context = new TestContext();

            Should.NotThrow(() => context.ConstructUUT_Execute());
        }

        #endregion Constructor(execute) Tests

        /**********************************************************************/
        #region Constructor(execute, canExecute) Tests

        [Test]
        public void Constructor_ExecuteCanExecute_ExecuteIsNull_ThrowsException()
        {
            var context = new TestContext()
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
            var context = new TestContext();

            Should.NotThrow(() => context.ConstructUUT_Execute_CanExecute());
        }

        #endregion Constructor(execute, canExecute) Tests

        /**********************************************************************/
        #region Constructor(execute, canExecute, commandManager) Tests

        [Test]
        public void Constructor_ExecuteCanExecuteCommandManager_ExecuteIsNull_ThrowsException()
        {
            var context = new TestContext()
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
            var context = new TestContext();

            Should.NotThrow(() => context.ConstructUUT_Execute_CanExecute_CommandManager());
        }

        #endregion Constructor(execute, canExecute, commandManager) Tests

        /**********************************************************************/
        #region RaiseCanExecuteChanged Tests

        [Test]
        public void RaiseCanExecuteChanged_CanExecuteChangedIsNull_DoesNotThrowException()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            Should.NotThrow(() => uut.RaiseCanExecuteChanged());
        }

        [Test]
        public void RaiseCanExecuteChanged_Otherwise_RaisesCanExecuteChanged()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged += handler;

            uut.RaiseCanExecuteChanged();

            handler.Received(1).Invoke(uut, Arg.Any<EventArgs>());
        }

        #endregion RaiseCanExecuteChanged Tests

        /**********************************************************************/
        #region Execute Tests

        [TestCase(null)]
        [TestCase("parameter")]
        [TestCase(1)]
        public void Execute_Always_InvokesExecute(object parameter)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            uut.Execute(parameter);

            context.execute.Received(1).Invoke();
        }

        #endregion Execute Tests

        /**********************************************************************/
        #region CanExecute Tests

        [TestCase(null)]
        [TestCase("parameter")]
        [TestCase(1)]
        public void CanExecute_CanExecuteIsNull_ReturnsTrue(object parameter)
        {
            var context = new TestContext()
            {
                canExecute = null
            };
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            uut.CanExecute(parameter).ShouldBeTrue();
        }

        [TestCase(null)]
        [TestCase("parameter")]
        [TestCase(1)]
        public void CanExecute_Otherwise_InvokesCanExecute(object parameter)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            uut.CanExecute(parameter);

            context.canExecute.Received(1).Invoke();
        }

        [Test, Combinatorial]
        public void CanExecute_Otherwise_ReturnsCanExecute([Values(null, "parameter", 1)] object parameter, [Values(true, false)] bool expected)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            context.canExecute.Invoke().Returns(expected);

            uut.CanExecute(parameter).ShouldBe(expected);
        }

        #endregion CanExecute Tests

        /**********************************************************************/
        #region CanExecuteChanged Tests

        [Test]
        public void CanExecuteChangedAdd_CommandManagerIsNull_DoesNotThrowException()
        {
            var context = new TestContext()
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
            var context = new TestContext()
            {
                commandManager = null
            };
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();

            Should.NotThrow(() => uut.CanExecuteChanged -= handler);
        }

        [Test]
        public void CanExecuteChangedAdd_CanExecuteIsNullAndCommandManagerIsNotNull_InvokesCommandManagerRequerySuggestedAdd()
        {
            var context = new TestContext()
            {
                canExecute = null
            };
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged += handler;

            context.commandManager.DidNotReceive().RequerySuggested += handler;
        }

        [Test]
        public void CanExecuteChangedRemove_CanExecuteIsNullAndCommandManagerIsNotNull_InvokesCommandManagerRequerySuggestedRemove()
        {
            var context = new TestContext()
            {
                canExecute = null
            };
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged -= handler;

            context.commandManager.DidNotReceive().RequerySuggested -= handler;
        }

        [Test]
        public void CanExecuteChangedAdd_CanExecuteIsNotNullAndCommandManagerIsNotNull_InvokesCommandManagerRequerySuggestedAdd()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged += handler;

            context.commandManager.Received(1).RequerySuggested += handler;
        }

        [Test]
        public void CanExecuteChangedRemove_CanExecuteIsNotNullAndCommandManagerIsNotNull_InvokesCommandManagerRequerySuggestedRemove()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Execute_CanExecute_CommandManager();

            var handler = Substitute.For<EventHandler>();
            uut.CanExecuteChanged -= handler;

            context.commandManager.Received(1).RequerySuggested -= handler;
        }

        #endregion CanExecuteChanged Tests
    }
}
