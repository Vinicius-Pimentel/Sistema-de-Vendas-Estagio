using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace Sistema_de_Vendas
{
    public partial class RelatorioClientes : Form
    {
        private string connectionString;
        public RelatorioClientes()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dataInicio = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dataFim = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            DataTable dt = ObterDadosLucratividade(dataInicio, dataFim);

            if (dt != null && dt.Rows.Count > 0)
            {
                // Crie o arquivo Excel
                FileInfo fileInfo = new FileInfo("RelatorioClientes.xlsx");

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Relatório");

                    // Defina os nomes das colunas
                    for (int i = 1; i <= dt.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i].Value = dt.Columns[i - 1].ColumnName;
                    }

                    // Preencha os dados
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            if (dt.Columns[col].DataType == typeof(DateTime))
                            {
                                // Formate as colunas de datas adequadamente
                                worksheet.Cells[row + 2, col + 1].Value = Convert.ToDateTime(dt.Rows[row][col]);
                                worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "yyyy-MM-dd";
                            }
                            else
                            {
                                worksheet.Cells[row + 2, col + 1].Value = dt.Rows[row][col];
                            }
                        }
                    }

                    // Especifique o caminho completo para o arquivo Excel
                    string caminhoExcel = "C:/Users/Fartech/Desktop/RelatorioClientes.xlsx";

                    // Salve a planilha no arquivo
                    var excelFile = new FileInfo(caminhoExcel);
                    package.SaveAs(excelFile);
                }

                System.Diagnostics.Process.Start("RelatorioClientes.xlsx");
            }
            else
            {
                MessageBox.Show("Nenhum dado disponível para gerar o relatório de clientes.");
            }
        }

        private string ConverterDataParaFormatoBanco(string dataUsuario)
        {
            if (DateTime.TryParseExact(dataUsuario, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataConvertida))
            {
                return dataConvertida.ToString("yyyy-MM-dd");
            }

            return string.Empty;
        }

        private DataTable ObterDadosLucratividade(string dataInicio, string dataFim)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                    string query = "select * from clientes where data_cadastro between @dataInicio and @dataFim";

                    MySqlCommand cmd = new MySqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@dataInicio", dataInicio);
                    cmd.Parameters.AddWithValue("@dataFim", dataFim);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro ao obter dados de clientes: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
