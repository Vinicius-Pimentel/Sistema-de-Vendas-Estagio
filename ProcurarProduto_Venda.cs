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
using Sistema_de_Vendas;

namespace Sistema_de_Vendas
{
    public partial class ProcurarProduto_Venda : Form
    {
        public event Action<string, string> ProdutoSelecionado;
        public event Action<string, string, decimal> ProdutoSelecionadoVenda;
        private string connectionString;
        public ProcurarProduto_Venda()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
            InitializeDataGridView();
        }

        
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Define a coluna "MinhaColuna" como somente leitura
            // dataGridView1.Columns["MinhaColuna"].ReadOnly = true;

            // Obtenha o código do produto selecionado
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                string codigoProduto = dataGridView1.Rows[e.RowIndex].Cells["Código"].Value.ToString();

                // Verifique se o código do produto já foi adicionado globalmente em qualquer tela
                if (Entrada_Cadastro.GlobalData.CodigosProdutosAdicionados.Contains(codigoProduto))
                {
                    MessageBox.Show("Este produto já foi adicionado.");
                    return; // Não adicione o produto duplicado
                }

                // Adicione o produto globalmente
                Entrada_Cadastro.GlobalData.CodigosProdutosAdicionados.Add(codigoProduto);

                string nomeProduto = dataGridView1.Rows[e.RowIndex].Cells["Nome do produto"].Value.ToString();
                decimal valorUnitario = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["Valor de Venda"].Value);

                // Dispare o evento para notificar o formulário principal
                ProdutoSelecionado?.Invoke(codigoProduto, nomeProduto);
                ProdutoSelecionadoVenda?.Invoke(codigoProduto, nomeProduto, valorUnitario);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Comando SQL de consulta padrão (por nome)
                    string sql = "SELECT codigo as 'Código', nome as 'Nome do produto', quantidade_estoque as 'Estoque', quantidade_minima as 'Quantidade Mínima', " +
                        "preco_custo as 'Preço de Custo', preco_venda as 'Valor de Venda' FROM produtos";

                    string textoPesquisa = textBox1.Text;

                    // Verifique se o texto de pesquisa é um número
                    if (int.TryParse(textoPesquisa, out _))
                    {
                        // Se for um número, pesquise por código
                        sql += $" WHERE codigo = {textoPesquisa}";
                    }
                    else
                    {
                        // Caso contrário, pesquise por nome
                        sql += $" WHERE nome LIKE '%{textoPesquisa}%'";
                    }

                    // Crie um objeto MySqlCommand
                    MySqlCommand cmd = new MySqlCommand(sql, conexao);

                    // Crie um adaptador MySQL para preencher o DataGridView
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    // Crie um DataTable para armazenar os resultados da consulta
                    DataTable dt = new DataTable();

                    // Preencha o DataTable com os resultados da consulta
                    adapter.Fill(dt);

                    // Associe o DataTable ao DataGridView que já existe no formulário
                    dataGridView1.DataSource = dt;
                }
                catch (MySqlException ex)
                {
                    // Lida com exceções de conexão ou comando SQL
                    MessageBox.Show("Erro de conexão: " + ex.Message);
                }
            }
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

    }
}
