using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPFSplitView.Animations
{
    public class GridLengthAnimation : GridLengthAnimationBase
    {
        public GridLengthAnimation() : base()
        {
            
        }

        public GridLengthAnimation(GridLength toValue, Duration duration) 
            : this()
        {
            To = toValue;
            Duration = duration;
        }

        public GridLengthAnimation(GridLength toValue, Duration duration, FillBehavior fillBehavior)
            : this(toValue, duration)
        {
            FillBehavior = fillBehavior;
        }

        public GridLengthAnimation(GridLength fromValue, GridLength toValue, Duration duration)
             : this(toValue, duration)
        {
            From = fromValue;
        }

        public GridLengthAnimation(GridLength fromValue, GridLength toValue, Duration duration, FillBehavior fillBehavior) 
            : this(fromValue, toValue, duration)
        {
            FillBehavior = fillBehavior;
        }        

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }
        
        protected override GridLength GetCurrentValueCore(GridLength defaultOriginValue, GridLength defaultDestinationValue, AnimationClock animationClock)
        {
            if (From == null && To == null && By == null)
                throw new Exception("Unknown animation type");

            GridLength from = From ?? defaultOriginValue;
            GridLength to = To ?? defaultDestinationValue;
            
            double? progress = animationClock.CurrentProgress;   // What if it is null? Is GetCurrentValueCore ever called?
                                                                 //https://msdn.microsoft.com/en-us/library/system.windows.media.animation.clock.currentprogress(v=vs.110).aspx
            if (EasingFunction != null)
                progress = EasingFunction.Ease((double)progress);

            if (To == null && By != null)
            {
                if (From == null)
                    To = new GridLength(defaultOriginValue.Value + (double)By);
                else
                    To = new GridLength(From.Value.Value + (double)By);
            }

            if (from.GridUnitType != to.GridUnitType)
                return to;

            return new GridLength(from.Value + (to.Value - from.Value) * progress.Value, from.GridUnitType);
        }

        public override bool IsDestinationDefault
        {
            get
            {
                return false; // TODO: Implement
            }
        }

        public GridLength? From
        {
            get { return (GridLength?)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }
        
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(GridLength?), typeof(GridLengthAnimation), new PropertyMetadata(null));

        public GridLength? To
        {
            get { return (GridLength?)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }
        
        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(GridLength?), typeof(GridLengthAnimation), new PropertyMetadata(null));

        public double? By
        {
            get { return (double?)GetValue(ByProperty); }
            set { SetValue(ByProperty, value); }
        }
        
        public static readonly DependencyProperty ByProperty =
            DependencyProperty.Register("By", typeof(double?), typeof(GridLengthAnimation), new PropertyMetadata(null));

        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }
        
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(GridLengthAnimation));
        
    }
}
