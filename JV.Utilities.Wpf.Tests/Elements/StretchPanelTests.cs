using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Extensions;

using JV.Utilities.Wpf.Elements;

namespace JV.Utilities.Wpf.Tests.Elements
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class StretchPanelTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext
        {
            public Orientation orientation;
            public FakeFrameworkElement[] children;

            public StretchPanel ConstructUUT()
            {
                var uut = new StretchPanel();

                uut.Orientation = orientation;

                children?.ForEach(x => uut.Children.Add(x));

                return uut;
            }
        }

        private class FakeFrameworkElement : FrameworkElement
        {
            public Size MeasureOverride_AvailableSize;

            private Size? _desiredSizeOverride = null;

            public void SetDesiredSize(Size desiredSize)
            {
                _desiredSizeOverride = desiredSize;
                Measure(desiredSize);
            }

            protected override Size MeasureOverride(Size availableSize)
            {
                MeasureOverride_AvailableSize = availableSize;

                if (_desiredSizeOverride.HasValue)
                    return _desiredSizeOverride.Value;

                return ((availableSize.Width == double.PositiveInfinity) || (availableSize.Height == double.PositiveInfinity)) ? 
                       new Size() : 
                       availableSize;
            }

            protected override Size ArrangeOverride(Size finalSize)
                => finalSize;
        }

        #endregion Test Context

        /**********************************************************************/
        #region Test Data

        private static readonly object[] TestCases_SizeAndChildrenEachSpan =
        {
            new object[] { 1.0, 2.0,  new[] { 1.0 } },
            new object[] { 1.0, 2.0,  new[] { 1.0, 1.0, 1.0 } },
            new object[] { 1.0, 2.0,  new[] { 1.0, 2.0, 3.0 } }
        };

        #endregion Test Data

        /**********************************************************************/
        #region Test Procedures

        private static FakeFrameworkElement[] MakeFakeChildren(IEnumerable<double> eachSpan)
            => eachSpan.Select(x => new { Element = new FakeFrameworkElement(), Span = x })
                       .Do(x => StretchPanel.SetSpan(x.Element, x.Span))
                       .Select(x => x.Element)
                       .ToArray();

        private static FakeFrameworkElement[] MakeFakeChildren(IEnumerable<Size> eachSize)
            => eachSize.Select(x => new { Element = new FakeFrameworkElement(), Size = x })
                       .Do(x => x.Element.SetDesiredSize(x.Size))
                       .Select(x => x.Element)
                       .ToArray();

        private static Size[] MakeSizeArray(IEnumerable<double> sizesFlattened)
            => sizesFlattened.Partition(2)
                             .Select(x => x.ToArray())
                             .Where(x => (x.Length > 0))
                             .Select(x => new Size(x[0], x[1]))
                             .ToArray();

        #endregion Test Procedures

        /**********************************************************************/
        #region Span Tests

        [Test]
        public void SpanProperty_ByDefault_IsNotNull()
        {
            StretchPanel.SpanProperty.ShouldNotBeNull();
        }

        [TestCase(1.2)]
        public void SetSpan_Always_InvokesSetValueWithSpanProperty(double value)
        {
            var uut = new FrameworkElement();

            StretchPanel.SetSpan(uut, value);

            uut.GetValue(StretchPanel.SpanProperty).ShouldBe(value);
        }

        [TestCase(-1.0)]
        public void SetSpan_ValueIsNegative_ThrowsException(double value)
        {
            var uut = new FrameworkElement();

            var result = Should.Throw<ArgumentException>(() =>
            {
                StretchPanel.SetSpan(uut, value);
            });

            result.ShouldSatisfyAllConditions(
                () => result.Message.ShouldContain(StretchPanel.SpanProperty.Name),
                () => result.Message.ShouldContain(value.ToString()));
        }

        [TestCase(1.2)]
        public void GetSpan_Always_InvokesGetValueWithSpanProperty(double value)
        {
            var uut = new FrameworkElement();

            uut.SetValue(StretchPanel.SpanProperty, value);

            StretchPanel.GetSpan(uut).ShouldBe(value);
        }

        #endregion Span Tests

        /**********************************************************************/
        #region Orientation Tests
        
        [Test]
        public void OrientationProperty_ByDefault_IsNotNull()
        {
            StretchPanel.OrientationProperty.ShouldNotBeNull();
        }

        [TestCase(Orientation.Vertical)]
        [TestCase(Orientation.Horizontal)]
        public void OrientationSet_Always_InvokesSetValueWithOrientationProperty(Orientation value)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            uut.Orientation = value;

            uut.GetValue(StretchPanel.OrientationProperty).ShouldBe(value);
        }

        [TestCase(-1)]
        public void Orientation_ValueIsInvalid_ThrowsException(Orientation value)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            var result = Should.Throw<ArgumentException>(() =>
            {
                uut.Orientation = value;
            });

            result.ShouldSatisfyAllConditions(
                () => result.Message.ShouldContain(nameof(uut.Orientation)),
                () => result.Message.ShouldContain(value.ToString()));
        }

        [TestCase(Orientation.Vertical)]
        [TestCase(Orientation.Horizontal)]
        public void OrientationGet_Always_InvokesGetValueWithOrientationProperty(Orientation value)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            uut.SetValue(StretchPanel.OrientationProperty, value);

            uut.Orientation.ShouldBe(value);
        }

        #endregion Orientation Tests

        /**********************************************************************/
        #region MeasureOverride Tests

        [TestCase(Orientation.Vertical,   1.0)]
        [TestCase(Orientation.Horizontal, 1.0)]
        [TestCase(Orientation.Vertical,   1.0, 1.0, 1.0)]
        [TestCase(Orientation.Horizontal, 1.0, 1.0, 1.0)]
        [TestCase(Orientation.Vertical,   1.0, 2.0, 3.0)]
        [TestCase(Orientation.Horizontal, 1.0, 2.0, 3.0)]
        public void MeasureOverride_AvailableSizeIsInfinity_InvokesChildrenEachMeasure(Orientation orientation, params double[] childrenEachSpan)
        {
            var context = new TestContext()
            {
                children = MakeFakeChildren(childrenEachSpan)
            };
            var uut = context.ConstructUUT();

            var availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

            uut.Measure(availableSize);

            context.children.Select(x => x.MeasureOverride_AvailableSize).ShouldAllBe(x => x == availableSize);
        }

        [TestCaseSource(nameof(TestCases_SizeAndChildrenEachSpan))]
        public void MeasureOverride_OrientationIsVertial_InvokesChildrenEachMeasure(double availableSizeWidth, double availableSizeHeight, params double[] childrenEachSpan)
        {
            var context = new TestContext()
            {
                orientation = Orientation.Vertical,
                children = MakeFakeChildren(childrenEachSpan)
            };
            var uut = context.ConstructUUT();

            var totalSpan = childrenEachSpan.Sum();

            var availableSize = new Size(availableSizeWidth, availableSizeHeight);
            uut.Measure(availableSize);

            context.children.ShouldSatisfyAllConditions(
                () => context.children.ForEach(x => x.MeasureOverride_AvailableSize.Width.ShouldBe(availableSizeWidth)),
                () => context.children.ForEach(x => x.MeasureOverride_AvailableSize.Height.ShouldBe(availableSizeHeight * StretchPanel.GetSpan(x) / totalSpan)));
        }

        [TestCaseSource(nameof(TestCases_SizeAndChildrenEachSpan))]
        public void MeasureOverride_OrientationIsHorizontal_InvokesChildrenEachMeasure(double availableSizeWidth, double availableSizeHeight, params double[] childrenEachSpan)
        {
            var context = new TestContext()
            {
                orientation = Orientation.Horizontal,
                children = MakeFakeChildren(childrenEachSpan)
            };
            var uut = context.ConstructUUT();

            var totalSpan = childrenEachSpan.Sum();

            var availableSize = new Size(availableSizeWidth, availableSizeHeight);
            uut.Measure(availableSize);

            context.children.ShouldSatisfyAllConditions(
                () => context.children.ForEach(x => x.MeasureOverride_AvailableSize.Width.ShouldBe(availableSizeWidth * StretchPanel.GetSpan(x) / totalSpan)),
                () => context.children.ForEach(x => x.MeasureOverride_AvailableSize.Height.ShouldBe(availableSizeHeight)));
        }

        [TestCase(Orientation.Vertical,   0.0,  0.0)]
        [TestCase(Orientation.Horizontal, 0.0,  0.0)]
        [TestCase(Orientation.Vertical,   1.0,  2.0,  1.0, 2.0)]
        [TestCase(Orientation.Horizontal, 1.0,  2.0,  1.0, 2.0)]
        [TestCase(Orientation.Vertical,   3.0,  6.0,  1.0, 2.0,  3.0, 4.0)]
        [TestCase(Orientation.Horizontal, 4.0,  4.0,  1.0, 2.0,  3.0, 4.0)]
        [TestCase(Orientation.Vertical,   5.0, 12.0,  1.0, 2.0,  3.0, 4.0,  5.0, 6.0)]
        [TestCase(Orientation.Horizontal, 9.0,  6.0,  1.0, 2.0,  3.0, 4.0,  5.0, 6.0)]
        public void MeasureOverride_Always_ReturnsExpected(Orientation orientation, double expectedResultWidth, double expectedResultHeight, params double[] childrenEachDesiredSizeFlattened)
        {
            var context = new TestContext()
            {
                orientation = orientation,
                children = MakeFakeChildren(MakeSizeArray(childrenEachDesiredSizeFlattened))
            };
            var uut = context.ConstructUUT();

            uut.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            uut.DesiredSize.ShouldBe(new Size(expectedResultWidth, expectedResultHeight));
        }

        #endregion MeasureOverride Tests

        /**********************************************************************/
        #region ArrangeOverride Tests

        [TestCaseSource(nameof(TestCases_SizeAndChildrenEachSpan))]
        public void ArrangeOverride_OrientationIsVertial_InvokesChildrenEachArrangeWithFinalRectSizeExpected(double finalSizeWidth, double finalSizeHeight, params double[] childrenEachSpan)
        {
            var context = new TestContext()
            {
                orientation = Orientation.Vertical,
                children = MakeFakeChildren(childrenEachSpan)
            };
            var uut = context.ConstructUUT();

            var totalSpan = childrenEachSpan.Sum();

            var finalSize = new Size(finalSizeWidth, finalSizeHeight);
            uut.Arrange(new Rect(finalSize));

            context.children.ShouldSatisfyAllConditions(
                () => context.children.ForEach(x => x.ActualWidth.ShouldBe(finalSizeWidth)),
                () => context.children.ForEach(x => x.ActualHeight.ShouldBe(finalSizeHeight * StretchPanel.GetSpan(x) / totalSpan)));
        }

        [TestCaseSource(nameof(TestCases_SizeAndChildrenEachSpan))]
        public void ArrangeOverride_OrientationIsHorizontal_InvokesChildrenEachArrangeWithFinalRectSizeExpected(double finalSizeWidth, double finalSizeHeight, params double[] childrenEachSpan)
        {
            var context = new TestContext()
            {
                orientation = Orientation.Horizontal,
                children = MakeFakeChildren(childrenEachSpan)
            };
            var uut = context.ConstructUUT();

            var totalSpan = childrenEachSpan.Sum();

            var finalSize = new Size(finalSizeWidth, finalSizeHeight);
            uut.Arrange(new Rect(finalSize));

            context.children.ShouldSatisfyAllConditions(
                () => context.children.ForEach(x => x.ActualWidth.ShouldBe(finalSizeWidth * StretchPanel.GetSpan(x) / totalSpan)),
                () => context.children.ForEach(x => x.ActualHeight.ShouldBe(finalSizeHeight)));
        }

        [TestCaseSource(nameof(TestCases_SizeAndChildrenEachSpan))]
        public void ArrangeOverride_OrientationIsVertial_InvokesChildrenEachArrangeWithFinalRectLocationExpected(double finalSizeWidth, double finalSizeHeight, params double[] childrenEachSpan)
        {
            var context = new TestContext()
            {
                orientation = Orientation.Vertical,
                children = MakeFakeChildren(childrenEachSpan)
            };
            var uut = context.ConstructUUT();

            var totalSpan = childrenEachSpan.Sum();

            var finalSize = new Size(finalSizeWidth, finalSizeHeight);
            uut.Arrange(new Rect(finalSize));

            var childY = 0.0;

            context.children.ShouldSatisfyAllConditions(
                () => context.children.ForEach(x => x.TranslatePoint(new Point(), uut).X.ShouldBe(0)),
                () => context.children.Do(x => x.TranslatePoint(new Point(), uut).Y.ShouldBe(childY))
                                      .Do(x => childY += x.ActualHeight)
                                      .ForEach());
        }

        [TestCaseSource(nameof(TestCases_SizeAndChildrenEachSpan))]
        public void ArrangeOverride_OrientationIsHorizontal_InvokesChildrenEachArrangeWithFinalRectLocationExpected(double finalSizeWidth, double finalSizeHeight, params double[] childrenEachSpan)
        {
            var context = new TestContext()
            {
                orientation = Orientation.Horizontal,
                children = MakeFakeChildren(childrenEachSpan)
            };
            var uut = context.ConstructUUT();

            var totalSpan = childrenEachSpan.Sum();

            var finalSize = new Size(finalSizeWidth, finalSizeHeight);
            uut.Arrange(new Rect(finalSize));

            var childX = 0.0;

            context.children.ShouldSatisfyAllConditions(
                () => context.children.Do(x => x.TranslatePoint(new Point(), uut).X.ShouldBe(childX))
                                      .Do(x => childX += x.ActualWidth)
                                      .ForEach(),
                () => context.children.ForEach(x => x.TranslatePoint(new Point(), uut).Y.ShouldBe(0)));
        }

        [TestCase(Orientation.Vertical,   0.0, 0.0,  5.0, 6.0, 7.0)]
        [TestCase(Orientation.Horizontal, 0.0, 0.0,  5.0, 6.0, 7.0)]
        [TestCase(Orientation.Vertical,   1.0, 2.0,  5.0, 6.0, 7.0)]
        [TestCase(Orientation.Horizontal, 1.0, 2.0,  5.0, 6.0, 7.0)]
        [TestCase(Orientation.Vertical,   3.0, 4.0,  5.0, 6.0, 7.0)]
        [TestCase(Orientation.Horizontal, 3.0, 4.0,  5.0, 6.0, 7.0)]
        public void ArrangeOverride_Always_ReturnsFinalSize(Orientation orientation, double finalSizeWidth, double finalSizeHeight, params double[] childrenEachSpan)
        {
            var context = new TestContext()
            {
                orientation = orientation,
                children = MakeFakeChildren(childrenEachSpan)
            };
            var uut = context.ConstructUUT();

            var totalSpan = childrenEachSpan.Sum();

            var finalSize = new Size(finalSizeWidth, finalSizeHeight);
            uut.Arrange(new Rect(finalSize));

            uut.ShouldSatisfyAllConditions(
                () => uut.ActualWidth.ShouldBe(finalSize.Width),
                () => uut.ActualHeight.ShouldBe(finalSize.Height));
        }

        #endregion ArrangeOverride Tests
    }
}
