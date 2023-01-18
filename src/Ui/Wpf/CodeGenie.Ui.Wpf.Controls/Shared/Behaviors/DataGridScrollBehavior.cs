using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CodeGenie.Ui.Wpf.Controls.Shared.Utilities;

namespace CodeGenie.Ui.Wpf.Controls.Shared.Behaviors
{
    public class DataGridScrollBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AttachScrollHandling;
        }

        AttachTracker<ScrollViewer> _attachedScrollViewer = new AttachTracker<ScrollViewer>();
        AttachTracker<INotifyCollectionChanged> _attachedCollection = new AttachTracker<INotifyCollectionChanged>();
        private void AttachScrollHandling(object sender, RoutedEventArgs e)
        {
            var scrollViewer = AssociatedObject.GetFirstChildElement<ScrollViewer>();
            if (scrollViewer != null)
            {
                if (!_attachedScrollViewer.WasPreviouslyAttached(scrollViewer))
                {
                    if (_attachedScrollViewer.Attached != null)
                    {
                        _attachedScrollViewer.Attached.MouseWheel -= Attached_MouseWheel;
                    }

                    _attachedScrollViewer.Attached = scrollViewer;
                    _attachedScrollViewer.Attached.MouseWheel += Attached_MouseWheel;
                }
            }

            if (AssociatedObject.Items is INotifyCollectionChanged collectionChanged)
            {
                if (!_attachedCollection.WasPreviouslyAttached(collectionChanged))
                {
                    if (_attachedCollection.Attached != null)
                    {
                        _attachedCollection.Attached.CollectionChanged -= ScrollToBottomIfNoScrollRequestReceived;
                    }

                    _attachedCollection.Attached = collectionChanged;
                    _attachedCollection.Attached.CollectionChanged += ScrollToBottomIfNoScrollRequestReceived;
                }
            }
        }


        private bool _wasScrolledToBottom = false;
        private void Attached_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                _wasScrolledToBottom = scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight;
            }
        }

        private bool _ignoreScrollEvents = false;
        private void ScrollToBottomIfNoScrollRequestReceived(object sender, NotifyCollectionChangedEventArgs collectionChangedArgs)
        {
            if (_wasScrolledToBottom) return;
            if (AssociatedObject.Items.Count <= 0) return;
            _ignoreScrollEvents = true;
            AssociatedObject.ScrollIntoView(AssociatedObject.Items[AssociatedObject.Items.Count - 1]);
            _ignoreScrollEvents = false;
        }
    }

    public class AttachTracker<TObject>
    {
        public TObject Attached { get; set; }
        public bool WasPreviouslyAttached(TObject target)
        {
            if (Attached == null) return false;
            if (target == null) return false;
            return ReferenceEquals(Attached, target);
        }
    }
}
