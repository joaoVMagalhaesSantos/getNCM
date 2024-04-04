using System.Diagnostics.Metrics;
using getNCM.Class;
using MySqlConnector;
using Newtonsoft.Json;

namespace getNCM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using var myConnection = new MySqlConnection(Conexao.StrConexao().ConnectionString);
                myConnection.Open();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar" + ex.Message, "Alerta");
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Conexao.NOM_QUERY("delete from ncm");
            Conexao.NOM_QUERY("alter table ncm auto_increment = 0");
            

            progressBar1.Minimum = 0;

            string apiUrl = "https://val.portalunico.siscomex.gov.br/classif/api/publico/nomenclatura/download/json";

            var nomenclaturas = await GetNCM.ObterNomenclaturasAsync(apiUrl);

            progressBar1.Maximum = nomenclaturas.Count();
            progressBar1.Step = 1;
            if (nomenclaturas != null)
            {
                foreach (var nomenclatura in nomenclaturas)
                {
                    if(nomenclatura.codigo != null)
                    {
                        nomenclatura.codigo = nomenclatura.codigo.Replace(".", "").PadRight(9, '0');
                    }
                    
                    if (nomenclatura.descricao != null)
                    {
                        nomenclatura.descricao = nomenclatura.descricao.Replace("'", "");
                    }

                    if (nomenclatura.nomero_ato_ini == null)
                    {
                        nomenclatura.nomero_ato_ini = "n";
                    }
                    try
                    {
                        progressBar1.PerformStep();
                        Conexao.NOM_QUERY("INSERT INTO ncm (codigo,descricao,datainicio,datafim,tipoAtoIni,numeroAtoIni,anoAtoIni) values ( '" + nomenclatura.codigo + "','" + nomenclatura.descricao + "','" + nomenclatura.data_inicio + "','" + nomenclatura.data_fim + "','" + nomenclatura.tipo_ato_ini + "','" + nomenclatura.nomero_ato_ini + "'," + nomenclatura.ano_ato_ini + ")");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro: " + ex);
                    }

                }
            }

        }
    }
}