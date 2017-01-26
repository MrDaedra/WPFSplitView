using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xaml;

namespace WPFSplitView.Extensions
{
    [MarkupExtensionReturnType(typeof(object))]
    [ContentProperty("Path")]
    public class InitializerExtension : MarkupExtension
    {
        public PropertyPath Path { get; set; }

        public IValueConverter Converter { get; set; }

        public object Default { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                return DefaultOrValue(this);
            Type targetType = null;

            try
            {
                targetType = GetTargetType(serviceProvider);
            }
            catch (NullReferenceException)
            {
                return DefaultOrValue(this);
            }

            object source = null;

            try
            {
                source = GetSourceObject(serviceProvider);
            }
            catch (NullReferenceException)
            {
                return DefaultOrValue(GetDefault(targetType));
            }

            source = GetSourcePropertyValue(source);
            if (Converter != null)
                return Converter.Convert(source, targetType, null, CultureInfo.CurrentUICulture);
            return source;
        }

        object GetSourceObject(IServiceProvider serviceProvider)
        {
            IRootObjectProvider rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            FrameworkElement rootObject = rootObjectProvider?.RootObject as FrameworkElement;
            if (rootObject == null)
                throw new NullReferenceException();
            if (ElementName != null)
                return rootObject.FindName(ElementName);
            else
                return rootObject;
        }

        object GetSourcePropertyValue(object sourceObject)
        {
            IEnumerable<string> properties = Path.Path.Split('.');
            if (Path != null)
            {
                foreach (var property in properties)
                {
                    sourceObject = GetPropertyValue(sourceObject, property);
                }
            }
            return sourceObject;
        }

        Type GetTargetType(IServiceProvider serviceProvider)
        {
            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget == null)
                throw new NullReferenceException();

            DependencyObject targetObject = provideValueTarget.TargetObject as DependencyObject;

            if (targetObject == null)
                throw new NullReferenceException();
            
            object targetProperty = provideValueTarget.TargetProperty;
            Type targetType = targetProperty.GetType();
            if (targetProperty is DependencyProperty)
            {                
                targetType = ((DependencyProperty)targetProperty).PropertyType;
            }
            return targetType;
        }  
        
        public object DefaultOrValue(object value)
        {
            if (Default != null)
                return Default;
            return value;
        }      

        public object GetPropertyValue(object source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName).GetValue(source, null);
        }

        public string ElementName { get; set; }

        object GetDefault(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            return null;
        }
    }
}
