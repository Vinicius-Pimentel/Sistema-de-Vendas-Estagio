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
    public partial class CadastroProduto_Cadastro : Form
    {
        private string connectionString;
        public string CodigoProduto { get; set; }
        public CadastroProduto_Cadastro()
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
                    if (!int.TryParse(pdt_estoque.Text, out _))
                    {
                        MessageBox.Show("Estoque deve ser um número inteiro válido.");
                        return; // Saia da função
                    }

                    // Verifique se o campo de quantidade mínima é um número
                    if (!int.TryParse(pdt_qtd_minima.Text, out _))
                    {
                        MessageBox.Show("Quantidade mínima deve ser um número inteiro válido.");
                        return; // Saia da função
                    }

                    // Verifique se o campo de preço de custo é um número de ponto flutuante
                    if (!float.TryParse(pdt_preco_c.Text, out _))
                    {
                        MessageBox.Show("Preço de custo deve ser um número de ponto flutuante válido.");
                        return; // Saia da função
                    }

                    // Verifique se o campo de preço de venda é um número de ponto flutuante
                    if (!float.TryParse(pdt_preco_v.Text, out _))
                    {
                        MessageBox.Show("Preço de venda deve ser um número de ponto flutuante válido.");
                        return; // Saia da função
                    }
                    if (string.IsNullOrWhiteSpace(pdt_cliente.Text) ||
                    string.IsNullOrWhiteSpace(pdt_estoque.Text) ||
                    string.IsNullOrWhiteSpace(pdt_preco_c.Text) ||
                    string.IsNullOrWhiteSpace(pdt_preco_v.Text))
                    {
                        MessageBox.Show("Certifique-se de preencher todos os campos obrigatórios.");
                        return;
                    }
                    conexao.Open();
                    // Comando SQL de inserção
                    string sql = "INSERT INTO produtos (nome, quantidade_estoque, quantidade_minima, preco_custo, preco_venda, data_cadastro) VALUES (@nome, @quantidade, @quantidade_m, @preco_c, @preco_v, @data_cadastro)";

                    // Crie um objeto MySqlCommand
                    MySqlCommand cmd = new MySqlCommand(sql, conexao);

                    // Defina os parâmetros
                    cmd.Parameters.AddWithValue("@nome", pdt_cliente.Text); // Substitua pelo nome do produto
                    cmd.Parameters.AddWithValue("@quantidade", pdt_estoque.Text); // Substitua pelo preço do produto
                    cmd.Parameters.AddWithValue("@data_cadastro", DateTime.Now);
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                // Verifique se o controle é um TextBox
                if (control is TextBox)
                {
                    // Limpe o conteúdo do TextBox
                    ((TextBox)control).Text = string.Empty;
                }
            }
        }
    }
}
