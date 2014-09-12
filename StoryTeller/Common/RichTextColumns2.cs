using StoryTeller.Converter;
using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;

namespace StoryTeller.Common
{
    /// <summary>
    /// Wrapper for <see cref="RichTextBlock"/> that creates as many additional overflow
    /// columns as needed to fit the available content.
    /// </summary>
    /// <example>
    /// The following creates a collection of 400-pixel wide columns spaced 50 pixels apart
    /// to contain arbitrary data-bound content:
    /// <code>
    /// <RichTextColumns>
    ///     <RichTextColumns.ColumnTemplate>
    ///         <DataTemplate>
    ///             <RichTextBlockOverflow Width="400" Margin="50,0,0,0"/>
    ///         </DataTemplate>
    ///     </RichTextColumns.ColumnTemplate>
    ///     <RichTextColumns.InitialColumnTemplate>
    ///         <DataTemplate>
    ///             <RichTextBlock MaxLines="31" Width="400" Margin="50,0,0,0"/>
    ///         </DataTemplate>
    ///     </RichTextColumns.InitialColumnTemplate>
    ///     
    ///     <RichTextBlock Width="400">
    ///         <Paragraph>
    ///             <Run Text="{Binding Content}"/>
    ///         </Paragraph>
    ///     </RichTextBlock>
    /// </RichTextColumns>
    /// </code>
    /// </example>
    /// <remarks>Typically used in a horizontally scrolling region where an unbounded amount of
    /// space allows for all needed columns to be created.  When used in a vertically scrolling
    /// space there will never be any additional columns.</remarks>
    [Windows.UI.Xaml.Markup.ContentProperty(Name = "RichTextContent")]
    public sealed class RichTextColumns2 : Panel
    {
        /// <summary>
        /// Identifies the <see cref="RichTextContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RichTextContentProperty =
            DependencyProperty.Register("RichTextContent", typeof(RichTextBlock),
            typeof(RichTextColumns), new PropertyMetadata(null, ResetOverflowLayout));

        /// <summary>
        /// Identifies the <see cref="ColumnTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnTemplateProperty =
            DependencyProperty.Register("ColumnTemplate", typeof(DataTemplate),
            typeof(RichTextColumns), new PropertyMetadata(null, ResetOverflowLayout));

        // Using a DependencyProperty as the backing store for InitialColumnTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InitialColumnTemplateProperty =
            DependencyProperty.Register("InitialColumnTemplate", typeof(DataTemplate), typeof(RichTextColumns2), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Scenes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScenesProperty =
            DependencyProperty.Register("Scenes", typeof(object), typeof(RichTextColumns2), new PropertyMetadata(null));

        public object Scenes
        {
            get { return GetValue(ScenesProperty) as IEnumerable<IScene>; }
            set { SetValue(ScenesProperty, value); }
        }

        public DataTemplate InitialColumnTemplate
        {
            get { return (DataTemplate)GetValue(InitialColumnTemplateProperty); }
            set { SetValue(InitialColumnTemplateProperty, value); }
        }

        /// <summary>
        /// Lists overflow columns already created.  Must maintain a 1:1 relationship with
        /// instances in the <see cref="Panel.Children"/> collection following the initial
        /// RichTextBlock child.
        /// </summary>
        private List<RichTextBlockOverflow> _overflowColumns = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextColumns"/> class.
        /// </summary>
        public RichTextColumns2()
        {
            this.HorizontalAlignment = HorizontalAlignment.Left;
        }

        /// <summary>
        /// Gets or sets the initial rich text content to be used as the first column.
        /// </summary>
        public RichTextBlock RichTextContent
        {
            get { return (RichTextBlock)GetValue(RichTextContentProperty); }
            set { SetValue(RichTextContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the template used to create additional
        /// <see cref="RichTextBlockOverflow"/> instances.
        /// </summary>
        public DataTemplate ColumnTemplate
        {
            get { return (DataTemplate)GetValue(ColumnTemplateProperty); }
            set { SetValue(ColumnTemplateProperty, value); }
        }

        /// <summary>
        /// Invoked when the content or overflow template is changed to recreate the column layout.
        /// </summary>
        /// <param name="d">Instance of <see cref="RichTextColumns"/> where the change
        /// occurred.</param>
        /// <param name="e">Event data describing the specific change.</param>
        private static void ResetOverflowLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // When dramatic changes occur, rebuild the column layout from scratch
            var target = d as RichTextColumns2;
            if (target != null)
            {
                target._overflowColumns = new List<RichTextBlockOverflow>();
                target.Children.Clear();
                target.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Determines whether additional overflow columns are needed and if existing columns can
        /// be removed.
        /// </summary>
        /// <param name="availableSize">The size of the space available, used to constrain the
        /// number of additional columns that can be created.</param>
        /// <returns>The resulting size of the original content plus any extra columns.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            availableSize = new Size(double.MaxValue, availableSize.Height);
            if (this.Scenes == null) return new Size(0, 0);

            // Make sure the RichTextBlock is a child, using the lack of
            // a list of additional columns as a sign that this hasn't been
            // done yet
            this._overflowColumns = new List<RichTextBlockOverflow>();
            int overflowIndex = 0;
            double maxWidth = int.MinValue;
            double maxHeight = int.MinValue;
            IEnumerable<IScene> scenes = Scenes as IEnumerable<IScene>;
            if (null == scenes) return new Size(0, 0);
            
            foreach (IScene scene in scenes)
            {
                RichTextBlock initialBlock;
                var xaml = StringToRtf.PlainTextToBlocks(scene.Content.Content);// XamlReader.Load(StringToRtf.PlainTextToXaml(scene.Content.Content)) as System.Collections.IEnumerable;
                initialBlock = InitialColumnTemplate.LoadContent() as RichTextBlock;
                foreach (Block block in xaml)
                {
                    initialBlock.Blocks.Add(block);
                }

                //if (this._overflowColumns == null)
                //{
                Children.Add(initialBlock);
                //    this._overflowColumns = new List<RichTextBlockOverflow>();
                //}

                // Start by measuring the original RichTextBlock content
                initialBlock.Measure(availableSize);
                maxWidth = Math.Max(maxWidth, initialBlock.DesiredSize.Width);
                maxHeight = Math.Max(maxHeight, initialBlock.DesiredSize.Height);
                var hasOverflow = initialBlock.HasOverflowContent;

                // Make sure there are enough overflow columns
                while (hasOverflow && maxWidth < availableSize.Width && this.ColumnTemplate != null)
                {
                    // Use existing overflow columns until we run out, then create
                    // more from the supplied template
                    RichTextBlockOverflow overflow;
                    if (this._overflowColumns.Count > overflowIndex)
                    {
                        overflow = this._overflowColumns[overflowIndex];
                    }
                    else
                    {
                        overflow = (RichTextBlockOverflow)this.ColumnTemplate.LoadContent();
                        this._overflowColumns.Add(overflow);
                        this.Children.Add(overflow);
                        if (overflowIndex == 0)
                        {
                            initialBlock.OverflowContentTarget = overflow;
                        }
                        else
                        {
                            this._overflowColumns[overflowIndex - 1].OverflowContentTarget = overflow;
                        }
                    }

                    // Measure the new column and prepare to repeat as necessary
                    overflow.Measure(new Size(availableSize.Width - maxWidth, availableSize.Height));
                    maxWidth += overflow.DesiredSize.Width;
                    maxHeight = Math.Max(maxHeight, overflow.DesiredSize.Height);
                    hasOverflow = overflow.HasOverflowContent;
                    overflowIndex++;
                }
            }

            // Disconnect extra columns from the overflow chain, remove them from our private list
            // of columns, and remove them as children
            if (this._overflowColumns.Count > overflowIndex)
            {
                if (overflowIndex == 0)
                {
                    this.RichTextContent.OverflowContentTarget = null;
                }
                else
                {
                    this._overflowColumns[overflowIndex - 1].OverflowContentTarget = null;
                }
                while (this._overflowColumns.Count > overflowIndex)
                {
                    this._overflowColumns.RemoveAt(overflowIndex);
                    this.Children.RemoveAt(overflowIndex + 1);
                }
            }

            // Report final determined size
            return new Size(maxWidth, maxHeight);
        }

        /// <summary>
        /// Arranges the original content and all extra columns.
        /// </summary>
        /// <param name="finalSize">Defines the size of the area the children must be arranged
        /// within.</param>
        /// <returns>The size of the area the children actually required.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double maxWidth = 0;
            double maxHeight = 0;
            foreach (var child in Children)
            {
                child.Arrange(new Rect(maxWidth, 0, child.DesiredSize.Width, finalSize.Height));
                maxWidth += child.DesiredSize.Width;
                maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
            }

            return new Size(maxWidth, maxHeight);
        }
    }
}
