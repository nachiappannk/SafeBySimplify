using System;
using System.ComponentModel;
using NUnit.Framework;

namespace SafeViewModelTests.TestTools
{
    public static class PropertyChangedEventExtentions
    {

        public static ViewModelPropertyObserver<T> GetPropertyObserver<T>(this INotifyPropertyChanged notifyPropertyChanged, string propertyName)
        {
            ViewModelPropertyObserver<T> observer = new ViewModelPropertyObserver<T>();
            var property = notifyPropertyChanged.GetType().GetProperty(propertyName);
            if(property == null) throw new Exception("Can not obeserve property");
            notifyPropertyChanged.PropertyChanged += (s, args) =>
            {
                if(s != notifyPropertyChanged) return;
                if(args.PropertyName != propertyName) return;
                observer.PropertyValue =  (T)property.GetValue(s, null);
                observer.NumberOfTimesPropertyChanged++;
            };

            return observer;
        }

        public static void AssertProperyHasChanged<T>(this ViewModelPropertyObserver<T> observer, T t)
        {
            Assert.AreEqual(t, observer.PropertyValue);
            Assert.AreNotEqual(0, observer.NumberOfTimesPropertyChanged);
        }
    }

    public class ViewModelPropertyObserver<T>
    {
        public ViewModelPropertyObserver()
        {
            ResetObserver();
        }
        public T PropertyValue { get; set; }
        public int NumberOfTimesPropertyChanged { get; set; }

        public void ResetObserver()
        {
            PropertyValue = default(T);
            NumberOfTimesPropertyChanged = 0;
        }
    }
}