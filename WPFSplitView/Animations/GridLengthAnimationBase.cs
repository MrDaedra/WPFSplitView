using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPFSplitView.Animations
{
    public abstract class GridLengthAnimationBase : AnimationTimeline
    {
        public override Type TargetPropertyType
        {
            get
            {
                return typeof(GridLength);
            }
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {            
            GridLength origin;
            GridLength destination;
            if (defaultOriginValue is GridLength)
                origin = (GridLength)defaultOriginValue;
            else
                throw new ArgumentException("Wrong argument type in GetCurrentValue", "OriginValue");
            if (defaultDestinationValue is GridLength)
                destination = (GridLength)defaultDestinationValue;
            else
                throw new ArgumentException("Wrong argument type in GetCurrentValue", "DestinationValue");
            return GetCurrentValueCore(origin, destination, animationClock);
        }

        protected abstract GridLength GetCurrentValueCore(GridLength defaultOriginValue, GridLength defaultDestinationValue, AnimationClock animationClock);
    }
}
