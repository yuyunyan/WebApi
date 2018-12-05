namespace Sourceportal.Reports
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for QuoteDetails.
    /// </summary>
    public partial class SalesOrder : Telerik.Reporting.Report
    {
        public SalesOrder()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            InitializeCustom();
        }

        public void InitializeCustom()
        { //may need to set these in designer too, value keeps resetting
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.000000000000000D); //conditonal formatted columns appear at design time and keep stretching report
            this.panel3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8D), Telerik.Reporting.Drawing.Unit.Inch(2.6600000858306885D));
        }
    }
}