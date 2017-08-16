using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;

using JV.Utilities.Wpf.Commands.Interfaces;

namespace JV.Utilities.Wpf.Commands
{
    /// <summary>
    /// Implements <see cref="ICommandManager"/> by wrapping the static members of <see cref="CommandManager"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class WpfCommandManager : ICommandManager
    {
        /**********************************************************************/
        #region Static Fields

        /// <summary>
        /// Default instance of <see cref="WpfCommandManager"/>.
        /// </summary>
        public static readonly ICommandManager Default = new WpfCommandManager();

        #endregion Static Fields

        /**********************************************************************/
        #region Constructors

        internal WpfCommandManager() { }

        #endregion Constructors

        /**********************************************************************/
        #region ICommandManager

        /// <summary>
        /// See <see cref="ICommandManager.CanExecuteEvent"/>.
        /// </summary>
        public RoutedEvent CanExecuteEvent
            => CommandManager.CanExecuteEvent;

        /// <summary>
        /// See <see cref="ICommandManager.ExecutedEvent"/>.
        /// </summary>
        public RoutedEvent ExecutedEvent
            => CommandManager.ExecutedEvent;

        /// <summary>
        /// See <see cref="ICommandManager.PreviewCanExecuteEvent"/>.
        /// </summary>
        public RoutedEvent PreviewCanExecuteEvent
            => CommandManager.PreviewCanExecuteEvent;

        /// <summary>
        /// See <see cref="ICommandManager.PreviewExecutedEvent"/>.
        /// </summary>
        public RoutedEvent PreviewExecutedEvent
            => CommandManager.PreviewExecutedEvent;

        /// <summary>
        /// See <see cref="ICommandManager.RequerySuggested"/>.
        /// </summary>
        public event EventHandler RequerySuggested
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// See <see cref="ICommandManager.AddCanExecuteHandler"/>.
        /// </summary>
        public void AddCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler)
            => CommandManager.AddCanExecuteHandler(element, handler);

        /// <summary>
        /// See <see cref="ICommandManager.AddExecutedHandler"/>.
        /// </summary>
        public void AddExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler)
            => CommandManager.AddExecutedHandler(element, handler);

        /// <summary>
        /// See <see cref="ICommandManager.AddPreviewCanExecuteHandler"/>.
        /// </summary>
        public void AddPreviewCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler)
            => CommandManager.AddPreviewCanExecuteHandler(element, handler);

        /// <summary>
        /// See <see cref="ICommandManager.AddPreviewExecutedHandler"/>.
        /// </summary>
        public void AddPreviewExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler)
            => CommandManager.AddPreviewExecutedHandler(element, handler);

        /// <summary>
        /// See <see cref="ICommandManager.InvalidateRequerySuggested"/>.
        /// </summary>
        public void InvalidateRequerySuggested()
            => CommandManager.InvalidateRequerySuggested();

        /// <summary>
        /// See <see cref="ICommandManager.RegisterClassCommandBinding"/>.
        /// </summary>
        public void RegisterClassCommandBinding(Type type, CommandBinding commandBinding)
            => CommandManager.RegisterClassCommandBinding(type, commandBinding);

        /// <summary>
        /// See <see cref="ICommandManager.RegisterClassInputBinding"/>.
        /// </summary>
        public void RegisterClassInputBinding(Type type, InputBinding inputBinding)
            => CommandManager.RegisterClassInputBinding(type, inputBinding);

        /// <summary>
        /// See <see cref="ICommandManager.RemoveCanExecuteHandler"/>.
        /// </summary>
        public void RemoveCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler)
            => CommandManager.RemoveCanExecuteHandler(element, handler);

        /// <summary>
        /// See <see cref="ICommandManager.RemoveExecutedHandler"/>.
        /// </summary>
        public void RemoveExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler)
            => CommandManager.RemoveExecutedHandler(element, handler);

        /// <summary>
        /// See <see cref="ICommandManager.RemovePreviewCanExecuteHandler"/>.
        /// </summary>
        public void RemovePreviewCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler)
            => CommandManager.RemovePreviewCanExecuteHandler(element, handler);

        /// <summary>
        /// See <see cref="ICommandManager.RemovePreviewExecutedHandler"/>.
        /// </summary>
        public void RemovePreviewExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler)
            => CommandManager.RemovePreviewExecutedHandler(element, handler);

        #endregion ICommandManager
    }
}
