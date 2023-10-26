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
    public partial class RelatorioVendas : Form
    {
        private string connectionString;
        public RelatorioVendas()
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
                string caminhoExcel = "C:/Users/Fartech/Desktop/RelatorioVendas.xlsx";

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Relatório");

                    // Defina o cabeçalho da planilha com os nomes das colunas
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dt.Columns[i].ColumnName;
                    }

                    // Preencha os dados
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].DataType == typeof(DateTime))
                            {
                                DateTime data = Convert.ToDateTime(dt.Rows[i][j]);
                                worksheet.Cells[i + 2, j + 1].Value = data.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                worksheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j];
                            }
                        }
                    }

                    // Salve a planilha no arquivo especificado
                    var excelFile = new FileInfo(caminhoExcel);
                    package.SaveAs(excelFile);

                    // Abra o arquivo Excel com o aplicativo padrão
                    System.Diagnostics.Process.Start(caminhoExcel);
                }
            }
            else
            {
                MessageBox.Show("Nenhum dado disponível para gerar o relatório.");
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
                    string query = "select * from vendas where data_venda between @dataInicio and @dataFim";

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
                    MessageBox.Show("Erro ao obter dados de vendas: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
