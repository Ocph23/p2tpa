using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Main.DataAccess
{
    public class ExcelContext : IDisposable
    {
       public SpreadsheetDocument document { get; set; }

        public DataRows GetRange(string sheetName, string range)
        {
            string start = range.Split(':')[0];
            string end = range.Split(':')[1];

            var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();
            if (sheet==null)
            {
                // The specified worksheet does not exist.
                return null;
            }

            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
            IEnumerable<Row> rows = from sd in worksheetPart.Worksheet.Elements<SheetData>()
                                           from r in sd.Elements<Row>()
                                           select r;
            SharedStringTablePart shareStringPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();

            return new DataRows(shareStringPart, rows.Take((int)GetRowIndex(end)));

        }

        // Given a cell name, parses the specified cell to get the row index.
        private uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }

        public ExcelContext(string fileName)
        {
            document = SpreadsheetDocument.Open(fileName, false);
        }



        public void Dispose()
        {
            document.Close();
        }

        // Given a cell name, parses the specified cell to get the column name.
      
    }


    public class DataRows
    {
        private SharedStringTablePart shareStringPart;
        private IEnumerable<Row> rows;
        public int Count { get { return rows.Count(); } }

        public DataRows(SharedStringTablePart shareStringPart, IEnumerable<Row> enumerable)
        {
            this.shareStringPart = shareStringPart;
            this.rows = enumerable;
        }

       public string Cell(int row, string col)
        {

            string cellName = $"{col}{row}";
            var itemRow = rows.Where(x => x.RowIndex == row).FirstOrDefault();
            if (itemRow != null)
            {
                var cells = itemRow.Elements<Cell>().Where(c => string.Compare(GetColumnName(c.CellReference.Value), GetColumnName(cellName), true) == 0
                && GetRowIndex(c.CellReference.Value) == GetRowIndex(cellName))
                .OrderBy(r => GetRowIndex(r.CellReference));

                Cell headCell = cells.First();

                // If the content of the first cell is stored as a shared string, get the text of the first cell
                // from the SharedStringTablePart and return it. Otherwise, return the string value of the cell.
                if (headCell.DataType != null && headCell.DataType.Value == CellValues.SharedString)
                {
                    SharedStringItem[] items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();
                    return items[int.Parse(headCell.CellValue.Text)].InnerText;
                }
                else
                {
                    return headCell.CellValue != null ? headCell.CellValue.Text : null;
                }
            }
            return null;
        }

        private string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }

        // Given a cell name, parses the specified cell to get the row index.
        private uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }
    }




}
