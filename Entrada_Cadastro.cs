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
using System.Xml;
using System.Globalization;

namespace Sistema_de_Vendas
{
    public partial class Entrada_Cadastro : Form
    {
        private string connectionString;
        private DataGridView dataGridViewProdutos;
        private List<string> codigosNaoCadastrados = new List<string>();

        private void Entrada_Cadastro_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Limpe a lista quando a tela estiver sendo fechada
            GlobalData.CodigosProdutosAdicionados.Clear();
        }

        public Entrada_Cadastro()
        {
            InitializeComponent();
            connectionString = $"Server=estagio_facul.mysql.dbaas.com.br;Database=estagio_facul;Uid=estagio_facul;Pwd=Vinicius2002@;Charset=utf8mb4;";
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dataGridViewProdutos = new DataGridView();
            dataGridViewProdutos.AllowUserToAddRows = false;
            dataGridViewProdutos.Columns.Add("Codigo", "Código");
            dataGridViewProdutos.Columns.Add("Nome", "Nome"); // Adicione a coluna "Nome" aqui
            dataGridViewProdutos.Columns.Add("Valor Total", "Valor Total");
            dataGridViewProdutos.Columns.Add("Quantidade", "Quantidade");
            dataGridViewProdutos.Columns.Add("Peso", "Peso");
            dataGridViewProdutos.Columns.Add("Quantidade Real", "Quantidade Real"); // Adicione a coluna "Quantidade Real" aqui
            //dataGridViewProdutos.Columns["Codigo"].ReadOnly = true;
            //dataGridViewProdutos.Columns.Add("Cor", "Cor");
            dataGridViewProdutos.Dock = DockStyle.Fill;

            // Adicione a DataGridView como um controle filho do painel DataGridView1
            dataGridView1.Controls.Add(dataGridViewProdutos);
            dataGridViewProdutos.KeyDown += dataGridView1_KeyDown;
            dataGridViewProdutos.CellEndEdit += dataGridView1_CellEndEdit;
        }

        public static class GlobalData
        {
            public static HashSet<string> CodigosProdutosAdicionados { get; } = new HashSet<string>();
        }

        private void button1_Click(object sender, EventArgs e)
    {
        ProcurarProduto_Venda frm = new ProcurarProduto_Venda();
        
        // Registre um manipulador de eventos para o evento ProdutoSelecionado
        frm.ProdutoSelecionado += (codigoProduto, nomeProduto) =>
        {
            // Adicione o produto à tela de entrada
            ExibirProdutoNaTelaProcurar(codigoProduto, nomeProduto, Color.Green);
        };
        
        frm.Show();
    }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Arquivos XML (*.xml)|*.xml";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string xmlFilePath = openFileDialog.FileName;

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlFilePath);

                    // Crie um namespace manager para lidar com namespaces no XML
                    XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                    nsManager.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");

                    // Selecione o nó infNFe usando o namespace
                    XmlNode infNFeNode = xmlDoc.SelectSingleNode("//nfe:infNFe", nsManager);
                    if (infNFeNode != null)
                    {
                        // Preencha os campos com os dados do XML
                        textBoxChaveAcesso.Text = infNFeNode.Attributes["Id"].Value.Replace("NFe", "");

                        // Restante do código para preencher outros campos
                        textBoxNomeFornecedor.Text = infNFeNode.SelectSingleNode("//nfe:emit/nfe:xNome", nsManager).InnerText;
                        XmlNode dhEmiNode = infNFeNode.SelectSingleNode("//nfe:dhEmi", nsManager);
                        if (dhEmiNode != null)
                        {
                            // Obtenha o valor da tag <dhEmi>
                            string dhEmiValue = dhEmiNode.InnerText;

                            // Converta a data e hora para um formato desejado (dia/mês/ano)
                            if (DateTime.TryParse(dhEmiValue, out DateTime dataEmissao))
                            {
                                string dataEmissaoFormatada = dataEmissao.ToString("dd/MM/yyyy");
                                textBoxDataEmissao.Text = dataEmissaoFormatada;
                            }
                            else
                            {
                                textBoxDataEmissao.Text = "Data inválida";
                            }
                        }
                        XmlNode valorTotalNode = infNFeNode.SelectSingleNode("//nfe:ICMSTot/nfe:vNF", nsManager);
                        if (valorTotalNode != null)
                        {
                            string valorTotal = valorTotalNode.InnerText;
                            textBoxValorTotal.Text = valorTotal;
                        }

                        // Adicione mais campos conforme necessário
                    }

                    // Verifique os produtos do XML e exiba na tela
                    XmlNodeList produtosNodes = xmlDoc.SelectNodes("//nfe:det", nsManager);
                    foreach (XmlNode produtoNode in produtosNodes)
                    {
                        string codigoProduto = produtoNode.SelectSingleNode("./nfe:prod/nfe:cProd", nsManager).InnerText;
                        string nomeProduto = produtoNode.SelectSingleNode("./nfe:prod/nfe:xProd", nsManager).InnerText;
                        string valorTotalProduto = produtoNode.SelectSingleNode(".//nfe:prod/nfe:vProd", nsManager).InnerText;
                        string quantidadeProduto = produtoNode.SelectSingleNode(".//nfe:prod/nfe:qCom", nsManager).InnerText;

                        if (ProdutoExisteNoBanco(codigoProduto))
                        {
                            ExibirProdutoNaTela(codigoProduto, nomeProduto, valorTotalProduto, quantidadeProduto, Color.Green);
                        }
                        else
                        {
                            ExibirProdutoNaTela(codigoProduto, nomeProduto, valorTotalProduto, quantidadeProduto, Color.Red);
                        }
                    }

                    MessageBox.Show("Dados importados com sucesso.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao importar dados: {ex.Message}");
            }
        }
        private bool ProdutoExisteNoBanco(string codigoProduto)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                    string query = "SELECT COUNT(*) FROM produtos WHERE codigo = @codigoProduto";
                    MySqlCommand cmd = new MySqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@codigoProduto", codigoProduto);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro de MySQL: " + ex.Message);
                    return false;
                }
            }
        }

        // Função para exibir o produto na tela de entrada
        private void ExibirProdutoNaTela(string codigoProduto, string nomeProduto, string valorTotalProduto, string quantidadeProduto, Color cor)
        {
            // Verifique se o código do produto já foi adicionado
            if (GlobalData.CodigosProdutosAdicionados.Contains(codigoProduto))
            {
                MessageBox.Show("Este produto já foi adicionado.");
                return;
            }

            // Adicione o produto à lista de produtos já adicionados
            GlobalData.CodigosProdutosAdicionados.Add(codigoProduto);
            int rowIndex = dataGridViewProdutos.Rows.Add();
            dataGridViewProdutos.Rows[rowIndex].Cells["Codigo"].Value = codigoProduto;
            dataGridViewProdutos.Rows[rowIndex].Cells["Nome"].Value = nomeProduto; // Adicione uma coluna "Nome" na DataGridView
            dataGridViewProdutos.Rows[rowIndex].Cells["Valor Total"].Value = valorTotalProduto;
            dataGridViewProdutos.Rows[rowIndex].Cells["Quantidade"].Value = quantidadeProduto;
            dataGridViewProdutos.Rows[rowIndex].DefaultCellStyle.BackColor = cor;
            // Se a linha for vermelha, adicione o código à lista de não cadastrados
            if (cor == Color.Red)
            {
                codigosNaoCadastrados.Add(codigoProduto);
            }
        }

        private void ExibirProdutoNaTelaProcurar(string codigoProduto, string nomeProduto, Color cor)
        {

            int rowIndex = dataGridViewProdutos.Rows.Add();
            dataGridViewProdutos.Rows[rowIndex].Cells["Codigo"].Value = codigoProduto;
            dataGridViewProdutos.Rows[rowIndex].Cells["Nome"].Value = nomeProduto; // Adicione uma coluna "Nome" na DataGridView
            dataGridViewProdutos.Rows[rowIndex].DefaultCellStyle.BackColor = cor;
            // Se a linha for vermelha, adicione o código à lista de não cadastrados
            if (cor == Color.Red)
            {
                codigosNaoCadastrados.Add(codigoProduto);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CadastroProduto_Entrada frm = new CadastroProduto_Entrada();
            frm.Show();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifique se a tecla pressionada é Enter (código de tecla 13)
            if (e.KeyCode == Keys.Enter)
            {
                // Obtenha a célula atual
                DataGridViewCell currentCell = dataGridViewProdutos.CurrentCell;

                // Verifique se a célula atual pertence à coluna "Peso"
                if (currentCell != null && currentCell.OwningColumn.Name == "Peso")
                {
                    // Obtenha o índice da linha onde ocorreu a edição
                    int rowIndex = currentCell.RowIndex;

                    // Obtenha o valor digitado na célula "Peso"
                    string pesoStr = dataGridViewProdutos.Rows[rowIndex].Cells["Peso"].Value?.ToString();

                    // Obtenha a quantidade original da célula "Quantidade"
                    string quantidadeStr = dataGridViewProdutos.Rows[rowIndex].Cells["Quantidade"].Value?.ToString();

                    // Realize o cálculo da "Quantidade Real" e exiba o resultado
                    if (!string.IsNullOrEmpty(pesoStr) && !string.IsNullOrEmpty(quantidadeStr))
                    {
                        if (double.TryParse(pesoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double peso) &&
                            double.TryParse(quantidadeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double quantidade))
                        {
                            double quantidadeReal = quantidade / peso;
                            dataGridViewProdutos.Rows[rowIndex].Cells["Quantidade Real"].Value = quantidadeReal.ToString("0.00", CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dataGridViewProdutos.Columns["Codigo"].Index) // Verifique se a edição foi na coluna de Código
                {
                    // Obtenha o valor da célula editada
                    string codigoProduto = dataGridViewProdutos.Rows[e.RowIndex].Cells["Codigo"].Value?.ToString();

                    // Verifique se o código é nulo ou vazio
                    if (string.IsNullOrEmpty(codigoProduto))
                    {
                        // Remova a linha
                        dataGridViewProdutos.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ao apagar o código, você deve clicar na coluna e depois pressionar a tecla 'Enter'.");
            }

        }

        private bool VerificarProdutosCadastrados()
        {
            foreach (DataGridViewRow row in dataGridViewProdutos.Rows)
            {
                string codigoProduto = row.Cells["Codigo"].Value.ToString();

                if (!ProdutoExisteNoBanco(codigoProduto))
                {
                    return false;
                }
            }

            return true;
        }

        private bool NotaEntradaExiste(string chaveAcesso)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Execute uma consulta SQL para verificar se já existe uma nota com a mesma chave de acesso
                    string query = "SELECT COUNT(*) FROM notas_de_entrada WHERE chave_de_acesso_nfe = @chaveAcesso";
                    MySqlCommand cmd = new MySqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@chaveAcesso", chaveAcesso);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    return count > 0;
                }
                catch (MySqlException ex)
                {
                    // Lida com exceções de conexão ou comando SQL
                    MessageBox.Show("Erro de conexão: " + ex.Message);
                    return false;
                }
            }
        }

        private bool ValidarQuantidadeReal()
        {
            foreach (DataGridViewRow row in dataGridViewProdutos.Rows)
            {
                string quantidadeReal = row.Cells["Quantidade Real"].Value?.ToString();

                if (string.IsNullOrEmpty(quantidadeReal))
                {
                    // Exiba uma mensagem de erro
                    MessageBox.Show("A coluna 'Quantidade Real' não pode estar vazia.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private bool TryParseDataEmissao(out string dataEmissao)
        {
            string dataEmissaoFormatada = string.Empty;

            // Tente fazer a conversão
            if (DateTime.TryParseExact(textBoxDataEmissao.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
            {
                // Se a conversão for bem-sucedida, formate a data no formato MySQL
                dataEmissaoFormatada = data.ToString("yyyy-MM-dd HH:mm:ss");
                dataEmissao = dataEmissaoFormatada;
                return true;
            }
            else
            {
                // Se a conversão falhar, defina a data de emissão como vazia
                dataEmissao = string.Empty;
                return false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {

                if (string.IsNullOrWhiteSpace(textBoxChaveAcesso.Text) ||
                    string.IsNullOrWhiteSpace(textBoxNomeFornecedor.Text) ||
                    string.IsNullOrWhiteSpace(textBoxValorTotal.Text) ||
                    string.IsNullOrWhiteSpace(textBoxDataEmissao.Text))
                {
                    MessageBox.Show("Certifique-se de preencher todos os campos obrigatórios.");
                    return;
                }


                string chaveAcesso = textBoxChaveAcesso.Text;

                if (!ValidarQuantidadeReal())
                {
                    // Se a validação falhar, saia da função
                    return;
                }

                string dataEmissao;

                // Tente obter a data de emissão no formato MySQL
                if (!TryParseDataEmissao(out dataEmissao))
                {
                    // Exiba uma mensagem de erro se a data não estiver no formato correto
                    MessageBox.Show("A data de emissão não está em um formato válido (dd/MM/yyyy).", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verifique se uma nota com a mesma chave de acesso já existe
                if (NotaEntradaExiste(chaveAcesso))
                {
                    MessageBox.Show("Uma nota com a mesma chave de acesso já existe. Não é possível inserir novamente.");
                    return;
                }
                try
                {
                    conexao.Open();


                    // Verifique se todos os produtos estão cadastrados
                    bool todosProdutosCadastrados = VerificarProdutosCadastrados();

                    if (!todosProdutosCadastrados)
                    {
                        MessageBox.Show("Pelo menos um produto não está cadastrado. Verifique os produtos e tente novamente.");
                        return;
                    }

                    // Verifique o formato da data de inclusão
                    DateTime dataInclusao;
                    if (!DateTime.TryParseExact(textBox5.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataInclusao))
                    {
                        // Se o formato estiver incorreto ou o campo estiver vazio, use a data atual
                        dataInclusao = DateTime.Now;
                    }

                    float valorTotal;

                    if (float.TryParse(textBoxValorTotal.Text.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out valorTotal))
                    {
                        // O valor foi convertido com sucesso, você pode usá-lo
                    }

                    // Insira os dados na tabela notas_de_entrada
                    string inserirNotaEntradaSql = "INSERT INTO notas_de_entrada (nome_fornecedor, chave_de_acesso_nfe, data_emissao, data_inclusao, valor_total_nfe) " +
                                                   "VALUES (@nomeFornecedor, @chaveAcessoNFe, @dataEmissao, @dataInclusao, @valorTotalNFe);";

                    MySqlCommand cmdInserirNotaEntrada = new MySqlCommand(inserirNotaEntradaSql, conexao);
                    cmdInserirNotaEntrada.Parameters.AddWithValue("@nomeFornecedor", textBoxNomeFornecedor.Text);
                    cmdInserirNotaEntrada.Parameters.AddWithValue("@chaveAcessoNFe", textBoxChaveAcesso.Text);
                    cmdInserirNotaEntrada.Parameters.AddWithValue("@dataEmissao", dataEmissao);
                    cmdInserirNotaEntrada.Parameters.AddWithValue("@dataInclusao", dataInclusao);
                    cmdInserirNotaEntrada.Parameters.AddWithValue("@valorTotalNFe", valorTotal);

                    cmdInserirNotaEntrada.ExecuteNonQuery();

                    // Obtenha o código da nota de entrada recém-inserida
                    int codigoNotaEntrada;
                    string obterCodigoSql = "SELECT LAST_INSERT_ID();";
                    MySqlCommand cmdObterCodigo = new MySqlCommand(obterCodigoSql, conexao);
                    codigoNotaEntrada = Convert.ToInt32(cmdObterCodigo.ExecuteScalar());

                    // Insira os dados na tabela notas_de_entrada_produtos
                    foreach (DataGridViewRow row in dataGridViewProdutos.Rows)
                    {
                        if (!row.IsNewRow) // Verifica se a linha não é nova (em branco)
                        {
                            string codigoProduto = row.Cells["Codigo"].Value.ToString();
                            float valorProduto = 0.00f;
                            int quantidade;

                            if (!string.IsNullOrEmpty(row.Cells["Valor Total"].Value?.ToString()))
                            {
                                string valorStr = row.Cells["Valor Total"].Value.ToString().Replace(",", ".");
                                float.TryParse(valorStr, NumberStyles.Float, CultureInfo.InvariantCulture, out valorProduto);
                            }

                            if (int.TryParse(Math.Round(Convert.ToDouble(row.Cells["Quantidade Real"].Value), MidpointRounding.AwayFromZero).ToString(), out quantidade))
                            {
                                // O valor foi convertido com sucesso, você pode usá-lo
                            }

                            string inserirProdutoSql = "INSERT INTO notas_de_entrada_produtos (codigo_nota, codigo_produto, valor_produto, quantidade) " +
                                                        "VALUES (@codigoNota, @codigoProduto, @valorProduto, @quantidade);";

                            MySqlCommand cmdInserirProduto = new MySqlCommand(inserirProdutoSql, conexao);
                            cmdInserirProduto.Parameters.AddWithValue("@codigoNota", codigoNotaEntrada);
                            cmdInserirProduto.Parameters.AddWithValue("@codigoProduto", codigoProduto);
                            cmdInserirProduto.Parameters.AddWithValue("@valorProduto", valorProduto);
                            cmdInserirProduto.Parameters.AddWithValue("@quantidade", quantidade/100);

                            cmdInserirProduto.ExecuteNonQuery();

                            // Execute a consulta SQL de atualização do estoque
                            string atualizarEstoqueSql = "UPDATE produtos " +
                                                        "SET quantidade_estoque = quantidade_estoque + @quantidadeEntrada " +
                                                        "WHERE codigo = @codigoProduto;";

                            MySqlCommand cmdAtualizarEstoque = new MySqlCommand(atualizarEstoqueSql, conexao);
                            cmdAtualizarEstoque.Parameters.AddWithValue("@quantidadeEntrada", quantidade/100);
                            cmdAtualizarEstoque.Parameters.AddWithValue("@codigoProduto", codigoProduto);

                            cmdAtualizarEstoque.ExecuteNonQuery();
                        }
                    }
                    GlobalData.CodigosProdutosAdicionados.Clear();
                    MessageBox.Show("Dados da nota de entrada e produtos inseridos com sucesso.");
                    this.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro de MySQL: " + ex.Message);
                }
            }
        }
        private void AtualizarCoresProdutos()
        {
            foreach (DataGridViewRow row in dataGridViewProdutos.Rows)
            {
                string codigoProduto = row.Cells["Codigo"].Value.ToString();

                if (ProdutoExisteNoBanco(codigoProduto))
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Atualize as cores dos produtos na grade
            AtualizarCoresProdutos();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ProcurarFornecedor_Entrada frm = new ProcurarFornecedor_Entrada();

            // Registra um manipulador de eventos para o evento ClienteSelecionado
            frm.FornecedorSelecionado += (nomeFornecedor) =>
            {
                // Atualize o textbox6 com o nome do cliente selecionado
                textBoxNomeFornecedor.Text = nomeFornecedor;
            };

            frm.Show();
        }
    }
}


