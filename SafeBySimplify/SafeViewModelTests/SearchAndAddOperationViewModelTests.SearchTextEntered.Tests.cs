using NUnit.Framework;

namespace SafeViewModelTests
{
    public partial class SearchAndAddOperationViewModelTests
    {
        public partial class SearchTextEntered
        {
            public class Tests : SearchTextEntered
            {
                [Test]
                public void Seach_in_progress_indicator_is_enabled()
                {
                    Assert.True(_searchProgressIndicatorObserver.PropertyValue); 
                }
            }
        }
    }
}