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






namespace PacificWeb.Components.Pages.Chart
{
    public partial class ChartPage
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
    }
}
