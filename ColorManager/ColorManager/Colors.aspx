<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Colors.aspx.cs" Inherits="ColorManager.Colors" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>ניהול צבעים</title>
    <link href="CSS/Style.css" rel="stylesheet" />
    <link href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gvColors" runat="server" AutoGenerateColumns="False"
                DataKeyNames="ID"
                OnRowCommand="gvColors_RowCommand"
                OnRowDeleting="gvColors_RowDeleting"
                OnRowEditing="gvColors_RowEditing"
                OnRowUpdating="gvColors_RowUpdating"
                OnRowCancelingEdit="gvColors_RowCancelingEdit">

                <Columns>

                    <asp:TemplateField HeaderText="תיאור">
                        <ItemTemplate>
                            <%# Eval("color_name") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtColorNameEdit" runat="server" Text='<%# Bind("color_name") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="מחיר">
                        <ItemTemplate>
                            <%# Eval("price") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPriceEdit" runat="server" Text='<%# Bind("price") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="צבע">
                        <ItemTemplate>
                            <div class='color-box <%# Eval("color_name") %>'></div>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="סדר">
                        <ItemTemplate>
                            <%# Eval("display_order") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDisplayOrderEdit" runat="server" Text='<%# Bind("display_order") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="במלאי">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("is_in_stock")) 
            ? "<span class='stock-status in-stock'>✔ במלאי</span>" 
            : "<span class='stock-status out-of-stock'>✖ לא במלאי</span>" %>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="סטטוס מלאי">
                        <ItemTemplate>
                            <asp:Button ID="btnToggleStock" runat="server"
                                Text='<%# Convert.ToBoolean(Eval("is_in_stock")) ? "הסר מהמלאי" : "הוסף למלאי" %>'
                                CommandName="ToggleStock"
                                CommandArgument='<%# Eval("ID") %>'
                                CssClass='<%# Convert.ToBoolean(Eval("is_in_stock")) ? "btn-out-stock" : "btn-in-stock" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ButtonType="Button"
                        EditText="ערוך" UpdateText="עדכן" CancelText="ביטול" DeleteText="מחק" />

                </Columns>
            </asp:GridView>

            <asp:Label ID="lblTotalRecords" runat="server" CssClass="total-records" />

            <hr />

            <div class="add-color-container">
                <h3>🎨 הוספת צבע חדש</h3>

                <div class="form-group">
                    <label for="txtColorName">* שם הצבע:</label>
                    <asp:TextBox ID="txtColorName" runat="server" CssClass="form-input" Placeholder="שם הצבע (עברית בלבד)"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtPrice">* מחיר:</label>
                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-input" Placeholder="מחיר"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtDisplayOrder">סדר הצגה:</label>
                    <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="form-input" Placeholder="סדר הצגה"></asp:TextBox>
                </div>

                <div class="form-group checkbox-group">
                    <asp:CheckBox ID="chkIsInStock" runat="server" Text="במלאי" CssClass="form-checkbox" />
                </div>

                <asp:Button ID="btnAddColor" runat="server" Text="➕ הוסף צבע"
                    CssClass="btn-add"
                    OnClick="btnAddColor_Click"
                    OnClientClick="return validateAddForm();" />

                <br />
                <asp:Label ID="lblMessage" runat="server" CssClass="form-message" />
            </div>

            <asp:Label ID="Label1" runat="server" Font-Bold="True" />
        </div>
    </form>
    <script src="js/JavaScripts.js"></script>
</body>
</html>
