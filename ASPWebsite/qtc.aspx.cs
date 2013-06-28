using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class qtc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SqlConnection cnn = new SqlConnection(ConfigurationManager.AppSettings["Connection String"]);
        //SqlDataAdapter incomingCommand = new SqlDataAdapter("SELECT * FROM qtc where status = 'incoming'", cnn);
        // Create and fill a DataSet.
        //DataSet incomingDataSet = new DataSet();
        //incomingCommand.Fill(incomingDataSet);
        //// Bind MyRepeater to the  DataSet. QtcRepeater is the ID of the
        //// Repeater control in the HTML section of the page.
        //incomingRepeater.DataSource = incomingDataSet;
        //incomingRepeater.DataBind();

        //Incoming tab
        DataSet incomingDataSet = new DataSet();
        SqlCommand ofStatus = new SqlCommand("ofStatus", cnn);
        ofStatus.CommandType = CommandType.StoredProcedure;
        ofStatus.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar));
        ofStatus.Parameters["@status"].Value = "incoming";
        SqlDataAdapter incomingDA = new SqlDataAdapter(ofStatus);
        incomingDA.Fill(incomingDataSet);
        incomingRepeater.DataSource = incomingDataSet;
        incomingRepeater.DataBind();

        //triage tab
        DataSet triageDataSet = new DataSet();
        SqlCommand ofStatusTriage = new SqlCommand("ofStatus", cnn);
        ofStatusTriage.CommandType = CommandType.StoredProcedure;
        ofStatusTriage.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar));
        ofStatusTriage.Parameters["@status"].Value = "triage";
        SqlDataAdapter triageDA = new SqlDataAdapter(ofStatusTriage);
        triageDA.Fill(triageDataSet);
        triageRepeater.DataSource = triageDataSet;
        triageRepeater.DataBind();
        
        //Followup tab
        DataSet followupDataSet = new DataSet();
        SqlCommand ofStatusFollowup = new SqlCommand("ofStatus", cnn);
        ofStatusFollowup.CommandType = CommandType.StoredProcedure;
        ofStatusFollowup.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar));
        ofStatusFollowup.Parameters["@status"].Value = "followup";
        SqlDataAdapter followupDA = new SqlDataAdapter(ofStatusFollowup);
        followupDA.Fill(followupDataSet);
        followupRepeater.DataSource = followupDataSet;
        followupRepeater.DataBind();
    }



    //This function is used with the repeater
    //Whenever view, approve, or reject is clicked, these methods get triggered.
    protected void incomingRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        //Open connection
        SqlConnection cnn = new SqlConnection(ConfigurationManager.AppSettings["Connection String"]);
        cnn.Open();
        //SqlDataAdapter myCommand = new SqlDataAdapter("SELECT * FROM qtc", cnn);

        //The hidden field of the unique primary ID
        var uniqueID = (e.Item.FindControl("uniqueID")) as HiddenField;
        switch (e.CommandName)
        {

            case "ViewButton":
                // SqlCommand myCommand = new SqlCommand("UPDATE qtc SET claim_number='14' WHERE id = " + uniqueID.Value, cnn);
                //myCommand.CommandType = CommandType.Text;
                //myCommand.ExecuteReader();
                Response.Redirect("ViewPage.aspx?uniqueID=" + uniqueID.Value);
                // ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('Hello World');", true);

                Response.Write(uniqueID.Value);
                Response.Write("View");

                break;

            case "Approve":
                //SqlCommand approveCommand = new SqlCommand("UPDATE qtc SET status = 'triage' WHERE id = " + uniqueID.Value, cnn);
                //approveCommand.CommandType = CommandType.Text;
                //approveCommand.ExecuteReader();

                String id2 = uniqueID.Value;
                String status2 = "triage";
                SqlCommand cmd2 = new SqlCommand("approve_reject", cnn);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar)).Value = id2;
                cmd2.Parameters.Add(new SqlParameter("@triage_followup", SqlDbType.VarChar)).Value = status2;

                cmd2.CommandTimeout = 600;
                int affectedRows2 = cmd2.ExecuteNonQuery();

                Response.Redirect(Request.RawUrl);

                break;

            case "Reject":
                //SqlCommand rejectCommand = new SqlCommand("UPDATE qtc SET status = 'followup' WHERE id = " + uniqueID.Value, cnn);
                //rejectCommand.CommandType = CommandType.Text;
                //rejectCommand.ExecuteReader();

                String id3 = uniqueID.Value;
                String status3 = "followup";
                SqlCommand cmd3 = new SqlCommand("approve_reject", cnn);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar)).Value = id3;
                cmd3.Parameters.Add(new SqlParameter("@triage_followup", SqlDbType.VarChar)).Value = status3;

                cmd3.CommandTimeout = 600;
                int affectedRows3 = cmd3.ExecuteNonQuery();

                Response.Redirect(Request.RawUrl);
                break;

            default:
                break;
        }
    }
}