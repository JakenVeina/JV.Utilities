using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Observation;

namespace JV.Utilities.Tests.Observation
{
    [TestFixture]
    public class DisposeInvokerTests
    {
        /**********************************************************************/
        #region Constructor Tests

        [Test]
        public void Constructor_ActionIsNull_ThrowsException()
        {
            var action = (Action)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                new DisposeInvoker(action);
            });

            result.ParamName.ShouldBe(nameof(action));
        }

        #endregion Constructor Tests

        /**********************************************************************/
        #region Dispose Tests

        [Test]
        public void Dispose_DisposeHasNotBeenInvoked_InvokesAction()
        {
            var action = Substitute.For<Action>();

            var uut = new DisposeInvoker(action);

            uut.Dispose();

            action.Received(1).Invoke();
        }

        [Test]
        public void Dispose_DisposeHasBeenInvoked_InvokesAction()
        {
            var action = Substitute.For<Action>();

            var uut = new DisposeInvoker(action);
            uut.Dispose();
            action.ClearReceivedCalls();

            uut.Dispose();

            action.DidNotReceive().Invoke();
        }

        #endregion Dispose Tests
    }
}
