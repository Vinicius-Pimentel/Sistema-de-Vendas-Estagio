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
    public partial class CadastroCliente_Excluir : Form
    {
        private string connectionString;
        public CadastroCliente_Excluir()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Obtemos o código do cliente a partir do TextBox
            int codigoClienteParaExcluir = ObterCodigoClienteParaExcluir();

            if (codigoClienteParaExcluir != -1)
            {
                // A função retornou um código de cliente válido
                // Agora podemos chamar a função para excluir o cliente
                ExcluirCliente(codigoClienteParaExcluir);
            }
        }

        private int ObterCodigoClienteParaExcluir()
        {
            int codigoClienteParaExcluir;

            if (int.TryParse(textBox1.Text, out codigoClienteParaExcluir))
            {
                // A conversão foi bem-sucedida, você tem o código do cliente
                return codigoClienteParaExcluir;
            }
            else
            {
                MessageBox.Show("Por favor, insira um código de cliente válido.");
                return -1;  // Retorna -1 se não for possível obter um código válido
            }
        }

        private void ExcluirCliente(int codigoCliente)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Comando SQL para excluir o cliente com o código fornecido
                    string query = "DELETE FROM clientes WHERE codigo = @codigo";

                    MySqlCommand cmd = new MySqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@codigo", codigoCliente);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cliente excluído com sucesso.");
                        string sqlLog = "INSERT INTO logs (antes, depois, tabela_modificada, data_alteracao, usuario_alterou, tipo) VALUES (@antes, @depois, @tabela_modificada, @data_alteracao, @usuario_alterou, @tipo)";

                        // Crie um objeto MySqlCommand
                        MySqlCommand cmdLog = new MySqlCommand(sqlLog, conexao);


                        string tabela = "clientes";
                        string sqlConsulta = "SELECT nome FROM login_logs ORDER BY data_login DESC LIMIT 1";
                        MySqlCommand cmdConsulta = new MySqlCommand(sqlConsulta, conexao);
                        cmdConsulta.ExecuteNonQuery();
                        string nomeUsuario = cmdConsulta.ExecuteScalar() as string;
                        string efeitoPlacebo = "null";
                        string tipo = "Exclusão";

                        // Defina os parâmetros
                        cmdLog.Parameters.AddWithValue("@antes", efeitoPlacebo); // Substitua pelo nome do produto
                        cmdLog.Parameters.AddWithValue("@depois", textBox1.Text); // Substitua pelo preço do produto
                        cmdLog.Parameters.AddWithValue("@tabela_modificada", tabela); // Substitua pelo preço do produto
                        cmdLog.Parameters.AddWithValue("@data_alteracao", DateTime.Now);
                        cmdLog.Parameters.AddWithValue("@usuario_alterou", nomeUsuario); // Substitua pelo preço do produto
                        cmdLog.Parameters.AddWithValue("@tipo", tipo);
                        cmdLog.ExecuteNonQuery();
                        this.Close();
                    }
                    else
                        MessageBox.Show("Cliente não encontrado com o código fornecido.");
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro ao excluir cliente: " + ex.Message);
                }
            }
        }
    }
}
