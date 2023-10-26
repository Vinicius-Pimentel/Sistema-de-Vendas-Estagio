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
    public partial class Logs : Form
    {
        private string connectionString;
        public Logs()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
        }

        private void Logs_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                // Todas as ComboBox estão vazias, execute uma consulta abrangente em todas as logs
                ConsultaAbrangenteLogs();
            }
            else
            {
                // Execute uma consulta com base nas seleções nas ComboBox
                ConsultaEspecificaLogs();
            }
        }

        private void ConsultaAbrangenteLogs()
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Comando SQL para obter todos os logs
                    string query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs";

                    MySqlCommand cmd = new MySqlCommand(query, conexao);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Preencher a DataGridView1 com os resultados da consulta
                    dataGridView1.DataSource = dt;
                }
                catch (MySqlException ex)
                {
                    // Lidar com exceções, como problemas de conexão
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }

        private void ConsultaEspecificaLogs()
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                    string query = "";

                    if (comboBox1.SelectedItem != null)
                    {
                        string tabelaSelecionada1 = comboBox1.SelectedItem.ToString();
                        string tabelaSelecionada2 = comboBox2.SelectedItem != null ? comboBox2.SelectedItem.ToString() : null;

                        if (tabelaSelecionada1 == "Usuários")
                        {
                            query = "select * from login_logs";
                        }
                        else if (tabelaSelecionada1 == "Produtos")
                            if (tabelaSelecionada2 == "Cadastro")
                                {
                                    query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'produtos' and tipo = 'Cadastro'";
                                }                         
                            else if (tabelaSelecionada2 == "Alteração")
                                {
                                    query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'produtos' and tipo = 'Alteração'";
                                }
                            else if (tabelaSelecionada2 == "Exclusão")
                                {
                                    query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'produtos' and tipo = 'Exclusão'";
                                }
                            else
                                {
                                    query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'produtos'";
                                }
                        else if (tabelaSelecionada1 == "Clientes")
                            if (tabelaSelecionada2 == "Cadastro")
                            {
                                query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'clientes' and tipo = 'Cadastro'";
                            }
                            else if (tabelaSelecionada2 == "Alteração")
                            {
                                query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'clientes' and tipo = 'Alteração'";
                            }
                            else if (tabelaSelecionada2 == "Exclusão")
                            {
                                query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'clientes' and tipo = 'Exclusão'";
                            }
                            else
                            {
                                query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'clientes'";
                            }
                        else if (tabelaSelecionada1 == "Fornecedores")
                            if (tabelaSelecionada2 == "Cadastro")
                            {
                                query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'fornecedores' and tipo = 'Cadastro'";
                            }
                            else if (tabelaSelecionada2 == "Alteração")
                            {
                                query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'fornecedores' and tipo = 'Alteração'";
                            }
                            else if (tabelaSelecionada2 == "Exclusão")
                            {
                                query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'fornecedores' and tipo = 'Exclusão'";
                            }
                            else
                            {
                                query = "SELECT antes as 'Antes da Modificação', depois as 'Depois da Modificação', tabela_modificada as 'Tabela da Alteração', data_alteracao as 'Data da Ação', usuario_alterou as 'Usuário', tipo as 'Tipo de Ação' FROM logs where tabela_modificada = 'fornecedores'";
                            }
                        // Outros casos para tabelas específicas podem ser adicionados aqui
                    }

                    if (query != "")
                    {
                        MySqlCommand cmd = new MySqlCommand(query, conexao);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Selecione uma tabela válida na ComboBox1.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }
    }
}
