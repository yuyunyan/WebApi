using System;

namespace Sourceportal.Reports
{
    partial class Terms
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Terms));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector1 = new Telerik.Reporting.Drawing.DescendantSelector();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector2 = new Telerik.Reporting.Drawing.DescendantSelector();
            this.detail = new Telerik.Reporting.DetailSection();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.pictureBox2 = new Telerik.Reporting.PictureBox();
            this.uspPurchaseOrderLinesGet = new Telerik.Reporting.SqlDataSource();
            this.uspPurchaseOrderPriceSumGet = new Telerik.Reporting.SqlDataSource();
            this.uspPurchaseOrderOrganizationGet = new Telerik.Reporting.SqlDataSource();
            this.uspPurchaseOrderReportDetailsGet = new Telerik.Reporting.SqlDataSource();
            this.uspPurchaseOrderReportLocationGet_SHIPTO = new Telerik.Reporting.SqlDataSource();
            this.uspPurchaseOrderReportLocationGet_BILLTO = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(22.000001907348633D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pictureBox1,
            this.pictureBox2});
            this.detail.Name = "detail";
            this.detail.Style.BackgroundColor = System.Drawing.Color.Transparent;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(2.980232949312267E-09D));
            this.pictureBox1.MimeType = "image/png";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.4999217987060547D), Telerik.Reporting.Drawing.Unit.Inch(10.999922752380371D));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(11D));
            this.pictureBox2.MimeType = "image/png";
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.4999217987060547D), Telerik.Reporting.Drawing.Unit.Inch(10.999922752380371D));
            this.pictureBox2.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.pictureBox2.Value = ((object)(resources.GetObject("pictureBox2.Value")));
            // 
            // uspPurchaseOrderLinesGet
            // 
            this.uspPurchaseOrderLinesGet.ConnectionString = "SourcePortalConnection";
            this.uspPurchaseOrderLinesGet.Name = "uspPurchaseOrderLinesGet";
            this.uspPurchaseOrderLinesGet.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@POLineID", System.Data.DbType.Int32, null),
            new Telerik.Reporting.SqlDataSourceParameter("@PurchaseOrderID", System.Data.DbType.Int32, "= Parameters.PurchaseOrderID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@POVersionID", System.Data.DbType.Int32, "= Parameters.VersionID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@RowOffset", System.Data.DbType.Int32, "0"),
            new Telerik.Reporting.SqlDataSourceParameter("@RowLimit", System.Data.DbType.Int32, "9999999"),
            new Telerik.Reporting.SqlDataSourceParameter("@SortBy", System.Data.DbType.String, null),
            new Telerik.Reporting.SqlDataSourceParameter("@DescSort", System.Data.DbType.Boolean, "false"),
            new Telerik.Reporting.SqlDataSourceParameter("@CommentTypeID", System.Data.DbType.Int32, null)});
            this.uspPurchaseOrderLinesGet.SelectCommand = "dbo.uspPurchaseOrderLinesGet";
            this.uspPurchaseOrderLinesGet.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // uspPurchaseOrderPriceSumGet
            // 
            this.uspPurchaseOrderPriceSumGet.ConnectionString = "SourcePortalConnection";
            this.uspPurchaseOrderPriceSumGet.Name = "uspPurchaseOrderPriceSumGet";
            this.uspPurchaseOrderPriceSumGet.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@PurchaseOrderID", System.Data.DbType.Int32, "= Parameters.PurchaseOrderID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@VersionID", System.Data.DbType.Int32, "= Parameters.VersionID.Value")});
            this.uspPurchaseOrderPriceSumGet.SelectCommand = "dbo.uspPurchaseOrderPriceSumGet";
            this.uspPurchaseOrderPriceSumGet.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // uspPurchaseOrderOrganizationGet
            // 
            this.uspPurchaseOrderOrganizationGet.ConnectionString = "SourcePortalConnection";
            this.uspPurchaseOrderOrganizationGet.Name = "uspPurchaseOrderOrganizationGet";
            this.uspPurchaseOrderOrganizationGet.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@PurchaseOrderID", System.Data.DbType.Int32, "= Parameters.PurchaseOrderID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@VersionID", System.Data.DbType.Int32, "= Parameters.VersionID.Value")});
            this.uspPurchaseOrderOrganizationGet.SelectCommand = "dbo.uspPurchaseOrderOrganizationGet";
            this.uspPurchaseOrderOrganizationGet.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // uspPurchaseOrderReportDetailsGet
            // 
            this.uspPurchaseOrderReportDetailsGet.ConnectionString = "SourcePortalConnection";
            this.uspPurchaseOrderReportDetailsGet.Name = "uspPurchaseOrderReportDetailsGet";
            this.uspPurchaseOrderReportDetailsGet.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@PurchaseOrderID", System.Data.DbType.Int32, "= Parameters.PurchaseOrderID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@VersionID", System.Data.DbType.Int32, "= Parameters.VersionID.Value")});
            this.uspPurchaseOrderReportDetailsGet.SelectCommand = "dbo.uspPurchaseOrderReportDetailsGet";
            this.uspPurchaseOrderReportDetailsGet.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // uspPurchaseOrderReportLocationGet_SHIPTO
            // 
            this.uspPurchaseOrderReportLocationGet_SHIPTO.ConnectionString = "SourcePortalConnection";
            this.uspPurchaseOrderReportLocationGet_SHIPTO.Name = "uspPurchaseOrderReportLocationGet_SHIPTO";
            this.uspPurchaseOrderReportLocationGet_SHIPTO.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@PurchaseOrderID", System.Data.DbType.Int32, "= Parameters.PurchaseOrderID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@VersionID", System.Data.DbType.Int32, "= Parameters.VersionID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@LocationTypeID", System.Data.DbType.Int32, "2")});
            this.uspPurchaseOrderReportLocationGet_SHIPTO.SelectCommand = "dbo.uspPurchaseOrderReportLocationGet";
            this.uspPurchaseOrderReportLocationGet_SHIPTO.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // uspPurchaseOrderReportLocationGet_BILLTO
            // 
            this.uspPurchaseOrderReportLocationGet_BILLTO.ConnectionString = "SourcePortalConnection";
            this.uspPurchaseOrderReportLocationGet_BILLTO.Name = "uspPurchaseOrderReportLocationGet_BILLTO";
            this.uspPurchaseOrderReportLocationGet_BILLTO.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@PurchaseOrderID", System.Data.DbType.Int32, "= Parameters.PurchaseOrderID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@VersionID", System.Data.DbType.Int32, "= Parameters.VersionID.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@LocationTypeID", System.Data.DbType.Int32, "1")});
            this.uspPurchaseOrderReportLocationGet_BILLTO.SelectCommand = "dbo.uspPurchaseOrderReportLocationGet";
            this.uspPurchaseOrderReportLocationGet_BILLTO.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // Terms
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "Terms";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.Name = "PurchaseOrderID";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Value = 0;
            reportParameter2.Name = "VersionID";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.Value = 1;
            reportParameter3.Name = "UserID";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter3.Value = 0;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.Table), "Normal.TableNormal")});
            styleRule2.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule2.Style.Color = System.Drawing.Color.Black;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableBody")});
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector1});
            styleRule3.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule3.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableHeader")});
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector2});
            styleRule4.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.4999618530273438D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.SqlDataSource uspPurchaseOrderPriceSumGet;
        private Telerik.Reporting.SqlDataSource uspPurchaseOrderReportDetailsGet;
        private Telerik.Reporting.SqlDataSource uspPurchaseOrderLinesGet;
        private Telerik.Reporting.SqlDataSource uspPurchaseOrderReportLocationGet_BILLTO;
        private Telerik.Reporting.SqlDataSource uspPurchaseOrderReportLocationGet_SHIPTO;
        private Telerik.Reporting.SqlDataSource uspPurchaseOrderOrganizationGet;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.PictureBox pictureBox2;
    }
}