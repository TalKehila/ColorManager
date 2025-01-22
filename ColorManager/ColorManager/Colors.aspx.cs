using System;
using ColorManager.DAL;
using System.Web.Services;
using System.Collections.Generic;
using ColorManager.Models;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Data;

namespace ColorManager
{
    public partial class Colors : System.Web.UI.Page
    {
        ColorDataAccess colorDAL = new ColorDataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadColors();
            }
        }

        private void LoadColors()
        {
            DataTable dt = colorDAL.GetAllColors();
            gvColors.DataSource = dt;
            gvColors.DataBind();

            // Update the total records label
            lblTotalRecords.Text = $"Total: {dt.Rows.Count} records found";
        }

        [WebMethod]
        public static void UpdateDisplayOrder(List<OrderItem> order)
        {
            ColorDataAccess colorDAL = new ColorDataAccess();
            foreach (var item in order)
            {
                colorDAL.UpdateDisplayOrder(item.ID, item.DisplayOrder);
            }
        }

        protected void gvColors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvColors.DataKeys[e.RowIndex].Value);
            colorDAL.DeleteColor(id);

            ReassignDisplayOrders(); // קריאה לפונקציית סידור
            LoadColors();
        }

        private void ReassignDisplayOrders()
        {
            var dt = colorDAL.GetAllColors();
            int order = 1;
            foreach (DataRow row in dt.Rows)
            {
                int colorID = Convert.ToInt32(row["ID"]);
                colorDAL.UpdateDisplayOrder(colorID, order);
                order++;
            }
        }

        protected void gvColors_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvColors.EditIndex = e.NewEditIndex;
            LoadColors();
        }

        protected void gvColors_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvColors.DataKeys[e.RowIndex].Value);

            TextBox txtColorNameEdit = (TextBox)gvColors.Rows[e.RowIndex].FindControl("txtColorNameEdit");
            TextBox txtPriceEdit = (TextBox)gvColors.Rows[e.RowIndex].FindControl("txtPriceEdit");
            TextBox txtDisplayOrderEdit = (TextBox)gvColors.Rows[e.RowIndex].FindControl("txtDisplayOrderEdit");

            string colorName = txtColorNameEdit.Text.Trim();
            string priceText = txtPriceEdit.Text.Trim();
            string displayOrderText = txtDisplayOrderEdit.Text.Trim();

            // ולידציה על שם הצבע
            if (!Regex.IsMatch(colorName, @"^[\u0590-\u05FF\s]+$"))
            {
                lblMessage.Text = "שם הצבע חייב להיות בעברית בלבד.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // ולידציה על מחיר
            if (!decimal.TryParse(priceText, out decimal price) || price < 0)
            {
                lblMessage.Text = "מחיר חייב להיות מספר חיובי.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // ולידציה על סדר הצגה
            if (!int.TryParse(displayOrderText, out int displayOrder) || displayOrder < 0)
            {
                lblMessage.Text = "סדר הצגה חייב להיות מספר שלם חיובי.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // בדיקה אם סדר ההצגה תפוס על ידי פריט אחר
            if (colorDAL.IsDisplayOrderExists(displayOrder, id))
            {
                lblMessage.Text = $"סדר ההצגה {displayOrder} כבר בשימוש. אנא בחר מספר אחר.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // עדכון הצבע במידה והכל תקין
            colorDAL.UpdateColor(id, colorName, price, displayOrder);

            gvColors.EditIndex = -1; // יציאה ממצב עריכה
            LoadColors(); // רענון הטבלה

            lblMessage.Text = "עודכן בהצלחה.";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }

        protected void gvColors_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvColors.EditIndex = -1;
            LoadColors();
        }

        protected void btnAddColor_Click(object sender, EventArgs e)
        {
            string colorName = txtColorName.Text.Trim();
            string priceText = txtPrice.Text.Trim();
            string displayOrderText = txtDisplayOrder.Text.Trim();
            bool isInStock = chkIsInStock.Checked;

            // Validate color name
            if (!Regex.IsMatch(colorName, @"^[\u0590-\u05FF\s]+$"))
            {
                lblMessage.Text = "שם הצבע חייב להיות בעברית בלבד.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Validate price
            if (!decimal.TryParse(priceText, out decimal price) || price < 0)
            {
                lblMessage.Text = "מחיר חייב להיות מספר חיובי.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Validate display order
            if (!int.TryParse(displayOrderText, out int displayOrder) || displayOrder < 0)
            {
                lblMessage.Text = "סדר הצגה חייב להיות מספר שלם חיובי.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Check if display order already exists
            if (colorDAL.IsDisplayOrderExists(displayOrder))  // ✅ No ID needed here
            {
                lblMessage.Text = $"סדר ההצגה {displayOrder} כבר בשימוש. אנא בחר מספר אחר.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Check if color already exists
            if (colorDAL.IsColorExists(colorName))
            {
                lblMessage.Text = $"הצבע '{colorName}' כבר קיים!";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                // Insert the color if all validations pass
                colorDAL.InsertColor(colorName, price, displayOrder, isInStock);
                Response.Redirect(Request.RawUrl);


            }
        }

        protected void gvColors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleStock")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                // קבלת המצב הנוכחי
                DataTable dt = colorDAL.GetColorById(id);
                if (dt.Rows.Count > 0)
                {
                    bool isInStock = Convert.ToBoolean(dt.Rows[0]["is_in_stock"]);

                    // החלפת מצב במלאי
                    colorDAL.UpdateStockStatus(id, !isInStock);

                    // רענון הטבלה
                    LoadColors();
                }
            }
        }

    }
}