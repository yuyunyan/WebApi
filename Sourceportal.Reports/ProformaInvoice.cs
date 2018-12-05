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
    public partial class ProFormaInvoice : Telerik.Reporting.Report
    {
        public ProFormaInvoice()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //

        }

    }
}