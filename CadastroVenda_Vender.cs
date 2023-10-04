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
using System.Globalization;

namespace Sistema_de_Vendas
{
    public partial class CadastroVenda_Vender : Form
    {
        private string connectionString;
        public CadastroVenda_Vender()
        {
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
            InitializeComponent();
            textBox4.Text = DateTime.Now.ToString("dd/MM/yyyy");
            textBox2.Text = "0";
        }

        private void CadastroVenda_Vender_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Limpe a lista quando a tela estiver sendo fechada
            Entrada_Cadastro.GlobalData.CodigosProdutosAdicionados.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcurarProduto_Venda frm = new ProcurarProduto_Venda();
            frm.Show();
            // Registre um manipulador de eventos para o evento ProdutoSelecionado
            frm.ProdutoSelecionadoVenda += (codigoProduto, nomeProduto, valorUnitario) =>
            {
                // Adicione o produto à tela de entrada
                ExibirProdutoNaTelaProcurar(codigoProduto, nomeProduto, valorUnitario);
            };

            frm.Show();
        }

        private void ExibirProdutoNaTelaProcurar(string codigoProduto, string nomeProduto, decimal valorUnitario)
        {

            int rowIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[rowIndex].Cells["Codigo"].Value = codigoProduto;
            dataGridView1.Rows[rowIndex].Cells["Nome"].Value = nomeProduto; // Adicione uma coluna "Nome" na DataGridView
            dataGridView1.Rows[rowIndex].Cells["Valor_Unitario"].Value = valorUnitario;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Quantidade"].Index && e.RowIndex >= 0)
            {
                // Obtenha a quantidade e o valor unitário da célula editada
                int quantidade = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Quantidade"].Value);

                // Certifique-se de que o valor unitário é interpretado corretamente como decimal
                decimal valorUnitario = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["Valor_Unitario"].Value.ToString(), CultureInfo.InvariantCulture);

                // Calcule o valor total para essa linha
                decimal valorTotal = quantidade * valorUnitario;

                // Atualize a célula "Valor_Total" para exibir o valor total
                dataGridView1.Rows[e.RowIndex].Cells["Valor_Total"].Value = valorTotal;

                // Atualize o textBox5 com a soma dos valores totais de todas as linhas
                decimal somaValoresTotais = CalcularSomaValoresTotais();
                try
                {
                    textBox5.Text = (somaValoresTotais - decimal.Parse(textBox2.Text)).ToString();
                }
                catch
                {
                    MessageBox.Show("O desconto deve ser colocado antes dos produtos serem selecionados.");
                }
            }
        }

        private decimal CalcularSomaValoresTotais()
        {
            decimal somaValoresTotais = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Valor_Total"].Value != null)
                {
                    decimal valorTotal = Convert.ToDecimal(row.Cells["Valor_Total"].Value);
                    somaValoresTotais += valorTotal;
                }
            }

            return somaValoresTotais;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Verifique se os campos estão vazios
            if (string.IsNullOrWhiteSpace(textBox6.Text) ||
                comboBox1.SelectedItem == null ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Preencha todos os campos antes de finalizar a venda.");
                return;  // Não permita a finalização da venda
            }
            // Obter informações necessárias para a venda
            string dataVenda = DateTime.Now.ToString("yyyy-MM-dd");  // Formato aceito pelo MySQL
            string nomeCliente = textBox6.Text;  // Supondo que o nome do cliente está no textBox1
            decimal valorTotalVenda = decimal.Parse(textBox5.Text);  // Valor total da venda
            string tipoTransporte = comboBox1.Text;  // Supondo que o tipo de transporte está em um ComboBox chamado comboBoxTipoTransporte

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command;

            try
            {
                connection.Open();

                // Inserir dados na tabela 'vendas'
                string queryVendas = "INSERT INTO vendas (data_venda, nome_cliente, valor_total_venda, tipo_transporte) " +
                                     "VALUES (@dataVenda, @nomeCliente, @valorTotalVenda, @tipoTransporte)";
                command = new MySqlCommand(queryVendas, connection);
                command.Parameters.AddWithValue("@dataVenda", dataVenda);
                command.Parameters.AddWithValue("@nomeCliente", nomeCliente);
                command.Parameters.AddWithValue("@valorTotalVenda", valorTotalVenda);
                command.Parameters.AddWithValue("@tipoTransporte", tipoTransporte);
                command.ExecuteNonQuery();  // Executa a inserção na tabela vendas

                // Obter o código da venda inserida
                int codigoVenda = (int)command.LastInsertedId;

                // Inserir dados na tabela 'vendas_produtos' para cada produto na DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string codigoProduto = row.Cells["Codigo"].Value.ToString();
                    string nomeProduto = row.Cells["Nome"].Value.ToString();

                    decimal valorProduto = Convert.ToDecimal(row.Cells["Valor_Total"].Value);
                    int quantidade = Convert.ToInt32(row.Cells["Quantidade"].Value);

                    string queryVendasProdutos = "INSERT INTO vendas_produtos (codigo_venda, codigo_produto, nome_produto, valor_produto, quantidade) " +
                                                 "VALUES (@codigoVenda, @codigoProduto, @nomeProduto, @valorProduto, @quantidade)";
                    command = new MySqlCommand(queryVendasProdutos, connection);
                    command.Parameters.AddWithValue("@codigoVenda", codigoVenda);
                    command.Parameters.AddWithValue("@codigoProduto", codigoProduto);
                    command.Parameters.AddWithValue("@nomeProduto", nomeProduto);
                    command.Parameters.AddWithValue("@valorProduto", valorProduto);
                    command.Parameters.AddWithValue("@quantidade", quantidade);
                    command.ExecuteNonQuery();  // Executa a inserção na tabela vendas_produtos
                }

                // Execute a consulta SQL de atualização do estoque
                string atualizarEstoqueSql = "UPDATE produtos " +
                                            "SET quantidade_estoque = quantidade_estoque - @quantidadeVenda " +
                                            "WHERE codigo = @codigoProduto;";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string codigoProduto = row.Cells["Codigo"].Value.ToString();
                    string quantidadeStr = row.Cells["Quantidade"].Value.ToString();

                    if (int.TryParse(quantidadeStr, out int quantidade))
                    {
                        // Atualize o estoque somente se a quantidade for válida
                        int novaQuantidade = ObterNovaQuantidadeEstoque(codigoProduto, quantidade);

                        if (novaQuantidade < 0)
                        {
                            // A nova quantidade será negativa, exiba um aviso
                            MessageBox.Show($"O produto {codigoProduto} terá estoque negativo ({novaQuantidade} unidades) após a venda.",
                                            "Aviso de Estoque Negativo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            MySqlCommand cmdAtualizarEstoque = new MySqlCommand(atualizarEstoqueSql, connection);
                            cmdAtualizarEstoque.Parameters.AddWithValue("@quantidadeVenda", quantidade);
                            cmdAtualizarEstoque.Parameters.AddWithValue("@codigoProduto", codigoProduto);

                            cmdAtualizarEstoque.ExecuteNonQuery();
                        }
                        else
                        {
                            MySqlCommand cmdAtualizarEstoque = new MySqlCommand(atualizarEstoqueSql, connection);
                            cmdAtualizarEstoque.Parameters.AddWithValue("@quantidadeVenda", quantidade);
                            cmdAtualizarEstoque.Parameters.AddWithValue("@codigoProduto", codigoProduto);

                            cmdAtualizarEstoque.ExecuteNonQuery();
                        }
                    }
                }



                MessageBox.Show("Venda finalizada com sucesso!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao finalizar a venda: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private int ObterNovaQuantidadeEstoque(string codigoProduto, int quantidadeVenda)
        {
            int estoqueAtual = ObterQuantidadeEstoqueAtual(codigoProduto);

            // Verifique se a venda levará o estoque a um valor negativo
            int novaQuantidade = estoqueAtual - quantidadeVenda;
            return novaQuantidade;
        }

        private int ObterQuantidadeEstoqueAtual(string codigoProduto)
        {
            int quantidadeAtual = 0;

            // Consulta ao banco de dados para obter a quantidade em estoque
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT quantidade_estoque FROM produtos WHERE codigo = @codigoProduto";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@codigoProduto", codigoProduto);

                    // Execute a consulta e obtenha a quantidade atual em estoque
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        quantidadeAtual = Convert.ToInt32(result);
                    }
                }
                catch (MySqlException ex)
                {
                    // Lida com exceções de conexão ou comando SQL
                    MessageBox.Show("Erro de conexão: " + ex.Message);
                }
            }

            return quantidadeAtual;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ProcurarCliente_Venda frm = new ProcurarCliente_Venda();

            // Registra um manipulador de eventos para o evento ClienteSelecionado
            frm.ClienteSelecionado += (nomeCliente) =>
            {
                // Atualize o textbox6 com o nome do cliente selecionado
                textBox6.Text = nomeCliente;
            };

            frm.Show();
        }
    }
}
