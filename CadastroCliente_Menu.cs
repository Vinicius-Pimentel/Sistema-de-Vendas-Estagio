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
    public partial class CadastroCliente_Menu : Form
    {
        private string connectionString;
        public CadastroCliente_Menu()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CadastroCliente_Cadastro frm = new CadastroCliente_Cadastro();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Comando SQL de consulta padrão (por nome)
                    string sql = "SELECT codigo as 'Código', nome as 'Nome do Cliente', cpf as 'CPF', endereco as 'Endereço', " +
                        "cidade as 'Cidade', estado as 'estado', telefone as 'Telefone', email as 'E-mail' FROM clientes";

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

        private void AtualizarValorNoBancoDeDados(object primaryKeyValue, int columnIndex, object editedValue)
        {
            // Use a chave primária (primaryKeyValue), a coluna (columnIndex) e o novo valor (editedValue)
            // para atualizar o registro correspondente no banco de dados
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Construa um comando SQL UPDATE com base nos parâmetros
                    string updateSql = "UPDATE clientes SET ";
                    string coluna = dataGridView1.Columns[columnIndex].Name;

                    // Ajuste o nome da coluna com base no seu design da tabela e na estrutura do banco de dados
                    // Você deve mapear os nomes das colunas na DataGridView para os nomes corretos no banco de dados
                    // No exemplo abaixo, usamos "NomeDaColunaNoBanco" como o nome da coluna no banco de dados
                    switch (coluna)
                    {
                        case "Código":
                            updateSql += "codigo = @editedValue";
                            break;
                        case "Nome do Cliente":
                            updateSql += "nome = @editedValue";
                            break;
                        case "CPF":
                            updateSql += "cpf = @editedValue";
                            break;
                        case "Endereço":
                            updateSql += "endereco = @editedValue";
                            break;
                        case "Cidade":
                            updateSql += "cidade = @editedValue";
                            break;
                        case "Estado":
                            updateSql += "estado = @editedValue";
                            break;
                        case "Telefone":
                            updateSql += "telefone = @editedValue";
                            break;
                        case "E-mail":
                            updateSql += "email = @editedValue";
                            break;
                            // Adicione outros casos para outras colunas, se necessário
                    }

                    // Adicione a cláusula WHERE com base na chave primária
                    updateSql += " WHERE codigo = @primaryKeyValue";

                    // Crie um comando MySqlCommand com a consulta SQL
                    MySqlCommand cmd = new MySqlCommand(updateSql, conexao);
                    cmd.Parameters.AddWithValue("@editedValue", editedValue);
                    cmd.Parameters.AddWithValue("@primaryKeyValue", primaryKeyValue);

                    // Execute o comando SQL UPDATE
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Sucesso!");

                    string sqlLog = "INSERT INTO logs (antes, depois, tabela_modificada, data_alteracao, usuario_alterou, tipo) VALUES (@antes, @depois, @tabela_modificada, @data_alteracao, @usuario_alterou, @tipo)";

                    // Crie um objeto MySqlCommand
                    MySqlCommand cmdLog = new MySqlCommand(sqlLog, conexao);


                    string tabela = "clientes";
                    string sqlConsulta = "SELECT nome FROM login_logs ORDER BY data_login DESC LIMIT 1";
                    MySqlCommand cmdConsulta = new MySqlCommand(sqlConsulta, conexao);
                    cmdConsulta.ExecuteNonQuery();
                    string nomeUsuario = cmdConsulta.ExecuteScalar() as string;
                    string efeitoPlacebo = "null";
                    string tipo = "Alteração";

                    // Defina os parâmetros
                    cmdLog.Parameters.AddWithValue("@antes", efeitoPlacebo); // Substitua pelo nome do produto
                    cmdLog.Parameters.AddWithValue("@depois", editedValue); // Substitua pelo preço do produto
                    cmdLog.Parameters.AddWithValue("@tabela_modificada", tabela); // Substitua pelo preço do produto
                    cmdLog.Parameters.AddWithValue("@data_alteracao", DateTime.Now);
                    cmdLog.Parameters.AddWithValue("@usuario_alterou", nomeUsuario); // Substitua pelo preço do produto
                    cmdLog.Parameters.AddWithValue("@tipo", tipo);
                    cmdLog.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Lida com exceções de conexão ou comando SQL
                    MessageBox.Show("Erro ao atualizar valor: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            {
                // Obtém a linha e a coluna em que a edição ocorreu
                int rowIndex = e.RowIndex;
                int columnIndex = e.ColumnIndex;

                // Obtém o valor editado
                object editedValue = dataGridView1.Rows[rowIndex].Cells[columnIndex].Value;

                // Obtém o valor da coluna que representa a chave primária (por exemplo, "Código")
                object primaryKeyValue = dataGridView1.Rows[rowIndex].Cells["Código"].Value;

                // Atualize o valor no banco de dados
                AtualizarValorNoBancoDeDados(primaryKeyValue, columnIndex, editedValue);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CadastroCliente_Excluir frm = new CadastroCliente_Excluir();
            frm.Show();
        }
    }
}
