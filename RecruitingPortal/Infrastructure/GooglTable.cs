using RecruitingPortal.Models;
using RecruitingPortal.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Infrastructure
{
    public static class GooglTable
    {
        //public static GoogleVisualizationDataTable ConstructDataTable(List<DataValueAndValueSummaryViewModel> data)
        //{
        //    var dataTable = new GoogleVisualizationDataTable();

        //    // Get distinct markets from the data
        //    var CompanyNames = data.Select(x => x.Name).Distinct().OrderBy(x => x);

        //    //var results = from p in data
        //    //              group p by p.Name into g
        //    //              select new DataValueGroupByTransactionViewModel
        //    //              {
        //    //                  Name = g.Key,
        //    //                  TransactionCount = g.Count(),
        //    //                  TransactionSum = g.Select(m => m.TransactionSum).Sum()
        //    //              };



        //    // Specify the columns for the DataTable.
        //    // In this example, it is Market and then a column for each year.
        //    dataTable.AddColumn("Name", "string");
        //    dataTable.AddColumn("TransactionSum", "number");

        //    // Specify the rows for the DataTable.
        //    // Each Market will be its own row, containing the total sales for each year.
        //    foreach (var company in CompanyNames)
        //    {
        //        var values = new List<object>(new[] { company });

        //        var transactionSum = data
        //            .Where(x => x.Name == company)
        //            .Sum(x => x.TOTAL ?? 0.0M);
        //        //.Select(x => x.TransactionSum)

        //        values.Add(transactionSum);

        //        dataTable.AddRow(values);
        //    }

        //    return dataTable;
        //}

        //public static GoogleVisualizationDataTable ConstructDataTable(List<BestnTOPItemSalesViewModel> data)
        //{
        //    var dataTable = new GoogleVisualizationDataTable();

        //    // data = data.OrderByDescending(x => x.SalePrice).ToList< DataValueBestWorstItemSaleViewModel>();
        //    var ProductNames = data.Select(x => x.ProductName).Distinct();

        //    // Specify the columns for the DataTable.
        //    // In this example, it is Market and then a column for each year.
        //    dataTable.AddColumn("Amount", "number");
        //    dataTable.AddColumn("Item Name", "string");
        //    dataTable.AddColumn("Sales", "number");
        //    dataTable.AddColumn("%", "number");
        //    dataTable.AddColumn("Rank", "number");
        //    dataTable.AddColumn("Rate Of Increase", "string");
        //    dataTable.AddColumn("Qty", "number");

        //    // Specify the rows for the DataTable.
        //    // Each Market will be its own row, containing the total sales for each year.
        //    int rank = 1;
        //    foreach (var productName in ProductNames)
        //    {
        //        var salePrice = data.Where(x => x.ProductName == productName).Select(x => x.AmountSum).FirstOrDefault();
        //        var values = new List<object>(new[] { salePrice.ToString() });

        //        var product = data
        //            .Where(x => x.ProductName == productName)
        //            .Select(x => x.ProductName)
        //            .FirstOrDefault();

        //        var salePercent = data
        //            .Where(x => x.ProductName == productName)
        //            .Select(x => x.SalePercent)
        //            .FirstOrDefault();

        //        var price = data
        //            .Where(x => x.ProductName == productName)
        //            .Select(x => x.AmountSum)
        //            .FirstOrDefault();

        //        var salePercentDifference = data
        //            .Where(x => x.ProductName == productName)
        //            .Select(x => x.SalePercentDifferece)
        //            .FirstOrDefault();

        //        var qtySum = data
        //            .Where(x => x.ProductName == productName)
        //            .Select(x => x.QtySum)
        //            .FirstOrDefault();

        //        values.Add(product);
        //        values.Add(price);
        //        values.Add(Math.Round((double)salePercent, 2));
        //        values.Add(rank);

        //        var salePercentDiff = Math.Round((double)salePercentDifference, 2);
        //        var salePercentDiffGlypticon = "<span class='text-success glyphicon glyphicon-triangle-top'></span>";
        //        if (salePercentDiff < 0.0D)
        //        {
        //            salePercentDiffGlypticon = "<span class='text-danger glyphicon glyphicon-triangle-bottom'></span>";
        //        }
        //        values.Add(salePercentDiffGlypticon + "&nbsp;" + salePercentDiff.ToString());
        //        values.Add(qtySum);

        //        dataTable.AddRow(values);
        //        rank++;
        //    }
        //    return dataTable;
        //}

        //public static GoogleVisualizationDataTable ConstructDataTable(List<BestnTOPItemSalesReportViewModel> data, ModuleName moduleName, string range1)
        //{
        //    var dataTable = new GoogleVisualizationDataTable();
        //    var ProductNames = data.Select(x => x.ProductName).Distinct();

        //    var labels = GetLabel(moduleName, range1, null);

        //    var previousColumnLabel = labels.First().Key;
        //    var currentColumnLabel = labels.First().Value;

        //    dataTable.AddColumn("ProductName", "string");
        //    dataTable.AddColumn(currentColumnLabel, "number");
        //    dataTable.AddColumn(previousColumnLabel, "number");

        //    // Specify the rows for the DataTable.
        //    // Each Market will be its own row, containing the total sales for each year.
        //    int rank = 1;
        //    foreach (var productName in ProductNames)
        //    {
        //        var values = new List<object>(new[] { productName });

        //        var amountSumCurrent = data
        //            .Where(x => x.ProductName == productName)
        //            .Select(x => x.AmountSumCurrent)
        //            .FirstOrDefault();

        //        var amountSumPrevious = data
        //            .Where(x => x.ProductName == productName)
        //            .Select(x => x.AmountSumPrevious)
        //            .FirstOrDefault();

        //        values.Add(amountSumCurrent);
        //        values.Add(amountSumPrevious);


        //        dataTable.AddRow(values);
        //        rank++;
        //    }

        //    return dataTable;
        //}

        //public static GoogleVisualizationDataTable ConstructDataTable(decimal bestNCurrentTotal, decimal bestNPreviousTotal, decimal CurrentAllTotal, decimal PreviousAllTotal, ModuleName moduleName, string range1, bool isAllSum)
        //{
        //    var dataTable = new GoogleVisualizationDataTable();


        //    var labels = GetLabel(moduleName, range1, null);

        //    var previousColumnLabel = labels.First().Key;
        //    var currentColumnLabel = labels.First().Value;

        //    dataTable.AddColumn("Period", "string");
        //    dataTable.AddColumn("TOP 10 SALES", "number");

        //    if (isAllSum)
        //    {
        //        // 1 st row
        //        var values = new List<object>(new[] { previousColumnLabel });
        //        values.Add(PreviousAllTotal);
        //        dataTable.AddRow(values);

        //        // 2 nd row
        //        values = new List<object>(new[] { currentColumnLabel });
        //        values.Add(CurrentAllTotal);
        //        dataTable.AddRow(values);
        //    }
        //    else
        //    {
        //        // 1 st row
        //        var values = new List<object>(new[] { previousColumnLabel });
        //        values.Add(bestNPreviousTotal);
        //        dataTable.AddRow(values);

        //        // 2 nd row
        //        values = new List<object>(new[] { currentColumnLabel });
        //        values.Add(bestNCurrentTotal);
        //        dataTable.AddRow(values);
        //    }

        //    return dataTable;
        //}

        //public static GoogleVisualizationDataTable ConstructDataTable(List<DataValueWithTimeFrameViewModel> data, SumOf sumOf)
        //{
        //    var dataTable = new GoogleVisualizationDataTable();

        //    // Get distinct markets from the data
        //    // var everyTwoHours = data.Select(x => x.TimeFrameName).Distinct().OrderBy(x => x);
        //    var orderedData = data.Distinct().OrderBy(x => x.Id);

        //    // Specify the columns for the DataTable.
        //    // In this example, it is Market and then a column for each year.
        //    dataTable.AddColumn("PerHour", "string");

        //    if (sumOf == SumOf.Count)
        //    {
        //        dataTable.AddColumn("SaleCount", "number");

        //        // Specify the rows for the DataTable.
        //        // Each Market will be its own row, containing the total sales for each year.
        //        foreach (var hour in orderedData.Select(x => x.TimeFrameName))
        //        {
        //            var values = new List<object>(new[] { hour });

        //            var totalSales = data
        //                .Where(x => x.TimeFrameName == hour)
        //                .Select(x => x.SaleCount)
        //                .FirstOrDefault();
        //            values.Add(totalSales);

        //            dataTable.AddRow(values);
        //        }
        //    }
        //    else if (sumOf == SumOf.Sale)
        //    {
        //        dataTable.AddColumn("SalePrice", "number");

        //        // Specify the rows for the DataTable.
        //        // Each Market will be its own row, containing the total sales for each year.
        //        foreach (var hour in orderedData.Select(x => x.TimeFrameName))
        //        {
        //            var values = new List<object>(new[] { hour });

        //            var totalSales = data
        //                .Where(x => x.TimeFrameName == hour)
        //                .Select(x => x.SalePrice)
        //                .FirstOrDefault();
        //            values.Add(totalSales);

        //            dataTable.AddRow(values);
        //        }
        //    }

        //    return dataTable;
        //}

        //public static GoogleVisualizationDataTable ConstructDataTable(DataTable data)
        //{
        //    var dataTable = new GoogleVisualizationDataTable();
        //    dataTable.AddColumn("PerHour", "string");
        //    for (int j = 1; j < data.Columns.Count; j++)
        //    {
        //        dataTable.AddColumn(data.Columns[j].ColumnName, "number");
        //    }

        //    for (int i = 0; i < data.Rows.Count; i++)
        //    {
        //        var values = new List<object>(new[] { data.Rows[i][0] });
        //        for (int j = 1; j < data.Columns.Count; j++)
        //        {
        //            decimal v = 0;
        //            if (data.Rows[i][j] != null && !string.IsNullOrEmpty(data.Rows[i][j].ToString()))
        //            {
        //                v = decimal.Parse(data.Rows[i][j].ToString());
        //            }
        //            values.Add(v);
        //        }

        //        dataTable.AddRow(values);
        //    }

        //    // NOTE: google table requires at least two columns (x Axis and y Axis)
        //    if (dataTable.cols.Count == 1)
        //    {
        //        dataTable.AddColumn("", "number");
        //    }

        //    return dataTable;
        //}

        //// two time frame
        //public static GoogleVisualizationDataTable ConstructDataTable(List<DataValuePerTwoTimeFrameViewModelComparison> data, SumOf sumOf, ModuleName moduleName, string range1, DateSearchRange? enumRange)
        //{
        //    var dataTable = new GoogleVisualizationDataTable();
        //    var orderedData = data.Distinct().OrderBy(x => x.Id);

        //    var labels = GetLabel(moduleName, range1, enumRange);

        //    var previousColumnLabel = labels.First().Key;
        //    var currentColumnLabel = labels.First().Value;

        //    int currentCount = 0;
        //    int previousCount = 0;
        //    decimal currentSalePrice = 0.0M;
        //    decimal previousSalePrice = 0.0M;
        //    decimal averageCount = 0.0M;
        //    decimal averageSalePrice = 0.0M;

        //    // Specify the columns for the DataTable.
        //    // In this example, it is Market and then a column for each year.
        //    dataTable.AddColumn("PerHour", "string");
        //    dataTable.AddColumn(previousColumnLabel, "number");
        //    dataTable.AddColumn(currentColumnLabel, "number");
        //    dataTable.AddColumn("Average", "number");

        //    // Specify the rows for the DataTable.
        //    // Each Market will be its own row, containing the total sales for each year.
        //    foreach (var member in orderedData.Select(x => x.TimeFrameName))
        //    {
        //        var values = new List<object>(new[] { member });

        //        if (sumOf == SumOf.Count)
        //        {
        //            previousCount = data
        //                .Where(x => x.TimeFrameName == member)
        //                .Select(x => x.PreviousCount)
        //                .FirstOrDefault();
        //            values.Add(previousCount);

        //            currentCount = data
        //                .Where(x => x.TimeFrameName == member)
        //                .Select(x => x.CurrentCount)
        //                .FirstOrDefault();
        //            values.Add(currentCount);

        //            averageCount = data
        //                .Where(x => x.TimeFrameName == member)
        //                .Select(x => x.AverageCount)
        //                .FirstOrDefault();
        //            values.Add(averageCount);
        //        }
        //        else if (sumOf == SumOf.Sale)
        //        {
        //            previousSalePrice = data
        //            .Where(x => x.TimeFrameName == member)
        //            .Select(x => x.PreviousSalePrice)
        //            .FirstOrDefault();
        //            values.Add(previousSalePrice);

        //            currentSalePrice = data
        //                .Where(x => x.TimeFrameName == member)
        //                .Select(x => x.CurrentSalePrice)
        //                .FirstOrDefault();
        //            values.Add(currentSalePrice);

        //            averageSalePrice = data
        //                .Where(x => x.TimeFrameName == member)
        //                .Select(x => x.AverageSalePrice)
        //                .FirstOrDefault();
        //            values.Add(averageSalePrice);
        //        }
        //        dataTable.AddRow(values);
        //    }
        //    return dataTable;
        //}

        //// single time frame
        //public static GoogleVisualizationDataTable ConstructDataTable(List<DataValueWithTimeFrameViewModel> data, SumOf sumOf, ModuleName moduleName, string range1, DateSearchRange? enumRange, List<string> chosenProductItems)
        //{
        //    var dataTable = new GoogleVisualizationDataTable();
        //    decimal SalePrice = 0.0M;
        //    int SaleCount = 0;

        //    if (chosenProductItems.Count == 1) // only 1 item
        //    {
        //        var orderedData = data.OrderBy(x => x.Id);
        //        var firstItemColumnLabel = data.FirstOrDefault().ProductName; // first item name

        //        dataTable.AddColumn("PerHour", "string");
        //        dataTable.AddColumn(firstItemColumnLabel, "number");

        //        foreach (var member in orderedData.Select(x => x.TimeFrameName))
        //        {
        //            var values = new List<object>(new[] { member.ToString() });

        //            if (sumOf == SumOf.Sale)
        //            {
        //                SalePrice = data
        //                .Where(x => x.TimeFrameName == member)
        //                .Select(x => x.SalePrice)
        //                .FirstOrDefault();

        //                values.Add(SalePrice);
        //            }
        //            else if (sumOf == SumOf.Count)
        //            {
        //                SaleCount = data
        //                .Where(x => x.TimeFrameName == member)
        //                .Select(x => x.SaleCount)
        //                .FirstOrDefault();

        //                values.Add(SaleCount);
        //            }

        //            dataTable.AddRow(values);
        //        }
        //    }
        //    else if (chosenProductItems.Count == 2) // 2 items
        //    {
        //        dataTable.AddColumn("PerHour", "string");
        //        foreach (var innerMember in chosenProductItems)
        //        {
        //            dataTable.AddColumn(innerMember, "number");
        //        }

        //        var orderedData = data.OrderBy(x => x.Id);

        //        List<object> values = null;

        //        int i = 0;
        //        foreach (var member in orderedData.Select(x => x.TimeFrameName))
        //        {
        //            if (i % chosenProductItems.Count == 0) // even -->> first item
        //            {
        //                // on first looping, define pk value
        //                values = new List<object>(new[] { member.ToString() });
        //            }

        //            foreach (var innerMember in chosenProductItems)
        //            {
        //                if (sumOf == SumOf.Sale)
        //                {
        //                    SalePrice = data
        //                    .Where(x => x.TimeFrameName == member && x.ProductName.Equals(innerMember, StringComparison.InvariantCultureIgnoreCase))
        //                    .Select(x => x.SalePrice)
        //                    .FirstOrDefault();

        //                    values.Add(SalePrice);
        //                }
        //                else if (sumOf == SumOf.Count)
        //                {
        //                    SaleCount = data
        //                    .Where(x => x.TimeFrameName == member && x.ProductName.Equals(innerMember, StringComparison.InvariantCultureIgnoreCase))
        //                    .Select(x => x.SaleCount)
        //                    .FirstOrDefault();

        //                    values.Add(SaleCount);
        //                }
        //            }

        //            if (i % chosenProductItems.Count == 1) // odd -->> second item
        //            {
        //                // on second looping, add row
        //                dataTable.AddRow(values);
        //            }

        //            i++;
        //        }
        //    }

        //    return dataTable;
        //}

        public static GoogleVisualizationDataTable ConstructDataTable(List<AppliedCandidateViewModel> data)
        {
            var dataTable = new GoogleVisualizationDataTable();

            // Get distinct markets from the data
            // var CompanyNames = data.Select(x => x.).Distinct().OrderBy(x => x);

            // Specify the columns for the DataTable.
            // In this example, it is Market and then a column for each year.
            dataTable.AddColumn("ServiceName", "string");
            dataTable.AddColumn("AppliedCount", "number");
            

            // Specify the rows for the DataTable.
            // Each Market will be its own row, containing the total sales for each year.
            foreach (var member in data)
            {
                var values = new List<object>(new[] { member.ServiceName });
                values.Add(member.AppliedCount);
                dataTable.AddRow(values);
            }

            return dataTable;
        }

        public static GoogleVisualizationDataTable ConstructDataTable(List<JobStatusStatisticsViewModel> data)
        {
            var dataTable = new GoogleVisualizationDataTable();

            // Get distinct markets from the data
            // var CompanyNames = data.Select(x => x.).Distinct().OrderBy(x => x);

            // Specify the columns for the DataTable.
            // In this example, it is Market and then a column for each year.
            dataTable.AddColumn("StatusName", "string");
            dataTable.AddColumn("StatusCount", "number");


            // Specify the rows for the DataTable.
            // Each Market will be its own row, containing the total sales for each year.
            foreach (var member in data)
            {
                var values = new List<object>(new[] { member.StatusName });
                values.Add(member.StatusCount);
                dataTable.AddRow(values);
            }

            return dataTable;
        }
        
        public static GoogleVisualizationDataTable ConstructDataTable(List<JobPostStatisticsViewModel> data)
        {
            var dataTable = new GoogleVisualizationDataTable();

            // Get distinct markets from the data
            // var CompanyNames = data.Select(x => x.).Distinct().OrderBy(x => x);

            // Specify the columns for the DataTable.
            // In this example, it is Market and then a column for each year.
            dataTable.AddColumn("StatusName", "string");
            dataTable.AddColumn("PostedCount", "number");


            // Specify the rows for the DataTable.
            // Each Market will be its own row, containing the total sales for each year.
            foreach (var member in data)
            {
                var values = new List<object>(new[] { member.ServiceName });
                values.Add(member.PostedCount);
                dataTable.AddRow(values);
            }

            return dataTable;
        }

        public static GoogleVisualizationDataTable ConstructDataTable(List<RequestGuardStatisticsViewModel> data)
        {
            var dataTable = new GoogleVisualizationDataTable();

            // Get distinct markets from the data
            // var CompanyNames = data.Select(x => x.).Distinct().OrderBy(x => x);

            // Specify the columns for the DataTable.
            // In this example, it is Market and then a column for each year.
            dataTable.AddColumn("StatusName", "string");
            dataTable.AddColumn("RequestedCount", "number");
            dataTable.AddColumn("PostedCount", "number");


            // Specify the rows for the DataTable.
            // Each Market will be its own row, containing the total sales for each year.
            foreach (var member in data)
            {
                var values = new List<object>(new[] { member.ServiceName });
                values.Add(member.RequestedCount);
                values.Add(member.PostedCount);
                dataTable.AddRow(values);
            }

            return dataTable;
        }

        //private static Dictionary<string, string> GetLabel(ModuleName moduleName, string range1, DateSearchRange? enumRange)
        //{
        //    Dictionary<string, string> PreviousCurrentLabel = new Dictionary<string, string>();

        //    switch (moduleName)
        //    {
        //        case ModuleName.Overview:
        //        case ModuleName.HeadOffice:
        //        case ModuleName.SaleSummary:
        //        case ModuleName.SaleSummaryHO:
        //        case ModuleName.SaleTrend:
        //        case ModuleName.SaleTrendHO:
        //        case ModuleName.Transaction:
        //        case ModuleName.TransactionHO:
        //        case ModuleName.TransactionItem:
        //        case ModuleName.TransactionItemHO:
        //            switch (enumRange)
        //            {
        //                case DateSearchRange.Daily:
        //                    PreviousCurrentLabel.Add("Yesterday", "Today");
        //                    break;
        //                case DateSearchRange.Weekly:
        //                    PreviousCurrentLabel.Add("Last Week", "This Week");
        //                    break;
        //                case DateSearchRange.Monthly:
        //                    PreviousCurrentLabel.Add("Last Month", "This Month");
        //                    break;
        //                case DateSearchRange.Yearly:
        //                    PreviousCurrentLabel.Add("Last Year", "This Year");
        //                    break;
        //            }
        //            break;
        //        case ModuleName.ReportBasic:
        //        case ModuleName.ReportAdvance:
        //            switch (range1.ToLowerInvariant())
        //            {
        //                case "d":
        //                    PreviousCurrentLabel.Add("Previous Day", "Current Day");
        //                    break;
        //                case "w":
        //                    PreviousCurrentLabel.Add("Previous Week", "Current Week");
        //                    break;
        //                case "m":
        //                    PreviousCurrentLabel.Add("Previous Month", "Current Month");
        //                    break;
        //                case "q":
        //                    PreviousCurrentLabel.Add("Previous Quarter", "Current Quarter");
        //                    break;
        //                case "y":
        //                    PreviousCurrentLabel.Add("Previous Year", "Current Year");
        //                    break;

        //            }
        //            break;
        //    }

        //    return PreviousCurrentLabel;
        //}
    }
}