using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MyVSTool
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class OptionsPage : DialogPage
    {
        private string format = "CPU: <#CPU> | <#TOTAL_CPU>  RAM: <#RAM> | <#TOTAL_USE_RAM%>";

        private int interval = 1000;

        private string prodirname = "Work_Directory";

        [Category("Design")]
        [Description("Sets Project Directory Name To Open.")]
        [DisplayName("Project Directory Name")]
        public string ProDirName
        {
            get
            {
                return this.prodirname;
            }
            set
            {
                this.prodirname = value;
                this.OptionUpdated("ProDirName", value);
            }
        }

        [Category("General")]
        [Description("Sets the format of the information.\r\n \"CPU\", \"TOTAL_CPU\", \"RAM\", \"FREE_RAM\", \"TOTAL_USE_RAM\", \"RAM%\", \"FREE_RAM%\", \"TOTAL_USE_RAM%\"")]
        [DisplayName("Format")]
        public string Format
        {
            get
            {
                return this.format;
            }
            set
            {
                this.format = value;
                this.OptionUpdated("Format", value);
            }
        }

        [Category("General")]
        [Description("Sets the refresh interval in ms.")]
        [DisplayName("Interval")]
        public int Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
                this.OptionUpdated("Interval", value);
            }
        }

        private void OptionUpdated(string pName, object pValue)
        {
            var statusInfoPackage = (StatusInfoPackage)this.GetService(typeof(StatusInfoPackage));
            statusInfoPackage?.OptionUpdated(pName, pValue);
        }
    }
}