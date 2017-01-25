using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFSplitView
{
    public class SplitViewTemplateSettings : DependencyObject
    {
        public SplitViewTemplateSettings(SplitView splitView)
        {
            this.splitView = splitView;
        }

        SplitView splitView;        

        public GridLength OpenPaneGridLength
        {
            get { return new GridLength(OpenPaneLength); }
        }
        public double OpenPaneLength
        {
            get { return splitView.OpenPaneLength; }
        }   

        public double OpenPaneLengthMinusCompactLength
        {
            get { return OpenPaneLength - splitView.CompactPaneLength; }
        }        

        public GridLength CompactPaneGridLength
        {
            get { return new GridLength(splitView.CompactPaneLength); }
        }

        public double NegativeOpenPaneLength
        {
            get { return -OpenPaneLength; }
        }

        public double NegativeOpenPaneLengthMinusCompactLength
        {
            get { return -OpenPaneLengthMinusCompactLength; }
        }

    }
}
