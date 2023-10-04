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

namespace Sistema_de_Vendas
{
    public partial class CadastroProduto_Entrada : Form
    {
        private string connectionString;
        public CadastroProduto_Entrada()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                    // Comando SQL de inserção
                    string sql = "INSERT INTO produtos (codigo, nome, quantidade_estoque, quantidade_minima, preco_custo, preco_venda) VALUES (@codigo, @nome, @quantidade, @quantidade_m, @preco_c, @preco_v)";

                    // Crie um objeto MySqlCommand
                    MySqlCommand cmd = new MySqlCommand(sql, conexao);

                    // Defina os parâmetros
                    cmd.Parameters.AddWithValue("@codigo", pdt_codigo.Text);
                    cmd.Parameters.AddWithValue("@nome", pdt_cliente.Text); // Substitua pelo nome do produto
                    cmd.Parameters.AddWithValue("@quantidade", pdt_estoque.Text); // Substitua pelo preço do produto
                    if (string.IsNullOrWhiteSpace(pdt_qtd_minima.Text) || pdt_qtd_minima.Text == "0")
                    {
                        cmd.Parameters.AddWithValue("@quantidade_m", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@quantidade_m", pdt_qtd_minima.Text);
                    }
                    cmd.Parameters.AddWithValue("@preco_c", pdt_preco_c.Text);
                    cmd.Parameters.AddWithValue("@preco_v", pdt_preco_v.Text);

                    // Execute o comando de inserção
                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        MessageBox.Show("Inserção bem-sucedida.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma linha foi inserida.");
                    }
                }
                catch (MySqlException ex)
                {
                    // Lida com exceções de conexão ou comando SQL
                    MessageBox.Show("Erro de conexão: " + ex.Message);
                }
                // A partir daqui, você pode executar consultas, comandos SQL, etc.

            }

        }
    }
}
