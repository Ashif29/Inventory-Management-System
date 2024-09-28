using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Extentions
{
    public class PdfService
    {
        private readonly IConverter _converter;
        private readonly IViewEngine _viewEngine;
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public PdfService(IConverter converter, IViewEngine viewEngine, ITempDataDictionaryFactory tempDataFactory)
        {
            _converter = converter;
            _viewEngine = viewEngine;
            _tempDataFactory = tempDataFactory;
        }

        public async Task<byte[]> GeneratePdfAsync<T>(string viewName, T model, ActionContext actionContext)
        {
            var htmlContent = await RenderViewToStringAsync(viewName, model, actionContext);
            var pdfDocument = CreatePdfDocument(htmlContent);
            return _converter.Convert(pdfDocument);
        }

        private HtmlToPdfDocument CreatePdfDocument(string htmlContent)
        {
            return new HtmlToPdfDocument
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
            },
                Objects = {
                new ObjectSettings
                {
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" },
                }
            }
            };
        }

        private async Task<string> RenderViewToStringAsync<T>(string viewName, T model, ActionContext actionContext)
        {
            // Get TempData using the provided ActionContext
            //var tempData = _tempDataFactory.GetTempData(actionContext);
            //var viewData = new ViewDataDictionary<T>(tempData, model);

            using var writer = new StringWriter();
            var viewResult = _viewEngine.FindView(actionContext, viewName, false);

            if (viewResult.View == null)
            {
                throw new FileNotFoundException($"View '{viewName}' not found.");
            }

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                tempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }
    }

}
