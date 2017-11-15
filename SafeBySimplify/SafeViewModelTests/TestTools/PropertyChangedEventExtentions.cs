using System;
using System.ComponentModel;

namespace SafeViewModelTests
{
    public static class PropertyChangedEventExtentions
    {

        public static Func<PropertyChangedEventInfo<T>> GetPropertyChangedEventInfoFactory<T>(this INotifyPropertyChanged notifyPropertyChanged, string propertyName)
        {
            PropertyChangedEventInfo<T> eventInfo = new PropertyChangedEventInfo<T>();
            var property = notifyPropertyChanged.GetType().GetProperty(propertyName);

            notifyPropertyChanged.PropertyChanged += (s, args) =>
            {
                if(s != notifyPropertyChanged) return;
                if(args.PropertyName != propertyName) return;
                eventInfo.Value =  (T)property.GetValue(s, null);
                eventInfo.EventReceived = true;
            };

            return () => eventInfo;
        }
    }

    public class PropertyChangedEventInfo<T>
    {
        public T Value { get; set; }
        public bool EventReceived { get; set; }
    }
}