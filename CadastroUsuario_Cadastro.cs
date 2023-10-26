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
    public partial class CadastroUsuario_Cadastro : Form
    {
        private string connectionString;
        public CadastroUsuario_Cadastro()
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
                    string sql = "INSERT INTO usuarios (nome, senha) VALUES (@nome, @senha)";

                    // Crie um objeto MySqlCommand
                    MySqlCommand cmd = new MySqlCommand(sql, conexao);

                    // Defina os parâmetros
                    
                    if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
                    {
                        MessageBox.Show("É preciso preencher os dois campos para cadastrar o usuário.");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nome", textBox2.Text); // Substitua pelo nome do produto
                        cmd.Parameters.AddWithValue("@senha", textBox3.Text); // Substitua pelo preço do produto
                    }

                    // Execute o comando de inserção
                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        MessageBox.Show("Inserção bem-sucedida.");
                        string sqlLog = "INSERT INTO logs (antes, depois, tabela_modificada, data_alteracao, usuario_alterou, tipo) VALUES (@antes, @depois, @tabela_modificada, @data_alteracao, @usuario_alterou, @tipo)";

                        // Crie um objeto MySqlCommand
                        MySqlCommand cmdLog = new MySqlCommand(sqlLog, conexao);


                        string tabela = "usuarios";
                        string sqlConsulta = "SELECT nome FROM login_logs ORDER BY data_login DESC LIMIT 1";
                        MySqlCommand cmdConsulta = new MySqlCommand(sqlConsulta, conexao);
                        cmdConsulta.ExecuteNonQuery();
                        string nomeUsuario = cmdConsulta.ExecuteScalar() as string;
                        string efeitoPlacebo = "null";
                        string tipo = "Cadastro";

                        // Defina os parâmetros
                        cmdLog.Parameters.AddWithValue("@antes", efeitoPlacebo); // Substitua pelo nome do produto
                        cmdLog.Parameters.AddWithValue("@depois", textBox2.Text); // Substitua pelo preço do produto
                        cmdLog.Parameters.AddWithValue("@tabela_modificada", tabela); // Substitua pelo preço do produto
                        cmdLog.Parameters.AddWithValue("@data_alteracao", DateTime.Now);
                        cmdLog.Parameters.AddWithValue("@usuario_alterou", nomeUsuario); // Substitua pelo preço do produto
                        cmdLog.Parameters.AddWithValue("@tipo", tipo);
                        cmdLog.ExecuteNonQuery();
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
    }
}
