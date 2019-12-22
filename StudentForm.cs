using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
            InitializeComponent();
            BindGrid();

            btnUpdate.Visible = false;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Student obj = new Student();
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            obj.Name = firstName + " " + lastName;
            obj.Address = txtAddress.Text;
            obj.Email = txtEmail.Text;
            obj.BirthDate = dpBirthDate.Value;
            obj.ContactNo = txtContactNo.Text;
            obj.Gender = cbGender.SelectedItem.ToString();
            obj.Add(obj);
            BindGrid();
            Clear();
        }
        private void BindGrid()
        {
            Student obj = new Student();
            List<Student> listStudents = obj.List();
            DataTable dt = Utility.ConvertToDataTable(listStudents);
            dataGridStudents.DataSource = dt;
            BindChart(listStudents);
        }
        private void Clear()
        {
            txtId.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            dpBirthDate.Value = DateTime.Today;
            txtContactNo.Text = "";
            cbGender.SelectedItem = null;

        }

        private void GridRow_DbClick(object sender, DataGridViewRowEventArgs e)
        {
            int id = 0;
            string myValue = dataGridStudents[e.Row.Index, 0].Value.ToString();

            //get the clicked id 
            //read text file 
            Student obj = new Student();
            List<Student> listStudents = obj.List();
            Student s = listStudents.Where(x => x.Id == id).FirstOrDefault();
            //txtFirstName.Text = s.Name.Split(' ')[0];
            //txtLastName.Text = s.Name.Split(' ')[1];
        }

        private void dataGridStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Student obj = new Student();
            if (e.ColumnIndex == 0)
            {
                //get the value of the clicked rows id column
                string value = dataGridStudents[2, e.RowIndex].Value.ToString();
                int id = 0;
                if (String.IsNullOrEmpty(value))
                {
                    MessageBox.Show("Invalid Data");
                }
                else
                {
                    id = int.Parse(value);
                    Student s = obj.List().Where(x => x.Id == id).FirstOrDefault();
                    txtId.Text = s.Id.ToString();
                    txtFirstName.Text = s.Name.Split(' ')[0];
                    txtLastName.Text = s.Name.Split(' ')[1];
                    txtAddress.Text = s.Address;
                    txtEmail.Text = s.Email;
                    dpBirthDate.Value = s.BirthDate;
                    txtContactNo.Text = s.ContactNo;
                    cbGender.SelectedItem = s.Gender;
                    btnSubmit.Visible = false;
                    btnUpdate.Visible = true;
                }
            }
            else if (e.ColumnIndex == 1)
            {
                string message = "Do you want to Delete this Record?";
                string title = "Delete Confirmation";
                MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                    //get the value of the clicked rows id column
                    string value = dataGridStudents[2, e.RowIndex].Value.ToString();
                    obj.Delete(int.Parse(value));
                    BindGrid();
                    MessageBox.Show("Record Successfully Deleted");
                }
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Student obj = new Student();
            obj.Id = int.Parse(txtId.Text);
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            obj.Name = firstName + " " + lastName;
            obj.Address = txtAddress.Text;
            obj.Email = txtEmail.Text;
            obj.BirthDate = dpBirthDate.Value;
            obj.ContactNo = txtContactNo.Text;
            obj.Gender = cbGender.SelectedItem.ToString();
            obj.Edit(obj);
            BindGrid();
            Clear();
            btnUpdate.Visible = false;
            btnSubmit.Visible = true;
        }
        private void BindChart(List<Student> lst)
        {
            if (lst != null)
            {
                var result = lst
                    .GroupBy(l => l.Gender)
                    .Select(cl => new
                    {
                        Gender = cl.First().Gender,
                        Count = cl.Count().ToString()
                    }).ToList();
                DataTable dt = Utility.ConvertToDataTable(result);
                chart1.DataSource = dt;
                chart1.Name = "Gender";
                chart1.Series["Series1"].XValueMember = "Gender";
                chart1.Series["Series1"].YValueMembers = "Count";
                this.chart1.Titles.Remove(this.chart1.Titles.FirstOrDefault());
                this.chart1.Titles.Add("Weekly Enrollment Chart");
                chart1.Series["Series1"].IsValueShownAsLabel = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
