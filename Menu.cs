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
    public partial class Menu : Form
    {
        private string connectionString;
        public Menu()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();

                // Comando SQL de consulta padrão (por nome)
                string sql = "SELECT codigo as 'Código', nome as 'Nome do produto', quantidade_estoque as 'Estoque', quantidade_minima as 'Quantidade Mínima', " +
                    "preco_custo as 'Preço de Custo', preco_venda as 'Valor de Venda' FROM produtos where quantidade_estoque < quantidade_minima";

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
                dataGridView1.AllowUserToAddRows = false;
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.ReadOnly = true;

                }
            }
        }

        private void cadastrosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CadastroCliente_Menu frm = new CadastroCliente_Menu();
            frm.Show();
        }

        private void entradaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void relatóriosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void produtosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CadastroProduto_Menu frm = new CadastroProduto_Menu();
            frm.Show();
        }

        private void fornecedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CadastroFornecedor_Menu frm = new CadastroFornecedor_Menu();
            frm.Show();
        }

        private void usuáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CadastroUsuario_Menu frm = new CadastroUsuario_Menu();
            frm.Show();
        }

        private void venderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CadastroVenda_Vender frm = new CadastroVenda_Vender();
            frm.Show();
        }

        private void consultarVendasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CadastroVenda_Consulta frm = new CadastroVenda_Consulta();
            frm.Show();
        }

        private void entradaDeNFeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Entrada_Menu frm = new Entrada_Menu();
            frm.Show();
        }

        private void lucratividadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relatorio frm = new Relatorio();
            frm.Show();
        }

        private void vendasPorPeríodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relatorio frm = new Relatorio();
            frm.Show();
        }

        private void clientesCadastradosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relatorio frm = new Relatorio();
            frm.Show();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();

                // Comando SQL de consulta padrão (por nome)
                string sql = "SELECT codigo as 'Código', nome as 'Nome do produto', quantidade_estoque as 'Estoque', quantidade_minima as 'Quantidade Mínima', " +
                    "preco_custo as 'Preço de Custo', preco_venda as 'Valor de Venda' FROM produtos where quantidade_estoque < quantidade_minima";

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
                dataGridView1.AllowUserToAddRows = false;
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.ReadOnly = true;

                }
            }
        }

        private void logsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logs frm = new Logs();
            frm.Show();
        }
    }
}
