using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace KitaplikProje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection baglantidoOleDbConnection =new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\fenny\Desktop\Kitaplik.mdb");

        void Getir()
        {
            DataTable dt=new DataTable();
            OleDbDataAdapter da=new OleDbDataAdapter("Select * From kitaplar",baglantidoOleDbConnection);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Getir();
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            Getir();
        }
        /*Radio Buton değer KOntrolü yönem 2
         * public string durum;
         *
         */
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (CmbTur.Text!= "")
            {
                DialogResult result = MessageBox.Show("Kaydetmek İstediğinize Emin misiniz?", "Onay",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result==DialogResult.Yes)
                {
                    baglantidoOleDbConnection.Open();
                    OleDbCommand komutkaydet = new OleDbCommand("insert into kitaplar (KitapAd,KitapYazar,KitapTur,KitapSayfaSayisi,KitapDurum) values (@p1,@p2,@p3,@p4,@p5)", baglantidoOleDbConnection);
                    komutkaydet.Parameters.AddWithValue("@p1", TxtKitapAdi.Text);
                    komutkaydet.Parameters.AddWithValue("@p2", TxtYazar.Text);
                    komutkaydet.Parameters.AddWithValue("@p3", CmbTur.Text);
                    komutkaydet.Parameters.AddWithValue("@p4", TxtSayfaSayisi.Text);
                    /*radio buton değer kontrolü yöntem 2
                     *komutkaydet.Parameters.AddWithValue("@p5",durum);
                     */
                    if (RbtnKullanılmış.Checked==true)
                    {
                        komutkaydet.Parameters.AddWithValue("@p5","0");
                    }

                    if (RrbtnSifir.Checked==true)
                    {
                        komutkaydet.Parameters.AddWithValue("@p5", "1");
                    }

                    komutkaydet.ExecuteNonQuery();
                    
                    MessageBox.Show("Kayıt İşlemi Yapıldı");
                    Getir();
                }
                else
                {
                    MessageBox.Show("Kayıt İşlemi İptal Edildi");
                }
            }
            else
            {
                MessageBox.Show("Lütfen Kitap Türünü Seçiniz");
            }
            baglantidoOleDbConnection.Close();

        }

        private void RbtnKullanılmış_CheckedChanged(object sender, EventArgs e)
        {
            /*radio buton değer kontrolü yöntem 2
             * durum="0";
             */
            //durum = "0";
        }

        private void RrbtnSifir_CheckedChanged(object sender, EventArgs e)
        {
            /*radio buton değer kontrolü yöntem 2*/
            //durum = "1";

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            TxtKitapID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtKitapAdi.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtYazar.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            CmbTur.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtSayfaSayisi.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            if (dataGridView1.Rows[secilen].Cells[5].Value.ToString()=="True")
            {
                RrbtnSifir.Checked = true;
            }
            else
            {
                RbtnKullanılmış.Checked=true;
            }
            
            
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(TxtKitapAdi.Text + " Adlı Kitap Silinecektir Onaylıyor musunuz?",
                "Silme Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result==DialogResult.Yes)
            {
                baglantidoOleDbConnection.Open();
                OleDbCommand komutole = new OleDbCommand("Delete From kitaplar where KitapId=@p1", baglantidoOleDbConnection);
                komutole.Parameters.AddWithValue("@p1", TxtKitapID.Text);
                komutole.ExecuteNonQuery();
                baglantidoOleDbConnection.Close();
                MessageBox.Show(TxtKitapAdi.Text + " Adlı Kitap Silindi");
                Getir();
            }
            else
            {
                MessageBox.Show("Silme İşlemi İptal Edildi");
            }

        }

        
        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            baglantidoOleDbConnection.Open();
            OleDbCommand komut=new OleDbCommand("update kitaplar set KitapAd=@p1,KitapYazar=@p2,KitapTur=@p3,KitapSayfaSayisi=@p4,KitapDurum=@p5 where KitapId=@p6", baglantidoOleDbConnection);
            
            komut.Parameters.AddWithValue("@p1", TxtKitapAdi.Text);
            komut.Parameters.AddWithValue("@p2", TxtYazar.Text);
            komut.Parameters.AddWithValue("@p3", CmbTur.Text);
            komut.Parameters.AddWithValue("@p4", TxtSayfaSayisi.Text);
            if (RbtnKullanılmış.Checked == true)
            {
                komut.Parameters.AddWithValue("@p5", "0");
            }

            if (RrbtnSifir.Checked == true)
            {
                komut.Parameters.AddWithValue("@p5", "1");
            }
            komut.Parameters.AddWithValue("@p6", TxtKitapID.Text);
            komut.ExecuteNonQuery();
            baglantidoOleDbConnection.Close();
            MessageBox.Show(TxtKitapAdi.Text + " Adlı Kitap Güncellendi");
            Getir();
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            if (TxtKitapBul.Text == "")
            {
                Getir();
            }
            else
            {
                baglantidoOleDbConnection.Open();
                OleDbCommand komut = new OleDbCommand("Select * From kitaplar where KitapAd=@p1", baglantidoOleDbConnection);
                komut.Parameters.AddWithValue("@p1", TxtKitapBul.Text);
                DataTable dt = new DataTable();
                OleDbDataAdapter da = new OleDbDataAdapter(komut);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                baglantidoOleDbConnection.Close();
            }
            

        }

        void ara()
        {
            if (TxtKitapBul.Text == "")
            {
                Getir();
            }
            else
            {
                baglantidoOleDbConnection.Open();
                OleDbCommand komut = new OleDbCommand("Select * From kitaplar where KitapAd like '%" + TxtKitapBul.Text + "%'", baglantidoOleDbConnection);
                DataTable dt = new DataTable();
                OleDbDataAdapter da = new OleDbDataAdapter(komut);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                baglantidoOleDbConnection.Close();
            }
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            ara();
        }

        private void TxtKitapBul_TextChanged(object sender, EventArgs e)
        {
           baglantidoOleDbConnection.Open();
           DataTable dt = new DataTable();
           OleDbDataAdapter da=new OleDbDataAdapter("Select * From kitaplar where KitapAd like '%" + TxtKitapBul.Text + "%'",baglantidoOleDbConnection);
           da.Fill(dt);
           baglantidoOleDbConnection.Close();
           dataGridView1.DataSource = dt;
        }
    }
}
