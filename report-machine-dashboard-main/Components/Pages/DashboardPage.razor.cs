using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Models;
using Radzen;
using Radzen.Blazor;
using Services;
using ClosedXML.Excel;






namespace PacificWeb.Components.Pages
{
    public partial class DashboardPage
    {

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public pdjService pdjService { get; set; }

        IList<MasterCustomer> selectedFromGrid;
        IList<MasterCustomer> selectedCustomer;
        // protected RadzenDataGrid<Models.MasterCustomer> grid0;

        protected IEnumerable<MasterCustomer> tbListData;

        protected IEnumerable<MasterCustomer> tbListDataFromDb;

        protected string namaMenu = "Dashboard";


        protected string BtnTambahStyleEnabled = "background-color: transparent; color: #1e3a6a; opacity: 1";

        protected string BtnTambahStyleDisabled = "background-color:transparent; color: #1e3a6a; opacity: 0.5";

        protected bool BtnTambahIsDisabled = false;

        protected string search = "";
        protected bool BtnActionIsVisible = true;
        protected string selectedFilter = "Aktif";

        protected bool allowPickColumn = false;
        protected string columnToggleIcon = "toggle_off";
        protected string columnToggleText = "Pilih Kolom";

        protected bool myColumnVisible = true;
        protected string searchText = "";



        string SearchText { get; set; }
        string GridSearchText { get; set; }





        protected override void OnInitialized()
        {
            GenerateDummyData();
            LoadDummyDataHistory();

            GenerateDummyDataChartRadzen();
            visibleDatasets = dataSets.Keys.ToHashSet();

            GenerateDummyDataChartRadzenDropdown();
            availableDataSets = dataSets.Keys.ToList();
            selectedDataSets = availableDataSets.Take(3).ToList();

            // Set timer untuk update setiap 1 menit (60.000 ms)
            updateTimer = new System.Timers.Timer(5000);
            updateTimer.Elapsed += async (s, e) => await UpdateDataAsync();
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;
        }


        // ~~~~~Table Real-time~~~~~
        RadzenDataGrid<MeasurementData> Grid;
        private List<MeasurementData> measurementList = new();
        private System.Timers.Timer updateTimer;
        private DateTime lastUpdated = DateTime.Now;
        private Random rand = new();
        public class MeasurementData
        {
            public string Measurement { get; set; }
            public double Value { get; set; }
            public string Units { get; set; }
        }
        private async Task UpdateDataAsync()
        {
            await InvokeAsync(() =>
            {
                GenerateDummyData();
                lastUpdated = DateTime.Now;
                StateHasChanged();
            });
        }
        private void GenerateDummyData()
        {
            measurementList = new List<MeasurementData>
            {
                new MeasurementData { Measurement = "Temperature", Value = Math.Round(35 + rand.NextDouble() * 5, 2), Units = "°C" },
                new MeasurementData { Measurement = "Pressure", Value = Math.Round(3 + rand.NextDouble(), 2), Units = "Pa" },
                new MeasurementData { Measurement = "Flowrate", Value = Math.Round(50 + rand.NextDouble() * 15, 2), Units = "L/s" },
                new MeasurementData { Measurement = "Energy Consumption", Value = Math.Round(8 + rand.NextDouble() * 5, 2), Units = "kWh" }
            };
        }
        public void Dispose()
        {
            updateTimer?.Dispose();
        }



        // ~~~~~Table History~~~~~
        RadzenDataGrid<HistoryRecord> GridRecord;
        private bool ExportSelectedRowsOnly { get; set; }
        private List<HistoryRecord> HistoryData = new();
        private DateTime StartDate = DateTime.Now.AddHours(-6);
        private DateTime EndDate = DateTime.Now;
        private bool isRefresh = false;
        private bool isRefreshExport = false;
        public class HistoryRecord
        {
            public string Measurement { get; set; }
            public double Value { get; set; }
            public string Units { get; set; }
            public DateTime Timestamp { get; set; }
        }
        private void LoadDummyDataHistory()
        {
            var rnd = new Random();
            string[] measurements = { "Temperature", "Pressure", "Flowrate", "Energy Consumption" };
            string[] units = { "°C", "PA", "L/s", "kWh" };

            HistoryData = Enumerable.Range(1, 50).Select(i => new HistoryRecord
            {
                Measurement = measurements[rnd.Next(measurements.Length)],
                Value = Math.Round(rnd.NextDouble() * 100, 2),
                Units = units[rnd.Next(units.Length)],
                Timestamp = DateTime.Now.AddMinutes(-rnd.Next(360))
            })
            .Where(h => h.Timestamp >= StartDate && h.Timestamp <= EndDate)
            .OrderByDescending(h => h.Timestamp)
            .ToList();
        }
        private void RefreshData()
        {
            isRefresh = true;
            LoadDummyDataHistory();
            isRefresh = false;
            StateHasChanged();
        }
        //private async Task ExportCsv_Click()
        //{
        //    isRefreshExport = true;
        //    await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions()
        //    {
        //        ExportSelectedRowsOnly = ExportSelectedRowsOnly
        //    });
        //    isRefreshExport = false;
        //}
        public async Task ExportTable(string type)
        {
            isRefreshExport = true;
            StateHasChanged();

            if (type.ToLower() == "csv")
            {
                var csv = new StringBuilder();
                csv.AppendLine("Measurement,Value,Units,Timestamp");

                foreach (var item in HistoryData)
                {
                    csv.AppendLine($"{item.Measurement},{item.Value},{item.Units},{item.Timestamp:yyyy-MM-dd HH:mm:ss}");
                }

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                var fileName = $"HistoryData_{DateTime.Now:yyyyMMddHHmmss}.csv";

                using var stream = new MemoryStream(bytes);
                using var streamRef = new Microsoft.JSInterop.DotNetStreamReference(stream);
                await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            }
            else if (type.ToLower() == "excel")
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("History Data");

                // Header
                worksheet.Cell(1, 1).Value = "Measurement";
                worksheet.Cell(1, 2).Value = "Value";
                worksheet.Cell(1, 3).Value = "Units";
                worksheet.Cell(1, 4).Value = "Timestamp";

                // Data rows
                int row = 2;
                foreach (var item in HistoryData)
                {
                    worksheet.Cell(row, 1).Value = item.Measurement;
                    worksheet.Cell(row, 2).Value = item.Value;
                    worksheet.Cell(row, 3).Value = item.Units;
                    worksheet.Cell(row, 4).Value = item.Timestamp;
                    worksheet.Cell(row, 4).Style.DateFormat.Format = "yyyy-MM-dd HH:mm:ss";
                    row++;
                }

                // Auto-fit kolom
                worksheet.Columns().AdjustToContents();

                // Simpan workbook ke MemoryStream
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                // Nama file
                var fileName = $"HistoryData_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                // Download via JS
                using var streamRef = new DotNetStreamReference(stream);
                await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            }

            isRefreshExport = false;
            StateHasChanged();
        }



        // ~~~~~ChartJS~~~~~
        private List<string> labels = new();
        private List<object> datasets = new();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadMultiLineChart();
                await JSRuntime.InvokeVoidAsync("renderMultiLineChart", labels, datasets);
            }
        }
        private async Task LoadMultiLineChart()
        {
            // Label hari dalam seminggu
            labels = new List<string> { "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu", "Minggu" };

            // Warna berbeda untuk tiap dataset
            string[] colors = {
                "#1E88E5", "#43A047", "#E53935", "#FB8C00",
                "#8E24AA", "#00ACC1", "#C0CA33", "#F4511E"
            };

            // Dummy data (8 jenis produk misalnya)
            for (int i = 0; i < 8; i++)
            {
                var random = new Random();
                var dataPoints = Enumerable.Range(0, labels.Count)
                                           .Select(_ => random.Next(10, 100))
                                           .ToList();

                datasets.Add(new
                {
                    label = $"Produk {i + 1}",
                    data = dataPoints,
                    color = colors[i],
                    backgroundColor = $"{colors[i]}33"
                });
            }
        }


        // ~~~~~Radzen Chart~~~~~
        class DataItem
        {
            public string Date { get; set; }
            public double Revenue { get; set; }
        }
        private Dictionary<string, List<DataItem>> dataSets = new();
        private HashSet<string> visibleDatasets = new();

        string FormatAsUSD(object value)
        {
            return $"${value}";
        }

        private void GenerateDummyDataChartRadzen()
        {
            var rnd = new Random();
            string[] names = { "2023", "2024", "Product A", "Product B", "Product C", "Product D", "Region X", "Region Y" };
            var dates = Enumerable.Range(1, 8).Select(i => $"Day {i}").ToList();

            foreach (var name in names)
            {
                var list = new List<DataItem>();
                foreach (var date in dates)
                {
                    list.Add(new DataItem
                    {
                        Date = date,
                        Revenue = Math.Round(rnd.NextDouble() * 10000, 2)
                    });
                }
                dataSets[name] = list;
            }
        }

        private void ToggleDataset(string name)
        {
            if (visibleDatasets.Contains(name))
                visibleDatasets.Remove(name);
            else
                visibleDatasets.Add(name);

            StateHasChanged();
        }

        private Dictionary<string, List<DataItem>> dataSetsDropdown = new();
        private List<string> availableDataSets = new();
        private List<string> selectedDataSets = new();
        private void GenerateDummyDataChartRadzenDropdown()
        {
            var rnd = new Random();
            string[] names = { "2023", "2024", "Product A", "Product B", "Product C", "Product D", "Region X", "Region Y" };
            var dates = Enumerable.Range(1, 8).Select(i => $"Day {i}").ToList();

            foreach (var name in names)
            {
                var list = new List<DataItem>();
                foreach (var date in dates)
                {
                    list.Add(new DataItem
                    {
                        Date = date,
                        Revenue = Math.Round(rnd.NextDouble() * 10000, 2)
                    });
                }
                dataSetsDropdown.Add(name, list);
            }
        }


        // ~~~~~Button Export~~~~~
        private bool isExportPdf = false;
        private async Task Export(string idChart)
        {

            isExportPdf = true;
            StateHasChanged();
            
            await JSRuntime.InvokeVoidAsync("exportChart", idChart);

            isExportPdf = false;
            StateHasChanged();
        }
        private async Task ExportImage(string idChart, string format)
        {
            isExportPdf = true;
            StateHasChanged();

            await JSRuntime.InvokeVoidAsync("exportChartImage", idChart, format);

            isExportPdf = false;
            StateHasChanged();
        }
    }
}
