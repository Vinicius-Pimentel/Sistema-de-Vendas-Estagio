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
    public partial class CadastroCliente_Cadastro : Form
    {
        private string connectionString;
        public CadastroCliente_Cadastro()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                // Verifique se o controle é um TextBox ou uma ComboBox
                if (control is TextBox)
                {
                    // Limpe o conteúdo do TextBox
                    ((TextBox)control).Text = string.Empty;
                }
                else if (control is ComboBox)
                {
                    // Limpe o conteúdo da ComboBox
                    ((ComboBox)control).SelectedItem = null;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                    // Comando SQL de inserção
                    string sql = "INSERT INTO clientes (nome, cpf, endereco, cidade, estado, telefone, email, data_cadastro) VALUES (@nome, @cpf, @endereco, @cidade, @estado, @telefone, @email, @data_cadastro)";

                    // Crie um objeto MySqlCommand
                    MySqlCommand cmd = new MySqlCommand(sql, conexao);

                    // Defina os parâmetros
                    cmd.Parameters.AddWithValue("@nome", textBox2.Text); // Substitua pelo nome do produto
                    cmd.Parameters.AddWithValue("@cpf", textBox3.Text); // Substitua pelo preço do produto
                    cmd.Parameters.AddWithValue("@data_cadastro", DateTime.Now);
                    cmd.Parameters.AddWithValue("@endereco", textBox4.Text);
                    cmd.Parameters.AddWithValue("@cidade", textBox5.Text);
                    cmd.Parameters.AddWithValue("@estado", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@telefone", textBox7.Text);
                    cmd.Parameters.AddWithValue("@email", textBox8.Text);

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
            }
        }
    }
}
