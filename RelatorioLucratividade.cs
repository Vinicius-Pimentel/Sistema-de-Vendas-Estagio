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
    public partial class RelatorioLucratividade : Form
    {
        private string connectionString;
        public RelatorioLucratividade()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Converter as datas para o formato do banco de dados
            string dataInicio = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dataFim = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            // Realizar a consulta SQL com as datas no formato correto
            DataTable dt = ObterDadosLucratividade(dataInicio, dataFim);

            if (dt != null && dt.Rows.Count > 0)
            {
                // Crie uma nova planilha Excel
                using (ExcelPackage package = new ExcelPackage())
                {
                    // Adicione uma nova planilha
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("RelatorioLucratividade");

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
                            worksheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j];
                        }
                    }

                    // Especifique o caminho completo para o arquivo Excel
                    string caminhoExcel = "C:/Users/Fartech/Desktop/RelatorioLucratividade.xlsx";

                    // Salve a planilha no arquivo
                    var excelFile = new FileInfo(caminhoExcel);
                    package.SaveAs(excelFile);

                    // Abra o arquivo Excel com o aplicativo padrão
                    System.Diagnostics.Process.Start(caminhoExcel);
                }
            }
            else
            {
                MessageBox.Show("Nenhum dado disponível para gerar o relatório de lucratividade.");
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
                    string query = @"
SELECT
    p.codigo AS 'Código do Produto',
    p.nome AS 'Nome do Produto',
    SUM(p.preco_custo) AS 'Preço de Custo Total',
    SUM(vp.valor_produto) AS 'Total de Vendas',
    FORMAT(((SUM(vp.valor_produto) - SUM(p.preco_custo)) / SUM(p.preco_custo)) * 100, 2) AS 'Lucratividade (%)'
FROM
    produtos p
LEFT JOIN
    (
        SELECT
            vp.codigo_produto,
            vp.valor_produto
        FROM
            vendas_produtos vp
        LEFT JOIN
            vendas v ON vp.codigo_venda = v.codigo
        WHERE
            v.data_venda BETWEEN @dataInicio AND @dataFim
    ) vp ON p.codigo = vp.codigo_produto
GROUP BY
    p.codigo, p.nome";

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
                    MessageBox.Show("Erro ao obter dados de lucratividade: " + ex.Message);
                    return null;
                }
            }
        }
    }
}