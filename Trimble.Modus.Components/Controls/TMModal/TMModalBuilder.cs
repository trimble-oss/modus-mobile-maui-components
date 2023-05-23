using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trimble.Modus.Components
{
    public class TMModalBuilder
    {
        private string TitleText = null;

        private ImageSource TitleIcon = null;

        private string PrimaryText = null;

        private string SecondaryText = null;

        private string TertiaryText = null;

        private string DestructiveButtonText = null;

        /// <summary>
        /// Action triggered when primary button is clicked
        /// </summary>
        private event EventHandler PrimaryButtonClicked = null;

        /// <summary>
        /// Action triggered when secondary button is clicked
        /// </summary>
        private event EventHandler SecondaryButtonClicked = null;

        /// <summary>
        /// Action triggered when Tertiary button is clicked
        /// </summary>
        private event EventHandler TertiaryButtonClicked = null;

        /// <summary>
        /// Action triggered when Destructive button is clicked
        /// </summary>
        private event EventHandler DestructiveButtonClicked = null;

        public TMModalBuilder SetTitleText(string value)
        {
            TitleText = value;
            return this;
        }

        public TMModalBuilder SetTitleIcon(ImageSource value)
        {
            TitleIcon = value;
            return this;
        }

        public TMModalBuilder SetPrimaryText(string value)
        {
            PrimaryText = value;
            return this;
        }

        public TMModalBuilder SetSecondaryText(string value)
        {
            SecondaryText = value;
            return this;
        }

        public TMModalBuilder SetTertiaryText(string value)
        {
            TertiaryText = value;
            return this;
        }

        public TMModalBuilder SetDestructiveText(string value)
        {
            DestructiveButtonText = value;
            return this;
        }

        public TMModalBuilder SetPrimaryButtonClickEvent(EventHandler value)
        {
            PrimaryButtonClicked = value;
            return this;
        }

        public TMModalBuilder SetSecondaryButtonClickEvent(EventHandler value)
        {
            SecondaryButtonClicked = value;
            return this;
        }

        public TMModalBuilder SetTertiaryButtonClickEvent(EventHandler value)
        {
            TertiaryButtonClicked = value;
            return this;
        }

        public TMModalBuilder SetDestructiveButtonClickEvent(EventHandler value)
        {
            DestructiveButtonClicked = value;
            return this;
        }

        public TMModal Build()
        {
            return new TMModal(TitleText, TitleIcon, PrimaryText, SecondaryText, TertiaryText, DestructiveButtonText, PrimaryButtonClicked, SecondaryButtonClicked, TertiaryButtonClicked, DestructiveButtonClicked);
        }

    }
}
