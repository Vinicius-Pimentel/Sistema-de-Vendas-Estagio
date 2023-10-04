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
    public partial class CadastroUsuario_Menu : Form
    {
        private string connectionString;
        public CadastroUsuario_Menu()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CadastroUsuario_Cadastro frm = new CadastroUsuario_Cadastro();
            frm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void CadastroProduto_Menu_Load(object sender, EventArgs e)
        {
            // Preencha a DataGridView com os dados da tabela "produtos" do banco de dados
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM usuarios";
                MySqlCommand cmd = new MySqlCommand(sql, conexao);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Associe o DataTable à DataGridView
                dataGridView1.DataSource = dataTable;
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
                    string sql = "SELECT codigo as 'Código', nome as 'Nome do Usuário' from usuarios";

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
