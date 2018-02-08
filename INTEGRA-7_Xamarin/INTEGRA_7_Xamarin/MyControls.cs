using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    // Mapping controls for compatibility with Xamarin:
    public class TextBlock : Editor
    {
        public TextBlock()
        {
            this.IsEnabled = false;
        }
    }


    // My conveniance controls:
    public enum _orientation
    {
        HORIZONTAL,
        VERTICAL,
    }

    public enum _labelPosition
    {
        BEFORE,
        AFTER,
    }

    public enum _colorSettings
    {
        DARK,
        LIGHT,
    }

    public class ColorSettings
    {
        public Color Background { get; set; }
        public Color Frame { get; set; }
        public Color Text { get; set; }

        public ColorSettings(_colorSettings colorSettings)
        {
            switch (colorSettings)
            {
                case _colorSettings.DARK:
                    break;
                case _colorSettings.LIGHT:
                    Background = Color.SeaShell;
                    Frame = Color.Green;
                    Text = Color.Black;
                    break;
            }
        }
    }

    public class LabeledText : Grid
    {
        //public Grid TheGrid { get; set; }
        public _orientation Orientation { get; set; }
        public _labelPosition LabelPosition { get; set; }
        public Label Label { get; set; }
        public Label Text { get; set; }

        public LabeledText(String LabelText, String Text, byte[] Sizes = null)
        {
            labeledText(LabelText, Text, _orientation.HORIZONTAL, _labelPosition.BEFORE, Sizes);
        }

        public LabeledText(String LabelText, String Text, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledText(LabelText, Text, Orientation, LabelPosition, Sizes);
        }

        private void labeledText(String LabelText, String Text, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            this.Orientation = Orientation;
            this.LabelPosition = LabelPosition;
            this.Label = new Label();
            this.Label.Text = LabelText;
            this.Text = new Label();
            this.Text.Text = Text;
            byte[] sizes;
            if (Sizes == null || Sizes.Count() != 2)
            {
                sizes = new byte[] { 1, 1 };
            }
            else
            {
                sizes = Sizes;
            }

            this.Text.VerticalOptions = LayoutOptions.FillAndExpand;
            this.Label.VerticalOptions = LayoutOptions.FillAndExpand;
            if (Orientation == _orientation.HORIZONTAL)
            {

                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Text.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Label, this.Text }, sizes, true)).Row);
                }
                else
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.Text.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Text, this.Label }, sizes, true)).Row);
                }
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.Text.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label }, sizes, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Text }, sizes, true)).Row);
                }
                else
                {
                    this.Text.HorizontalOptions = LayoutOptions.Start;
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Text }, sizes, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Label }, sizes, true)).Row);
                }
            }
        }
    }

    public class LabeledTextInput : Grid
    {
        //public Grid TheGrid { get; set; }
        public _orientation Orientation { get; set; }
        public _labelPosition LabelPosition { get; set; }
        public Label Label { get; set; }
        public Editor Editor { get; set; }

        public LabeledTextInput(String LabelText, byte[] Sizes = null)
        {
            labeledTextInput(LabelText, "", _orientation.HORIZONTAL, _labelPosition.BEFORE, Sizes);
        }

        public LabeledTextInput(String LabelText, String EditorText = "", _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledTextInput(LabelText, EditorText, Orientation, LabelPosition, Sizes);
        }

        private void labeledTextInput(String LabelText, String EditorText = "", _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            this.Orientation = Orientation;
            this.LabelPosition = LabelPosition;
            this.Label = new Label();
            this.Label.Text = LabelText;
            this.Editor = new Editor();
            this.Editor.Text = EditorText;
            byte[] sizes;
            if (Sizes == null || Sizes.Count() != 2)
            {
                sizes = new byte[] { 1, 1 };
            }
            else
            {
                sizes = Sizes;
            }

            this.Editor.VerticalOptions = LayoutOptions.FillAndExpand;
            this.Label.VerticalOptions = LayoutOptions.FillAndExpand;
            if (Orientation == _orientation.HORIZONTAL)
            {

                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label, this.Editor }, sizes, true)).Row);
                }
                else
                {
                    this.Editor.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Editor, this.Label }, sizes, true)).Row);
                }
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.Editor.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label }, null,  true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Editor }, null, true)).Row);
                }
                else
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Editor.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Editor }, null, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Label }, null, true)).Row);
                }
            }
        }
    }

    public class LabeledPicker : Grid
    {
        //public Grid TheGrid { get; set; }
        public _orientation Orientation { get; set; }
        public _labelPosition LabelPosition { get; set; }
        public Label Label { get; set; }
        public Picker Picker { get; set; }

        public LabeledPicker(String LabelText)
        {
            labeledPicker(LabelText, null, 0, _orientation.HORIZONTAL, _labelPosition.BEFORE, null);
        }

        public LabeledPicker(String LabelText, Picker Picker = null, byte[] Sizes = null)
        {
            labeledPicker(LabelText, Picker, 0, _orientation.HORIZONTAL, _labelPosition.BEFORE, Sizes);
        }

        public LabeledPicker(String LabelText, Picker Picker = null, Int32 SelectedIndex = 0, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledPicker(LabelText, Picker, SelectedIndex, Orientation, LabelPosition, Sizes);
        }

        private void labeledPicker(String LabelText, Picker Picker = null, Int32 SelectedIndex = 0, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            this.Orientation = Orientation;
            this.LabelPosition = LabelPosition;
            this.Label = new Label();
            this.Label.Text = LabelText;
            if (Picker == null)
            {
                this.Picker = new Picker();
            }
            else
            {
                this.Picker = Picker;
            }
            byte[] sizes;
            if (Sizes == null || Sizes.Count() != 2)
            {
                sizes = new byte[] { 1, 1 };
            }
            else
            {
                sizes = Sizes;
            }

            this.Picker.VerticalOptions = LayoutOptions.FillAndExpand;
            this.Label.VerticalOptions = LayoutOptions.FillAndExpand;
            if (Orientation == _orientation.HORIZONTAL)
            {

                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label, this.Picker }, sizes, true)).Row);
                }
                else
                {
                    this.Picker.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Picker, this.Label }, sizes, true)).Row);
                }
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.Picker.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label }, null, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Picker }, null, true)).Row);
                }
                else
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Picker.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Picker }, null, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Label }, null, true)).Row);
                }
            }
            this.Picker.SelectedIndex = SelectedIndex;
        }
    }

    public class LabeledSwitch : Grid
    {
        //public Grid TheGrid { get; set; }
        public _orientation Orientation { get; set; }
        public _labelPosition LabelPosition { get; set; }
        public Label Label { get; set; }
        public Switch Switch { get; set; }

        public LabeledSwitch(String LabelText)
        {
            labeledSwitch(LabelText, null, false, _orientation.HORIZONTAL, _labelPosition.BEFORE, null);
        }

        public LabeledSwitch(String LabelText, Switch Switch = null, byte[] Sizes = null)
        {
            labeledSwitch(LabelText, Switch, false, _orientation.HORIZONTAL, _labelPosition.BEFORE, Sizes);
        }

        public LabeledSwitch(String LabelText, Switch Switch = null, Boolean IsSelected = false, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledSwitch(LabelText, Switch, IsSelected, Orientation, LabelPosition, Sizes);
        }

        private void labeledSwitch(String LabelText, Switch Switch = null, Boolean IsSelected = false, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            this.Orientation = Orientation;
            this.LabelPosition = LabelPosition;
            this.Label = new Label();
            this.Label.Text = LabelText;
            if (Switch == null)
            {
                this.Switch = new Switch();
            }
            else
            {
                this.Switch = Switch;
            }
            byte[] sizes;
            if (Sizes == null || Sizes.Count() != 2)
            {
                sizes = new byte[] { 1, 1 };
            }
            else
            {
                sizes = Sizes;
            }

            this.Switch.VerticalOptions = LayoutOptions.FillAndExpand;
            this.Label.VerticalOptions = LayoutOptions.FillAndExpand;
            if (Orientation == _orientation.HORIZONTAL)
            {

                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label, this.Switch }, sizes, true)).Row);
                }
                else
                {
                    this.Switch.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Switch, this.Label }, sizes, true)).Row);
                }
                this.Label.VerticalOptions = LayoutOptions.Center;
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.Switch.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label }, null, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Switch }, null, true)).Row);
                }
                else
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Switch.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Switch }, null, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Label }, null, true)).Row);
                }
                this.Label.HorizontalOptions = LayoutOptions.Center;
            }
            this.Switch.IsToggled = IsSelected;
        }
    }

    //public class RadioButton : Grid
    //{
    //    public _orientation Orientation { get; set; }
    //    public _labelPosition LabelPosition { get; set; }
    //    public Label Label { get; set; }
    //    public Switch Switch { get; set; }

    //    public RadioButton(String LabelText)
    //    {
    //        radioButton(LabelText, null, 0, _orientation.HORIZONTAL, _labelPosition.BEFORE, null);
    //    }

    //    public RadioButton(String LabelText, Picker Picker = null, byte[] Sizes = null)
    //    {
    //        radioButton(LabelText, Switch, 0, _orientation.HORIZONTAL, _labelPosition.BEFORE, Sizes);
    //    }

    //    public RadioButton(String LabelText, Picker Picker = null, Int32 SelectedIndex = 0, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
    //    {
    //        radioButton(LabelText, Switch, SelectedIndex, Orientation, LabelPosition, Sizes);
    //    }

    //private void radioButton(String LabelText, String[] ItemLabels, Switch Switch = null, Int32 SelectedIndex = 0, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
    //{
    //    this.Orientation = Orientation;
    //    this.LabelPosition = LabelPosition;
    //    this.Label = new Label();
    //    this.Label.Text = LabelText;
    //    if (Switch == null)
    //    {
    //        this.Switch = new Switch();
    //    }
    //    else
    //    {
    //        this.Switch = Switch;
    //    }
    //    byte[] sizes;
    //    if (Sizes == null || Sizes.Count() != 2)
    //    {
    //        sizes = new byte[] { 1, 1 };
    //    }
    //    else
    //    {
    //        sizes = Sizes;
    //    }

    //    this.Switch.VerticalOptions = LayoutOptions.FillAndExpand;
    //    this.Label.VerticalOptions = LayoutOptions.FillAndExpand;
    //    if (Orientation == _orientation.HORIZONTAL)
    //    {

    //        if (LabelPosition == _labelPosition.BEFORE)
    //        {
    //            this.Label.HorizontalOptions = LayoutOptions.End;
    //            this.Children.Add((new GridRow(0, new View[] { this.Label, this.Switch }, sizes, true)).Row);
    //        }
    //        else
    //        {
    //            this.Switch.HorizontalOptions = LayoutOptions.Start;
    //            this.Children.Add((new GridRow(0, new View[] { this.Switch, this.Label }, sizes, true)).Row);
    //        }
    //    }
    //    else
    //    {
    //        if (LabelPosition == _labelPosition.BEFORE)
    //        {
    //            this.Label.HorizontalOptions = LayoutOptions.Start;
    //            this.Switch.HorizontalOptions = LayoutOptions.End;
    //            this.Children.Add((new GridRow(0, new View[] { this.Label }, null, true)).Row);
    //            this.Children.Add((new GridRow(1, new View[] { this.Switch }, null, true)).Row);
    //        }
    //        else
    //        {
    //            this.Label.HorizontalOptions = LayoutOptions.End;
    //            this.Switch.HorizontalOptions = LayoutOptions.Start;
    //            this.Children.Add((new GridRow(0, new View[] { this.Switch }, null, true)).Row);
    //            this.Children.Add((new GridRow(1, new View[] { this.Label }, null, true)).Row);
    //        }
    //    }
    //    this.Switch.IsToggled = false;
    //}
    //}
}
