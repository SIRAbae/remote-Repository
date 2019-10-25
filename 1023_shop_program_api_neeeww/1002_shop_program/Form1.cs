using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Application = Microsoft.Office.Interop.Excel.Application;
using Microsoft.Office.Interop.Excel;

namespace _1002_shop_program
{
    public partial class Form1 : Form
    {

        #region API
        BuyDateSearcher bd = new BuyDateSearcher();
        BlogDateSearcher blogd = new BlogDateSearcher();
        #endregion

        #region 컬렉션
        List<BuyDate> buylist = new List<BuyDate>();
        List<BlogDate> bloglist = new List<BlogDate>();
        #endregion

        #region 속성 및 프로퍼티
        FolderBrowserDialog folderBrowser;
        Setting setting;
        int y =0; //상품리스트 좌표
        int x =0; //상품리스트 좌표
        int searchNum; //검색번호
        string SaveLink; //저장경로
        string FileLink; //불러오기경로
        int display = 10; //상품출력 개수
        int Money = 0; //예산
        bool MoneyMode = false;//예산모드

        #endregion

        #region 생성자
        public Form1()
        {
            InitializeComponent();
            comboBox2.Text = "관련도";
            folderBrowser = new FolderBrowserDialog();
            setting = new Setting(this);
            SaveLink = string.Empty;
        }
        #endregion

        #region 상품검색
        //검색
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                searchNum = 1;
                textBox2.Text = searchNum.ToString();
                bd.SearchBuy(textBox1.Text, comboBox2.Text, searchNum.ToString(),display.ToString());
                button1.Enabled = true;
                button3.Enabled = true;
                PlintSearchList();
            }
            catch (Exception)
            {
                MessageBox.Show("검색할 수 없습니다.");
            }
        }

        //다음>>
        private void button1_Click(object sender, EventArgs e)
        {
            searchNum++;
            int num = searchNum * display;
            textBox2.Text = searchNum.ToString();
            bd.SearchBuy(textBox1.Text, comboBox2.Text, num.ToString(), display.ToString());
            PlintSearchList();
        }

        //<<이전
        private void button3_Click(object sender, EventArgs e)
        {
            if (searchNum <= 1)
                return;

            searchNum--;
            textBox2.Text = searchNum.ToString();
            bd.SearchBuy(textBox1.Text, comboBox2.Text, searchNum.ToString(), display.ToString());
            PlintSearchList();
        }

        //출력
        private void PlintSearchList()
        {

            panel1.Controls.Clear();
            x = 0;
            y = 0;

            Loading lo = new Loading();

            lo.Function = (() =>
            {
                for (int i = 0; i < bd.buydateList.Count; i++)
                {
                    string filepath = bd.buydateList[i].Image;
                    byte[] data = new System.Net.WebClient().DownloadData(filepath);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
                    Image img = Image.FromStream(ms);

                    shopingItem item =
                        new shopingItem(img, bd.buydateList[i].Title,
                                        bd.buydateList[i].Lprice_s,
                                        bd.buydateList[i].Link.ToString(),
                                        this);

                    this.Invoke(new MethodInvoker(
                         delegate ()
                         {
                             panel1.Controls.Add(item);
                         }
                     )
                    );

                    if (x == 0 && y == 0)
                    {
                        this.Invoke(new MethodInvoker(
                            delegate ()
                            {
                                item.Location = new System.Drawing.Point(x, y + 10);
                            }
                          )
                        );
                        x = item.Location.X;
                        y = item.Location.Y;
                    }
                    else
                    {
                        this.Invoke(new MethodInvoker(
                            delegate ()
                            {
                                item.Location = new System.Drawing.Point(x, y + 180);
                            }
                          )
                        );
                        x = item.Location.X;
                        y = item.Location.Y;
                    }

                    item.num.Text = (i + 1).ToString();
                }
            });
            lo.ShowDialog();
        }

        //브라우저 검색
        public void GoBrowser(string link)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            webBrowser1.Navigate(link);    //통합검색
        }

        //검색 문자열 출력
        public void PrintSearchTxt(string Title)
        {
            string searchword = string.Empty;
            searchword += Title;
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                searchword += listBox2.Items[i];
            }

            textBox4.Text = searchword;
        }
        //장바구니 데이터 개별 선택
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            tabControl1.SelectedTab = tabControl1.TabPages[2];

            panel3.Controls.Clear();

            int idx = listBox1.SelectedIndex;

            string filepath = buylist[idx].Image;
            byte[] data = new System.Net.WebClient().DownloadData(filepath);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
            Image img = Image.FromStream(ms);

            shopingItem item =
                        new shopingItem(img, buylist[idx].Title,
                                        buylist[idx].Lprice_s.ToString(),
                                        buylist[idx].Link.ToString(),
                                        this);
            this.Invoke(new MethodInvoker(
                         delegate ()
                         {
                             panel3.Controls.Add(item);
                         }
                     ));
        }

        #endregion

        #region 정보출력
        private void PrintBuylist()
        {
            foreach (BuyDate bu in buylist)
            {
                listBox1.Items.Add(bu.Title);
            }
        }

       
        #endregion

        #region 장바구니
        //장바구니 추가(shopingitem에서 호출)
        public void AddList(int idx)
        {
            if (MoneyMode == true)
            {
                if ((AllMoney() + bd.buydateList[idx - 1].Lprice) > Money)
                {
                    string str = string.Format("예산초과 : 예산 : {0}원", Money);
                    MessageBox.Show(str);
                    return;
                }


            }

            buylist.Add(bd.buydateList[idx - 1]);
            listBox1.Items.Clear();
            PrintBuylist();
            AllMoney();
        }
        //장바구니 물건 삭제
        private void button2_Click(object sender, EventArgs e)
        {
            if (buylist.Count <= 0 || listBox1.SelectedIndex ==-1)
                return;

            int a = listBox1.SelectedIndices.Count;
            BuyDate[] buys = new BuyDate[a];

            for (int i = 0; i < a; i++)
            {
                buys[i] = buylist[listBox1.SelectedIndices[i]];
            }

            for (int i = 0; i < a; i++)
            {
                buylist.Remove(buys[i]);
            }

            listBox1.Items.Clear();
            foreach (BuyDate bu in buylist)
            {
                listBox1.Items.Add(bu.Title);
            }
            AllMoney();
        }

        //장바구니 전체 돈 출력
        private int AllMoney()
        {
            int all = 0;
            foreach (BuyDate bu in buylist)
            {
                all += bu.Lprice;
            }
            textBox7.Text = all.ToString();

            return all;
        }

        //장바구니 탭 누르면 장바구니 데이터 출력
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedTab != tabControl1.TabPages[2])
            //    return;

            //PrintBuyData();

        }

        //장바구니 데이터 출력
        private void PrintBuyData()
        {
            panel3.Controls.Clear();
            x = 0;
            y = 0;

            Loading lo = new Loading();

            lo.Function = (() =>
            {
                for (int i = 0; i < buylist.Count; i++)
                {
                    string filepath = buylist[i].Image;
                    byte[] data = new System.Net.WebClient().DownloadData(filepath);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
                    Image img = Image.FromStream(ms);

                    shopingItem item =
                        new shopingItem(img, buylist[i].Title,
                                        buylist[i].Lprice_s,
                                        buylist[i].Link.ToString(),
                                        this);

                    this.Invoke(new MethodInvoker(
                         delegate ()
                         {
                             panel3.Controls.Add(item);
                         }
                     )
                    );

                    if (x == 0 && y == 0)
                    {
                        this.Invoke(new MethodInvoker(
                            delegate ()
                            {
                                item.Location = new System.Drawing.Point(x, y + 10);
                            }
                          )
                        );
                        x = item.Location.X;
                        y = item.Location.Y;
                    }
                    else
                    {
                        this.Invoke(new MethodInvoker(
                            delegate ()
                            {
                                item.Location = new System.Drawing.Point(x, y + 180);
                            }
                          )
                        );
                        x = item.Location.X;
                        y = item.Location.Y;
                    }

                    item.num.Text = (i + 1).ToString();
                }
            });
            lo.ShowDialog();
        }

        //장바구니 데이터 단일출력
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          

        }

        //장바구니 전체출력
        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[2];
            PrintBuyData();
        }
        #endregion

        #region 메뉴
        private void 저장경로ToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            SaveData();
        }

        private void 프로그램설정SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (setting.ShowDialog() == DialogResult.OK)
            {
                display = setting.Display;
                Money = setting.Money;
                MoneyMode = setting.MoneyMode;
            }
        }

        private bool GetSaveURL()
        {
            DialogResult dr = folderBrowser.ShowDialog();
            if (dr == DialogResult.OK)
            {
               SaveLink = folderBrowser.SelectedPath;
                return true;
            }
            return false;
        }

        private void GetLoadFile()
        {
            OpenFileDialog op = new OpenFileDialog();
            DialogResult dr = op.ShowDialog();
            if (dr == DialogResult.OK)
            {
                FileLink = op.FileName;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("이용해주셔서 감사합니다.");
        }

        private void 프로그램종료EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }







        #endregion

        #region 키워드 검색
        //키워드 추가
        private void button5_Click(object sender, EventArgs e)
        {

            if (textBox3.Text.Trim() == "")
                return;

            string rogic = string.Empty;

            switch (comboBox1.Text)
            {
                case "포함": rogic = "+"; break;
                case "제외": rogic = "-"; break;
                case "정확히 일치": rogic = "\""; break;
                case "특정파일타입": rogic = "filetype:"; break;
                default: MessageBox.Show("키워드 종류를 선택해 주세요"); return;
            }

            if (comboBox1.Text.Equals("정확히 일치"))
                listBox2.Items.Add(" " + rogic + textBox3.Text+rogic);
            else
                listBox2.Items.Add(" " +rogic + textBox3.Text);

            textBox3.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        public void SearchBrowser(string name)
        {
            string searchword = textBox4.Text;
            //searchword += name;
            //for (int i = 0; i < listBox2.Items.Count; i++)
            //{
            //    searchword += listBox2.Items[i];
            //}
            searchword = searchword.Replace("+", "%2B");

            tabControl1.SelectedTab = tabControl1.TabPages[1];
            webBrowser1.Navigate("https://search.naver.com/search.naver?where=nexearch&sm=tab_jum&query=" + searchword);    //통합검색
        }
        #endregion

        #region 저장
        private void SaveData()
        {

            if (GetSaveURL() == false)
                return;

            string filename = string.Format("MyShoping_{0}", DateTime.Now.ToString("yy-MM-dd_HH_mm_ss"));

            try
            {
                

                exelSave(filename);
                string path = SaveLink + "\\My" + filename + ".txt";
                //@"C:\Users\USER\Desktop\이것저것\MyShoping.txt";
                string save = string.Empty;
                save += "합계 :" + textBox7.Text + "원" + "\r\n" +
                "===============================" + "\r\n";
                foreach (BuyDate bu in buylist)
                {
                    save += "상품명 :" + bu.Title + "\r\n" +
                            "URL :" + bu.Link + "\r\n" +
                            "최고가 :" + bu.Hprice + "\r\n" +
                            "최저가 :" + bu.Lprice + "\r\n" +
                            "===============================" + "\r\n";
                }
                System.IO.File.WriteAllText(path, save, Encoding.Default);

                MessageBox.Show("저장완료");
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장실패");
                MessageBox.Show(ex.Message);

            }
        }

        private void exelSave(string filename)
        {
            Application app = new Application();
            Workbook workbook = app.Workbooks.Add();
            //workbook.SaveAs(Filename: @"C:\Users\USER\Desktop\이것저것\MyShoping.xlsx");
            workbook.SaveAs(Filename: SaveLink + "\\My" + filename + ".xlsx");

            //workbook = app.Workbooks.Open(Filename: @"C:\Users\USER\Desktop\이것저것\test2.xlsx");
            //workbook.SaveAs(Filename: SaveLink + "\\My" + filename + ".xlsx");

            Worksheet worksheet = workbook.Worksheets.Add();

            int r = 1;

            worksheet.Cells[r, 1] = "상품명";
            worksheet.Cells[r, 2] = "최저가";
            worksheet.Cells[r, 3] = "최고가";
            worksheet.Cells[r, 4] = "URL";
            worksheet.Cells[r, 5] = "타입";
            worksheet.Cells[r, 6] = "이미지";
            r++;

            foreach (BuyDate d in buylist)
            {
                worksheet.Cells[r, 1] = d.Title;
                worksheet.Cells[r, 2] = d.Lprice;
                worksheet.Cells[r, 3] = d.Hprice;
                worksheet.Cells[r, 4] = d.Link;
                worksheet.Cells[r, 5] = d.ProductType;
                worksheet.Cells[r, 6] = d.Image;
                r += 1;
            }
            worksheet.Columns["E:F"].EntireColumn.Hidden = true;

            worksheet.Columns.AutoFit();
            workbook.Save();
            //workbook.SaveAs(Filename: @"C:\Users\USER\Desktop\이것저것\test2.xlsx");
            workbook.Close();

        }
        #endregion

        #region 불러오기
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                GetLoadFile();

                string path = FileLink;
                ReadExcel(path);
                MessageBox.Show("불러오기 완료");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //엑셀파일 불러오기
        public void ReadExcel(string path)
        {
            // path는 Excel파일의 전체 경로입니다.
            // 예. D:\test\test.xslx
            Application excelApp = null;
            Workbook wb = null;
            Worksheet ws = null;

            string title = string.Empty;
            int lprice = 0;
            int hprice = 0;
            string url = string.Empty;
            int type = 0;
            string image = string.Empty;
            try
            {
                excelApp = new Application();
                wb = excelApp.Workbooks.Open(path);
                // path 대신 문자열도 가능합니다
                // 예. Open(@"D:\test\test.xslx");
                ws = wb.Worksheets.get_Item(1) as Worksheet;
                // 첫번째 Worksheet를 선택합니다.
                Range rng = ws.UsedRange;   // '여기'
                                            // 현재 Worksheet에서 사용된 셀 전체를 선택합니다.
                object[,] data = rng.Value;
                // 열들에 들어있는 Data를 배열 (One-based array)로 받아옵니다. // -->, i = 0
                for (int r = 2; r <= data.GetLength(0); r++)
                {

                    for (int c = 1; c <= data.GetLength(1); c++)
                    {
                        if (data[r, c] == null)
                        {
                            continue;
                        }

                        if (c == 1)
                        {
                            title = (string)(rng.Cells[r, c] as Range).Value2; // 열과 행에 해당하는 데이터를 문자열로 반환
                        }
                        else if (c == 2)
                        {
                            lprice = (int)(rng.Cells[r, c] as Range).Value2; // 열과 행에 해당하는 데이터를 문자열로 반환
                        }
                        else if (c == 3)
                        {
                            hprice = (int)(rng.Cells[r, c] as Range).Value2; // 열과 행에 해당하는 데이터를 문자열로 반환
                        }
                        else if (c == 4)
                        {
                            url = (string)(rng.Cells[r, c] as Range).Value2;
                        }
                        else if (c == 5)
                        {
                            type = (int)(rng.Cells[r, c] as Range).Value2;
                        }
                        else
                        {
                            image = (string)(rng.Cells[r, c] as Range).Value2; // 열과 행에 해당하는 데이터를 문자열로 반환
                        }

                    }
                    BuyDate buy = new BuyDate(title, url, image, lprice, hprice, type);
                    buylist.Add(buy);
                    // Data 빼오기
                    // data[r, c] 는 excel의 (r, c) 셀 입니다.
                    // data.GetLength(0)은 엑셀에서 사용되는 행의 수를 가져오는 것이고,
                    // data.GetLength(1)은 엑셀에서 사용되는 열의 수를 가져오는 것입니다.
                    // GetLength와 [ r, c] 의 순서를 바꿔서 사용할 수 있습니다.
                }
                wb.Close(true);
                excelApp.Quit();

                listBox1.Items.Clear();
                PrintBuylist();
                AllMoney();


            }
            catch (Exception ex)
            {
                //wb.Close(true);
                throw ex;
            }
            finally
            {
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
                //wb.Close(true);
            }
        }

        static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }

        }

        #endregion


        
      
       

       
    }
}
