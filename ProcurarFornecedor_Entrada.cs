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
    public partial class ProcurarFornecedor_Entrada : Form
    {
        // Define o delegate para o evento do cliente selecionado
        public delegate void FornecedorSelecionadoHandler(string nomeFornecedor);

        // Evento acionado quando um cliente é selecionado
        public event FornecedorSelecionadoHandler FornecedorSelecionado;
        private string connectionString;
        public ProcurarFornecedor_Entrada()
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
                    string sql = "SELECT codigo as 'Código', nome as 'Nome do Fornecedor', cnpj as 'CNPJ', endereco as 'Endereço', " +
                        "cidade as 'Cidade', estado as 'estado', telefone as 'Telefone', email as 'E-mail' FROM fornecedores";

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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se há uma célula selecionada
            if (e.RowIndex >= 0)
            {
                // Obtém o nome do cliente da célula selecionada
                string nomeFornecedor = dataGridView1.Rows[e.RowIndex].Cells["Nome do Fornecedor"].Value.ToString();

                // Aciona o evento de cliente selecionado e transmite o nome do cliente
                FornecedorSelecionado?.Invoke(nomeFornecedor);
            }
            this.Close();
        }
    }
}
