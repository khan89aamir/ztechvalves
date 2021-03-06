﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Admin_Login : System.Web.UI.Page
{
    clsMySQLCoreApp ObjCon = new clsMySQLCoreApp();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] != null)
        {
            string logoutValue = Request.QueryString["logout"];

            if (logoutValue == "1")
            {
                LogOut();
            }
            else
            {
                Response.Redirect("Admin_Manager.aspx");
            }
        }
    }
    public void LogOut()
    {
        Session["User"] = null;
        Session.Abandon();
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string userName = val_UserName.Value;
        string password = val_Password.Value;

        ObjCon.SetStoreProcedureData("ParmUserName", MySql.Data.MySqlClient.MySqlDbType.VarChar, userName);
        ObjCon.SetStoreProcedureData("ParmPassword ", MySql.Data.MySqlClient.MySqlDbType.VarChar, password);
        DataSet ds = ObjCon.ExecuteStoreProcedure_Get("ztech.spr_Validate_Login");
        //DataTable dtuser = ObjCon.ExecuteSelectStatement("SELECT 1 FROM tblUserDetails WHERE UserName='" + userName + "' AND Password='" + password + "'");
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dtuser = ds.Tables[0];
            if (dtuser != null && dtuser.Rows.Count > 0)
            {
                //Session["User"] = userName;
                Session["User"] = dtuser.Rows[0]["ID"];
                Response.Redirect("/Admin_Manager.aspx");
            }
            else
            {
                Control control = Page.Master.FindControl("pnlProfile");
                control.Visible = false;
                NavigationURL.Value = "Admin_Login.aspx";
                ShowMessageBox("Incorrect User Name or Password.", false);
            }
        }
        else
        {
            Control control = Page.Master.FindControl("pnlProfile");
            control.Visible = false;
            NavigationURL.Value = "Admin_Login.aspx";
            ShowMessageBox("Incorrect User Name or Password.", false);
        }
    }

    private void ShowMessageBox(string msg, bool IsSuccess = true)
    {
        HtmlGenericControl lblUserVal = (HtmlGenericControl)Page.Master.FindControl("lblMessage");
        lblUserVal.InnerText = msg;
        string script = "";
        if (IsSuccess)
        {
            script = "$('#mdlNormalMessage').modal(); ";
        }
        else
        {
            script = "$('#mdlNormalMessage').modal(); " +
                     "$('#iconMsg').removeClass('fa-check-circle').addClass('fa-times-circle');" +
                     "$('#iconMsg').css('color','red');";
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), "func", script, true);
    }
}