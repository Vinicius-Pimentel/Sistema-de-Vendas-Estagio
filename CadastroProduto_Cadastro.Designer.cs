
namespace Sistema_de_Vendas
{
    partial class CadastroProduto_Cadastro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pdt_cliente = new System.Windows.Forms.TextBox();
            this.pdt_estoque = new System.Windows.Forms.TextBox();
            this.pdt_qtd_minima = new System.Windows.Forms.TextBox();
            this.pdt_preco_c = new System.Windows.Forms.TextBox();
            this.pdt_preco_v = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nome";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Estoque";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Quantidade Mínima";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Preço de Custo";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Preço de Venda";
            // 
            // pdt_cliente
            // 
            this.pdt_cliente.Location = new System.Drawing.Point(58, 12);
            this.pdt_cliente.Name = "pdt_cliente";
            this.pdt_cliente.Size = new System.Drawing.Size(403, 20);
            this.pdt_cliente.TabIndex = 7;
            this.pdt_cliente.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // pdt_estoque
            // 
            this.pdt_estoque.Location = new System.Drawing.Point(58, 51);
            this.pdt_estoque.Name = "pdt_estoque";
            this.pdt_estoque.Size = new System.Drawing.Size(34, 20);
            this.pdt_estoque.TabIndex = 8;
            // 
            // pdt_qtd_minima
            // 
            this.pdt_qtd_minima.Location = new System.Drawing.Point(118, 87);
            this.pdt_qtd_minima.Name = "pdt_qtd_minima";
            this.pdt_qtd_minima.Size = new System.Drawing.Size(34, 20);
            this.pdt_qtd_minima.TabIndex = 9;
            // 
            // pdt_preco_c
            // 
            this.pdt_preco_c.Location = new System.Drawing.Point(98, 129);
            this.pdt_preco_c.Name = "pdt_preco_c";
            this.pdt_preco_c.Size = new System.Drawing.Size(54, 20);
            this.pdt_preco_c.TabIndex = 10;
            // 
            // pdt_preco_v
            // 
            this.pdt_preco_v.Location = new System.Drawing.Point(98, 170);
            this.pdt_preco_v.Name = "pdt_preco_v";
            this.pdt_preco_v.Size = new System.Drawing.Size(54, 20);
            this.pdt_preco_v.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(145, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Cadastrar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(252, 210);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Excluir";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CadastroProduto_Cadastro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 244);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pdt_preco_v);
            this.Controls.Add(this.pdt_preco_c);
            this.Controls.Add(this.pdt_qtd_minima);
            this.Controls.Add(this.pdt_estoque);
            this.Controls.Add(this.pdt_cliente);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "CadastroProduto_Cadastro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cadastro de Produtos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox pdt_cliente;
        private System.Windows.Forms.TextBox pdt_estoque;
        private System.Windows.Forms.TextBox pdt_qtd_minima;
        private System.Windows.Forms.TextBox pdt_preco_c;
        private System.Windows.Forms.TextBox pdt_preco_v;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}