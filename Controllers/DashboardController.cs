using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales_Dash_Board.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Sales_Dash_Board.Models.Document;
using Version = Sales_Dash_Board.Models.Version;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sales_Dash_Board.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        // GET: api/<DashboardController>
        private readonly SalesDashboard_dbContext _context;

        public DashboardController(SalesDashboard_dbContext context)
        {
            _context = context;
        }

        // GET: api/SalesTrack
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesTrack>>> GetSalesTracks()
        {
            return await _context.SalesTrack.ToListAsync();
        }

        [HttpGet("SalesList")]
        public IEnumerable<SalesTrack> SalesList(string FromDate, string ToDate, int SalesPersonID = 0, int VersionID = 0, int StateID = 0)
        {
            if (FromDate == "" || FromDate == null) { FromDate = DateTime.Now.ToString("yyyy-MM-dd"); }
            if (ToDate == "" || ToDate == null) { ToDate = DateTime.Now.ToString("yyyy-MM-dd"); }
            FromDate = FromDate + " 00:00:01";
            ToDate = ToDate + " 23:59:59";

            var salesList = _context.SalesList(FromDate, ToDate, SalesPersonID, VersionID, StateID);
            return salesList;
        }
        // GET: api/SalesTrack/5
        [HttpGet("GetSalesTrack")]
        public async Task<ActionResult<SalesTrack>> GetSalesTrack(int id)
        {
            var salesTrack = await _context.SalesTrack.FindAsync(id);

            if (salesTrack == null)
            {
                return NotFound();
            }

            return salesTrack;
        }

        [HttpPost("CreateSalesTrack")]
        public async Task<ActionResult<SalesTrack>> CreateSalesTrack(SalesTrack salesTrack)
        {
            _context.SalesTrack.Add(salesTrack);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalesTrack), new { id = salesTrack.Id }, salesTrack);
        }
        [HttpPost("ModifyDocument")]
        public async Task<ActionResult<DocModify>> ModifyDocument(DocModify Doc, string newCustomerName, string newBillingAddress)
        {
            // Specify the paths to your Word documents
            string sourceFilePath = @"TemplateFiles\TemplateDocument.docx";
            string destinationFilePath = @"TemplateFiles\ModifiedFiles\ModifiedDocument.Docx";

            // Create a backup copy of the original document in case something goes wrong
            string backupFilePath = Path.ChangeExtension(destinationFilePath, ".backup");
            System.IO.File.Copy(destinationFilePath, backupFilePath, true);

            try
            {
                // Copy the original template to the destination file
                System.IO.File.Copy(sourceFilePath, destinationFilePath, true);

                // Load the Word document using OpenXml from the destination file
                using (WordprocessingDocument document = WordprocessingDocument.Open(destinationFilePath, true))
                {
                    // Replace Customer Name
                    ReplaceText(document, "Sandeep", newCustomerName);

                    // Replace Billing Address
                    ReplaceText(document, "BAdress", newBillingAddress);

                    Doc.TextFilePath = "../TemplateFiles/ModifiedFiles/ModifiedDocument.Docx";
                    // Save the changes to the document
                    document.Save();

                }

                return CreatedAtAction(nameof(GetSalesTrack), new { id = Doc.VersionId }, Doc);
            }
            catch (Exception ex)
            {
                // Restore the destination document from the backup
                System.IO.File.Copy(backupFilePath, destinationFilePath, true);
                throw; // Rethrow the exception after restoring the destination document
            }
            finally
            {
                // Remove the backup file
                System.IO.File.Delete(backupFilePath);
            }
        }

        private void ReplaceText(WordprocessingDocument document, string placeholder, string replacement)
        {
            var body = document.MainDocumentPart.Document.Body;

            foreach (var textElement in body.Descendants<Text>().Where(t => t.Text.Contains(placeholder)))
            {
                Console.WriteLine($"Found text: {textElement.Text}");

                // Replace the text within the Text element
                textElement.Text = textElement.Text.Replace(placeholder, replacement);

                Console.WriteLine($"Replaced with: {textElement.Text}");
            }
        }



        [HttpPut("EditSalesTrack")]
        public async Task<IActionResult> EditSalesTrack(int id, SalesTrack salesTrack)
        {
            if (id != salesTrack.Id)
            {
                return BadRequest();
            }

            _context.Entry(salesTrack).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SalesTrack.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpGet("GetSalesDashboard")]
        public ActionResult<DataSet> GetSalesDashboard(string FromDate, string ToDate, int salesPersonId = 0)
        {
            try
            {
                if (FromDate == "" || FromDate == null) { FromDate = DateTime.Now.ToString("yyyy-MM-dd"); }
                if (ToDate == "" || ToDate == null) { ToDate = DateTime.Now.ToString("yyyy-MM-dd"); }

                ToDate = ToDate + " 23:59:59";
                FromDate = FromDate + " 00:00:01";
                var result = _context.GetSalesDashboard(FromDate, ToDate, salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // DELETE: api/SalesTrack/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesTrack(int id)
        {
            var salesTrack = await _context.SalesTrack.FindAsync(id);
            if (salesTrack == null)
            {
                return NotFound();
            }

            _context.SalesTrack.Remove(salesTrack);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("SalesPersons")]
        public async Task<ActionResult<IEnumerable<SalesPerson>>> GetSalesPersons()
        {
            try
            {
                var salesPersons = await _context.SalesPerson.ToListAsync();
                return Ok(salesPersons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetSourceLead")]
        public async Task<ActionResult<IEnumerable<SourceId>>> GetSourceLead()
        {
            try
            {
                var sourceId = await _context.SourceId.ToListAsync();
                return Ok(sourceId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetSourcePerson")]
        public async Task<ActionResult<IEnumerable<SourcePerson>>> GetSourcePerson()
        {
            try
            {
                var sourcePerson = await _context.SourcePerson.ToListAsync();
                return Ok(sourcePerson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetVersion")]
        public async Task<ActionResult<IEnumerable<Version>>> GetVersion()
        {
            try
            {
                var version = await _context.Version.ToListAsync();
                return Ok(version);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetSalesPerson")]
        public async Task<ActionResult<IEnumerable<SalesPerson>>> GetSalesPerson()
        {
            try
            {
                var salesPerson = await _context.SalesPerson.ToListAsync();
                return Ok(salesPerson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetState")]
        public async Task<ActionResult<IEnumerable<StateId>>> GetState()
        {
            try
            {
                var stateId = await _context.StateId.ToListAsync();
                return Ok(stateId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("downloadDocs")]
        public IActionResult downloadDocs()
        {
            // Logic to read and send the DOCX file
            // Replace the following line with your actual file path and content type
            var fileBytes = System.IO.File.ReadAllBytes("TemplateFiles/ModifiedFiles/ModifiedDocument.Docx");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "yourfile.docx");
        }
    }
}
