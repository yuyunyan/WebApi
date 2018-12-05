using System;

namespace Sourceportal.Reports
{
    partial class InspectionIdentifiedStockContainer
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector1 = new Telerik.Reporting.Drawing.DescendantSelector();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector2 = new Telerik.Reporting.Drawing.DescendantSelector();
            this.detail = new Telerik.Reporting.DetailSection();
            this.table1 = new Telerik.Reporting.Table();
            this.InspectionIdentifiedStockSubReport = new Telerik.Reporting.SubReport();
            this.uspQCIdentifiedStockParametersGet = new Telerik.Reporting.SqlDataSource();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.lblPage = new Telerik.Reporting.TextBox();
            this.lblPageNumber = new Telerik.Reporting.TextBox();
            this.lblPageOf = new Telerik.Reporting.TextBox();
            this.lblPageTotal = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(1.5520439147949219D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.table1});
            this.detail.Name = "detail";
            this.detail.Style.BackgroundColor = System.Drawing.Color.Transparent;
            // 
            // table1
            // 
            this.table1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(8.02187728881836D)));
            this.table1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(1.4000002145767212D)));
            this.table1.Body.SetCellContent(0, 0, this.InspectionIdentifiedStockSubReport);
            tableGroup1.Name = "itemStockID";
            this.table1.ColumnGroups.Add(tableGroup1);
            this.table1.DataSource = this.uspQCIdentifiedStockParametersGet;
            this.table1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.InspectionIdentifiedStockSubReport});
            this.table1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9365557313431054E-05D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.table1.Name = "table1";
            tableGroup2.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup2.Name = "detail";
            this.table1.RowGroups.Add(tableGroup2);
            this.table1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.02187728881836D), Telerik.Reporting.Drawing.Unit.Inch(1.4000002145767212D));
            this.table1.Style.BorderColor.Default = System.Drawing.Color.Transparent;
            this.table1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.table1.Style.Color = System.Drawing.Color.Transparent;
            this.table1.Style.LineColor = System.Drawing.Color.Transparent;
            this.table1.StyleName = "Normal.TableNormal";
            // 
            // InspectionIdentifiedStockSubReport
            // 
            this.InspectionIdentifiedStockSubReport.Name = "InspectionIdentifiedStockSubReport";
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("InspectionID", "= Parameters.InspectionID.Value"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("StockID", "= Fields.StockID"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("Discrepant", "= Fields.isDiscrepant"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("ReportTitle", "= Fields.ReportTitle"));
            typeReportSource1.TypeName = "Sourceportal.Reports.InspectionIdentifiedStock, Sourceportal.Reports, Version=1.0" +
    ".0.0, Culture=neutral, PublicKeyToken=null";
            this.InspectionIdentifiedStockSubReport.ReportSource = typeReportSource1;
            this.InspectionIdentifiedStockSubReport.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.02187728881836D), Telerik.Reporting.Drawing.Unit.Inch(1.4000002145767212D));
            this.InspectionIdentifiedStockSubReport.Style.BorderColor.Default = System.Drawing.Color.Transparent;
            this.InspectionIdentifiedStockSubReport.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.InspectionIdentifiedStockSubReport.Style.LineColor = System.Drawing.Color.Transparent;
            this.InspectionIdentifiedStockSubReport.StyleName = "Normal.TableBody";
            // 
            // uspQCIdentifiedStockParametersGet
            // 
            this.uspQCIdentifiedStockParametersGet.ConnectionString = "SourcePortalConnection";
            this.uspQCIdentifiedStockParametersGet.Name = "uspQCIdentifiedStockParametersGet";
            this.uspQCIdentifiedStockParametersGet.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@InspectionID", System.Data.DbType.Int32, "= Parameters.InspectionID.Value")});
            this.uspQCIdentifiedStockParametersGet.SelectCommand = "dbo.uspQCIdentifiedStockParametersGet";
            this.uspQCIdentifiedStockParametersGet.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.15208308398723602D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.lblPage,
            this.lblPageNumber,
            this.lblPageOf,
            this.lblPageTotal});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // lblPage
            // 
            this.lblPage.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.9999222755432129D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.50238120555877686D), Telerik.Reporting.Drawing.Unit.Inch(0.15208308398723602D));
            this.lblPage.Style.Font.Bold = true;
            this.lblPage.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.lblPage.Value = "Page:";
            // 
            // lblPageNumber
            // 
            this.lblPageNumber.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.5023026466369629D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.lblPageNumber.Name = "lblPageNumber";
            this.lblPageNumber.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.2000003308057785D), Telerik.Reporting.Drawing.Unit.Inch(0.15208308398723602D));
            this.lblPageNumber.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.lblPageNumber.Value = "= PageNumber";
            // 
            // lblPageOf
            // 
            this.lblPageOf.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.7023024559021D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.lblPageOf.Name = "lblPageOf";
            this.lblPageOf.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.09761936217546463D), Telerik.Reporting.Drawing.Unit.Inch(0.15208308398723602D));
            this.lblPageOf.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.lblPageOf.Value = "/";
            // 
            // lblPageTotal
            // 
            this.lblPageTotal.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.8000006675720215D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.lblPageTotal.Name = "lblPageTotal";
            this.lblPageTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.19305546581745148D), Telerik.Reporting.Drawing.Unit.Inch(0.15208308398723602D));
            this.lblPageTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.lblPageTotal.Value = "= PageCount";
            // 
            // QCInspection
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail,
            this.pageFooterSection1});
            this.Name = "QCInspection";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.Name = "InspectionID";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            this.ReportParameters.Add(reportParameter1);
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
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.0219554901123047D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.SubReport InspectionIdentifiedStockSubReport;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.TextBox lblPage;
        private Telerik.Reporting.TextBox lblPageNumber;
        private Telerik.Reporting.TextBox lblPageOf;
        private Telerik.Reporting.TextBox lblPageTotal;
        private Telerik.Reporting.Table table1;
        private Telerik.Reporting.SqlDataSource uspQCIdentifiedStockParametersGet;
    }
}