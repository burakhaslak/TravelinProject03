using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Project3Travelin.Entities;
using Project3Travelin.Settings;

namespace Project3Travelin.Controllers
{
    public class AdminReportController : Controller
    {
        private readonly IMongoCollection<Booking> _bookingCollection;
        private readonly IMongoCollection<Tour> _tourCollection;

        public AdminReportController(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _bookingCollection = database.GetCollection<Booking>(settings.BookingCollectionName);
            _tourCollection = database.GetCollection<Tour>(settings.TourCollectionName);
        }

        // ── REPORTS PAGE ──────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingCollection.Find(x => true).ToListAsync();
            var tours = await _tourCollection.Find(x => true).ToListAsync();

            var approvedBookings = bookings.Where(b => b.Status == "Approved").ToList();
            var totalRevenue = approvedBookings.Sum(b => b.TotalPrice);
            var avgRevenue = approvedBookings.Count > 0
                ? approvedBookings.Average(b => (double)b.TotalPrice)
                : 0;

            ViewBag.TotalBookings = bookings.Count;
            ViewBag.PendingBookings = bookings.Count(b => b.Status == "Pending");
            ViewBag.ApprovedBookings = bookings.Count(b => b.Status == "Approved");
            ViewBag.TotalRevenue = totalRevenue.ToString("N0");
            ViewBag.AvgRevenue = avgRevenue.ToString("N0");
            ViewBag.TotalTours = tours.Count;
            ViewBag.TotalCountries = tours.Select(t => t.Country).Distinct().Count();

            return View();
        }

        // ── BOOKING EXCEL ─────────────────────────────────────────────
        public async Task<IActionResult> ExportBookingsExcel()
        {
            var bookings = await _bookingCollection.Find(x => true)
                .SortByDescending(x => x.BookingDate).ToListAsync();

            using var wb = new XLWorkbook();

            // Sheet 1: All Bookings
            var ws = wb.Worksheets.Add("All Bookings");
            var headers = new[] { "Name", "Email", "Phone", "Tour", "Persons", "Unit Price", "Total Price", "Booking Date", "Status" };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2563eb");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            for (int i = 0; i < bookings.Count; i++)
            {
                var b = bookings[i];
                var row = i + 2;
                ws.Cell(row, 1).Value = b.NameSurname;
                ws.Cell(row, 2).Value = b.Mail;
                ws.Cell(row, 3).Value = b.Phone;
                ws.Cell(row, 4).Value = b.TourTitle;
                ws.Cell(row, 5).Value = b.PersonCount;
                ws.Cell(row, 6).Value = (double)b.UnitPrice;
                ws.Cell(row, 7).Value = (double)b.TotalPrice;
                ws.Cell(row, 8).Value = b.BookingDate.ToString("dd.MM.yyyy");
                ws.Cell(row, 9).Value = b.Status;

                // Status renklendirme
                var statusCell = ws.Cell(row, 9);
                statusCell.Style.Font.FontColor = b.Status == "Approved"
                    ? XLColor.FromHtml("#16a34a")
                    : b.Status == "Cancelled"
                        ? XLColor.FromHtml("#e53e3e")
                        : XLColor.FromHtml("#d97706");
                statusCell.Style.Font.Bold = true;

                if (i % 2 == 1)
                    ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#f8fafc");
            }

            // Totals row
            var totalRow = bookings.Count + 2;
            ws.Cell(totalRow, 6).Value = "TOTAL";
            ws.Cell(totalRow, 6).Style.Font.Bold = true;
            ws.Cell(totalRow, 7).FormulaA1 = $"=SUM(G2:G{bookings.Count + 1})";
            ws.Cell(totalRow, 7).Style.Font.Bold = true;
            ws.Cell(totalRow, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#eff6ff");

            ws.Columns().AdjustToContents();

            // Sheet 2: Status Summary
            var ws2 = wb.Worksheets.Add("Status Summary");
            ws2.Cell(1, 1).Value = "Status";
            ws2.Cell(1, 2).Value = "Count";
            ws2.Cell(1, 1).Style.Font.Bold = true;
            ws2.Cell(1, 2).Style.Font.Bold = true;

            var statuses = bookings.GroupBy(b => b.Status).ToList();
            for (int i = 0; i < statuses.Count; i++)
            {
                ws2.Cell(i + 2, 1).Value = statuses[i].Key;
                ws2.Cell(i + 2, 2).Value = statuses[i].Count();
            }
            ws2.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Bookings_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        // ── BOOKING PDF ───────────────────────────────────────────────
        public async Task<IActionResult> ExportBookingsPdf()
        {
            var bookings = await _bookingCollection.Find(x => true)
                .SortByDescending(x => x.BookingDate).ToListAsync();

            using var stream = new MemoryStream();
            var doc = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();

            // Title
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var subFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.GRAY);
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.WHITE);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);

            doc.Add(new Paragraph("Booking Report", titleFont));
            doc.Add(new Paragraph($"Generated: {DateTime.Now:dd MMMM yyyy HH:mm}  |  Total: {bookings.Count} bookings", subFont));
            doc.Add(new Paragraph(" "));

            var table = new PdfPTable(8) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 2.5f, 2.5f, 2f, 1f, 1.5f, 1.5f, 2f, 1.5f });

            var headerBg = new BaseColor(37, 99, 235);
            foreach (var h in new[] { "Name", "Email", "Tour", "Persons", "Unit Price", "Total Price", "Date", "Status" })
            {
                var cell = new PdfPCell(new Phrase(h, headerFont))
                {
                    BackgroundColor = headerBg,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 6
                };
                table.AddCell(cell);
            }

            bool alt = false;
            foreach (var b in bookings)
            {
                var bg = alt ? new BaseColor(248, 250, 252) : BaseColor.WHITE;
                var values = new[] { b.NameSurname, b.Mail, b.TourTitle, b.PersonCount.ToString(), $"€{b.UnitPrice:N0}", $"€{b.TotalPrice:N0}", b.BookingDate.ToString("dd.MM.yyyy"), b.Status };

                foreach (var v in values)
                {
                    var isStatus = v == b.Status;
                    var font = isStatus ? boldFont : cellFont;
                    if (isStatus)
                    {
                        font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8,
                            v == "Approved" ? new BaseColor(22, 163, 74)
                            : v == "Cancelled" ? new BaseColor(229, 62, 62)
                            : new BaseColor(217, 119, 6));
                    }
                    table.AddCell(new PdfPCell(new Phrase(v ?? "-", font))
                    {
                        BackgroundColor = bg,
                        Padding = 5,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                }
                alt = !alt;
            }

            doc.Add(table);
            doc.Close();

            return File(stream.ToArray(), "application/pdf", $"Bookings_{DateTime.Now:yyyyMMdd}.pdf");
        }

        // ── REVENUE EXCEL ─────────────────────────────────────────────
        public async Task<IActionResult> ExportRevenueExcel()
        {
            var bookings = await _bookingCollection.Find(x => x.Status == "Approved").ToListAsync();

            using var wb = new XLWorkbook();

            // Sheet 1: Revenue by Tour
            var ws = wb.Worksheets.Add("Revenue by Tour");
            ws.Cell(1, 1).Value = "Tour";
            ws.Cell(1, 2).Value = "Bookings";
            ws.Cell(1, 3).Value = "Total Revenue (€)";
            ws.Cell(1, 4).Value = "Avg per Booking (€)";
            ws.Row(1).Style.Font.Bold = true;
            ws.Row(1).Style.Fill.BackgroundColor = XLColor.FromHtml("#16a34a");
            ws.Row(1).Style.Font.FontColor = XLColor.White;

            var byTour = bookings.GroupBy(b => b.TourTitle).OrderByDescending(g => g.Sum(b => b.TotalPrice)).ToList();
            for (int i = 0; i < byTour.Count; i++)
            {
                var row = i + 2;
                ws.Cell(row, 1).Value = byTour[i].Key;
                ws.Cell(row, 2).Value = byTour[i].Count();
                ws.Cell(row, 3).Value = (double)byTour[i].Sum(b => b.TotalPrice);
                ws.Cell(row, 4).Value = (double)byTour[i].Average(b => b.TotalPrice);
                if (i % 2 == 1) ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#f0fdf4");
            }

            var totalRow = byTour.Count + 2;
            ws.Cell(totalRow, 1).Value = "TOTAL";
            ws.Cell(totalRow, 1).Style.Font.Bold = true;
            ws.Cell(totalRow, 2).FormulaA1 = $"=SUM(B2:B{byTour.Count + 1})";
            ws.Cell(totalRow, 3).FormulaA1 = $"=SUM(C2:C{byTour.Count + 1})";
            ws.Row(totalRow).Style.Font.Bold = true;
            ws.Row(totalRow).Style.Fill.BackgroundColor = XLColor.FromHtml("#eff6ff");
            ws.Columns().AdjustToContents();

            // Sheet 2: Monthly Revenue
            var ws2 = wb.Worksheets.Add("Monthly Revenue");
            ws2.Cell(1, 1).Value = "Month";
            ws2.Cell(1, 2).Value = "Bookings";
            ws2.Cell(1, 3).Value = "Revenue (€)";
            ws2.Row(1).Style.Font.Bold = true;
            ws2.Row(1).Style.Fill.BackgroundColor = XLColor.FromHtml("#16a34a");
            ws2.Row(1).Style.Font.FontColor = XLColor.White;

            var byMonth = bookings.GroupBy(b => b.BookingDate.ToString("yyyy-MM")).OrderBy(g => g.Key).ToList();
            for (int i = 0; i < byMonth.Count; i++)
            {
                var row = i + 2;
                ws2.Cell(row, 1).Value = byMonth[i].Key;
                ws2.Cell(row, 2).Value = byMonth[i].Count();
                ws2.Cell(row, 3).Value = (double)byMonth[i].Sum(b => b.TotalPrice);
                if (i % 2 == 1) ws2.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#f0fdf4");
            }
            ws2.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Revenue_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        // ── REVENUE PDF ───────────────────────────────────────────────
        public async Task<IActionResult> ExportRevenuePdf()
        {
            var bookings = await _bookingCollection.Find(x => x.Status == "Approved").ToListAsync();

            using var stream = new MemoryStream();
            var doc = new Document(PageSize.A4, 30, 30, 40, 40);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var subFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.GRAY);
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);

            doc.Add(new Paragraph("Revenue Report", titleFont));
            doc.Add(new Paragraph($"Generated: {DateTime.Now:dd MMMM yyyy HH:mm}  |  Approved bookings only", subFont));
            doc.Add(new Paragraph(" "));

            // Summary box
            var totalRevenue = bookings.Sum(b => b.TotalPrice);
            doc.Add(new Paragraph($"Total Revenue: €{totalRevenue:N0}  |  Total Bookings: {bookings.Count}  |  Avg: €{(bookings.Count > 0 ? bookings.Average(b => (double)b.TotalPrice) : 0):N0}", boldFont));
            doc.Add(new Paragraph(" "));

            // By Tour table
            doc.Add(new Paragraph("Revenue by Tour", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)));
            doc.Add(new Paragraph(" "));

            var table = new PdfPTable(4) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 3f, 1.5f, 2f, 2f });

            var headerBg = new BaseColor(22, 163, 74);
            foreach (var h in new[] { "Tour", "Bookings", "Total Revenue", "Avg / Booking" })
            {
                table.AddCell(new PdfPCell(new Phrase(h, headerFont))
                {
                    BackgroundColor = headerBg,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 7
                });
            }

            var byTour = bookings.GroupBy(b => b.TourTitle).OrderByDescending(g => g.Sum(b => b.TotalPrice)).ToList();
            bool alt = false;
            foreach (var g in byTour)
            {
                var bg = alt ? new BaseColor(240, 253, 244) : BaseColor.WHITE;
                foreach (var v in new[] { g.Key, g.Count().ToString(), $"€{g.Sum(b => b.TotalPrice):N0}", $"€{g.Average(b => (double)b.TotalPrice):N0}" })
                {
                    table.AddCell(new PdfPCell(new Phrase(v ?? "-", cellFont))
                    {
                        BackgroundColor = bg,
                        Padding = 5,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                }
                alt = !alt;
            }

            doc.Add(table);
            doc.Close();

            return File(stream.ToArray(), "application/pdf", $"Revenue_{DateTime.Now:yyyyMMdd}.pdf");
        }

        // ── TOURS EXCEL ───────────────────────────────────────────────
        public async Task<IActionResult> ExportToursExcel()
        {
            var tours = await _tourCollection.Find(x => true).SortBy(x => x.Country).ToListAsync();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Tours");

            var headers = new[] { "Title", "Country", "City", "Continent", "Capacity", "Price (€)", "Start Date", "End Date", "Duration" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#7c3aed");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            for (int i = 0; i < tours.Count; i++)
            {
                var t = tours[i];
                var row = i + 2;
                ws.Cell(row, 1).Value = t.Title;
                ws.Cell(row, 2).Value = t.Country;
                ws.Cell(row, 3).Value = t.City;
                ws.Cell(row, 4).Value = t.Continent ?? "-";
                ws.Cell(row, 5).Value = t.Capacity;
                ws.Cell(row, 6).Value = (double)t.Price;
                ws.Cell(row, 7).Value = t.TourStart.ToString("dd.MM.yyyy");
                ws.Cell(row, 8).Value = t.TourEnd.ToString("dd.MM.yyyy");
                ws.Cell(row, 9).Value = t.DayNight;

                if (i % 2 == 1)
                    ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#faf5ff");
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Tours_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        // ── TOURS PDF ─────────────────────────────────────────────────
        public async Task<IActionResult> ExportToursPdf()
        {
            var tours = await _tourCollection.Find(x => true).SortBy(x => x.Country).ToListAsync();

            using var stream = new MemoryStream();
            var doc = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var subFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.GRAY);
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.WHITE);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);

            doc.Add(new Paragraph("Tour Report", titleFont));
            doc.Add(new Paragraph($"Generated: {DateTime.Now:dd MMMM yyyy HH:mm}  |  Total: {tours.Count} tours", subFont));
            doc.Add(new Paragraph(" "));

            var table = new PdfPTable(8) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 3f, 1.5f, 1.5f, 1.5f, 1f, 1.2f, 1.5f, 1.5f });

            var headerBg = new BaseColor(124, 58, 237);
            foreach (var h in new[] { "Title", "Country", "City", "Continent", "Capacity", "Price", "Start", "End" })
            {
                table.AddCell(new PdfPCell(new Phrase(h, headerFont))
                {
                    BackgroundColor = headerBg,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 6
                });
            }

            bool alt = false;
            foreach (var t in tours)
            {
                var bg = alt ? new BaseColor(250, 245, 255) : BaseColor.WHITE;
                foreach (var v in new[] { t.Title, t.Country, t.City, t.Continent ?? "-", t.Capacity.ToString(), $"€{t.Price:N0}", t.TourStart.ToString("dd.MM.yy"), t.TourEnd.ToString("dd.MM.yy") })
                {
                    table.AddCell(new PdfPCell(new Phrase(v ?? "-", cellFont))
                    {
                        BackgroundColor = bg,
                        Padding = 5,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                }
                alt = !alt;
            }

            doc.Add(table);
            doc.Close();

            return File(stream.ToArray(), "application/pdf", $"Tours_{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}