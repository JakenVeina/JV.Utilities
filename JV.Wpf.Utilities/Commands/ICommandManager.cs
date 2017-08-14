using System;
using System.Windows;
using System.Windows.Input;

namespace JV.Wpf.Utilities.Commands
{
    /// <summary>
    /// See <see cref="CommandManager"/>.
    /// </summary>
    public interface ICommandManager
    {
        /**********************************************************************/
        #region Properties

        /// <summary>
        /// See <see cref="CommandManager.CanExecuteEvent"/>.
        /// </summary>
        RoutedEvent CanExecuteEvent { get; }

        /// <summary>
        /// See <see cref="CommandManager.ExecutedEvent"/>.
        /// </summary>
        RoutedEvent ExecutedEvent { get; }

        /// <summary>
        /// See <see cref="CommandManager.PreviewCanExecuteEvent"/>.
        /// </summary>
        RoutedEvent PreviewCanExecuteEvent { get; }

        /// <summary>
        /// See <see cref="CommandManager.PreviewExecutedEvent"/>.
        /// </summary>
        RoutedEvent PreviewExecutedEvent { get; }

        #endregion Events

        /**********************************************************************/
        #region Events

        /// <summary>
        /// See <see cref="CommandManager.RequerySuggested"/>.
        /// </summary>
        event EventHandler RequerySuggested;

        #endregion Events

        /**********************************************************************/
        #region Methods

        /// <summary>
        /// See <see cref="CommandManager.AddCanExecuteHandler"/>.
        /// </summary>
        void AddCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler);

        /// <summary>
        /// See <see cref="CommandManager.AddExecutedHandler"/>.
        /// </summary>
        void AddExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler);

        /// <summary>
        /// See <see cref="CommandManager.AddPreviewCanExecuteHandler"/>.
        /// </summary>
        void AddPreviewCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler);

        /// <summary>
        /// See <see cref="CommandManager.AddPreviewExecutedHandler"/>.
        /// </summary>
        void AddPreviewExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler);

        /// <summary>
        /// See <see cref="CommandManager.InvalidateRequerySuggested"/>.
        /// </summary>
        void InvalidateRequerySuggested();

        /// <summary>
        /// See <see cref="CommandManager.RegisterClassCommandBinding"/>.
        /// </summary>
        void RegisterClassCommandBinding(Type type, CommandBinding commandBinding);

        /// <summary>
        /// See <see cref="CommandManager.RegisterClassInputBinding"/>.
        /// </summary>
        void RegisterClassInputBinding(Type type, InputBinding inputBinding);

        /// <summary>
        /// See <see cref="CommandManager.RemoveCanExecuteHandler"/>.
        /// </summary>
        void RemoveCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler);

        /// <summary>
        /// See <see cref="CommandManager.RemoveExecutedHandler"/>.
        /// </summary>
        void RemoveExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler);

        /// <summary>
        /// See <see cref="CommandManager.RemovePreviewCanExecuteHandler"/>.
        /// </summary>
        void RemovePreviewCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler);

        /// <summary>
        /// See <see cref="CommandManager.RemovePreviewExecutedHandler"/>.
        /// </summary>
        void RemovePreviewExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler);

        #endregion Methods
    }
}
