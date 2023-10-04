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
    public partial class Login : Form
    {
        private string connectionString;
        // Variável para armazenar o nome do usuário logado
        public string NomeUsuarioLogado { get; private set; }
        public Login()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
            // Impede o redimensionamento da janela
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Defina a cor de fundo para a tela de login
            this.BackColor = Color.FromArgb(45, 45, 48);

            // Defina a cor de fundo para os campos de texto
            textBoxUsuario.BackColor = Color.FromArgb(63, 63, 70);
            textBoxSenha.BackColor = Color.FromArgb(63, 63, 70);

            // Defina a cor do texto para os campos de texto
            textBoxUsuario.ForeColor = Color.White;
            textBoxSenha.ForeColor = Color.White;

            // Defina a fonte para os campos de texto
            textBoxUsuario.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            textBoxSenha.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            // Defina a cor de fundo para o botão de login
            button1.BackColor = Color.FromArgb(0, 123, 255);

            // Defina a cor do texto para o botão de login
            button1.ForeColor = Color.White;

            // Defina a fonte para o botão de login
            button1.Font = new Font("Segoe UI", 14, FontStyle.Bold);

            // Centralize os campos de texto e o botão
            textBoxUsuario.TextAlign = HorizontalAlignment.Center;
            textBoxSenha.TextAlign = HorizontalAlignment.Center;
            button1.TextAlign = ContentAlignment.MiddleCenter;

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBoxUsuario.Text;
            string senha = textBoxSenha.Text;

            if (ValidarCredenciais(usuario, senha))
            {
                // Credenciais válidas, armazene o nome do usuário
                NomeUsuarioLogado = usuario;

                // Abra a tela do sistema
                Menu menuForm = new Menu();
                this.Hide();
                menuForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciais incorretas. Tente novamente.");
            }
        }

        private bool ValidarCredenciais(string usuario, string senha)
        {
            try
            {
                using (MySqlConnection conexao = new MySqlConnection(connectionString))
                {
                    conexao.Open();

                    string query = "SELECT COUNT(*) FROM usuarios WHERE nome = @usuario AND senha = @senha";
                    MySqlCommand cmd = new MySqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@senha", senha);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    return count > 0;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro ao validar credenciais: " + ex.Message);
                return false;
            }
        }
    }
}
