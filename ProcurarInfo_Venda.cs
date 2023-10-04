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
    
    public partial class ProcurarInfo_Venda : Form
    {
        private string connectionString;
        public ProcurarInfo_Venda()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Comando SQL de consulta padrão (por nome)
                    string sql = "SELECT codigo_venda as 'Código da venda', nome_produto as 'Nome do produto', valor_produto as 'Valor total do produto', ROUND(valor_produto/quantidade, 2) as 'Valor unitário do produto', quantidade as 'Quantidade vendida' FROM vendas_produtos";

                    string textoPesquisa = textBox1.Text;

                    // Verifique se o texto de pesquisa é um número
                    if (int.TryParse(textoPesquisa, out _))
                    {
                        // Se for um número, pesquise por código
                        sql += $" WHERE codigo_venda = {textoPesquisa}";
                    }
                    else
                    {
                        // Caso contrário, pesquise por nome
                        sql += $" WHERE nome_produto LIKE '%{textoPesquisa}%'";
                    }

                    // Crie um objeto MySqlCommand
                    MySqlCommand cmd = new MySqlCommand(sql, conexao);

                    // Crie um adaptador MySQL para preencher o DataGridView
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    // Crie um DataTable para armazenar os resultados da consulta
                    DataTable dt = new DataTable();

                    // Preencha o DataTable com os resultados da consulta
                    adapter.Fill(dt);

                    // Associe o DataTable ao DataGridView
                    dataGridView1.DataSource = dt;
                }
                catch (MySqlException ex)
                {
                    // Lida com exceções de conexão ou comando SQL
                    MessageBox.Show("Erro de conexão: " + ex.Message);
                }
            }
        }
    }
}
