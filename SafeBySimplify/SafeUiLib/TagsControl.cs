using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SafeUiLib
{
    public class TagsControl : RichTextBox
    {

        public static readonly DependencyProperty TagTemplateProperty =
            DependencyProperty.Register("TagTemplate", typeof(DataTemplate), typeof(TagsControl));

        public DataTemplate TagTemplate
        {
            get { return (DataTemplate)GetValue(TagTemplateProperty); }
            set { SetValue(TagTemplateProperty, value); }
        }

        public string Tags
        {
            get { return (string)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }

        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register("Tags", typeof(string), typeof(TagsControl), new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnTagsChanging)
            { BindsTwoWayByDefault = true });

        private Paragraph _paragraph;

        public TagsControl()
        {
            TextChanged += OnTokenTextChanged;
            TextChanged += (sender, args) =>
            {
                if (this._paragraph.Inlines.Count == 0) this.Opacity = .5;
                else Opacity = 1;
            }; 

            LostFocus += OnLoosingFocus;

            var inline = new Run();
            _paragraph = new Paragraph(inline);
            var flowDocument = new FlowDocument(_paragraph);
            Document = flowDocument;
        }

        private void OnTokenTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged -= OnTokenTextChanged;
            var runs = GetRuns();
            runs.ForEach(AddTagsAndRemoveRunIfEmpty);
            TextChanged += OnTokenTextChanged;
        }

        private void OnLoosingFocus(object sender, RoutedEventArgs e)
        {
            LostFocus -= OnLoosingFocus;
            var runs = GetRuns();
            runs.ForEach(CloseRunIfNecessary);
            runs.ForEach(AddTagsAndRemoveRunIfEmpty);
            LostFocus += OnLoosingFocus;

            var tokens = _paragraph.Inlines.Select(GetStringValue).ToArray();
            var concatenatedTokens = string.Join(";", tokens);
            Tags = concatenatedTokens;
        }




        private static void OnTagsChanging(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue == e.OldValue) return;

            var propertyValue = e.NewValue as string;
            if (propertyValue == null) return;
            var values = propertyValue.Split(';').Where(x => !string.IsNullOrEmpty(x));
            var thisControl = (TagsControl)d;
            thisControl._paragraph.Inlines.Clear();
            foreach (var value in values)
            {
                var tokenContainer = thisControl.CreateInLineUiContainer(value);
                thisControl._paragraph.Inlines.Add(tokenContainer);
            }
        }

        private string ExtractTokenWithSeparator(string text)
        {
            if (!text.Contains(";")) return null;
            var length = text.IndexOf(";", 0, StringComparison.Ordinal);
            return text.Substring(0, length + 1).Trim();
        }

        private string RemoveSeparator(string tokenWithSeparator)
        {
            return tokenWithSeparator.Replace(";", string.Empty);
        }

        private List<Run> GetRuns()
        {
            return _paragraph.Inlines.OfType<Run>().ToList();
        }

        string GetStringValue(ContentPresenter contentPresenter)
        {
            var stringValue = contentPresenter.Content as string;
            if (stringValue == null) return string.Empty;
            return stringValue;
        }

        string GetStringValue(Inline inline)
        {
            var inLineUiContainer = inline as InlineUIContainer;
            if (inLineUiContainer == null) throw new Exception("There is inline that's not InLineUiContiner");
            var child = inLineUiContainer.Child;
            if (child is ContentPresenter) return GetStringValue(child as ContentPresenter);
            return string.Empty;
        }

        private void AddTagsAndRemoveRunIfEmpty(Run run)
        {
            AddTagsForRun(run);
            RemoveRunIfEmpty(run);
        }

        private void RemoveRunIfEmpty(Run run)
        {
            if (string.IsNullOrEmpty(run.Text))
            {
                _paragraph.Inlines.Remove(run);
            }
        }

        private void AddTagsForRun(Run run)
        {
            while (run.Text.Contains(";"))
            {
                AddTagForRun(run);
            }
        }

        private void AddTagForRun(Run run)
        {
            var currentRun = run;
            var runText = currentRun.Text;
            var runTokenWithSeparator = ExtractTokenWithSeparator(runText);
            var tokenContainer = CreateInLineUiContainer(RemoveSeparator(runTokenWithSeparator));
            var remainingText = runText.Replace(runTokenWithSeparator, "");
            _paragraph.Inlines.InsertBefore(currentRun, tokenContainer);
            currentRun.Text = remainingText.Trim();
        }

        private static void CloseRunIfNecessary(Run r)
        {
            if (string.IsNullOrEmpty(r.Text)) return;
            if (r.Text.EndsWith(";")) return;
            r.Text = r.Text + ";";
        }

        private InlineUIContainer CreateInLineUiContainer(string inputText)
        {
            var contentPresenter = new ContentPresenter()
            {
                Content = inputText,
                ContentTemplate = TagTemplate,
            };
            return new InlineUIContainer(contentPresenter) { BaselineAlignment = BaselineAlignment.TextBottom };
        }
    }
}
